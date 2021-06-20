using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestBehaviour : MonoBehaviour
{
    public enum ChestRarity { Common, Uncommon, Rare, Legendary, Cursed, Masterwork}
    public ChestRarity chestRarity = ChestRarity.Common;
    public int itemCount = 0;

    public float chestCost = 10;
    [SerializeField] float itemBonusRarity = 0;

    public List<GameObject> itemDrops;

    [SerializeField] Text chestCostText;

    private const float MIN_DROP_RING = 0.5f;
    private const float MAX_DROP_RING = 2.0f;

    // Here we roll the contents of this treasure chest.
    void Start()
    {
        if(chestRarity != ChestRarity.Cursed)
            chestCostText.text = (int)chestCost + "";
    }

    // Used to check what the contents of this chest will be.
    private void RollChestContents()
    {
            // Check the rarity of the chest and base the contents of it off the rarity.
            switch (chestRarity)
            {
                case ChestRarity.Common:
                    itemCount = 1;
                    break;
                case ChestRarity.Uncommon:
                    itemCount = 1;
                    break;
                case ChestRarity.Rare:
                    itemCount = Random.Range(1, 3);
                    break;
                case ChestRarity.Legendary:
                    itemCount = Random.Range(1, 3);
                    break;
                case ChestRarity.Masterwork:
                    itemCount = Random.Range(1, 4);
                    break;
                case ChestRarity.Cursed:
                    itemCount = Random.Range(0, 8);
                    break;
                default:
                    break;
            }

            for (int index = 0; index < itemCount; index++)
                itemDrops.Add(ItemGenerator.instance.RollItem(itemBonusRarity));
    }

    // Grab the contents of the chest, and spawn them as treasure that flies upwards then out towards the player.
    public void OpenChest()
    {
        RollChestContents();
        //Debug.Log(" The contetns of the chest are " + treasureCount + " treasure and " + itemCount + " items");

        GetComponent<Animator>().SetTrigger("OpenSesame");
        Destroy(GetComponent<SphereCollider>());

        // spawn the loot.
        foreach (GameObject lootsidoodle in itemDrops)
        {
            GameObject currentObject = Instantiate(lootsidoodle, transform.position, transform.rotation * Quaternion.Euler(new Vector3(0,Random.Range(-50,50),0)));
            currentObject.GetComponentInChildren<Item>().RollItemTraitsAffinityAndModifiers();
            currentObject.GetComponentInChildren<Item>().ItemPopIn(currentObject.transform.position + currentObject.transform.forward * Random.Range(MIN_DROP_RING, MAX_DROP_RING));
        }
        transform.GetComponentInChildren<ParticleSystem>().Stop();
        Destroy(transform.GetComponentInChildren<Light>());
        Destroy(chestCostText.transform.parent.gameObject);
    }
}
