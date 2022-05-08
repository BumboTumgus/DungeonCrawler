using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrineBehaviour_UnstableCrystal : ShrineBehaviour
{
    [SerializeField] GameObject gameobjectOnFail;
    [SerializeField] GameObject gameobjectOnSuccess;
    private const float PERCENT_SUCCESS_CHANCE = 50f;
    private const float LUCK_GAIN = 5f;

    public override void OnInteract(GameObject player)
    {
        base.OnInteract(player);

        if(Random.Range(0f,100f) < PERCENT_SUCCESS_CHANCE + GameManager.instance.combinedPlayerLuck)
        {
            // Success
            player.GetComponent<PlayerStats>().luck += LUCK_GAIN;
            player.GetComponent<PlayerStats>().StatSetup(false, false);
            player.GetComponent<Inventory>().ShowHint("You feel luckier...");

            Instantiate(gameobjectOnSuccess, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        else
        {
            // Failure
            EnemyManager.instance.OnLevelUpEnemies();

            Instantiate(gameobjectOnFail, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
