using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrineBehaviour_Obsidian : ShrineBehaviour
{
    public override void OnInteract(GameObject player)
    {
        base.OnInteract(player);

        player.GetComponent<BuffsManager>().NewBuff(BuffsManager.BuffType.Curse, 0, null);
        player.GetComponent<PlayerStats>().AddGold(GameManager.instance.shrineCost * 20);
        GetComponent<AudioSource>().Play();
    }
}
