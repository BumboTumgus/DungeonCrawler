using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    public static ItemGenerator instance;

    public GameObject[] weapons1hCommon;
    public GameObject[] weapons1hUncommon;
    public GameObject[] weapons1hRare;
    public GameObject[] weapons1hLegendary;
    public GameObject[] weapons1hMasterwork;

    public GameObject[] weapons2hCommon;
    public GameObject[] weapons2hUncommon;
    public GameObject[] weapons2hRare;
    public GameObject[] weapons2hLegendary;
    public GameObject[] weapons2hMasterwork;

    public GameObject[] chestArmorCommon;
    public GameObject[] chestArmorUncommon;
    public GameObject[] chestArmorRare;
    public GameObject[] chestArmorLegendary;
    public GameObject[] chestArmorMasterwork;

    public GameObject[] legArmorCommon;
    public GameObject[] legArmorUncommon;
    public GameObject[] legArmorRare;
    public GameObject[] legArmorLegendary;
    public GameObject[] legArmorMasterwork;

    public GameObject[] helmetsCommon;
    public GameObject[] helmetsUncommon;
    public GameObject[] helmetsRare;
    public GameObject[] helmetsLegendary;
    public GameObject[] helmetsMasterwork;

    public GameObject[] trinketsCapeCommon;
    public GameObject[] trinketsCapeUncommon;
    public GameObject[] trinketsCapeRare;
    public GameObject[] trinketsCapeLegendary;
    public GameObject[] trinketsCapeMasterwork;

    public GameObject[] trinketsRingCommon;
    public GameObject[] trinketsRingUncommon;
    public GameObject[] trinketsRingRare;
    public GameObject[] trinketsRingLegendary;
    public GameObject[] trinketsRingMasterwork;

    public GameObject[] trinketsBraceletCommon;
    public GameObject[] trinketsBraceletUncommon;
    public GameObject[] trinketsBraceletRare;
    public GameObject[] trinketsBraceletLegendary;
    public GameObject[] trinketsBraceletMasterwork;

    public GameObject[] trinketsWaistCommon;
    public GameObject[] trinketsWaistUncommon;
    public GameObject[] trinketsWaistRare;
    public GameObject[] trinketsWaistLegendary;
    public GameObject[] trinketsWaistMasterwork;

    public GameObject[] skillsCommon;
    public GameObject[] skillsUncommon;
    public GameObject[] skillsRare;
    public GameObject[] skillsLegendary;
    public GameObject[] skillsMasterwork;

    public int currentRcIndex = -1;

    private float[] commonRcValues = { 75, 56, 22, 10, 8, 7, 0, 0, 0 };
    private float[] uncommonRcValues = { 20, 30, 50, 40, 20, 15, 10, 0, 0 };
    private float[] rareRcValues = { 4, 10, 22, 40, 50, 40, 38, 34, 20 };
    private float[] legendarytRcValues = { 1, 4, 6, 10, 20, 30, 40, 50, 60 };
    private float[] masterworkRcValues = { 0, 0, 0, 0, 2, 8, 12, 16, 20 };

    private float commonRC = 40;
    private float uncommonRC = 30;
    private float rareRC = 30;
    private float legendaryRC = 35;
    private float masterworkRC = 5;
    // by the 11th level, we cap it so the player only recieves legendary at 87.5% percent and masterwork at 12.5%;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        IncrementRcIndex();
    }

    public void IncrementRcIndex()
    {
        currentRcIndex++;

        if (currentRcIndex >= commonRcValues.Length)
            return;
        else
        {
            commonRC = commonRcValues[currentRcIndex];
            uncommonRC = uncommonRcValues[currentRcIndex];
            rareRC = rareRcValues[currentRcIndex];
            legendaryRC = legendarytRcValues[currentRcIndex];
            masterworkRC = masterworkRcValues[currentRcIndex];
        }
    }

    public GameObject RollItem(float bonusToRarityRoll)
    {
        GameObject itemRolled = null;

        float itemDiceRoll = Random.Range(0, 9);
        Item.ItemType itemType = Item.ItemType.Weapon;
        float itemRarityDiceRoll = Random.Range(0, 100) - bonusToRarityRoll;
        Item.ItemRarity itemRarity = Item.ItemRarity.Common;

        if (itemDiceRoll == 0 || itemDiceRoll == 1)
            if (Random.Range(0, 100) < 66)
                itemType = Item.ItemType.Weapon;
            else
                itemType = Item.ItemType.TwoHandWeapon;
        else if(itemDiceRoll == 2 || itemDiceRoll == 3)
        {
            int randomIndex = Random.Range(0, 4);
            switch (randomIndex)
            {
                case 0:
                    itemType = Item.ItemType.TrinketCape;
                    break;
                case 1:
                    itemType = Item.ItemType.TrinketBracelet;
                    break;
                case 2:
                    itemType = Item.ItemType.TrinketRing;
                    break;
                case 3:
                    itemType = Item.ItemType.TrinketWaistItem;
                    break;
                default:
                    break;
            }
        }
        else if (itemDiceRoll == 4 || itemDiceRoll == 5)
            itemType = Item.ItemType.Skill;
        else if (itemDiceRoll == 6)
            itemType = Item.ItemType.Helmet;
        else if (itemDiceRoll == 7)
            itemType = Item.ItemType.Armor;
        else if (itemDiceRoll == 8)
            itemType = Item.ItemType.Legs;

        //Debug.Log(itemDiceRoll + " | this means we have a " + itemType);

        if (itemRarityDiceRoll <= masterworkRC && masterworkRC != 0)
            itemRarity = Item.ItemRarity.Masterwork;
        else if (itemRarityDiceRoll <= masterworkRC + legendaryRC && legendaryRC != 0)
            itemRarity = Item.ItemRarity.Legendary;
        else if (itemRarityDiceRoll <= masterworkRC + legendaryRC + rareRC && rareRC != 0)
            itemRarity = Item.ItemRarity.Rare;
        else if (itemRarityDiceRoll <= masterworkRC + legendaryRC + rareRC + uncommonRC && uncommonRC != 0)
            itemRarity = Item.ItemRarity.Uncommon;
        else if (itemRarityDiceRoll <= masterworkRC + legendaryRC + rareRC + uncommonRC + commonRC && commonRC != 0)
            itemRarity = Item.ItemRarity.Common;

        //Debug.Log(itemRarityDiceRoll + " | this means our rarity is " + itemRarity);

        // This assigns us a bank of items for us to use.
        switch (itemType)
        {
            case Item.ItemType.TrinketCape:
                switch (itemRarity)
                {
                    case Item.ItemRarity.Common:
                        itemRolled = trinketsCapeCommon[Random.Range(0, trinketsCapeCommon.Length)];
                        break;
                    case Item.ItemRarity.Uncommon:
                        itemRolled = trinketsCapeUncommon[Random.Range(0, trinketsCapeUncommon.Length)];
                        break;
                    case Item.ItemRarity.Rare:
                        itemRolled = trinketsCapeRare[Random.Range(0, trinketsCapeRare.Length)];
                        break;
                    case Item.ItemRarity.Legendary:
                        itemRolled = trinketsCapeLegendary[Random.Range(0, trinketsCapeLegendary.Length)];
                        break;
                    case Item.ItemRarity.Masterwork:
                        itemRolled = trinketsCapeMasterwork[Random.Range(0, trinketsCapeMasterwork.Length)];
                        break;
                    default:
                        break;
                }
                break;
            case Item.ItemType.TrinketBracelet:
                switch (itemRarity)
                {
                    case Item.ItemRarity.Common:
                        itemRolled = trinketsBraceletCommon[Random.Range(0, trinketsBraceletCommon.Length)];
                        break;
                    case Item.ItemRarity.Uncommon:
                        itemRolled = trinketsBraceletUncommon[Random.Range(0, trinketsBraceletUncommon.Length)];
                        break;
                    case Item.ItemRarity.Rare:
                        itemRolled = trinketsBraceletRare[Random.Range(0, trinketsBraceletRare.Length)];
                        break;
                    case Item.ItemRarity.Legendary:
                        itemRolled = trinketsBraceletLegendary[Random.Range(0, trinketsBraceletLegendary.Length)];
                        break;
                    case Item.ItemRarity.Masterwork:
                        itemRolled = trinketsBraceletMasterwork[Random.Range(0, trinketsBraceletMasterwork.Length)];
                        break;
                    default:
                        break;
                }
                break;
            case Item.ItemType.TrinketRing:
                switch (itemRarity)
                {
                    case Item.ItemRarity.Common:
                        itemRolled = trinketsRingCommon[Random.Range(0, trinketsRingCommon.Length)];
                        break;
                    case Item.ItemRarity.Uncommon:
                        itemRolled = trinketsRingUncommon[Random.Range(0, trinketsRingUncommon.Length)];
                        break;
                    case Item.ItemRarity.Rare:
                        itemRolled = trinketsRingRare[Random.Range(0, trinketsRingRare.Length)];
                        break;
                    case Item.ItemRarity.Legendary:
                        itemRolled = trinketsRingLegendary[Random.Range(0, trinketsRingLegendary.Length)];
                        break;
                    case Item.ItemRarity.Masterwork:
                        itemRolled = trinketsRingMasterwork[Random.Range(0, trinketsRingMasterwork.Length)];
                        break;
                    default:
                        break;
                }
                break;
            case Item.ItemType.TrinketWaistItem:
                switch (itemRarity)
                {
                    case Item.ItemRarity.Common:
                        itemRolled = trinketsWaistCommon[Random.Range(0, trinketsWaistCommon.Length)];
                        break;
                    case Item.ItemRarity.Uncommon:
                        itemRolled = trinketsWaistUncommon[Random.Range(0, trinketsWaistUncommon.Length)];
                        break;
                    case Item.ItemRarity.Rare:
                        itemRolled = trinketsWaistRare[Random.Range(0, trinketsWaistRare.Length)];
                        break;
                    case Item.ItemRarity.Legendary:
                        itemRolled = trinketsWaistLegendary[Random.Range(0, trinketsWaistLegendary.Length)];
                        break;
                    case Item.ItemRarity.Masterwork:
                        itemRolled = trinketsWaistMasterwork[Random.Range(0, trinketsWaistMasterwork.Length)];
                        break;
                    default:
                        break;
                }
                break;
            case Item.ItemType.Weapon:
                switch (itemRarity)
                {
                    case Item.ItemRarity.Common:
                        itemRolled = weapons1hCommon[Random.Range(0, weapons1hCommon.Length)];
                        break;
                    case Item.ItemRarity.Uncommon:
                        itemRolled = weapons1hUncommon[Random.Range(0, weapons1hUncommon.Length)];
                        break;
                    case Item.ItemRarity.Rare:
                        itemRolled = weapons1hRare[Random.Range(0, weapons1hRare.Length)];
                        break;
                    case Item.ItemRarity.Legendary:
                        itemRolled = weapons1hLegendary[Random.Range(0, weapons1hLegendary.Length)];
                        break;
                    case Item.ItemRarity.Masterwork:
                        itemRolled = weapons1hMasterwork[Random.Range(0, weapons1hMasterwork.Length)];
                        break;
                    default:
                        break;
                }
                break;
            case Item.ItemType.TwoHandWeapon:
                switch (itemRarity)
                {
                    case Item.ItemRarity.Common:
                        itemRolled = weapons2hCommon[Random.Range(0, weapons2hCommon.Length)];
                        break;
                    case Item.ItemRarity.Uncommon:
                        itemRolled = weapons2hUncommon[Random.Range(0, weapons2hUncommon.Length)];
                        break;
                    case Item.ItemRarity.Rare:
                        itemRolled = weapons2hRare[Random.Range(0, weapons2hRare.Length)];
                        break;
                    case Item.ItemRarity.Legendary:
                        itemRolled = weapons2hLegendary[Random.Range(0, weapons2hLegendary.Length)];
                        break;
                    case Item.ItemRarity.Masterwork:
                        itemRolled = weapons2hMasterwork[Random.Range(0, weapons2hMasterwork.Length)];
                        break;
                    default:
                        break;
                }
                break;
            case Item.ItemType.Helmet:
                switch (itemRarity)
                {
                    case Item.ItemRarity.Common:
                        itemRolled = helmetsCommon[Random.Range(0, helmetsCommon.Length)];
                        break;
                    case Item.ItemRarity.Uncommon:
                        itemRolled = helmetsUncommon[Random.Range(0, helmetsUncommon.Length)];
                        break;
                    case Item.ItemRarity.Rare:
                        itemRolled = helmetsRare[Random.Range(0, helmetsRare.Length)];
                        break;
                    case Item.ItemRarity.Legendary:
                        itemRolled = helmetsLegendary[Random.Range(0, helmetsLegendary.Length)];
                        break;
                    case Item.ItemRarity.Masterwork:
                        itemRolled = helmetsMasterwork[Random.Range(0, helmetsMasterwork.Length)];
                        break;
                    default:
                        break;
                }
                break;
            case Item.ItemType.Legs:
                switch (itemRarity)
                {
                    case Item.ItemRarity.Common:
                        itemRolled = legArmorCommon[Random.Range(0, legArmorCommon.Length)];
                        break;
                    case Item.ItemRarity.Uncommon:
                        itemRolled = legArmorUncommon[Random.Range(0, legArmorUncommon.Length)];
                        break;
                    case Item.ItemRarity.Rare:
                        itemRolled = legArmorRare[Random.Range(0, legArmorRare.Length)];
                        break;
                    case Item.ItemRarity.Legendary:
                        itemRolled = legArmorLegendary[Random.Range(0, legArmorLegendary.Length)];
                        break;
                    case Item.ItemRarity.Masterwork:
                        itemRolled = legArmorMasterwork[Random.Range(0, legArmorMasterwork.Length)];
                        break;
                    default:
                        break;
                }
                break;
            case Item.ItemType.Armor:
                switch (itemRarity)
                {
                    case Item.ItemRarity.Common:
                        itemRolled = chestArmorCommon[Random.Range(0, chestArmorCommon.Length)];
                        break;
                    case Item.ItemRarity.Uncommon:
                        itemRolled = chestArmorUncommon[Random.Range(0, chestArmorUncommon.Length)];
                        break;
                    case Item.ItemRarity.Rare:
                        itemRolled = chestArmorRare[Random.Range(0, chestArmorRare.Length)];
                        break;
                    case Item.ItemRarity.Legendary:
                        itemRolled = chestArmorLegendary[Random.Range(0, chestArmorLegendary.Length)];
                        break;
                    case Item.ItemRarity.Masterwork:
                        itemRolled = chestArmorMasterwork[Random.Range(0, chestArmorMasterwork.Length)];
                        break;
                    default:
                        break;
                }
                break;
            case Item.ItemType.Skill:
                switch (itemRarity)
                {
                    case Item.ItemRarity.Common:
                        itemRolled = skillsCommon[Random.Range(0, skillsCommon.Length)];
                        break;
                    case Item.ItemRarity.Uncommon:
                        itemRolled = skillsUncommon[Random.Range(0, skillsUncommon.Length)];
                        break;
                    case Item.ItemRarity.Rare:
                        itemRolled = skillsRare[Random.Range(0, skillsRare.Length)];
                        break;
                    case Item.ItemRarity.Legendary:
                        itemRolled = skillsLegendary[Random.Range(0, skillsLegendary.Length)];
                        break;
                    case Item.ItemRarity.Masterwork:
                        itemRolled = skillsMasterwork[Random.Range(0, skillsMasterwork.Length)];
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }

        //Debug.Log("the item rolled was: " + itemRolled);
        return itemRolled;
    }
}
