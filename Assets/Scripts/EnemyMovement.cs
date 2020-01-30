using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public bool arrivedAtTarget = true;
    public bool aggresive = false;

    private NavMeshAgent agent;
    private EnemyCombatManager combatManager;
    private PlayerStats myStats;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        myStats = GetComponent<PlayerStats>();
        combatManager = GetComponent<EnemyCombatManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // If the aggresive option is ticked, then we are in combat with this object.
        if (aggresive && !arrivedAtTarget && (agent.destination - transform.position).sqrMagnitude <= myStats.attackRange * myStats.attackRange)
        {
            arrivedAtTarget = true;
            agent.speed = 0;
            combatManager.InRange();
        }
    }

    // Used to set the target position as well as the destination marker.
    public void SetTarget(Vector3 Position)
    {
        agent.destination = Position;
    }
}

