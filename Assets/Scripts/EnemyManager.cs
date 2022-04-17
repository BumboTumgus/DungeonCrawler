using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    public List<PlayerStats> enemyStats = new List<PlayerStats>();
    public bool allowEnemySpawns = true;
    public bool canRollBosses = false;

    [SerializeField] Transform[] enemySpawns;
    [SerializeField] float enemyPoints = 0;
    [SerializeField] float enemyPointsBonusMultiplier = 1;
    [SerializeField] float currentTimer = 0f;
    [SerializeField] float targetTimer = 120;
    [SerializeField] private int enemyLevel = 0;
    [SerializeField] private float enemyEliteChance = 0.05f;
    [SerializeField] private float bossSpawnChance = 0;

    [SerializeField] private GameObject[] enemyBank;
    [SerializeField] private GameObject[] enemyBossBank;
    [SerializeField] private GameObject spawnEffectSmall;

    [SerializeField] private GameObject[] enemyBankBeeElites;
    [SerializeField] private GameObject[] enemyBankBruteElites;
    [SerializeField] private GameObject[] enemyBankCobraElites;
    [SerializeField] private GameObject[] enemyBankDragonElites;
    [SerializeField] private GameObject[] enemyBankForgeGiantElites;
    [SerializeField] private GameObject[] enemyBankGoblinElites;
    [SerializeField] private GameObject[] enemyBankGolemElites;
    [SerializeField] private GameObject[] enemyBankWolfElites;

    Coroutine enemySpawnRoutine;

    const float MAXIMUM_DISTANCE_FROM_PLAYER = 200;
    const float MAXIMUM_ATTEMPTED_SPAWNS = 10;
    const float BONUS_POINT_MULTIPLIER_GROWTH = 1.3f;


    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }


    private void Update()
    {
        currentTimer += Time.deltaTime;
        enemyPoints += Time.deltaTime * enemyPointsBonusMultiplier;

        if (currentTimer >= targetTimer)
        {
            currentTimer -= targetTimer;
            enemyLevel++;
            Debug.Log("------------------- Enemy Level Up -------------------");
            //Debug.Log("upgrading all the enemies stats");
            foreach (PlayerStats stats in enemyStats)
            {
                stats.level = enemyLevel;
                stats.LevelEnemyCalculation();
            }
            enemyEliteChance += 0.025f;
            bossSpawnChance += 0.01f;
            enemyPointsBonusMultiplier *= BONUS_POINT_MULTIPLIER_GROWTH;

            GameManager.instance.trapStats.level = enemyLevel;
            GameManager.instance.trapStats.StatSetup(true, true);
            GameManager.instance.SetTrapDamage();
        }

        /*
        if (Input.GetKeyDown(KeyCode.Semicolon))
        {
            GameObject enemy = Instantiate(enemyBank[0], new Vector3(Random.Range(-15, 15), 0, Random.Range(25, 30)), Quaternion.identity);
        }
        */
    }

    public void StartEnemyBatchSpawnCoroutine()
    {
        // if we have a routine, dont do anything.
        if (enemySpawnRoutine != null)
            StopCoroutine(enemySpawnRoutine);
            
        enemySpawnRoutine = StartCoroutine(SpawnEnemyBatch());
    }

    public void WipeAllEnemyAudioSources()
    {
        foreach (PlayerStats enemy in enemyStats)
            enemy.GetComponent<AudioManager>().WipeAllAudioSources();
    }

    IEnumerator SpawnEnemyBatch()
    {
        //Debug.Log("SpawnBAtch coroutine started");
        while (allowEnemySpawns)
        {
            // create the enemy batch we want to spawn.
            List<GameObject> enemyBatch = new List<GameObject>();
            //int enemyBatchEnemyCount = Random.Range(3, 7);
            int enemyBatchEnemyCount = 1;
            float batchCost = 0;
            float batchSpawnDelay = Random.Range(3, 10);

            for (int index = 0; index < enemyBatchEnemyCount; index++)
            {
                GameObject enemyToAddToBatch = enemyBank[Random.Range(0, enemyBank.Length)];

                if (canRollBosses && Random.Range(0f, 1f) < bossSpawnChance)
                    enemyToAddToBatch = enemyBossBank[Random.Range(0, enemyBossBank.Length)];

                //Check to see if we rolled an elite, if not continue on.
                if(Random.Range(0f,1f) < enemyEliteChance)
                {
                    switch (enemyToAddToBatch.GetComponent<PlayerStats>().entityType)
                    {
                        case PlayerStats.EnemyEntityType.Goblin:
                            enemyToAddToBatch = enemyBankGoblinElites[Random.Range(0, enemyBankGoblinElites.Length)];
                            break;
                        case PlayerStats.EnemyEntityType.Bee:
                            enemyToAddToBatch = enemyBankBeeElites[Random.Range(0, enemyBankBeeElites.Length)];
                            break;
                        case PlayerStats.EnemyEntityType.Snake:
                            enemyToAddToBatch = enemyBankCobraElites[Random.Range(0, enemyBankCobraElites.Length)];
                            break;
                        case PlayerStats.EnemyEntityType.Wolf:
                            enemyToAddToBatch = enemyBankWolfElites[Random.Range(0, enemyBankWolfElites.Length)];
                            break;
                        case PlayerStats.EnemyEntityType.Brute:
                            enemyToAddToBatch = enemyBankBruteElites[Random.Range(0, enemyBankBruteElites.Length)];
                            break;
                        case PlayerStats.EnemyEntityType.Dragon:
                            enemyToAddToBatch = enemyBankDragonElites[Random.Range(0, enemyBankDragonElites.Length)];
                            break;
                        case PlayerStats.EnemyEntityType.Golem:
                            enemyToAddToBatch = enemyBankGolemElites[Random.Range(0, enemyBankGolemElites.Length)];
                            break;
                        case PlayerStats.EnemyEntityType.ForgeGiant:
                            enemyToAddToBatch = enemyBankForgeGiantElites[Random.Range(0, enemyBankForgeGiantElites.Length)];
                            break;
                        default:
                            break;
                    }
                }

                enemyBatch.Add(enemyToAddToBatch);
                batchCost += enemyToAddToBatch.GetComponent<PlayerStats>().enemyBatchCost;
            }
            //Debug.Log("Batch creaed oif cost: " + batchCost);

            // We now have a batch of enemies with a total cost. We will wait until the cost has been reached and spawn them in.
            yield return new WaitForSeconds(batchSpawnDelay);
            while (enemyPoints < batchCost)
            {
                yield return new WaitForEndOfFrame();
            }

            //Debug.Log("Batch being spawned ");
            // When we have the budget to spawn these enemies, figure out if this batch will be true random around the map or in a bunch near the player
            bool spawnNearPlayer = false;
            if (Random.Range(0, 100) >= 50)
                spawnNearPlayer = true;

            foreach (GameObject enemy in enemyBatch)
            {
                // grab a random spawn, if its suitable use it if not pick another
                Vector3 spawnPoint = Vector3.zero;
                bool suitableSpawn = false;
                int attemptedSpawns = 0;

                while (!suitableSpawn)
                {
                    //Debug.Log("we will find a spawn here");
                    attemptedSpawns++;
                    if (!spawnNearPlayer)
                    {
                        //Debug.Log("we are not spawning near the player");
                        spawnPoint = enemySpawns[Random.Range(0, enemySpawns.Length)].position;
                        suitableSpawn = true;
                    }
                    else
                    {
                        //Debug.Log("we are spawning near the player");
                        spawnPoint = enemySpawns[Random.Range(0, enemySpawns.Length)].position;
                        if ((spawnPoint - GameManager.instance.currentPlayers[0].transform.position).sqrMagnitude <= MAXIMUM_DISTANCE_FROM_PLAYER)
                            suitableSpawn = true;
                        if (attemptedSpawns >= MAXIMUM_ATTEMPTED_SPAWNS)
                            suitableSpawn = true;

                    }
                }
                //Debug.Log("Created an enemy:  " + enemy);
                GameObject enemyGO = Instantiate(enemy, spawnPoint, Quaternion.identity);
                enemyGO.GetComponent<DamageNumberManager>().primaryCanvas = GameManager.instance.playerUis[0].transform;
                enemyGO.GetComponent<PlayerStats>().level = enemyLevel;
                Instantiate(spawnEffectSmall, spawnPoint, Quaternion.identity);
            }
            //Debug.Log("Chilling before we loop back to the top or end");

            yield return new WaitForSeconds(1);
        }
    }

    public void SpawnTargetedBoss()
    {
        Debug.Log("The targeted boss has been spawned now");
        GameObject selectedBossEnemy = enemyBossBank[Random.Range(0, enemyBossBank.Length)];

        Vector3 spawnPoint = enemySpawns[Random.Range(0, enemySpawns.Length)].position;

        //Debug.Log("Created an enemy:  " + enemy);
        GameObject enemyGO = Instantiate(selectedBossEnemy, spawnPoint, Quaternion.identity);
        enemyGO.GetComponent<DamageNumberManager>().primaryCanvas = GameManager.instance.playerUis[0].transform;
        enemyGO.GetComponent<PlayerStats>().level = enemyLevel;
        enemyGO.GetComponent<PlayerStats>().entityType = PlayerStats.EnemyEntityType.TargetBoss;
        Instantiate(spawnEffectSmall, spawnPoint, Quaternion.identity);
    }

    public void LevelSetup()
    {
        //Debug.Log("Enemy manager setup compelte");
        enemySpawns = GameObject.Find("EnemySpawns").GetComponentsInChildren<Transform>();
        allowEnemySpawns = true;
        enemyPoints = 5;
        enemyStats = new List<PlayerStats>();
        StartEnemyBatchSpawnCoroutine();
    }

    public void SpawnGoblin()
    {
        GameObject enemyGO = Instantiate(enemyBank[0], new Vector3(0,0,45), Quaternion.identity);
        enemyGO.GetComponent<DamageNumberManager>().primaryCanvas = GameManager.instance.playerUis[0].transform;
        enemyGO.GetComponent<PlayerStats>().level = enemyLevel;
        Instantiate(spawnEffectSmall, new Vector3(0, 0, 45), Quaternion.identity);
    }
}
