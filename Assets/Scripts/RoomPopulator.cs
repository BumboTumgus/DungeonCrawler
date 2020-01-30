using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomPopulator : MonoBehaviour
{
    private Transform[] spawnsChest;
    private Transform[] spawnsEnemy;
    private FloorManager floorManager;
    [SerializeField] private int enemyCount;
    [SerializeField] private int treasureCount;
    private bool treasureInRoom = false;
    [SerializeField] private int enemyCountMin = 1;
    [SerializeField] private int enemyCountMax = 6;
    [SerializeField] private int treasureCountMin = 1;
    [SerializeField] private int treasureCountMax = 1;

    private void Start()
    {
        floorManager = GameObject.Find("FloorManager").GetComponent<FloorManager>();
        enemyCount = Random.Range(enemyCountMin, enemyCountMax + 1);
        treasureCount = Random.Range(treasureCountMin, treasureCountMax + 1);

        // Here we do a check to see if there should be treasure in the room.
        if (Random.Range(0, 101) >= 75)
            treasureInRoom = true;
    }

    // Used to fill the room with enemies and treasure and more.
    public void PopulateRoom()
    {
        SetupSpawns();

        // Spawn Enemies until we are out of enemyCount points.
        while(enemyCount > 0)
        {
            bool suitableEnemy = false;
            while (!suitableEnemy)
            {
                GameObject randomEnemy = floorManager.enemies[Random.Range(0, floorManager.enemies.Length)];

                // Check to see if the we have a suitable enemy, if we do spawn it if not pick another.
                if(randomEnemy.GetComponent<PlayerStats>().enemyCost <= enemyCount)
                {
                    suitableEnemy = true;
                    Instantiate(randomEnemy, spawnsEnemy[Random.Range(0,
                        spawnsEnemy.Length)].position +
                        new Vector3(Random.Range(-0.3f, 0.3f), 0, Random.Range(-0.3f, 0.3f)),
                        transform.rotation);
                    enemyCount -= randomEnemy.GetComponent<PlayerStats>().enemyCost;
                }
            }
        }

        // Spawn the chests.
        if(treasureInRoom)
        {
            while(treasureCount > 0)
            {
                GameObject randomTreasure = floorManager.treasures[Random.Range(0, floorManager.treasures.Length)];

                Instantiate(randomTreasure,
                    spawnsChest[Random.Range(0, spawnsChest.Length)].position +
                    new Vector3(Random.Range(-0.3f, 0.3f), 0, Random.Range(-0.3f, 0.3f)),
                    transform.rotation);
                treasureCount--; 
            }
        }
    }

    // Used to grab our spawn points and set them up.
    private void SetupSpawns()
    {
        // Sets up the array of chest spawns.
        Transform chestSpawnParent = transform.Find("Spawns_Chests");
        if (chestSpawnParent)
        {
            spawnsChest = new Transform[chestSpawnParent.childCount];
            for (int index = 0; index < chestSpawnParent.childCount; index++)
            {
                spawnsChest[index] = chestSpawnParent.GetChild(index);
            }
        }

        // Sets up the array of enemy spawns.
        Transform enemySpawnParent = transform.Find("Spawns_Enemies");
        if (enemySpawnParent != null)
        {
            spawnsEnemy = new Transform[enemySpawnParent.childCount];
            for (int index = 0; index < enemySpawnParent.childCount; index++)
            {
                spawnsEnemy[index] = enemySpawnParent.GetChild(index);
            }
        }
    }
}
