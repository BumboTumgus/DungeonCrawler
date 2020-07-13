using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpenVolumeBehaviour : MonoBehaviour
{
    public bool openInwards = false;

    private DoorBehaviour doorBehaviour;

    private void Start()
    {
        doorBehaviour = transform.parent.GetComponent<DoorBehaviour>();
    }

    // Used when the player wishes to interact with the door.
    public void InteractWithDoor()
    {
        doorBehaviour.InteractWithDoor(openInwards);
    }
}
