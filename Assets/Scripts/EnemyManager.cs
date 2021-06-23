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
    [SerializeField] float targetTimer = 120f;
    [SerializeField] private int enemyLevel = 0;

    [SerializeField] private GameObject[] enemyBank;

    Coroutine enemySpawnRoutine;

    const float MAXIMUM_DISTANCE_FROM_PLAYER = 200;
    const float MAXIMUM_ATTEMPTED_SPAWNS = 50;



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
        enemyPoints += Time.deltaTime;

        if (currentTimer >= targetTimer)
        {
            currentTimer -= targetTimer;
            //Debug.Log("upgrading all the enemies stats");
            foreach (PlayerStats stats in enemyStats)
            {
                stats.LevelUpEnemy(enemyLevel);
            }
        }

        if (Input.GetKeyDown(KeyCode.Semicolon))
        {
            GameObject enemy = Instantiate(enemyBank[0], new Vector3(Random.Range(-15, 15), 0, Random.Range(25,30)), Quaternion.identity);
        }
    }

    public void StartEnemyBatchSpawnCoroutine()
    {
        // if we have a routine, dont do anything.
        if(enemySpawnRoutine == null)
            enemySpawnRoutine = StartCoroutine(SpawnEnemyBatch());
    }

    IEnumerator SpawnEnemyBatch()
    {
        Debug.Log("SpawnBAtch coroutine started");
        while (allowEnemySpawns)
        {
            // create the enemy batch we want to spawn.
            List<GameObject> enemyBatch = new List<GameObject>();
            int enemyBatchEnemyCount = Random.Range(3, 7);
            float batchCost = 0;
            float batchSpawnDelay = Random.Range(3, 10);

            for (int index = 0; index < enemyBatchEnemyCount; index++)
            {
                GameObject enemyToAddToBatch = enemyBank[Random.Range(0, enemyBank.Length)];
                enemyBatch.Add(enemyToAddToBatch);
                batchCost += enemyToAddToBatch.GetComponent<PlayerStats>().enemyBatchCost;
            }
            Debug.Log("Batch creaed oif cost: " + batchCost);

            // We now have a batch of enemies with a total cost. We will wait until the cost has been reached and spawn them in.
            yield return new WaitForSeconds(batchSpawnDelay);
            while (enemyPoints < batchCost)
            {
                yield return new WaitForEndOfFrame();
            }

            Debug.Log("Batchbeing spawned ");
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
                    Debug.Log("we will find a spawn here");
                    attemptedSpawns++;
                    if (!spawnNearPlayer)
                    {
                        Debug.Log("we are not spawning near the player");
                        spawnPoint = enemySpawns[Random.Range(0, enemySpawns.Length)].position;
                        suitableSpawn = true;
                    }
                    else
                    {
                        Debug.Log("we are spawning near the player");
                        spawnPoint = enemySpawns[Random.Range(0, enemySpawns.Length)].position;
                        if ((spawnPoint - GameManager.instance.currentPlayers[0].transform.position).sqrMagnitude <= MAXIMUM_DISTANCE_FROM_PLAYER)
                            suitableSpawn = true;
                        if (attemptedSpawns >= MAXIMUM_ATTEMPTED_SPAWNS)
                            suitableSpawn = true;

                    }
                }
                Debug.Log("Created an enemy ");
                GameObject enemyGO = Instantiate(enemy, spawnPoint, Quaternion.identity);
                enemyGO.GetComponent<DamageNumberManager>().primaryCanvas = GameManager.instance.playerUis[0].transform;
            }
            Debug.Log("Chilling before we loop back to the top or end");

            yield return new WaitForSeconds(1);
        }
    }

    public void LevelSetup()
    {
        Debug.Log("Enemy manager setup compelte");
        enemySpawns = GameObject.Find("EnemySpawns").GetComponentsInChildren<Transform>();
        allowEnemySpawns = true;
        enemyPoints = 30;
        StartEnemyBatchSpawnCoroutine();
    }
}
