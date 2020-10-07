using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDraggable : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Transform parentToInteractWith = null;
    public Transform myParent = null;

    public GameObject attachedItem;

    private InventoryPopupTextManager popupManager;

    private void Start()
    {
        popupManager = transform.parent.parent.GetComponent<InventoryPopupTextManager>();
    }

    // When we click and start dragging this dude around, set our parent and our parent to return to.
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("On Begin Drag");

        parentToInteractWith = null;
        myParent = transform.parent;

        transform.SetParent(transform.parent.parent);
        transform.SetAsLastSibling();

        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    // While moving around, set our position to the mouse so we follow it.
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
        popupManager.lockPointer = true;
    }

    // When we end, set us back to our original parent unless we were dropped on a valid slot.
    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("On End Drag");
        popupManager.lockPointer = false;

        if (parentToInteractWith != null)
            transform.SetParent(parentToInteractWith);
        else
        {
            transform.SetParent(myParent);
            popupManager.HidePopups();
        }
        transform.localPosition = Vector3.zero;
        parentToInteractWith = null;

        myParent = transform.parent;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    // Used when the mouse hovers over this item.
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("we have a collision with a standard item");
        if(!popupManager.lockPointer)
        {
            transform.parent.SetAsLastSibling();
            popupManager.ShowPopup(transform, transform.parent.GetComponent<ItemDropZone>().popUpDirection);
        }
    }

    // Used when the mouse is no longer hovering over this item.
    public void OnPointerExit(PointerEventData eventData)
    {
        popupManager.HidePopups();
    }
}
