using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUiManager : MonoBehaviour
{
    public Inventory playerInventory;
    public SkillsManager playerSkills;

    public GameObject[] inventorySlots;
    public GameObject[] trinketSlots;
    public GameObject[] skillSlots;
    public GameObject leftHandSlot;
    public GameObject rightHandSlot;
    public GameObject helmetSlot;
    public GameObject chestSlot;
    public GameObject legsSlot;

    public Color[] itemOutlineColors;

    private void Start()
    {
        foreach (GameObject slot in inventorySlots)
            WipeSlot(slot);
        foreach (GameObject slot in trinketSlots)
            WipeSlot(slot);
        foreach (GameObject slot in skillSlots)
            WipeSlot(slot);
        WipeSlot(leftHandSlot);
        WipeSlot(rightHandSlot);
        WipeSlot(helmetSlot);
        WipeSlot(chestSlot);
        WipeSlot(legsSlot);
        CheckActiveSkillSlots();

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
        
        Image outlineImage = slotToUpdate.transform.Find("ItemPanel").Find("Outline").GetComponent<Image>();
        switch (item.itemRarity)
        {
            case Item.ItemRarity.Common:
                outlineImage.color = itemOutlineColors[0];
                break;
            case Item.ItemRarity.Uncommon:
                outlineImage.color = itemOutlineColors[1];
                break;
            case Item.ItemRarity.Rare:
                outlineImage.color = itemOutlineColors[2];
                break;
            case Item.ItemRarity.Legendary:
                outlineImage.color = itemOutlineColors[3];
                break;
            case Item.ItemRarity.Masterwork:
                outlineImage.color = itemOutlineColors[4];
                break;
            default:
                break;
        }
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

        slot.transform.Find("ItemPanel").Find("Outline").GetComponent<Image>().color = new Color(255, 255, 255, 0);
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

    public void CheckActiveSkillSlots()
    {
        //Debug.Log("checking the active skill slots");
        // Here I will only have active skillslots equal to the number of spells the player can currently have.
        //int currentSkillMax = playerSkills.maxSkillNumber;
        for (int index = 0; index < skillSlots.Length; index++)
        {
            if (index < playerSkills.maxSkillNumber)
                skillSlots[index].SetActive(true);
            else
            {
                skillSlots[index].SetActive(false);
                // check to see if there a skill in this skill slot, if so, disable it then move it to inventory. if there is no room in the inventory, drop the skill.
                if(skillSlots[index].GetComponentInChildren<ItemDraggable>().attachedItem != null)
                {
                    //Debug.Log("skill detected in lost skill slot, removing skill");
                    playerSkills.RemoveSkill(index);
                    // Check if we have room in the inventory
                    if (playerInventory.inventory.Count < playerInventory.INVENTORY_MAX - 1)
                    {
                        //Debug.Log("There is room in the inventory so we move this into the next unoccupied slot");
                        Item itemToMove = skillSlots[index].GetComponentInChildren<ItemDraggable>().attachedItem.GetComponent<Item>();
                        itemToMove.inventoryIndex = playerInventory.FindFirstAvaibleSlot();
                        playerInventory.inventory.Add(itemToMove);
                        UpdateInventorySlot(itemToMove);
                        WipeSlot(skillSlots[index]);
                    }
                    else
                    {
                        //Debug.Log("There is no room in the inventory for this item");
                        Item itemToYeet = skillSlots[index].GetComponentInChildren<ItemDraggable>().attachedItem.GetComponent<Item>();
                        itemToYeet.ComfirmDrop();
                        WipeSlot(skillSlots[index]);
                    }
                }
            }
        }
    }


}
