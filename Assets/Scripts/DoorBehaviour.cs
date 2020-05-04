using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{
    public bool doorOpen = true;

    bool canInteractWithDoor = true;
    
    public void InteractWithDoor()
    {
        if(doorOpen)
        {
            // Close the door
            GetComponentInChildren<Animator>().Play("Close");
            StartCoroutine(DoorTimer());
        }
        else
        {
            // Open the door
            GetComponentInChildren<Animator>().Play("Open");
            StartCoroutine(DoorTimer());
        }
    }

    // Used to set a timer before the door can be interacted with again.
    IEnumerator DoorTimer()
    {
        if (doorOpen)
            doorOpen = false;
        else
            doorOpen = true;

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
