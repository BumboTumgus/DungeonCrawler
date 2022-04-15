using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombatController : MonoBehaviour
{
    public float agroRange = 20f;
    public bool usePrimaryAttack = true;
    [SerializeField] private float basicAttackDuration = 0.8f;
    [SerializeField] private float basicAttackDelay = 3f;

    public EnemyAbilityBank.EnemyAbility specialOneAbility;
    public EnemyAbilityBank.EnemyAbility specialTwoAbility;
    public EnemyAbilityBank.EnemyAbility specialThreeAbility;
    public EnemyAbilityBank.EnemyAbility specialFourAbility;
    public float specialOneRange = 100;
    public float specialTwoRange = 100;
    public float specialThreeRange = 100;
    public float specialFourRange = 100;

    public float specialOneCooldown = 5f;
    public float specialTwoCooldown = 5f;
    public float specialThreeCooldown = 5f;
    public float specialFourCooldown = 5f;

    [SerializeField] private float specialOneCurrentCooldown = 0f;
    [SerializeField] private float specialTwoCurrentCooldown = 0f;
    [SerializeField] private float specialThreeCurrentCooldown = 0f;
    [SerializeField] private float specialFourCurrentCooldown = 0f;


    public GameObject myTarget;
    // A list of potential actions or behaviours
    public enum ActionType { Attack, ChaseTarget, SpecialOne, SpecialTwo, SpecialThree, SpecialFour, LossOfControl};
    public ActionType myCurrentAction = ActionType.ChaseTarget;

    // This is the action hierarchy List, The actions go in in order of importance.s
    public ActionType[] actionHierarchy;
    public float[] actionChances;
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
        playerPositions = GameManager.instance.currentPlayers;
        myStats = GetComponent<PlayerStats>();
        anim = GetComponent<Animator>();
        hitBoxManager = GetComponent<HitBoxManager>();
        buffManager = GetComponent<BuffsManager>();

        abilityBank = GetComponent<EnemyAbilityBank>();
        myTarget = GameManager.instance.currentPlayers[0];
        SwitchAction(ActionType.ChaseTarget);
    }

    // Update is called once per frame
    void Update()
    {
        myStats.currentAttackDelay += Time.deltaTime;
        specialOneCurrentCooldown += Time.deltaTime;
        specialTwoCurrentCooldown += Time.deltaTime;
        specialThreeCurrentCooldown += Time.deltaTime;
        specialFourCurrentCooldown += Time.deltaTime;
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
            case ActionType.LossOfControl:
                StopAllCoroutines();
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
                specialThreeCurrentCooldown = 0;
                myCurrentAction = ActionType.SpecialThree;
                abilityBank.CastSpell(specialThreeAbility);
                break;
            case ActionType.SpecialFour:
                specialFourCurrentCooldown = 0;
                myCurrentAction = ActionType.SpecialFour;
                abilityBank.CastSpell(specialFourAbility);
                break;
            default:
                break;
        }
    }

    // the corotuine that is called when we are to chase the player.
    IEnumerator ChaseTarget()
    {
        // allow us to chase the target in question.
        if(myTarget.CompareTag("Player"))
            movementManager.SetTarget(myTarget.transform.position, myTarget.GetComponent<PlayerMovementController>().transformNavMeshPosition);
        else
            movementManager.SetTarget(myTarget.transform.position, Vector3.zero);
        movementManager.enableMovement = true;
        myCurrentAction = ActionType.ChaseTarget;

        float currentTimer = 0;
        float targetTimer = 0.05f;

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        CheckActionHierarchy();
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
            //Debug.Log("We arrived at our destination are we in range to attack here?");
            // Check the attack range distance, if we are out of range, start chasing.
            if(usePrimaryAttack && CheckDistance(myStats.attackRange, myTarget.transform))
            {
                //Debug.Log("We can attack and are in range");
                // Can we currently attack?
                if (myStats.currentAttackDelay > basicAttackDelay / myStats.attackSpeed )
                {
                    //Debug.Log("We have waited long enough to attack");
                    // Launch the attack.
                    myStats.currentAttackDelay = 0;
                    anim.SetTrigger("Attack");
                    anim.SetFloat("AttackAnimSpeed", myStats.attackSpeed);

                    // Set up the timers.
                    float currentTimer = 0;
                    float targetTimer = basicAttackDuration / myStats.attackSpeed;

                    //Debug.Log($"our duration is {basicAttackDuration} / {myStats.attackSpeed} to equal {basicAttackDuration / myStats.attackSpeed}");
                    while(currentTimer < targetTimer)
                    {
                        currentTimer += Time.deltaTime;

                        // Rotate towards the target.
                        //movementManager.RotateToTarget(myTarget.transform.position);
                        yield return new WaitForEndOfFrame();
                    }

                    CheckActionHierarchy();
                }
                else
                    movementManager.RotateToTarget(myTarget.transform.position);
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
            case ActionType.SpecialThree:
                if (specialThreeCurrentCooldown < specialThreeCooldown)
                    actionReady = false;
                else if (specialThreeAbility == EnemyAbilityBank.EnemyAbility.None)
                    actionReady = false;
                break;
            case ActionType.SpecialFour:
                if (specialFourCurrentCooldown < specialFourCooldown)
                    actionReady = false;
                else if (specialFourAbility == EnemyAbilityBank.EnemyAbility.None)
                    actionReady = false;
                break;
            default:
                break;
        }
        return actionReady;
    }

    // Used When the player we were fighting dies.
    public void TargetDeath()
    {
        //Debug.Log("The target is dead so wed deagro on them here.");
        StopAllCoroutines();
        myTarget = null;
        myCurrentAction = ActionType.ChaseTarget;
        SwitchAction(ActionType.ChaseTarget);
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
            SwitchAction(ActionType.ChaseTarget);
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
                case ActionType.SpecialOne:
                    if (specialOneCurrentCooldown > specialOneCooldown && Random.Range(0, 100) > 100 - actionChances[index] && CheckDistance(specialOneRange, myTarget.transform))
                        actionFound = true;
                    break;
                case ActionType.SpecialTwo:
                    if (specialTwoCurrentCooldown > specialTwoCooldown && Random.Range(0, 100) > 100 - actionChances[index] && CheckDistance(specialTwoRange, myTarget.transform))
                        actionFound = true;
                    break;
                case ActionType.SpecialThree:
                    if (specialThreeCurrentCooldown > specialThreeCooldown && Random.Range(0, 100) > 100 - actionChances[index] && CheckDistance(specialThreeRange, myTarget.transform))
                        actionFound = true;
                    break;
                case ActionType.SpecialFour:
                    if (specialFourCurrentCooldown > specialFourCooldown && Random.Range(0, 100) > 100 - actionChances[index] && CheckDistance(specialFourRange, myTarget.transform))
                        actionFound = true;
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
