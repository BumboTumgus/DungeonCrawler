using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrineBehaviour_Torment : ShrineBehaviour
{
    public override void OnInteract(GameObject player)
    {
        base.OnInteract(player);

        EnemyManager.instance.OnLevelUpEnemies();
        player.GetComponent<PlayerStats>().goldMultiplier += 0.25f;
        GetComponent<AudioSource>().Play();
    }
}
