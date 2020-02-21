using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Item> inventory = new List<Item>();
    public List<Item> trinkets = new List<Item>();
    public List<Item> weapons = new List<Item>();
    public List<Item> itemsInRange = new List<Item>();

    public InventoryUiManager inventoryUI;
    // This representts the maximum amount of items our chracter can hold.
    public float INVENTORY_MAX = 15;

    private Transform inventoryContainer;
    private PlayerInputs playerInputs;
    private PlayerStats stats;
    private PlayerGearManager gearManager;


    // USed to set up the inventory transform parent.
    private void Start()
    {
        inventoryContainer = transform.Find("Inventory");
        playerInputs = GetComponent<PlayerInputs>();
        inventoryUI.playerInventory = this;
        stats = GetComponent<PlayerStats>();
        gearManager = GetComponent<PlayerGearManager>();
    }


    // SUed to check for inputs to pick up items.
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            PickUpItem(GrabClosestItem());
    }

    // USed to pickup the item we are closest to.
    public void PickUpItem(Item item)
    {
        if (item != null)
        {
            Debug.Log("the inventory coutn is: " + inventory.Count);
            if (inventory.Count >= INVENTORY_MAX)
            {
                Debug.Log("Your inventory is full!");
                DistributeStacks(item);

                // Since we know the inventory is full, we do not add the item at all, we just check it it has stacks left afetr distrubution
                if(item.currentStack == 0)
                {
                    itemsInRange.Remove(item);
                    Destroy(item.gameObject);
                }
            }
            else
            {
                // set the item as part of our inventory if it cant combine with a similar stackable item with the same name.
                Debug.Log("We have picked up " + item.gameObject.name);
                DistributeStacks(item);
                
                // After we are done distributing into stacks, check to see if we have any leftover and need to make a new stack.
                itemsInRange.Remove(item);
                if (item.currentStack != 0)
                {
                    item.ComfirmPickup(inventoryContainer, FindFirstAvaibleSlot());
                    inventory.Add(item);
                    inventoryUI.UpdateInventorySlot(item);
                }
                else
                    Destroy(item.gameObject);
            }
        }
    }

    // Used to drop a specific item.
    public void DropItem(int index)
    {
        if (inventory.Count > 0)
        {
            Item targetItem = inventory[0];
            for (int currentIndex = 0; currentIndex < inventory.Count; currentIndex++)
            {
                if (inventory[currentIndex].inventoryIndex == index)
                    targetItem = inventory[currentIndex];
            }

            targetItem.previousOwner = gameObject;
            inventory.Remove(targetItem);
            targetItem.ComfirmDrop();
            inventoryUI.UpdateInventorySlot(targetItem.inventoryIndex);
        }
        Debug.Log(inventory.Count);
    }
    // Used to drop a specific item. TESTING ONLY
    public void DropItem()
    {
        if (inventory.Count > 0)
        {
            Item targetItem = inventory[0];
            targetItem.previousOwner = gameObject;
            inventory.Remove(targetItem);
            targetItem.ComfirmDrop();
            inventoryUI.UpdateInventorySlot(targetItem.inventoryIndex);
        }
    }

    // If we enter an items trigegr sphere, well add it to our list of items in range.
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Item"))
        {
            Item currentItem = other.GetComponent<Item>();
            if(currentItem.instantPickup && currentItem.previousOwner != gameObject)
            {
                Debug.Log("the item " + other.name + " was instantly picked up");
                // Instantly pick up the itme if we have room or an incomplete stack.
                DistributeStacks(currentItem);

                // After distributing the item to our stacks, we check if we have room for a new stack.
                if (currentItem.currentStack != 0)
                {
                    // If we have room we add the item to our inventory.
                    if (inventory.Count < INVENTORY_MAX)
                    {
                        currentItem.ComfirmPickup(inventoryContainer, FindFirstAvaibleSlot());
                        inventory.Add(currentItem);
                        inventoryUI.UpdateInventorySlot(currentItem);
                    }
                }
                else
                    Destroy(currentItem.gameObject);
            }
            else
                itemsInRange.Add(currentItem);
        }
    }

    // USed to find the first availible index that we can assign to an item.
    private int FindFirstAvaibleSlot()
    {
        int firstIndex = 0;
        bool emptyIndex = false;
        if (inventory.Count > 0)
        {
            // check the ivnentory, if one matches increment the index and continue.
            for (int index = 0; index <= inventory.Count; index++)
            {
                bool compatibleIndex = true;
                foreach (Item item in inventory)
                {
                    // If any match, break from this loop and do not set this as the first availible index.
                    if (item.inventoryIndex == index)
                        compatibleIndex = false;

                }

                // If the index is the ideal one, set it as the index to be assigned to the titem and break;
                if (compatibleIndex)
                {
                    firstIndex = index;
                    emptyIndex = true;
                    break;
                }
                // If we found an index, assign the item to it otherwise try the next index.
                if (emptyIndex)
                    break;
            }
        }

        // Debug.Log("the index is: " + firstIndex + ". The ivnentory count is: " + inventory.Count);
        return firstIndex;
    }

    // Used when we leave an items trigger sphere to remove them from our list.
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            itemsInRange.Remove(other.GetComponent<Item>());
        }
    }

    private Item GrabClosestItem()
    {
        Item closestItem = null;
        float closestItemDistance = 50f;

        // Check every item in range.
        foreach(Item item in itemsInRange)
        {
            // If the item is closer then previously checked items, set it as the closest.
            float distance = (item.transform.position - transform.position).sqrMagnitude;
            if(distance < closestItemDistance)
            {
                closestItemDistance = distance;
                closestItem = item;
            }
        }

        return closestItem;
    }

    // Used to transfer an item to a different type of slot
    public void TransferItem(Item item, ItemDropZone.SlotType originalType , ItemDropZone.SlotType newType)
    {
        //Debug.Log("item transfer");
        // remove the item from each list depending on the original type.
        switch (originalType)
        {
            case ItemDropZone.SlotType.Inventory:
                inventory.Remove(item);
                break;
            case ItemDropZone.SlotType.Trinket:
                trinkets.Remove(item);
                stats.RemoveItemStats(item, true);
                break;
            case ItemDropZone.SlotType.Weapon:
                weapons.Remove(item);
                stats.RemoveItemStats(item, true);
                gearManager.HideItem(item);
                break;
            case ItemDropZone.SlotType.Armor:
                stats.RemoveItemStats(item, true);
                gearManager.HideItem(item);
                break;
            case ItemDropZone.SlotType.Helmet:
                stats.RemoveItemStats(item, true);
                gearManager.HideItem(item);
                break;
            case ItemDropZone.SlotType.Leggings:
                stats.RemoveItemStats(item, true);
                gearManager.HideItem(item);
                break;
            default:
                break;
        }

        //Debug.Log("halfway there");

        // add the item to the appropriate list based on the type.
        switch (newType)
        {
            case ItemDropZone.SlotType.Inventory:
                inventory.Add(item);
                break;
            case ItemDropZone.SlotType.Trinket:
                trinkets.Add(item);
                stats.AddItemStats(item, true);
                break;
            case ItemDropZone.SlotType.Weapon:
                weapons.Add(item);
                stats.AddItemStats(item, true);
                gearManager.ShowItem(item);
                break;
            case ItemDropZone.SlotType.Armor:
                stats.AddItemStats(item, true);
                gearManager.ShowItem(item);
                break;
            case ItemDropZone.SlotType.Helmet:
                stats.AddItemStats(item, true);
                gearManager.ShowItem(item);
                break;
            case ItemDropZone.SlotType.Leggings:
                stats.AddItemStats(item, true);
                gearManager.ShowItem(item);
                break;
            default:
                break;
        }
    }

    // make an item switch from the left hand to the right or vice versa, but do not recalculate stats for said item.
    public void SwitchHands(Item item)
    {
        gearManager.HideItem(item);

        if (item.equippedToRightHand)
            item.equippedToRightHand = false;
        else
            item.equippedToRightHand = true;

        gearManager.ShowItem(item);
    }

    // Used to place an item into other incomplete stacks of said item.
    private void DistributeStacks(Item item)
    {
        // Debug.Log("distributing stacks");
        foreach (Item currentItem in inventory)
        {
            // break from this loop if our item in question's current stack is zero as that means it was distrubusted into previously unfinished stacks.
            if (item.currentStack == 0)
                break;
            // If the item matches the other item, is stackable, and it's stack isn't full add as many as possible 
            if (currentItem.itemName == item.itemName && item.stackable && currentItem.currentStack < currentItem.maxStack)
            {
                currentItem.currentStack += item.currentStack;
                item.currentStack = 0;
                // If we have too many...
                if (currentItem.currentStack > currentItem.maxStack)
                {
                    // Remove until we have the proper amount.
                    int amountToRemove = currentItem.currentStack - currentItem.maxStack;
                    currentItem.currentStack -= amountToRemove;
                    item.currentStack = amountToRemove;
                }
                inventoryUI.UpdateInventorySlot(currentItem);
            }
        }
    }
}
