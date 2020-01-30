using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBehaviour : MonoBehaviour
{
    public enum ItemType {  Treasure, Weapon, Boots, Helmet, Armor, Leggings, Consumable, Undetermined}
    public ItemType itemType = ItemType.Treasure;
    public int treasureValue = 10;
    public bool randomize = true;

    private const int ROLLABLE_ITEM_COUNT = 6;

    // Roll the item here.
    private void Start()
    {
        // Check if the item should be rolled or it is static.
        if (randomize)
            RollItem();
    }

    // Used to roll what the item will be.
    private void RollItem()
    {
        // If the item is treasure, how much is it worth?
        if (itemType == ItemType.Treasure)
            treasureValue = Random.Range(5, 20);
        // If the item is an actual item, 
        if (itemType == ItemType.Undetermined)
        {
            // Set what type of item we will roll.
            int randomNum = Random.Range(0, ROLLABLE_ITEM_COUNT);
            switch (randomNum)
            {
                case 0:
                    itemType = ItemType.Armor;
                    break;
                case 1:
                    itemType = ItemType.Boots;
                    break;
                case 2:
                    itemType = ItemType.Consumable;
                    break;
                case 3:
                    itemType = ItemType.Helmet;
                    break;
                case 4:
                    itemType = ItemType.Leggings;
                    break;
                case 5:
                    itemType = ItemType.Weapon;
                    break;
                default:
                    break;
            }

            // Here we would set up the items stats and rarity. Use a switch?
        }
    }

    // Used to attempt to pickup the Item
    public void AttemptPickup()
    {
        if (itemType != ItemType.Treasure)
            Debug.Log("Player has picked up an item of type: " + itemType);
        else
            Debug.Log("Player has found " + treasureValue + "gP worth of treasure!");

        // Remove ourselve as the selected Object.
        GameObject.Find("ObjectSelectionManager").GetComponent<SelectionManager>().DeselectSecondaryRing();

        // Destroys self.
        Destroy(gameObject);
    }
}
