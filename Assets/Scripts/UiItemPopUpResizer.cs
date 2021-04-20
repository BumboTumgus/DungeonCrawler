using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiItemPopUpResizer : MonoBehaviour
{
    public GameObject popUpTextPrefab;

    public Text itemName;
    public List<Text> itemBaseStatsText = new List<Text>();
    public List<GameObject> itemBaseStatContainers = new List<GameObject>();
    public List<Text> weaponIdentifierText = new List<Text>();
    public Text valueText;
    public Text countText;
    public Text itemTypeText;
    public Image popUpImage;
    public Image popUpImageOutline;
    public Image itemTypeImage;
    public RectTransform traitContainer;
    public RectTransform descriptionBox;
    public Color[] itemOutlineColors;
    public Sprite[] itemTypeSprites;
    public Color[] traitTextColors;
    public float PopUpTraitHeight = 0f;

    private SkillBank skillBank;
    private RectTransform primaryPopUp;
    private PlayerStats stats;

    private void Start()
    {
        skillBank = FindObjectOfType<SkillBank>();
        primaryPopUp = GetComponent<RectTransform>();
        stats = transform.parent.GetComponent<InventoryUiManager>().playerSkills.GetComponent<PlayerStats>();
        ClearPopUp();
    }

    // Call this method when we laucnh the item panel, so we may resize it properly.
    public void ShowPopUp(Item targetItem)
    {
        // Sets the damage fields of the popup if this item type is a weapon
        if (targetItem.itemType == Item.ItemType.Weapon || targetItem.itemType == Item.ItemType.TwoHandWeapon)
        {
            itemBaseStatContainers[0].SetActive(true);
            itemBaseStatContainers[1].SetActive(true);
            itemBaseStatContainers[2].SetActive(true);

            itemBaseStatContainers[3].SetActive(false);
            itemBaseStatContainers[4].SetActive(false);
            itemBaseStatContainers[5].SetActive(false);

            itemBaseStatContainers[6].SetActive(false);
            itemBaseStatContainers[7].SetActive(false);
            itemBaseStatContainers[8].SetActive(false);
            itemBaseStatContainers[9].SetActive(false);

            itemBaseStatContainers[10].SetActive(false);

            itemBaseStatsText[0].text = string.Format("{0}", targetItem.baseDamageScaling * stats.baseDamage);
            itemBaseStatsText[1].text = string.Format("{0:0.0}", targetItem.attacksPerSecond);
            itemBaseStatsText[2].text = string.Format("{0}", targetItem.stacksToAddOnHit);

            int primaryColorIndex = 0;
            int secondaryColorIndex = 0;
            switch (targetItem.damageType)
            {
                case HitBox.DamageType.Physical:
                    primaryColorIndex = 1;
                    secondaryColorIndex = 1;
                    weaponIdentifierText[0].text = "Physical Damage";
                    weaponIdentifierText[1].text = "No Affliction";
                    break;
                case HitBox.DamageType.Fire:
                    primaryColorIndex = 4;
                    secondaryColorIndex = 4;
                    weaponIdentifierText[0].text = "Fire Damage";
                    weaponIdentifierText[1].text = "Aflame Stacks";
                    break;
                case HitBox.DamageType.Ice:
                    primaryColorIndex = 5;
                    secondaryColorIndex = 5;
                    weaponIdentifierText[0].text = "Ice Damage";
                    weaponIdentifierText[1].text = "Frostbite Stacks";
                    break;
                case HitBox.DamageType.Lightning:
                    primaryColorIndex = 6;
                    secondaryColorIndex = 6;
                    weaponIdentifierText[0].text = "Lightning Damage";
                    weaponIdentifierText[1].text = "Overcharge Stacks";
                    break;
                case HitBox.DamageType.Nature:
                    primaryColorIndex = 7;
                    secondaryColorIndex = 7;
                    weaponIdentifierText[0].text = "Nature Damage";
                    weaponIdentifierText[1].text = "Overgrowth Stacks";
                    break;
                case HitBox.DamageType.Earth:
                    primaryColorIndex = 9;
                    secondaryColorIndex = 9;
                    weaponIdentifierText[0].text = "Earth Damage";
                    weaponIdentifierText[1].text = "Sunder Stacks";
                    break;
                case HitBox.DamageType.Wind:
                    primaryColorIndex = 8;
                    secondaryColorIndex = 8;
                    weaponIdentifierText[0].text = "Wind Damage";
                    weaponIdentifierText[1].text = "Windshear Stacks";
                    break;
                case HitBox.DamageType.Poison:
                    primaryColorIndex = 1;
                    secondaryColorIndex = 11;
                    weaponIdentifierText[0].text = "Physical Damage";
                    weaponIdentifierText[1].text = "Poison Stacks";
                    break;
                case HitBox.DamageType.Bleed:
                    primaryColorIndex = 1;
                    secondaryColorIndex = 10;
                    weaponIdentifierText[0].text = "Physical Damage";
                    weaponIdentifierText[1].text = "Bleed Stacks";
                    break;
                default:
                    break;
            }
            itemBaseStatContainers[0].GetComponent<Image>().color = traitTextColors[primaryColorIndex];
            itemBaseStatContainers[2].GetComponent<Image>().color = traitTextColors[secondaryColorIndex];
            weaponIdentifierText[0].GetComponent<Text>().color = traitTextColors[primaryColorIndex];
            weaponIdentifierText[1].GetComponent<Text>().color = traitTextColors[secondaryColorIndex];
        }
        else if(targetItem.itemType == Item.ItemType.Skill)
        {
            itemBaseStatContainers[0].SetActive(false);
            itemBaseStatContainers[1].SetActive(false);
            itemBaseStatContainers[2].SetActive(false);

            itemBaseStatContainers[3].SetActive(false);
            itemBaseStatContainers[4].SetActive(false);
            itemBaseStatContainers[5].SetActive(false);

            itemBaseStatContainers[6].SetActive(false);
            itemBaseStatContainers[7].SetActive(false);
            itemBaseStatContainers[8].SetActive(false);
            itemBaseStatContainers[9].SetActive(false);

            itemBaseStatContainers[10].SetActive(true);

            itemBaseStatsText[10].text = string.Format("{0:0.0}", skillBank.GrabSkillCooldown(targetItem.skillName) * (1 - stats.cooldownReduction));
        }
        else if(targetItem.itemType == Item.ItemType.Helmet || targetItem.itemType == Item.ItemType.Armor || targetItem.itemType == Item.ItemType.Legs || targetItem.itemType == Item.ItemType.Shield)
        {
            itemBaseStatContainers[0].SetActive(false);
            itemBaseStatContainers[1].SetActive(false);
            itemBaseStatContainers[2].SetActive(false);

            itemBaseStatContainers[3].SetActive(false);
            itemBaseStatContainers[4].SetActive(false);
            itemBaseStatContainers[5].SetActive(false);

            itemBaseStatContainers[6].SetActive(true);
            itemBaseStatContainers[7].SetActive(true);
            itemBaseStatContainers[8].SetActive(true);
            itemBaseStatContainers[9].SetActive(true);

            itemBaseStatContainers[10].SetActive(false);

            // gotta add the armor here;
            float armorValue = 0f;
            float flatDamageReductionValue = 0f;
            float healthGainValue = 0f;
            float movespeedValue = 0f;

            foreach (ItemTrait trait in targetItem.itemTraits)
            {
                if (trait.traitType == ItemTrait.TraitType.Armor)
                    armorValue = trait.traitBonus;
                else if (trait.traitType == ItemTrait.TraitType.FlatDamageReduction)
                    flatDamageReductionValue = trait.traitBonus;
                else if (trait.traitType == ItemTrait.TraitType.HealthFlat)
                    healthGainValue = trait.traitBonus;
                else if (trait.traitType == ItemTrait.TraitType.MoveSpeed)
                    movespeedValue = trait.traitBonus;
            }

            itemBaseStatsText[6].text = string.Format("{0:0}", (int) armorValue);
            itemBaseStatsText[7].text = string.Format("{0:0}", (int) flatDamageReductionValue);
            itemBaseStatsText[8].text = string.Format("{0:0}", (int) healthGainValue);
            itemBaseStatsText[9].text = string.Format("{0:0}%", (int) (movespeedValue * 100));

        }
        else
        {
            itemBaseStatContainers[0].SetActive(false);
            itemBaseStatContainers[1].SetActive(false);
            itemBaseStatContainers[2].SetActive(false);

            itemBaseStatContainers[3].SetActive(false);
            itemBaseStatContainers[4].SetActive(false);
            itemBaseStatContainers[5].SetActive(false);

            itemBaseStatContainers[6].SetActive(false);
            itemBaseStatContainers[7].SetActive(false);
            itemBaseStatContainers[8].SetActive(false);
            itemBaseStatContainers[9].SetActive(false);

            itemBaseStatContainers[10].SetActive(false);
        }

        // Sets up the description for the popup.
        descriptionBox.GetComponentInChildren<Text>().text = targetItem.description;
        descriptionBox.transform.Find("Text").GetComponent<RectTransform>().sizeDelta = new Vector2(descriptionBox.sizeDelta.x * 2, descriptionBox.GetComponentInChildren<Text>().preferredHeight);
        descriptionBox.sizeDelta = new Vector2(descriptionBox.sizeDelta.x, descriptionBox.GetComponentInChildren<Text>().preferredHeight / 2);

        // Set the sprite's artwork based on the item type
        switch (targetItem.itemType)
        {
            case Item.ItemType.TrinketRing:
                itemTypeImage.sprite = itemTypeSprites[0];
                itemTypeText.text = "Ring";
                break;
            case Item.ItemType.Weapon:
                itemTypeImage.sprite = itemTypeSprites[1];
                itemTypeText.text = "Weapon";
                break;
            case Item.ItemType.TwoHandWeapon:
                itemTypeImage.sprite = itemTypeSprites[2];
                itemTypeText.text = "Two Hand Weapon";
                break;
            case Item.ItemType.Helmet:
                itemTypeImage.sprite = itemTypeSprites[3];
                itemTypeText.text = "Helmet";
                break;
            case Item.ItemType.Legs:
                itemTypeImage.sprite = itemTypeSprites[4];
                itemTypeText.text = "Legs";
                break;
            case Item.ItemType.Armor:
                itemTypeImage.sprite = itemTypeSprites[5];
                itemTypeText.text = "Armor";
                break;
            case Item.ItemType.Skill:
                itemTypeImage.sprite = itemTypeSprites[6];
                itemTypeText.text = "Skill";
                break;
            case Item.ItemType.Shield:
                itemTypeImage.sprite = itemTypeSprites[7];
                itemTypeText.text = "Shield";
                break;
            case Item.ItemType.TrinketBracelet:
                itemTypeImage.sprite = itemTypeSprites[8];
                itemTypeText.text = "Bracelet";
                break;
            case Item.ItemType.TrinketCape:
                itemTypeImage.sprite = itemTypeSprites[9];
                itemTypeText.text = "Cape";
                break;
            case Item.ItemType.TrinketWaistItem:
                itemTypeImage.sprite = itemTypeSprites[10];
                itemTypeText.text = "Waist Trinket";
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

        PopUpTraitHeight += descriptionBox.sizeDelta.y;
        traitContainer.sizeDelta = new Vector2(traitContainer.sizeDelta.x, PopUpTraitHeight);
        primaryPopUp.sizeDelta = new Vector2(primaryPopUp.sizeDelta.x, 170 + traitContainer.sizeDelta.y);
    }

    // Used to clear every trait from the previous item from the popup
    private void ClearPopUp()
    {
        PopUpTraitHeight = 23;
        // Clear everything except the description and the description bar
        for (int index = 0; index < traitContainer.childCount; index++)
        {
            if (index > 2)
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
            traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(200f, 9f);

            switch (item.itemTraits[index].traitType)
            {
                case ItemTrait.TraitType.Armor:
                    traitText.GetComponentInChildren<Text>().text = string.Format("+{0:0} Armor", item.itemTraits[index].traitBonus * item.itemTraits[index].traitBonusMultiplier);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[0];
                    if (item.itemType == Item.ItemType.Helmet || item.itemType == Item.ItemType.Armor || item.itemType == Item.ItemType.Legs || item.itemType == Item.ItemType.Shield)
                    {
                        traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(200f, 0f);
                        Destroy(traitText);
                    }
                    break;
                case ItemTrait.TraitType.HealthRegen:
                    traitText.GetComponentInChildren<Text>().text = string.Format("+{0:0.0} Health regen per second", item.itemTraits[index].traitBonus * item.itemTraits[index].traitBonusMultiplier);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[3];
                    break;
                case ItemTrait.TraitType.CooldownReduction:
                    traitText.GetComponentInChildren<Text>().text = string.Format("+{0:0}% Cooldown reduction", item.itemTraits[index].traitBonus * item.itemTraits[index].traitBonusMultiplier * 100);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[16];
                    break;
                case ItemTrait.TraitType.AflameResistance:
                    traitText.GetComponentInChildren<Text>().text = string.Format("+{0:0}% Fire resist", item.itemTraits[index].traitBonus * item.itemTraits[index].traitBonusMultiplier * 100);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.FrostbiteResistance:
                    traitText.GetComponentInChildren<Text>().text = string.Format("+{0:0}% Frostbite resist", item.itemTraits[index].traitBonus * item.itemTraits[index].traitBonusMultiplier * 100);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[5];
                    break;
                case ItemTrait.TraitType.OverchargeResistance:
                    traitText.GetComponentInChildren<Text>().text = string.Format("+{0:0}% Lightning resist", item.itemTraits[index].traitBonus * item.itemTraits[index].traitBonusMultiplier * 100);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[6];
                    break;
                case ItemTrait.TraitType.OvergrowthResistance:
                    traitText.GetComponentInChildren<Text>().text = string.Format("+{0:0}% Nature resist", item.itemTraits[index].traitBonus * item.itemTraits[index].traitBonusMultiplier * 100);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[7];
                    break;
                case ItemTrait.TraitType.WindshearResistance:
                    traitText.GetComponentInChildren<Text>().text = string.Format("+{0:0}% Wind resist", item.itemTraits[index].traitBonus * item.itemTraits[index].traitBonusMultiplier * 100);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[8];
                    break;
                case ItemTrait.TraitType.SunderResistance:
                    traitText.GetComponentInChildren<Text>().text = string.Format("+{0:0}% Earth resist", item.itemTraits[index].traitBonus * item.itemTraits[index].traitBonusMultiplier * 100);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[9];
                    break;
                case ItemTrait.TraitType.BleedResistance:
                    traitText.GetComponentInChildren<Text>().text = string.Format("+{0:0}% Bleed resist", item.itemTraits[index].traitBonus * item.itemTraits[index].traitBonusMultiplier * 100);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[10];
                    break;
                case ItemTrait.TraitType.PoisonResistance:
                    traitText.GetComponentInChildren<Text>().text = string.Format("+{0:0}% Poison resist", item.itemTraits[index].traitBonus * item.itemTraits[index].traitBonusMultiplier * 100);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[11];
                    break;
                case ItemTrait.TraitType.AsleepResistance:
                    traitText.GetComponentInChildren<Text>().text = string.Format("+{0:0}% Sleep resist", item.itemTraits[index].traitBonus * item.itemTraits[index].traitBonusMultiplier * 100);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[12];
                    break;
                case ItemTrait.TraitType.StunResistance:
                    traitText.GetComponentInChildren<Text>().text = string.Format("+{0:0}% Stun resist", item.itemTraits[index].traitBonus * item.itemTraits[index].traitBonusMultiplier * 100);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[13];
                    break;
                case ItemTrait.TraitType.KnockbackResistance:
                    traitText.GetComponentInChildren<Text>().text = string.Format("+{0:0}% Knockback resist", item.itemTraits[index].traitBonus * item.itemTraits[index].traitBonusMultiplier * 100);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[14];
                    break;
                case ItemTrait.TraitType.AttackSpeed:
                    traitText.GetComponentInChildren<Text>().text = string.Format("+{0:0}% Attack speed", item.itemTraits[index].traitBonus * item.itemTraits[index].traitBonusMultiplier * 100);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[15];
                    break;
                case ItemTrait.TraitType.HealthFlat:
                    traitText.GetComponentInChildren<Text>().text = string.Format("+{0:0} Health", item.itemTraits[index].traitBonus * item.itemTraits[index].traitBonusMultiplier);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[3];
                    if (item.itemType == Item.ItemType.Helmet || item.itemType == Item.ItemType.Armor || item.itemType == Item.ItemType.Legs || item.itemType == Item.ItemType.Shield)
                    {
                        traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(200f, 0f);
                        Destroy(traitText);
                    }
                    break;
                case ItemTrait.TraitType.HealthPercent:
                    traitText.GetComponentInChildren<Text>().text = string.Format("+{0:0}% Increased health", item.itemTraits[index].traitBonus * 100 * item.itemTraits[index].traitBonusMultiplier);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[3];
                    break;
                case ItemTrait.TraitType.HealingOnHit:
                    traitText.GetComponentInChildren<Text>().text = string.Format("+{0:0} Health on hit", item.itemTraits[index].traitBonus * item.itemTraits[index].traitBonusMultiplier);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[2];
                    break;
                case ItemTrait.TraitType.HealingOnKill:
                    traitText.GetComponentInChildren<Text>().text = string.Format("+{0:0} Health on kill", item.itemTraits[index].traitBonus * item.itemTraits[index].traitBonusMultiplier);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[2];
                    break;
                case ItemTrait.TraitType.MoveSpeed:
                    traitText.GetComponentInChildren<Text>().text = string.Format("+{0:0}% Increased movespeed", item.itemTraits[index].traitBonus * 100 * item.itemTraits[index].traitBonusMultiplier);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[16];
                    if (item.itemType == Item.ItemType.Helmet || item.itemType == Item.ItemType.Armor || item.itemType == Item.ItemType.Legs || item.itemType == Item.ItemType.Shield)
                    {
                        traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(200f, 0f);
                        Destroy(traitText);
                    }
                    break;
                case ItemTrait.TraitType.Jumps:
                    traitText.GetComponentInChildren<Text>().text = string.Format("+{0:0} Extra jumps", item.itemTraits[index].traitBonus * item.itemTraits[index].traitBonusMultiplier);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[16];
                    break;
                case ItemTrait.TraitType.CritChance:
                    traitText.GetComponentInChildren<Text>().text = string.Format("+{0:0}% Critical strike chance", item.itemTraits[index].traitBonus * item.itemTraits[index].traitBonusMultiplier * 100);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[1];
                    break;
                case ItemTrait.TraitType.CritDamage:
                    traitText.GetComponentInChildren<Text>().text = string.Format("+{0:0}% Critical strike damage", item.itemTraits[index].traitBonus * item.itemTraits[index].traitBonusMultiplier * 100);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[1];
                    break;
                case ItemTrait.TraitType.FlatDamageReduction:
                    traitText.GetComponentInChildren<Text>().text = string.Format("+{0:0} Flat damage reduction", item.itemTraits[index].traitBonus * item.itemTraits[index].traitBonusMultiplier);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[17];
                    if (item.itemType == Item.ItemType.Helmet || item.itemType == Item.ItemType.Armor || item.itemType == Item.ItemType.Legs || item.itemType == Item.ItemType.Shield)
                    {
                        traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(200f, 0f);
                        Destroy(traitText);
                    }
                    break;
                case ItemTrait.TraitType.FireExplosionOnKill:
                    traitText.GetComponentInChildren<Text>().text = "Consuming blaze" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    // traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(200f, 14f);
                    //traitText.transform.Find("Text").GetComponent<RectTransform>().sizeDelta = new Vector2(400f, 30f);
                    break;
                case ItemTrait.TraitType.MoreAflameStacksOnHitThreshold:
                    traitText.GetComponentInChildren<Text>().text = "Raging inferno" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.BurnDoesMaxHpDamageAtThreshold:
                    traitText.GetComponentInChildren<Text>().text = "Fire overwhelming" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.BasicAttacksShredArmorOnAflame:
                    traitText.GetComponentInChildren<Text>().text = "Shredding flames" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.FlameVamperism:
                    traitText.GetComponentInChildren<Text>().text = "Flame vamperism" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.RingOfFireOnHit:
                    traitText.GetComponentInChildren<Text>().text = "Matrimony of flame" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.AflameToSunderStackOnEarthSpell:
                    traitText.GetComponentInChildren<Text>().text = "Wrath of the <color=#B0946C>blazing earth" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.SunderFurtherDecreasesFireResist:
                    traitText.GetComponentInChildren<Text>().text = "Deep rooted <color=#B0946C>flames" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.AflameSunderCritsSummonFireballs:
                    traitText.GetComponentInChildren<Text>().text = "Devasting flame <color=#B0946C>geyser" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.AflameWindshearWindAttacksGainCritOnBurningTarget:
                    traitText.GetComponentInChildren<Text>().text = "Blazing <color=#ABD1E0>exposure" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.AflameWindshearSummonFirePillarsOnHit:
                    traitText.GetComponentInChildren<Text>().text = "Flaring <color=#ABD1E0>winds" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.AflameWindshearWindSpellsAddFireStacks:
                    traitText.GetComponentInChildren<Text>().text = "Roaring <color=#ABD1E0>blaze" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.AflamePhysicalAddFireStacksOnHit:
                    traitText.GetComponentInChildren<Text>().text = "Searing <color=#E94453>metal" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.AflamePhysicalDamageAmpOnBurningTarget:
                    traitText.GetComponentInChildren<Text>().text = "Smoldering <color=#E94453>steel" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.AflamePhysicalBladeExplosionOnKill:
                    traitText.GetComponentInChildren<Text>().text = "Red-hot <color=#E94453>metallic implosion" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.AflamePhysicalBigHitsAddAflame:
                    traitText.GetComponentInChildren<Text>().text = "Searing <color=#E94453>strikes" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.AflameBleedIncreasesFlameCritChance:
                    traitText.GetComponentInChildren<Text>().text = "Searing <color=#AB181D>wounds" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.AflameBleedFireDamageAmpOnBleedThreshold:
                    traitText.GetComponentInChildren<Text>().text = "Boiling <color=#AB181D>blood" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.AflameBleedAflameAddsBleedAtThreshhold:
                    traitText.GetComponentInChildren<Text>().text = "Blaze of <color=#AB181D>exsanguination" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.AflameBleedAflameRemovesBleedResist:
                    traitText.GetComponentInChildren<Text>().text = "Flames of <color=#AB181D>hemorrhage" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.AflameBleedDamageAmpOnDoubleThreshhold:
                    traitText.GetComponentInChildren<Text>().text = "Consuming <color=#AB181D>blaze" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                default:
                    break;
            }

            PopUpTraitHeight += traitText.GetComponent<RectTransform>().sizeDelta.y;
        }
    }

    private string GetTraitValueRomanNumeral(ItemTrait itemTrait)
    {
        string traitNumeral = "";
        switch (itemTrait.traitBonusMultiplier)
        {
            case 1:
                traitNumeral = "";
                break;
            case 2:
                traitNumeral = " - II";
                break;
            case 3:
                traitNumeral = " - III";
                break;
            case 4:
                traitNumeral = " - IV";
                break;
            case 5:
                traitNumeral = " - V";
                break;
            case 6:
                traitNumeral = " - VI";
                break;
            case 7:
                traitNumeral = " - VII";
                break;
            case 8:
                traitNumeral = " - VIII";
                break;
            case 9:
                traitNumeral = " - IX";
                break;
            case 10:
                traitNumeral = " - X";
                break;
            default:
                break;
        }
        return traitNumeral;
    }
}
