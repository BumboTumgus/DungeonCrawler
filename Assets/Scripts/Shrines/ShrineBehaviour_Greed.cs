using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShrineBehaviour_Greed : ShrineBehaviour
{
    [SerializeField] Text moneyCostText;
    [SerializeField] float shrineCost;
    [SerializeField] ParticleSystem winEffect;
    [SerializeField] Transform itemSpawnPoint;
    AudioManager audiomanager;

    private void Start()
    {
        shrineCost = GameManager.instance.shrineCost;
        moneyCostText.text = string.Format("{0:0}", shrineCost);
        moneyCostText.transform.parent.GetComponent<FaceNearestPlayerBehaviour>().playerToFace = GameManager.instance.currentPlayers[0].transform;
        audiomanager = GetComponent<AudioManager>();
    }

    public override void OnInteract(GameObject player)
    {
        PlayerStats stats = player.GetComponent<PlayerStats>();
        if (stats.gold >= shrineCost)
        {
            base.OnInteract(player);
            stats.AddGold(shrineCost * -1);

            // ROll an item here
            if(Random.Range(0f, 100f) < 50 + GameManager.instance.combinedPlayerLuck)
            {
                // WIN AN ITEM
                float[] rollChances = GameManager.instance.chestRarityRC;
                float randomRoll = Random.Range(0, 100) + GameManager.instance.combinedPlayerLuck;
                Item.ItemRarity itemRarity = Item.ItemRarity.Common;

                if (randomRoll <= rollChances[0] && rollChances[0] != 0)
                    itemRarity = Item.ItemRarity.Uncommon;
                else if (randomRoll <= rollChances[0] + rollChances[1] && rollChances[1] != 0)
                    itemRarity = Item.ItemRarity.Rare;
                else if (randomRoll <= rollChances[0] + rollChances[1] + rollChances[2] && rollChances[2] != 0)
                    itemRarity = Item.ItemRarity.Legendary;
                else if (randomRoll <= rollChances[0] + rollChances[1] + rollChances[2] + rollChances[3] && rollChances[3] != 0)
                    itemRarity = Item.ItemRarity.Masterwork;
                else
                    itemRarity = Item.ItemRarity.Masterwork;

                GameObject item = ItemGenerator.instance.RollItem(itemRarity);
                GameObject spawnedItem = Instantiate(item, itemSpawnPoint.position, Quaternion.identity);
                spawnedItem.GetComponentInChildren<Item>().RollItemTraitsAffinityAndModifiers();

                winEffect.Play();
                audiomanager.PlayAudio(0);
            }
            else
            {
                // NO WIN
                audiomanager.PlayAudio(1);
            }

            // Increase the cost here.
            shrineCost *= 1.25f;
            moneyCostText.text = string.Format("{0:0}", shrineCost);
        }
    }
}
