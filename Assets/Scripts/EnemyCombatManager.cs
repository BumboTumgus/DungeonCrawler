using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class EnemyCombatManager : MonoBehaviour
{
    public bool inCombat = false;
    public GameObject target;
    public enum ActiveEnemyTask {Engage, WaitForAttack, Attack, Idle, Follow, Patrol};
    public ActiveEnemyTask myTask;
    public LayerMask wallCheckMask;
    private Animator anim;
    private HitBoxManager hitBoxManager;
    private NavMeshAgent navMeshAgent;

    private bool grounded = true;
    private PlayerStats myStats;
    private PlayerStats playerStats;
    private EnemyMovement enemyMovement;
    [SerializeField] private GameObject[] playerPositions;
    private bool losingAgro = false;
    private LayerMask groundingRayMask = 1 << 10;
    private bool attackReady = true;
    private bool staggered = false;

    private const float DEAGRO_TIMER = 1;
    private const float DEAGRO_RANGE_MULTIPLIER = 1.7f;
    private const float ANIM_SPEED_REDUCTION = 5f;
    private const float ATTACK_DURATION = 0.5f;
    private const float GROUNDING_RAY_LENGTH = 0.6f;
    private const float ATTACK_SPEED_MULTIPLIER = 0.4f;
    private const float PLAYER_ROTATION_SPEED = 25;
    private const float STAGGER_DURATION = 0.8f;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Enemy Setup");
        myStats = GetComponent<PlayerStats>();
        enemyMovement = GetComponent<EnemyMovement>();
        anim = GetComponent<Animator>();
        hitBoxManager = GetComponent<HitBoxManager>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        playerPositions = GameObject.Find("GameManager").GetComponent<GameManager>().currentPlayers;
        myTask = ActiveEnemyTask.Idle;
        StartCoroutine(Idle());
    }

    // This method sets up which enemy our player will be in combat with.
    public void StartCombat(GameObject targetToFight)
    {
        target = targetToFight;
        inCombat = true;
        attackReady = true;
        losingAgro = false;
        myStats.currentAttackDelay = myStats.attackDelay;
        playerStats = target.GetComponent<PlayerStats>();
        StartEngage();
        StopCoroutine("Idle");
        StartCoroutine(Combat());
        Debug.Log("we are now in combat");
    }

    // This method is used to cancel combat and end the fight
    public void EndCombat()
    {
        target = null;
        inCombat = false;
        losingAgro = false;
        myTask = ActiveEnemyTask.Idle;
        enemyMovement.aggresive = false;
        enemyMovement.SetTarget(transform.position);
        playerStats = null;
        StopCoroutine("Combat");
        StartCoroutine(Idle());
        anim.SetFloat("Speed", 0);
        Debug.Log(" we have now left combat");
    }

    // This method is used to start the movement of the player to check to see if they are in range.
    public void StartEngage()
    {
        myTask = ActiveEnemyTask.Engage;
        enemyMovement.aggresive = true;
        enemyMovement.SetTarget(target.transform.position);
        navMeshAgent.speed = myStats.speed;
        enemyMovement.arrivedAtTarget = false;
    }

    // Used by the player movment to let us know we are in range.
    public void InRange()
    {
        myTask = ActiveEnemyTask.WaitForAttack;
    }
    
    // USed to start the attack logic.
    IEnumerator Attack()
    {
        attackReady = false;
        myStats.currentAttackDelay = 0;
        anim.SetTrigger("Attack");
        anim.SetFloat("AnimSpeed", myStats.speed / ANIM_SPEED_REDUCTION);
        navMeshAgent.speed = myStats.speed * ATTACK_SPEED_MULTIPLIER;

        // Steup the attack timers here
        float currentTimer = 0;
        float targetTimer = ATTACK_DURATION * ANIM_SPEED_REDUCTION / myStats.speed;
        bool attackLaunched = false;
        while(currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;
            // Rotate to the target, and if we wait for long enough launch the hitbox.
            RotateToTarget();
            if(!attackLaunched && currentTimer > targetTimer / 2)
            {
                attackLaunched = true;
                hitBoxManager.LaunchHitBox(0);
            }
            // If the enemy gets staggered mid attack, break from this corotuine
            if (staggered)
                break;
            myStats.currentAttackDelay = 0;
            yield return new WaitForEndOfFrame();
        }

        attackReady = true;
        navMeshAgent.speed = myStats.speed;
        // Sets our current task to waiting until our attack can go off sicne we are in range and just attacked.
        myTask = ActiveEnemyTask.WaitForAttack;
    }

    // Used to check and see if a player is in their agro range. 
    private void CheckAgroRange(float agroRangeMultiplier)
    {
        // Check to see if we have line of sight of any player.
        GameObject closestPlayer = null;
        float closestPlayerDistance = Mathf.Infinity;

        foreach(GameObject player in playerPositions)
        {
            Ray ray = new Ray(transform.position + Vector3.up, (player.transform.position - transform.position).normalized);
            RaycastHit rayhit = new RaycastHit();
            Debug.DrawRay(transform.position + Vector3.up, (player.transform.position - transform.position).normalized * myStats.agroRange * agroRangeMultiplier);

            if (Physics.Raycast(ray, out rayhit, myStats.agroRange * agroRangeMultiplier, wallCheckMask))
            {
                // Check to see if the raycast hit a player, and check to see if the previous players were closer. Also check if the player we hit is dead
                if (rayhit.transform.tag == "Player" && (player.transform.position - transform.position).sqrMagnitude < closestPlayerDistance && !player.GetComponent<PlayerStats>().dead)
                {
                    // Set this player as the closest player that we will agro onto later.
                    closestPlayer = player;
                    closestPlayerDistance = (player.transform.position - transform.position).sqrMagnitude;
                }
            }
        }

        // Check if we are in combat or not. if not, agro ontpo the closest player.
        if (!inCombat && closestPlayer != null)
            StartCombat(closestPlayer);
        // If we are in combat but there is a closer player, switch agro.
        else if (inCombat && closestPlayer != null && closestPlayer != target)
            StartCombat(closestPlayer);
        // If no one ius nearby start the deagro check.
        else if (!losingAgro && inCombat)
            StartCoroutine(OutOfCombatCheck());
    }

    // USed to rotate the enemy to thweir target so they face their target while they attack.
    private void RotateToTarget()
    {
        Vector3 horizontalMovement = target.transform.position - transform.position;
        horizontalMovement.y = 0;
        if (horizontalMovement.sqrMagnitude >= 0.2)
        {
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(horizontalMovement.x, 0, horizontalMovement.z).normalized, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, PLAYER_ROTATION_SPEED / 100);
        }
    }

    // This method is the standard combat loop
    IEnumerator Combat()
    {
        // As long as the player is in combat
        while (inCombat)
        {
            // Attack delay timer increment
            myStats.currentAttackDelay += Time.deltaTime;

            switch (myTask)
            {
                case ActiveEnemyTask.Engage:
                    // Used when the player is moving towards the target, 
                    // we don't do anythign here, the playermovement class does this for us.
                    enemyMovement.SetTarget(target.transform.position);
                    CheckAgroRange(DEAGRO_RANGE_MULTIPLIER);
                    anim.SetFloat("AnimSpeed", myStats.speed / ANIM_SPEED_REDUCTION);
                    anim.SetFloat("Speed", 1);
                    CheckGrounded();
                    //yield return new WaitForSeconds(2);
                    break;
                case ActiveEnemyTask.WaitForAttack:
                    RotateToTarget();
                    // We never have to check for agro range here. If the target is in range of our attack, they should be agrod.
                    // If the target is dead, check agro range again t reagro on a new player.
                    if (playerStats.dead)   
                        CheckAgroRange(DEAGRO_RANGE_MULTIPLIER);

                    // If the target is out of range restart the engage loop.
                    if ((target.transform.position - transform.position).sqrMagnitude >= myStats.attackRange * myStats.attackRange)
                        StartEngage();
                    // If the above is not true then we are in range and we can poll the attack timer.
                    else if (myStats.currentAttackDelay > myStats.attackDelay && !staggered)
                        myTask = ActiveEnemyTask.Attack;

                    anim.SetFloat("AnimSpeed", myStats.speed / ANIM_SPEED_REDUCTION);
                    anim.SetFloat("Speed", 0);
                    CheckGrounded();
                    break;
                case ActiveEnemyTask.Attack:
                    // Launcht the attack
                    if (attackReady && !staggered)
                        StartCoroutine(Attack());
                    CheckGrounded();
                    break;
                default:
                    break;
            }
            yield return new WaitForEndOfFrame();
        }
    }
    
    // Used to check if the player is still grounded
    private void CheckGrounded()
    {
        Ray groundRay = new Ray(transform.position + Vector3.up * 0.5f, Vector3.down);
        RaycastHit groundRayHit;
        // Shoot a ray, if it we hit we are grounded if not we are no longer grounded. If we just jumped ignore this and set us as not grounded.
        if (Physics.Raycast(groundRay, out groundRayHit, GROUNDING_RAY_LENGTH, groundingRayMask))
        {
            grounded = true;
            anim.SetBool("Grounded", true);
        }
        else
        {
            grounded = false;
            anim.SetBool("Grounded", false);
        }
    }

    // USed to check if enemies are in range.
    IEnumerator Idle()
    {
        while(!inCombat)
        {
            CheckAgroRange(1);
            CheckGrounded();
            yield return new WaitForEndOfFrame();
        }
    }

    // USed to see if we have been out of combat for 3 full seconds
    IEnumerator OutOfCombatCheck()
    {
        losingAgro = true;

        yield return new WaitForSeconds(DEAGRO_TIMER);

        // Check to see if we have line of sight of the player. If we dont, then we lose agro. If we do and they are in range we retain agro.
        Ray ray = new Ray(transform.position, (target.transform.position - transform.position).normalized);
        RaycastHit rayhit = new RaycastHit();
        Debug.DrawRay(transform.position, (target.transform.position - transform.position).normalized * myStats.agroRange * DEAGRO_RANGE_MULTIPLIER);

        if (Physics.Raycast(ray, out rayhit, myStats.agroRange * DEAGRO_RANGE_MULTIPLIER, wallCheckMask))
        {
            // Check to see if the raycast hit a player that is alive, if so, break the loop.
            if (rayhit.transform.gameObject == target && !target.GetComponent<PlayerStats>().dead)
                losingAgro = false;
            // If we do not hit a player, end the combat and deagro.
            else
                EndCombat();
        }
        // If we do not hit anything, end combat
        else
            EndCombat();
    }
    
    // USed to stagger the player when their poise gets broken.
    public void StaggerLaunch()
    {
        StartCoroutine(Stagger());
    }

    // The stagger coroutine. Laucnehs a player back a setp or based based on a direction, and makes them unable to act for this time.
    IEnumerator Stagger()
    {
        staggered = true;
        anim.SetTrigger("Staggered");
        navMeshAgent.speed = 0;
        yield return new WaitForSeconds(STAGGER_DURATION);
        navMeshAgent.speed = myStats.speed;
        staggered = false;
    }
}
