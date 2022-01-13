using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    public List<PlayerStats> enemyStats = new List<PlayerStats>();
    public bool allowEnemySpawns = true;

    [SerializeField] Transform[] enemySpawns;
    [SerializeField] float enemyPoints = 0;
    [SerializeField] float currentTimer = 0f;
    [SerializeField] float targetTimer = 80f;
    [SerializeField] private int enemyLevel = 0;

    [SerializeField] private GameObject[] enemyBank;
    [SerializeField] private GameObject[] enemyBossBank;
    [SerializeField] private List<GameObject> spawnableEnemies = new List<GameObject>();
    [SerializeField] private GameObject spawnEffectSmall;

    Coroutine enemySpawnRoutine;

    const float MAXIMUM_DISTANCE_FROM_PLAYER = 200;
    const float MAXIMUM_ATTEMPTED_SPAWNS = 10;


    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        foreach (GameObject enemy in enemyBank)
            spawnableEnemies.Add(enemy);
    }

    public void AddEligibleEnemiesToSpawn()
    {
        foreach (GameObject boss in enemyBossBank)
            spawnableEnemies.Add(boss);
    }

    private void Update()
    {
        currentTimer += Time.deltaTime;
        enemyPoints += Time.deltaTime;

        if (currentTimer >= targetTimer)
        {
            currentTimer -= targetTimer;
            enemyLevel++;
            Debug.Log("------------------- Enemy Level Up -------------------");
            //Debug.Log("upgrading all the enemies stats");
            foreach (PlayerStats stats in enemyStats)
            {
                stats.level = enemyLevel;
                stats.StatSetup(true, true);
            }
        }

        if (Input.GetKeyDown(KeyCode.Semicolon))
        {
            GameObject enemy = Instantiate(enemyBank[0], new Vector3(Random.Range(-15, 15), 0, Random.Range(25, 30)), Quaternion.identity);
        }
        /*
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
                GameObject enemyToAddToBatch = spawnableEnemies[Random.Range(0, spawnableEnemies.Count)];
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
}
