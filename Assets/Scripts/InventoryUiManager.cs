using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUiManager : MonoBehaviour
{
    public Inventory playerInventory;

    public GameObject[] inventorySlots;
    public GameObject[] trinketSlots;
    public GameObject leftHandSlot;
    public GameObject rightHandSlot;
    public GameObject helmetSlot;
    public GameObject chestSlot;
    public GameObject legsSlot;

    private void Start()
    {
        foreach (GameObject slot in inventorySlots)
            WipeSlot(slot);
        foreach (GameObject slot in trinketSlots)
            WipeSlot(slot);
        WipeSlot(leftHandSlot);
        WipeSlot(rightHandSlot);
        WipeSlot(helmetSlot);
        WipeSlot(chestSlot);
        WipeSlot(legsSlot);
        gameObject.SetActive(false);
    }

    public void UpdateInventorySlot(Item item)
    {
         // If we have an item, set the picture to that associated with the item ans show thbe image.
         GameObject slotToUpdate = inventorySlots[item.inventoryIndex];

         slotToUpdate.transform.Find("ItemPanel").GetComponent<ItemDraggable>().enabled = true;
         slotToUpdate.transform.Find("ItemPanel").GetComponent<ItemDraggable>().attachedItem = item.gameObject;

         Image targetImage = slotToUpdate.transform.Find("ItemPanel").Find("ItemImage").GetComponent<Image>();
         targetImage.color = new Color(255, 255, 255, 255);
         targetImage.sprite = item.artwork;

         // If we have more then one in this stack of item enable the counter.
         if (item.currentStack > 1)
             slotToUpdate.transform.Find("ItemPanel").Find("ItemCount").GetComponent<Text>().text = "x" + item.currentStack;
         else
             slotToUpdate.transform.Find("ItemPanel").Find("ItemCount").GetComponent<Text>().text = "";
    }

    public void UpdateInventorySlot(int index)
    {
        // Used if we are clearing this inventory slot.
        GameObject slotToUpdate = inventorySlots[index];

        WipeSlot(slotToUpdate);
    }

    public void WipeSlot(GameObject slot)
    {
        // Disable the draggable script on this item so that we cant drag around empty items.
        if (slot.transform.Find("ItemPanel") != null)
            slot.transform.Find("ItemPanel").GetComponent<ItemDraggable>().enabled = false;
        slot.transform.Find("ItemPanel").GetComponent<ItemDraggable>().attachedItem = null;

        if (slot.transform.Find("ItemPanel").Find("ItemCount") != null)
            slot.transform.Find("ItemPanel").Find("ItemCount").GetComponent<Text>().text = "";

        Image targetImage = slot.transform.Find("ItemPanel").Find("ItemImage").GetComponent<Image>();
        targetImage.color = new Color(255, 255, 255, 0);
        targetImage.sprite = null;
    }

    public ItemDraggable GetNextEmptySlot()
    {
        // Return the next empty draggable item in the inventory.
        ItemDraggable inventorySlot = null;
        foreach(GameObject slot in inventorySlots)
        {
            // Check to see if the slot is null, just in case the item is unparented because it is being dragged around,
            if (slot.transform.Find("ItemPanel") != null && slot.transform.Find("ItemPanel").GetComponent<ItemDraggable>().attachedItem == null)
            {
                inventorySlot = slot.transform.Find("ItemPanel").GetComponent<ItemDraggable>();
                break;
            }
        }

        return inventorySlot;
    }
}
