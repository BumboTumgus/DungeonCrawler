using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestBehaviour : MonoBehaviour
{
    public enum ChestRarity { Common, Uncommon, Rare, Legendary, Cursed, Masterwork}
    public ChestRarity chestRarity = ChestRarity.Common;

    public float chestCost = 10;

    public List<GameObject> itemDrops = new List<GameObject>();

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
        if((chestRarity != ChestRarity.Masterwork || chestRarity != ChestRarity.Cursed) && Random.Range(0f,100f) + GameManager.instance.combinedPlayerLuck > 90)
        {
            Debug.Log("Item tiered UPGRADE");
            switch (chestRarity)
            {
                case ChestRarity.Common:
                    chestRarity = ChestRarity.Uncommon;
                    break;
                case ChestRarity.Uncommon:
                    chestRarity = ChestRarity.Rare;
                    break;
                case ChestRarity.Rare:
                    chestRarity = ChestRarity.Legendary;
                    break;
                case ChestRarity.Legendary:
                    chestRarity = ChestRarity.Masterwork;
                    break;
                default:
                    break;
            }
        }

        switch (chestRarity)
        {
            case ChestRarity.Common:
                itemDrops.Add(ItemGenerator.instance.RollItem(Item.ItemRarity.Common));
                break;
            case ChestRarity.Uncommon:
                itemDrops.Add(ItemGenerator.instance.RollItem(Item.ItemRarity.Uncommon));
                break;
            case ChestRarity.Rare:
                itemDrops.Add(ItemGenerator.instance.RollItem(Item.ItemRarity.Rare));
                break;
            case ChestRarity.Legendary:
                itemDrops.Add(ItemGenerator.instance.RollItem(Item.ItemRarity.Legendary));
                break;
            case ChestRarity.Cursed:
                itemDrops.Add(ItemGenerator.instance.RollItem(Item.ItemRarity.Legendary));
                itemDrops.Add(ItemGenerator.instance.RollItem(Item.ItemRarity.Legendary));
                itemDrops.Add(ItemGenerator.instance.RollItem(Item.ItemRarity.Legendary));
                break;
            case ChestRarity.Masterwork:
                itemDrops.Add(ItemGenerator.instance.RollItem(Item.ItemRarity.Masterwork));
                break;
            default:
                break;
        }

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
