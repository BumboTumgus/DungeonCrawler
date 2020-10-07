using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoreInfoPopUpManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject[] showOnMouseOver;
    public GameObject[] hideOnMouseOver;

    public GameObject disjointedValueText;
    
    private InventoryPopupTextManager popupManager;

    // Start is called before the first frame update
    void Start()
    {
        popupManager = transform.parent.parent.GetComponent<InventoryPopupTextManager>();
        HideElements();
    }
    
    // Used when the mouse hovers over this item.
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!popupManager.lockPointer)
        {
            ShowElements();
            transform.SetAsLastSibling();
        }
    }

    // Used when the mouse is no longer hovering over this item.
    public void OnPointerExit(PointerEventData eventData)
    {
        //popupManager.HidePopups();
        HideElements();
    }

    //USed to show all object when we are moused over
    private void ShowElements()
    {
        foreach (GameObject gm in showOnMouseOver)
            gm.SetActive(true);
        foreach (GameObject gm in hideOnMouseOver)
            gm.SetActive(false);
    }

    //USed to show all object when we are moused over
    private void HideElements()
    {
        foreach (GameObject gm in showOnMouseOver)
            gm.SetActive(false);
        foreach (GameObject gm in hideOnMouseOver)
            gm.SetActive(true);

        if (disjointedValueText != null)
            disjointedValueText.transform.SetAsLastSibling();
    }
}
