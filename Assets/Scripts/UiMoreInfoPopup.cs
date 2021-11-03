using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiMoreInfoPopup : MonoBehaviour
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

    public Transform traitContainerScrollParent;
    public Transform traitContainerNonScrollParent;
    public GameObject traitScrollContainer;
    public Transform traitScrollContent;

    public RectTransform descriptionBox;
    public Color[] itemOutlineColors;
    public Sprite[] itemTypeSprites;
    public Color[] traitTextColors;
    public float PopUpTraitHeight = 0f;
    private int traitContainerSiblingIndex;

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
        traitContainerSiblingIndex = traitContainer.GetSiblingIndex();
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
            if (stats.bonusStacksOnHit > 0)
                itemBaseStatsText[2].text = string.Format("{0}+{1}", targetItem.stacksToAddOnHit, stats.bonusStacksOnHit);
            else
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
        else if (targetItem.itemType == Item.ItemType.Skill)
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
        else if (targetItem.itemType == Item.ItemType.Helmet || targetItem.itemType == Item.ItemType.Armor || targetItem.itemType == Item.ItemType.Legs || targetItem.itemType == Item.ItemType.Shield)
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

            itemBaseStatsText[6].text = string.Format("{0:0}", (int)armorValue);
            itemBaseStatsText[7].text = string.Format("{0:0}", (int)flatDamageReductionValue);
            itemBaseStatsText[8].text = string.Format("{0:0}", (int)healthGainValue);
            itemBaseStatsText[9].text = string.Format("{0:0}%", (int)(movespeedValue * 100));

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

        if (targetItem.itemType != Item.ItemType.Skill)
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

        // Should we make the traits scrollable or not.
        if(PopUpTraitHeight > 450 - 238)
        {
            traitScrollContent.GetComponent<RectTransform>().sizeDelta = new Vector2(0, PopUpTraitHeight);
            traitScrollContent.transform.localPosition = Vector3.zero;

            PopUpTraitHeight = 450 - 238;
            // move the traits into the scrollable content and set it's local xy

            traitContainer.transform.SetParent(traitContainerScrollParent);
            traitContainer.anchoredPosition = new Vector3(8.5f, 0f, 0f);

            traitScrollContainer.SetActive(true);
        }
        else
        {
            // move the triats out of the scrollable and disable the scrolling window.
            traitContainer.transform.SetParent(traitContainerNonScrollParent);

            traitContainer.anchoredPosition = new Vector3(0f, -186f, 0f);
            traitContainer.SetSiblingIndex(traitContainerSiblingIndex);

            traitScrollContainer.SetActive(false);
        }

        traitContainer.sizeDelta = new Vector2(traitContainer.sizeDelta.x, PopUpTraitHeight);
        if (!primaryPopUp)
            primaryPopUp = GetComponent<RectTransform>();
        primaryPopUp.sizeDelta = new Vector2(primaryPopUp.sizeDelta.x, 238 + traitContainer.sizeDelta.y);
    }

    // Used to clear every trait from the previous item from the popup
    private void ClearPopUp()
    {
        PopUpTraitHeight = 40;
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
        for (int index = 0; index < item.itemTraits.Count; index++)
        {
            GameObject traitText = Instantiate(popUpTextPrefab, traitContainer.transform);
            traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 13f);
            float traitWidth = 100;

            switch (item.itemTraits[index].traitType)
            {
                case ItemTrait.TraitType.Armor:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = string.Format(GetTraitPrefix(item.itemTraits[index].traitBonus) + "{0:0} Armor - ", item.itemTraits[index].traitBonus * item.itemTraits[index].traitBonusMultiplier);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "Armor increases your characters resistance to damage. Armor reduces damage by a percentage.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24f);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[0];
                    if (item.itemType == Item.ItemType.Helmet || item.itemType == Item.ItemType.Armor || item.itemType == Item.ItemType.Legs || item.itemType == Item.ItemType.Shield)
                    {
                        traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350f, 0f);
                        Destroy(traitText);
                    }
                    break;
                case ItemTrait.TraitType.HealthRegen:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = string.Format(GetTraitPrefix(item.itemTraits[index].traitBonus) + "{0:0.0} Health Regen per Second - ", item.itemTraits[index].traitBonus * item.itemTraits[index].traitBonusMultiplier);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "Health regeneration is the rate at which your character regains hitpoints.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24f);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[3];
                    break;
                case ItemTrait.TraitType.CooldownReduction:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = string.Format(GetTraitPrefix(item.itemTraits[index].traitBonus) + "{0:0}% Cooldown Reduction - ", item.itemTraits[index].traitBonus * item.itemTraits[index].traitBonusMultiplier * 100);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "Reduces the cooldown time of skills by a percentage.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 20);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[16];
                    break;
                case ItemTrait.TraitType.AflameResistance:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = string.Format(GetTraitPrefix(item.itemTraits[index].traitBonus) + "{0:0}% Fire Resist - ", item.itemTraits[index].traitBonus * item.itemTraits[index].traitBonusMultiplier * 100);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "Reduces the chance of recieving the <color=#FF932E>Aflame</color> debuff by a percentage.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 20);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.FrostbiteResistance:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = string.Format(GetTraitPrefix(item.itemTraits[index].traitBonus) + "{0:0}% Frostbite Resist - ", item.itemTraits[index].traitBonus * item.itemTraits[index].traitBonusMultiplier * 100);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "Reduces the chance of recieving the <color=#5AD9F5>Frostbite</color> debuff by a percentage.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24f);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[5];
                    break;
                case ItemTrait.TraitType.OverchargeResistance:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = string.Format(GetTraitPrefix(item.itemTraits[index].traitBonus) + "{0:0}% Lightning Resist - ", item.itemTraits[index].traitBonus * item.itemTraits[index].traitBonusMultiplier * 100);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "Reduces the chance of recieving the <color=#CA65FF>Overcharge</color> debuff by a percentage.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24f);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[6];
                    break;
                case ItemTrait.TraitType.OvergrowthResistance:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = string.Format(GetTraitPrefix(item.itemTraits[index].traitBonus) + "{0:0}% Nature Resist - ", item.itemTraits[index].traitBonus * item.itemTraits[index].traitBonusMultiplier * 100);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "Reduces the chance of recieving the <color=#4ED477>Overgrowth</color> debuff by a percentage.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24f);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[7];
                    break;
                case ItemTrait.TraitType.WindshearResistance:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = string.Format(GetTraitPrefix(item.itemTraits[index].traitBonus) + "{0:0}% Wind Resist - ", item.itemTraits[index].traitBonus * item.itemTraits[index].traitBonusMultiplier * 100);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "Reduces the chance of recieving the <color=#ABD1E0>Windshear</color> debuff by a percentage.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24f);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[8];
                    break;
                case ItemTrait.TraitType.SunderResistance:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = string.Format(GetTraitPrefix(item.itemTraits[index].traitBonus) + "{0:0}% Earth Resist - ", item.itemTraits[index].traitBonus * item.itemTraits[index].traitBonusMultiplier * 100);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "Reduces the chance of recieving the <color=#B0946C>Sunder</color> debuff by a percentage.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24f);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[9];
                    break;
                case ItemTrait.TraitType.BleedResistance:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = string.Format(GetTraitPrefix(item.itemTraits[index].traitBonus) + "{0:0}% Bleed Resist - ", item.itemTraits[index].traitBonus * item.itemTraits[index].traitBonusMultiplier * 100);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "Reduces the chance of recieving the <color=#AB181D>Bleed</color> debuff by a percentage.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 20);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 13f);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[10];
                    break;
                case ItemTrait.TraitType.PoisonResistance:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = string.Format(GetTraitPrefix(item.itemTraits[index].traitBonus) + "{0:0}% Poison Resist - ", item.itemTraits[index].traitBonus * item.itemTraits[index].traitBonusMultiplier * 100);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "Reduces the chance of recieving the <color=#93D916>Poison</color> debuff by a percentage.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24f);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[11];
                    break;
                case ItemTrait.TraitType.AsleepResistance:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = string.Format(GetTraitPrefix(item.itemTraits[index].traitBonus) + "{0:0}% Sleep Resist - ", item.itemTraits[index].traitBonus * item.itemTraits[index].traitBonusMultiplier * 100);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "Reduces the chance of recieving the <color=#6D4880>Asleep</color> debuff by a percentage.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24f);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[12];
                    break;
                case ItemTrait.TraitType.StunResistance:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = string.Format(GetTraitPrefix(item.itemTraits[index].traitBonus) + "{0:0}% Stun Resist - ", item.itemTraits[index].traitBonus * item.itemTraits[index].traitBonusMultiplier * 100);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "Reduces the chance of recieving the <color=#FFF04F>Stun</color> debuff by a percentage.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 20);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[13];
                    break;
                case ItemTrait.TraitType.KnockbackResistance:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = string.Format(GetTraitPrefix(item.itemTraits[index].traitBonus) + "{0:0}% Knockback Resist - ", item.itemTraits[index].traitBonus * item.itemTraits[index].traitBonusMultiplier * 100);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "Reduces the chance of recieving the <color=#1F86CA>Knockback</color> debuff by a percentage.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24f);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[14];
                    break;
                case ItemTrait.TraitType.AttackSpeed:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = string.Format(GetTraitPrefix(item.itemTraits[index].traitBonus) + "{0:0}% Attack Speed - ", item.itemTraits[index].traitBonus * item.itemTraits[index].traitBonusMultiplier * 100);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "Increases the speed you perform basic attacks and cast skills.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 20);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[15];
                    break;
                case ItemTrait.TraitType.HealthFlat:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = string.Format(GetTraitPrefix(item.itemTraits[index].traitBonus) + "{0:0} Health - ", item.itemTraits[index].traitBonus * item.itemTraits[index].traitBonusMultiplier);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "Increases your characters health by a flat amount.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 20);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[3];
                    if (item.itemType == Item.ItemType.Helmet || item.itemType == Item.ItemType.Armor || item.itemType == Item.ItemType.Legs || item.itemType == Item.ItemType.Shield)
                    {
                        traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350f, 0f);
                        Destroy(traitText);
                    }
                    break;
                case ItemTrait.TraitType.HealthPercent:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = string.Format(GetTraitPrefix(item.itemTraits[index].traitBonus) + "{0:0}% Increased Health - ", item.itemTraits[index].traitBonus * 100 * item.itemTraits[index].traitBonusMultiplier);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "Increases your characters health by a percentage of its current value.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 20);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[3];
                    break;
                case ItemTrait.TraitType.HealingOnHit:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = string.Format(GetTraitPrefix(item.itemTraits[index].traitBonus) + "{0:0} Health on Hit - ", item.itemTraits[index].traitBonus * item.itemTraits[index].traitBonusMultiplier);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "Amount of hitpoints restored upon striking a target with a non damage over time source of damage.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24f);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[2];
                    break;
                case ItemTrait.TraitType.HealingOnKill:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = string.Format(GetTraitPrefix(item.itemTraits[index].traitBonus) + "{0:0} Health on Kill - ", item.itemTraits[index].traitBonus * item.itemTraits[index].traitBonusMultiplier);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "Amount of hitpoints restored upon killing an enemy";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 20);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[2];
                    break;
                case ItemTrait.TraitType.MoveSpeed:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = string.Format(GetTraitPrefix(item.itemTraits[index].traitBonus) + "{0:0}% Movespeed - ", item.itemTraits[index].traitBonus * 100 * item.itemTraits[index].traitBonusMultiplier);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "The percent modifier of your player's move speed and roll speed.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 20);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[16];
                    if (item.itemType == Item.ItemType.Helmet || item.itemType == Item.ItemType.Armor || item.itemType == Item.ItemType.Legs || item.itemType == Item.ItemType.Shield)
                    {
                        traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350f, 0f);
                        Destroy(traitText);
                    }
                    break;
                case ItemTrait.TraitType.Jumps:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = string.Format(GetTraitPrefix(item.itemTraits[index].traitBonus) + "{0:0} Extra Jumps - ", item.itemTraits[index].traitBonus * item.itemTraits[index].traitBonusMultiplier);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "The number of bonus jumps the player can perform.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 20);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[16];
                    break;
                case ItemTrait.TraitType.CritChance:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = string.Format(GetTraitPrefix(item.itemTraits[index].traitBonus) + "{0:0}% Critical Strike Chance - ", item.itemTraits[index].traitBonus * item.itemTraits[index].traitBonusMultiplier * 100);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "The percent chance the player has to critically strike on their attacks, dealing bonus damage.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24f);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[1];
                    break;
                case ItemTrait.TraitType.CritDamage:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = string.Format(GetTraitPrefix(item.itemTraits[index].traitBonus) + "{0:0}% Critical Strike Damage - ", item.itemTraits[index].traitBonus * item.itemTraits[index].traitBonusMultiplier * 100);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "The percent damage increase when the player critically strikes.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24f);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[1];
                    break;
                case ItemTrait.TraitType.FlatDamageReduction:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = string.Format(GetTraitPrefix(item.itemTraits[index].traitBonus) + "{0:0} Flat Damage Reduction - ", item.itemTraits[index].traitBonus * item.itemTraits[index].traitBonusMultiplier);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "The amount of flat reduced damage your character mitigates from attacks.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24f);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[17];
                    if (item.itemType == Item.ItemType.Helmet || item.itemType == Item.ItemType.Armor || item.itemType == Item.ItemType.Legs || item.itemType == Item.ItemType.Shield)
                    {
                        traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350f, 0f);
                        Destroy(traitText);
                    }
                    break;
                case ItemTrait.TraitType.SpellDamage:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = string.Format(GetTraitPrefix(item.itemTraits[index].traitBonus) + "{0:0}% Increased Skill Damage - ", item.itemTraits[index].traitBonus * item.itemTraits[index].traitBonusMultiplier * 100);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "Increases the damage done by your skills by a percentage.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24f);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[16];
                    break;
                case ItemTrait.TraitType.BasicAttackAmp:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = string.Format(GetTraitPrefix(item.itemTraits[index].traitBonus) + "{0:0}% Increased Basic Attack Damage - ", item.itemTraits[index].traitBonus * item.itemTraits[index].traitBonusMultiplier * 100);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "Increases the damage done by your basic attacks by a percentage.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24f);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[1];
                    break;
                case ItemTrait.TraitType.Luck:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = string.Format(GetTraitPrefix(item.itemTraits[index].traitBonus) + "{0:0} Luck - ", item.itemTraits[index].traitBonus * item.itemTraits[index].traitBonusMultiplier);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "Increases your chance at finding higher rarity gear and skills from chests.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 20);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[18];
                    break;
                case ItemTrait.TraitType.BonusStacksOnHit:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = string.Format(GetTraitPrefix(item.itemTraits[index].traitBonus) + "{0:0} Bonus Stacks On Hit - ", item.itemTraits[index].traitBonus * item.itemTraits[index].traitBonusMultiplier);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "The amount of extra affliction stacks added on a basic attack. Has no effect if the weapon does not inflict an affliction.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24f);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[19];
                    break;
                case ItemTrait.TraitType.FireExplosionOnKill:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Consuming Blaze" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "Killing an enemy causes an explosion that deals <color=#FFFFFF>100% + 1% base damage</color> per <color=#FF932E>stack of Aflame.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24f);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.MoreAflameStacksOnHitThreshold:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Raging Inferno" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "Summon a pulse of flame when the target reaches <color=#FF932E>15+ Aflame stacks</color>, adding <color=#FF932E>1 Aflame stack</color> to nearby enemies.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24f);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.BurnDoesMaxHpDamageAtThreshold:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Fire Overwhelming" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "Targets with <color=#FF932E>25+ Aflame stacks</color> take additional burn damage equal to <color=#AD2A2A>1% of the targets max health</color> as bonus damage per second, capped at <color=#FFFFFF>1000% base damage</color> per second.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 60);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 35f);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.BasicAttacksShredArmorOnAflame:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Shredding Flames" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "Basic attacks on targets with <color=#FF932E>15+ Aflame stacks</color> remove <color=#FAFF00>5% of the target's armor</color> for 10 seconds.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24f);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.FlameVamperism:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Flame Vamperism" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#60D46D>Heal 1 hitpoint</color> per <color=#FF932E>10 Aflame stacks</color> every second per afflicted target.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24f);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.RingOfFireOnHit:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Matrimony of Flame" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#FF932E>Fire damage</color> gains a 10% chance to summon a ring of fire, dealing <color=#FFFFFF>50% base damage</color> a second for 5 seconds.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24f);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.AflameToSunderStackOnEarthSpell:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Wrath of the <color=#B0946C>Blazing Earth" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#B0946C>Earth skills</color> add <color=#FF932E>10% of current Aflame stacks</color> as <color=#B0946C>bonus Sunder stacks</color> on hit.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24f);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.SunderFurtherDecreasesFireResist:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Deep Rooted <color=#B0946C>Flames" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#B0946C>Sunder</color> further reduces the target's <color=#FF932E>Aflame resistance by 1%</color> per <color=#B0946C>Sunder stack.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24f);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.AflameSunderCritsSummonFireballs:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Devasting Flame <color=#B0946C>Geyser" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#E94453>Critical strikes</color> on targets with <color=#FF932E>30+ combined Aflame</color> <color=#B0946C>and Sunder stacks</color> summons a fireball that deals <color=#FFFFFF>120% base damage.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 60);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 35f);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.AflameWindshearWindAttacksGainCritOnBurningTarget:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Blazing <color=#ABD1E0>Exposure" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#ABD1E0>Wind attacks</color> gain a bonus <color=#E94453>0.5% crit chance</color> per <color=#FF932E>stack of Aflame</color> on the target.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24f);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.AflameWindshearSummonFirePillarsOnHit:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Flaring <color=#ABD1E0>Winds" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#ABD1E0>Wind skills</color> have a 20% chance to summon a fire geyser on hit against targets with <color=#FF932E>30+ Aflame stacks</color>, dealing <color=#FFFFFF>100% base damage</color> a second for 5 seconds.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 60);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 35f);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.AflameWindshearWindSpellsAddFireStacks:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Roaring <color=#ABD1E0>Blaze" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#ABD1E0>Wind attacks</color> have a 10% chance to summon a fire pulse, adding <color=#FF932E>20% of the targets current Aflame stacks</color> to all nearby enemies.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24f);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.AflamePhysicalAddFireStacksOnHit:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Searing <color=#E94653>Metal" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#E94453>Physical damage</color> adds <color=#FF932E>1 Aflame stack</color> on hit.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 20);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 13f);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.AflamePhysicalDamageAmpOnBurningTarget:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Smoldering <color=#E94653>Steel" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#E94453>Physical damage</color> deals <color=#FF932E>5% more damage per stack of Aflame</color> on the target.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24f);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.AflamePhysicalBladeExplosionOnKill:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Red-hot <color=#E94653>Metallic Implosion" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "Killing a target with <color=#FF932E>25+ Aflame stacks</color> causes a blade explosion, dealing <color=#FFFFFF>100% base damage</color> to nearby enemies.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 60);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 35f);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.AflamePhysicalBigHitsAddAflame:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Searing <color=#E94653>Strikes" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#E94453>Physical damage</color> that deals <color=#FFFFFF>over 400% base damage</color> adds <color=#FF932E>50% of the current Aflame stacks as bonus Aflame stacks.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24f);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.AflameBleedIncreasesFlameCritChance:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Searing <color=#AB181D>Wounds" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#FF932E>Fire damage</color> gains a bonus <color=#E94453>0.5% critical strike chance</color> per <color=#AB181D>stack of Bleed</color> on the target.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24f);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.AflameBleedFireDamageAmpOnBleedThreshold:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Boiling <color=#AB181D>Blood" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#FF932E>Fire attacks</color> consumes the target's <color=#AB181D>Bleed stacks</color> to deal <color=#AB181D>10% more damage per Bleed stack</color> consumed.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24f);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.AflameBleedAflameAddsBleedAtThreshhold:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Blaze of <color=#AB181D>Exsanguination" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "Adding <color=#FF932E>Aflame stacks</color> on a target with <color=#FF932E>30+ Aflame stacks</color> adds a <color=#AB181D>bonus Bleed stack.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24f);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.AflameBleedAflameRemovesBleedResist:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Flames of <color=#AB181D>Hemorrhage" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#FF932E>Aflame stacks</color> remove <color=#AB181D>1% of the targets Bleed resistance</color> per stack.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24f);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.AflameBleedDamageAmpOnDoubleThreshhold:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Consuming <color=#AB181D>Blaze" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "Targets with <color=#FF932E>30+ Aflame stacks</color> and <color=#AB181D>30+ Bleed stacks</color> take an additional 50% damage from both afflictions.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24f);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.AflamePoisonBurningEnemySpreadPoisonStacksOnDeath:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Fiery <color=#93D916>Contagion" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "Killing a target with <color=#FF932E>20+ Aflame stacks</color> summons a poison pulse, spreading <color=#93D916>50% of their current Poison stacks</color> to nearby enemies.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24f);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.AflamePoisonGreviousWoundsOnStackThreshold:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Virulent <color=#93D916>Toxins" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "Targets with <color=#FF932E>40+ combined Aflame</color> and <color=#93D916>Poisoned stacks</color> are greviously wounded, <color=#60D46D>reducing healing by 75%</color> for 10 seconds.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24f);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.AflamePoisonPoisonReducesFireResist:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Incendiary <color=#93D916>Plague" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#93D916>Poison</color> reduces the target's <color=#FF932E>Aflame resistance by 1%</color> per <color=#93D916>stack of Poison.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24f);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.AflamePoisonFireSpellsSummonsPoisonBurst:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Infernal <color=#93D916>Epidemic" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "Your first <color=#FF932E>fire attack</color> on a target with <color=#93D916>30+ Poison stacks</color> summons a poison cloud, adding <color=#93D916>5 Poison stacks</color> to nearby enemies.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 60);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 35);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.AflamePoisonFireAmpsPoison:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Scalding <color=#93D916>Poison" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#FF932E>Aflame stacks</color> increase the damage of <color=#93D916>Poison by 1%</color> per <color=#FF932E>stack of Aflame.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24f);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.AflamePoisonPoisonCloudOnFireKill:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Toxic <color=#93D916>Smoke" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "Killing an enemy with <color=#93D916>Poison stacks</color> with a <color=#FF932E>fire attack</color> causes a posion cloud to erupt, dealing <color=#FFFFFF>10% base damage a second</color> for 10 seconds.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24f);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.AflameStunPeriodBurnStun:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Dazing <color=#FFF04F>Flames" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "Targets that gain <color=#FF932E>Aflame stacks</color> are <color=#FFF04F>Stunned</color> once every 20 seconds.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 20);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 13f);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.AflameStunStunOnThreshold:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Stunning <color=#FFF04F>Inferno" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#FFF04F>Stun</color> a target that reaches <color=#FF932E>15+ stacks of Aflame.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 20);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 13f);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.AflameStunStunReducesFireResistance:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Confounding <color=#FFF04F>Conflagration" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "A <color=#FFF04F>Stunned</color> enemy has it's <color=#FF932E>Fire resistance reduced by 20%</color> for the duration.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.AflameStunStunAmpsBurnDamage:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Dazing Flare <color=#FFF04F>Amplification" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "A <color=#FFF04F>Stunned</color> target recieves <color=#FF932E>200% more damage from the Aflame damage over time.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.AflameKnockbackAflameReducesKnockbackResist:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Staggering <color=#1F86CA>Flames" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#FF932E>Aflame</color> reduces the target's <color=#1F86CA>Knockback resistance by 1%</color> per <color=#FF932E>stack of Aflame.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.AflameKnockbackAflameSpellsOnKnockbackedTargetExplode:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Infernal <color=#1F86CA>Waves" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#1F86CA>Knocking back</color> a target with <color=#FF932E>20+ Aflame stacks</color> causes an explosion, dealing <color=#FFFFFF>125% base damage</color> to nearby targets.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.AflameKnockbackKnockbackAmpsFireDamage:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Searing <color=#1F86CA>Impulse" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#1F86CA>Knocking back</color> a target causes them to recieve 25% more damage from the <color=#FF932E>Aflame damage over time</color> for the <color=#1F86CA>duration of the Knockback.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 60);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 35);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[4];
                    break;
                case ItemTrait.TraitType.IceFreezeAtStackThreshold:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Deep Freeze" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#5AD9F5>Freeze</color> a target once they reach <color=#5AD9F5>20+ Frostbite stacks.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 20);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 13f);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[5];
                    break;
                case ItemTrait.TraitType.IceAmpAllDamageAtThreshold:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Biting Cold" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "A target with <color=#5AD9F5>40+ Frostbite stacks</color> recieves 25% increased damage from all sources.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[5];
                    break;
                case ItemTrait.TraitType.IceBasicAttacksConsumeStacksAtThreshold:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Shattered Ice" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "Basic attacks on a target with <color=#5AD9F5>30+ Frostbite stacks</color> consume the stacks to summon an icicle explosion, dealing <color=#FFFFFF>100% base damage</color>, plus an additional <color=#FFFFFF>10% base damage</color> per <color=#5AD9F5>Frostbite stack consumed.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 60);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 35);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[5];
                    break;
                case ItemTrait.TraitType.IceEnemyAttacksWeakendAtThreshold:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Brittle Steel" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "Enemies with <color=#5AD9F5>30+ Frostbite stacks</color> deal 50% reduced damage.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 20);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 13f);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[5];
                    break;
                case ItemTrait.TraitType.IceEnemiesGainFrostbiteOnStrikingYou:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Frigid Armor" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "Enemies that strike you recieve <color=#5AD9F5>3 Frostbite stacks.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 20);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 13f);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[5];
                    break;
                case ItemTrait.TraitType.IceAddStacksToNearbyEnemies:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Arctic Aura" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "Add <color=#5AD9F5>1 Frostbite stack</color> to nearby enemies every 2 seconds.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 20);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 13f);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[5];
                    break;
                case ItemTrait.TraitType.IceAmpFrostbiteDamage:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Bone Chilling Frost" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "Increases <color=#5AD9F5>Frostbite's damage over time</color> by 25%";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 20);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 13f);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[5];
                    break;
                case ItemTrait.TraitType.IceEarthFrostToEarthBonusDamage:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Frosted <color=#B0946C>Earth" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#B0946C>Earth attacks</color> consume <color=#5AD9F5>Frostbite on their target</color>, dealing 5% more damage per stack consumed and adding <color=#5AD9F5>half of the initial Frostbite stacks</color> as <color=#B0946C>Sunder stacks.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 60);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 35);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[5];
                    break;
                case ItemTrait.TraitType.IceEarthSunderAmpsIceDamage:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Brittle <color=#B0946C>Flesh" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#5AD9F5>Ice attacks</color> <color=#B0946C>consume Sunder stacks</color> on the target, deaing a bonus 10% more damage per stack consumed.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[5];
                    break;
                case ItemTrait.TraitType.IceEarthIceDOTAtThreshold:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Icy <color=#B0946C>Reverberations" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#5AD9F5>Ice attacks</color> deal an additional 80% damage as a damage over time effect over 5 seconds on targets with <color=#B0946C>50+ Sunder stacks.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[5];
                    break;
                case ItemTrait.TraitType.IceEarthEarthSpellBonusCritDamage:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Devasting <color=#B0946C>Thaw" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#B0946C>Earth attacks</color> <color=#E94453>gain an additional 2.5% critical strike damage</color> per <color=#5AD9F5>stack of Frostbite</color> on the target.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[5];
                    break;
                case ItemTrait.TraitType.IceWindWindAmpsFrostbiteDamage:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Biting <color=#ABD1E0>Winds" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#5AD9F5>Frostbite's damage over time</color> deals an additional 10% bonus damage per <color=#ABD1E0>stack of Windshear.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[5];
                    break;
                case ItemTrait.TraitType.IceWindWindSpellsDamageAmp:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Howling <color=#ABD1E0>Frostbite" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#ABD1E0>Wind attacks</color> deal an 3% increased damage per <color=#5AD9F5>stack of Frostbite.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[5];
                    break;
                case ItemTrait.TraitType.IceWindIncreaseArmorShredPerFrostbite:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Shreddings Winds <color=#ABD1E0>of Frost" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#ABD1E0>Windshear</color> shreds an additional <color=#FAFF00>2.5% of the targets armor</color> per <color=#5AD9F5>stack of Frostbite.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[5];
                    break;
                case ItemTrait.TraitType.IceWindSummonTornadoOnHit:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Polar <color=#ABD1E0>Vortex" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#ABD1E0>Wind attacks</color> on a target with <color=#5AD9F5>25+ Frostbite stacks</color> have a 10% chance to summon a wind tornado, dealing <color=#FFFFFF>125% base damage</color> over 2 seconds.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 60);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 35);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[5];
                    break;
                case ItemTrait.TraitType.IcePhysicalFrostbiteAmpsPhysicalCritDamage:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Shattering <color=#E94653>Strikes" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#E94653>Physical attacks deal 2% more critical strike damage</color> per <color=#5AD9F5>stack of Frostbite.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[5];
                    break;
                case ItemTrait.TraitType.IcePhysicalPhysicalVampOnFrostbite:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Vamperic <color=#E94653>Frost" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#E94653>Physical attacks</color><color=#60D46D> heal for 1% of their damage dealt</color> if the target is <color=#5AD9F5>Frostbitten.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[5];
                    break;
                case ItemTrait.TraitType.IcePhysicalBladeVortexOnHit:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Icy <color=#AB181D>Blades" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#E94453>Physical attacks</color> on target's with <color=#5AD9F5>30+ Frostbite stacks</color> consume the stacks to summon a blade vortex, dealing <color=#FFFFFF>120% base damage over 3 seconds.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 60);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 35);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[5];
                    break;
                case ItemTrait.TraitType.IceBleedFrostbiteAmpsBleed:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Frosted <color=#AB181D>Veins" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#AB181D>Bleeding</color> <color=#FFF00>shreds an additional 2.5% of the targets armor</color> per <color=#5AD9F5>stack of Frostbite.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[5];
                    break;
                case ItemTrait.TraitType.IceBleedBleedDoesDamageInstantlyOnThreshold:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Winter's Bloody<color=#AB181D> Vengeance" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#5AD9F5>Frosbite stacks</color> increase <color=#AB181D>Bleed's damage over time by 2%</color> <color=#5AD9F5>per stack of Frostbite.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[5];
                    break;
                case ItemTrait.TraitType.IcePoisonFreezingPoison:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Icy<color=#93D916> Toxins" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "Every <color=#93D916>Poison damage tick</color> has a <color=#5AD9F5>5% chance to add a Frostbite stack.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 20);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 13f);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[5];
                    break;
                case ItemTrait.TraitType.IcePoisonFrostbiteResetsPoisonAndAmps:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Toxic <color=#93D916>Frost" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#5AD9F5>Frostbite</color> <color=#93D916>resets Poison's duration,</color> and increases the damage it deals by 50% for 5 seconds.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[5];
                    break;
                case ItemTrait.TraitType.IcePoisonSummonPoisonPillarOnThreshold:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Frozen Relic <color=#93D916>of Plagues" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "Klling a target with <color=#93D916>Poison stacks</color> and <color=#5AD9F5>25+ Frostbite stacks</color> summons a toxic pillar, adding <color=#93D916>1 Poison</color> and <color=#5AD9F5>Frostbite</color> to nearby enemies for 13 seconds.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 60);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 35);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[5];
                    break;
                case ItemTrait.TraitType.IceStunRudeAwakening:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Shattering <color=#FFF04F>Awakening" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "Hitting a <color=#FFF04F>Stunned target</color> with an <color=#5AD9F5>ice attack adds 2 extra Frostbite stacks.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[5];
                    break;
                case ItemTrait.TraitType.IceStunIceRefreshesStun:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Stunning <color=#FFF04F>Cold" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "Your first <color=#5AD9F5>ice attack</color> on a <color=#FFF04F>Stunned target</color> deals 75% additional damage and <color=#FFF04F>resets the Stun's duration.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[5];
                    break;
                case ItemTrait.TraitType.IceKnockbackFrostbiteIncreasesKnockbackForce:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Aval<color=#1F86CA>anche" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#1F86CA>Knockback force</color> is increased by <color=#5AD9F5>5% per stack of Frostbite.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 20);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 13f);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[5];
                    break;
                case ItemTrait.TraitType.IceKnockbackSnowEruptionOnKnockback:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Staggering <color=#1F86CA>Snow" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#1F86CA>Knocking back</color> a target with <color=#5AD9F5>25+ Frostbite stacks</color> summons a pulse of snow dealing <color=#FFFFFF>60% base damage</color> to neearby enemies.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[5];
                    break;
                case ItemTrait.TraitType.IceKnockbackBonusStacksOnDownedTargets:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Shivering <color=#1F86CA>Shock" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#5AD9F5>Ice Attacks</color> on targets that are <color=#1F86CA>Knocked back</color> add <color=#5AD9F5>3 additional Frostbite stacks.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[5];
                    break;
                case ItemTrait.TraitType.EarthMaxHpDamageAtThreshold:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Sundering Blows" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "When a target reaches <color=#B0946C>25+ Sunder stacks</color>, they take <color=#AD2A2A>10% of their maximum health</color> as <color=#FFFFFF>True damage</color>.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[9];
                    break;
                case ItemTrait.TraitType.EarthAmpAllAfflictionsOnThreshhold:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Exposing Earth" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "Targets with <color=#B0946C>20+ Sunder stacks</color> take 20% more damage from <color=#AD88C5>all Afflictions.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[9];
                    break;
                case ItemTrait.TraitType.EarthSunderedEnemiesDealLessDamage:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Earthern Decay" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#B0946C>Sundered Targets</color> deal 10% reduced damage.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 20);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 13f);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[9];
                    break;
                case ItemTrait.TraitType.EarthRockRingExplosionOnKill:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Rumbling Rocks" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "Killing a <color=#B0946C>Sundered target</color> causes a rock ring explosion, shooting out 8 - 28 rock spears in a circle depending on the <color=#B0946C>target's Sunder stacks</color>. Each spear deals <color=#FFFFFF>125% base damage.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 60);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 35);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[9];
                    break;
                case ItemTrait.TraitType.EarthTrueDamageAtThreshold:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Weathering Earth" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "Targets with <color=#B0946C>50+ Sunder stacks</color> convert all damage they take to <color=#FFFFFF>True damage.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 20);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 13f);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[9];
                    break;
                case ItemTrait.TraitType.EarthSunderFurtherReducesResistances:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Surging Silt" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#B0946C>Sunder</color> reduces the <color=#C353AC>target's Resistances by an additional 15%.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 20);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 13f);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[9];
                    break;
                case ItemTrait.TraitType.EarthIncreasedDamageToLowerArmorTargets:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Splintered Defences" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#B0946C>Earth attacks</color> deal 200% increased damage to targets with <color=#FAFF00>less than 35% of your total armor.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[9];
                    break;
                case ItemTrait.TraitType.EarthAmpDamageOnHealthyTargets:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Subdueing Sands" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#B0946C>Earth attacks</color> deal 150% increased damage to <color=#AD2A2A>targets above 90% hp.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[9];
                    break;
                case ItemTrait.TraitType.EarthHealOnCritAtSunderThreshold:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Renewing Earths" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#E94653>Critting</color> a target with <color=#B0946C>25+ Sunder stacks</color> <color=#60D46D>heals you for 25% of the damage dealt.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[9];
                    break;
                case ItemTrait.TraitType.EarthPhysicalBonusSunderStacksOnThreshold:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Sundering <color=#E94653>Steel" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#E94653>Physical attacks</color> on targets with <color=#B0946C>20+ Sunder stacks</color> adds an <color=#B0946C>additional 3 Sunder stacks.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[9];
                    break;
                case ItemTrait.TraitType.EarthPhysicalSunderAmpsCrits:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Devasting Dance <color=#E94653>of Stone" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#E94653>Physical attacks that critically strike</color> deal an additional <color=#E94653>2% increased critical damage</color> per <color=#B0946C>stack of Sunder.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[9];
                    break;
                case ItemTrait.TraitType.EarthPhysicalSunderAmpsDamage:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Brutal Ballad <color=#E94653>of Earth" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#E94653>Physical attacks deal 1% increased damage</color> per <color=#B0946C>stack of Sunder.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[9];
                    break;
                case ItemTrait.TraitType.EarthBleedBonusCritChanceOnBleedingTarget:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Exposing <color=#AB181D>Stones" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#B0946C>Earth attacks</color> on a target with both <color=#AB181D>Bleeding</color> and <color=#B0946C>Sunder</color> gains <color=#E94653>1% critical strike chance</color> <color=#B0946C>per stack of Sunder.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[9];
                    break;
                case ItemTrait.TraitType.EarthBleedSunderAddsPercentageOfBleed:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Shards of <color=#AB181D>Shredding Stone" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#B0946C>Earth attacks</color> add <color=#AB181D>5% of the current Bleeding stacks as new Bleeding stacks</color>, rounded down.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[9];
                    break;
                case ItemTrait.TraitType.EarthBleedBonusEarthDamageToBleeding:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Muddled <color=#AB181D>Blood" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#B0946C>Earth attacks</color> on a <color=#AB181D>target with Bleeding deals 3% more damage per stack of Bleeding.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[9];
                    break;
                case ItemTrait.TraitType.EarthBleedBloodExplosionOnBleed:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Well of <color=#AB181D>Blood" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#AB181D>Bleed attacks</color> on a target with <color=#B0946C>20+ Sunder stacks</color> summons a blood explosion, dealing <color=#FFFFFF>125% base damage</color> to nearby enemies.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[9];
                    break;
                case ItemTrait.TraitType.EarthPoisonAddSunderedOnPoisonTick:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Sundering <color=#93D916>Venom" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#93D916>Poison damage</color> over time has a 5% chance to add a <color=#B0946C>Sunder stack</color> every tick.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[9];
                    break;
                case ItemTrait.TraitType.EarthPoisonSummonPillarOnThreshold:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Pillar of <color=#93D916>Plagues" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "The first <color=#B0946C>Earth attack</color> that hits a target with <color=#93D916>20+ Poison stacks</color> summons a poison pillar, adding <color=#B0946C>1 Sunder</color> and <color=#93D916>Poison stacks</color> to nearby enemies every second for 13 seconds.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 60);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 35);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[9];
                    break;
                case ItemTrait.TraitType.EarthPoisonSunderToPoisonConversion:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Plagued <color=#93D916>Earth" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#93D916>Poison attacks</color> on a target with <color=#B0946C>20+ Sunder stacks</color> convert all <color=#B0946C>Sunder stacks</color> into <color=#93D916>Poison stacks, plus 1 additional Poison stack.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[9];
                    break;
                case ItemTrait.TraitType.EarthPoisonSunderToPoisonOnCrit:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Toxic Mud <color=#93D916>of Devastation" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#93D916>Poison attacks</color> that <color=#E94453>critically strike</color> a target with <color=#B0946C>Sunder stacks</color> add <color=#93D916>bonus Poison stacks</color> equal to the <color=#B0946C>Sunder stacks + 1.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 60);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 35);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[9];
                    break;
                case ItemTrait.TraitType.EarthStunStunOnThreshold:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Stunning <color=#FFF04F>Earth" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#FFF04F>Stun a target</color> that reaches <color=#B0946C>10+ Sunder stacks.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 20);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 13f);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[9];
                    break;
                case ItemTrait.TraitType.EarthStunBonusDamageOnStun:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Dazing <color=#FFF04F>Dirt" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "Attacks on a <color=#B0946C>Sundered target</color> that is also <color=#FFF04F>Stunned</color> deal a bonus 75% base damage.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[9];
                    break;
                case ItemTrait.TraitType.EarthStunKillingStunnedWithEarthRefundsCooldowns:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Barraging <color=#FFF04F>Earth" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "Killing a <color=#FFF04F>Stunned target</color> with an <color=#B0946C>Earth attack</color> refunds <color=#5DB1E5>25% of your ability cooldowns.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[9];
                    break;
                case ItemTrait.TraitType.EarthStunStunningAddsSunder:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Stone Strikes <color=#FFF04F>of Confoundment" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#FFF04F>Stunning</color> a target adds <color=#B0946C>1 Sunder stack.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 20);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 13f);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[9];
                    break;
                case ItemTrait.TraitType.EarthStunSunderAmpsStunDamageLength:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Earth <color=#FFF04F>Amplification" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "Increase duration of <color=#FFF04F>Stuns by 1%</color> for every <color=#B0946C>Sunder stack.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 20);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 13f);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[9];
                    break;
                case ItemTrait.TraitType.EarthKnockbackTremorsOnKnockback:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Revirbe<color=#1F86CA>rations" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#1F86CA>Knocking back</color> a target with <color=#B0946C>10+ Sunder stacks</color> creates revirberations on the target, dealing <color=#FFFFFF>60% base damage</color> every second for 4 seconds.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 60);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 35);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[9];
                    break;
                case ItemTrait.TraitType.EarthKnockbackSunderReducesKnockbackResistance:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Unstable <color=#1F86CA>Earth" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#B0946C>Sundering</color> a target shreds an additional <color=#1F86CA>5% Knockback Resistance</color>, stacking up to 2 times.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[9];
                    break;
                case ItemTrait.TraitType.EarthKnockbackSummonRocksOnRecentKnockbackTarget:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Stone <color=#1F86CA>Reverberation" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "Your first <color=#B0946C>earth attack</color> on a <color=#1F86CA>Knocked back</color> target summons an rock pulse, dealing <color=#FFFFFF>300% base damage.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[9];
                    break;
                case ItemTrait.TraitType.WindAmpsDamageTaken:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Cutting Winds" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#ABD1E0>Windshear</color> increases all damage taken by 0.5% per stack of windshear.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[8];
                    break;
                case ItemTrait.TraitType.WindAmpsComboArmorShred:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Raging Gusts" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "Combos ignore <color=#FAFF00>20% more armor</color> for targets with <color=#ABD1E0>20+ Windshear stacks.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[8];
                    break;
                case ItemTrait.TraitType.WindTargetGainsBleedOnAttack:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Singing Slices" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "Targets that attack with <color=#ABD1E0>20+ Windshear stacks</color> gain <color=#AB181D>1 Bleeding stack.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 20);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 13f);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[8];
                    break;
                case ItemTrait.TraitType.WindSummonAerobladesOnThreshold:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Song of Dancing Blades" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#ABD1E0>Wind spells</color> on targets with <color=#ABD1E0>20+ Windshear stacks</color> summon 3 airblades that deal <color=#FFFFFF>35% base damage</color> to targets";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[8];
                    break;
                case ItemTrait.TraitType.WindWindshearAmpsTrueDamage:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Rending Galestorm" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "Targets take <color=#FFFFFF>1% more True damage</color> from all sources per <color=#ABD1E0>stack of Windshear.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[8];
                    break;
                case ItemTrait.TraitType.WindAddMoreStacksOnInitialStack:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Sudden Gails" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "Your first application of <color=#ABD1E0>Windshear</color> on a target applies an <color=#ABD1E0>extra 3 Windshear stacks.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[8];
                    break;
                case ItemTrait.TraitType.WindMoreDamageOnMaximumStacks:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Exposing Zephyr" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "Targets with <color=#ABD1E0>maximum Windshear stacks</color> recieve 200% more damage from all sources.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[8];
                    break;
                case ItemTrait.TraitType.WindPhysicalSummonWhirlwindOnSkillHit:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Steel <color=#E94653>Whirlwind" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#E94653>Physical skills</color> that strike a target with <color=#ABD1E0>Windshear stacks</color> summons a whirlwind, dealing <color=#FFFFFF>80% base damage</color> a second for 4 seconds.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[8];
                    break;
                case ItemTrait.TraitType.WindPhysicalWindshearAmpsBasicAttacks:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Wrath of the <color=#E94653>Biting Wind" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "Basic attacks deal 3% more damage per <color=#ABD1E0>stack of Windshear.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[8];
                    break;
                case ItemTrait.TraitType.WindPhysicalCritsDealArmorAsDamage:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Devasting <color=#E94653>Rend" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#E94653>Physical Critical strikes</color> deal <color=#FAFF00>5% of the target's armor</color> as bonus damage per <color=#ABD1E0>stack of Windshear.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[8];
                    break;
                case ItemTrait.TraitType.WindBleedAmpBleedAtThreshold:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Maiming <color=#AB181D>Gusts" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "Targets with <color=#ABD1E0>25+ Windshear stacks</color> recieve <color=#AB181D>35% more damage from Bleeding.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[8];
                    break;
                case ItemTrait.TraitType.WindBleedMoreBleedStacksAThreshold:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Rending <color=#AB181D>Currents" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#AB181D>Bleeding a target</color> with <color=#ABD1E0>50+ Windshear stacks</color> adds <color=#AB181D>50% of the current Bleeding stacks as bonus Bleeding stacks.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[8];
                    break;
                case ItemTrait.TraitType.WindBleedBleedGrantsWindCritChance:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Gouging <color=#AB181D>Gale" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#ABD1E0>Wind attacks</color> gain <color=#E94453>1% increased critical strike chance</color> per <color=#AB181D>stack of Bleeding on the target.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[8];
                    break;
                case ItemTrait.TraitType.WindBleedAddBleedOnWindCrit:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Razer <color=#AB181D>Zephyr" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#ABD1E0>Wind Critical Strikes</color> add <color=#AB181D>2 bonus Bleeding stacks.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 20);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 13f);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[8];
                    break;
                case ItemTrait.TraitType.WindPoisonTransferPoisonStacksOnKill:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Toxic <color=#93D916>Front" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "Killing a target with <color=#ABD1E0>10+ stacks of Windshear</color> causes your next attack to add <color=#93D916>bonus Poison stacks</color> equal to the number of <color=#93D916>Poison stacks</color> the target had when they died.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 60);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 35);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[8];
                    break;
                case ItemTrait.TraitType.WindPoisonWindAddsPercentageOfPoisonOnHit:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Plagued <color=#93D916>Winds" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#ABD1E0>Wind attacks</color> on a target add <color=#93D916>20% of the target's current Poison stacks as bonus Poison stacks.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[8];
                    break;
                case ItemTrait.TraitType.WindPoisonPoisonBurstAtWindThreshold:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Venomous <color=#93D916>Pulse" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#93D916>Poison attacks</color> on a target with <color=#ABD1E0>15+ Windshear stacks</color> summons a poison burst, adding <color=#93D916>1 poison stack</color> to nearby enemies.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[8];
                    break;
                case ItemTrait.TraitType.WindStunStunDealsTrueDamageAtThreshold:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Stunning <color=#FFF04F>Gale" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#FFF04F>Stunning attacks</color> deal 300% more damage on a traget with <color=#ABD1E0>50+ Windshear stacks</color> and consumes the stacks.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[8];
                    break;
                case ItemTrait.TraitType.WindStunWindblastOnStun:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Dazing <color=#FFF04F>Blast" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#FFF04F>Stunning attacks</color> on a target with <color=#ABD1E0>30+ Windshear stacks</color> summons a wind burst, dealing <color=#FFFFFF>100% base damage</color> to nearby enemies.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[8];
                    break;
                case ItemTrait.TraitType.WindStunStunAmpsWindshearGain:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Exposing <color=#FFF04F>Stuns" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#FFF04F>Stunned targets</color> recieve <color=#ABD1E0>100% more stacks of Windshear.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 20);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 13f);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[8];
                    break;
                case ItemTrait.TraitType.WindKnockbackKnockbackSummonsMiniCyclone:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Howling <color=#1F86CA>Gales" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#1F86CA>Knocking back</color> a target with <color=#ABD1E0>15+ Windshear stacks</color> summons a whirlwind dealing <color=#FFFFFF>66% base damage</color> a second for 4 seconds.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[8];
                    break;
                case ItemTrait.TraitType.WindKnockbackLoseKnockbackResistanceOnThreshold:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Unstable <color=#1F86CA>Footing" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "Targets with <color=#ABD1E0>10+ Windshear stacks</color> have their <color=#1F86CA>Knockback Resistance reduced by 25%</color>.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[8];
                    break;
                case ItemTrait.TraitType.WindKnockbackWindshearDoesDamageIfKnockedBack:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Unending <color=#1F86CA>Gusts" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "Adding <color=#ABD1E0>Windshear</color> to a <color=#1F86CA>Knocked back target</color> deals a bonus <color=#FFFFFF>20% base damage.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[8];
                    break;
                case ItemTrait.TraitType.PhysicalPhysicalAmpsCritChance:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Calculated Strikes" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#E94453>Physical attacks</color> grant you <color=#E94453>2% Critical Strike chance</color> for 10 seconds, stacking up to 10 seconds.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[1];
                    break;
                case ItemTrait.TraitType.PhysicalPhysicalSkillsComboAmp:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Press the Advantage" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#E94453>Physical skills</color> ignore <color=#FAFF00>35% more armor</color> from combos.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 20);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 13f);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[1];
                    break;
                case ItemTrait.TraitType.PhysicalLifestealAmp:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Hungering Steel" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#60D46D>Lifesteal</color> on a <color=#E94453>Physical attack</color> is increased by 35%.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 20);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 13f);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[1];
                    break;
                case ItemTrait.TraitType.PhysicalSkillAmpArmorOnKill:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Bolstering Blades" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "Killing an enemy with a <color=#E94453>Physical attack</color> increases your <color=#FAFF00>Armor by 5%</color> for 5 seconds, stacking up to 3 times.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[1];
                    break;
                case ItemTrait.TraitType.PhysicalAmpDamageBelowHalfHp:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Beserker Rage" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#E94453>Physical attacks</color> deal 50% increased damage if you are below <color=#AD2A2A>50% your maximum health</color> total.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[1];
                    break;
                case ItemTrait.TraitType.PhysicalBleedBleedAmpsPhysicalDamage:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Wounding <color=#AB181D>Cuts" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#E94453>Physical damage deals 1% more damage</color> per <color=#AB181D>stack of Bleeding</color> on the target.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[1];
                    break;
                case ItemTrait.TraitType.PhysicalBleedPhysicalSkillsAddBleed:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Evice<color=#AB181D>rate" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#E94453>Physical skills</color> add <color=#AB181D>1 Bleeding stack</color> to targets on hit.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 20);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 13f);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[1];
                    break;
                case ItemTrait.TraitType.PhysicalBleedSkillsDoTrueDamageAtThreshold:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Rending <color=#AB181D>Assault" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#E94453>Physical skills</color> on a target with <color=#AB181D>50+ Bleeding stacks</color> deal <color=#FFFFFF>True damage.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[1];
                    break;
                case ItemTrait.TraitType.PhysicalPoisonPhysicalAmpsPoisonDamage:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Poison Coa<color=#93D916>ted Steel" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#E94453>Physical attacks</color> amp <color=#93D916>Poison damage</color> by 10% for 5 seconds, stacking up to 5 times.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[1];
                    break;
                case ItemTrait.TraitType.PhysicalPoisonPlayerMaxHpDamageOnThreshold:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Virulent <color=#93D916>Toxins" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#E94453>Physical attacks</color> to targets with <color=#93D916>25+ Poison stacks</color> deal <color=#AD2A2A>5% of the target's maximum health</color> as bonus damage.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[1];
                    break;
                case ItemTrait.TraitType.PhysicalPoisonPoisonAmpsPhysicalDamage:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Weakening <color=#93D916>Venom" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#93D916>Poison damage</color> amplifies <color=#E94453>Physical damage</color> by 10% for 5 seconds, stacking up to 5 times.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[1];
                    break;
                case ItemTrait.TraitType.PhysicalStunAmpDamageOnStunned:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Oppurtune <color=#FFF04F>Strikes" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#E94453>Physical attacks</color> on a <color=#FFF04F>Stunned</color> target deal 200% increased damage.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[1];
                    break;
                case ItemTrait.TraitType.PhysicalStunBladeRiftOnStun:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Whirling <color=#FFF04F>Blades" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#E94453>Stunning</color> a target summons a blade rift, dealing <color=#FFFFFF>100% base damage</color> a second for 5 seconds.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[1];
                    break;
                case ItemTrait.TraitType.PhysicalKnockbackKnockbackKillAmpsPhysicalDamage:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Rising <color=#1F86CA>Force" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "Killing a <color=#1F86CA>Knocked back</color> target with a <color=#E94453>Physical damage</color> attack causes your next <color=#E94453>Physical attack</color> within 10 seconds to deal 400% increased damage.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 60);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 35);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[1];
                    break;
                case ItemTrait.TraitType.PhysicalKnockbackPhysicalAttacksGainInnateKnockback:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Crushing <color=#1F86CA>Blows" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#E94453>Physical attacks</color> have a 10% chance to <color=#1F86CA>Knockback</color> their targets.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 20);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 13f);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[1];
                    break;
                case ItemTrait.TraitType.PhysicalKnockbackSummonKnivesOnKnockbackHit:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Dancing <color=#1F86CA>Blades" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#E94453>Knocking back</color> a target summons 3 tracking knives that deal <color=#FFFFFF>50% base damage</color> each.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[1];
                    break;
                case ItemTrait.TraitType.BleedReducesResistances:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Vulnerable Cuts" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#AB181D>Bleed stacks</color> now reduce the target's <color=#C353AC>Resistances by 0.2%</color> a stack.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[10];
                    break;
                case ItemTrait.TraitType.BleedAmpsCritHitsAddsBleedToNearby:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Rushing Blood" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#E94453>Critical strikes</color> deal 2.5% increased damage per <color=#AB181D>stack of Bleed,</color> and summon a blood explosion adding <color=#AB181D>Bleed</color> to neaarby enemies.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[10];
                    break;
                case ItemTrait.TraitType.BleedSlowsTargets:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Crippling Strikes" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#AB181D>Bleed stacks</color> <color=#5DB1E5>slow the target</color> by 2% per stack of <color=#AB181D>Bleed.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 20);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 13f);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[10];
                    break;
                case ItemTrait.TraitType.BleedAmpDamageTakenOnAttack:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Deep Wounds" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "Targets that attack with <color=#AB181D>stacks of Bleed</color> take 50% more damage from the <color=#AB181D>Bleed affliction.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[10];
                    break;
                case ItemTrait.TraitType.BleedHealOnBleedingEnemyKill:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Sanguine Pact" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "Killing a target <color=#60D46D>heals you for 1hp</color> per <color=#AB181D>stack of Bleed</color> on the target.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 20);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 13f);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[10];
                    break;
                case ItemTrait.TraitType.BleedCritsConsumeBleed:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Expunging Strike" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#E94453>Critical strikes</color> consume <color=#AB181D>Bleed stacks</color> to increase <color=#E94453>Crit damage by 10%</color> per <color=#AB181D>stack of Bleed.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[10];
                    break;
                case ItemTrait.TraitType.BleedAmpDamageAtThreshold:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Flowing Blood" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "Targets with <color=#AB181D>10+ stacks of Bleed</color> take 25% increased damage from all sources.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 20);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 13f);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[10];
                    break;
                case ItemTrait.TraitType.BleedBloodWellAtThreshold:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Tides Of Blood" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "Striking a target with <color=#AB181D>20+ Bleed stacks</color> summons a bloodwell, dealing <color=#FFFFFF>100% base damage</color> a second for 7 seconds.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[10];
                    break;
                case ItemTrait.TraitType.BleedExpungeBleedAtThreshold:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Torrential Blood Letting" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "Targets that reach <color=#AB181D>100+ stacks of Bleed</color> will expunge the bleed, removing the affliction and dealing it's damage over time instantly.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 60);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 35);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[10];
                    break;
                case ItemTrait.TraitType.BleedPoisonReduceOtherResistances:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Immunocompro<color=#93D916>misation" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#93D916>Poison</color> reduces <color=#AB181D>Bleed resistance</color> by 1% per <color=#93D916>stack of Poison.</color> <color=#AB181D>Bleeding</color> reduces <color=#93D916>Poison resistance</color> by 1% per <color=#AB181D>stack of Bleeding.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 60);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 35);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[10];
                    break;
                case ItemTrait.TraitType.BleedPoisonChanceOfPoisonCloudOnBleed:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Vaporizing <color=#93D916>Toxins" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "Striking a target with <color=#93D916>10+ Poison stacks</color> and <color=#AB181D>10+ Bleeding stacks</color> has a 20% chance to summon a poison cloud adding <color=#93D916>2 Poison stacks<color> to nearby enemies.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 60);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 35);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[10];
                    break;
                case ItemTrait.TraitType.BleedStunStunReducesBleedResistance:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Exposing <color=#FFF04F>Wounds" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "A <color=#FFF04F>Stunned</color> target has is <color=#AB181D>Bleed resistance</color> reduced by 20% for the <color=#FFF04F>stun's duration.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[10];
                    break;
                case ItemTrait.TraitType.BleedStunStunAtThresholdBelowHalfHP:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Dazing <color=#FFF04F>Cuts" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "Adding <color=#AB181D>Bleed</color> to a target with <color=#AB181D>10+ Bleed stacks</color> <color=#AD2A2A>below 50% max health</color> <color=#FFF04F>Stuns the target.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[10];
                    break;
                case ItemTrait.TraitType.BleedStunStunAddsBleed:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Egregious <color=#FFF04F>Stun" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#FFF04F>Stunning</color> a target adds <color=#AB181D>3 Bleed stacks</color> over 3 seconds.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 20);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 13f);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[10];
                    break;
                case ItemTrait.TraitType.BleedKnockbackBonusBleed:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Lacerating <color=#1F86CA>Knockbacks" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#AB181D>Bleed attacks</color> on a <color=#1F86CA>Knocked back</color> target adds <color=#AB181D>1 bonus Bleed stack.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[10];
                    break;
                case ItemTrait.TraitType.BleedKnockbackKnockbackExposionOfBlood:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Pulse of <color=#1F86CA>Blood" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#1F86CA>Knocking back</color> a target with <color=#AB181D>15+ Bleed stacks</color> causes a blood explosion, dealing <color=#FFFFFF>150% base damage</color> and adding <color=#AB181D>3 Bleed stacks</color> to enemies hit.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 60);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 35);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[10];
                    break;
                case ItemTrait.TraitType.BleedKnockbackKnockbackAmpsDamage:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Sanguine <color=#1F86CA>Force" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#1F86CA>Knocking back</color> a target with <color=#AB181D>10+ Bleed stacks</color> causes the target to recieve 25% more damage from all sources for 10 seconds.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[10];
                    break;
                case ItemTrait.TraitType.PoisonSpreadStacksOnDeath:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Decaying Flesh" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "Killing a target summons a poison pulse, transferring <color=#93D916>10% of the target's Poison stacks</color> to nearby enemies.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[11];
                    break;
                case ItemTrait.TraitType.PoisonAmpsLifesteal:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Rejuvinating Venom" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#60D46D>Lifesteal is 25% more effective</color> on targets with <color=#93D916>10+ Poison stacks.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[11];
                    break;
                case ItemTrait.TraitType.PoisonAmpNextAttackAfterPoisonKill:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Contagion" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "Killing a target with <color=#93D916>Poison damage</color> causes your next <color=#93D916>Poison attack</color> within 15 seconds to <color=#E94453>instantly criticaly strike and have it's critical strike damage increased by 100%.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 60);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 35);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[11];
                    break;
                case ItemTrait.TraitType.PoisonAmpDamageOnFirstStack:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Patient Zero" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#93D916>Poisoning</color> a target with no previous <color=#93D916>Poisoned stacks</color> causes the target to take 50% increased damage from <color=#93D916>Poison's damage over time</color> for 10 seconds.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 60);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 35);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[11];
                    break;
                case ItemTrait.TraitType.PoisonPrimaryTraitsAmpPoison:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Multi-front Contagion" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#93D916>Poison deals 50% more damage over time</color> for every other <color=#AD88C5>primary trait the target is afflicted with.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[11];
                    break;
                case ItemTrait.TraitType.PoisonShredArmorOnThreshold:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Corrosive Toxins" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#93D916>Poisoning a target with 30+ Poison stacks</color> <color=#FAFF00>reduces their armor by 40%</color> for 10 seconds.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[11];
                    break;
                case ItemTrait.TraitType.PoisonEnemiesAmpPoisonOnKill:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Plagued Corpses" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "Killing an enemy makes all nearby enemies take <color=#93D916>100% increased damage from Poison's damage over time</color> effect for 10 seconds.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[11];
                    break;
                case ItemTrait.TraitType.PoisonTrueDamageAtThreshold:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Lethal Venom" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "Target's below <color=#AD2A2A>25% maximum health</color> take <color=#FFFFFF>True damage</color> from <color=#93D916>Poison's damage over time</color> effect.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[11];
                    break;
                case ItemTrait.TraitType.PoisonVamperism:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Toxic Predator" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#60D46D>Heal for 5% of the damage dealt</color> by <color=#93D916>Poison's damage over time effect.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[11];
                    break;
                case ItemTrait.TraitType.PoisonStunPoisonReducesStunResist:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Dazing <color=#FFF04F>Pestilence" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "A <color=#93D916>Poisoned</color> target's <color=#FFF04F>Stun resistance</color> is reduced by <color=#93D916>2% per stack of Poison.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[11];
                    break;
                case ItemTrait.TraitType.PoisonStunPoisonSpreadOnThreshold:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Virulent <color=#FFF04F>Pestilence" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "A <color=#FFF04F>Stunning attack</color> on a target with <color=#93D916>20+ Poison stacks</color> consumes the stacks to summon a poison pulse, dealing <color=#FFFFFF>245% base damage</color> and adding <color=#93D916>half of the consumed stacks to nearby enemies.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 80);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 46);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[11];
                    break;
                case ItemTrait.TraitType.PoisonKnockbackPoisonReducesKnockbackResistance:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Knockback of <color=#1F86CA>Plagues" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#93D916>Poisoned</color> targets have their <color=#1F86CA>Knockback resistance</color> reduced by 2% per <color=#93D916>stack of Poisoned.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[11];
                    break;
                case ItemTrait.TraitType.PoisonKnockbackConsumePoisonStacksForTrueDamage:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Venomous <color=#1F86CA>Blow" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#93D916>Knocking back</color> a <color=#93D916>Poisoned target consumes the poison</color> to deal <color=#FFFFFF>50% base damage</color> <color=#93D916>per stack of Poison</color> as <color=#FFFFFF>True damage.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[11];
                    break;
                case ItemTrait.TraitType.StunReducesArmor:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Armor Rending Stuns" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#FFF04F>Stunning</color> a target <color=#FAFF00>reduces their armor by 25%</color> for the <color=#FFF04F>Stun's</color> duration.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[13];
                    break;
                case ItemTrait.TraitType.StunAmpsDamageTaken:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Expose Vulnerbilities" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#FFF04F>Stunned targets</color> recieve 10% more damage from all sources.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 20);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 13f);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[13];
                    break;
                case ItemTrait.TraitType.StunAmpsAfflictionGain:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Calamity Rising" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "A <color=#FFF04F>Stunned target</color> recieves <color=#AD88C5>25% more stacks of primary Afflictions.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[13];
                    break;
                case ItemTrait.TraitType.StunAmpDuration:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Enchanced Stupor" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "Increased <color=#FFF04F>Stun duration by 25%</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 20);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 11);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[13];
                    break;
                case ItemTrait.TraitType.StunShockwaveOnStunningStunned:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Dazing Wave" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#FFF04F>Stunning a target</color> that is currently <color=#FFF04F>Stunned</color> summons a shockwave dealing <color=#FFFFFF>80% base damage.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[13];
                    break;
                case ItemTrait.TraitType.StunOnStunDealsAdditionalBaseDamage:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Echoeing Trauma" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#FFF04F>Stunning a target</color> that is currently <color=#FFF04F>Stunned</color> causes the target to take <color=#FFFFFF>150% base damage.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[13];
                    break;
                case ItemTrait.TraitType.StunAmpDurationOnThreshold:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Misery's Company" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#FFF04F>Stunning a target</color> with <color=#AD88C5>50+ stacks of any primary Affliction</color> increases the <color=#FFF04F>Stun duration by 66%.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[13];
                    break;
                case ItemTrait.TraitType.StunAmpCritDamage:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Mighty Slashes" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#E94453>Critical strikes</color> on a <color=#FFF04F>Stunned target</color> have their <color=#E94453>critical strike damage increased by 25%.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[13];
                    break;
                case ItemTrait.TraitType.StunAmpsMovespeed:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Tactical Repositioning" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#FFF04F>Stunning a target </color><color=#5DB1E5>increases your movespeed by 15%</color> for 8 seconds.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[13];
                    break;
                case ItemTrait.TraitType.StunKnockbackStunReduceResistance:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Unstable <color=#1F86CA>Footing" + GetTraitValueRomanNumeral(item.itemTraits[index]) + "</color>";
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "A <color=#FFF04F>Stunned target</color> has it's <color=#1F86CA>Knockback resistance reduced by 50%</color> for the duration of the <color=#FFF04F>Stun.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[13];
                    break;
                case ItemTrait.TraitType.KnockbackReducesSpellCooldowns:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Press the Assault" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "A <color=#FFF04F>Stunned target</color> has it's <color=#1F86CA>Knockback resistance reduced by 50%</color> for the duration of the stun.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[14];
                    break;
                case ItemTrait.TraitType.KnockBackAmpsBasicAttacks:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Nimble Duelist" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#1F86CA>Knocking back</color> a target increases basic attack damage by 100% for the next three basic attacks.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[14];
                    break;
                case ItemTrait.TraitType.KnockbackAmpKnockbackForce:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Mountains Strength" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#1F86CA>Knockback force</color> is increased by 10%.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 20);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 13);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[14];
                    break;
                case ItemTrait.TraitType.KnockbackAmpsDamageTaken:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Shredded Armor" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "A <color=#1F86CA>Knocked back targe</color>t takes 25% more damage from all sources.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 20);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 13);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[14];
                    break;
                case ItemTrait.TraitType.KnockbackAmpsArmor:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Barricade" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#1F86CA>Knocking back</color> a target <color=#FAFF00>increases your armor by 5% for 8 seconds</color>, stacks up to 20 times.";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[14];
                    break;
                case ItemTrait.TraitType.KnockbackDoesBonusDamageOnKnockbacked:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Chained Blows" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#1F86CA>Knocking back</color> a target that is <color=#1F86CA>Knocked back</color> deals an additional <color=#FFFFFF>33% base damage.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 40);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 24);

                    traitText.GetComponentInChildren<Text>().color = traitTextColors[14];
                    break;
                case ItemTrait.TraitType.KnockbackReducesResistances:
                    traitText.transform.Find("TraitName").GetComponent<Text>().text = "Violent Strikes" + GetTraitValueRomanNumeral(item.itemTraits[index]);
                    traitWidth = traitText.transform.Find("TraitName").GetComponent<Text>().preferredWidth;
                    traitText.transform.Find("TraitName").GetComponent<RectTransform>().sizeDelta = new Vector2(traitWidth, 20);
                    traitText.transform.Find("TraitDesc").GetComponent<Text>().text = "<color=#1F86CA>Knocking back</color> a target also <color=#C353AC>reduces it's resistances by 33%.</color>";
                    traitText.transform.Find("TraitDesc").GetComponent<RectTransform>().sizeDelta = new Vector2(700 - traitWidth, 20);
                    traitText.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 13);

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
        string traitNumeral = " - ";
        switch (itemTrait.traitBonusMultiplier)
        {
            case 1:
                traitNumeral = " - ";
                break;
            case 2:
                traitNumeral = " - II - ";
                break;
            case 3:
                traitNumeral = " - III - ";
                break;
            case 4:
                traitNumeral = " - IV - ";
                break;
            case 5:
                traitNumeral = " - V - ";
                break;
            case 6:
                traitNumeral = " - VI - ";
                break;
            case 7:
                traitNumeral = " - VII - ";
                break;
            case 8:
                traitNumeral = " - VIII - ";
                break;
            case 9:
                traitNumeral = " - IX - ";
                break;
            case 10:
                traitNumeral = " - X - ";
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
