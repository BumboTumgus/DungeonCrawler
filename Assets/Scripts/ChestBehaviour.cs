using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestBehaviour : MonoBehaviour
{
    public enum ChestRarity { Common, Uncommon, Rare, Legendary}
    public ChestRarity chestRarity = ChestRarity.Common;
    public int treasureCount = 1;
    public int itemCount = 0;

    public GameObject treasureDrop;
    public GameObject itemDrop;

    // Here we roll the contents of this treasure chest.
    void Start()
    {
        RollChestContents();
    }

    // Used to check what the contents of this chest will be.
    private void RollChestContents()
    {
        // Check the rarity of the chest and base the contents of it off the rarity.
        switch (chestRarity)
        {
            case ChestRarity.Common:
                treasureCount = Random.Range(0, 3);
                itemCount = Random.Range(0, 1);
                break;
            case ChestRarity.Uncommon:
                treasureCount = Random.Range(2, 6);
                itemCount = Random.Range(1, 3);
                break;
            case ChestRarity.Rare:
                treasureCount = Random.Range(4, 10);
                itemCount = Random.Range(2, 5);
                break;
            case ChestRarity.Legendary:
                treasureCount = Random.Range(8, 15);
                itemCount = Random.Range(3, 7);
                break;
            default:
                break;
        }
    }

    // Grab the contents of the chest, and spawn them as treasure that flies upwards then out towards the player.
    public void OpenChest()
    {
        Debug.Log(" The contetns of the chest are " + treasureCount + " treasure and " + itemCount + " items");

        // Spawns the loot.
        for(int x = 0; x <= treasureCount -1; x++)
        {
            GameObject currentTreasure = Instantiate(treasureDrop, transform.position + new Vector3(Random.Range(-1.5f, 1.5f), 0.1f, Random.Range(-1.5f, 1.5f)), transform.rotation);
        }

        // Spawns the items.
        for (int x = 0; x <= itemCount - 1; x++)
        {
            GameObject currentItems = Instantiate(itemDrop, transform.position + new Vector3(Random.Range(-1.5f, 1.5f), 0.1f, Random.Range(-1.5f, 1.5f)), transform.rotation);
        }

        // Remove ourselve as the selected Object.
        GameObject.Find("ObjectSelectionManager").GetComponent<SelectionManager>().DeselectSecondaryRing();
        
        // Destroys self.
        Destroy(gameObject);
    }
}
