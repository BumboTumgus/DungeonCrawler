using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrineBehaviour_ElementalFountain : ShrineBehaviour
{
    public override void OnInteract(GameObject player)
    {
        base.OnInteract(player);
    }

    public void OnInteract(Item.AffinityType primaryAffinity, Item.AffinityType secondaryAffinity)
    {
        if (gameObjectsToHideOnUse.Length > 0)
        {
            foreach (GameObject go in gameObjectsToHideOnUse)
                go.SetActive(false);
        }

        switch (primaryAffinity)
        {
            case Item.AffinityType.Fire:
                particleSystemsToPlayOnUse[0].Play();
                break;
            case Item.AffinityType.Ice:
                particleSystemsToPlayOnUse[1].Play();
                break;
            case Item.AffinityType.Earth:
                particleSystemsToPlayOnUse[2].Play();
                break;
            case Item.AffinityType.Wind:
                particleSystemsToPlayOnUse[3].Play();
                break;
            case Item.AffinityType.Physical:
                particleSystemsToPlayOnUse[4].Play();
                break;
            case Item.AffinityType.Bleed:
                particleSystemsToPlayOnUse[5].Play();
                break;
            case Item.AffinityType.Poison:
                particleSystemsToPlayOnUse[6].Play();
                break;
            case Item.AffinityType.Stun:
                particleSystemsToPlayOnUse[4].Play();
                break;
            case Item.AffinityType.Knockback:
                particleSystemsToPlayOnUse[4].Play();
                break;
            default:
                break;
        }
        if (secondaryAffinity != primaryAffinity)
        {
            switch (secondaryAffinity)
            {
                case Item.AffinityType.Fire:
                    particleSystemsToPlayOnUse[0].Play();
                    break;
                case Item.AffinityType.Ice:
                    particleSystemsToPlayOnUse[1].Play();
                    break;
                case Item.AffinityType.Earth:
                    particleSystemsToPlayOnUse[2].Play();
                    break;
                case Item.AffinityType.Wind:
                    particleSystemsToPlayOnUse[3].Play();
                    break;
                case Item.AffinityType.Physical:
                    particleSystemsToPlayOnUse[4].Play();
                    break;
                case Item.AffinityType.Bleed:
                    particleSystemsToPlayOnUse[5].Play();
                    break;
                case Item.AffinityType.Poison:
                    particleSystemsToPlayOnUse[6].Play();
                    break;
                case Item.AffinityType.Stun:
                    particleSystemsToPlayOnUse[4].Play();
                    break;
                case Item.AffinityType.Knockback:
                    particleSystemsToPlayOnUse[4].Play();
                    break;
                default:
                    break;
            }
        }

        GetComponent<AudioSource>().Play();

        Collider[] cols = GetComponentsInChildren<Collider>();
        foreach (Collider col in cols)
            if (col.isTrigger)
                col.enabled = false;
    }
}
