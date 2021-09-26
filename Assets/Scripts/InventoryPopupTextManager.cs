using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPopupTextManager : MonoBehaviour
{
    public bool lockPointer = false;
    public enum PopUpDirection { TL, TR, BL, BR }
    
    public GameObject itemPopUp;

    public GameObject moreInfoPopUp;

    public MoreInfoPopUpManager[] moreInfoPanels;

    private const float POPUP_OFFSET = 10;

    public Color[] itemOutlineColors;

    // Start is called before the first frame update
    void Start()
    {
        itemPopUp = transform.Find("ItemPopUp").gameObject;
        itemPopUp.GetComponent<UiItemPopUpResizer>().Initialize();
        itemPopUp.SetActive(false);

        moreInfoPopUp = transform.Find("ItemAdvancedDescription").gameObject;
        moreInfoPopUp.GetComponent<UiMoreInfoPopup>().Initialize();
        moreInfoPopUp.SetActive(false);
    }
    
    // Used to enable the popup.
    public void ShowPopup(Transform popUpParent, PopUpDirection direction)
    {
        itemPopUp.transform.SetParent(popUpParent);
        itemPopUp.transform.SetAsLastSibling();
        moreInfoPopUp.transform.SetAsLastSibling();
        itemPopUp.transform.localPosition = Vector3.zero;

        //Hide the hotbar number if there is one in the parent.
        if (itemPopUp.transform.parent.parent.Find("HotbarNumber") != null)
            itemPopUp.transform.parent.parent.Find("HotbarNumber").gameObject.SetActive(false);

        itemPopUp.GetComponent<UiItemPopUpResizer>().ShowPopUp(popUpParent.GetComponent<ItemDraggable>().attachedItem.GetComponent<Item>());

        //SetPopUpStats(popUpParent.GetComponent<ItemDraggable>().attachedItem.GetComponent<Item>());

        RectTransform popUpRect = itemPopUp.GetComponent<RectTransform>();
        switch (direction)
        {
            case PopUpDirection.TL:
                popUpRect.anchorMin = new Vector2(1, 0);
                popUpRect.anchorMax = new Vector2(1, 0);
                popUpRect.pivot = new Vector2(1, 0);
                popUpRect.anchoredPosition = new Vector3(POPUP_OFFSET, -POPUP_OFFSET, 0);
                break;
            case PopUpDirection.TR:
                popUpRect.anchorMin = new Vector2(0, 0);
                popUpRect.anchorMax = new Vector2(0, 0);
                popUpRect.pivot = new Vector2(0, 0);
                popUpRect.anchoredPosition = new Vector3(-POPUP_OFFSET, -POPUP_OFFSET, 0);
                break;
            case PopUpDirection.BL:
                popUpRect.anchorMin = new Vector2(1, 1);
                popUpRect.anchorMax = new Vector2(1, 1);
                popUpRect.pivot = new Vector2(1, 1);
                popUpRect.anchoredPosition = new Vector3(POPUP_OFFSET, POPUP_OFFSET, 0);
                break;
            case PopUpDirection.BR:
                popUpRect.anchorMin = new Vector2(0, 1);
                popUpRect.anchorMax = new Vector2(0, 1);
                popUpRect.pivot = new Vector2(0, 1);
                popUpRect.anchoredPosition = new Vector3(-POPUP_OFFSET, POPUP_OFFSET, 0);
                break;
            default:
                break;
        }

        itemPopUp.SetActive(true);
    }

    // USed to show the advanced info popup
    public void ShowMoreInfoPopup(Transform popUpParent)
    {
        moreInfoPopUp.transform.SetAsLastSibling();
        moreInfoPopUp.GetComponent<UiMoreInfoPopup>().ShowPopUp(popUpParent.GetComponent<ItemDraggable>().attachedItem.GetComponent<Item>());
        moreInfoPopUp.SetActive(true);
    }

    // Used to hide the Popups
    public void HidePopups(bool hideMoreInfoPanel)
    {
        //Show the hotbar number if there is one in the parent.
        if (itemPopUp.transform.parent.parent.Find("HotbarNumber") != null)
            itemPopUp.transform.parent.parent.Find("HotbarNumber").gameObject.SetActive(true);

        if (!lockPointer)
         itemPopUp.SetActive(false);

        if(hideMoreInfoPanel)
            moreInfoPopUp.SetActive(false);

        foreach (MoreInfoPopUpManager panel in moreInfoPanels)
            panel.HideElements();
    }
    
}
