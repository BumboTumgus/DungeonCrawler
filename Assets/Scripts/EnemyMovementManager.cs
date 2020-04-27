using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovementManager : MonoBehaviour
{
    public bool arrivedAtTarget = true;
    public bool enableMovement = false;

    private NavMeshAgent agent;
    private EnemyCombatController combatController;
    private PlayerStats myStats;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        myStats = GetComponent<PlayerStats>();
        combatController = GetComponent<EnemyCombatController>();
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
    }

    // used to stop us from moving 
    public void StopMovement()
    {
        agent.destination = transform.position;
        agent.speed = 0;
    }
}
