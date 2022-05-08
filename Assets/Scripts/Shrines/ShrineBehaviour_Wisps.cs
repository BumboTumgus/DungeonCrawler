using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrineBehaviour_Wisps : ShrineBehaviour
{
    public override void OnInteract(GameObject player)
    {
        base.OnInteract(player);
        GetComponent<AudioSource>().Play();

        switch (Random.Range(0, 4))
        {
            case 0:
                player.GetComponent<BuffsManager>().NewBuff(BuffsManager.BuffType.WispsDamage, 0, player.GetComponent<PlayerStats>());
                break;
            case 1:
                player.GetComponent<BuffsManager>().NewBuff(BuffsManager.BuffType.WispsInvulnerbility, 0, player.GetComponent<PlayerStats>());
                break;
            case 2:
                player.GetComponent<BuffsManager>().NewBuff(BuffsManager.BuffType.WispsSpeed, 0, player.GetComponent<PlayerStats>());
                break;
            case 3:
                player.GetComponent<BuffsManager>().NewBuff(BuffsManager.BuffType.WispsCooldownReduction, 0, player.GetComponent<PlayerStats>());
                break;
            default:
                break;
        }
    }
}
