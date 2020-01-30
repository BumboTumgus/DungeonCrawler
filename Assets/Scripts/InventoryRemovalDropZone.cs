using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryRemovalDropZone : MonoBehaviour, IDropHandler
{
    public Inventory inventory;
    public InventoryUiManager inventoryUI;

    private InventoryPopupTextManager popupManager;
    
    private void Start()
    {
        inventoryUI = transform.parent.GetComponent<InventoryUiManager>();
        popupManager = transform.parent.GetComponent<InventoryPopupTextManager>();
    }

    // This is used to see if the item was dropped on an appropriate slot.
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("item was dropped on " + gameObject.name);

        ItemDraggable movedItem = eventData.pointerDrag.GetComponent<ItemDraggable>();
        // Remove the item frok the players inventory.
        //inventoryUI.WipeSlot(movedItem.transform.parent.gameObject);
        movedItem.transform.SetParent(movedItem.myParent);
        movedItem.transform.localPosition = Vector3.zero;
        movedItem.GetComponent<CanvasGroup>().blocksRaycasts = true;
        inventory.DropItem(movedItem.attachedItem.GetComponent<Item>().inventoryIndex);

        popupManager.lockPointer = false;
        popupManager.HidePopups();
    }
}
