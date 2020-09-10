using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomAdjacencyChecker : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // If the adjacency checker hits a room volume, we will add the respective room to our connected rooms
        if(collision.transform.CompareTag("RoomVolume"))
        {
            transform.parent.GetComponent<RoomManager>().connectedRooms.Add(collision.transform.parent.GetComponent<RoomManager>());
        }
    }
}
