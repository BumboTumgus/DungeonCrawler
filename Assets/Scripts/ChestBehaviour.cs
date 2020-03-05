using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestBehaviour : MonoBehaviour
{
    public enum ChestRarity { Treasure, Common, Uncommon, Rare, Legendary}
    public ChestRarity chestRarity = ChestRarity.Common;
    public int treasureCount = 1;
    public int itemCount = 0;

    public List<GameObject> treasureDrops;
    public List<GameObject> itemDrops;

    private ItemGenerator itemGenerator;

    // Here we roll the contents of this treasure chest.
    void Start()
    {
        itemGenerator = FindObjectOfType<ItemGenerator>();
        //Debug.Log(itemGenerator);
        RollChestContents();
    }

    // Used to check what the contents of this chest will be.
    private void RollChestContents()
    {
        // Check the rarity of the chest and base the contents of it off the rarity.
        switch (chestRarity)
        {
            case ChestRarity.Treasure:
                treasureCount = Random.Range(2, 4);
                itemCount = 0;
                break;
            case ChestRarity.Common:
                treasureCount = Random.Range(1, 4);
                itemCount = Random.Range(1, 2);
                break;
            case ChestRarity.Uncommon:
                treasureCount = Random.Range(2, 6);
                itemCount = Random.Range(2, 4);
                break;
            case ChestRarity.Rare:
                treasureCount = Random.Range(4, 10);
                itemCount = Random.Range(3, 6);
                break;
            case ChestRarity.Legendary:
                treasureCount = Random.Range(8, 15);
                itemCount = Random.Range(4, 8);
                break;
            default:
                break;
        }
        for (int index = 0; index < itemCount; index++)
            itemDrops.Add(itemGenerator.RollItem());
        for (int index = 0; index < treasureCount; index++)
            treasureDrops.Add(itemGenerator.RollTreasure());
    }

    // Grab the contents of the chest, and spawn them as treasure that flies upwards then out towards the player.
    public void OpenChest()
    {
        Debug.Log(" The contetns of the chest are " + treasureCount + " treasure and " + itemCount + " items");

        GetComponent<Animator>().SetTrigger("OpenSesame");
        Destroy(GetComponent<SphereCollider>());

        // spawn the loot.
        foreach(GameObject lootsidoodle in itemDrops)
            Instantiate(lootsidoodle, transform.position + transform.forward * Random.Range(0.5f, 1.5f) + transform.right * Random.Range(-1f, 1f), Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)));
        foreach(GameObject gold in treasureDrops)
        {
            GameObject treasure = Instantiate(gold, transform.position + transform.forward * Random.Range(0.5f, 1.5f) + transform.right * Random.Range(-1f, 1f), Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)));
            
            Item treasureItem = treasure.GetComponent<Item>();
            treasureItem.currentStack = Random.Range(treasureItem.currentStack / 2, treasureItem.currentStack * 2);
            // Debug.Log(treasureItem.currentStack);
        }
    }
}
