using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;

    [SerializeField] float currentTimer = 0f;
    [SerializeField] float targetTimer = 30f;
    [SerializeField] private int enemyLevel = 0;
    [SerializeField] private GameObject[] enemyBank;

    private GameManager gm;

    public List<PlayerStats> enemyStats = new List<PlayerStats>();


    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        gm = GetComponent<GameManager>();
    }

    private void PopulateDungeon()
    {
        float enemyCount = gm.rooms.Count * 3;
    }

    private void Update()
    {
        currentTimer += Time.deltaTime;
        if (currentTimer >= targetTimer)
        {
            currentTimer -= targetTimer;
            //Debug.Log("upgrading all the enemies stats");
            foreach(PlayerStats stats in enemyStats)
            {
                stats.LevelUpEnemy(enemyLevel);
            }
        }
    }
}
