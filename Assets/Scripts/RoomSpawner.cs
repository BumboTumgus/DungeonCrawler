using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public float priority;

    public enum DoorOpening { TopLeft, TopRight, BottomLeft, BottomRight, Null};
    public DoorOpening doorDirection;

    public List<DoorOpening> requirements = new List<DoorOpening>();
    public List<RoomSpawner> overlayedSpawns = new List<RoomSpawner>();

    public bool obstructed = false;
    public bool lowerPrio = false;
    public bool ignoreObstruction = false;

    private void Start()
    {
        priority = Random.Range(0f, 10f);
    }

    // Called when we enter collision with another spawn point. This lets us know that two room will be spawning in the same location.
    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Spawn had a collision");
        if(collision.transform.CompareTag("RoomSpawner"))
        {
            // If the other has a higher priority then this, then delete ourselves.
            if (collision.gameObject.GetComponent<RoomSpawner>().priority > priority)
                lowerPrio = true;

            // Add this overlayed spawn to our list.
            overlayedSpawns.Add(collision.gameObject.GetComponent<RoomSpawner>());

            // If this spawn is set to spawn a room still, it will send a message to it's room Manager of the new updated info.
            // Check each index of the arry, if it's null add in our requirement.
            requirements.Add(collision.gameObject.GetComponent<RoomSpawner>().doorDirection);
        }
        else if (collision.transform.CompareTag("RoomVolume"))
        {
            obstructed = true;
            WipePriorityData();
        }
    }

    // Called to wipe all the spawn overlay data in the evnt of an obstruction.
    private void WipePriorityData()
    {
        requirements = new List<DoorOpening>();
        overlayedSpawns = new List<RoomSpawner>();
    }   

    // Used when we nee to wipe some of the priority data, in the case there is a registered overlay but then the room gets rerolled.
    public void RemovePriorityData()
    {
        // for every spawn we recognized we were overlayed on, we call this function.
        foreach(RoomSpawner spawn in overlayedSpawns)
        {
            spawn.RemovePriorityData(doorDirection, this);
        }
        WipePriorityData();
    }
    // This is the second version of the function that starts the actual removal process.
    public void RemovePriorityData(DoorOpening targetData, RoomSpawner targetDataContainer)
    {
        // Check our lists for the data na dthe data contaienr and remove them both, then recheck our priority.
        if (requirements.Contains(targetData))
            requirements.Remove(targetData);
        if (overlayedSpawns.Contains(targetDataContainer))
            overlayedSpawns.Remove(targetDataContainer);

        // If the data list is empty, we can ignore the priority check entirely.
        if (requirements.Count <= 0)
            lowerPrio = false;
        // Check the priority between the other remianing spawns.
        else
        {
            lowerPrio = false;
            foreach(RoomSpawner spawn in overlayedSpawns)
            {
                if (priority < spawn.priority)
                    lowerPrio = true;
            }
        }
    }

    // Used to wipe all the data associated with this spawn in the case a large room has been shifetd over.
    public void ClearSpawnData()
    {
        // for every spawn we recognized we were overlayed on, we call this function.
        foreach (RoomSpawner spawn in overlayedSpawns)
        {
            spawn.RemovePriorityData(doorDirection, this);
        }
        WipePriorityData();
        obstructed = false;
        lowerPrio = false;
        ignoreObstruction = false;

        // now we remove the rigidbody and box collider and re add them in the next frame as a collision check.
        GetComponent<BoxCollider>().enabled = false;
        //new WaitForEndOfFrame();
        GetComponent<BoxCollider>().enabled = true;
    }
}
