using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombatController : MonoBehaviour
{
    public bool patrol = false;
    public bool inCombat = false;
    public float agroRange = 20f;
    public bool usePrimaryAttack = true;

    public EnemyAbilityBank.EnemyAbility specialOneAbility;
    public EnemyAbilityBank.EnemyAbility specialTwoAbility;
    public float specialOneRange = 100;
    public float specialTwoRange = 100;

    public float specialOneCooldown = 5f;
    public float specialTwoCooldown = 5f;
    public float onHitSpecialOneCooldown = 5f;

    public EnemyAbilityBank.EnemyAbility onHitSpecialOne;

    [SerializeField] private float specialOneCurrentCooldown = 0f;
    [SerializeField] private float specialTwoCurrentCooldown = 0f;
    [SerializeField] private float onHitSpecialOneCurrentCooldown = 0f;

    public GameObject myTarget;
    // A list of potential actions or behaviours
    public enum ActionType { Attack, ChaseTarget, MaintainDistance, RetreatWhenLow, Idle, SpecialOne, SpecialTwo, SpecialThree, SpecialFour, LosingAgro, OnHitSpecialOne, LossOfControl};
    public ActionType myCurrentAction = ActionType.Idle;

    // This is the action hierarchy List, The actions go in in order of importance.s
    public ActionType[] actionHierarchy;
    public float[] actionChances;
    public ActionType[] onHitActionHierarchy;
    public float[] onHitActionChances;
    public LayerMask wallColMask;

    private EnemyMovementManager movementManager;
    [SerializeField] private GameObject[] playerPositions;
    private PlayerStats myStats;
    private Animator anim;
    private HitBoxManager hitBoxManager;
    private EnemyAbilityBank abilityBank;
    private BuffsManager buffManager;

    // Start is called before the first frame update
    void Start()
    {
        movementManager = GetComponent<EnemyMovementManager>();
        playerPositions = GameObject.Find("GameManager").GetComponent<GameManager>().currentPlayers;
        myStats = GetComponent<PlayerStats>();
        anim = GetComponent<Animator>();
        hitBoxManager = GetComponent<HitBoxManager>();
        buffManager = GetComponent<BuffsManager>();

        inCombat = false;
        abilityBank = GetComponent<EnemyAbilityBank>();
        SwitchAction(ActionType.Idle);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2) && CompareTag("Enemy"))
            GetComponent<EnemyCrowdControlManager>().KnockbackLaunch(Vector3.up + transform.forward * 10, GetComponent<PlayerStats>());

        myStats.currentAttackDelay += Time.deltaTime;
        specialOneCurrentCooldown += Time.deltaTime;
        specialTwoCurrentCooldown += Time.deltaTime;
        onHitSpecialOneCurrentCooldown += Time.deltaTime;
    }

    // This method is called when we need to revaluate what state we are in and what to do next.
    public void SwitchAction(ActionType newAction)
    {
        // Debug.Log("switching to a new action: " + newAction);
        StopAllCoroutines();
        switch (newAction)
        {
            case ActionType.Attack:
                StartCoroutine(Attack());
                break;
            case ActionType.ChaseTarget:
                StartCoroutine(ChaseTarget());
                break;
            case ActionType.MaintainDistance:
                break;
            case ActionType.RetreatWhenLow:
                break;
            case ActionType.LossOfControl:
                StopAllCoroutines();
                break;
            case ActionType.Idle:
                StartCoroutine(Idle());
                break;
            case ActionType.SpecialOne:
                specialOneCurrentCooldown = 0;
                myCurrentAction = ActionType.SpecialOne;
                abilityBank.CastSpell(specialOneAbility);
                break;
            case ActionType.SpecialTwo:
                specialTwoCurrentCooldown = 0;
                myCurrentAction = ActionType.SpecialTwo;
                abilityBank.CastSpell(specialTwoAbility);
                break;
            case ActionType.SpecialThree:
                break;
            case ActionType.SpecialFour:
                break;
            case ActionType.LosingAgro:
                StartCoroutine(LosingAgro());
                break;
            case ActionType.OnHitSpecialOne:
                onHitSpecialOneCurrentCooldown = 0;
                myCurrentAction = ActionType.OnHitSpecialOne;
                abilityBank.CastSpell(onHitSpecialOne);
                break;
            default:
                break;
        }
    }

    // The idle corotuine, we check to see if enemies are in range
    IEnumerator Idle()
    {
        movementManager.StopMovement();
        anim.SetFloat("Speed", 0);
        myCurrentAction = ActionType.Idle;
        inCombat = false;

        float targetTimer = 0.2f;
        float currentTimer = 0;
        while(!inCombat)
        {
            if(myTarget != null)
                movementManager.RotateToTarget(myTarget.transform.position);
            // Increment the time and see if we surpassed our check target.
            currentTimer += Time.deltaTime;
            if (currentTimer > targetTimer) 
            {
                currentTimer -= targetTimer;
                AgroOntoNearestTarget();
            }
            yield return new WaitForEndOfFrame();
        }

    }

    // the corotuine that is called when we are to chase the player.
    IEnumerator ChaseTarget()
    {
        // allow us to chase the target in question.
        movementManager.SetTarget(myTarget.transform.position);
        movementManager.enableMovement = true;
        myCurrentAction = ActionType.ChaseTarget;
        anim.SetFloat("Speed", 1);

        float currentTimer = 0;
        float targetTimer = 0.1f;
        while(inCombat)
        {
            currentTimer += Time.deltaTime;
            if(currentTimer > targetTimer)
            {
                currentTimer -= targetTimer;
                // Here we check for a deagro and check to see if we are in range for an attack.
                if (CheckDistanceWallObstructed(agroRange, myTarget.transform))
                {
                    // They are in range and we see them.
                    //Debug.Log("checking action hierarchy");
                    CheckActionHierarchy();
                    //if (!movementManager.arrivedAtTarget)
                    //{
                    //ActionType[] actions = { ActionType.SpecialOne, ActionType.SpecialTwo, ActionType.CircleTarget };
                    //float[] chances = { 10, 10, circleTargetChance / 2 };
                    //RollChanceForActions(actions, chances);
                    //}
                    // We are in range to hit them enter the attack state.
                    //else
                    //{
                    //    SwitchAction(ActionType.Attack);
                    //}
                }
                else
                {
                    // Begin the deagro coroutine.
                    SwitchAction(ActionType.LosingAgro);
                }
            }
            yield return new WaitForEndOfFrame();
        }

        anim.SetFloat("Speed", 0);
    }

    // The losing agro coroutine
    IEnumerator LosingAgro()
    {
        myCurrentAction = ActionType.LosingAgro;
        // wait for 3 seconds then check to see if we can see any player. if we can we agro onto them if we cant we idle.
        float currentTimer = 0;
        float targetTimer = 0.2f;
        float currentDuration = 0;
        float targetDuration = 3f;
        while(currentDuration < targetDuration)
        {
            currentDuration += Time.deltaTime;
            currentTimer += Time.deltaTime;
            // here we check if we should do an agro check.
            if(currentTimer > targetTimer)
            {
                currentTimer -= targetTimer;
                AgroOntoNearestTarget();
            }
            yield return new WaitForEndOfFrame();
        }
        inCombat = false;
        SwitchAction(ActionType.Idle);
    }

    IEnumerator Attack()
    {
        myCurrentAction = ActionType.Attack;
        anim.SetFloat("Speed", 0);
        movementManager.StopMovement();
        buffManager.ProcOnAttack();

        // Check to see if our attack is ready and if the enemy is in range.
        while(movementManager.arrivedAtTarget)
        {
            // Check the attack range distance, if we are out of range, start chasing.
            if(usePrimaryAttack && CheckDistance(myStats.attackRange, myTarget.transform))
            {
                // Can we currently attack?
                if (myStats.currentAttackDelay > 1 / myStats.attackSpeed)
                {
                    // Launch the attack.
                    myStats.currentAttackDelay = 0;
                    anim.SetTrigger("Attack");
                    anim.SetFloat("AttackAnimSpeed", myStats.attackSpeed);

                    // Set up the timers.
                    float currentTimer = 0;
                    float targetTimer = 0.8f / myStats.attackSpeed;

                    while(currentTimer < targetTimer)
                    {
                        currentTimer += Time.deltaTime;

                        // Rotate towards the target.
                        movementManager.RotateToTarget(myTarget.transform.position);
                        yield return new WaitForEndOfFrame();
                    }

                    //ActionType[] actions = { ActionType.SpecialOne, ActionType.SpecialTwo, ActionType.TauntTarget, ActionType.Backpeddle, ActionType.CircleTarget };
                    //float[] chances = { 25, 25, tauntTargetChance / 2, backpeddleChance, circleTargetChance};
                    //RollChanceForActions(actions, chances);
                    CheckActionHierarchy();
                }
            }
            else
            {
                SwitchAction(ActionType.ChaseTarget);
            }
            yield return new WaitForEndOfFrame();
        }
    }

    // Thsi method checks the dustance between myself and the target
    public bool CheckDistance(float distance, Transform target)
    {
        bool inRange = false;

        // Check the duistance between the two.
        if (target.GetComponent<PlayerStats>().dead)
            TargetDeath();
        else if ((target.position - transform.position).sqrMagnitude < distance * distance)
            inRange = true;

        return inRange;
    }

    // This method is used to check the distance between myserlf and a target, but does not allow us to return true if something is obstructin the view of the player completely.
    public bool CheckDistanceWallObstructed(float distance, Transform target)
    {
        //Debug.Log("checking for distance difference and wall interference.");
        bool inRange = false;

        // Check the distance between the two.
        if ((target.position - transform.position).sqrMagnitude < distance * distance)
        {
            // If it's in range shoot a ray at the target to see if we hit a wall.
            Ray ray = new Ray(transform.position + new Vector3(0,1,0), (target.position - transform.position).normalized);
            // Debug.DrawRay(transform.position + new Vector3(0, 1, 0), (target.position - transform.position).normalized * 30, Color.red);
            RaycastHit rayhit;

            if (Physics.Raycast(ray, out rayhit, distance, wallColMask))
            {
                if (rayhit.transform.CompareTag("Player") && rayhit.transform.GetComponent<PlayerStats>().dead)
                    TargetDeath();
                else if (rayhit.transform.CompareTag("Player"))
                    inRange = true;
                else
                    inRange = false;
            }
            else
                inRange = true;
        }

        return inRange;
    }

    // USed to make this enemy agro onto the nearest player, if applicable.
    public void AgroOntoNearestTarget()
    {
        //Debug.Log("checking for nearest player to agro onto");
        // Check to see if any players are in range.
        bool playersInRange = false;
        float closestDistance = Mathf.Infinity;
        GameObject closestPlayer = null;
        foreach (GameObject player in playerPositions)
        {
            // Check if the player is within my agro range, and if they are blocked by walls. Grab the closest player within the agro range.
            if(CheckDistanceWallObstructed(agroRange, player.transform))
            {
                // If this player is closer, set it as the closest player to me. If the player is dead ignore them
                if(closestDistance > (player.transform.position - transform.position).sqrMagnitude && !player.GetComponent<PlayerStats>().dead)
                {
                    playersInRange = true;
                    closestDistance = (player.transform.position - transform.position).sqrMagnitude;
                    closestPlayer = player;
                }
            }
        }

        // If we have a player in range, start combat with that player.
        if (playersInRange)
            StartCombat(closestPlayer);
    }

    // USed to start combat with a certain target
    public void StartCombat(GameObject target)
    {
        //Debug.Log("we are starting combat now");
        inCombat = true;
        myTarget = target;
        SwitchAction(ActionType.ChaseTarget);
    }

    // Used to check wether we should make this enmy body commit an action or not based on a percentage chance
    public void RollChanceForAction(ActionType actionToSwitchTo, float percentChance)
    {
        //Debug.Log("We are rolling to see if we are switching to the action: " + actionToSwitchTo);

        if (Random.Range(0, 100) > 100 - percentChance)
        {
            //Debug.Log("We are switching to: " + actionToSwitchTo);
            SwitchAction(actionToSwitchTo);
        }
    }

    // Used to check if any of the following actions should be commmited in a hierachical order.
    public void RollChanceForActions(ActionType[] actionToSwitchTo, float[] percentChances)
    {
        //Debug.Log("We are rolling for another action to switch to.");
        for(int index = 0; index < actionToSwitchTo.Length; index++)
        {
            if (Random.Range(0, 100) > 100 - percentChances[index])
            {
                if (CheckActionReadyStatus(actionToSwitchTo[index]))
                {
                    //Debug.Log("We are switching to: " + actionToSwitchTo[index]);
                    SwitchAction(actionToSwitchTo[index]);
                    break;
                }
            }
        }
    }

    // SUed to check the cooldown of a specific action, and if it's ready.
    public bool CheckActionReadyStatus(ActionType action)
    {
       // Debug.Log("Checking action ready status");
        bool actionReady = true;
        switch (action)
        {
            case ActionType.SpecialOne:
                if (specialOneCurrentCooldown < specialOneCooldown)
                    actionReady = false;
                else if (specialOneAbility == EnemyAbilityBank.EnemyAbility.None)
                    actionReady = false;
                break;
            case ActionType.SpecialTwo:
                if (specialTwoCurrentCooldown < specialTwoCooldown)
                    actionReady = false;
                else if (specialTwoAbility == EnemyAbilityBank.EnemyAbility.None)
                    actionReady = false;
                break;
            default:
                break;
        }
        return actionReady;
    }

    // Used when this object is hit. Check to see if we have any on hit actions we want to do like a retaliation.
    public void CheckOnHitActionHierarchy()
    {
        bool actionFound = false;
        //Debug.Log("There are " + onHitActionHierarchy.Length + " different actions i could take");
        // check each action in the action hierachy.
        for (int index = 0; index < onHitActionHierarchy.Length; index++)
        {
            //Debug.Log("Checking at index " + index);
            // Compare this action to what we need to do.
            ActionType currentActionToCheck = onHitActionHierarchy[index];
            switch (currentActionToCheck)
            {
                case ActionType.OnHitSpecialOne:
                    //Debug.Log(" on hit special one is being tested: " + onHitSpecialOneCurrentCooldown + "/" + onHitSpecialOneCooldown + "   This has a %" + onHitActionChances[index] + " of goinf off");
                    if (onHitSpecialOneCurrentCooldown > onHitSpecialOneCooldown && Random.Range(0, 100) > 100 - onHitActionChances[index])
                        actionFound = true;
                    break;
                default:
                    break;
            }

            // if we found an action to commit to break from this.
            if (actionFound)
            {
                //Debug.Log("We have found an action");
                SwitchAction(currentActionToCheck);
                break;
            }
        }

    }

    // Used when this unit dies
    public void UnitDeath()
    {
        
    }

    // Used When the player we were fighting dies.
    public void TargetDeath()
    {
        //Debug.Log("The target is dead so wed deagro on them here.");
        StopAllCoroutines();
        myTarget = null;
        myCurrentAction = ActionType.Idle;
        SwitchAction(ActionType.Idle);
    }

    // Used to check the action hierachy we set t0o see what our nect course of action should be.
    public void CheckActionHierarchy()
    {
        // Debug.Log("we have entered the action hierarchy");
        // If this bool gets switched to true, we have found an action and can break the for loop.
        bool actionFound = false;

        // Check if our target is alive
        if(myTarget.GetComponent<PlayerStats>() != null && myTarget.GetComponent<PlayerStats>().dead)
        {
            SwitchAction(ActionType.LosingAgro);
            return;
        }


        // check each action in the action hierachy.
        for(int index = 0; index < actionHierarchy.Length; index ++)
        {
            // Compare this action to what we need to do.
            ActionType currentActionToCheck = actionHierarchy[index];
            switch (currentActionToCheck)
            {
                case ActionType.Attack:
                    // if we are in range, attack the dude.
                    if (CheckDistance(myStats.attackRange, myTarget.transform))
                        actionFound = true;
                    break;
                    // This action is known as a baseline action, it will always execute no matter the situation.
                case ActionType.ChaseTarget:
                    actionFound = true;
                    break;
                case ActionType.MaintainDistance:
                    break;
                case ActionType.RetreatWhenLow:
                    break;
                case ActionType.SpecialOne:
                    if (specialOneCurrentCooldown > specialOneCooldown && Random.Range(0, 100) > 100 - actionChances[index])
                        actionFound = true;
                    break;
                case ActionType.SpecialTwo:
                    if (specialTwoCurrentCooldown > specialTwoCooldown && Random.Range(0, 100) > 100 - actionChances[index])
                        actionFound = true;
                    break;
                case ActionType.SpecialThree:
                    break;
                case ActionType.SpecialFour:
                    break;
                default:
                    break;
            }

            // if we found an action to commit to break from this.
            if (actionFound)
            {
                SwitchAction(currentActionToCheck);
                break;
            }
        }
    }

}
