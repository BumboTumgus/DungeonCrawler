using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    // This acts as a bank of potential enemies, room, chests, events, items, and traps we can randomly spawn for this floor.

    // An array of gameobjects with any door that opens to the specific direction.
    public GameObject[] allRoomsTL;
    public GameObject[] allRoomsTR;
    public GameObject[] allRoomsBL;
    public GameObject[] allRoomsBR;

    public GameObject startingRoom;

    public GameObject[] enemies;
    public GameObject[] treasures;
    public GameObject[] traps;
}
