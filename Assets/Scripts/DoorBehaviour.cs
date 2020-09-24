using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{
    public enum DoorState { OpenedOutward, OpenedInward, Closed};
    public DoorState doorState = DoorState.Closed;

    public Animator[] animators;

    //private bool canInteractWithDoor = true;

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

                DoorOpened();
                StartCoroutine(DoorTimer());
                break;
            default:
                break;
        }
    }

    // Used to set a timer before the door can be interacted with again.
    IEnumerator DoorTimer()
    {
        //canInteractWithDoor = false;
        float currentTimer = 0;
        float targetTimer = 2;

        while(currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;
            yield return null;
        }

        //canInteractWithDoor = true;
    }

    //Used when a door is opened. this will ensure that all other doors in the room get closed while this one opens.
    private void DoorOpened()
    {
        Debug.Log("A door was opened, we now begin closing all others");
        // Launches the door opened method in the room manager one level above me in the hierarchy in my parent.
        RoomManager myRoom = transform.parent.GetComponent<RoomManager>();
        if (myRoom)
            myRoom.DoorOpen(this);
        else
            Debug.Log("this door didnt find it's conencted room something might not be connected");
    }
}
