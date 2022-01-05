using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Item> inventory = new List<Item>();
    public List<Item> trinkets = new List<Item>();
    public List<Item> weapons = new List<Item>();
    public List<Item> itemsInRange = new List<Item>();
    public List<GameObject> interactablesInRange = new List<GameObject>();

    public InteractPromptController interactPrompt;
    public InteractPromptController hintPrompt;
    public InventoryUiManager inventoryUI;

    public bool showPromptUI = true;

    private Animator anim;
    // This representts the maximum amount of items our chracter can hold.
    public float INVENTORY_MAX = 15;

    private Transform inventoryContainer;
    private PlayerInputs playerInputs;
    private PlayerStats stats;
    private PlayerGearManager gearManager;
    private bool firstItemPickup = true;


    // USed to set up the inventory transform parent.
    private void Start()
    {
        inventoryContainer = transform.Find("Inventory");
        playerInputs = GetComponent<PlayerInputs>();
        inventoryUI.playerInventory = this;
        inventoryUI.playerSkills = GetComponent<SkillsManager>();
        stats = GetComponent<PlayerStats>();
        gearManager = GetComponent<PlayerGearManager>();
        anim = GetComponent<Animator>();
        StartCoroutine(UpdatePromptUI());
    }

    // USed to pickup the item we are closest to.
    public void PickUpItem(Item item)
    {
        if(firstItemPickup)
        {
            firstItemPickup = false;
            // Launch our hint here.
            StartCoroutine(ShowInventoryHint());
        }

        if (item != null)
        {
            // Debug.Log("the inventory coutn is: " + inventory.Count);
            if (inventory.Count >= INVENTORY_MAX)
            {
                // Debug.Log("Your inventory is full!");
                DistributeStacks(item);

                // Since we know the inventory is full, we do not add the item at all, we just check it it has stacks left after distrubution
                if(item.currentStack == 0)
                {
                    itemsInRange.Remove(item);
                    Destroy(item.gameObject);
                }
            }
            else
            {
                // set the item as part of our inventory if it cant combine with a similar stackable item with the same name.
                // Debug.Log("We have picked up " + item.gameObject.name);
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

    IEnumerator ShowInventoryHint()
    {
        hintPrompt.SetText("Press I to access your inventory.");
        yield return new WaitForSecondsRealtime(3f);
        hintPrompt.SetText("");
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
            targetItem.ItemPopIn(transform.position + transform.forward * Random.Range(0.5f, 1.5f));
            inventoryUI.UpdateInventorySlot(targetItem.inventoryIndex);
            GetComponent<AudioManager>().PlayAudio(17);
        }
        // Debug.Log(inventory.Count);
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
    public void ItemInRange(Item item)
    {
        Item currentItem = item;
        if(currentItem.instantPickup && currentItem.previousOwner != gameObject && currentItem.itemPickUpAllowed)
        {
            // Debug.Log("the item " + item.gameObject.name + " was instantly picked up");
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

    // USed to find the first availible index that we can assign to an item.
    public int FindFirstAvaibleSlot()
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
        else if (other.CompareTag("Interactable"))
        {
            interactablesInRange.Remove(other.gameObject);
        }
    }

    public Item GrabClosestItem()
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

    public GameObject GrabClosestInteractable()
    {
        GameObject closestInteractable = null;
        float closestItemDistance = 50f;

        // Check every item in range.
        foreach (GameObject interactable in interactablesInRange)
        {
            // If the item is closer then previously checked items, set it as the closest.
            float distance = (interactable.transform.position - transform.position).sqrMagnitude;
            if (distance < closestItemDistance)
            {
                closestItemDistance = distance;
                closestInteractable = interactable;
            }
        }

        return closestInteractable;
    }

    // Used to transfer an item to a different type of slot
    public void TransferItem(Item item, ItemDropZone.SlotType originalType , ItemDropZone.SlotType newType)
    {
        // WE add first to avoid issues with gear removing the layers spell slots when Remove Item Stats is called, but then the spells take the most recent slot
        // which is where we were trying to put an item.

        // add the item to the appropriate list based on the type.
        switch (newType)
        {
            case ItemDropZone.SlotType.Inventory:
                inventory.Add(item);
                break;
            case ItemDropZone.SlotType.Trinket:
                trinkets.Add(item);
                stats.AddItemStats(item, true, false);
                gearManager.ShowItem(item);
                break;
            case ItemDropZone.SlotType.Weapon:
                //Debug.Log("we are transferring in a item called: " + item.itemName);
                weapons.Add(item);
                CheckMoveset();
                //Debug.Log(" we added the items stats here");
                stats.AddItemStats(item, true, false);
                //Debug.Log(" the item should appear here");
                gearManager.ShowItem(item);
                break;
            case ItemDropZone.SlotType.Armor:
                stats.AddItemStats(item, true, false);
                gearManager.ShowItem(item);
                break;
            case ItemDropZone.SlotType.Helmet:
                stats.AddItemStats(item, true, false);
                gearManager.ShowItem(item);
                break;
            case ItemDropZone.SlotType.Leggings:
                stats.AddItemStats(item, true, false);
                gearManager.ShowItem(item);
                break;
            default:
                break;
        }


        //Debug.Log("item transfer");
        // remove the item from each list depending on the original type.
        switch (originalType)
        {
            case ItemDropZone.SlotType.Inventory:
                inventory.Remove(item);
                break;
            case ItemDropZone.SlotType.Trinket:
                trinkets.Remove(item);
                stats.RemoveItemStats(item, true, false);
                gearManager.HideItem(item);
                break;
            case ItemDropZone.SlotType.Weapon:
                weapons.Remove(item);
                CheckMoveset();
                stats.RemoveItemStats(item, true, false);
                gearManager.HideItem(item);
                break;
            case ItemDropZone.SlotType.Armor:
                stats.RemoveItemStats(item, true, false);
                gearManager.HideItem(item);
                break;
            case ItemDropZone.SlotType.Helmet:
                stats.RemoveItemStats(item, true, false);
                gearManager.HideItem(item);
                break;
            case ItemDropZone.SlotType.Leggings:
                stats.RemoveItemStats(item, true, false);
                gearManager.HideItem(item);
                break;
            default:
                break;
        }

        //Debug.Log("halfway there");

    }

    // make an item switch from the left hand to the right or vice versa, but do not recalculate stats for said item.
    public void SwitchHands(Item primaryItem, Item secondaryItem)
    {
        Debug.Log("switching hands");
        gearManager.HideItem(primaryItem);

        if (primaryItem.equippedToRightHand)
            primaryItem.equippedToRightHand = false;
        else
            primaryItem.equippedToRightHand = true;

        if(secondaryItem != null)
        {
            gearManager.HideItem(secondaryItem);

            if (secondaryItem.equippedToRightHand)
                secondaryItem.equippedToRightHand = false;
            else
                secondaryItem.equippedToRightHand = true;
        }

        stats.myStats.UpdateTooltips(stats);

        gearManager.ShowItem(primaryItem);
        if (secondaryItem != null)
            gearManager.ShowItem(secondaryItem);
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

    // Used to check what weapons i currently have equipped and change my move set based on which ones i have.
    private void CheckMoveset()
    {
        Item rightHandWeapon = null;
        Item leftHandWeapon = null;
        foreach(Item weapon in weapons)
        {
            if (weapon.equippedToRightHand)
                rightHandWeapon = weapon;
            else
                leftHandWeapon = weapon;
        }

        if (rightHandWeapon == null || rightHandWeapon.itemMoveset == 0 && leftHandWeapon == null || rightHandWeapon.itemMoveset == 0 && leftHandWeapon.itemMoveset == 6)
            anim.SetInteger("CurrentStance", 0);
        else if (rightHandWeapon.itemMoveset == 0 && leftHandWeapon.itemMoveset == 0)
            anim.SetInteger("CurrentStance", 4);
        else if (rightHandWeapon.itemMoveset == 1)
            anim.SetInteger("CurrentStance", 1);
        else if (rightHandWeapon.itemMoveset == 2)
            anim.SetInteger("CurrentStance", 2);
        else if (rightHandWeapon.itemMoveset == 3)
            anim.SetInteger("CurrentStance", 3);
    }

    // This coroutine checks the length of the interactables list, and if it's more than 2 items, grabs the closest one and changes the prompt on the ui to match it.
    IEnumerator UpdatePromptUI()
    {
        yield return new WaitForSeconds(0.1f);

        float currentTimer = 0;
        float targetTimer = 0.15f;
        
        while(showPromptUI)
        {
            currentTimer += Time.deltaTime;
            if (currentTimer > targetTimer)
            {
                currentTimer -= targetTimer;
                GameObject closestTarget = null;

                if (interactablesInRange.Count > 1)
                    closestTarget = GrabClosestInteractable();
                else if (interactablesInRange.Count == 1)
                    closestTarget = interactablesInRange[0];
                else if (itemsInRange.Count > 1)
                    closestTarget = GrabClosestItem().gameObject;
                else if (itemsInRange.Count == 1)
                    closestTarget = itemsInRange[0].gameObject;

                if (closestTarget != null)
                {
                    // Debug.Log(" the current closest Item is: " + closestTarget);
                    if (closestTarget.GetComponent<ChestBehaviour>() != null)
                        interactPrompt.SetText("Press E to open chest for " + closestTarget.GetComponent<ChestBehaviour>().chestCost);
                    else if (closestTarget.GetComponent<Item>() != null)
                    {
                        switch (closestTarget.GetComponent<Item>().itemRarity)
                        {
                            case Item.ItemRarity.Common:
                                interactPrompt.SetText(string.Format("Press E to pickup <color=#ffffff>{0}</color>", closestTarget.GetComponent<Item>().itemName));
                                break;
                            case Item.ItemRarity.Uncommon:
                                interactPrompt.SetText(string.Format("Press E to pickup <color=#60ec60>{0}</color>", closestTarget.GetComponent<Item>().itemName));
                                break;
                            case Item.ItemRarity.Rare:
                                interactPrompt.SetText(string.Format("Press E to pickup <color=#60c7ec>{0}</color>", closestTarget.GetComponent<Item>().itemName));
                                break;
                            case Item.ItemRarity.Legendary:
                                interactPrompt.SetText(string.Format("Press E to pickup <color=#a760ec>{0}</color>", closestTarget.GetComponent<Item>().itemName));
                                break;
                            case Item.ItemRarity.Masterwork:
                                interactPrompt.SetText(string.Format("Press E to pickup <color=#f80f41>{0}</color>", closestTarget.GetComponent<Item>().itemName));
                                break;
                            default:
                                break;
                        }
                    }
                    else if (closestTarget.GetComponent<DoorOpenVolumeBehaviour>() != null && closestTarget.GetComponentInParent<DoorBehaviour>().doorState == DoorBehaviour.DoorState.Closed)
                        interactPrompt.SetText("Press E to open door");
                    else if (closestTarget.GetComponent<DoorOpenVolumeBehaviour>() != null && closestTarget.GetComponentInParent<DoorBehaviour>().doorState != DoorBehaviour.DoorState.Closed)
                        interactPrompt.SetText("Press E to close door");
                    else if (closestTarget.transform.root.GetComponent<TeleporterBehaviour>() != null && closestTarget.transform.root.GetComponent<TeleporterBehaviour>().teleporterActive)
                        interactPrompt.SetText("Press E to teleport");
                    else if (closestTarget.GetComponent<ArtifactBehaviour>() != null)
                        interactPrompt.SetText("Press E to gather this Artifact");
                }
                else
                    interactPrompt.SetText("");
            }
            yield return new WaitForEndOfFrame();
        }
    }
}
