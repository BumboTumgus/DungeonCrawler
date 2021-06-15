using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public string[] sceneNames;
    //public List<RoomManager> rooms = new List<RoomManager>();
    public GameObject[] currentPlayers;
    public Transform[] spawnsPlayer;
    public Transform[] spawnsChest;
    //public Transform[] spawnsEnemy;
    //public NavMeshSurface walkableFloor;
    //public int enemyCount = 3;
    [SerializeField] GameObject chestPrefab;
    [SerializeField] GameObject teleporterPrefab;
    [SerializeField] GameObject playerCharacterPrefab;
    [SerializeField] GameObject playerUiPrefab;
    [SerializeField] GameObject playerCameraPrefab;
    [SerializeField] GameObject eventSystemUI;
    [SerializeField] Animator cameraFadeAnim;

    //public int roomTarget = 20;

    //public float currentRoomGenTimer = 0;

    //private GameObject startingRoom;

    private const float MINIMUM_DISTANCE_FROM_TELEPORTER = 1000f;
    //private const float TAREGT_ROOM_GEN_TIMER = 0.5f;

    // Start is called before the first frame update
    void Awake()
    {
        // Debug.Log("Game manager setup");
        if (instance == null)
            instance = this;

        // Delete the main camera so we can replace it with ours.
        Destroy(Camera.main.gameObject);

        // Here we create a player, their ui and a camera and connect everything.
        GameObject player = Instantiate(playerCharacterPrefab);
        GameObject camera = Instantiate(playerCameraPrefab);
        GameObject playerUi = Instantiate(playerUiPrefab);
        GameObject eventSystem = Instantiate(eventSystemUI);

        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(player);
        DontDestroyOnLoad(camera);
        DontDestroyOnLoad(playerUi);
        DontDestroyOnLoad(eventSystem);

        cameraFadeAnim = playerUi.transform.Find("FadeInOutPanel").GetComponent<Animator>();

        // Create all the necessary connections between the player and the ui and the camera
        camera.GetComponent<FollowPlayer>().playerTarget = player.transform;

        playerUi.GetComponent<PauseMenuController>().pmc = player.GetComponent<PlayerMovementController>();
        playerUi.transform.Find("InventoryPanel").GetComponent<InventoryUiManager>().playerInventory = player.GetComponent<Inventory>();
        playerUi.transform.Find("InventoryPanel").GetComponent<InventoryUiManager>().playerSkills = player.GetComponent<SkillsManager>();

        player.GetComponent<PlayerStats>().myStats = playerUi.transform.Find("InventoryPanel").Find("Stats").GetComponent<StatUpdater>();
        player.GetComponent<PlayerStats>().healthBar = playerUi.transform.Find("PlayerStats").Find("HealthBar").Find("HealthBarBackground").GetComponent<BarManager>();
        player.GetComponent<Inventory>().interactPrompt = playerUi.transform.Find("InteractPrompt").GetComponent<InteractPromptController>();
        player.GetComponent<Inventory>().inventoryUI = playerUi.transform.Find("InventoryPanel").GetComponent<InventoryUiManager>();
        player.GetComponent<BuffsManager>().canvasParent = playerUi.transform.Find("PlayerStats").Find("BuffIconParents");
        player.GetComponent<SkillsManager>().iconParent = playerUi.transform.Find("SkillsIcons");
        player.GetComponent<SkillsManager>().inventory = playerUi.transform.Find("InventoryPanel").GetComponent<InventoryUiManager>();
        player.GetComponent<PlayerMovementController>().inventoryWindow = playerUi.transform.Find("InventoryPanel").gameObject;
        player.GetComponent<PlayerMovementController>().mainCameraTransform = camera.transform.Find("RotateAroundPlayer");
        player.GetComponent<ComboManager>().comboAnim = playerUi.transform.Find("PlayerStats").Find("ComboMeterParent").Find("ComboMeter").GetComponent<Animator>();
        player.GetComponent<RagdollManager>().cameraFollow = camera.GetComponent<FollowPlayer>();
        player.GetComponent<DamageNumberManager>().primaryCanvas = playerUi.transform;
        player.GetComponent<CameraShakeManager>().cameraToShake = camera.transform.Find("RotateAroundPlayer").Find("Main Camera");




        currentPlayers = GameObject.FindGameObjectsWithTag("Player");

        StartCoroutine(Initialization());
    }

    IEnumerator Initialization()
    {
        Debug.Log("Items should have been set to persistent between scenes, launch the first level");

        AsyncOperation levelOne = SceneManager.LoadSceneAsync(sceneNames[0]);
        Debug.Log("starting the level setup");

        //yield return new WaitForSeconds(3f);

        while (!levelOne.isDone)
        {
            yield return null;
            Debug.Log("Boop");
        }

        yield return new WaitForEndOfFrame();

        cameraFadeAnim.SetTrigger("FadeIn");
        cameraFadeAnim.transform.Find("AreaTitle").GetComponent<Text>().text = SceneManager.GetActiveScene().name;
        Debug.Log("We are setting up the level here");
        LevelSetup();
    }

    // Used to set up the level we are on
    public void LevelSetup()
    {
        Transform[] teleporterSpawns = GameObject.Find("TeleporterSpawns").GetComponentsInChildren<Transform>();
        Transform[] chestSpawns = GameObject.Find("ChestSpawns").GetComponentsInChildren<Transform>();

        // Spawn all the chests
        int chestCount = Random.Range(5 + currentPlayers.Length * 2, 10 + currentPlayers.Length * 4);
        for(int index = 0; index < chestCount; index++)
        {
            bool chestSuccessfullySpawned = false;

            while (!chestSuccessfullySpawned)
            {
                int chestSpawnIndex = Random.Range(0, chestSpawns.Length);

                if (chestSpawns[chestSpawnIndex] != null)
                {
                    Instantiate(chestPrefab, chestSpawns[chestSpawnIndex].position, chestSpawns[chestSpawnIndex].rotation);
                    chestSpawns[chestSpawnIndex] = null;
                    chestSuccessfullySpawned = true;
                }
            }

        }

        // Spawn the teleporter
        int teleporterIndex = Random.Range(0, teleporterSpawns.Length);
        Instantiate(teleporterPrefab, teleporterSpawns[teleporterIndex].position, teleporterSpawns[teleporterIndex].rotation);

        // Grab the player spawns 
        bool spawnedGrabbed = false;
        while(!spawnedGrabbed)
        {
            Debug.Log("checking spawns");
            Vector3 spawnSelected = teleporterSpawns[Random.Range(0, teleporterSpawns.Length)].position;

            if((spawnSelected - teleporterSpawns[teleporterIndex].position).sqrMagnitude >= MINIMUM_DISTANCE_FROM_TELEPORTER)
            {
                foreach(GameObject player in currentPlayers)
                {
                    Debug.Log("setting the players position");
                    Debug.Log("player position before: " + player.transform.position);
                    player.transform.position = spawnSelected + new Vector3(Random.Range(-2f, 2f), 0, Random.Range(-2f, 2f));
                    player.GetComponent<CameraShakeManager>().cameraToShake.root.GetComponent<FollowPlayer>().ResetCameraOrientation();
                    Debug.Log("player position after: " + player.transform.position);
                }
                Debug.Log("spawn found");
                spawnedGrabbed = true;
            }
        }
    }

    //The corotuine that runs on level start.
    /*
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
    */

    /*
    public void AddRoom(RoomManager room)
    {
        rooms.Add(room);
        currentRoomGenTimer = 0;
    }
    */

    /*
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
    */

    /*
    // Used to spawn in the current players into the room.
    private void SpawnPlayers()
    {
        foreach (GameObject player in currentPlayers)
        {
            player.transform.position = spawnsPlayer[Random.Range(0, spawnsPlayer.Length)].transform.position;
        }
    }
    */

    /*
    // Used to hide all rooms then show the ones that are adjacent to the new room
    public void ShowRoom(RoomManager targetRoom)
    {
        //Debug.Log("We are showing room: " + targetRoom.gameObject.name);

        targetRoom.ShowAdjacentRooms();

        foreach (RoomManager room in rooms)
            if(!targetRoom.connectedRooms.Contains(room) && room != targetRoom)
                room.HideRoom();
        
    }
    */

    public void LaunchPlayerTeleport()
    {
        StartCoroutine(StartTeleporting());
    }

    // Used when the player start teleporting.
    IEnumerator StartTeleporting()
    {
        Debug.Log("Starting the teleport logic");
        AsyncOperation sceneToLoad = SceneManager.LoadSceneAsync(sceneNames[0]);
        sceneToLoad.allowSceneActivation = false;

        // STart loading shit here
        foreach(GameObject player in currentPlayers)
        {
            player.GetComponent<BuffsManager>().psSystems[30].Play();
            player.GetComponent<BuffsManager>().psSystems[31].Play();
        }

        yield return new WaitForSeconds(5f);
        Debug.Log("teleport player here");

        foreach (GameObject player in currentPlayers)
        {
            Instantiate(player.GetComponent<SkillsManager>().skillProjectiles[83], player.transform.position + Vector3.up, Quaternion.identity);
            player.GetComponent<PlayerMovementController>().enabled = false;
            player.GetComponent<CharacterController>().enabled = false;
            player.transform.Find("EntityModel").gameObject.SetActive(false);

        }

        yield return new WaitForSeconds(1f);
        cameraFadeAnim.SetTrigger("FadeOut");
        yield return new WaitForSeconds(1f);

        Debug.Log("zoom to next level");
        sceneToLoad.allowSceneActivation = true;

        while(!sceneToLoad.isDone)
        {
            yield return null;
        }

        yield return new WaitForEndOfFrame();
        Debug.Log("We are setting up the level here");
        LevelSetup();
        cameraFadeAnim.SetTrigger("FadeIn");
        cameraFadeAnim.transform.Find("AreaTitle").GetComponent<Text>().text = SceneManager.GetActiveScene().name;

        foreach (GameObject player in currentPlayers)
        {
            Instantiate(player.GetComponent<SkillsManager>().skillProjectiles[83], player.transform.position + Vector3.up, Quaternion.identity);
            player.GetComponent<PlayerMovementController>().enabled = true;
            player.GetComponent<CharacterController>().enabled = true;
            player.GetComponent<BuffsManager>().StopAllParticles();
            player.transform.Find("EntityModel").gameObject.SetActive(true);

        }
    }
}


