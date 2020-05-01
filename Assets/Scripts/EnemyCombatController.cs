using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombatController : MonoBehaviour
{
    public bool patrol = false;
    public bool inCombat = false;
    public float agroRange = 20f;
    public float circleTargetChance = 0f;
    public float tauntTargetChance = 0f;
    public float waitChance = 0f;
    public float backpeddleChance = 0f;
    public float circleMinRange = 3;
    public float circleMaxRange = 7;
    public float circleSpeedMultiplier = 0.7f;
    public float waitMinRange = 1;
    public float waitMaxRange = 5;
    public float backpeddleMinRange = 1;
    public float backpeddleMaxRange = 1;
    public float backpeddleSpeedMultiplier = 0.7f;

    public EnemyAbilityBank.EnemyAbility specialOneAbility;
    public EnemyAbilityBank.EnemyAbility specialTwoAbility;

    public float circleCooldown = 3f;
    public float tauntCooldown = 3f;
    public float waitCooldown = 3f;
    public float backpeddleCooldown = 3f;
    public float specialOneCooldown = 5f;
    public float specialTwoCooldown = 5f;

    [SerializeField] private float circleCurrentCooldown = 0f;
    [SerializeField] private float tauntCurrentCooldown = 0f;
    [SerializeField] private float waitCurrentCooldown = 0f;
    [SerializeField] private float backpeddleCurrentCooldown = 0f;
    [SerializeField] private float specialOneCurrentCooldown = 0f;
    [SerializeField] private float specialTwoCurrentCooldown = 0f;

    public GameObject myTarget;
    // A list of potential actions or behaviours
    public enum ActionType { Attack, ChaseTarget, CircleTarget, TauntTarget, MaintainDistance, ChaseLowTargets, RetreatWhenLow, RetreatWhenNoLeader, Idle, Patrolling, SpecialOne, SpecialTwo, SpecialThree, SpecialFour, LosingAgro, WaitInCombat, Backpeddle};
    public ActionType myCurrentAction = ActionType.Idle;

    // This is the action hierarchy List, The actions go in in order of importance.
    public List<ActionType> ActionHierarchy = new List<ActionType>();
    public LayerMask wallColMask;

    private EnemyMovementManager movementManager;
    [SerializeField] private GameObject[] playerPositions;
    private PlayerStats myStats;
    private Animator anim;
    private HitBoxManager hitBoxManager;
    private EnemyAbilityBank abilityBank;

    // Start is called before the first frame update
    void Start()
    {
        movementManager = GetComponent<EnemyMovementManager>();
        playerPositions = GameObject.Find("GameManager").GetComponent<GameManager>().currentPlayers;
        myStats = GetComponent<PlayerStats>();
        anim = GetComponent<Animator>();
        hitBoxManager = GetComponent<HitBoxManager>();
        inCombat = false;
        abilityBank = GetComponent<EnemyAbilityBank>();
        SwitchAction(ActionType.Idle);
    }

    // Update is called once per frame
    void Update()
    {
        myStats.currentAttackDelay += Time.deltaTime;
        circleCurrentCooldown += Time.deltaTime;
        tauntCurrentCooldown += Time.deltaTime;
        waitCurrentCooldown += Time.deltaTime;
        backpeddleCurrentCooldown += Time.deltaTime;
        specialOneCurrentCooldown += Time.deltaTime;
        specialTwoCooldown += Time.deltaTime;
    }

    // This method is called when we need to revaluate what state we are in and what to do next.
    public void SwitchAction(ActionType newAction)
    {
        Debug.Log("switching to a new action: " + newAction);
        StopAllCoroutines();
        switch (newAction)
        {
            case ActionType.Attack:
                StartCoroutine(Attack());
                break;
            case ActionType.ChaseTarget:
                StartCoroutine(ChaseTarget());
                break;
            case ActionType.CircleTarget:
                StartCoroutine(CircleTarget());
                break;
            case ActionType.TauntTarget:
                StartCoroutine(Taunt());
                break;
            case ActionType.MaintainDistance:
                break;
            case ActionType.ChaseLowTargets:
                break;
            case ActionType.RetreatWhenLow:
                break;
            case ActionType.RetreatWhenNoLeader:
                break;
            case ActionType.Idle:
                StartCoroutine(Idle());
                break;
            case ActionType.Patrolling:
                break;
            case ActionType.SpecialOne:
                specialOneCurrentCooldown = 0;
                abilityBank.CastSpell(specialOneAbility);
                break;
            case ActionType.SpecialTwo:
                specialTwoCurrentCooldown = 0;
                abilityBank.CastSpell(specialTwoAbility);
                break;
            case ActionType.SpecialThree:
                break;
            case ActionType.SpecialFour:
                break;
            case ActionType.LosingAgro:
                StartCoroutine(LosingAgro());
                break;
            case ActionType.WaitInCombat:
                StartCoroutine(WaitInCombat());
                break;
            case ActionType.Backpeddle:
                StartCoroutine(Backpeddle());
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
        float targetTimer = 0.2f;
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
                    if (!movementManager.arrivedAtTarget)
                    {
                        ActionType[] actions = { ActionType.SpecialOne, ActionType.CircleTarget };
                        float[] chances = { 10, circleTargetChance / 2};
                        RollChanceForActions(actions, chances);
                    }
                    // We are in range to hit them enter the attack state.
                    else
                    {
                        SwitchAction(ActionType.Attack);
                    }

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

        // Check to see if our attack is ready and if the enemy is in range.
        while(movementManager.arrivedAtTarget)
        {
            // Check the attack range distance, if we are out of range, start chasing.
            if(CheckDistance(myStats.attackRange, myTarget.transform))
            {
                // Can we currently attack?
                if (myStats.currentAttackDelay > myStats.attackDelay)
                {
                    // Launch the attack.
                    myStats.currentAttackDelay = 0;
                    anim.SetTrigger("Attack");
                    anim.SetFloat("AttackAnimSpeed", (1 / myStats.attackDelay));

                    // Set up the timers.
                    float currentTimer = 0;
                    float targetTimer = myStats.attackDelay;
                    bool attackedLaunched = false;

                    while(currentTimer < targetTimer)
                    {
                        currentTimer += Time.deltaTime;
                        // If we have waited a sufficient amount of time, launch the attack
                        if(!attackedLaunched && currentTimer > targetTimer / 2)
                        {
                            attackedLaunched = true;
                            hitBoxManager.LaunchHitBox(0);
                        }
                        // Rotate towards the target.
                        movementManager.RotateToTarget(myTarget.transform.position);
                        yield return new WaitForEndOfFrame();
                    }
                    
                    ActionType[] actions = { ActionType.SpecialOne, ActionType.TauntTarget, ActionType.Backpeddle, ActionType.CircleTarget };
                    float[] chances = { 25, tauntTargetChance / 2, backpeddleChance, circleTargetChance};
                    RollChanceForActions(actions, chances);
                }
            }
            else
            {
                SwitchAction(ActionType.ChaseTarget);
            }
            yield return new WaitForEndOfFrame();
        }
    }

    // Used to taunt the enemy, used as a spereate idle animation. Sometimes completed afetr attack.
    IEnumerator Taunt()
    {
        tauntCurrentCooldown = 0;
        myCurrentAction = ActionType.TauntTarget;
        float currentTimer = 0;
        float targetTimer = 2;
        anim.SetTrigger("Taunt");
        movementManager.StopMovement();

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        tauntCurrentCooldown = 0;
        ActionType[] actions = { ActionType.SpecialOne, ActionType.WaitInCombat, ActionType.Attack};
        float[] chances = { 25, waitChance, 100};
        RollChanceForActions(actions, chances);
    }

    // Used to start circling the target.
    IEnumerator CircleTarget()
    {
        circleCurrentCooldown = 0;
        myCurrentAction = ActionType.CircleTarget;
        movementManager.StopMovement();
        float currentTimer = 0;
        float targetTimer = Random.Range(3, 10);
        float targetRadiusFromTarget = Random.Range(circleMinRange, circleMaxRange);
        float currentPointTimer = 0;
        float targetPointTimer = 0.1f;
        float direction = Random.Range(0, 100);
        anim.SetBool("Strafing", true);
        anim.SetFloat("Speed", 1);
        if (direction > 50)
            anim.SetFloat("Direction", 1);
        else
            anim.SetFloat("Direction", -1);

        while (currentTimer < targetTimer)
        {
            // increment the timers.
            currentTimer += Time.deltaTime;
            currentPointTimer += Time.deltaTime;

            // If we waited loing enough, grab a new target for us to run to.
            if(currentPointTimer > targetPointTimer)
            {
                currentPointTimer -= targetPointTimer;
                Vector3 slidePosition = Vector3.zero; 
                if (direction > 50)
                    slidePosition = transform.position + (transform.right * targetRadiusFromTarget).normalized * 2;
                else
                    slidePosition = transform.position + (-transform.right * targetRadiusFromTarget).normalized * 2;

                Debug.DrawRay(transform.position + Vector3.up, (slidePosition - transform.position), Color.green, 2f);
                movementManager.MoveToTarget(slidePosition, myStats.speed * circleSpeedMultiplier);
            }
            movementManager.RotateToTarget(myTarget.transform.position);
            yield return new WaitForEndOfFrame();
        }

        movementManager.StopMovement();
        anim.SetBool("Strafing", false);
        anim.SetFloat("Direction", 0);
        anim.SetFloat("Speed", 0);

        circleCurrentCooldown = 0;
        ActionType[] actions = { ActionType.SpecialOne, ActionType.TauntTarget, ActionType.WaitInCombat, ActionType.Attack};
        float[] chances = { 25, tauntTargetChance, waitChance * 5, 100 };
        RollChanceForActions(actions, chances);
    }

    // The wait in combat coroutine. This is called when the enemy should take a break as to not run down the player all the time.
    IEnumerator WaitInCombat()
    {
        waitCurrentCooldown = 0;
        movementManager.StopMovement();
        anim.SetFloat("Speed", 0);
        myCurrentAction = ActionType.WaitInCombat;

        float targetTimer = Random.Range(waitMinRange, waitMaxRange);
        float currentTimer = 0;
        while (inCombat)
        {
            // Increment the time and see if we surpassed our check target.
            currentTimer += Time.deltaTime;
            if (currentTimer > targetTimer)
                break;
            yield return new WaitForEndOfFrame();
        }
        waitCurrentCooldown = 0;
        SwitchAction(ActionType.Attack);
    }

    // Used to make the target backpeddle for a bit then wait.
    IEnumerator Backpeddle()
    {
        backpeddleCurrentCooldown = 0;
        movementManager.StopMovement();
        anim.SetFloat("Speed", -1);

        myCurrentAction = ActionType.Backpeddle;
        float targetTimer = Random.Range(backpeddleMinRange, backpeddleMaxRange);
        float currentTimer = 0;
        float currentPositionTimer = 0;
        float targetPositionTimer = 0.2f;
        while(inCombat)
        {
            currentTimer += Time.deltaTime;
            currentPositionTimer += Time.deltaTime;
            if (currentTimer > targetTimer)
                break;
            if(currentPositionTimer > targetPositionTimer)
            {
                currentPositionTimer -= targetPositionTimer;
                movementManager.MoveToTarget(transform.position + transform.forward * -1, myStats.speed * backpeddleSpeedMultiplier);
                movementManager.RotateToTarget(myTarget.transform.position);
            }
            yield return new WaitForEndOfFrame();
        }
        
        anim.SetFloat("Speed", 0);

        backpeddleCurrentCooldown = 0;
        ActionType[] actions = {ActionType.SpecialOne, ActionType.TauntTarget, ActionType.CircleTarget, ActionType.WaitInCombat };
        float[] chances = { 25, tauntTargetChance * 4, circleTargetChance * 2, 100 };
        RollChanceForActions(actions, chances);
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
            Debug.DrawRay(transform.position + new Vector3(0, 1, 0), (target.position - transform.position).normalized * 30, Color.red);
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
            Debug.Log("We are switching to: " + actionToSwitchTo);
            SwitchAction(actionToSwitchTo);
        }
    }

    // Used to check if any of the following actions should be commmited in a hierachical order.
    public void RollChanceForActions(ActionType[] actionToSwitchTo, float[] percentChances)
    {
        Debug.Log("We are rolling for another action to switch to.");
        for(int index = 0; index < actionToSwitchTo.Length; index++)
        {
            if (Random.Range(0, 100) > 100 - percentChances[index])
            {
                if (CheckActionReadyStatus(actionToSwitchTo[index]))
                {
                    Debug.Log("We are switching to: " + actionToSwitchTo[index]);
                    SwitchAction(actionToSwitchTo[index]);
                    break;
                }
            }
        }
    }

    // SUed to check the cooldown of a specific action, and if it's ready.
    public bool CheckActionReadyStatus(ActionType action)
    {
        Debug.Log("Checking action ready status");
        bool actionReady = true;
        switch (action)
        {
            case ActionType.CircleTarget:
                if (circleCurrentCooldown < circleCooldown)
                    actionReady = false;
                break;
            case ActionType.TauntTarget:
                if (tauntCurrentCooldown < tauntCooldown)
                    actionReady = false;
                break;
            case ActionType.SpecialOne:
                if (specialOneCurrentCooldown < specialOneCooldown)
                    actionReady = false;
                break;
            case ActionType.SpecialTwo:
                if (specialTwoCurrentCooldown < specialTwoCooldown)
                    actionReady = false;
                break;
            case ActionType.WaitInCombat:
                if (waitCurrentCooldown < waitCooldown)
                    actionReady = false;
                break;
            case ActionType.Backpeddle:
                if (backpeddleCurrentCooldown < backpeddleCooldown)
                    actionReady = false;
                break;
            default:
                break;
        }
        return actionReady;
    }
    // Used when this unit dies
    public void UnitDeath()
    {

    }

    // Used When the player we were fighting dies.
    public void TargetDeath()
    {
        Debug.Log("The target is dead so wed deagro on them here.");
        StopAllCoroutines();
        myTarget = null;
        myCurrentAction = ActionType.Idle;
        SwitchAction(ActionType.Idle);
    }
}
