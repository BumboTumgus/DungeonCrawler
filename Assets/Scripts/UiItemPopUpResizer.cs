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
    public Image imageBackground;
    public Image popUpImageOutline;
    public Image itemTypeImage;
    public RectTransform traitContainer;
    public RectTransform descriptionBox;
    public Color[] itemOutlineColors;
    public Sprite[] itemTypeSprites;
    public Color[] traitTextColors;
    public float PopUpTraitHeight = 0f;

    public Color[] iconAffintiyColors;
    public Color[] iconAffintiyBackgroundColors;
    public Color[] backgroundRarityColor;
    public Sprite[] iconAffinityIcons;

    private SkillBank skillBank;
    [SerializeField] private RectTransform primaryPopUp;
    private PlayerStats stats;

    public void Initialize()
    {
        //Debug.Log("initilaize function started");
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

            //Debug.Log("maybe the target item is busted lets try checking that: " + targetItem);
            //Debug.Log("check stats? " + stats);
            //Debug.Log("cjheck item base stats texts" + itemBaseStatsText[0]);
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
        itemName.text = targetItem.itemNameModifiersPrefix + targetItem.itemName + " " + targetItem.itemNameAffinitySuffix;


        if (targetItem.currentStack > 1)
            countText.text = string.Format("x{0}", targetItem.currentStack);
        else
            countText.text = "";

        switch (targetItem.itemRarity)
        {
            case Item.ItemRarity.Common:
                popUpImageOutline.color = itemOutlineColors[0];
                itemTypeImage.color = itemOutlineColors[0];
                imageBackground.color = backgroundRarityColor[0];
                itemName.color = new Color(itemOutlineColors[0].r, itemOutlineColors[0].g, itemOutlineColors[0].b, 1);
                break;
            case Item.ItemRarity.Uncommon:
                popUpImageOutline.color = itemOutlineColors[1];
                itemTypeImage.color = itemOutlineColors[1];
                imageBackground.color = backgroundRarityColor[1];
                itemName.color = new Color(itemOutlineColors[1].r, itemOutlineColors[1].g, itemOutlineColors[1].b, 1);
                break;
            case Item.ItemRarity.Rare:
                popUpImageOutline.color = itemOutlineColors[2];
                itemTypeImage.color = itemOutlineColors[2];
                imageBackground.color = backgroundRarityColor[2];
                itemName.color = new Color(itemOutlineColors[2].r, itemOutlineColors[2].g, itemOutlineColors[2].b, 1);
                break;
            case Item.ItemRarity.Legendary:
                popUpImageOutline.color = itemOutlineColors[3];
                itemTypeImage.color = itemOutlineColors[3];
                imageBackground.color = backgroundRarityColor[3];
                itemName.color = new Color(itemOutlineColors[3].r, itemOutlineColors[3].g, itemOutlineColors[3].b, 1);
                break;
            case Item.ItemRarity.Masterwork:
                popUpImageOutline.color = itemOutlineColors[4];
                itemTypeImage.color = itemOutlineColors[4];
                imageBackground.color = backgroundRarityColor[4];
                itemName.color = new Color(itemOutlineColors[4].r, itemOutlineColors[4].g, itemOutlineColors[4].b, 1);
                break;
            default:
                break;
        }

        if(targetItem.itemType != Item.ItemType.Skill)
        {

            Image primaryAffinityBackground = imageBackground.transform.Find("Affinity_Primary").GetComponent<Image>();
            switch (targetItem.affinityPrimary)
            {
                case Item.AffinityType.None:
                    primaryAffinityBackground.gameObject.SetActive(false);
                    break;
                case Item.AffinityType.Fire:
                    primaryAffinityBackground.gameObject.SetActive(true);
                    primaryAffinityBackground.color = iconAffintiyBackgroundColors[0];
                    primaryAffinityBackground.transform.Find("Icon").GetComponent<Image>().color = iconAffintiyColors[0];
                    primaryAffinityBackground.transform.Find("Icon").GetComponent<Image>().sprite = iconAffinityIcons[0];
                    break;
                case Item.AffinityType.Ice:
                    primaryAffinityBackground.gameObject.SetActive(true);
                    primaryAffinityBackground.color = iconAffintiyBackgroundColors[1];
                    primaryAffinityBackground.transform.Find("Icon").GetComponent<Image>().color = iconAffintiyColors[1];
                    primaryAffinityBackground.transform.Find("Icon").GetComponent<Image>().sprite = iconAffinityIcons[1];
                    break;
                case Item.AffinityType.Earth:
                    primaryAffinityBackground.gameObject.SetActive(true);
                    primaryAffinityBackground.color = iconAffintiyBackgroundColors[2];
                    primaryAffinityBackground.transform.Find("Icon").GetComponent<Image>().color = iconAffintiyColors[2];
                    primaryAffinityBackground.transform.Find("Icon").GetComponent<Image>().sprite = iconAffinityIcons[2];
                    break;
                case Item.AffinityType.Wind:
                    primaryAffinityBackground.gameObject.SetActive(true);
                    primaryAffinityBackground.color = iconAffintiyBackgroundColors[3];
                    primaryAffinityBackground.transform.Find("Icon").GetComponent<Image>().color = iconAffintiyColors[3];
                    primaryAffinityBackground.transform.Find("Icon").GetComponent<Image>().sprite = iconAffinityIcons[3];
                    break;
                case Item.AffinityType.Physical:
                    primaryAffinityBackground.gameObject.SetActive(true);
                    primaryAffinityBackground.color = iconAffintiyBackgroundColors[4];
                    primaryAffinityBackground.transform.Find("Icon").GetComponent<Image>().color = iconAffintiyColors[4];
                    primaryAffinityBackground.transform.Find("Icon").GetComponent<Image>().sprite = iconAffinityIcons[4];
                    break;
                case Item.AffinityType.Bleed:
                    primaryAffinityBackground.gameObject.SetActive(true);
                    primaryAffinityBackground.color = iconAffintiyBackgroundColors[5];
                    primaryAffinityBackground.transform.Find("Icon").GetComponent<Image>().color = iconAffintiyColors[5];
                    primaryAffinityBackground.transform.Find("Icon").GetComponent<Image>().sprite = iconAffinityIcons[5];
                    break;
                case Item.AffinityType.Poison:
                    primaryAffinityBackground.gameObject.SetActive(true);
                    primaryAffinityBackground.color = iconAffintiyBackgroundColors[6];
                    primaryAffinityBackground.transform.Find("Icon").GetComponent<Image>().color = iconAffintiyColors[6];
                    primaryAffinityBackground.transform.Find("Icon").GetComponent<Image>().sprite = iconAffinityIcons[6];
                    break;
                case Item.AffinityType.Stun:
                    primaryAffinityBackground.gameObject.SetActive(true);
                    primaryAffinityBackground.color = iconAffintiyBackgroundColors[7];
                    primaryAffinityBackground.transform.Find("Icon").GetComponent<Image>().color = iconAffintiyColors[7];
                    primaryAffinityBackground.transform.Find("Icon").GetComponent<Image>().sprite = iconAffinityIcons[7];
                    break;
                case Item.AffinityType.Knockback:
                    primaryAffinityBackground.gameObject.SetActive(true);
                    primaryAffinityBackground.color = iconAffintiyBackgroundColors[8];
                    primaryAffinityBackground.transform.Find("Icon").GetComponent<Image>().color = iconAffintiyColors[8];
                    primaryAffinityBackground.transform.Find("Icon").GetComponent<Image>().sprite = iconAffinityIcons[8];
                    break;
                default:
                    break;
            }

            Image secondaryAffinityBackground = imageBackground.transform.Find("Affinity_Secondary").GetComponent<Image>();
            switch (targetItem.affinitySecondary)
            {
                case Item.AffinityType.None:
                    secondaryAffinityBackground.gameObject.SetActive(false);
                    break;
                case Item.AffinityType.Fire:
                    secondaryAffinityBackground.gameObject.SetActive(true);
                    secondaryAffinityBackground.color = iconAffintiyBackgroundColors[0];
                    secondaryAffinityBackground.transform.Find("Icon").GetComponent<Image>().color = iconAffintiyColors[0];
                    secondaryAffinityBackground.transform.Find("Icon").GetComponent<Image>().sprite = iconAffinityIcons[0];
                    break;
                case Item.AffinityType.Ice:
                    secondaryAffinityBackground.gameObject.SetActive(true);
                    secondaryAffinityBackground.color = iconAffintiyBackgroundColors[1];
                    secondaryAffinityBackground.transform.Find("Icon").GetComponent<Image>().color = iconAffintiyColors[1];
                    secondaryAffinityBackground.transform.Find("Icon").GetComponent<Image>().sprite = iconAffinityIcons[1];
                    break;
                case Item.AffinityType.Earth:
                    secondaryAffinityBackground.gameObject.SetActive(true);
                    secondaryAffinityBackground.color = iconAffintiyBackgroundColors[2];
                    secondaryAffinityBackground.transform.Find("Icon").GetComponent<Image>().color = iconAffintiyColors[2];
                    secondaryAffinityBackground.transform.Find("Icon").GetComponent<Image>().sprite = iconAffinityIcons[2];
                    break;
                case Item.AffinityType.Wind:
                    secondaryAffinityBackground.gameObject.SetActive(true);
                    secondaryAffinityBackground.color = iconAffintiyBackgroundColors[3];
                    secondaryAffinityBackground.transform.Find("Icon").GetComponent<Image>().color = iconAffintiyColors[3];
                    secondaryAffinityBackground.transform.Find("Icon").GetComponent<Image>().sprite = iconAffinityIcons[3];
                    break;
                case Item.AffinityType.Physical:
                    secondaryAffinityBackground.gameObject.SetActive(true);
                    secondaryAffinityBackground.color = iconAffintiyBackgroundColors[4];
                    secondaryAffinityBackground.transform.Find("Icon").GetComponent<Image>().color = iconAffintiyColors[4];
                    secondaryAffinityBackground.transform.Find("Icon").GetComponent<Image>().sprite = iconAffinityIcons[4];
                    break;
                case Item.AffinityType.Bleed:
                    secondaryAffinityBackground.gameObject.SetActive(true);
                    secondaryAffinityBackground.color = iconAffintiyBackgroundColors[5];
                    secondaryAffinityBackground.transform.Find("Icon").GetComponent<Image>().color = iconAffintiyColors[5];
                    secondaryAffinityBackground.transform.Find("Icon").GetComponent<Image>().sprite = iconAffinityIcons[5];
                    break;
                case Item.AffinityType.Poison:
                    secondaryAffinityBackground.gameObject.SetActive(true);
                    secondaryAffinityBackground.color = iconAffintiyBackgroundColors[6];
                    secondaryAffinityBackground.transform.Find("Icon").GetComponent<Image>().color = iconAffintiyColors[6];
                    secondaryAffinityBackground.transform.Find("Icon").GetComponent<Image>().sprite = iconAffinityIcons[6];
                    break;
                case Item.AffinityType.Stun:
                    secondaryAffinityBackground.gameObject.SetActive(true);
                    secondaryAffinityBackground.color = iconAffintiyBackgroundColors[7];
                    secondaryAffinityBackground.transform.Find("Icon").GetComponent<Image>().color = iconAffintiyColors[7];
                    secondaryAffinityBackground.transform.Find("Icon").GetComponent<Image>().sprite = iconAffinityIcons[7];
                    break;
                case Item.AffinityType.Knockback:
                    secondaryAffinityBackground.gameObject.SetActive(true);
                    secondaryAffinityBackground.color = iconAffintiyBackgroundColors[8];
                    secondaryAffinityBackground.transform.Find("Icon").GetComponent<Image>().color = iconAffintiyColors[8];
                    secondaryAffinityBackground.transform.Find("Icon").GetComponent<Image>().sprite = iconAffinityIcons[8];
                    break;
                default:
                    break;
            }

            Image tertiaryAffinityBackground = imageBackground.transform.Find("Affinity_Tertiary").GetComponent<Image>();
            switch (targetItem.affinityTertiary)
            {
                case Item.AffinityType.None:
                    tertiaryAffinityBackground.gameObject.SetActive(false);
                    break;
                case Item.AffinityType.Fire:
                    tertiaryAffinityBackground.gameObject.SetActive(true);
                    tertiaryAffinityBackground.color = iconAffintiyBackgroundColors[0];
                    tertiaryAffinityBackground.transform.Find("Icon").GetComponent<Image>().color = iconAffintiyColors[0];
                    tertiaryAffinityBackground.transform.Find("Icon").GetComponent<Image>().sprite = iconAffinityIcons[0];
                    break;
                case Item.AffinityType.Ice:
                    tertiaryAffinityBackground.gameObject.SetActive(true);
                    tertiaryAffinityBackground.color = iconAffintiyBackgroundColors[1];
                    tertiaryAffinityBackground.transform.Find("Icon").GetComponent<Image>().color = iconAffintiyColors[1];
                    tertiaryAffinityBackground.transform.Find("Icon").GetComponent<Image>().sprite = iconAffinityIcons[1];
                    break;
                case Item.AffinityType.Earth:
                    tertiaryAffinityBackground.gameObject.SetActive(true);
                    tertiaryAffinityBackground.color = iconAffintiyBackgroundColors[2];
                    tertiaryAffinityBackground.transform.Find("Icon").GetComponent<Image>().color = iconAffintiyColors[2];
                    tertiaryAffinityBackground.transform.Find("Icon").GetComponent<Image>().sprite = iconAffinityIcons[2];
                    break;
                case Item.AffinityType.Wind:
                    tertiaryAffinityBackground.gameObject.SetActive(true);
                    tertiaryAffinityBackground.color = iconAffintiyBackgroundColors[3];
                    tertiaryAffinityBackground.transform.Find("Icon").GetComponent<Image>().color = iconAffintiyColors[3];
                    tertiaryAffinityBackground.transform.Find("Icon").GetComponent<Image>().sprite = iconAffinityIcons[3];
                    break;
                case Item.AffinityType.Physical:
                    tertiaryAffinityBackground.gameObject.SetActive(true);
                    tertiaryAffinityBackground.color = iconAffintiyBackgroundColors[4];
                    tertiaryAffinityBackground.transform.Find("Icon").GetComponent<Image>().color = iconAffintiyColors[4];
                    tertiaryAffinityBackground.transform.Find("Icon").GetComponent<Image>().sprite = iconAffinityIcons[4];
                    break;
                case Item.AffinityType.Bleed:
                    tertiaryAffinityBackground.gameObject.SetActive(true);
                    tertiaryAffinityBackground.color = iconAffintiyBackgroundColors[5];
                    tertiaryAffinityBackground.transform.Find("Icon").GetComponent<Image>().color = iconAffintiyColors[5];
                    tertiaryAffinityBackground.transform.Find("Icon").GetComponent<Image>().sprite = iconAffinityIcons[5];
                    break;
                case Item.AffinityType.Poison:
                    tertiaryAffinityBackground.gameObject.SetActive(true);
                    tertiaryAffinityBackground.color = iconAffintiyBackgroundColors[6];
                    tertiaryAffinityBackground.transform.Find("Icon").GetComponent<Image>().color = iconAffintiyColors[6];
                    tertiaryAffinityBackground.transform.Find("Icon").GetComponent<Image>().sprite = iconAffinityIcons[6];
                    break;
                case Item.AffinityType.Stun:
                    tertiaryAffinityBackground.gameObject.SetActive(true);
                    tertiaryAffinityBackground.color = iconAffintiyBackgroundColors[7];
                    tertiaryAffinityBackground.transform.Find("Icon").GetComponent<Image>().color = iconAffintiyColors[7];
                    tertiaryAffinityBackground.transform.Find("Icon").GetComponent<Image>().sprite = iconAffinityIcons[7];
                    break;
                case Item.AffinityType.Knockback:
                    tertiaryAffinityBackground.gameObject.SetActive(true);
                    tertiaryAffinityBackground.color = iconAffintiyBackgroundColors[8];
                    tertiaryAffinityBackground.transform.Find("Icon").GetComponent<Image>().color = iconAffintiyColors[8];
                    tertiaryAffinityBackground.transform.Find("Icon").GetComponent<Image>().sprite = iconAffinityIcons[8];
                    break;
                default:
                    break;
            }

            Image secondaryAffinityBackgroundTwo = imageBackground.transform.Find("Affinity_Secondary").Find("Mask").Find("Affinity_Secondary_LeftHalf").GetComponent<Image>();
            if (targetItem.affinitySecondaryMultiElement)
            {
                secondaryAffinityBackgroundTwo.gameObject.SetActive(true);
                switch (targetItem.affinityPrimary)
                {
                    case Item.AffinityType.None:
                        secondaryAffinityBackgroundTwo.gameObject.SetActive(false);
                        break;
                    case Item.AffinityType.Fire:
                        secondaryAffinityBackgroundTwo.gameObject.SetActive(true);
                        secondaryAffinityBackgroundTwo.color = iconAffintiyBackgroundColors[0];
                        secondaryAffinityBackgroundTwo.transform.Find("Icon").GetComponent<Image>().color = iconAffintiyColors[0];
                        secondaryAffinityBackgroundTwo.transform.Find("Icon").GetComponent<Image>().sprite = iconAffinityIcons[0];
                        break;
                    case Item.AffinityType.Ice:
                        secondaryAffinityBackgroundTwo.gameObject.SetActive(true);
                        secondaryAffinityBackgroundTwo.color = iconAffintiyBackgroundColors[1];
                        secondaryAffinityBackgroundTwo.transform.Find("Icon").GetComponent<Image>().color = iconAffintiyColors[1];
                        secondaryAffinityBackgroundTwo.transform.Find("Icon").GetComponent<Image>().sprite = iconAffinityIcons[1];
                        break;
                    case Item.AffinityType.Earth:
                        secondaryAffinityBackgroundTwo.gameObject.SetActive(true);
                        secondaryAffinityBackgroundTwo.color = iconAffintiyBackgroundColors[2];
                        secondaryAffinityBackgroundTwo.transform.Find("Icon").GetComponent<Image>().color = iconAffintiyColors[2];
                        secondaryAffinityBackgroundTwo.transform.Find("Icon").GetComponent<Image>().sprite = iconAffinityIcons[2];
                        break;
                    case Item.AffinityType.Wind:
                        secondaryAffinityBackgroundTwo.gameObject.SetActive(true);
                        secondaryAffinityBackgroundTwo.color = iconAffintiyBackgroundColors[3];
                        secondaryAffinityBackgroundTwo.transform.Find("Icon").GetComponent<Image>().color = iconAffintiyColors[3];
                        secondaryAffinityBackgroundTwo.transform.Find("Icon").GetComponent<Image>().sprite = iconAffinityIcons[3];
                        break;
                    case Item.AffinityType.Physical:
                        secondaryAffinityBackgroundTwo.gameObject.SetActive(true);
                        secondaryAffinityBackgroundTwo.color = iconAffintiyBackgroundColors[4];
                        secondaryAffinityBackgroundTwo.transform.Find("Icon").GetComponent<Image>().color = iconAffintiyColors[4];
                        secondaryAffinityBackgroundTwo.transform.Find("Icon").GetComponent<Image>().sprite = iconAffinityIcons[4];
                        break;
                    case Item.AffinityType.Bleed:
                        secondaryAffinityBackgroundTwo.gameObject.SetActive(true);
                        secondaryAffinityBackgroundTwo.color = iconAffintiyBackgroundColors[5];
                        secondaryAffinityBackgroundTwo.transform.Find("Icon").GetComponent<Image>().color = iconAffintiyColors[5];
                        secondaryAffinityBackgroundTwo.transform.Find("Icon").GetComponent<Image>().sprite = iconAffinityIcons[5];
                        break;
                    case Item.AffinityType.Poison:
                        secondaryAffinityBackgroundTwo.gameObject.SetActive(true);
                        secondaryAffinityBackgroundTwo.color = iconAffintiyBackgroundColors[6];
                        secondaryAffinityBackgroundTwo.transform.Find("Icon").GetComponent<Image>().color = iconAffintiyColors[6];
                        secondaryAffinityBackgroundTwo.transform.Find("Icon").GetComponent<Image>().sprite = iconAffinityIcons[6];
                        break;
                    case Item.AffinityType.Stun:
                        secondaryAffinityBackgroundTwo.gameObject.SetActive(true);
                        secondaryAffinityBackgroundTwo.color = iconAffintiyBackgroundColors[7];
                        secondaryAffinityBackgroundTwo.transform.Find("Icon").GetComponent<Image>().color = iconAffintiyColors[7];
                        secondaryAffinityBackgroundTwo.transform.Find("Icon").GetComponent<Image>().sprite = iconAffinityIcons[7];
                        break;
                    case Item.AffinityType.Knockback:
                        secondaryAffinityBackgroundTwo.gameObject.SetActive(true);
                        secondaryAffinityBackgroundTwo.color = iconAffintiyBackgroundColors[8];
                        secondaryAffinityBackgroundTwo.transform.Find("Icon").GetComponent<Image>().color = iconAffintiyColors[8];
                        secondaryAffinityBackgroundTwo.transform.Find("Icon").GetComponent<Image>().sprite = iconAffinityIcons[8];
                        break;
                    default:
                        break;
                }
            }
            else
                secondaryAffinityBackgroundTwo.gameObject.SetActive(false);

            Image tertiaryAffinityBackgroundTwo = imageBackground.transform.Find("Affinity_Tertiary").Find("Mask").Find("Affinity_Secondary_LeftHalf").GetComponent<Image>();
            if (targetItem.affinityTertiaryMultiElement)
            {
                tertiaryAffinityBackgroundTwo.gameObject.SetActive(true);
                switch (targetItem.affinityPrimary)
                {
                    case Item.AffinityType.None:
                        tertiaryAffinityBackgroundTwo.gameObject.SetActive(false);
                        break;
                    case Item.AffinityType.Fire:
                        tertiaryAffinityBackgroundTwo.gameObject.SetActive(true);
                        tertiaryAffinityBackgroundTwo.color = iconAffintiyBackgroundColors[0];
                        tertiaryAffinityBackgroundTwo.transform.Find("Icon").GetComponent<Image>().color = iconAffintiyColors[0];
                        tertiaryAffinityBackgroundTwo.transform.Find("Icon").GetComponent<Image>().sprite = iconAffinityIcons[0];
                        break;
                    case Item.AffinityType.Ice:
                        tertiaryAffinityBackgroundTwo.gameObject.SetActive(true);
                        tertiaryAffinityBackgroundTwo.color = iconAffintiyBackgroundColors[1];
                        tertiaryAffinityBackgroundTwo.transform.Find("Icon").GetComponent<Image>().color = iconAffintiyColors[1];
                        tertiaryAffinityBackgroundTwo.transform.Find("Icon").GetComponent<Image>().sprite = iconAffinityIcons[1];
                        break;
                    case Item.AffinityType.Earth:
                        tertiaryAffinityBackgroundTwo.gameObject.SetActive(true);
                        tertiaryAffinityBackgroundTwo.color = iconAffintiyBackgroundColors[2];
                        tertiaryAffinityBackgroundTwo.transform.Find("Icon").GetComponent<Image>().color = iconAffintiyColors[2];
                        tertiaryAffinityBackgroundTwo.transform.Find("Icon").GetComponent<Image>().sprite = iconAffinityIcons[2];
                        break;
                    case Item.AffinityType.Wind:
                        tertiaryAffinityBackgroundTwo.gameObject.SetActive(true);
                        tertiaryAffinityBackgroundTwo.color = iconAffintiyBackgroundColors[3];
                        tertiaryAffinityBackgroundTwo.transform.Find("Icon").GetComponent<Image>().color = iconAffintiyColors[3];
                        tertiaryAffinityBackgroundTwo.transform.Find("Icon").GetComponent<Image>().sprite = iconAffinityIcons[3];
                        break;
                    case Item.AffinityType.Physical:
                        tertiaryAffinityBackgroundTwo.gameObject.SetActive(true);
                        tertiaryAffinityBackgroundTwo.color = iconAffintiyBackgroundColors[4];
                        tertiaryAffinityBackgroundTwo.transform.Find("Icon").GetComponent<Image>().color = iconAffintiyColors[4];
                        tertiaryAffinityBackgroundTwo.transform.Find("Icon").GetComponent<Image>().sprite = iconAffinityIcons[4];
                        break;
                    case Item.AffinityType.Bleed:
                        tertiaryAffinityBackgroundTwo.gameObject.SetActive(true);
                        tertiaryAffinityBackgroundTwo.color = iconAffintiyBackgroundColors[5];
                        tertiaryAffinityBackgroundTwo.transform.Find("Icon").GetComponent<Image>().color = iconAffintiyColors[5];
                        tertiaryAffinityBackgroundTwo.transform.Find("Icon").GetComponent<Image>().sprite = iconAffinityIcons[5];
                        break;
                    case Item.AffinityType.Poison:
                        tertiaryAffinityBackgroundTwo.gameObject.SetActive(true);
                        tertiaryAffinityBackgroundTwo.color = iconAffintiyBackgroundColors[6];
                        tertiaryAffinityBackgroundTwo.transform.Find("Icon").GetComponent<Image>().color = iconAffintiyColors[6];
                        tertiaryAffinityBackgroundTwo.transform.Find("Icon").GetComponent<Image>().sprite = iconAffinityIcons[6];
                        break;
                    case Item.AffinityType.Stun:
                        tertiaryAffinityBackgroundTwo.gameObject.SetActive(true);
                        tertiaryAffinityBackgroundTwo.color = iconAffintiyBackgroundColors[7];
                        tertiaryAffinityBackgroundTwo.transform.Find("Icon").GetComponent<Image>().color = iconAffintiyColors[7];
                        tertiaryAffinityBackgroundTwo.transform.Find("Icon").GetComponent<Image>().sprite = iconAffinityIcons[7];
                        break;
                    case Item.AffinityType.Knockback:
                        tertiaryAffinityBackgroundTwo.gameObject.SetActive(true);
                        tertiaryAffinityBackgroundTwo.color = iconAffintiyBackgroundColors[8];
                        tertiaryAffinityBackgroundTwo.transform.Find("Icon").GetComponent<Image>().color = iconAffintiyColors[8];
                        tertiaryAffinityBackgroundTwo.transform.Find("Icon").GetComponent<Image>().sprite = iconAffinityIcons[8];
                        break;
                    default:
                        break;
                }
            }
            else
                tertiaryAffinityBackgroundTwo.gameObject.SetActive(false);
        }
        else
        {
            imageBackground.transform.Find("Affinity_Primary").GetComponent<Image>().gameObject.SetActive(false);
            imageBackground.transform.Find("Affinity_Secondary").GetComponent<Image>().gameObject.SetActive(false);
            imageBackground.transform.Find("Affinity_Tertiary").GetComponent<Image>().gameObject.SetActive(false);
        }


        // Clear the trait aspect of the popup.
        ClearPopUp();
        PopulatePopUp(targetItem);

        PopUpTraitHeight += descriptionBox.sizeDelta.y;
        traitContainer.sizeDelta = new Vector2(traitContainer.sizeDelta.x, PopUpTraitHeight);
        if(!primaryPopUp)
            primaryPopUp = GetComponent<RectTransform>();
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
                    traitText.GetComponentInChildren<Text>().text = string.Format(GetTraitPrefix(item.itemTraits[index].traitBonus) + "{0:0} Armor", item.itemTraits[index].traitBonus * item.itemTraits[index].traitBonusMultiplier);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[0];
                    if (item.itemType == Item.ItemType.Helmet || item.itemType == Item.ItemType.Armor || item.itemType == Item.ItemType.Legs || item.itemType == Item.ItemType.Shield)
                    {
                        traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(200f, 0f);
                        Destroy(traitText);
                    }
                    break;
                case ItemTrait.TraitType.HealthRegen:
                    traitText.GetComponentInChildren<Text>().text = string.Format(GetTraitPrefix(item.itemTraits[index].traitBonus) + "{0:0.0} Health Regen per Second", item.itemTraits[index].traitBonus * item.itemTraits[index].traitBonusMultiplier);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[3];
                    break;
                case ItemTrait.TraitType.CooldownReduction:
                    traitText.GetComponentInChildren<Text>().text = string.Format(GetTraitPrefix(item.itemTraits[index].traitBonus) + "{0:0}% Cooldown Reduction", item.itemTraits[index].traitBonus * item.itemTraits[index].traitBonusMultiplier * 100);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[16];
                    break;
                case ItemTrait.TraitType.AflameResistance:
                    traitText.GetComponentInChildren<Text>().text = string.Format(GetTraitPrefix(item.itemTraits[index].traitBonus) + "{0:0}% Fire Resist", item.itemTraits[index].traitBonus * item.itemTraits[index].traitBonusMultiplier * 100);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.FrostbiteResistance:
                    traitText.GetComponentInChildren<Text>().text = string.Format(GetTraitPrefix(item.itemTraits[index].traitBonus) + "{0:0}% Frostbite Resist", item.itemTraits[index].traitBonus * item.itemTraits[index].traitBonusMultiplier * 100);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[5];
                    break;
                case ItemTrait.TraitType.OverchargeResistance:
                    traitText.GetComponentInChildren<Text>().text = string.Format(GetTraitPrefix(item.itemTraits[index].traitBonus) + "{0:0}% Lightning Resist", item.itemTraits[index].traitBonus * item.itemTraits[index].traitBonusMultiplier * 100);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[6];
                    break;
                case ItemTrait.TraitType.OvergrowthResistance:
                    traitText.GetComponentInChildren<Text>().text = string.Format(GetTraitPrefix(item.itemTraits[index].traitBonus) + "{0:0}% Nature Resist", item.itemTraits[index].traitBonus * item.itemTraits[index].traitBonusMultiplier * 100);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[7];
                    break;
                case ItemTrait.TraitType.WindshearResistance:
                    traitText.GetComponentInChildren<Text>().text = string.Format(GetTraitPrefix(item.itemTraits[index].traitBonus) + "{0:0}% Wind Resist", item.itemTraits[index].traitBonus * item.itemTraits[index].traitBonusMultiplier * 100);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[8];
                    break;
                case ItemTrait.TraitType.SunderResistance:
                    traitText.GetComponentInChildren<Text>().text = string.Format(GetTraitPrefix(item.itemTraits[index].traitBonus) + "{0:0}% Earth Resist", item.itemTraits[index].traitBonus * item.itemTraits[index].traitBonusMultiplier * 100);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[9];
                    break;
                case ItemTrait.TraitType.BleedResistance:
                    traitText.GetComponentInChildren<Text>().text = string.Format(GetTraitPrefix(item.itemTraits[index].traitBonus) + "{0:0}% Bleed Resist", item.itemTraits[index].traitBonus * item.itemTraits[index].traitBonusMultiplier * 100);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[10];
                    break;
                case ItemTrait.TraitType.PoisonResistance:
                    traitText.GetComponentInChildren<Text>().text = string.Format(GetTraitPrefix(item.itemTraits[index].traitBonus) + "{0:0}% Poison Resist", item.itemTraits[index].traitBonus * item.itemTraits[index].traitBonusMultiplier * 100);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[11];
                    break;
                case ItemTrait.TraitType.AsleepResistance:
                    traitText.GetComponentInChildren<Text>().text = string.Format(GetTraitPrefix(item.itemTraits[index].traitBonus) + "{0:0}% Sleep Resist", item.itemTraits[index].traitBonus * item.itemTraits[index].traitBonusMultiplier * 100);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[12];
                    break;
                case ItemTrait.TraitType.StunResistance:
                    traitText.GetComponentInChildren<Text>().text = string.Format(GetTraitPrefix(item.itemTraits[index].traitBonus) + "{0:0}% Stun Resist", item.itemTraits[index].traitBonus * item.itemTraits[index].traitBonusMultiplier * 100);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[13];
                    break;
                case ItemTrait.TraitType.KnockbackResistance:
                    traitText.GetComponentInChildren<Text>().text = string.Format(GetTraitPrefix(item.itemTraits[index].traitBonus) + "{0:0}% Knockback Resist", item.itemTraits[index].traitBonus * item.itemTraits[index].traitBonusMultiplier * 100);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[14];
                    break;
                case ItemTrait.TraitType.AttackSpeed:
                    traitText.GetComponentInChildren<Text>().text = string.Format(GetTraitPrefix(item.itemTraits[index].traitBonus) + "{0:0}% Attack Speed", item.itemTraits[index].traitBonus * item.itemTraits[index].traitBonusMultiplier * 100);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[15];
                    break;
                case ItemTrait.TraitType.HealthFlat:
                    traitText.GetComponentInChildren<Text>().text = string.Format(GetTraitPrefix(item.itemTraits[index].traitBonus) + "{0:0} Health", item.itemTraits[index].traitBonus * item.itemTraits[index].traitBonusMultiplier);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[3];
                    if (item.itemType == Item.ItemType.Helmet || item.itemType == Item.ItemType.Armor || item.itemType == Item.ItemType.Legs || item.itemType == Item.ItemType.Shield)
                    {
                        traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(200f, 0f);
                        Destroy(traitText);
                    }
                    break;
                case ItemTrait.TraitType.HealthPercent:
                    traitText.GetComponentInChildren<Text>().text = string.Format(GetTraitPrefix(item.itemTraits[index].traitBonus) + "{0:0}% Increased Health", item.itemTraits[index].traitBonus * 100 * item.itemTraits[index].traitBonusMultiplier);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[3];
                    break;
                case ItemTrait.TraitType.HealingOnHit:
                    traitText.GetComponentInChildren<Text>().text = string.Format(GetTraitPrefix(item.itemTraits[index].traitBonus) + "{0:0} Health on Hit", item.itemTraits[index].traitBonus * item.itemTraits[index].traitBonusMultiplier);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[2];
                    break;
                case ItemTrait.TraitType.HealingOnKill:
                    traitText.GetComponentInChildren<Text>().text = string.Format(GetTraitPrefix(item.itemTraits[index].traitBonus) + "{0:0} Health on Kill", item.itemTraits[index].traitBonus * item.itemTraits[index].traitBonusMultiplier);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[2];
                    break;
                case ItemTrait.TraitType.MoveSpeed:
                    traitText.GetComponentInChildren<Text>().text = string.Format(GetTraitPrefix(item.itemTraits[index].traitBonus) + "{0:0}% Movespeed", item.itemTraits[index].traitBonus * 100 * item.itemTraits[index].traitBonusMultiplier);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[16];
                    if (item.itemType == Item.ItemType.Helmet || item.itemType == Item.ItemType.Armor || item.itemType == Item.ItemType.Legs || item.itemType == Item.ItemType.Shield)
                    {
                        traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(200f, 0f);
                        Destroy(traitText);
                    }
                    break;
                case ItemTrait.TraitType.Jumps:
                    traitText.GetComponentInChildren<Text>().text = string.Format(GetTraitPrefix(item.itemTraits[index].traitBonus) + "{0:0} Extra Jumps", item.itemTraits[index].traitBonus * item.itemTraits[index].traitBonusMultiplier);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[16];
                    break;
                case ItemTrait.TraitType.CritChance:
                    traitText.GetComponentInChildren<Text>().text = string.Format(GetTraitPrefix(item.itemTraits[index].traitBonus) + "{0:0}% Critical Strike Chance", item.itemTraits[index].traitBonus * item.itemTraits[index].traitBonusMultiplier * 100);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[1];
                    break;
                case ItemTrait.TraitType.CritDamage:
                    traitText.GetComponentInChildren<Text>().text = string.Format(GetTraitPrefix(item.itemTraits[index].traitBonus) + "{0:0}% Critical Strike Damage", item.itemTraits[index].traitBonus * item.itemTraits[index].traitBonusMultiplier * 100);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[1];
                    break;
                case ItemTrait.TraitType.FlatDamageReduction:
                    traitText.GetComponentInChildren<Text>().text = string.Format(GetTraitPrefix(item.itemTraits[index].traitBonus) + "{0:0} Flat Damage Reduction", item.itemTraits[index].traitBonus * item.itemTraits[index].traitBonusMultiplier);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[17];
                    if (item.itemType == Item.ItemType.Helmet || item.itemType == Item.ItemType.Armor || item.itemType == Item.ItemType.Legs || item.itemType == Item.ItemType.Shield)
                    {
                        traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(200f, 0f);
                        Destroy(traitText);
                    }
                    break;
                case ItemTrait.TraitType.SpellDamage:
                    traitText.GetComponentInChildren<Text>().text = string.Format(GetTraitPrefix(item.itemTraits[index].traitBonus) + "{0:0}% Increased Spell Damage", item.itemTraits[index].traitBonus * item.itemTraits[index].traitBonusMultiplier * 100);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[16];
                    break;
                case ItemTrait.TraitType.BasicAttackAmp:
                    traitText.GetComponentInChildren<Text>().text = string.Format(GetTraitPrefix(item.itemTraits[index].traitBonus) + "{0:0}% Increased Basic Attack Damage", item.itemTraits[index].traitBonus * item.itemTraits[index].traitBonusMultiplier * 100);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[1];
                    break;
                case ItemTrait.TraitType.Luck:
                    traitText.GetComponentInChildren<Text>().text = string.Format(GetTraitPrefix(item.itemTraits[index].traitBonus) + "{0:0} Luck", item.itemTraits[index].traitBonus * item.itemTraits[index].traitBonusMultiplier);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[18];
                    break;
                case ItemTrait.TraitType.BonusStacksOnHit:
                    traitText.GetComponentInChildren<Text>().text = string.Format(GetTraitPrefix(item.itemTraits[index].traitBonus) + "{0:0} Bonus Stacks On Hit", item.itemTraits[index].traitBonus * item.itemTraits[index].traitBonusMultiplier);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[19];
                    break;
                case ItemTrait.TraitType.FireExplosionOnKill:
                    traitText.GetComponentInChildren<Text>().text = "Consuming Blaze" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    // traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(200f, 14f);
                    //traitText.transform.Find("Text").GetComponent<RectTransform>().sizeDelta = new Vector2(400f, 30f);
                    break;
                case ItemTrait.TraitType.MoreAflameStacksOnHitThreshold:
                    traitText.GetComponentInChildren<Text>().text = "Raging Inferno" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.BurnDoesMaxHpDamageAtThreshold:
                    traitText.GetComponentInChildren<Text>().text = "Fire Overwhelming" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.BasicAttacksShredArmorOnAflame:
                    traitText.GetComponentInChildren<Text>().text = "Shredding Flames" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.FlameVamperism:
                    traitText.GetComponentInChildren<Text>().text = "Flame Vamperism" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.RingOfFireOnHit:
                    traitText.GetComponentInChildren<Text>().text = "Matrimony of Flame" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.AflameToSunderStackOnEarthSpell:
                    traitText.GetComponentInChildren<Text>().text = "Wrath of the <color=#B0946C>Blazing Earth" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.SunderFurtherDecreasesFireResist:
                    traitText.GetComponentInChildren<Text>().text = "Deep Rooted <color=#B0946C>Flames" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.AflameSunderCritsSummonFireballs:
                    traitText.GetComponentInChildren<Text>().text = "Devasting Flame <color=#B0946C>Geyser" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.AflameWindshearWindAttacksGainCritOnBurningTarget:
                    traitText.GetComponentInChildren<Text>().text = "Blazing <color=#ABD1E0>Exposure" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.AflameWindshearSummonFirePillarsOnHit:
                    traitText.GetComponentInChildren<Text>().text = "Flaring <color=#ABD1E0>Winds" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.AflameWindshearWindSpellsAddFireStacks:
                    traitText.GetComponentInChildren<Text>().text = "Roaring <color=#ABD1E0>Blaze" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.AflamePhysicalAddFireStacksOnHit:
                    traitText.GetComponentInChildren<Text>().text = "Searing <color=#E94453>Metal" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.AflamePhysicalDamageAmpOnBurningTarget:
                    traitText.GetComponentInChildren<Text>().text = "Smoldering <color=#E94453>Steel" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.AflamePhysicalBladeExplosionOnKill:
                    traitText.GetComponentInChildren<Text>().text = "Red-hot <color=#E94453>Metallic Implosion" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.AflamePhysicalBigHitsAddAflame:
                    traitText.GetComponentInChildren<Text>().text = "Searing <color=#E94453>Strikes" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.AflameBleedIncreasesFlameCritChance:
                    traitText.GetComponentInChildren<Text>().text = "Searing <color=#AB181D>Wounds" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.AflameBleedFireDamageAmpOnBleedThreshold:
                    traitText.GetComponentInChildren<Text>().text = "Boiling <color=#AB181D>Blood" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.AflameBleedAflameAddsBleedAtThreshhold:
                    traitText.GetComponentInChildren<Text>().text = "Blaze of <color=#AB181D>Exsanguination" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.AflameBleedAflameRemovesBleedResist:
                    traitText.GetComponentInChildren<Text>().text = "Flames of <color=#AB181D>Hemorrhage" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.AflameBleedDamageAmpOnDoubleThreshhold:
                    traitText.GetComponentInChildren<Text>().text = "Consuming <color=#AB181D>Blaze" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.AflamePoisonBurningEnemySpreadPoisonStacksOnDeath:
                    traitText.GetComponentInChildren<Text>().text = "Fiery <color=#93D916>Contagion" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.AflamePoisonGreviousWoundsOnStackThreshold:
                    traitText.GetComponentInChildren<Text>().text = "Virulent <color=#93D916>Toxins" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.AflamePoisonPoisonReducesFireResist:
                    traitText.GetComponentInChildren<Text>().text = "Incendiary <color=#93D916>Plague" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.AflamePoisonFireSpellsSummonsPoisonBurst:
                    traitText.GetComponentInChildren<Text>().text = "Infernal <color=#93D916>Epidemic" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.AflamePoisonFireAmpsPoison:
                    traitText.GetComponentInChildren<Text>().text = "Scalding <color=#93D916>Poison" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.AflamePoisonPoisonCloudOnFireKill:
                    traitText.GetComponentInChildren<Text>().text = "Toxic <color=#93D916>Smoke" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.AflameStunPeriodBurnStun:
                    traitText.GetComponentInChildren<Text>().text = "Dazing <color=#FFF04F>Flames" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.AflameStunStunOnThreshold:
                    traitText.GetComponentInChildren<Text>().text = "Stunning <color=#FFF04F>Inferno" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.AflameStunStunReducesFireResistance:
                    traitText.GetComponentInChildren<Text>().text = "Confounding <color=#FFF04F>Conflagration" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.AflameStunStunAmpsBurnDamage:
                    traitText.GetComponentInChildren<Text>().text = "Dazing Flare <color=#FFF04F>Amplification" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.AflameKnockbackAflameReducesKnockbackResist:
                    traitText.GetComponentInChildren<Text>().text = "Staggering <color=#1F86CA>Flames" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.AflameKnockbackAflameSpellsOnKnockbackedTargetExplode:
                    traitText.GetComponentInChildren<Text>().text = "Infernal <color=#1F86CA>Waves" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.AflameKnockbackKnockbackAmpsFireDamage:
                    traitText.GetComponentInChildren<Text>().text = "Searing <color=#1F86CA>Impulse" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.IceFreezeAtStackThreshold:
                    traitText.GetComponentInChildren<Text>().text = "Deep Freeze" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[5];
                    break;
                case ItemTrait.TraitType.IceAmpAllDamageAtThreshold:
                    traitText.GetComponentInChildren<Text>().text = "Biting Cold" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[5];
                    break;
                case ItemTrait.TraitType.IceBasicAttacksConsumeStacksAtThreshold:
                    traitText.GetComponentInChildren<Text>().text = "Shattered Ice" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[5];
                    break;
                case ItemTrait.TraitType.IceEnemyAttacksWeakendAtThreshold:
                    traitText.GetComponentInChildren<Text>().text = "Brittle Steel" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[5];
                    break;
                case ItemTrait.TraitType.IceEnemiesGainFrostbiteOnStrikingYou:
                    traitText.GetComponentInChildren<Text>().text = "Frigid Armor" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[5];
                    break;
                case ItemTrait.TraitType.IceAddStacksToNearbyEnemies:
                    traitText.GetComponentInChildren<Text>().text = "Arctic Aura" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[5];
                    break;
                case ItemTrait.TraitType.IceAmpFrostbiteDamage:
                    traitText.GetComponentInChildren<Text>().text = "Bone Chilling Frost" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[5];
                    break;
                case ItemTrait.TraitType.IceEarthFrostToEarthBonusDamage:
                    traitText.GetComponentInChildren<Text>().text = "Frosted <color=#B0946C>Earth" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[5];
                    break;
                case ItemTrait.TraitType.IceEarthSunderAmpsIceDamage:
                    traitText.GetComponentInChildren<Text>().text = "Brittle <color=#B0946C>Flesh" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[5];
                    break;
                case ItemTrait.TraitType.IceEarthIceDOTAtThreshold:
                    traitText.GetComponentInChildren<Text>().text = "Icy <color=#B0946C>Reverberations" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[5];
                    break;
                case ItemTrait.TraitType.IceEarthEarthSpellBonusCritDamage:
                    traitText.GetComponentInChildren<Text>().text = "Devasting <color=#B0946C>Thaw" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[5];
                    break;
                case ItemTrait.TraitType.IceWindWindAmpsFrostbiteDamage:
                    traitText.GetComponentInChildren<Text>().text = "Biting <color=#ABD1E0>Winds" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[5];
                    break;
                case ItemTrait.TraitType.IceWindWindSpellsDamageAmp:
                    traitText.GetComponentInChildren<Text>().text = "Howling <color=#ABD1E0>Frostbite" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[5];
                    break;
                case ItemTrait.TraitType.IceWindIncreaseArmorShredPerFrostbite:
                    traitText.GetComponentInChildren<Text>().text = "Shreddings Winds <color=#ABD1E0>of Frost" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[5];
                    break;
                case ItemTrait.TraitType.IceWindSummonTornadoOnHit:
                    traitText.GetComponentInChildren<Text>().text = "Polar <color=#ABD1E0>Vortex" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[5];
                    break;
                case ItemTrait.TraitType.IcePhysicalFrostbiteAmpsPhysicalCritDamage:
                    traitText.GetComponentInChildren<Text>().text = "Shattering <color=#E94453>Strikes" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[5];
                    break;
                case ItemTrait.TraitType.IcePhysicalPhysicalVampOnFrostbite:
                    traitText.GetComponentInChildren<Text>().text = "Vamperic <color=#E94453>Frost" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[5];
                    break;
                case ItemTrait.TraitType.IcePhysicalBladeVortexOnHit:
                    traitText.GetComponentInChildren<Text>().text = "Icy <color=#AB181D>Blades" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[5];
                    break;
                case ItemTrait.TraitType.IceBleedFrostbiteAmpsBleed:
                    traitText.GetComponentInChildren<Text>().text = "Frosted <color=#AB181D>Veins" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[5];
                    break;
                case ItemTrait.TraitType.IceBleedBleedDoesDamageInstantlyOnThreshold:
                    traitText.GetComponentInChildren<Text>().text = "Winter's Bloody<color=#AB181D> Vengeance" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[5];
                    break;
                case ItemTrait.TraitType.IcePoisonFreezingPoison:
                    traitText.GetComponentInChildren<Text>().text = "Icy<color=#93D916> Toxins" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[5];
                    break;
                case ItemTrait.TraitType.IcePoisonFrostbiteResetsPoisonAndAmps:
                    traitText.GetComponentInChildren<Text>().text = "Toxic <color=#93D916>Frost" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[5];
                    break;
                case ItemTrait.TraitType.IcePoisonSummonPoisonPillarOnThreshold:
                    traitText.GetComponentInChildren<Text>().text = "Frozen Relic <color=#93D916>of Plagues" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[5];
                    break;
                case ItemTrait.TraitType.IceStunRudeAwakening:
                    traitText.GetComponentInChildren<Text>().text = "Shattering <color=#FFF04F>Awakening" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[5];
                    break;
                case ItemTrait.TraitType.IceStunIceRefreshesStun:
                    traitText.GetComponentInChildren<Text>().text = "Stunning <color=#FFF04F>Cold" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[5];
                    break;
                case ItemTrait.TraitType.IceKnockbackFrostbiteIncreasesKnockbackForce:
                    traitText.GetComponentInChildren<Text>().text = "Aval<color=#1F86CA>anche" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[5];
                    break;
                case ItemTrait.TraitType.IceKnockbackSnowEruptionOnKnockback:
                    traitText.GetComponentInChildren<Text>().text = "Staggering <color=#1F86CA>Snow" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[5];
                    break;
                case ItemTrait.TraitType.IceKnockbackBonusStacksOnDownedTargets:
                    traitText.GetComponentInChildren<Text>().text = "Shivering <color=#1F86CA>Shock" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[5];
                    break;
                case ItemTrait.TraitType.EarthMaxHpDamageAtThreshold:
                    traitText.GetComponentInChildren<Text>().text = "Sundering Blows" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[9];
                    break;
                case ItemTrait.TraitType.EarthAmpAllAfflictionsOnThreshhold:
                    traitText.GetComponentInChildren<Text>().text = "Exposing Earth" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[9];
                    break;
                case ItemTrait.TraitType.EarthSunderedEnemiesDealLessDamage:
                    traitText.GetComponentInChildren<Text>().text = "Earthern Decay" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[9];
                    break;
                case ItemTrait.TraitType.EarthRockRingExplosionOnKill:
                    traitText.GetComponentInChildren<Text>().text = "Rumbling Rocks" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[9];
                    break;
                case ItemTrait.TraitType.EarthTrueDamageAtThreshold:
                    traitText.GetComponentInChildren<Text>().text = "Weathering Earth" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[9];
                    break;
                case ItemTrait.TraitType.EarthSunderFurtherReducesResistances:
                    traitText.GetComponentInChildren<Text>().text = "Surging Silt" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[9];
                    break;
                case ItemTrait.TraitType.EarthIncreasedDamageToLowerArmorTargets:
                    traitText.GetComponentInChildren<Text>().text = "Splintered Defences" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[9];
                    break;
                case ItemTrait.TraitType.EarthAmpDamageOnHealthyTargets:
                    traitText.GetComponentInChildren<Text>().text = "Subdueing Sands" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[9];
                    break;
                case ItemTrait.TraitType.EarthHealOnCritAtSunderThreshold:
                    traitText.GetComponentInChildren<Text>().text = "Renewing Earths" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[9];
                    break;
                case ItemTrait.TraitType.EarthPhysicalBonusSunderStacksOnThreshold:
                    traitText.GetComponentInChildren<Text>().text = "Sundering <color=#E94453>Steel" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[9];
                    break;
                case ItemTrait.TraitType.EarthPhysicalSunderAmpsCrits:
                    traitText.GetComponentInChildren<Text>().text = "Devasting Dance <color=#E94453>of Stone" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[9];
                    break;
                case ItemTrait.TraitType.EarthPhysicalSunderAmpsDamage:
                    traitText.GetComponentInChildren<Text>().text = "Brutal Ballad <color=#E94453>of Earth" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[9];
                    break;
                case ItemTrait.TraitType.EarthBleedBonusCritChanceOnBleedingTarget:
                    traitText.GetComponentInChildren<Text>().text = "Exposing <color=#AB181D>Stones" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[9];
                    break;
                case ItemTrait.TraitType.EarthBleedSunderAddsPercentageOfBleed:
                    traitText.GetComponentInChildren<Text>().text = "Shards of <color=#AB181D>Shredding Stone" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[9];
                    break;
                case ItemTrait.TraitType.EarthBleedBonusEarthDamageToBleeding:
                    traitText.GetComponentInChildren<Text>().text = "Muddled <color=#AB181D>Blood" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[9];
                    break;
                case ItemTrait.TraitType.EarthBleedBloodExplosionOnBleed:
                    traitText.GetComponentInChildren<Text>().text = "Well of <color=#AB181D>Blood" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[9];
                    break;
                case ItemTrait.TraitType.EarthPoisonAddSunderedOnPoisonTick:
                    traitText.GetComponentInChildren<Text>().text = "Sundering <color=#93D916>Venom" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[9];
                    break;
                case ItemTrait.TraitType.EarthPoisonSummonPillarOnThreshold:
                    traitText.GetComponentInChildren<Text>().text = "Pillar of <color=#93D916>Plagues" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[9];
                    break;
                case ItemTrait.TraitType.EarthPoisonSunderToPoisonConversion:
                    traitText.GetComponentInChildren<Text>().text = "Plagued <color=#93D916>Earth" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[9];
                    break;
                case ItemTrait.TraitType.EarthPoisonSunderToPoisonOnCrit:
                    traitText.GetComponentInChildren<Text>().text = "Toxic Mud <color=#93D916>of Devastation" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[9];
                    break;
                case ItemTrait.TraitType.EarthStunStunOnThreshold:
                    traitText.GetComponentInChildren<Text>().text = "Stunning <color=#FFF04F>Earth" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[9];
                    break;
                case ItemTrait.TraitType.EarthStunBonusDamageOnStun:
                    traitText.GetComponentInChildren<Text>().text = "Dazing <color=#FFF04F>Dirt" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[9];
                    break;
                case ItemTrait.TraitType.EarthStunKillingStunnedWithEarthRefundsCooldowns:
                    traitText.GetComponentInChildren<Text>().text = "Barraging <color=#FFF04F>Earth" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[9];
                    break;
                case ItemTrait.TraitType.EarthStunStunningAddsSunder:
                    traitText.GetComponentInChildren<Text>().text = "Stone Strikes <color=#FFF04F>of Confoundment" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[9];
                    break;
                case ItemTrait.TraitType.EarthStunSunderAmpsStunDamageLength:
                    traitText.GetComponentInChildren<Text>().text = "Earth <color=#FFF04F>Amplification" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[9];
                    break;
                case ItemTrait.TraitType.EarthKnockbackTremorsOnKnockback:
                    traitText.GetComponentInChildren<Text>().text = "Revirbe<color=#1F86CA>rations" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[9];
                    break;
                case ItemTrait.TraitType.EarthKnockbackSunderReducesKnockbackResistance:
                    traitText.GetComponentInChildren<Text>().text = "Unstable <color=#1F86CA>Earth" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[9];
                    break;
                case ItemTrait.TraitType.EarthKnockbackSummonRocksOnRecentKnockbackTarget:
                    traitText.GetComponentInChildren<Text>().text = "Stone <color=#1F86CA>Reverberation" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[9];
                    break;
                case ItemTrait.TraitType.WindAmpsDamageTaken:
                    traitText.GetComponentInChildren<Text>().text = "Cutting Winds" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[8];
                    break;
                case ItemTrait.TraitType.WindAmpsComboArmorShred:
                    traitText.GetComponentInChildren<Text>().text = "Raging Gusts" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[8];
                    break;
                case ItemTrait.TraitType.WindTargetGainsBleedOnAttack:
                    traitText.GetComponentInChildren<Text>().text = "Singing Slices" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[8];
                    break;
                case ItemTrait.TraitType.WindSummonAerobladesOnThreshold:
                    traitText.GetComponentInChildren<Text>().text = "Song of Dancing Blades" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[8];
                    break;
                case ItemTrait.TraitType.WindWindshearAmpsTrueDamage:
                    traitText.GetComponentInChildren<Text>().text = "Rending Galestorm" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[8];
                    break;
                case ItemTrait.TraitType.WindAddMoreStacksOnInitialStack:
                    traitText.GetComponentInChildren<Text>().text = "Sudden Gails" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[8];
                    break;
                case ItemTrait.TraitType.WindMoreDamageOnMaximumStacks:
                    traitText.GetComponentInChildren<Text>().text = "Exposing Zephyr" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[8];
                    break;
                case ItemTrait.TraitType.WindPhysicalSummonWhirlwindOnSkillHit:
                    traitText.GetComponentInChildren<Text>().text = "Steel <color=#E94453>Whirlwind" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[8];
                    break;
                case ItemTrait.TraitType.WindPhysicalWindshearAmpsBasicAttacks:
                    traitText.GetComponentInChildren<Text>().text = "Wrath of the <color=#E94453>Biting Wind" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[8];
                    break;
                case ItemTrait.TraitType.WindPhysicalCritsDealArmorAsDamage:
                    traitText.GetComponentInChildren<Text>().text = "Devasting <color=#E94453>Rend" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[8];
                    break;
                case ItemTrait.TraitType.WindBleedAmpBleedAtThreshold:
                    traitText.GetComponentInChildren<Text>().text = "Maiming <color=#AB181D>Gusts" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[8];
                    break;
                case ItemTrait.TraitType.WindBleedMoreBleedStacksAThreshold:
                    traitText.GetComponentInChildren<Text>().text = "Rending <color=#AB181D>Currents" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[8];
                    break;
                case ItemTrait.TraitType.WindBleedBleedGrantsWindCritChance:
                    traitText.GetComponentInChildren<Text>().text = "Gouging <color=#AB181D>Gale" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[8];
                    break;
                case ItemTrait.TraitType.WindBleedAddBleedOnWindCrit:
                    traitText.GetComponentInChildren<Text>().text = "Razer <color=#AB181D>Zephyr" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[8];
                    break;
                case ItemTrait.TraitType.WindPoisonTransferPoisonStacksOnKill:
                    traitText.GetComponentInChildren<Text>().text = "Toxic <color=#93D916>Front" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[8];
                    break;
                case ItemTrait.TraitType.WindPoisonWindAddsPercentageOfPoisonOnHit:
                    traitText.GetComponentInChildren<Text>().text = "Plagued <color=#93D916>Winds" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[8];
                    break;
                case ItemTrait.TraitType.WindPoisonPoisonBurstAtWindThreshold:
                    traitText.GetComponentInChildren<Text>().text = "Venomous <color=#93D916>Pulse" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[8];
                    break;
                case ItemTrait.TraitType.WindStunStunDealsTrueDamageAtThreshold:
                    traitText.GetComponentInChildren<Text>().text = "Stunning <color=#FFF04F>Gale" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[8];
                    break;
                case ItemTrait.TraitType.WindStunWindblastOnStun:
                    traitText.GetComponentInChildren<Text>().text = "Dazing <color=#FFF04F>Blast" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[8];
                    break;
                case ItemTrait.TraitType.WindStunStunAmpsWindshearGain:
                    traitText.GetComponentInChildren<Text>().text = "Exposing <color=#FFF04F>Stuns" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[8];
                    break;
                case ItemTrait.TraitType.WindKnockbackKnockbackSummonsMiniCyclone:
                    traitText.GetComponentInChildren<Text>().text = "Howling <color=#1F86CA>Gales" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[8];
                    break;
                case ItemTrait.TraitType.WindKnockbackLoseKnockbackResistanceOnThreshold:
                    traitText.GetComponentInChildren<Text>().text = "Unstable <color=#1F86CA>Footing" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[8];
                    break;
                case ItemTrait.TraitType.WindKnockbackWindshearDoesDamageIfKnockedBack:
                    traitText.GetComponentInChildren<Text>().text = "Unending <color=#1F86CA>Gusts" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[8];
                    break;
                case ItemTrait.TraitType.PhysicalPhysicalAmpsCritChance:
                    traitText.GetComponentInChildren<Text>().text = "Calculated Strikes" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[1];
                    break;
                case ItemTrait.TraitType.PhysicalPhysicalSkillsComboAmp:
                    traitText.GetComponentInChildren<Text>().text = "Press the Advantage" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[1];
                    break;
                case ItemTrait.TraitType.PhysicalLifestealAmp:
                    traitText.GetComponentInChildren<Text>().text = "Hungering Steel" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[1];
                    break;
                case ItemTrait.TraitType.PhysicalSkillAmpArmorOnKill:
                    traitText.GetComponentInChildren<Text>().text = "Bolstering Blades" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[1];
                    break;
                case ItemTrait.TraitType.PhysicalAmpDamageBelowHalfHp:
                    traitText.GetComponentInChildren<Text>().text = "Beserker Rage" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[1];
                    break;
                case ItemTrait.TraitType.PhysicalBleedBleedAmpsPhysicalDamage:
                    traitText.GetComponentInChildren<Text>().text = "Wounding <color=#AB181D>Gusts" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[1];
                    break;
                case ItemTrait.TraitType.PhysicalBleedPhysicalSkillsAddBleed:
                    traitText.GetComponentInChildren<Text>().text = "Evice<color=#AB181D>rate" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[1];
                    break;
                case ItemTrait.TraitType.PhysicalBleedSkillsDoTrueDamageAtThreshold:
                    traitText.GetComponentInChildren<Text>().text = "Rending <color=#AB181D>Assault" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[1];
                    break;
                case ItemTrait.TraitType.PhysicalPoisonPhysicalAmpsPoisonDamage:
                    traitText.GetComponentInChildren<Text>().text = "Poison Coa<color=#93D916>ted Steel" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[1];
                    break;
                case ItemTrait.TraitType.PhysicalPoisonPlayerMaxHpDamageOnThreshold:
                    traitText.GetComponentInChildren<Text>().text = "Virulent <color=#93D916>Toxins" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[1];
                    break;
                case ItemTrait.TraitType.PhysicalPoisonPoisonAmpsPhysicalDamage:
                    traitText.GetComponentInChildren<Text>().text = "Weakening <color=#93D916>Venom" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[1];
                    break;
                case ItemTrait.TraitType.PhysicalStunAmpDamageOnStunned:
                    traitText.GetComponentInChildren<Text>().text = "Oppurtune <color=#FFF04F>Strikes" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[1];
                    break;
                case ItemTrait.TraitType.PhysicalStunBladeRiftOnStun:
                    traitText.GetComponentInChildren<Text>().text = "Whirling <color=#FFF04F>Blades" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[1];
                    break;
                case ItemTrait.TraitType.PhysicalKnockbackKnockbackKillAmpsPhysicalDamage:
                    traitText.GetComponentInChildren<Text>().text = "Rising <color=#1F86CA>Force" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[1];
                    break;
                case ItemTrait.TraitType.PhysicalKnockbackPhysicalAttacksGainInnateKnockback:
                    traitText.GetComponentInChildren<Text>().text = "Crushing <color=#1F86CA>Blows" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[1];
                    break;
                case ItemTrait.TraitType.PhysicalKnockbackSummonKnivesOnKnockbackHit:
                    traitText.GetComponentInChildren<Text>().text = "Dancing <color=#1F86CA>Blades" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[1];
                    break;
                case ItemTrait.TraitType.BleedReducesResistances:
                    traitText.GetComponentInChildren<Text>().text = "Vulnerable Cuts" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[10];
                    break;
                case ItemTrait.TraitType.BleedAmpsCritHitsAddsBleedToNearby:
                    traitText.GetComponentInChildren<Text>().text = "Rushing Blood" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[10];
                    break;
                case ItemTrait.TraitType.BleedSlowsTargets:
                    traitText.GetComponentInChildren<Text>().text = "Crippling Strikes" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[10];
                    break;
                case ItemTrait.TraitType.BleedAmpDamageTakenOnAttack:
                    traitText.GetComponentInChildren<Text>().text = "Deep Wounds" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[10];
                    break;
                case ItemTrait.TraitType.BleedHealOnBleedingEnemyKill:
                    traitText.GetComponentInChildren<Text>().text = "Sanguine Pact" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[10];
                    break;
                case ItemTrait.TraitType.BleedCritsConsumeBleed:
                    traitText.GetComponentInChildren<Text>().text = "Expunging Strike" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[10];
                    break;
                case ItemTrait.TraitType.BleedAmpDamageAtThreshold:
                    traitText.GetComponentInChildren<Text>().text = "Flowing Blood" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[10];
                    break;
                case ItemTrait.TraitType.BleedBloodWellAtThreshold:
                    traitText.GetComponentInChildren<Text>().text = "Tides Of Blood" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[10];
                    break;
                case ItemTrait.TraitType.BleedExpungeBleedAtThreshold:
                    traitText.GetComponentInChildren<Text>().text = "Torrential Blood Letting" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[10];
                    break;
                case ItemTrait.TraitType.BleedPoisonReduceOtherResistances:
                    traitText.GetComponentInChildren<Text>().text = "Immunocompro<color=#93D916>misation" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[10];
                    break;
                case ItemTrait.TraitType.BleedPoisonChanceOfPoisonCloudOnBleed:
                    traitText.GetComponentInChildren<Text>().text = "Vaporizing <color=#93D916>Toxins" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[10];
                    break;
                case ItemTrait.TraitType.BleedStunStunReducesBleedResistance:
                    traitText.GetComponentInChildren<Text>().text = "Exposing <color=#FFF04F>Wounds" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[10];
                    break;
                case ItemTrait.TraitType.BleedStunStunAtThresholdBelowHalfHP:
                    traitText.GetComponentInChildren<Text>().text = "Dazing <color=#FFF04F>Cuts" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[10];
                    break;
                case ItemTrait.TraitType.BleedStunStunAddsBleed:
                    traitText.GetComponentInChildren<Text>().text = "Egregious <color=#FFF04F>Stun" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[10];
                    break;
                case ItemTrait.TraitType.BleedKnockbackBonusBleed:
                    traitText.GetComponentInChildren<Text>().text = "Lacerating <color=#1F86CA>Knockbacks" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[10];
                    break;
                case ItemTrait.TraitType.BleedKnockbackKnockbackExposionOfBlood:
                    traitText.GetComponentInChildren<Text>().text = "Pulse of <color=#1F86CA>Blood" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[10];
                    break;
                case ItemTrait.TraitType.BleedKnockbackKnockbackAmpsDamage:
                    traitText.GetComponentInChildren<Text>().text = "Sanguine <color=#1F86CA>Force" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[10];
                    break;
                case ItemTrait.TraitType.PoisonSpreadStacksOnDeath:
                    traitText.GetComponentInChildren<Text>().text = "Decaying Flesh" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[11];
                    break;
                case ItemTrait.TraitType.PoisonAmpsLifesteal:
                    traitText.GetComponentInChildren<Text>().text = "Rejuvinating Venom" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[11];
                    break;
                case ItemTrait.TraitType.PoisonAmpNextAttackAfterPoisonKill:
                    traitText.GetComponentInChildren<Text>().text = "Contagion" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[11];
                    break;
                case ItemTrait.TraitType.PoisonAmpDamageOnFirstStack:
                    traitText.GetComponentInChildren<Text>().text = "Patient Zero" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[11];
                    break;
                case ItemTrait.TraitType.PoisonPrimaryTraitsAmpPoison:
                    traitText.GetComponentInChildren<Text>().text = "Multi-front Contagion" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[11];
                    break;
                case ItemTrait.TraitType.PoisonShredArmorOnThreshold:
                    traitText.GetComponentInChildren<Text>().text = "Corrosive Toxins" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[11];
                    break;
                case ItemTrait.TraitType.PoisonEnemiesAmpPoisonOnKill:
                    traitText.GetComponentInChildren<Text>().text = "Plagued Corpses" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[11];
                    break;
                case ItemTrait.TraitType.PoisonTrueDamageAtThreshold:
                    traitText.GetComponentInChildren<Text>().text = "Lethal Venom" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[11];
                    break;
                case ItemTrait.TraitType.PoisonVamperism:
                    traitText.GetComponentInChildren<Text>().text = "Toxic Predator" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[11];
                    break;
                case ItemTrait.TraitType.PoisonStunPoisonReducesStunResist:
                    traitText.GetComponentInChildren<Text>().text = "Dazing <color=#FFF04F>Pestilence" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[11];
                    break;
                case ItemTrait.TraitType.PoisonStunPoisonSpreadOnThreshold:
                    traitText.GetComponentInChildren<Text>().text = "Virulent <color=#FFF04F>Pestilence" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[11];
                    break;
                case ItemTrait.TraitType.PoisonKnockbackPoisonReducesKnockbackResistance:
                    traitText.GetComponentInChildren<Text>().text = "Knockback of <color=#1F86CA>Plagues" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[11];
                    break;
                case ItemTrait.TraitType.PoisonKnockbackConsumePoisonStacksForTrueDamage:
                    traitText.GetComponentInChildren<Text>().text = "Venomous <color=#1F86CA>Blow" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[11];
                    break;
                case ItemTrait.TraitType.StunReducesArmor:
                    traitText.GetComponentInChildren<Text>().text = "Armor Rending Stuns" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[13];
                    break;
                case ItemTrait.TraitType.StunAmpsDamageTaken:
                    traitText.GetComponentInChildren<Text>().text = "Expose Vulnerbilities" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[13];
                    break;
                case ItemTrait.TraitType.StunAmpsAfflictionGain:
                    traitText.GetComponentInChildren<Text>().text = "Calamity Rising" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[13];
                    break;
                case ItemTrait.TraitType.StunAmpDuration:
                    traitText.GetComponentInChildren<Text>().text = "Enchanced Stupor" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[13];
                    break;
                case ItemTrait.TraitType.StunShockwaveOnStunningStunned:
                    traitText.GetComponentInChildren<Text>().text = "Dazing Wave" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[13];
                    break;
                case ItemTrait.TraitType.StunOnStunDealsAdditionalBaseDamage:
                    traitText.GetComponentInChildren<Text>().text = "True Strikes" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[13];
                    break;
                case ItemTrait.TraitType.StunAmpDurationOnThreshold:
                    traitText.GetComponentInChildren<Text>().text = "Misery's Company" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[13];
                    break;
                case ItemTrait.TraitType.StunAmpCritDamage:
                    traitText.GetComponentInChildren<Text>().text = "Mighty Slashes" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[13];
                    break;
                case ItemTrait.TraitType.StunAmpsMovespeed:
                    traitText.GetComponentInChildren<Text>().text = "Tactical Repositioning" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[13];
                    break;
                case ItemTrait.TraitType.StunKnockbackStunReduceResistance:
                    traitText.GetComponentInChildren<Text>().text = "Unstable <color=#1F86CA>Footing" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[13];
                    break;
                case ItemTrait.TraitType.KnockbackReducesSpellCooldowns:
                    traitText.GetComponentInChildren<Text>().text = "Press the Assault" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[14];
                    break;
                case ItemTrait.TraitType.KnockBackAmpsBasicAttacks:
                    traitText.GetComponentInChildren<Text>().text = "Nimble Duelist" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[14];
                    break;
                case ItemTrait.TraitType.KnockbackAmpKnockbackForce:
                    traitText.GetComponentInChildren<Text>().text = "Mountains Strength" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[14];
                    break;
                case ItemTrait.TraitType.KnockbackAmpsDamageTaken:
                    traitText.GetComponentInChildren<Text>().text = "Shredded Armor" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[14];
                    break;
                case ItemTrait.TraitType.KnockbackAmpsArmor:
                    traitText.GetComponentInChildren<Text>().text = "Barricade" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[14];
                    break;
                case ItemTrait.TraitType.KnockbackDoesBonusDamageOnKnockbacked:
                    traitText.GetComponentInChildren<Text>().text = "Chained Blows" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[14];
                    break;
                case ItemTrait.TraitType.KnockbackReducesResistances:
                    traitText.GetComponentInChildren<Text>().text = "Violent Strikes" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitText.GetComponentInChildren<Text>().color = traitTextColors[14];
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

    // Returns of a plus or nothing depending on wether the value is positive or not.
    private string GetTraitPrefix(float value)
    {
        string prefix = "";

        if (value > 0)
            prefix = "+";

        return prefix;
    }
}
