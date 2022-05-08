using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShrineBehaviour_Veteran : ShrineBehaviour
{
    [SerializeField] Text moneyCostText;
    [SerializeField] float shrineCost;

    private void Start()
    {
        shrineCost = GameManager.instance.shrineCost * 5;
        moneyCostText.text = string.Format("{0:0}", shrineCost);
    }

    public override void OnInteract(GameObject player)
    {
        PlayerStats stats = player.GetComponent<PlayerStats>();
        Inventory inventory = player.GetComponent<Inventory>();
        if (stats.gold >= shrineCost)
        {
            base.OnInteract(player);
            stats.AddGold(shrineCost * -1);
            Destroy(moneyCostText.transform.parent.gameObject);

            switch (Random.Range(0,9))
            {
                case 0:
                    stats.bonusHealthRegen += 10;
                    inventory.ShowHint("Health regen increased.");
                    break;
                case 1:
                    stats.bonusHealth += 250;
                    inventory.ShowHint("Health increased.");
                    break;
                case 2:
                    stats.bonusPercentHealth += 0.15f;
                    inventory.ShowHint("Percent maximum health increased.");
                    break;
                case 3:
                    stats.bonusAttackSpeed += 0.25f;
                    inventory.ShowHint("Attackspeed increased.");
                    break;
                case 4:
                    stats.movespeedPercentMultiplier += 0.15f;
                    inventory.ShowHint("Movespeed increased.");
                    break;
                case 5:
                    stats.cooldownReductionSources.Add(0.2f);
                    inventory.ShowHint("Cooldwon reduction increased.");
                    break;
                case 6:
                    stats.critChance += 0.05f;
                    inventory.ShowHint("Critical strike chance increased.");
                    break;
                case 7:
                    stats.critDamageMultiplier += 0.25f;
                    inventory.ShowHint("Critical strike damage increased.");
                    break;
                case 8:
                    stats.jumps += 1;
                    inventory.ShowHint("Maximum jumps increased.");
                    break;
                default:
                    break;
            }
            stats.StatSetup(false, true);
            player.GetComponent<HitBoxManager>().PlayParticles(8);
            GetComponent<AudioSource>().Play();
        }

    }
}
