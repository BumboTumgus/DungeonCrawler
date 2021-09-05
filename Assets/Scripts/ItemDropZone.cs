using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public enum SlotType { Inventory, Trinket, Weapon, Helmet, Armor, Leggings, Skill, DropItem}
    public SlotType slotType;

    public InventoryPopupTextManager.PopUpDirection popUpDirection;

    // This is solely used for the left and right hands in case we equip a 2 handed weapon.
    public ItemDropZone connectedSlot;
    public int slotIndex;

    private InventoryPopupTextManager popupManager;

    private void Start()
    {
        popupManager = transform.parent.GetComponent<InventoryPopupTextManager>();
    }

    // Used when the pointer enters the dropzone, we check to see if there is an atatched drag item to it.
    public void OnPointerEnter(PointerEventData eventData)
    {
        // If we have an object connected and this isa new slot we are hovering over, start the stat checker.
        if (eventData.pointerDrag != null)
        {
            ItemDraggable movedItem = eventData.pointerDrag.GetComponent<ItemDraggable>();
            Item.ItemType currentItemType = movedItem.attachedItem.GetComponent<Item>().itemType;

            bool addStats = true;
            bool suitableSlot = false;
            switch (slotType)
            {
                case SlotType.Inventory:
                    if (movedItem.myParent.GetComponent<ItemDropZone>().slotType != slotType)
                    {
                        //Debug.Log("checking the panel type");
                        // if there is no item OR the item matches our type OR the item is a weapon when trying to move a 2h weapon or 1 h weapon show the stats, also check to amek sure the item is not a skill.
                        if (transform.Find("ItemPanel").GetComponent<ItemDraggable>().attachedItem == null 
                            || transform.Find("ItemPanel").GetComponent<ItemDraggable>().attachedItem.GetComponent<Item>().itemType == currentItemType
                            || (transform.Find("ItemPanel").GetComponent<ItemDraggable>().attachedItem.GetComponent<Item>().itemType == Item.ItemType.TwoHandWeapon && currentItemType == Item.ItemType.Weapon)
                            || (transform.Find("ItemPanel").GetComponent<ItemDraggable>().attachedItem.GetComponent<Item>().itemType == Item.ItemType.Weapon && currentItemType == Item.ItemType.TwoHandWeapon))
                        {
                            //Debug.Log("the item is not null and equals this item's item type, or this item is a weapon on a two hand weapon.");
                            suitableSlot = true;
                            addStats = false;
                        }
                    }
                    break;
                case SlotType.Trinket:
                    if ((currentItemType == Item.ItemType.TrinketRing || currentItemType == Item.ItemType.TrinketBracelet || currentItemType == Item.ItemType.TrinketCape || currentItemType == Item.ItemType.TrinketWaistItem) && movedItem.myParent.GetComponent<ItemDropZone>().slotType != SlotType.Trinket)
                        suitableSlot = true;
                    break;
                case SlotType.Weapon:
                    // If we have a weapon, and this weapon is not coming from anopther weapon slot and there is not a 2h hand weapon ion the other connected slot, show the stats
                    if ((currentItemType == Item.ItemType.Weapon || currentItemType == Item.ItemType.TwoHandWeapon || currentItemType == Item.ItemType.Shield || currentItemType == Item.ItemType.MagicBooster) && movedItem.myParent.GetComponent<ItemDropZone>().slotType != SlotType.Weapon)
                    {
                        //Debug.Log(" we are moving a weapon over the weapon slot");
                        suitableSlot = true;
                        if (transform.Find("ItemPanel").GetComponent<ItemDraggable>().attachedItem == null
                            && connectedSlot.transform.Find("ItemPanel").GetComponent<ItemDraggable>().attachedItem != null
                            && connectedSlot.transform.Find("ItemPanel").GetComponent<ItemDraggable>().attachedItem.GetComponent<Item>().itemType == Item.ItemType.TwoHandWeapon
                            && currentItemType != Item.ItemType.TwoHandWeapon)
                            suitableSlot = false;
                        // here we will check if we have two weapons to replace with a two hand weapon, if we do, add stats is then false.
                        else if (currentItemType == Item.ItemType.TwoHandWeapon && transform.Find("ItemPanel").GetComponent<ItemDraggable>().attachedItem != null && connectedSlot.transform.Find("ItemPanel").GetComponent<ItemDraggable>().attachedItem != null)
                            addStats = false;
                    }

                       
                    break;
                case SlotType.Helmet:
                    if (currentItemType == Item.ItemType.Helmet)
                        suitableSlot = true;
                    break;
                case SlotType.Armor:
                    if (currentItemType == Item.ItemType.Armor)
                        suitableSlot = true;
                    break;
                case SlotType.Leggings:
                    if (currentItemType == Item.ItemType.Legs)
                        suitableSlot = true;
                    break;
                case SlotType.Skill:
                    suitableSlot = false;
                    break;
                case SlotType.DropItem:
                    suitableSlot = false;
                    break;
                default:
                    break;
            }

            //Debug.Log("suitable slot is currently: " + suitableSlot + ". add stats is currently : " + addStats);
            if (suitableSlot && movedItem.myParent != gameObject.transform && movedItem.attachedItem.GetComponent<Item>().itemType != Item.ItemType.Skill)
            {
                Transform myPanel = transform.Find("ItemPanel");
                ItemDraggable dropZoneItem = myPanel.GetComponent<ItemDraggable>();

                Item[] previousItems = new Item[2];
                if (dropZoneItem.attachedItem != null)
                    previousItems[0] = dropZoneItem.attachedItem.GetComponent<Item>();

                if(addStats)
                    transform.parent.GetComponent<InventoryUiManager>().playerInventory.GetComponent<PlayerStats>().CheckStatChange(movedItem.attachedItem.GetComponent<Item>(), previousItems);
                else
                {
                    previousItems = new Item[2];
                    previousItems[0] = movedItem.attachedItem.GetComponent<Item>();

                    //Debug.Log(" the logic has reached here");
                    // we see if the other item exists, if it does we set it as the item we want to add the stats from, else we dont add stats from an item.
                    if(transform.Find("ItemPanel").GetComponent<ItemDraggable>().attachedItem == null)
                        transform.parent.GetComponent<InventoryUiManager>().playerInventory.GetComponent<PlayerStats>().CheckStatChange(null, previousItems);
                    else
                    {
                        //Debug.Log("the item is not null");
                        // set items to remove as just our item, unless we are hovering over a 2h weapon.
                        if (myPanel.GetComponent<ItemDraggable>().attachedItem.GetComponent<Item>().itemType == Item.ItemType.TwoHandWeapon)
                        {
                            // add the off hand item to the previosu items to remvoe for the stat calculationss if it exists.
                            if(movedItem.myParent.GetComponent<ItemDropZone>().connectedSlot.transform.Find("ItemPanel").GetComponent<ItemDraggable>().attachedItem != null)
                                previousItems[1] = movedItem.myParent.GetComponent<ItemDropZone>().connectedSlot.transform.Find("ItemPanel").GetComponent<ItemDraggable>().attachedItem.GetComponent<Item>();
                            //Debug.Log("This is a tweo ahnd weapon weare hovering over");
                        }
                        // This is the case that a 2h weapon is hovering over a weapon slot and we have 2 weapons equipped.
                        else if(slotType == SlotType.Weapon && dropZoneItem.attachedItem != null && connectedSlot.transform.Find("ItemPanel").GetComponent<ItemDraggable>().attachedItem != null)
                        {
                            //Debug.Log("we are hovering over a 1h weapon with a 2h weapon while there are two equipped weapons at once.");
                            previousItems[0] = dropZoneItem.attachedItem.GetComponent<Item>();
                            previousItems[1] = connectedSlot.transform.Find("ItemPanel").GetComponent<ItemDraggable>().attachedItem.GetComponent<Item>();
                        }
                        if (slotType == SlotType.Weapon)
                        {
                            //Debug.Log("This is a weapon slot");
                            transform.parent.GetComponent<InventoryUiManager>().playerInventory.GetComponent<PlayerStats>().CheckStatChange(movedItem.attachedItem.GetComponent<Item>(), previousItems);
                        }
                        else
                            transform.parent.GetComponent<InventoryUiManager>().playerInventory.GetComponent<PlayerStats>().CheckStatChange(myPanel.GetComponent<ItemDraggable>().attachedItem.GetComponent<Item>(), previousItems);
                    }
                }
                //Debug.Log("A pointer with an attached object has entered a non inventory slot.");
            }

            popupManager.ShowPopup(popupManager.itemPopUp.transform.parent, popUpDirection);
        }
    }

    // USed when a pointer exits the dropzone.
    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            if (eventData.pointerDrag != null)
            {
                ItemDraggable movedItem = eventData.pointerDrag.GetComponent<ItemDraggable>();
                Item.ItemType currentItemType = movedItem.attachedItem.GetComponent<Item>().itemType;

                bool suitableSlot = false;
                switch (slotType)
                {
                    case SlotType.Inventory:
                        // This only happens if the moved item came from any other slot but an inventory slot.
                        if(movedItem.myParent.GetComponent<ItemDropZone>().slotType != slotType)
                        {
                            // This is originally set as null, however if we find an item it gets reset to that item.
                            Item myItem = null;
                            if (transform.Find("ItemPanel").GetComponent<ItemDraggable>().attachedItem != null)
                                myItem = transform.Find("ItemPanel").GetComponent<ItemDraggable>().attachedItem.GetComponent<Item>();

                            // If we matched the item, then we were showing stats and have to force a recheck.
                            if (transform.Find("ItemPanel").GetComponent<ItemDraggable>().attachedItem == null || myItem.itemType == currentItemType)
                                suitableSlot = true;
                            else if ((myItem.itemType == Item.ItemType.Weapon && movedItem.attachedItem.GetComponent<Item>().itemType == Item.ItemType.TwoHandWeapon) ||
                               (myItem.itemType == Item.ItemType.TwoHandWeapon && movedItem.attachedItem.GetComponent<Item>().itemType == Item.ItemType.Weapon))
                                 {
                                 suitableSlot = true;
                                 //Debug.Log("we have lef tthe hover zone while holding a 2 hand weapon oir 1 hand weapon on the other");
                                 }
                        }
                        break;
                    case SlotType.Trinket:
                        if (currentItemType == Item.ItemType.TrinketWaistItem || currentItemType == Item.ItemType.TrinketRing || currentItemType == Item.ItemType.TrinketBracelet || currentItemType == Item.ItemType.TrinketCape)
                            suitableSlot = true;
                        break;
                    case SlotType.Weapon:
                        if (currentItemType == Item.ItemType.Weapon || currentItemType == Item.ItemType.TwoHandWeapon || currentItemType == Item.ItemType.Shield || currentItemType == Item.ItemType.MagicBooster)
                            suitableSlot = true;
                        break;
                    case SlotType.Helmet:
                        if (currentItemType == Item.ItemType.Helmet)
                            suitableSlot = true;
                        break;
                    case SlotType.Armor:
                        if (currentItemType == Item.ItemType.Armor)
                            suitableSlot = true;
                        break;
                    case SlotType.Leggings:
                        if (currentItemType == Item.ItemType.Legs)
                            suitableSlot = true;
                        break;
                    case SlotType.Skill:
                        suitableSlot = false;
                        break;
                    case SlotType.DropItem:
                        suitableSlot = false;
                        break;
                    default:
                        break;
                }

                if (suitableSlot && movedItem.myParent != gameObject.transform)
                {
                    transform.parent.GetComponent<InventoryUiManager>().playerInventory.GetComponent<PlayerStats>().ForceStatRecheck();
                   // Debug.Log(" a pointer with an atatched object ahs left.");
                }
            }
        }
    }

    // This is used to see if the item was dropped on an appropriate slot.
    public void OnDrop(PointerEventData eventData)
    {
        //Debug.Log("item was dropped on " + gameObject.name);

        ItemDraggable movedItem = eventData.pointerDrag.GetComponent<ItemDraggable>();
        if(movedItem != null && movedItem.myParent != gameObject.transform)
        {

            Transform myPanel = transform.Find("ItemPanel");
            ItemDraggable dropZoneItem = null;

            if(slotType != SlotType.DropItem)
                dropZoneItem = myPanel.GetComponent<ItemDraggable>();

            // First we check if the slot has a type of item that is needed to be dragged in.
            bool validTarget = false;
            switch (slotType)
            {
                case SlotType.Inventory:
                    validTarget = true;
                    break;
                case SlotType.Trinket:
                    if (movedItem.GetComponent<ItemDraggable>().attachedItem.GetComponent<Item>().itemType == Item.ItemType.TrinketWaistItem ||
                        movedItem.GetComponent<ItemDraggable>().attachedItem.GetComponent<Item>().itemType == Item.ItemType.TrinketRing ||
                        movedItem.GetComponent<ItemDraggable>().attachedItem.GetComponent<Item>().itemType == Item.ItemType.TrinketBracelet ||
                        movedItem.GetComponent<ItemDraggable>().attachedItem.GetComponent<Item>().itemType == Item.ItemType.TrinketCape)
                        validTarget = true;
                    break;
                case SlotType.Weapon:
                    if (movedItem.GetComponent<ItemDraggable>().attachedItem.GetComponent<Item>().itemType == Item.ItemType.Weapon ||
                        movedItem.GetComponent<ItemDraggable>().attachedItem.GetComponent<Item>().itemType == Item.ItemType.TwoHandWeapon ||
                        movedItem.GetComponent<ItemDraggable>().attachedItem.GetComponent<Item>().itemType == Item.ItemType.Shield ||
                        movedItem.GetComponent<ItemDraggable>().attachedItem.GetComponent<Item>().itemType == Item.ItemType.MagicBooster)
                        validTarget = true;
                    break;
                case SlotType.Helmet:
                    if (movedItem.GetComponent<ItemDraggable>().attachedItem.GetComponent<Item>().itemType == Item.ItemType.Helmet)
                        validTarget = true;
                    break;
                case SlotType.Armor:
                    if (movedItem.GetComponent<ItemDraggable>().attachedItem.GetComponent<Item>().itemType == Item.ItemType.Armor)
                        validTarget = true;
                    break;
                case SlotType.Leggings:
                    if (movedItem.GetComponent<ItemDraggable>().attachedItem.GetComponent<Item>().itemType == Item.ItemType.Legs)
                        validTarget = true;
                    break;
                case SlotType.Skill:
                    if (movedItem.GetComponent<ItemDraggable>().attachedItem.GetComponent<Item>().itemType == Item.ItemType.Skill)
                        validTarget = true;
                    break;
                case SlotType.DropItem:
                    validTarget = false;
                    break;
                default:
                    break;
            }

            // This is only called when we try to transfer a 2 handed weapon over t a weapon slot. We need to check if we have two items in our hand slots and if we have room to
            // transfer them over to our inventory.
            if (movedItem.GetComponent<ItemDraggable>().attachedItem.GetComponent<Item>().itemType == Item.ItemType.TwoHandWeapon && slotType == SlotType.Weapon)
            {
                if(myPanel.GetComponent<ItemDraggable>().attachedItem != null && connectedSlot.transform.Find("ItemPanel").GetComponent<ItemDraggable>().attachedItem != null)
                {
                    Debug.Log("TWO HAND WEAPON with two items in the slots");
                    // Check to see if we have two slots open in our inventory, remove an extra because the weapon has not been removed from the ivnentory yet.
                    if (transform.parent.GetComponent<InventoryUiManager>().playerInventory.inventory.Count <= transform.parent.GetComponent<InventoryUiManager>().playerInventory.INVENTORY_MAX - 1)
                    {
                        // First we need to find this empty inventory slot at the closest index.
                        ItemDraggable emptyInventorySlot = transform.parent.GetComponent<InventoryUiManager>().GetNextEmptySlot();
                        // We have the slots for the items, we will transfer the one in the left hand slot to the empty slot in the inventory because the weapon will be replcaing the one on the right.
                        if(slotIndex == 0)
                        {
                            ItemDraggable connectedPanel = connectedSlot.transform.Find("ItemPanel").GetComponent<ItemDraggable>();

                            // First we need to switch the indexs of both items
                            connectedPanel.attachedItem.GetComponent<Item>().inventoryIndex = emptyInventorySlot.transform.parent.GetComponent<ItemDropZone>().slotIndex;
                            // Transfer this item into the inventory
                            //Debug.Log("the trasnfered item should be the " + connectedPanel.attachedItem.GetComponent<Item>());
                            transform.parent.GetComponent<InventoryUiManager>().playerInventory.TransferItem(connectedPanel.attachedItem.GetComponent<Item>(), slotType, emptyInventorySlot.transform.parent.GetComponent<ItemDropZone>().slotType);

                            Transform connectPanelParent = connectedPanel.transform.parent;
                            connectedPanel.transform.SetParent(emptyInventorySlot.transform.parent);
                            connectedPanel.transform.localPosition = Vector3.zero;
                            emptyInventorySlot.transform.SetParent(connectPanelParent);
                            emptyInventorySlot.transform.localPosition = Vector3.zero;
                        }
                        else
                        {
                            ItemDraggable localPanel = transform.Find("ItemPanel").GetComponent<ItemDraggable>();

                            // First we need to switch the indexs of both items
                            localPanel.attachedItem.GetComponent<Item>().inventoryIndex = emptyInventorySlot.transform.parent.GetComponent<ItemDropZone>().slotIndex;
                            // Transfer this item into the inventory
                            transform.parent.GetComponent<InventoryUiManager>().playerInventory.TransferItem(localPanel.attachedItem.GetComponent<Item>(), slotType, emptyInventorySlot.transform.parent.GetComponent<ItemDropZone>().slotType);
                            
                            Transform connectPanelParent = localPanel.transform.parent;
                            localPanel.transform.SetParent(emptyInventorySlot.transform.parent);
                            localPanel.transform.localPosition = Vector3.zero;
                            emptyInventorySlot.transform.SetParent(connectPanelParent);
                            emptyInventorySlot.transform.localPosition = Vector3.zero;
                        }
                    }
                    else
                       validTarget = false;
                }
            }

            // USed to see if we are moving a weapon from a weapon slot onto an itme that may not be valid to switch places with. example, gold;
            if (movedItem.myParent.GetComponent<ItemDropZone>().slotType == ItemDropZone.SlotType.Weapon)
            {
                if (dropZoneItem.attachedItem != null)
                {
                    if (dropZoneItem.attachedItem.GetComponent<Item>().itemType == Item.ItemType.Weapon || dropZoneItem.attachedItem.GetComponent<Item>().itemType == Item.ItemType.TwoHandWeapon || dropZoneItem.attachedItem.GetComponent<Item>().itemType == Item.ItemType.MagicBooster || dropZoneItem.attachedItem.GetComponent<Item>().itemType == Item.ItemType.Shield)
                    {
                        // Debug.Log("were clear to move");
                    }
                    else
                    {
                        // Now we will check to see if t he other item is null, if it is, we can move over this item.
                        validTarget = false;
                        //Debug.Log("The moved item is from a weapon slot and the other item is not a weapon");
                    }
                    // If we placed a 2h weapon on a 1h weapon, we need to see if we have enough room for both items to move and be unequipped if we two items equipped.
                    if (dropZoneItem.attachedItem.GetComponent<Item>().itemType == Item.ItemType.TwoHandWeapon && movedItem.attachedItem.GetComponent<Item>().itemType == Item.ItemType.Weapon && 
                        movedItem.myParent.GetComponent<ItemDropZone>().connectedSlot.transform.Find("ItemPanel").GetComponent<ItemDraggable>().attachedItem != null)
                    {
                        if (transform.parent.GetComponent<InventoryUiManager>().playerInventory.inventory.Count <= transform.parent.GetComponent<InventoryUiManager>().playerInventory.INVENTORY_MAX - 1)
                        {
                            //Debug.Log(" we have room and will commence the transition of the secondary item.");

                            // First we need to find this empty inventory slot at the closest index and the item in the other hand we need to remove.
                            ItemDraggable emptyInventorySlot = transform.parent.GetComponent<InventoryUiManager>().GetNextEmptySlot();
                            ItemDraggable targetItemToShift = movedItem.myParent.GetComponent<ItemDropZone>().connectedSlot.transform.Find("ItemPanel").GetComponent<ItemDraggable>();
                            
                            // First we need to switch the indexs of both items
                            targetItemToShift.attachedItem.GetComponent<Item>().inventoryIndex = emptyInventorySlot.transform.parent.GetComponent<ItemDropZone>().slotIndex;
                            // Transfer this item into the inventory
                            //Debug.Log("the trasnfered item should be the " + targetItemToShift.attachedItem.GetComponent<Item>());
                            transform.parent.GetComponent<InventoryUiManager>().playerInventory.TransferItem(targetItemToShift.attachedItem.GetComponent<Item>(), targetItemToShift.transform.parent.GetComponent<ItemDropZone>().slotType, emptyInventorySlot.transform.parent.GetComponent<ItemDropZone>().slotType);

                            Transform connectPanelParent = targetItemToShift.transform.parent;
                            targetItemToShift.transform.SetParent(emptyInventorySlot.transform.parent);
                            targetItemToShift.transform.localPosition = Vector3.zero;
                            emptyInventorySlot.transform.SetParent(connectPanelParent);
                            emptyInventorySlot.transform.localPosition = Vector3.zero;
                        }
                        else
                            validTarget = false;
                    }
                }
            }

            // Used to see if placing the one handed weapon in this slot is a valid move.
            if(movedItem.attachedItem.GetComponent<Item>().itemType == Item.ItemType.Weapon && slotType == SlotType.Weapon && movedItem.myParent.gameObject != connectedSlot.gameObject)
            {
                // If this is the left hand slot, we can check to see if there is a two handed weapon in the right handed slot.
                if(slotIndex == 1)
                {
                    if(connectedSlot.transform.Find("ItemPanel").GetComponent<ItemDraggable>().attachedItem != null && connectedSlot.transform.Find("ItemPanel").GetComponent<ItemDraggable>().attachedItem.GetComponent<Item>().itemType == Item.ItemType.TwoHandWeapon)
                    {
                        //Debug.Log("There is a two handed weapon in the other slot");
                        validTarget = false;
                    }
                }
            }

            // Check if this is a right hand weapon being moved into the left hand slot, and there is no weapon here, it is an invalid movemenet so cancel it.
            if (movedItem.myParent.GetComponent<ItemDropZone>().slotType == SlotType.Weapon && movedItem.myParent.GetComponent<ItemDropZone>().slotIndex == 0 && slotType == SlotType.Weapon && dropZoneItem.attachedItem == null)
                validTarget = false;


            // If we dropped an item on the dropitem type slot, return the item and wipe the slot then drop the item.
            if(slotType == SlotType.DropItem && movedItem.myParent.GetComponent<ItemDropZone>().slotType == SlotType.Inventory)
            {
                Debug.Log("we should drop the item here");
                popupManager.lockPointer = false;
                movedItem.transform.SetParent(movedItem.myParent);
                popupManager.HidePopups();

                movedItem.transform.localPosition = Vector3.zero;
                movedItem.parentToInteractWith = null;
                movedItem.myParent = transform.parent;
                movedItem.GetComponent<CanvasGroup>().blocksRaycasts = true;

                transform.parent.GetComponent<InventoryUiManager>().playerInventory.DropItem(movedItem.attachedItem.GetComponent<Item>().inventoryIndex);
            }

            // If we transfer a shield or magic booster to the left hand while we have a two handed weapon in the right slot, this isnt valid.
            if (slotType == SlotType.Weapon && slotIndex == 1 && connectedSlot.transform.Find("ItemPanel").GetComponent<ItemDraggable>().attachedItem != null && connectedSlot.transform.Find("ItemPanel").GetComponent<ItemDraggable>().attachedItem.GetComponent<Item>().itemType == Item.ItemType.TwoHandWeapon)
            {
                Debug.Log("not a valid choice");
                validTarget = false;
            }

            // WE HAVE A VALID TARGET BEGIN SHIFTING IT OVER BELOW
            // If the target was valid, beign our replacement or move logic.
            if (validTarget)
            {
                // This is called if we try to add a weapon to our left hand before there is one in the right hand
                if (slotType == SlotType.Weapon && slotIndex == 1 && transform.parent.GetComponent<InventoryUiManager>().playerInventory.weapons.Count < 2 && (movedItem.attachedItem.GetComponent<Item>().itemType == Item.ItemType.TwoHandWeapon || movedItem.attachedItem.GetComponent<Item>().itemType == Item.ItemType.Weapon || movedItem.attachedItem.GetComponent<Item>().itemType == Item.ItemType.Shield))
                {
                    // If we tried to put a weapon on the left hand and there is nothing on the right hand this weapon will be placed on the right hand instead.
                    // First we check if the connected slot's panel is null, if so then we are switching our items from left to right.
                    Debug.Log("a weapon was dropped on the left side and shhould be switched to the right side.");
                    if (connectedSlot.transform.Find("ItemPanel").GetComponent<ItemDraggable>() != null && connectedSlot.transform.Find("ItemPanel").GetComponent<ItemDraggable>().attachedItem == null)
                    {
                        //Debug.Log(" we have swithced the panels and the objects interacting with shit");
                        myPanel = connectedSlot.transform.Find("ItemPanel");
                        dropZoneItem = myPanel.GetComponent<ItemDraggable>();
                        //Debug.Log("myPanel is currently: " + myPanel.name + ". The dropzone item is: " + dropZoneItem);
                    }
                    else if(movedItem.attachedItem.GetComponent<Item>().itemType == Item.ItemType.TwoHandWeapon && connectedSlot.transform.Find("ItemPanel").GetComponent<ItemDraggable>().attachedItem != null)
                    {
                        Debug.Log("a two handed weapon was dropped on the left side so we have switched the panels it is interacting with.");
                        myPanel = connectedSlot.transform.Find("ItemPanel");
                        dropZoneItem = myPanel.GetComponent<ItemDraggable>();
                        //Debug.Log("myPanel is currently: " + myPanel.name + ". The dropzone item is: " + dropZoneItem);
                    }
                }
                /*
                // This is called if we try to add a shield to our right hand before there is one in the left hand
                if (slotType == SlotType.Weapon && slotIndex == 0 && transform.parent.GetComponent<InventoryUiManager>().playerInventory.weapons.Count < 2 && (movedItem.attachedItem.GetComponent<Item>().itemType == Item.ItemType.Shield || movedItem.attachedItem.GetComponent<Item>().itemType == Item.ItemType.MagicBooster))
                {
                    // If we tried to put a shield or magic booster on the right hand and there is nothing on the left hand this shield or magic booster will be placed on the left hand instead.
                    // First we check if the connected slot's panel is null, if so then we are switching our items from right to left.
                    Debug.Log("a shield was dropped on the right side and shhould be switched to the left side.");
                    if (connectedSlot.transform.Find("ItemPanel").GetComponent<ItemDraggable>() != null && connectedSlot.transform.Find("ItemPanel").GetComponent<ItemDraggable>().attachedItem == null)
                    {
                        Debug.Log(" we have swithced the panels for the SHIELD and the objects interacting with shit");
                        myPanel = connectedSlot.transform.Find("ItemPanel");
                        dropZoneItem = myPanel.GetComponent<ItemDraggable>();
                        Debug.Log("myPanel is currently: " + myPanel.name + ". The dropzone item is: " + dropZoneItem);
                    }
                    Debug.Log("here we should check if we have a 2h weapon on the right, if we do unequip it.");
                }
                */


                // make the items switch their indexes (case only works for two items)
                if (dropZoneItem.attachedItem != null)
                {
                    int dropZoneItemIndex = dropZoneItem.attachedItem.GetComponent<Item>().inventoryIndex;
                    dropZoneItem.attachedItem.GetComponent<Item>().inventoryIndex = movedItem.attachedItem.GetComponent<Item>().inventoryIndex;
                    movedItem.attachedItem.GetComponent<Item>().inventoryIndex = dropZoneItemIndex;
                }
                // The logic for when there is no itemn on this panel when we slide an item over.
                else
                    movedItem.attachedItem.GetComponent<Item>().inventoryIndex = slotIndex;


                // Here we need to change the actual physical item from our inventory list to our equipment, if applicable.
                SlotType otherSlotType = movedItem.myParent.GetComponent<ItemDropZone>().slotType;
                if (slotType != otherSlotType)
                {
                    // Switch weapons to right hand prio
                    if (slotType == SlotType.Weapon && slotIndex == 1 && transform.parent.GetComponent<InventoryUiManager>().playerInventory.weapons.Count < 1 && (movedItem.attachedItem.GetComponent<Item>().itemType == Item.ItemType.TwoHandWeapon || movedItem.attachedItem.GetComponent<Item>().itemType == Item.ItemType.Weapon || movedItem.attachedItem.GetComponent<Item>().itemType == Item.ItemType.Shield))
                        movedItem.attachedItem.GetComponent<Item>().equippedToRightHand = true;
                    // Switch shields to left hand prio if we have no other items, OR we only have only one 1 handed item in our weapon slots.
                    //else if (slotType == SlotType.Weapon && slotIndex == 0 && transform.parent.GetComponent<InventoryUiManager>().playerInventory.weapons.Count < 1 && (movedItem.attachedItem.GetComponent<Item>().itemType == Item.ItemType.Shield || movedItem.attachedItem.GetComponent<Item>().itemType == Item.ItemType.MagicBooster))
                        //movedItem.attachedItem.GetComponent<Item>().equippedToRightHand = false;
                    else
                    {
                        if (slotType == SlotType.Weapon && slotIndex == 0)
                            movedItem.attachedItem.GetComponent<Item>().equippedToRightHand = true;
                        else if (slotType == SlotType.Weapon && slotIndex == 1)
                            movedItem.attachedItem.GetComponent<Item>().equippedToRightHand = false;
                    }
                    

                    if (dropZoneItem.attachedItem != null)
                    {
                        // This is set for the gear mamanegr to know this item is being equiped to the right hand weapon slot. This is not called in the case of a weapon being dropped in the left hand slot then shifted to the right.
                        if (otherSlotType == SlotType.Weapon && movedItem.myParent.GetComponent<ItemDropZone>().slotIndex == 0)
                            dropZoneItem.attachedItem.GetComponent<Item>().equippedToRightHand = true;
                        else if (otherSlotType == SlotType.Weapon && movedItem.myParent.GetComponent<ItemDropZone>().slotIndex == 1)
                            dropZoneItem.attachedItem.GetComponent<Item>().equippedToRightHand = false;

                        transform.parent.GetComponent<InventoryUiManager>().playerInventory.TransferItem(dropZoneItem.attachedItem.GetComponent<Item>(), slotType, otherSlotType);
                    }

                    //Debug.Log("The item is being transfered and equipped here");
                    transform.parent.GetComponent<InventoryUiManager>().playerInventory.TransferItem(movedItem.attachedItem.GetComponent<Item>(), otherSlotType, slotType);
                }



                bool parentSetOverride = false;
                // Check to see if this was the right hand item slot, if its now empty, and if theres something in the left hand. If so slide it over to here.
                if (movedItem.myParent.GetComponent<ItemDropZone>().slotType == SlotType.Weapon && movedItem.myParent.GetComponent<ItemDropZone>().slotIndex == 0)
                {
                    // Check for the left hand item. This is done by checking how many weapons we have left, it should be 1. If it is one and we are switching in a two hander, this is because we removed the off hand item earlier.
                    if (transform.parent.GetComponent<InventoryUiManager>().playerInventory.weapons.Count == 1 && (myPanel.GetComponent<ItemDraggable>().attachedItem == null || myPanel.GetComponent<ItemDraggable>().attachedItem.GetComponent<Item>().itemType != Item.ItemType.TwoHandWeapon))
                    {
                        //Debug.Log("The left item will be slid over");
                        ItemDraggable leftHandItemDraggable = movedItem.myParent.GetComponent<ItemDropZone>().connectedSlot.transform.Find("ItemPanel").GetComponent<ItemDraggable>();
                        Debug.Log(leftHandItemDraggable.attachedItem.name);

                        // Here we will move the left hand item to the right hand one and switch it's indexs with the right hand one (which is just an empty item).
                        leftHandItemDraggable.attachedItem.GetComponent<Item>().inventoryIndex = 0;

                        parentSetOverride = true;
                        movedItem.parentToInteractWith = myPanel.parent;
                        
                        myPanel.SetParent(leftHandItemDraggable.myParent);
                        myPanel.localPosition = Vector3.zero;

                        leftHandItemDraggable.transform.SetParent(movedItem.myParent);
                        leftHandItemDraggable.transform.localPosition = Vector3.zero;

                        transform.parent.GetComponent<InventoryUiManager>().playerInventory.SwitchHands(leftHandItemDraggable.attachedItem.GetComponent<Item>(), null);
                    }
                }



                // This case is for when we are switching froma  weapon slot to a weapon slot, we just need to call the switchhands method twice.
                if(movedItem.myParent.GetComponent<ItemDropZone>().slotType == SlotType.Weapon && slotType == SlotType.Weapon)
                {
                    if(dropZoneItem.attachedItem != null)
                    {
                        //Debug.Log("I am going to swithc the weapons around here.");
                        transform.parent.GetComponent<InventoryUiManager>().playerInventory.SwitchHands(movedItem.attachedItem.GetComponent<Item>(), dropZoneItem.attachedItem.GetComponent<Item>());
                    }
                }



                // This is used to equip and unequip the varying skills the player will equip.
                if(slotType == SlotType.Skill || otherSlotType == SlotType.Skill)
                {
                    //Debug.Log("Here i would equip the skills by connecting it to the skillsmanager in the inventory ui manager");

                    // There are three cases here, they both are skills slots, or one or the other is a skill slot.
                    if (slotType == SlotType.Skill && otherSlotType == SlotType.Skill)
                    {
                        Debug.Log("Skill to Skill");

                        if(dropZoneItem.attachedItem != null)
                            transform.parent.GetComponent<InventoryUiManager>().playerSkills.SwapSkills(movedItem.myParent.GetComponent<ItemDropZone>().slotIndex, movedItem.attachedItem.GetComponent<Item>().skillName, dropZoneItem.transform.parent.GetComponent<ItemDropZone>().slotIndex, dropZoneItem.attachedItem.GetComponent<Item>().skillName);
                        else
                            transform.parent.GetComponent<InventoryUiManager>().playerSkills.SwapSkills(movedItem.myParent.GetComponent<ItemDropZone>().slotIndex, movedItem.attachedItem.GetComponent<Item>().skillName, slotIndex);

                        /*
                        transform.parent.GetComponent<InventoryUiManager>().playerSkills.AddSkill(slotIndex, movedItem.attachedItem.GetComponent<Item>().skillName);

                        // There will always be an item that is being moved, but we need to check if the other panel has an item attached.
                        if (dropZoneItem.attachedItem != null)
                        {
                            // There is an item here
                            transform.parent.GetComponent<InventoryUiManager>().playerSkills.AddSkill(movedItem.myParent.GetComponent<ItemDropZone>().slotIndex, dropZoneItem.attachedItem.GetComponent<Item>().skillName);
                        }
                        else
                            // There is no item, so we need to remove this skill from the first index.
                            transform.parent.GetComponent<InventoryUiManager>().playerSkills.RemoveSkill(movedItem.myParent.GetComponent<ItemDropZone>().slotIndex);
                        */
                    }

                    // This is the logic if we are moving from a full skill slot the the ivnentory, where we remove the skill.
                    else if (otherSlotType == SlotType.Skill)
                    {
                        Debug.Log("Skill to invenotry");
                        // If the inventory slot is blank.
                        if (dropZoneItem.attachedItem == null)
                            transform.parent.GetComponent<InventoryUiManager>().playerSkills.RemoveSkill(movedItem.myParent.GetComponent<ItemDropZone>().slotIndex);
                        else
                            transform.parent.GetComponent<InventoryUiManager>().playerSkills.AddSkill(movedItem.myParent.GetComponent<ItemDropZone>().slotIndex, dropZoneItem.attachedItem.GetComponent<Item>().skillName);
                    }

                    // This is the logic if we came from the iventory to the skill slots.
                    else if (slotType == SlotType.Skill)
                    {
                        //Debug.Log("invenotry to skill");
                        //Debug.Log(dropZoneItem);
                        transform.parent.GetComponent<InventoryUiManager>().playerSkills.AddSkill(dropZoneItem.transform.parent.GetComponent<ItemDropZone>().slotIndex, movedItem.attachedItem.GetComponent<Item>().skillName);
                    }
                }

                // Set the object at the target (here) to the parent of the object the player is moving.
                if (!parentSetOverride)
                {
                    movedItem.parentToInteractWith = myPanel.parent;

                    myPanel.SetParent(movedItem.myParent);
                    myPanel.localPosition = Vector3.zero;
                }

                popupManager.HidePopups();
            }
        }
    }
}
