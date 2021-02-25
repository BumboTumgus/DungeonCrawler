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
    public Text spellCostText;
    public Text spellCooldownText;
    public Text armorText;
    public Text resistanceText;
    public GameObject damageContainer;
    public GameObject attackSpeedContainer;
    public GameObject spellCostContainer;
    public GameObject spellCooldownContainer;
    public GameObject armorContainer;
    public GameObject resistanceContainer;
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

    private SkillBank skillBank;
    private RectTransform primaryPopUp;

    private void Start()
    {
        skillBank = FindObjectOfType<SkillBank>();
        primaryPopUp = GetComponent<RectTransform>();
        ClearPopUp();
    }

    // Call this method when we laucnh the item panel, so we may resize it properly.
    public void ShowPopUp(Item targetItem)
    {
        // Sets the damage fields of the popup if this item type is a weapon
        if (targetItem.itemType == Item.ItemType.Weapon || targetItem.itemType == Item.ItemType.TwoHandWeapon)
        {
            damageContainer.SetActive(true);
            attackSpeedContainer.SetActive(true);
            spellCostContainer.SetActive(false);
            spellCooldownContainer.SetActive(false);
            armorContainer.SetActive(false);
            resistanceContainer.SetActive(false);

            damageText.text = string.Format("{0}%", targetItem.baseDamageScaling * 100f);
            attackSpeedText.text = string.Format("{0}%", targetItem.attacksPerSecond * 100f);
        }
        else if(targetItem.itemType == Item.ItemType.Skill)
        {
            damageContainer.SetActive(false);
            attackSpeedContainer.SetActive(false);
            spellCostContainer.SetActive(true);
            spellCooldownContainer.SetActive(true);
            armorContainer.SetActive(false);
            resistanceContainer.SetActive(false);

            if (skillBank.GrabSkillCooldown(targetItem.skillName) > 0)
            {
                //spellCostText.text = string.Format("{0} mp", skillBank.GrabSkillCost(targetItem.skillName));
                spellCooldownText.text = string.Format("{0:0.0} sec", skillBank.GrabSkillCooldown(targetItem.skillName));
            }
            else
            {
                spellCostText.text = "Passive";
                spellCooldownText.text = "Passive";
            }
        }
        else if(targetItem.itemType == Item.ItemType.Helmet || targetItem.itemType == Item.ItemType.Armor || targetItem.itemType == Item.ItemType.Legs || targetItem.itemType == Item.ItemType.Trinket)
        {
            damageContainer.SetActive(false);
            attackSpeedContainer.SetActive(false);
            spellCostContainer.SetActive(false);
            spellCooldownContainer.SetActive(false);
            armorContainer.SetActive(true);
            resistanceContainer.SetActive(true);

            // gotta add the armor here;
            float armorValue = 0f;
            float resistanceValue = 0f;
            foreach (ItemTrait trait in targetItem.itemTraits)
            {
                if (trait.traitType == ItemTrait.TraitType.Armor)
                    armorValue = trait.traitBonus;
                if (trait.traitType == ItemTrait.TraitType.Resistance)
                    resistanceValue = trait.traitBonus;
            }

            armorText.text = string.Format("{0}", (int) armorValue);
            resistanceText.text = string.Format("{0}", (int) resistanceValue);
        }
        else
        {
            damageContainer.SetActive(false);
            attackSpeedContainer.SetActive(false);
            spellCostContainer.SetActive(false);
            spellCooldownContainer.SetActive(false);
            armorContainer.SetActive(false);
            resistanceContainer.SetActive(false);
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
        PopulatePopUp(targetItem);

        traitContainer.sizeDelta = new Vector2(traitContainer.sizeDelta.x, 13 * targetItem.itemTraits.Count + 10 + descriptionBox.sizeDelta.y);
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
    private void PopulatePopUp(Item item)
    {
        // Sets up the traits for the popup.
        for(int index = 0; index < item.itemTraits.Count; index ++)
        {
            GameObject traitText = Instantiate(popUpTextPrefab, traitContainer.transform);
            traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(200f, 13f);

            switch (item.itemTraits[index].traitType)
            {
                case ItemTrait.TraitType.Vit:
                    traitText.GetComponent<Text>().text = string.Format("+{0} Vit", item.itemTraits[index].traitBonus);
                    traitText.GetComponent<Text>().color = traitTextColors[0];
                    break;
                case ItemTrait.TraitType.Str:
                    traitText.GetComponent<Text>().text = string.Format("+{0} Str", item.itemTraits[index].traitBonus);
                    traitText.GetComponent<Text>().color = traitTextColors[1];
                    break;
                case ItemTrait.TraitType.Dex:
                    traitText.GetComponent<Text>().text = string.Format("+{0} Dex", item.itemTraits[index].traitBonus);
                    traitText.GetComponent<Text>().color = traitTextColors[2];
                    break;
                case ItemTrait.TraitType.Spd:
                    traitText.GetComponent<Text>().text = string.Format("+{0} Spd", item.itemTraits[index].traitBonus);
                    traitText.GetComponent<Text>().color = traitTextColors[3];
                    break;
                case ItemTrait.TraitType.Int:
                    traitText.GetComponent<Text>().text = string.Format("+{0} Int", item.itemTraits[index].traitBonus);
                    traitText.GetComponent<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.Wis:
                    traitText.GetComponent<Text>().text = string.Format("+{0} Wis", item.itemTraits[index].traitBonus);
                    traitText.GetComponent<Text>().color = traitTextColors[5];
                    break;
                case ItemTrait.TraitType.Health:
                    traitText.GetComponent<Text>().text = string.Format("+{0} Health", item.itemTraits[index].traitBonus);
                    traitText.GetComponent<Text>().color = traitTextColors[17];
                    break;
                case ItemTrait.TraitType.Mana:
                    traitText.GetComponent<Text>().text = string.Format("+{0} Mana", item.itemTraits[index].traitBonus);
                    traitText.GetComponent<Text>().color = traitTextColors[18];
                    break;
                case ItemTrait.TraitType.Armor:
                    traitText.GetComponent<Text>().text = string.Format("+{0} Armor", item.itemTraits[index].traitBonus);
                    traitText.GetComponent<Text>().color = traitTextColors[15];
                    if (item.itemType == Item.ItemType.Helmet || item.itemType == Item.ItemType.Armor || item.itemType == Item.ItemType.Legs || item.itemType == Item.ItemType.Trinket)
                        Destroy(traitText);
                    break;
                case ItemTrait.TraitType.Resistance:
                    traitText.GetComponent<Text>().text = string.Format("+{0} Resistance", item.itemTraits[index].traitBonus);
                    traitText.GetComponent<Text>().color = traitTextColors[16];
                    if (item.itemType == Item.ItemType.Helmet || item.itemType == Item.ItemType.Armor || item.itemType == Item.ItemType.Legs || item.itemType == Item.ItemType.Trinket)
                        Destroy(traitText);
                    break;
                case ItemTrait.TraitType.HealthRegen:
                    traitText.GetComponent<Text>().text = string.Format("+{0:0.0} Health Regen Per 5 Seconds", item.itemTraits[index].traitBonus);
                    traitText.GetComponent<Text>().color = traitTextColors[17];
                    break;
                case ItemTrait.TraitType.ManaRegen:
                    traitText.GetComponent<Text>().text = string.Format("+{0:0.0} Mana Regen Per 5 Seconds", item.itemTraits[index].traitBonus);
                    traitText.GetComponent<Text>().color = traitTextColors[18];
                    break;
                case ItemTrait.TraitType.CooldownReduction:
                    traitText.GetComponent<Text>().text = string.Format("+{0:0.0}% Cooldown Reduction", item.itemTraits[index].traitBonus * 100);
                    traitText.GetComponent<Text>().color = traitTextColors[20];
                    break;
                case ItemTrait.TraitType.SpellSlots:
                    traitText.GetComponent<Text>().text = string.Format("+{0} Spell Slots", item.itemTraits[index].traitBonus);
                    traitText.GetComponent<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.AflameResistance:
                    traitText.GetComponent<Text>().text = string.Format("+{0}% Fire Resistance", item.itemTraits[index].traitBonus * 100);
                    traitText.GetComponent<Text>().color = traitTextColors[6];
                    break;
                case ItemTrait.TraitType.AsleepResistance:
                    traitText.GetComponent<Text>().text = string.Format("+{0}% Sleep Resistance", item.itemTraits[index].traitBonus * 100);
                    traitText.GetComponent<Text>().color = traitTextColors[7];
                    break;
                case ItemTrait.TraitType.StunResistance:
                    traitText.GetComponent<Text>().text = string.Format("+{0}% Stun Resistance", item.itemTraits[index].traitBonus * 100);
                    traitText.GetComponent<Text>().color = traitTextColors[8];
                    break;
                case ItemTrait.TraitType.CurseResistance:
                    traitText.GetComponent<Text>().text = string.Format("+{0}% Curse Resistance", item.itemTraits[index].traitBonus * 100);
                    traitText.GetComponent<Text>().color = traitTextColors[9];
                    break;
                case ItemTrait.TraitType.BleedResistance:
                    traitText.GetComponent<Text>().text = string.Format("+{0}% Bleed Resistance", item.itemTraits[index].traitBonus * 100);
                    traitText.GetComponent<Text>().color = traitTextColors[10];
                    break;
                case ItemTrait.TraitType.PoisonResistance:
                    traitText.GetComponent<Text>().text = string.Format("+{0}% Poison Resistance", item.itemTraits[index].traitBonus * 100);
                    traitText.GetComponent<Text>().color = traitTextColors[11];
                    break;
                case ItemTrait.TraitType.FrostbiteResistance:
                    traitText.GetComponent<Text>().text = string.Format("+{0}% Frostbite Resistance", item.itemTraits[index].traitBonus * 100);
                    traitText.GetComponent<Text>().color = traitTextColors[12];
                    break;
                case ItemTrait.TraitType.KnockbackResistance:
                    traitText.GetComponent<Text>().text = string.Format("+{0}% Knockback Resistance", item.itemTraits[index].traitBonus * 100);
                    traitText.GetComponent<Text>().color = traitTextColors[13];
                    break;
                case ItemTrait.TraitType.AttackSpeed:
                    traitText.GetComponent<Text>().text = string.Format("+{0:0.0}% Attack Speed", item.itemTraits[index].traitBonus * 100);
                    traitText.GetComponent<Text>().color = traitTextColors[19];
                    break;
                default:
                    break;
            }
        }
    }
}
