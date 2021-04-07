using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is used as a method way to add certain traits to items. 
public class ItemTrait
{
    public enum TraitType { HealthFlat, HealthPercent, HealthRegen, HealingOnHit, HealingOnKill, Armor, CooldownReduction, AflameResistance, FrostbiteResistance, WindshearResistance, SunderResistance, OverchargeResistance, OvergrowthResistance, AsleepResistance, StunResistance,
        BleedResistance, PoisonResistance, KnockbackResistance, AttackSpeed, MoveSpeed, CritChance, CritDamage, Jumps, FlatDamageReduction};
    public TraitType traitType;
    public float traitBonus;

    // This is the constrructor for the new trait for the item.
    public ItemTrait(TraitType chosenTrait, float value)
    {
        traitType = chosenTrait;
        traitBonus = value;
    }

    public ItemTrait()
    {
    }

    // Used to grab a random trait from the bin of availible traits.
    public void GetRandomTrait()
    {
        int randomTrait = Random.Range(0, 17);
        switch (randomTrait)
        {
            case 0:
                traitType = TraitType.HealthFlat;
                break;
            case 1:
                traitType = TraitType.HealthPercent;
                break;
            case 2:
                traitType = TraitType.HealthRegen;
                break;
            case 3:
                traitType = TraitType.HealingOnHit;
                break;
            case 4:
                traitType = TraitType.HealingOnKill;
                break;
            case 5:
                traitType = TraitType.Armor;
                break;
            case 6:
                traitType = TraitType.CooldownReduction;
                break;
            case 7:
                traitType = TraitType.AttackSpeed;
                break;
            case 8:
                traitType = TraitType.MoveSpeed;
                break;
            case 9:
                traitType = TraitType.CritChance;
                break;
            case 10:
                traitType = TraitType.CritDamage;
                break;
            case 11:
                traitType = TraitType.Jumps;
                break;
            case 16:
                traitType = TraitType.FlatDamageReduction;
                break;
            default:
                break;
        }

        if(randomTrait >= 12 && randomTrait <= 16)
        {
            int randomResistanceIndex = Random.Range(0, 11);
            switch (randomResistanceIndex)
            {
                case 0:
                    traitType = TraitType.AflameResistance;
                    break;
                case 1:
                    traitType = TraitType.FrostbiteResistance;
                    break;
                case 2:
                    traitType = TraitType.WindshearResistance;
                    break;
                case 3:
                    traitType = TraitType.SunderResistance;
                    break;
                case 4:
                    traitType = TraitType.OverchargeResistance;
                    break;
                case 5:
                    traitType = TraitType.OvergrowthResistance;
                    break;
                case 6:
                    traitType = TraitType.BleedResistance;
                    break;
                case 7:
                    traitType = TraitType.PoisonResistance;
                    break;
                case 8:
                    traitType = TraitType.AsleepResistance;
                    break;
                case 9:
                    traitType = TraitType.StunResistance;
                    break;
                case 10:
                    traitType = TraitType.KnockbackResistance;
                    break;
                default:
                    break;
            }
        }
        
    }

    // USed to grab a random value for the trait
    public void GetRandomTraitValue(Item.ItemRarity rarity)
    {
        switch (traitType)
        {
            case TraitType.HealthFlat:
                switch (rarity)
                {
                    case Item.ItemRarity.Common:
                        traitBonus = (int)Random.Range(20, 40);
                        break;
                    case Item.ItemRarity.Uncommon:
                        traitBonus = (int)Random.Range(40, 80);
                        break;
                    case Item.ItemRarity.Rare:
                        traitBonus = (int)Random.Range(80, 160);
                        break;
                    case Item.ItemRarity.Legendary:
                        traitBonus = (int)Random.Range(160, 320);
                        break;
                    case Item.ItemRarity.Masterwork:
                        traitBonus = (int)Random.Range(320, 640);
                        break;
                    default:
                        break;
                }
                break;
            case TraitType.HealthPercent:
                switch (rarity)
                {
                    case Item.ItemRarity.Common:
                        traitBonus = (int)Random.Range(2, 5);
                        break;
                    case Item.ItemRarity.Uncommon:
                        traitBonus = (int)Random.Range(3, 7);
                        break;
                    case Item.ItemRarity.Rare:
                        traitBonus = (int)Random.Range(5, 13);
                        break;
                    case Item.ItemRarity.Legendary:
                        traitBonus = (int)Random.Range(10, 25);
                        break;
                    case Item.ItemRarity.Masterwork:
                        traitBonus = (int)Random.Range(20, 40);
                        break;
                    default:
                        break;
                }
                break;
            case TraitType.HealthRegen:
                switch (rarity)
                {
                    case Item.ItemRarity.Common:
                        traitBonus = Random.Range(0.2f, 1f);
                        break;
                    case Item.ItemRarity.Uncommon:
                        traitBonus = Random.Range(0.8f, 2.4f);
                        break;
                    case Item.ItemRarity.Rare:
                        traitBonus = Random.Range(2f, 10f);
                        break;
                    case Item.ItemRarity.Legendary:
                        traitBonus = Random.Range(8f, 20f);
                        break;
                    case Item.ItemRarity.Masterwork:
                        traitBonus = Random.Range(16f, 40f);
                        break;
                    default:
                        break;
                }
                break;
            case TraitType.HealingOnHit:
                switch (rarity)
                {
                    case Item.ItemRarity.Common:
                        traitBonus = (int)Random.Range(1, 2);
                        break;
                    case Item.ItemRarity.Uncommon:
                        traitBonus = (int)Random.Range(2, 3);
                        break;
                    case Item.ItemRarity.Rare:
                        traitBonus = (int)Random.Range(3, 5);
                        break;
                    case Item.ItemRarity.Legendary:
                        traitBonus = (int)Random.Range(5, 10);
                        break;
                    case Item.ItemRarity.Masterwork:
                        traitBonus = (int)Random.Range(10, 25);
                        break;
                    default:
                        break;
                }
                break;
            case TraitType.HealingOnKill:
                switch (rarity)
                {
                    case Item.ItemRarity.Common:
                        traitBonus = (int)Random.Range(10, 20);
                        break;
                    case Item.ItemRarity.Uncommon:
                        traitBonus = (int)Random.Range(15, 30);
                        break;
                    case Item.ItemRarity.Rare:
                        traitBonus = (int)Random.Range(50, 80);
                        break;
                    case Item.ItemRarity.Legendary:
                        traitBonus = (int)Random.Range(100, 200);
                        break;
                    case Item.ItemRarity.Masterwork:
                        traitBonus = (int)Random.Range(250, 500);
                        break;
                    default:
                        break;
                }
                break;
            case TraitType.Armor:
                switch (rarity)
                {
                    case Item.ItemRarity.Common:
                        traitBonus = (int)Random.Range(10, 20);
                        break;
                    case Item.ItemRarity.Uncommon:
                        traitBonus = (int)Random.Range(20, 40);
                        break;
                    case Item.ItemRarity.Rare:
                        traitBonus = (int)Random.Range(40, 80);
                        break;
                    case Item.ItemRarity.Legendary:
                        traitBonus = (int)Random.Range(60, 120);
                        break;
                    case Item.ItemRarity.Masterwork:
                        traitBonus = (int)Random.Range(100, 200);
                        break;
                    default:
                        break;
                }
                break;
            case TraitType.CooldownReduction:
                switch (rarity)
                {
                    case Item.ItemRarity.Common:
                        traitBonus = Random.Range(0.08f, 0.13f);
                        break;
                    case Item.ItemRarity.Uncommon:
                        traitBonus = Random.Range(0.11f, 0.21f);
                        break;
                    case Item.ItemRarity.Rare:
                        traitBonus = Random.Range(0.16f, 0.29f);
                        break;
                    case Item.ItemRarity.Legendary:
                        traitBonus = Random.Range(0.20f, 0.35f);
                        break;
                    case Item.ItemRarity.Masterwork:
                        traitBonus = Random.Range(0.30f, 0.45f);
                        break;
                    default:
                        break;
                }
                break;
            case TraitType.AflameResistance:
                switch (rarity)
                {
                    case Item.ItemRarity.Common:
                        traitBonus = Random.Range(10, 20);
                        break;
                    case Item.ItemRarity.Uncommon:
                        traitBonus = Random.Range(20, 35);
                        break;
                    case Item.ItemRarity.Rare:
                        traitBonus = Random.Range(30, 50);
                        break;
                    case Item.ItemRarity.Legendary:
                        traitBonus = Random.Range(40, 60);
                        break;
                    case Item.ItemRarity.Masterwork:
                        traitBonus = Random.Range(50, 100);
                        break;
                    default:
                        break;
                }
                break;
            case TraitType.FrostbiteResistance:
                switch (rarity)
                {
                    case Item.ItemRarity.Common:
                        traitBonus = Random.Range(10, 20);
                        break;
                    case Item.ItemRarity.Uncommon:
                        traitBonus = Random.Range(20, 35);
                        break;
                    case Item.ItemRarity.Rare:
                        traitBonus = Random.Range(30, 50);
                        break;
                    case Item.ItemRarity.Legendary:
                        traitBonus = Random.Range(40, 60);
                        break;
                    case Item.ItemRarity.Masterwork:
                        traitBonus = Random.Range(50, 100);
                        break;
                    default:
                        break;
                }
                break;
            case TraitType.OverchargeResistance:
                switch (rarity)
                {
                    case Item.ItemRarity.Common:
                        traitBonus = Random.Range(10, 20);
                        break;
                    case Item.ItemRarity.Uncommon:
                        traitBonus = Random.Range(20, 35);
                        break;
                    case Item.ItemRarity.Rare:
                        traitBonus = Random.Range(30, 50);
                        break;
                    case Item.ItemRarity.Legendary:
                        traitBonus = Random.Range(40, 60);
                        break;
                    case Item.ItemRarity.Masterwork:
                        traitBonus = Random.Range(50, 100);
                        break;
                    default:
                        break;
                }
                break;
            case TraitType.OvergrowthResistance:
                switch (rarity)
                {
                    case Item.ItemRarity.Common:
                        traitBonus = Random.Range(10, 20);
                        break;
                    case Item.ItemRarity.Uncommon:
                        traitBonus = Random.Range(20, 35);
                        break;
                    case Item.ItemRarity.Rare:
                        traitBonus = Random.Range(30, 50);
                        break;
                    case Item.ItemRarity.Legendary:
                        traitBonus = Random.Range(40, 60);
                        break;
                    case Item.ItemRarity.Masterwork:
                        traitBonus = Random.Range(50, 100);
                        break;
                    default:
                        break;
                }
                break;
            case TraitType.SunderResistance:
                switch (rarity)
                {
                    case Item.ItemRarity.Common:
                        traitBonus = Random.Range(10, 20);
                        break;
                    case Item.ItemRarity.Uncommon:
                        traitBonus = Random.Range(20, 35);
                        break;
                    case Item.ItemRarity.Rare:
                        traitBonus = Random.Range(30, 50);
                        break;
                    case Item.ItemRarity.Legendary:
                        traitBonus = Random.Range(40, 60);
                        break;
                    case Item.ItemRarity.Masterwork:
                        traitBonus = Random.Range(50, 100);
                        break;
                    default:
                        break;
                }
                break;
            case TraitType.WindshearResistance:
                switch (rarity)
                {
                    case Item.ItemRarity.Common:
                        traitBonus = Random.Range(10, 20);
                        break;
                    case Item.ItemRarity.Uncommon:
                        traitBonus = Random.Range(20, 35);
                        break;
                    case Item.ItemRarity.Rare:
                        traitBonus = Random.Range(30, 50);
                        break;
                    case Item.ItemRarity.Legendary:
                        traitBonus = Random.Range(40, 60);
                        break;
                    case Item.ItemRarity.Masterwork:
                        traitBonus = Random.Range(50, 100);
                        break;
                    default:
                        break;
                }
                break;
            case TraitType.BleedResistance:
                switch (rarity)
                {
                    case Item.ItemRarity.Common:
                        traitBonus = Random.Range(10, 20);
                        break;
                    case Item.ItemRarity.Uncommon:
                        traitBonus = Random.Range(20, 35);
                        break;
                    case Item.ItemRarity.Rare:
                        traitBonus = Random.Range(30, 50);
                        break;
                    case Item.ItemRarity.Legendary:
                        traitBonus = Random.Range(40, 60);
                        break;
                    case Item.ItemRarity.Masterwork:
                        traitBonus = Random.Range(50, 100);
                        break;
                    default:
                        break;
                }
                break;
            case TraitType.PoisonResistance:
                switch (rarity)
                {
                    case Item.ItemRarity.Common:
                        traitBonus = Random.Range(10, 20);
                        break;
                    case Item.ItemRarity.Uncommon:
                        traitBonus = Random.Range(20, 35);
                        break;
                    case Item.ItemRarity.Rare:
                        traitBonus = Random.Range(30, 50);
                        break;
                    case Item.ItemRarity.Legendary:
                        traitBonus = Random.Range(40, 60);
                        break;
                    case Item.ItemRarity.Masterwork:
                        traitBonus = Random.Range(50, 100);
                        break;
                    default:
                        break;
                }
                break;
            case TraitType.KnockbackResistance:
                switch (rarity)
                {
                    case Item.ItemRarity.Common:
                        traitBonus = Random.Range(10, 20);
                        break;
                    case Item.ItemRarity.Uncommon:
                        traitBonus = Random.Range(20, 35);
                        break;
                    case Item.ItemRarity.Rare:
                        traitBonus = Random.Range(30, 50);
                        break;
                    case Item.ItemRarity.Legendary:
                        traitBonus = Random.Range(40, 60);
                        break;
                    case Item.ItemRarity.Masterwork:
                        traitBonus = Random.Range(50, 100);
                        break;
                    default:
                        break;
                }
                break;
            case TraitType.AsleepResistance:
                switch (rarity)
                {
                    case Item.ItemRarity.Common:
                        traitBonus = Random.Range(10, 20);
                        break;
                    case Item.ItemRarity.Uncommon:
                        traitBonus = Random.Range(20, 35);
                        break;
                    case Item.ItemRarity.Rare:
                        traitBonus = Random.Range(30, 50);
                        break;
                    case Item.ItemRarity.Legendary:
                        traitBonus = Random.Range(40, 60);
                        break;
                    case Item.ItemRarity.Masterwork:
                        traitBonus = Random.Range(50, 100);
                        break;
                    default:
                        break;
                }
                break;
            case TraitType.StunResistance:
                switch (rarity)
                {
                    case Item.ItemRarity.Common:
                        traitBonus = Random.Range(10, 20);
                        break;
                    case Item.ItemRarity.Uncommon:
                        traitBonus = Random.Range(20, 35);
                        break;
                    case Item.ItemRarity.Rare:
                        traitBonus = Random.Range(30, 50);
                        break;
                    case Item.ItemRarity.Legendary:
                        traitBonus = Random.Range(40, 60);
                        break;
                    case Item.ItemRarity.Masterwork:
                        traitBonus = Random.Range(50, 100);
                        break;
                    default:
                        break;
                }
                break;
            case TraitType.AttackSpeed:
                switch (rarity)
                {
                    case Item.ItemRarity.Common:
                        traitBonus = Random.Range(0.05f, 0.15f);
                        break;
                    case Item.ItemRarity.Uncommon:
                        traitBonus = Random.Range(0.12f, 0.25f);
                        break;
                    case Item.ItemRarity.Rare:
                        traitBonus = Random.Range(0.20f, 0.35f);
                        break;
                    case Item.ItemRarity.Legendary:
                        traitBonus = Random.Range(0.30f, 0.50f);
                        break;
                    case Item.ItemRarity.Masterwork:
                        traitBonus = Random.Range(0.55f, 1f);
                        break;
                    default:
                        break;
                }
                break;
            case TraitType.MoveSpeed:
                switch (rarity)
                {
                    case Item.ItemRarity.Common:
                        traitBonus = Random.Range(0.05f, 0.15f);
                        break;
                    case Item.ItemRarity.Uncommon:
                        traitBonus = Random.Range(0.12f, 0.25f);
                        break;
                    case Item.ItemRarity.Rare:
                        traitBonus = Random.Range(0.20f, 0.35f);
                        break;
                    case Item.ItemRarity.Legendary:
                        traitBonus = Random.Range(0.30f, 0.50f);
                        break;
                    case Item.ItemRarity.Masterwork:
                        traitBonus = Random.Range(0.55f, 1f);
                        break;
                    default:
                        break;
                }
                break;
            case TraitType.CritChance:
                switch (rarity)
                {
                    case Item.ItemRarity.Common:
                        traitBonus = Random.Range(0.05f, 0.10f);
                        break;
                    case Item.ItemRarity.Uncommon:
                        traitBonus = Random.Range(0.8f, 0.15f);
                        break;
                    case Item.ItemRarity.Rare:
                        traitBonus = Random.Range(0.12f, 0.2f);
                        break;
                    case Item.ItemRarity.Legendary:
                        traitBonus = Random.Range(0.18f, 0.25f);
                        break;
                    case Item.ItemRarity.Masterwork:
                        traitBonus = Random.Range(0.25f, 0.35f);
                        break;
                    default:
                        break;
                }
                break;
            case TraitType.CritDamage:
                switch (rarity)
                {
                    case Item.ItemRarity.Common:
                        traitBonus = Random.Range(0.05f, 0.15f);
                        break;
                    case Item.ItemRarity.Uncommon:
                        traitBonus = Random.Range(0.12f, 0.25f);
                        break;
                    case Item.ItemRarity.Rare:
                        traitBonus = Random.Range(0.20f, 0.35f);
                        break;
                    case Item.ItemRarity.Legendary:
                        traitBonus = Random.Range(0.30f, 0.50f);
                        break;
                    case Item.ItemRarity.Masterwork:
                        traitBonus = Random.Range(0.55f, 1f);
                        break;
                    default:
                        break;
                }
                break;
            case TraitType.Jumps:
                switch (rarity)
                {
                    case Item.ItemRarity.Common:
                        traitBonus = 1;
                        break;
                    case Item.ItemRarity.Uncommon:
                        traitBonus = 1;
                        break;
                    case Item.ItemRarity.Rare:
                        traitBonus = 2;
                        break;
                    case Item.ItemRarity.Legendary:
                        traitBonus = 2;
                        break;
                    case Item.ItemRarity.Masterwork:
                        traitBonus = 3;
                        break;
                    default:
                        break;
                }
                break;
            case TraitType.FlatDamageReduction:
                switch (rarity)
                {
                    case Item.ItemRarity.Common:
                        traitBonus = Random.Range(1, 3);
                        break;
                    case Item.ItemRarity.Uncommon:
                        traitBonus = Random.Range(2, 6);
                        break;
                    case Item.ItemRarity.Rare:
                        traitBonus = Random.Range(4, 11);
                        break;
                    case Item.ItemRarity.Legendary:
                        traitBonus = Random.Range(7, 16);
                        break;
                    case Item.ItemRarity.Masterwork:
                        traitBonus = Random.Range(10, 26);
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }
    }
}
