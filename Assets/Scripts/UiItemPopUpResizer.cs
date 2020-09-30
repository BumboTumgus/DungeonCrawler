using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiItemPopUpResizer : MonoBehaviour
{
    public GameObject popUpTextPrefab;

    public Text itemName;
    public Text damageText;
    public Text attackSpeedText;
    public GameObject damageTextContainer;
    public GameObject attackSpeedContainer;
    public Text valueText;
    public Text countText;
    public Image popUpImage;
    public Image popUpImageOutline;
    public Image itemTypeImage;
    public RectTransform traitContainer;
    public RectTransform descriptionBox;
    public Color[] itemOutlineColors;
    public Sprite[] itemTypeSprites;
    public Color[] traitTextColors;

    private RectTransform primaryPopUp;

    private void Start()
    {
        primaryPopUp = GetComponent<RectTransform>();
        ClearPopUp();
    }

    // Call this method when we laucnh the item panel, so we may resize it properly.
    public void ShowPopUp(Item targetItem)
    {
        // Sets the damage fields of the popup if this item type is a weapon
        if (targetItem.itemType == Item.ItemType.Weapon || targetItem.itemType == Item.ItemType.TwoHandWeapon)
        {
            damageTextContainer.SetActive(true);
            attackSpeedContainer.SetActive(true);
            damageText.text = string.Format("{0}%", targetItem.baseDamageScaling * 100f);
            attackSpeedText.text = string.Format("{0}%", targetItem.attacksPerSecond * 100f);
        }
        else
        {
            damageTextContainer.SetActive(false);
            attackSpeedContainer.SetActive(false);
        }

        // Sets up the description for the popup.
        descriptionBox.GetComponent<Text>().text = targetItem.description;
        descriptionBox.sizeDelta = new Vector2(descriptionBox.sizeDelta.x, descriptionBox.GetComponent<Text>().preferredHeight);

        // Set the sprite's artwork based on the item type
        switch (targetItem.itemType)
        {
            case Item.ItemType.Consumable:
                itemTypeImage.sprite = itemTypeSprites[0];
                break;
            case Item.ItemType.Gold:
                itemTypeImage.sprite = itemTypeSprites[1];
                break;
            case Item.ItemType.Trinket:
                itemTypeImage.sprite = itemTypeSprites[2];
                break;
            case Item.ItemType.Weapon:
                itemTypeImage.sprite = itemTypeSprites[3];
                break;
            case Item.ItemType.TwoHandWeapon:
                itemTypeImage.sprite = itemTypeSprites[4];
                break;
            case Item.ItemType.Helmet:
                itemTypeImage.sprite = itemTypeSprites[5];
                break;
            case Item.ItemType.Legs:
                itemTypeImage.sprite = itemTypeSprites[6];
                break;
            case Item.ItemType.Armor:
                itemTypeImage.sprite = itemTypeSprites[7];
                break;
            case Item.ItemType.Skill:
                itemTypeImage.sprite = itemTypeSprites[8];
                break;
            default:
                break;
        }

        valueText.text = string.Format("{0} gp", targetItem.goldValue * targetItem.currentStack);
        popUpImage.sprite = targetItem.artwork;
        itemName.text = targetItem.itemName + "";


        if (targetItem.currentStack > 1)
            countText.text = string.Format("x{0}", targetItem.currentStack);
        else
            countText.text = "";

        switch (targetItem.itemRarity)
        {
            case Item.ItemRarity.Common:
                popUpImageOutline.color = itemOutlineColors[0];
                itemTypeImage.color = itemOutlineColors[0];
                itemName.color = new Color(itemOutlineColors[0].r, itemOutlineColors[0].g, itemOutlineColors[0].b, 1);
                break;
            case Item.ItemRarity.Uncommon:
                popUpImageOutline.color = itemOutlineColors[1];
                itemTypeImage.color = itemOutlineColors[1];
                itemName.color = new Color(itemOutlineColors[1].r, itemOutlineColors[1].g, itemOutlineColors[1].b, 1);
                break;
            case Item.ItemRarity.Rare:
                popUpImageOutline.color = itemOutlineColors[2];
                itemTypeImage.color = itemOutlineColors[2];
                itemName.color = new Color(itemOutlineColors[2].r, itemOutlineColors[2].g, itemOutlineColors[2].b, 1);
                break;
            case Item.ItemRarity.Legendary:
                popUpImageOutline.color = itemOutlineColors[3];
                itemTypeImage.color = itemOutlineColors[3];
                itemName.color = new Color(itemOutlineColors[3].r, itemOutlineColors[3].g, itemOutlineColors[3].b, 1);
                break;
            case Item.ItemRarity.Masterwork:
                popUpImageOutline.color = itemOutlineColors[4];
                itemTypeImage.color = itemOutlineColors[4];
                itemName.color = new Color(itemOutlineColors[4].r, itemOutlineColors[4].g, itemOutlineColors[4].b, 1);
                break;
            default:
                break;
        }


        // Clear the trait aspect of the popup.
        ClearPopUp();
        PopulatePopUp(targetItem.itemTraits);

        traitContainer.sizeDelta = new Vector2(traitContainer.sizeDelta.x, 12 * targetItem.itemTraits.Count + 10 + descriptionBox.sizeDelta.y);
        primaryPopUp.sizeDelta = new Vector2(primaryPopUp.sizeDelta.x, 170 + traitContainer.sizeDelta.y);
    }

    // Used to clear every trait from the previous item from the popup
    private void ClearPopUp()
    {
        // Clear everything except the description and the description bar
        for (int index = 0; index < traitContainer.childCount; index++)
        {
            if (index > 1)
                Destroy(traitContainer.GetChild(index).gameObject);
        }
    }

    // Used to populate the popup with the traits we have.
    private void PopulatePopUp(List<ItemTrait> itemTraits)
    {
        // Sets up the traits for the popup.
        for(int index = 0; index < itemTraits.Count; index ++)
        {
            GameObject traitText = Instantiate(popUpTextPrefab, traitContainer.transform);
            traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(200f, 12f * (index + 1));

            switch (itemTraits[index].traitType)
            {
                case ItemTrait.TraitType.Vit:
                    traitText.GetComponent<Text>().text = string.Format("+{0} Vit", itemTraits[index].traitBonus);
                    traitText.GetComponent<Text>().color = traitTextColors[0];
                    break;
                case ItemTrait.TraitType.Str:
                    traitText.GetComponent<Text>().text = string.Format("+{0} Str", itemTraits[index].traitBonus);
                    traitText.GetComponent<Text>().color = traitTextColors[1];
                    break;
                case ItemTrait.TraitType.Dex:
                    traitText.GetComponent<Text>().text = string.Format("+{0} Dex", itemTraits[index].traitBonus);
                    traitText.GetComponent<Text>().color = traitTextColors[2];
                    break;
                case ItemTrait.TraitType.Spd:
                    traitText.GetComponent<Text>().text = string.Format("+{0} Spd", itemTraits[index].traitBonus);
                    traitText.GetComponent<Text>().color = traitTextColors[3];
                    break;
                case ItemTrait.TraitType.Int:
                    traitText.GetComponent<Text>().text = string.Format("+{0} Int", itemTraits[index].traitBonus);
                    traitText.GetComponent<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.Wis:
                    traitText.GetComponent<Text>().text = string.Format("+{0} Wis", itemTraits[index].traitBonus);
                    traitText.GetComponent<Text>().color = traitTextColors[5];
                    break;
                case ItemTrait.TraitType.Health:
                    traitText.GetComponent<Text>().text = string.Format("+{0} Health", itemTraits[index].traitBonus);
                    traitText.GetComponent<Text>().color = traitTextColors[0];
                    break;
                case ItemTrait.TraitType.Mana:
                    traitText.GetComponent<Text>().text = string.Format("+{0} Mana", itemTraits[index].traitBonus);
                    traitText.GetComponent<Text>().color = traitTextColors[5];
                    break;
                case ItemTrait.TraitType.Armor:
                    traitText.GetComponent<Text>().text = string.Format("+{0} Armor", itemTraits[index].traitBonus);
                    traitText.GetComponent<Text>().color = traitTextColors[15];
                    break;
                case ItemTrait.TraitType.Resistance:
                    traitText.GetComponent<Text>().text = string.Format("+{0} Resistance", itemTraits[index].traitBonus);
                    traitText.GetComponent<Text>().color = traitTextColors[16];
                    break;
                case ItemTrait.TraitType.HealthRegen:
                    traitText.GetComponent<Text>().text = string.Format("+{0} Health Regen Per 5 Seconds", itemTraits[index].traitBonus);
                    traitText.GetComponent<Text>().color = traitTextColors[0];
                    break;
                case ItemTrait.TraitType.ManaRegen:
                    traitText.GetComponent<Text>().text = string.Format("+{0} Mana Regen Per 5 Seconds", itemTraits[index].traitBonus);
                    traitText.GetComponent<Text>().color = traitTextColors[5];
                    break;
                case ItemTrait.TraitType.CooldownReduction:
                    traitText.GetComponent<Text>().text = string.Format("+{0}% Cooldown Reduction", itemTraits[index].traitBonus);
                    traitText.GetComponent<Text>().color = traitTextColors[5];
                    break;
                case ItemTrait.TraitType.SpellSlots:
                    traitText.GetComponent<Text>().text = string.Format("+{0} Spell Slots", itemTraits[index].traitBonus);
                    traitText.GetComponent<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.AflameResistance:
                    traitText.GetComponent<Text>().text = string.Format("+{0}% Fire Resistance", itemTraits[index].traitBonus);
                    traitText.GetComponent<Text>().color = traitTextColors[6];
                    break;
                case ItemTrait.TraitType.AsleepResistance:
                    traitText.GetComponent<Text>().text = string.Format("+{0}% Sleep Resistance", itemTraits[index].traitBonus);
                    traitText.GetComponent<Text>().color = traitTextColors[7];
                    break;
                case ItemTrait.TraitType.StunResistance:
                    traitText.GetComponent<Text>().text = string.Format("+{0}% Tenacity", itemTraits[index].traitBonus);
                    traitText.GetComponent<Text>().color = traitTextColors[8];
                    break;
                case ItemTrait.TraitType.CurseResistance:
                    traitText.GetComponent<Text>().text = string.Format("+{0}% Curse Resistance", itemTraits[index].traitBonus);
                    traitText.GetComponent<Text>().color = traitTextColors[9];
                    break;
                case ItemTrait.TraitType.BleedResistance:
                    traitText.GetComponent<Text>().text = string.Format("+{0}% Bleed Resistance", itemTraits[index].traitBonus);
                    traitText.GetComponent<Text>().color = traitTextColors[10];
                    break;
                case ItemTrait.TraitType.PoisonResistance:
                    traitText.GetComponent<Text>().text = string.Format("+{0}% Poison Resistance", itemTraits[index].traitBonus);
                    traitText.GetComponent<Text>().color = traitTextColors[11];
                    break;
                case ItemTrait.TraitType.FrostbiteResistance:
                    traitText.GetComponent<Text>().text = string.Format("+{0}% Frostbite Resistance", itemTraits[index].traitBonus);
                    traitText.GetComponent<Text>().color = traitTextColors[12];
                    break;
                case ItemTrait.TraitType.KnockbackResistance:
                    traitText.GetComponent<Text>().text = string.Format("+{0}% Knockback Resistance", itemTraits[index].traitBonus);
                    traitText.GetComponent<Text>().color = traitTextColors[13];
                    break;
                case ItemTrait.TraitType.AttackSpeed:
                    traitText.GetComponent<Text>().text = string.Format("+{0}% Attack Speed", itemTraits[index].traitBonus);
                    traitText.GetComponent<Text>().color = traitTextColors[14];
                    break;
                default:
                    break;
            }
        }
    }
}
