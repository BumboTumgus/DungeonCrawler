using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
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

    public GameObject[] trinketsCommon;
    public GameObject[] trinketsUncommon;
    public GameObject[] trinketsRare;
    public GameObject[] trinketsLegendary;
    public GameObject[] trinketsMasterwork;

    public GameObject[] consumablesCommon;
    public GameObject[] consumablesUncommon;
    public GameObject[] consumablesRare;
    public GameObject[] consumablesLegendary;
    public GameObject[] consumablesMasterwork;

    public GameObject[] treasures;

    public float rarityLevelMod = 0;

    private const float weapon1hRC = 20;
    private const float weapon2hRC = 20;
    private const float chestArmorRC = 10;
    private const float legArmorRC = 10;
    private const float helmetsRC = 10;
    private const float trinketsRC = 10;
    private const float consumablesRC = 10;

    private const float commonRC = 40;
    private const float uncommonRC = 30;
    private const float rareRC = 30;
    private const float legendaryRC = 35;
    private const float masterworkRC = 5;
    // by the 11th level, we cap it so the player only recieves legendary at 87.5% percent and masterwork at 12.5%;

    public GameObject RollItem()
    {
        GameObject itemRolled = null;

        float itemDiceRoll = Random.Range(0, weapon1hRC + weapon2hRC + chestArmorRC + legArmorRC + helmetsRC + trinketsRC + consumablesRC);
        Item.ItemType itemType = Item.ItemType.Consumable;
        float itemRarityDiceRoll = Random.Range(0, 160) + rarityLevelMod;
        Item.ItemRarity itemRarity = Item.ItemRarity.Common;
        
        // Rolls the item type and compares its value in the table below.
        if (itemDiceRoll <= weapon1hRC)
            itemType = Item.ItemType.Weapon;
        else if (itemDiceRoll <= weapon1hRC + weapon2hRC)
            itemType = Item.ItemType.TwoHandWeapon;
        else if (itemDiceRoll <= weapon1hRC + weapon2hRC + chestArmorRC)
            itemType = Item.ItemType.Armor;
        else if (itemDiceRoll <= weapon1hRC + weapon2hRC + chestArmorRC + legArmorRC)
            itemType = Item.ItemType.Legs;
        else if (itemDiceRoll <= weapon1hRC + weapon2hRC + chestArmorRC + legArmorRC + helmetsRC)
            itemType = Item.ItemType.Helmet;
        else if (itemDiceRoll <= weapon1hRC + weapon2hRC + chestArmorRC + legArmorRC + helmetsRC + trinketsRC)
            itemType = Item.ItemType.Trinket;
        else if (itemDiceRoll <= weapon1hRC + weapon2hRC + chestArmorRC + legArmorRC + helmetsRC + trinketsRC + consumablesRC)
            itemType = Item.ItemType.Consumable;

        if (itemRarityDiceRoll <= commonRC)
            itemRarity = Item.ItemRarity.Common;
        else if (itemRarityDiceRoll <= commonRC + uncommonRC)
            itemRarity = Item.ItemRarity.Uncommon;
        else if (itemRarityDiceRoll <= commonRC + uncommonRC + rareRC)
            itemRarity = Item.ItemRarity.Rare;
        else if (itemRarityDiceRoll <= commonRC + uncommonRC + rareRC + legendaryRC)
            itemRarity = Item.ItemRarity.Legendary;
        else if (itemRarityDiceRoll <= commonRC + uncommonRC + rareRC + legendaryRC + masterworkRC)
            itemRarity = Item.ItemRarity.Masterwork;
        
        // This assigns us a bank of items for us to use.
        switch (itemType)
        {
            case Item.ItemType.Consumable:
                switch (itemRarity)
                {
                    case Item.ItemRarity.Common:
                        itemRolled = consumablesCommon[Random.Range(0, consumablesCommon.Length)];
                        break;
                    case Item.ItemRarity.Uncommon:
                        itemRolled = consumablesUncommon[Random.Range(0, consumablesUncommon.Length)];
                        break;
                    case Item.ItemRarity.Rare:
                        itemRolled = consumablesRare[Random.Range(0, consumablesRare.Length)];
                        break;
                    case Item.ItemRarity.Legendary:
                        itemRolled = consumablesLegendary[Random.Range(0, consumablesLegendary.Length)];
                        break;
                    case Item.ItemRarity.Masterwork:
                        itemRolled = consumablesMasterwork[Random.Range(0, consumablesMasterwork.Length)];
                        break;
                    default:
                        break;
                }
                break;
            case Item.ItemType.Trinket:
                switch (itemRarity)
                {
                    case Item.ItemRarity.Common:
                        itemRolled = trinketsCommon[Random.Range(0, trinketsCommon.Length)];
                        break;
                    case Item.ItemRarity.Uncommon:
                        itemRolled = trinketsUncommon[Random.Range(0, trinketsUncommon.Length)];
                        break;
                    case Item.ItemRarity.Rare:
                        itemRolled = trinketsRare[Random.Range(0, trinketsRare.Length)];
                        break;
                    case Item.ItemRarity.Legendary:
                        itemRolled = trinketsLegendary[Random.Range(0, trinketsLegendary.Length)];
                        break;
                    case Item.ItemRarity.Masterwork:
                        itemRolled = trinketsMasterwork[Random.Range(0, trinketsMasterwork.Length)];
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
            default:
                break;
        }

        //Debug.Log("the item rolled was: " + itemRolled);
        return itemRolled;
    }


    // Used to roll a piece of treasure
    public GameObject RollTreasure()
    {
        GameObject treasureRolled = treasures[Random.Range(0, treasures.Length)];
        return treasureRolled;
    }
}
