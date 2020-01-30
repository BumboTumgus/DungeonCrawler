using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionQueueManager : MonoBehaviour
{
    public enum ActiveTask { Idle, Walking, OpeningChest, Downed, Dead, PickUpItem, Engage, WaitForAttack, Attack};
    public ActiveTask activeTask;

    private GameObject targetObject;
    private PlayerMovement playerMovement;
    private ActionExecutor actionExecutor;
    private PlayerStats myStats;
    
    // Start is called before the first frame update
    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        actionExecutor = GetComponent<ActionExecutor>();
        myStats = GetComponent<PlayerStats>();
    }


    // Sets the character current action to walk towards and objecta nd interact with it.
    public void SetInteraction(GameObject target)
    {
        targetObject = target;

        if (target.tag == "Enemy")
            Debug.Log("oof");
        else
        {
            playerMovement.SetTarget(target.transform.position);
            playerMovement.arrivedAtTarget = false;
            activeTask = ActiveTask.Walking;
        }
        StopAllCoroutines();

        Debug.Log("Interaction Set");
    }


    // This method is sued when only movement is needed, and no object must be interacted with.
    public void SetInteraction(Vector3 Position)
    {
        playerMovement.SetTarget(Position);
        activeTask = ActiveTask.Walking;
        playerMovement.arrivedAtTarget = false;
        targetObject = null;
    }


    // Is called when the character arrives at the object and tries to interact with it.
    public void AttemptInteraction()
    {
        if(targetObject != null)
        {
            Debug.Log("Attempting interaction");
            if(targetObject.tag == "Chest")
            {
                activeTask = ActiveTask.OpeningChest;
                actionExecutor.SetAction(activeTask, targetObject);
            }
            if(targetObject.tag == "Item")
            {
                activeTask = ActiveTask.PickUpItem;
                actionExecutor.SetAction(activeTask, targetObject);
            }
        }
        else
            activeTask = ActiveTask.Idle;
    }
}
