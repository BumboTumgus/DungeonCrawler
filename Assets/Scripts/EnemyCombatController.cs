using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombatController : MonoBehaviour
{
    public bool patrol = false;
    public bool inCombat = false;
    public float agroRange = 20f;
    public GameObject myTarget;
    // A list of potential actions or behaviours
    public enum ActionType { Attack, ChaseTarget, CircleTarget, TauntTarget, MaintainDistance, ChaseLowTargets, RetreatWhenLow, RetreatWhenNoLeader, Idle, Patrolling, SpecialOne, SpecialTwo, SpecialThree, SpecialFour};
    public ActionType myCurrentAction = ActionType.Idle;

    // This is the action hierarchy List, The actions go in in order of importance.
    public List<ActionType> ActionHierarchy = new List<ActionType>();
    public LayerMask wallColMask;

    private EnemyMovementManager movementManager;
    [SerializeField] private GameObject[] playerPositions;
    private PlayerStats myStats;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        movementManager = GetComponent<EnemyMovementManager>();
        playerPositions = GameObject.Find("GameManager").GetComponent<GameManager>().currentPlayers;
        myStats = GetComponent<PlayerStats>();
        anim = GetComponent<Animator>();
        inCombat = false;
        SwitchAction(ActionType.Idle);
    }

    // Update is called once per frame
    void Update()
    {

    }

    // This method is called when we need to revaluate what state we are in and what to do next.
    public void SwitchAction(ActionType newAction)
    {
        Debug.Log("switching toi a new action");
        StopAllCoroutines();
        switch (newAction)
        {
            case ActionType.Attack:
                break;
            case ActionType.ChaseTarget:
                StartCoroutine(ChaseTarget());
                break;
            case ActionType.CircleTarget:
                break;
            case ActionType.TauntTarget:
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
                break;
            case ActionType.SpecialTwo:
                break;
            case ActionType.SpecialThree:
                break;
            case ActionType.SpecialFour:
                break;
            default:
                break;
        }
    }

    // The idle corotuine, we check to see if enemies are in range
    IEnumerator Idle()
    {
        Debug.Log("we are now idling");
        movementManager.StopMovement();
        myCurrentAction = ActionType.Idle;

        float targetTimer = 0.2f;
        float currentTimer = 0;
        while(!inCombat)
        {
            // Increment the time and see if we surpassed our check target.
            currentTimer += Time.deltaTime;
            if (currentTimer > targetTimer) 
            {
                currentTimer -= targetTimer;
                AgroOntoNearestTarget();
            }
            yield return null;
        }
    }

    // the corotuine that i9s called when we are to chase the player.
    IEnumerator ChaseTarget()
    {
        // allow us to chase the target in question.
        movementManager.SetTarget(myTarget.transform.position);
        movementManager.enableMovement = true;

        float currentTimer = 0;
        float targetTimer = 0.2f;
        while(inCombat)
        {
            currentTimer += Time.deltaTime;
            if(currentTimer > targetTimer)
            {
                currentTimer -= targetTimer;
                Debug.Log("deagro check.");
            }
            yield return null;
        }
        // Here we check for a deagro and check to see if we are in range for an attack.

        yield return null;
    }

    // Thsi method checks the dustance between myself and the target
    public bool CheckDistance(float distance, Transform target)
    {
        bool inRange = false;
        
        // Check the duistance between the two.
        if((target.position - transform.position).sqrMagnitude < distance * distance)
        {
            inRange = true;
        }

        return inRange;
    }

    // This method is used to check the distance between myserlf and a target, but does not allow us to return true if something is obstructin the view of the player completely.
    public bool CheckDistanceWallObstructed(float distance, Transform target)
    {
        bool inRange = false;

        // Check the distance between the two.
        if ((target.position - transform.position).sqrMagnitude < distance * distance)
        {
            Debug.Log("ray is being shot now because they are in range");
            // If it's in range shoot a ray at the target to see if we hit a wall.
            Ray ray = new Ray(transform.position + new Vector3(0,1,0), (target.position - transform.position).normalized);
            Debug.DrawRay(transform.position + new Vector3(0, 1, 0), (target.position - transform.position).normalized * 30, Color.red);
            RaycastHit rayhit;

            if (Physics.Raycast(ray, out rayhit, distance, wallColMask))
            {
                if (rayhit.transform.CompareTag("Player"))
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
        Debug.Log("checking for nearest player to agro onto");
        // Check to see if any players are in range.
        bool playersInRange = false;
        float closestDistance = Mathf.Infinity;
        GameObject closestPlayer = null;
        foreach (GameObject player in playerPositions)
        {
            // Check if the player is within my agro range, and if they are blocked by walls. Grab the closest player within the agro range.
            if(CheckDistanceWallObstructed(agroRange, player.transform))
            {
                // If this player is closer, set it as the closest player to me.
                if(closestDistance > (player.transform.position - transform.position).sqrMagnitude)
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
        Debug.Log("we are starting combat now");
        inCombat = true;
        myTarget = target;
        SwitchAction(ActionType.ChaseTarget);
    }
}
