using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUiManager : MonoBehaviour
{
    public Inventory playerInventory;
    public SkillsManager playerSkills;

    public GameObject[] inventorySlots;
    public GameObject[] trinketSlots;
    public GameObject[] skillSlots;
    public GameObject leftHandSlot;
    public GameObject rightHandSlot;
    public GameObject helmetSlot;
    public GameObject chestSlot;
    public GameObject legsSlot;

    public Color[] itemOutlineColors;
    public Color[] itemBackgroundColors;
    public Color[] skillIconColors;

    public Color[] iconAffintiyColors;
    public Color[] iconAffintiyBackgroundColors;
    public Sprite[] iconAffinityIcons;

    public AudioManager audioManager;

    private void Awake()
    {
        audioManager = transform.parent.GetComponent<AudioManager>();
    }

    private void Start()
    {
        Debug.Log("ALL OF THE SLOTS ARE BEING WIPED HERE");
        foreach (GameObject slot in inventorySlots)
            WipeSlot(slot);
        foreach (GameObject slot in trinketSlots)
            WipeSlot(slot);
        foreach (GameObject slot in skillSlots)
            WipeSlot(slot);
        WipeSlot(leftHandSlot);
        WipeSlot(rightHandSlot);
        WipeSlot(helmetSlot);
        WipeSlot(chestSlot);
        WipeSlot(legsSlot);
        CheckActiveSkillSlots();

        gameObject.SetActive(false);

    }

    public void UpdateInventorySlot(Item item)
    {
        // If we have an item, set the picture to that associated with the item ans show thbe image.
        GameObject slotToUpdate = inventorySlots[item.inventoryIndex];
        UpdateSlot(slotToUpdate, item);
    }

    public void UpdateEquipmentSlot(Item item)
    {
        // If we have an item, set the picture to that associated with the item ans show thbe image.
        GameObject slotToUpdate = null;// = inventorySlots[item.inventoryIndex];
        switch (item.itemType)
        {
            case Item.ItemType.Weapon:
                if (item.equippedToRightHand)
                    slotToUpdate = rightHandSlot;
                else
                    slotToUpdate = leftHandSlot;
                break;
            case Item.ItemType.TwoHandWeapon:
                slotToUpdate = rightHandSlot;
                break;
            case Item.ItemType.Helmet:
                slotToUpdate = helmetSlot;
                break;
            case Item.ItemType.Legs:
                slotToUpdate = legsSlot;
                break;
            case Item.ItemType.Armor:
                slotToUpdate = chestSlot;
                break;
            case Item.ItemType.Shield:
                if (item.equippedToRightHand)
                    slotToUpdate = rightHandSlot;
                else
                    slotToUpdate = leftHandSlot;
                break;
            case Item.ItemType.MagicBooster:
                if (item.equippedToRightHand)
                    slotToUpdate = rightHandSlot;
                else
                    slotToUpdate = leftHandSlot;
                break;
            case Item.ItemType.TrinketCape:
                slotToUpdate = trinketSlots[item.inventoryIndex];
                break;
            case Item.ItemType.TrinketBracelet:
                slotToUpdate = trinketSlots[item.inventoryIndex];
                break;
            case Item.ItemType.TrinketWaistItem:
                slotToUpdate = trinketSlots[item.inventoryIndex];
                break;
            case Item.ItemType.TrinketRing:
                slotToUpdate = trinketSlots[item.inventoryIndex];
                break;
            default:
                break;
        }

        UpdateSlot(slotToUpdate, item);
    }

    public void UpdateSlot(GameObject slotToUpdate, Item item)
    {
        slotToUpdate.transform.Find("ItemPanel").GetComponent<ItemDraggable>().enabled = true;
        slotToUpdate.transform.Find("ItemPanel").GetComponent<ItemDraggable>().attachedItem = item.gameObject;

        Image targetImage = slotToUpdate.transform.Find("ItemPanel").Find("ItemImage").GetComponent<Image>();
        targetImage.color = new Color(255, 255, 255, 255);
        targetImage.sprite = item.artwork;

        if (item.itemType == Item.ItemType.Skill)
        {
            switch (item.damageType)
            {
                case HitBox.DamageType.Physical:
                    targetImage.color = skillIconColors[0];
                    break;
                case HitBox.DamageType.Fire:
                    targetImage.color = skillIconColors[1];
                    break;
                case HitBox.DamageType.Ice:
                    targetImage.color = skillIconColors[2];
                    break;
                case HitBox.DamageType.Lightning:
                    targetImage.color = skillIconColors[3];
                    break;
                case HitBox.DamageType.Nature:
                    targetImage.color = skillIconColors[4];
                    break;
                case HitBox.DamageType.Earth:
                    targetImage.color = skillIconColors[6];
                    break;
                case HitBox.DamageType.Wind:
                    targetImage.color = skillIconColors[5];
                    break;
                default:
                    break;
            }
        }
        else
        {
            targetImage.color = new Color(255, 255, 255, 255);
        }

        // If we have more then one in this stack of item enable the counter.
        if (item.currentStack > 1)
            slotToUpdate.transform.Find("ItemPanel").Find("ItemCount").GetComponent<Text>().text = "x" + item.currentStack;
        else
            slotToUpdate.transform.Find("ItemPanel").Find("ItemCount").GetComponent<Text>().text = "";

        Image outlineImage = slotToUpdate.transform.Find("ItemPanel").Find("ItemImage").Find("Outline").GetComponent<Image>();
        Image backgroundImage = slotToUpdate.transform.Find("ItemPanel").Find("Background").GetComponent<Image>();
        switch (item.itemRarity)
        {
            case Item.ItemRarity.Common:
                outlineImage.color = itemOutlineColors[0];
                backgroundImage.color = itemBackgroundColors[0];
                break;
            case Item.ItemRarity.Uncommon:
                outlineImage.color = itemOutlineColors[1];
                backgroundImage.color = itemBackgroundColors[1];
                break;
            case Item.ItemRarity.Rare:
                outlineImage.color = itemOutlineColors[2];
                backgroundImage.color = itemBackgroundColors[2];
                break;
            case Item.ItemRarity.Legendary:
                outlineImage.color = itemOutlineColors[3];
                backgroundImage.color = itemBackgroundColors[3];
                break;
            case Item.ItemRarity.Masterwork:
                outlineImage.color = itemOutlineColors[4];
                backgroundImage.color = itemBackgroundColors[4];
                break;
            default:
                break;
        }

        Image primaryAffinityBackground = slotToUpdate.transform.Find("ItemPanel").Find("ItemImage").Find("Affinity_Primary").GetComponent<Image>();
        switch (item.affinityPrimary)
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

        Image secondaryAffinityBackground = slotToUpdate.transform.Find("ItemPanel").Find("ItemImage").Find("Affinity_Secondary").GetComponent<Image>();
        switch (item.affinitySecondary)
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

        Image tertiaryAffinityBackground = slotToUpdate.transform.Find("ItemPanel").Find("ItemImage").Find("Affinity_Tertiary").GetComponent<Image>();
        switch (item.affinityTertiary)
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

        Image secondaryAffinityBackgroundTwo = slotToUpdate.transform.Find("ItemPanel").Find("ItemImage").Find("Affinity_Secondary").Find("Mask").Find("Affinity_Secondary_LeftHalf").GetComponent<Image>();
        if (item.affinitySecondaryMultiElement)
        {
            secondaryAffinityBackgroundTwo.gameObject.SetActive(true);
            switch (item.affinityPrimary)
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

        Image tertiaryAffinityBackgroundTwo = slotToUpdate.transform.Find("ItemPanel").Find("ItemImage").Find("Affinity_Tertiary").Find("Mask").Find("Affinity_Secondary_LeftHalf").GetComponent<Image>();
        if (item.affinityTertiaryMultiElement)
        {
            tertiaryAffinityBackgroundTwo.gameObject.SetActive(true);
            switch (item.affinityPrimary)
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

    public void UpdateInventorySlot(int index)
    {
        // Used if we are clearing this inventory slot.
        GameObject slotToUpdate = inventorySlots[index];

        WipeSlot(slotToUpdate);
    }

    public void WipeSlot(GameObject slot)
    {
        // Disable the draggable script on this item so that we cant drag around empty items.
        if (slot.transform.Find("ItemPanel") != null)
            slot.transform.Find("ItemPanel").GetComponent<ItemDraggable>().enabled = false;
        slot.transform.Find("ItemPanel").GetComponent<ItemDraggable>().attachedItem = null;

        if (slot.transform.Find("ItemPanel").Find("ItemCount") != null)
            slot.transform.Find("ItemPanel").Find("ItemCount").GetComponent<Text>().text = "";

        Image targetImage = slot.transform.Find("ItemPanel").Find("ItemImage").GetComponent<Image>();
        targetImage.color = new Color(255, 255, 255, 0);
        targetImage.sprite = null;

        slot.transform.Find("ItemPanel").Find("ItemImage").Find("Affinity_Primary").gameObject.SetActive(false);
        slot.transform.Find("ItemPanel").Find("ItemImage").Find("Affinity_Secondary").gameObject.SetActive(false);
        slot.transform.Find("ItemPanel").Find("ItemImage").Find("Affinity_Tertiary").gameObject.SetActive(false);

        slot.transform.Find("ItemPanel").Find("ItemImage").Find("Outline").GetComponent<Image>().color = new Color(255, 255, 255, 0); 
        slot.transform.Find("ItemPanel").Find("Background").GetComponent<Image>().color = new Color(255, 255, 255, 0);
        slot.transform.Find("ItemPanel").Find("HighlightImage").gameObject.SetActive(false);
    }

    public ItemDraggable GetNextEmptySlot()
    {
        // Return the next empty draggable item in the inventory.
        ItemDraggable inventorySlot = null;
        foreach(GameObject slot in inventorySlots)
        {
            // Check to see if the slot is null, just in case the item is unparented because it is being dragged around,
            if (slot.transform.Find("ItemPanel") != null && slot.transform.Find("ItemPanel").GetComponent<ItemDraggable>().attachedItem == null)
            {
                inventorySlot = slot.transform.Find("ItemPanel").GetComponent<ItemDraggable>();
                break;
            }
        }

        return inventorySlot;
    }

    public void CheckActiveSkillSlots()
    {
        //Debug.Log("checking the active skill slots");
        // Here I will only have active skillslots equal to the number of spells the player can currently have.
        //int currentSkillMax = playerSkills.maxSkillNumber;
        for (int index = 0; index < skillSlots.Length; index++)
        {
            if (index < playerSkills.maxSkillNumber)
                skillSlots[index].SetActive(true);
            else
            {
                skillSlots[index].SetActive(false);
                // check to see if there a skill in this skill slot, if so, disable it then move it to inventory. if there is no room in the inventory, drop the skill.
                if(skillSlots[index].GetComponentInChildren<ItemDraggable>().attachedItem != null)
                {
                    //Debug.Log("skill detected in lost skill slot, removing skill");
                    playerSkills.RemoveSkill(index);
                    // Check if we have room in the inventory
                    if (playerInventory.inventory.Count < playerInventory.INVENTORY_MAX - 1)
                    {
                        //Debug.Log("There is room in the inventory so we move this into the next unoccupied slot");
                        Item itemToMove = skillSlots[index].GetComponentInChildren<ItemDraggable>().attachedItem.GetComponent<Item>();
                        itemToMove.inventoryIndex = playerInventory.FindFirstAvaibleSlot();
                        playerInventory.inventory.Add(itemToMove);
                        UpdateInventorySlot(itemToMove);
                        WipeSlot(skillSlots[index]);
                    }
                    else
                    {
                        //Debug.Log("There is no room in the inventory for this item");
                        Item itemToYeet = skillSlots[index].GetComponentInChildren<ItemDraggable>().attachedItem.GetComponent<Item>();
                        itemToYeet.ComfirmDrop();
                        WipeSlot(skillSlots[index]);
                    }
                }
            }
        }
    }

    public void AttemptSnapEquipmentToSlot(ItemDropZone itemSlot)
    {
        Item attachedItem = itemSlot.transform.Find("ItemPanel").GetComponent<ItemDraggable>().attachedItem.GetComponent<Item>();

        Debug.Log($"Attempted to snap {attachedItem} to a relevant slot.");
        // Check if this item dropzone is an inventory or an equipped item.
        if (itemSlot.slotType == ItemDropZone.SlotType.Inventory)
        {
            // Validate to see if we can move this item into an equipped slot here
        }
        else
        {
            Debug.Log("This item will be placed in an empty inventory slot.");
            // Validate that we have an open inventory slot to move this item to.
            if(playerInventory.inventory.Count < playerInventory.INVENTORY_MAX)
            {
                // Unequip our item and move it into the inventory slot
                Debug.Log("Moving out current item to the new slot. Unequipping it too.");
            }
            else
            {
                // Not enough room cancel the action here
                Debug.Log("We didnt have one cancel this movement");
                return;
            }
        }
    }

    public void HighlightSlotType(Item.ItemType itemType)
    {

        switch (itemType)
        {
            case Item.ItemType.TrinketRing:
                foreach (GameObject slot in trinketSlots)
                    slot.transform.Find("ItemPanel")?.Find("HighlightImage").gameObject.SetActive(true);
                break;
            case Item.ItemType.Weapon:
                leftHandSlot.transform.Find("ItemPanel")?.Find("HighlightImage").gameObject.SetActive(true);
                rightHandSlot.transform.Find("ItemPanel")?.Find("HighlightImage").gameObject.SetActive(true);
                break;
            case Item.ItemType.TwoHandWeapon:
                rightHandSlot.transform.Find("ItemPanel")?.Find("HighlightImage").gameObject.SetActive(true);
                break;
            case Item.ItemType.Helmet:
                helmetSlot.transform.Find("ItemPanel")?.Find("HighlightImage").gameObject.SetActive(true);
                break;
            case Item.ItemType.Legs:
                legsSlot.transform.Find("ItemPanel")?.Find("HighlightImage").gameObject.SetActive(true);
                break;
            case Item.ItemType.Armor:
                chestSlot.transform.Find("ItemPanel")?.Find("HighlightImage").gameObject.SetActive(true);
                break;
            case Item.ItemType.Shield:
                leftHandSlot.transform.Find("ItemPanel")?.Find("HighlightImage").gameObject.SetActive(true);
                rightHandSlot.transform.Find("ItemPanel")?.Find("HighlightImage").gameObject.SetActive(true);
                break;
            case Item.ItemType.MagicBooster:
                leftHandSlot.transform.Find("ItemPanel")?.Find("HighlightImage").gameObject.SetActive(true);
                rightHandSlot.transform.Find("ItemPanel")?.Find("HighlightImage").gameObject.SetActive(true);
                break;
            case Item.ItemType.Skill:
                foreach (GameObject slot in skillSlots)
                    slot.transform.Find("ItemPanel")?.Find("HighlightImage").gameObject.SetActive(true);
                break;
            case Item.ItemType.TrinketCape:
                foreach (GameObject slot in trinketSlots)
                    slot.transform.Find("ItemPanel")?.Find("HighlightImage").gameObject.SetActive(true);
                break;
            case Item.ItemType.TrinketBracelet:
                foreach (GameObject slot in trinketSlots)
                    slot.transform.Find("ItemPanel")?.Find("HighlightImage").gameObject.SetActive(true);
                break;
            case Item.ItemType.TrinketWaistItem:
                foreach (GameObject slot in trinketSlots)
                    slot.transform.Find("ItemPanel")?.Find("HighlightImage").gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void HighlightHideAll()
    {

        foreach (GameObject slot in inventorySlots)
            if(slot.transform.Find("ItemPanel") != null)
                slot.transform.Find("ItemPanel").Find("HighlightImage").gameObject.SetActive(false);
        foreach (GameObject slot in trinketSlots)
            slot.transform.Find("ItemPanel")?.Find("HighlightImage").gameObject.SetActive(false);
        foreach (GameObject slot in skillSlots)
            slot.transform.Find("ItemPanel")?.Find("HighlightImage").gameObject.SetActive(false);
        legsSlot.transform.Find("ItemPanel")?.Find("HighlightImage").gameObject.SetActive(false);
        chestSlot.transform.Find("ItemPanel")?.Find("HighlightImage").gameObject.SetActive(false);
        helmetSlot.transform.Find("ItemPanel")?.Find("HighlightImage").gameObject.SetActive(false);
        leftHandSlot.transform.Find("ItemPanel")?.Find("HighlightImage").gameObject.SetActive(false);
        rightHandSlot.transform.Find("ItemPanel")?.Find("HighlightImage").gameObject.SetActive(false);
    }


}
