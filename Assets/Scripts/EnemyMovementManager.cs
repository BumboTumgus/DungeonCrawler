using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovementManager : MonoBehaviour
{
    public bool arrivedAtTarget = true;
    public bool enableMovement = false;

    public float spinSpeed = 0.05f;
    public int currentStance = 0;

    public NavMeshAgent agent;
    private EnemyCombatController combatController;
    private PlayerStats myStats;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        myStats = GetComponent<PlayerStats>();
        rb = GetComponent<Rigidbody>();
        combatController = GetComponent<EnemyCombatController>();
        GetComponent<Animator>().SetInteger("CurrentStance", currentStance);
    }

    // Update is called once per frame
    void Update()
    {
        // If the aggresive option is ticked, then we are in combat with this object.
        if (enableMovement && !arrivedAtTarget && (agent.destination - transform.position).sqrMagnitude <= myStats.attackRange * myStats.attackRange)
        {
            arrivedAtTarget = true;
            agent.speed = 0;
        }
    }

    // Used to set the target position as well as the destination marker.
    public void SetTarget(Vector3 Position)
    {
        agent.destination = Position;
        agent.speed = myStats.speed * myStats.movespeedPercentMultiplier;
        arrivedAtTarget = false;
    }

    // used to stop us from moving 
    public void StopMovement()
    {
        //agent.destination = transform.position;
        if(agent != null)
            agent.speed = 0;
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        arrivedAtTarget = true;
    }

    // USed to rotate the enemy to their target so they face their target while they attack.
    public void RotateToTarget(Vector3 target)
    {
        Vector3 horizontalMovement = target - transform.position;
        horizontalMovement.y = 0;
        // Debug.Log("Rotating to target, the horizontal mvoeemnt is: " + horizontalMovement);
        if (horizontalMovement.sqrMagnitude >= 0.2)
        {
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(horizontalMovement.x, 0, horizontalMovement.z).normalized, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, spinSpeed);
        }
    }

    // Used to add a force to this unit and forcibly move it to a location
    public void MoveToTarget(Vector3 target, float speed)
    {
        Vector3 direction = (transform.position - target).normalized;
        rb.velocity = direction * speed * Time.deltaTime * -100;
        rb.angularVelocity = Vector3.zero;
        //Debug.Log("moving the unit with a force of " + direction * speed * Time.deltaTime * 1000);
    }
}
