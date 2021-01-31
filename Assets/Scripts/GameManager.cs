using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public List<RoomManager> rooms = new List<RoomManager>();
    public GameObject[] currentPlayers;
    public Transform[] spawnsPlayer;
    public Transform[] spawnsChest;
    public Transform[] spawnsEnemy;
    public NavMeshSurface walkableFloor;
    public int enemyCount = 3;
    public int chestCount = 3;
    public int roomTarget = 20;

    public float currentRoomGenTimer = 0;

    private GameObject startingRoom;

    private const float TAREGT_ROOM_GEN_TIMER = 0.5f;

    // Start is called before the first frame update
    void Awake()
    {
        // Debug.Log("Game manager setup");
        if (instance == null)
            instance = this;

        currentPlayers = GameObject.FindGameObjectsWithTag("Player");

        //StartCoroutine(Initialization());
    }

    //The corotuine that runs on level start.
    IEnumerator Initialization()
    {
        roomTarget = Random.Range(5, 20) + Random.Range(5,20);

        startingRoom = Instantiate(GameObject.Find("FloorManager").GetComponent<FloorManager>().startingRoom, Vector3.zero, Quaternion.identity);

        // Wait until room generation is done.
        while (currentRoomGenTimer < TAREGT_ROOM_GEN_TIMER)
        {
            currentRoomGenTimer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        //Call the function that checks for adjacent rooms on each room
        foreach (RoomManager room in rooms)
            room.LaunchAdjacencyChecker();

        // This is done afetr room generation is comfirmed to be completed.
        Debug.Log("Room population would begin now.");
        //walkableFloor.BuildNavMesh();
        foreach (RoomManager room in rooms)
        {
            //if(room.GetComponent<RoomPopulator>() != null)
            //  room.GetComponent<RoomPopulator>().PopulateRoom();
        }

        yield return new WaitForFixedUpdate();
        //Show the initial room's renderable
        ShowRoom(startingRoom.GetComponent<RoomManager>());

        GrabSpawns();
        SpawnPlayers();
    }

    public void AddRoom(RoomManager room)
    {
        rooms.Add(room);
        currentRoomGenTimer = 0;
    }

    // Used to grab and set all the spawns;
    private void GrabSpawns()
    {
        // Sets up the array of player spawns.
        Transform playerSpawnParent = startingRoom.transform.Find("Spawns_Players");
        spawnsPlayer = new Transform[playerSpawnParent.childCount];
        for (int index = 0; index < playerSpawnParent.childCount; index++)
        {
            spawnsPlayer[index] = playerSpawnParent.GetChild(index);
        }
    }

    // Used to spawn in the current players into the room.
    private void SpawnPlayers()
    {
        foreach (GameObject player in currentPlayers)
        {
            player.transform.position = spawnsPlayer[Random.Range(0, spawnsPlayer.Length)].transform.position;
        }
    }

    // Used to hide all rooms then show the ones that are adjacent to the new room
    public void ShowRoom(RoomManager targetRoom)
    {
        //Debug.Log("We are showing room: " + targetRoom.gameObject.name);

        targetRoom.ShowAdjacentRooms();

        foreach (RoomManager room in rooms)
            if(!targetRoom.connectedRooms.Contains(room) && room != targetRoom)
                room.HideRoom();
        
    }
}


