using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionExecutor : MonoBehaviour
{
    public GameObject actionCompletionBarParent;

    private ActionQueueManager actionQueueManager;
    private GameObject targetObject;
    private PlayerStats myStats;

    private BarManager actionBar;

    private void Start()
    {
        actionQueueManager = GetComponent<ActionQueueManager>();
        myStats = GetComponent<PlayerStats>();
        actionBar = actionCompletionBarParent.GetComponentInChildren<BarManager>();
        actionBar.SetValue(0);
    }

    public void CancelActions()
    {
        StopAllCoroutines();
    }

    // Used to set the players current action that they will attempt to complete. overrides previous actions.
    public void SetAction(ActionQueueManager.ActiveTask taskToStart, GameObject TargetObject)
    {
        CancelActions();
        if (TargetObject != null)
            targetObject = TargetObject;
        switch (taskToStart)
        {
            case ActionQueueManager.ActiveTask.Idle:
                // This should never happen hyptothetically.
                break;
            case ActionQueueManager.ActiveTask.Walking:
                break;
            case ActionQueueManager.ActiveTask.OpeningChest:
                StartCoroutine(OpenChest());
                break;
            case ActionQueueManager.ActiveTask.Downed:
                break;
            case ActionQueueManager.ActiveTask.Dead:
                break;
            case ActionQueueManager.ActiveTask.PickUpItem:
                AttemptItemPickUp();
                break;
            default:
                break;
        }
    }

    //Used when the player has finished their actions, could be connected to an action array later to queue up multiple actions.
    private void ActionCompleted()
    {
        actionQueueManager.activeTask = ActionQueueManager.ActiveTask.Idle;

        // Used to hide and reset the bar.
        actionCompletionBarParent.GetComponent<UiFollowTarget>().enabled = false;
        actionCompletionBarParent.transform.position = new Vector3(1000, 1000, 1000);
        actionBar.SetValue(0);
    }

    // Used when a player opens a chest.
    IEnumerator OpenChest()
    {
        // Enable the bars and set up the timers.
        actionCompletionBarParent.GetComponent<UiFollowTarget>().enabled = true;
        actionCompletionBarParent.GetComponent<UiFollowTarget>().target = targetObject.transform;
        float currentTimer = 0;
        float targetTimer = 5f;

        // Change the text on the bar and initialize the bar.
        actionCompletionBarParent.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = "Opening the chest...";
        actionBar.Initialize(targetTimer, false);
        actionBar.SetValue(0);
        UnityEngine.UI.Text timeLeftText = actionCompletionBarParent.transform.GetChild(2).GetComponent<UnityEngine.UI.Text>();

        // Wait for the allotted time, then open the chest.
        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;

            // Set the bar's value and format the remainder as a nice minute: second timer.
            actionBar.SetValue(currentTimer);
            // seconds = Mathf.Floor(targetTimer - currentTimer).ToString("00");
            // milliseconds = Mathf.Floor((targetTimer - currentTimer) / 10).ToString("00");
            // timeLeftText.text = string.Format("{0}:{1}", seconds, milliseconds);
            timeLeftText.text = (targetTimer - currentTimer).ToString("F1");

            yield return new WaitForEndOfFrame();
        }

        targetObject.GetComponent<ChestBehaviour>().OpenChest();
        Debug.Log("The Chest has been opened");
        
        ActionCompleted();
    }

    // Used by the player to attempt to pickUp an Item
    private void AttemptItemPickUp()
    {
        Debug.Log("The item has been attempted to be picked up");

        targetObject.GetComponent<ItemBehaviour>().AttemptPickup();

        ActionCompleted();
    }
}
