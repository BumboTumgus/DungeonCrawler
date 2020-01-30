using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomVolume : MonoBehaviour
{
    public bool obstructed = false;
    public List<RoomSpawner> connectedSpawners = new List<RoomSpawner>();

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("RoomVolume"))
        {
            obstructed = true;
        }
        // Add it to a list of spawns we obstructed that way if we move a room around we can clear the list of spawns we obstructed,
        else if ( collision.transform.CompareTag("RoomSpawner"))
        {
            RoomSpawner roomSpawn = collision.transform.GetComponent<RoomSpawner>();
            // We only add the room spawer if it isn't already obstructed. This means each spawner can only be obstructed by one volume at a time.
            if (!roomSpawn.obstructed)
                transform.parent.GetComponent<RoomManager>().obstructedSpawns.Add(roomSpawn);
        }
    }

    // Used to reset collision data and flicker the collider so that we register a new collision when our position is updated.
    public void ClearVolumeData()
    {
        obstructed = false;
        GetComponent<BoxCollider>().enabled = false;
        GetComponent<BoxCollider>().enabled = true;
    }
}
