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
    public List<GameObject> playerUis = new List<GameObject>();
    public List<GameObject> playerCameras = new List<GameObject>();
    public GameObject eventSystemReference;
    public Transform[] spawnsPlayer;
    public Transform[] spawnsChest;
    public int currentLevel = 0;
    public int currentLevelEnemyKillCount = 0;
    public float combinedPlayerLuck = 0;

    public float objectiveCurrentProgress;
    public float objectiveTarget;
    public enum ObjectiveType { GatherArtifacts, KillEnemies, KillSpecificEnemy, KillBoss, KingOfHill};
    public ObjectiveType objectiveType;

    //public Transform[] spawnsEnemy;
    //public NavMeshSurface walkableFloor;
    //public int enemyCount = 3;
    [SerializeField] GameObject[] chestPrefabs;
    public float[] chestRarityRC;
    [SerializeField] GameObject teleporterPrefab;
    [SerializeField] GameObject playerCharacterPrefab;
    [SerializeField] GameObject playerUiPrefab;
    [SerializeField] GameObject playerCameraPrefab;
    [SerializeField] GameObject eventSystemUI;
    [SerializeField] Animator cameraFadeAnim;
    [SerializeField] GameObject teleporter;
    [SerializeField] AudioFader levelMusic;
    [SerializeField] AudioFader deathMusic;
    [SerializeField] GameObject artifactObjective;

    //public int roomTarget = 20;

    //public float currentRoomGenTimer = 0;

    //private GameObject startingRoom;

    private const float MINIMUM_DISTANCE_FROM_TELEPORTER = 10000f;
    private const int targetLevelEnemyKillCount = 10;
    //private const float TAREGT_ROOM_GEN_TIMER = 0.5f;

    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        // Debug.Log("Game manager setup");
        if (instance == null)
            instance = this;

        if(SceneManager.GetActiveScene().name == "MainMenu")
            StartCoroutine(Initialization());
    }

    IEnumerator Initialization()
    {
        AsyncOperation levelOne = SceneManager.LoadSceneAsync(sceneNames[0]);

        while (!levelOne.isDone)
        {
            //Debug.Log(levelOne.progress);
            yield return null;
        }

        yield return new WaitForEndOfFrame();

        playerUis = new List<GameObject>();
        playerCameras = new List<GameObject>();

        //Debug.Log("creating EVERYTHING");
        // Here we create a player, their ui and a camera and connect everything.
        GameObject player = Instantiate(playerCharacterPrefab);
        GameObject camera = Instantiate(playerCameraPrefab);
        GameObject playerUi = Instantiate(playerUiPrefab);
        GameObject eventSystem = Instantiate(eventSystemUI);

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
        player.GetComponent<PlayerStats>().moneyCounter = playerUi.GetComponent<MoneyUiCounterBehaviour>();
        player.GetComponent<Inventory>().interactPrompt = playerUi.transform.Find("InteractPrompt").GetComponent<InteractPromptController>();
        player.GetComponent<Inventory>().hintPrompt = playerUi.transform.Find("HintBox").GetComponent<InteractPromptController>();
        player.GetComponent<Inventory>().inventoryUI = playerUi.transform.Find("InventoryPanel").GetComponent<InventoryUiManager>();
        player.GetComponent<BuffsManager>().canvasParent = playerUi.transform.Find("PlayerStats").Find("BuffIconParents");
        player.GetComponent<SkillsManager>().iconParent = playerUi.transform.Find("SkillsIcons");
        player.GetComponent<SkillsManager>().inventory = playerUi.transform.Find("InventoryPanel").GetComponent<InventoryUiManager>();
        player.GetComponent<PlayerMovementController>().inventoryWindow = playerUi.transform.Find("InventoryPanel").gameObject;
        player.GetComponent<PlayerMovementController>().mainCameraTransform = camera.transform.Find("RotateAroundPlayer");
        player.GetComponent<ComboManager>().comboAnim = playerUi.transform.Find("ComboMeterParent").Find("ComboMeter").GetComponent<Animator>();
        player.GetComponent<RagdollManager>().cameraFollow = camera.GetComponent<FollowPlayer>();
        player.GetComponent<DamageNumberManager>().primaryCanvas = playerUi.transform.Find("TemporaryUi");
        player.GetComponent<CameraShakeManager>().cameraToShake = camera.transform.Find("RotateAroundPlayer").Find("Main Camera");

        currentPlayers = GameObject.FindGameObjectsWithTag("Player");
        playerUis.Add(playerUi);
        playerCameras.Add(camera);
        eventSystemReference = eventSystem;



        cameraFadeAnim.gameObject.SetActive(true);
        cameraFadeAnim.SetTrigger("FadeIn");
        cameraFadeAnim.transform.Find("AreaTitle").GetComponent<Text>().text = SceneManager.GetActiveScene().name;
        Cursor.visible = false;

        deathMusic = GetComponent<AudioFader>();
        LevelSetup();
    }

    // USed when we kill a target, increment the count and then set our teleporter as active if we are above the target
    public void IncrementLevelEnemyDeathCount()
    {
        currentLevelEnemyKillCount++;
        if(currentLevelEnemyKillCount == targetLevelEnemyKillCount)
        {
            StartTeleporter();
        }
    }

    private void StartTeleporter()
    {
        // Set up our teleporter here to work.
        teleporter.GetComponent<TeleporterBehaviour>().StartTeleporter();
        foreach (GameObject player in currentPlayers)
        {
            player.GetComponent<AudioManager>().PlayAudio(30);
        }
    }

    public void UpdateObjectiveCount(float newValue)
    {
        objectiveCurrentProgress = newValue;

        if (objectiveCurrentProgress >= objectiveTarget)
        {
            StartTeleporter();
            foreach (GameObject ui in playerUis)
                ui.GetComponent<ObjectivePanelController>().SetupObjectivePanel("Objective Complete:", $"Get to the teleporter to proceed.");
        }
        else
            foreach (GameObject ui in playerUis)
            {
                switch (objectiveType)
                {
                    case ObjectiveType.GatherArtifacts:
                        ui.GetComponent<ObjectivePanelController>().UpdateObjectiveDecription($"Gather the artifacts:\nTotal Gathered: {objectiveCurrentProgress} / {objectiveTarget}");
                        break;
                    case ObjectiveType.KillEnemies:
                        ui.GetComponent<ObjectivePanelController>().UpdateObjectiveDecription($"Kill any enemies:\nEnemies Vaporized: {objectiveCurrentProgress} / {objectiveTarget}");
                        break;
                    case ObjectiveType.KillSpecificEnemy:
                        ui.GetComponent<ObjectivePanelController>().UpdateObjectiveDecription($"Kill specific enemies:\nSnakes Slayed: {objectiveCurrentProgress} / {objectiveTarget}");
                        break;
                    case ObjectiveType.KillBoss:
                        ui.GetComponent<ObjectivePanelController>().UpdateObjectiveDecription($"Slay the boss:\nBosses Killed: {objectiveCurrentProgress} / {objectiveTarget}");
                        break;
                    case ObjectiveType.KingOfHill:
                        ui.GetComponent<ObjectivePanelController>().UpdateObjectiveDecription($"Hold the point:\nPoint Progress: {objectiveCurrentProgress} / {objectiveTarget}");
                        break;
                    default:
                        break;
                }
            }
    }

    // Used to set up the level we are on
    public void LevelSetup()
    {
        currentLevel++;

        currentLevelEnemyKillCount = 0;

        if(currentLevel == 1)
            Instantiate(ItemGenerator.instance.RollItem(Item.ItemType.Skill, Item.ItemRarity.Common), GameObject.Find("GarenteedSkillSpawn").transform.position, Quaternion.identity);

        Transform[] teleporterSpawns = GameObject.Find("TeleporterSpawns").GetComponentsInChildren<Transform>();
        Transform[] chestSpawns = GameObject.Find("ChestSpawns").GetComponentsInChildren<Transform>();

        chestRarityRC = ItemGenerator.instance.ReturnRarityRollRCs();

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
                    GameObject chestToSpawn = null;
                    float chestRarityRoll = Random.Range(0, 100);
                    if (chestRarityRoll <= chestRarityRC[0] && chestRarityRC[0] != 0)
                        chestToSpawn = chestPrefabs[0];
                    else if (chestRarityRoll <= chestRarityRC[0] + chestRarityRC[1] && chestRarityRC[1] != 0)
                        chestToSpawn = chestPrefabs[1];
                    else if (chestRarityRoll <= chestRarityRC[0] + chestRarityRC[1] + chestRarityRC[2] && chestRarityRC[2] != 0)
                        chestToSpawn = chestPrefabs[2];
                    else if (chestRarityRoll <= chestRarityRC[0] + chestRarityRC[1] + chestRarityRC[2] + chestRarityRC[3] && chestRarityRC[3] != 0)
                    {
                        if (Random.Range(0, 100) <= 50)
                            chestToSpawn = chestPrefabs[3];
                        else
                            chestToSpawn = chestPrefabs[4];
                    }
                    else if (chestRarityRoll <= chestRarityRC[0] + chestRarityRC[1] + chestRarityRC[2] + chestRarityRC[3] + chestRarityRC[4] && chestRarityRC[4] != 0)
                        chestToSpawn = chestPrefabs[5];



                    GameObject chest = Instantiate(chestToSpawn, chestSpawns[chestSpawnIndex].position, chestSpawns[chestSpawnIndex].rotation);
                    chest.transform.Find("MoneyCostCanvas").GetComponent<FaceNearestPlayerBehaviour>().playerToFace = currentPlayers[0].transform;
                    chestSpawns[chestSpawnIndex] = null;
                    
                    chestSuccessfullySpawned = true;
                }
            }

            // SHould we add standard bosses to the spawn list?
            if (currentLevel == 5)
                EnemyManager.instance.AddEligibleEnemiesToSpawn();

        }

        // Spawn the teleporter
        int teleporterIndex = Random.Range(0, teleporterSpawns.Length);
        teleporter = Instantiate(teleporterPrefab, teleporterSpawns[teleporterIndex].position, teleporterSpawns[teleporterIndex].rotation);

        // Grab the player spawns 
        bool spawnedGrabbed = false;
        while(!spawnedGrabbed)
        {
            //Debug.Log("checking spawns");
            Vector3 spawnSelected = teleporterSpawns[Random.Range(0, teleporterSpawns.Length)].position;

            if((spawnSelected - teleporterSpawns[teleporterIndex].position).sqrMagnitude >= MINIMUM_DISTANCE_FROM_TELEPORTER)
            {
                foreach(GameObject player in currentPlayers)
                {
                    //Debug.Log("setting the players position");
                    //Debug.Log("player position before: " + player.transform.position);
                    player.transform.position = spawnSelected + new Vector3(Random.Range(-2f, 2f), 0, Random.Range(-2f, 2f));
                    player.GetComponent<CameraShakeManager>().cameraToShake.root.GetComponent<FollowPlayer>().ResetCameraOrientation();
                    //Debug.Log("player position after: " + player.transform.position);
                }
                //Debug.Log("spawn found");
                spawnedGrabbed = true;
            }
        }

        // Sets up the face player components of the cnavas for the garenteed chests.
        ChestBehaviour[] garenteedChests = GameObject.Find("GarenteedChestSpawns").GetComponentsInChildren<ChestBehaviour>();
        foreach(ChestBehaviour chest in garenteedChests)
        {
            chest.transform.Find("MoneyCostCanvas").GetComponent<FaceNearestPlayerBehaviour>().playerToFace = currentPlayers[0].transform;
        }

        EnemyManager.instance.LevelSetup();

        levelMusic = GameObject.Find("Audio_LevelTheme").GetComponent<AudioFader>();

        switch (Random.Range(0,1))
        {
            case 0:
                objectiveType = ObjectiveType.GatherArtifacts;
                Debug.Log("The objective for this level is collection, GATHER THE ARTIFACTS");
                objectiveCurrentProgress = 0;
                objectiveTarget = 8;

                foreach(GameObject ui in playerUis)
                    ui.GetComponent<ObjectivePanelController>().SetupObjectivePanel("New Objective:", $"Gather the artifacts:\nTotal Gathered: {objectiveCurrentProgress} / {objectiveTarget}");

                Transform[] artifactSpawns = GameObject.Find("ArtifactSpawns").GetComponentsInChildren<Transform>();

                // Spawn the artifacts.
                for(int index = 0; index < objectiveTarget; index++)
                {
                    int artifactSpawnIndex = Random.Range(0, artifactSpawns.Length);

                    if (artifactSpawns[artifactSpawnIndex] != null)
                    {
                        Instantiate(artifactObjective, artifactSpawns[artifactSpawnIndex].position, artifactSpawns[artifactSpawnIndex].rotation);
                        artifactSpawns[artifactSpawnIndex] = null;
                    }
                }

                break;
            case 1:
                objectiveType = ObjectiveType.KillEnemies;
                Debug.Log("The objective for this level is killing any enemy, KILL THEM MFS");
                objectiveCurrentProgress = 0;
                objectiveTarget = 25;

                foreach (GameObject ui in playerUis)
                    ui.GetComponent<ObjectivePanelController>().SetupObjectivePanel("New Objective:", $"Kill any enemies:\nEnemies Vaporized: {objectiveCurrentProgress} / {objectiveTarget}");
                break;
            case 2:
                objectiveType = ObjectiveType.KillSpecificEnemy;
                Debug.Log("The objective for this level is killing a specific type of enemy... HUNT THEM DOWN");
                objectiveCurrentProgress = 0;
                objectiveTarget = 5;

                foreach (GameObject ui in playerUis)
                    ui.GetComponent<ObjectivePanelController>().SetupObjectivePanel("New Objective:", $"Kill specific enemies:\nSnakes Slayed: {objectiveCurrentProgress} / {objectiveTarget}");
                break;
            case 3:
                objectiveType = ObjectiveType.KillBoss;
                Debug.Log("The objective for this level is killing a boss... FIND HIM NEAR THE PORTAL");
                objectiveCurrentProgress = 0;
                objectiveTarget = 1;

                foreach (GameObject ui in playerUis)
                    ui.GetComponent<ObjectivePanelController>().SetupObjectivePanel("New Objective:", $"Slay the boss:\nBosses Killed: {objectiveCurrentProgress} / {objectiveTarget}");
                break;
            case 4:
                objectiveType = ObjectiveType.KingOfHill;
                Debug.Log("The objective for this level is Holding a point... KING OF THE HILL");
                objectiveCurrentProgress = 0;
                objectiveTarget = 60;

                foreach (GameObject ui in playerUis)
                    ui.GetComponent<ObjectivePanelController>().SetupObjectivePanel("New Objective:", $"Hold the point:\nPoint Progress: {objectiveCurrentProgress} / {objectiveTarget}");
                break;
            default:
                break;
        }
    }

    public void LaunchPlayerTeleport()
    {
        StartCoroutine(StartTeleporting());
    }

    // Used when the player start teleporting.
    IEnumerator StartTeleporting()
    {
        //Debug.Log("Starting the teleport logic");
        AsyncOperation sceneToLoad = SceneManager.LoadSceneAsync(sceneNames[0]);
        sceneToLoad.allowSceneActivation = false;
        EnemyManager.instance.allowEnemySpawns = false;

        // STart loading shit here
        foreach(GameObject player in currentPlayers)
        {
            player.GetComponent<PlayerStats>().AddExp(player.GetComponent<PlayerStats>().gold);
            player.GetComponent<PlayerStats>().AddGold(player.GetComponent<PlayerStats>().gold * -1);
            player.GetComponent<BuffsManager>().psSystems[30].Play();
            player.GetComponent<BuffsManager>().psSystems[31].Play();
            player.GetComponent<AudioManager>().PlayAudio(31);
        }

        yield return new WaitForSeconds(5f);
        //Debug.Log("teleport player here");

        foreach (GameObject player in currentPlayers)
        {
            Instantiate(player.GetComponent<SkillsManager>().skillProjectiles[83], player.transform.position + Vector3.up, Quaternion.identity);
            player.GetComponent<PlayerMovementController>().enabled = false;
            player.GetComponent<CharacterController>().enabled = false;
            player.GetComponent<BuffsManager>().RemoveAllBuffs();
            player.transform.Find("EntityModel").gameObject.SetActive(false);

        }

        yield return new WaitForSeconds(1f);
        levelMusic.FadeOut(2f);
        cameraFadeAnim.SetTrigger("FadeOut");
        yield return new WaitForSeconds(1f);

        // Remove any of the ui cull behind camera list items from each camera
        foreach(GameObject camera in playerCameras)
        {
            camera.GetComponentInChildren<UiHideBehindPlayer>().targets = new List<UiFollowTarget>();
        }

        // REmove all the old uis from the temporary ui tab like damage numbers and health bars
        foreach(GameObject ui in playerUis)
        {
            //Debug.Log("The ui should be wiped here");
            Transform parentToWipe = ui.transform.Find("TemporaryUi");
            //Debug.Log(parentToWipe);
            //.Log(parentToWipe.childCount);

            foreach(Transform child in parentToWipe)
            {
                //Debug.Log(child.name);
                Destroy(child.gameObject);
            }

        }

        //Debug.Log("zoom to next level");
        sceneToLoad.allowSceneActivation = true;

        while(!sceneToLoad.isDone)
        {
            yield return null;
        }

        yield return new WaitForEndOfFrame();
        //Debug.Log("We are setting up the level here");
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

    // USed to destroy the game manager, the players,  their uis, and basically all the things we initally added in before transitioning scenes
    public void PreMenuSceneCleanup()
    {
        for(int index =0; index < currentPlayers.Length; index++)
            Destroy(currentPlayers[index]);
        for (int index = 0; index < playerUis.Count; index++)
            Destroy(playerUis[index], 0.05f);
        for (int index = 0; index < playerCameras.Count; index++)
            Destroy(playerCameras[index]);
        Destroy(eventSystemReference);
        Destroy(gameObject);
    }

    // Used when a player dies. Check to see if all the players are dead. If they are, fade to black to the end game screen.
    public void PlayerDeath()
    {
        //Debug.Log("a player died");
        bool allPlayersDead = true;
        foreach(GameObject player in currentPlayers)
        {
            if (!player.GetComponent<PlayerStats>().dead)
            {
                allPlayersDead = false;
                break;
            }
        }
        //Debug.Log("are all the players dead? " + allPlayersDead);

        // If all the players are dead, call a function for each of them that fades to black and end the game.
        if(allPlayersDead)
        {

            levelMusic.FadeOut(0.5f);
            GetComponent<AudioSource>().Play();
            deathMusic.FadeOut(15f);
            foreach(GameObject playerUI in playerUis)
            {
                //Debug.Log(playerUI);
                playerUI.GetComponent<GameOverMenuBehaviour>().GameOverScreenFadeIn();
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
                Time.timeScale = 0;
            }
        }
    }

    // USed when a player luck value changes, add it to the total when we spawn loot
    public void GrabPlayerLuckValues()
    {
        combinedPlayerLuck = 0;

        foreach (GameObject player in currentPlayers)
            combinedPlayerLuck += player.GetComponent<PlayerStats>().luck;
    }
}


