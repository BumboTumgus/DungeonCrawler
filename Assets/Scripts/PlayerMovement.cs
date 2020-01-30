using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerMovement : MonoBehaviour
{
    public bool arrivedAtTarget = true;
    public bool aggresive = false;
    
    private NavMeshAgent agent;
    private Transform destinationMarker;
    private ActionQueueManager actionQueueManager;
    private PlayerStats myStats;

    private const float INTERACTION_DISTANCE = 2f;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        destinationMarker = GameObject.Find("DestinationMarkerContainer").transform;
        actionQueueManager = GetComponent<ActionQueueManager>();
        myStats = GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    { 
        // Check to see if we are close enough to the target to interact with it.
        if (!aggresive && !arrivedAtTarget && (agent.destination - transform.position).sqrMagnitude <= INTERACTION_DISTANCE)
        {
            arrivedAtTarget = true;
            agent.destination = transform.position;
            destinationMarker.position = new Vector3(100, 100, 100);
            actionQueueManager.AttemptInteraction();
        }
        // If the aggresive option is ticked, then we are in combat with this object.
        else if(aggresive && !arrivedAtTarget && (agent.destination - transform.position).sqrMagnitude <= myStats.attackRange * myStats.attackRange)
        {
            arrivedAtTarget = true;
            agent.destination = transform.position;
            destinationMarker.position = new Vector3(100, 100, 100);
        }
    }

    // Used to set the target position as well as the destination marker.
    public void SetTarget(Vector3 Position)
    {
        agent.destination = Position;
        destinationMarker.position = Position;
    }
}
