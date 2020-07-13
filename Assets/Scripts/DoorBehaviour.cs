using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{
    public enum DoorState { OpenedOutward, OpenedInward, Closed};
    public DoorState doorState = DoorState.Closed;

    public Animator[] animators;

    private bool canInteractWithDoor = true;

    public void InteractWithDoor(bool openInward)
    {
        switch (doorState)
        {
            case DoorState.OpenedOutward:
                // Close the door
                foreach (Animator anim in animators)
                    anim.Play("CloseOutwards");
                doorState = DoorState.Closed;
                StartCoroutine(DoorTimer());
                break;
            case DoorState.OpenedInward:
                // Close the door
                foreach (Animator anim in animators)
                    anim.Play("CloseInwards");
                doorState = DoorState.Closed;
                StartCoroutine(DoorTimer());
                break;
            case DoorState.Closed:
                // Open the door
                if(openInward)
                {
                    foreach (Animator anim in animators)
                        anim.Play("OpenInwards");
                    doorState = DoorState.OpenedInward;
                }
                else
                {
                    foreach (Animator anim in animators)
                        anim.Play("OpenOutwards");
                    doorState = DoorState.OpenedOutward;
                }

                StartCoroutine(DoorTimer());
                break;
            default:
                break;
        }
    }

    // Used to set a timer before the door can be interacted with again.
    IEnumerator DoorTimer()
    {
        canInteractWithDoor = false;
        float currentTimer = 0;
        float targetTimer = 2;

        while(currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;
            yield return null;
        }

        canInteractWithDoor = true;
    }
}
