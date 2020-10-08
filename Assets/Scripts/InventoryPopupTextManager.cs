using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPopupTextManager : MonoBehaviour
{
    public bool lockPointer = false;
    public enum PopUpDirection { TL, TR, BL, BR }
    
    public GameObject itemPopUp;

    public MoreInfoPopUpManager[] moreInfoPanels;

    private const float POPUP_OFFSET = 10;

    public Color[] itemOutlineColors;

    // Start is called before the first frame update
    void Start()
    {
        itemPopUp = transform.Find("ItemPopUp").gameObject;
        itemPopUp.SetActive(false);
    }
    
    // Used to enable the popup.
    public void ShowPopup(Transform popUpParent, PopUpDirection direction)
    {
        itemPopUp.transform.SetParent(popUpParent);
        itemPopUp.transform.SetAsLastSibling();
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


    // Used to hide the Popups
    public void HidePopups()
    {
        //Show the hotbar number if there is one in the parent.
        if (itemPopUp.transform.parent.parent.Find("HotbarNumber") != null)
            itemPopUp.transform.parent.parent.Find("HotbarNumber").gameObject.SetActive(true);

        if (!lockPointer)
         itemPopUp.SetActive(false);

        foreach (MoreInfoPopUpManager panel in moreInfoPanels)
            panel.HideElements();
    }
    
    /*
    // Used to set the stats of the popup
    public void SetPopUpStats(Item item)
    {
        PlayerStats stats = GetComponent<InventoryUiManager>().playerInventory.GetComponent<PlayerStats>();
        Transform statContainer = itemPopUp.transform.Find("ItemDescription");
        // Set up all the different popup values.
        // Debug.Log(" we are setting the popup stats");

        itemPopUp.transform.Find("ItemImage").GetComponent<Image>().sprite = item.artwork;
        itemPopUp.transform.Find("ItemTitle").GetComponent<Text>().text = item.itemName + "";
        itemPopUp.transform.Find("ItemType").GetComponent<Text>().text = item.itemType + "";
        if(item.currentStack > 1)
            itemPopUp.transform.Find("ItemCount").GetComponent<Text>().text = "x" + item.currentStack;
        else
            itemPopUp.transform.Find("ItemCount").GetComponent<Text>().text = "";
        itemPopUp.transform.Find("ItemWorth").GetComponent<Text>().text = "Worth: " + (item.goldValue * item.currentStack) + "gp";

        statContainer.GetComponent<Text>().text = item.description;

        SetPlusMinusText(statContainer.Find("VitStatValue").GetComponent<Text>(), item.vitMod);
        SetPlusMinusText(statContainer.Find("StrStatValue").GetComponent<Text>(), item.strMod);
        SetPlusMinusText(statContainer.Find("DexStatValue").GetComponent<Text>(), item.dexMod);
        SetPlusMinusText(statContainer.Find("SpdStatValue").GetComponent<Text>(), item.spdMod);
        SetPlusMinusText(statContainer.Find("IntStatValue").GetComponent<Text>(), item.intMod);
        SetPlusMinusText(statContainer.Find("WisStatValue").GetComponent<Text>(), item.wisMod);
        SetPlusMinusText(statContainer.Find("ChaStatValue").GetComponent<Text>(), item.chaMod);

        SetLetterScaling(statContainer.Find("VitScaling").GetComponent<Text>(), item.vitScaling);
        SetLetterScaling(statContainer.Find("StrScaling").GetComponent<Text>(), item.strScaling);
        SetLetterScaling(statContainer.Find("DexScaling").GetComponent<Text>(), item.dexScaling);
        SetLetterScaling(statContainer.Find("SpdScaling").GetComponent<Text>(), item.spdScaling);
        SetLetterScaling(statContainer.Find("IntScaling").GetComponent<Text>(), item.intScaling);
        SetLetterScaling(statContainer.Find("WisScaling").GetComponent<Text>(), item.wisScaling);
        SetLetterScaling(statContainer.Find("ChaScaling").GetComponent<Text>(), item.chaScaling);
        
        float statBasedDamaged = stats.Str * item.strScaling + stats.Dex * item.dexScaling + stats.Vit * item.vitScaling + stats.Spd * item.spdScaling
                + stats.Int * item.intScaling + stats.Wis * item.wisScaling + stats.Cha * item.chaScaling;
        statContainer.Find("AttackSpeedValue").GetComponent<Text>().text = string.Format("{0:0.00}", (item.baseAttackDelay * (1 + 0.025f * stats.Spd + 0.0125f * stats.Dex)));
        statContainer.Find("DamageValue").GetComponent<Text>().text = string.Format("{0:0}",(item.hitBase + statBasedDamaged)) + " - " + string.Format("{0:0}", (item.hitBase + item.hitMax + statBasedDamaged));
        statContainer.Find("CritChanceValue").GetComponent<Text>().text = item.critChance + "%";
        statContainer.Find("CritDamageValue").GetComponent<Text>().text = item.critMod * 100  + "%";

        statContainer.Find("ArmorValue").GetComponent<Text>().text = item.armor + "";
        statContainer.Find("ResistanceValue").GetComponent<Text>().text = item.resistance + "";
        SetPlusMinusText(statContainer.Find("HealthValue").GetComponent<Text>(), item.health);
        statContainer.Find("HealthRegenValue").GetComponent<Text>().text = item.healthRegen + "";
        SetPlusMinusText(statContainer.Find("ManaValue").GetComponent<Text>(), item.mana);
        statContainer.Find("ManaRegenValue").GetComponent<Text>().text = item.manaRegen + "";

        switch (item.itemRarity)
        {
            case Item.ItemRarity.Common:
                itemPopUp.transform.Find("Outline").GetComponent<Image>().color = itemOutlineColors[0];
                break;
            case Item.ItemRarity.Uncommon:
                itemPopUp.transform.Find("Outline").GetComponent<Image>().color = itemOutlineColors[1];
                break;
            case Item.ItemRarity.Rare:
                itemPopUp.transform.Find("Outline").GetComponent<Image>().color = itemOutlineColors[2];
                break;
            case Item.ItemRarity.Legendary:
                itemPopUp.transform.Find("Outline").GetComponent<Image>().color = itemOutlineColors[3];
                break;
            case Item.ItemRarity.Masterwork:
                itemPopUp.transform.Find("Outline").GetComponent<Image>().color = itemOutlineColors[4];
                break;
            default:
                break;
        }
    }
*/
    /*
    // Used to set a plus/minus stat text field
    private void SetPlusMinusText(Text statText, float value)
    {
        if (value == 0)
            statText.text = "-";
        else if (value > 0)
            statText.text = "+" + value;
        else
            statText.text = "" + value;

    }

    // Used to set the letter scaling 
    private void SetLetterScaling(Text statText, float value)
    {
        string letterGrade = "-";
        if (value <= 0)
            letterGrade = "-";
        else if (value < 0.2f)
            letterGrade = "D-";
        else if (value < 0.35f)
            letterGrade = "D";
        else if (value < 0.5f)
            letterGrade = "C-";
        else if (value < 0.65f)
            letterGrade = "C";
        else if (value < 0.8f)
            letterGrade = "B-";
        else if (value < 0.95f)
            letterGrade = "B";
        else if (value < 1.1f)
            letterGrade = "A-";
        else if (value < 1.25f)
            letterGrade = "A";
        else if (value < 1.4f)
            letterGrade = "S-";
        else
            letterGrade = "S";
        statText.text = letterGrade;
    }
    */

}
