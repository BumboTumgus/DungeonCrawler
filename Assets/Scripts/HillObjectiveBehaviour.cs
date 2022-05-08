using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HillObjectiveBehaviour : MonoBehaviour
{
    [SerializeField] GameObject dissapearParticles;
    [SerializeField] GameObject waypoint;
    GameObject waypointReference;

    private int currentPlayerCount = 0;
    private int maximumPlayerCount = 1;

    private float chargeRate = 0f;

    float currentValue = 0f;
    float targetValue = 60f;

    private bool waveSpawnedPrimary = false;
    private bool waveSpawnedSecondary = false;


    private void Start()
    {
        maximumPlayerCount = GameManager.instance.currentPlayers.Length;

        waypointReference = Instantiate(waypoint, new Vector3(9999, 9999, 9999), Quaternion.identity, GameManager.instance.playerUis[0].transform.Find("TemporaryUi"));
        waypointReference.GetComponent<UiFollowTarget>().target = transform.Find("HillWaypointTarget");
        waypointReference.transform.SetAsFirstSibling();
        targetValue = GameManager.instance.hillChargeTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (chargeRate > 0)
        {
            currentValue += chargeRate * Time.deltaTime;
            GameManager.instance.UpdateObjectiveCount(currentValue / targetValue * 100);

            if (!waveSpawnedSecondary && currentValue/ targetValue > 0.5f)
            {
                waveSpawnedSecondary = true;
                EnemyManager.instance.SpawnHillBatch();
            }
            //if (currentValue >= targetValue)
            //Destroy(gameObject);
        }

        //if (Input.GetKeyDown(KeyCode.Alpha9))
            //Destroy(gameObject);
    }

    /// <summary>
    /// Update our charge rate based on how many players are in the zone.
    /// </summary>
    private void UpdateCounterSpeed()
    {
        if ((float)currentPlayerCount / (float)maximumPlayerCount >= 0.5f)
            chargeRate = 1f;
        else if (currentPlayerCount == 0)
            chargeRate = 0;
        else
            chargeRate = 0.5f;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            currentPlayerCount++;
        if(!waveSpawnedPrimary)
        {
            waveSpawnedPrimary = true;
            EnemyManager.instance.SpawnHillBatch();
        }

        UpdateCounterSpeed();
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            currentPlayerCount--;

        UpdateCounterSpeed();
    }

    private void OnDestroy()
    {
        waypointReference.GetComponent<UiFollowTarget>().RemoveFromCullList();
        Destroy(waypointReference);
        Instantiate(dissapearParticles, transform.position, transform.rotation);
    }
}
