using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is used as a method way to add certain traits to items. 
public class ItemTrait
{
    public enum TraitType { HealthFlat, HealthPercent, HealthRegen, HealingOnHit, HealingOnKill, Armor, CooldownReduction, AflameResistance, FrostbiteResistance, WindshearResistance, SunderResistance, OverchargeResistance, OvergrowthResistance, AsleepResistance, StunResistance,
        BleedResistance, PoisonResistance, KnockbackResistance, AttackSpeed, MoveSpeed, CritChance, CritDamage, Jumps, FlatDamageReduction, FireExplosionOnKill, MoreAflameStacksOnHitThreshold, BurnDoesMaxHpDamageAtThreshold, BasicAttacksShredArmorOnAflame, FlameVamperism, RingOfFireOnHit, AflameToSunderStackOnEarthSpell,
    SunderFurtherDecreasesFireResist, AflameSunderCritsSummonFireballs, AflameWindshearWindAttacksGainCritOnBurningTarget, AflameWindshearSummonFirePillarsOnHit, AflameWindshearWindSpellsAddFireStacks, AflamePhysicalAddFireStacksOnHit,  AflamePhysicalDamageAmpOnBurningTarget, AflamePhysicalBladeExplosionOnKill, AflamePhysicalBigHitsAddAflame,
    AflameBleedIncreasesFlameCritChance, AflameBleedFireDamageAmpOnBleedThreshold, AflameBleedAflameAddsBleedAtThreshhold, AflameBleedAflameRemovesBleedResist, AflameBleedDamageAmpOnDoubleThreshhold, AflamePoisonBurningEnemySpreadPoisonStacksOnDeath, AflamePoisonGreviousWoundsOnStackThreshold, AflamePoisonPoisonReducesFireResist, AflamePoisonFireSpellsSummonsPoisonBurst,
    AflamePoisonFireAmpsPoison, AflamePoisonPoisonCloudOnFireKill, AflameStunPeriodBurnStun};
    public TraitType traitType;
    public int traitBonusMultiplier = 1;
    public float traitBonus;

    // This is the constrructor for the new trait for the item.
    public ItemTrait(TraitType chosenTrait, float value, int multiplier)
    {
        traitType = chosenTrait;
        traitBonus = value;
        traitBonusMultiplier = multiplier;
    }

    public ItemTrait()
    {
    }

    // Used to grab a random trait from the bin of availible traits.
    public void GetRandomTrait()
    {
        int randomTrait = Random.Range(0, 42);
        switch (randomTrait)
        {
            case 0:
                traitType = TraitType.HealthFlat;
                traitBonus = 50;
                break;
            case 1:
                traitType = TraitType.HealthPercent;
                traitBonus = 0.08f;
                break;
            case 2:
                traitType = TraitType.HealthRegen;
                traitBonus = 2;
                break;
            case 3:
                traitType = TraitType.HealingOnHit;
                traitBonus = 1;
                break;
            case 4:
                traitType = TraitType.HealingOnKill;
                traitBonus = 10;
                break;
            case 5:
                traitType = TraitType.Armor;
                traitBonus = 40;
                break;
            case 6:
                traitType = TraitType.CooldownReduction;
                traitBonus = 0.2f;
                break;
            case 7:
                traitType = TraitType.AttackSpeed;
                traitBonus = 0.15f;
                break;
            case 8:
                traitType = TraitType.MoveSpeed;
                traitBonus = 0.08f;
                break;
            case 9:
                traitType = TraitType.CritChance;
                traitBonus = 0.1f;
                break;
            case 10:
                traitType = TraitType.CritDamage;
                traitBonus = 0.1f;
                break;
            case 11:
                traitType = TraitType.Jumps;
                traitBonus = 1;
                break;
            case 16:
                traitType = TraitType.FlatDamageReduction;
                traitBonus = 3;
                break;
            case 17:
                traitType = TraitType.MoreAflameStacksOnHitThreshold;
                traitBonus = 3;
                break;
            case 18:
                traitType = TraitType.BasicAttacksShredArmorOnAflame;
                traitBonus = 3;
                break;
            case 19:
                traitType = TraitType.FlameVamperism;
                traitBonus = 1;
                break;
            case 20:
                traitType = TraitType.RingOfFireOnHit;
                traitBonus = 0.5f;
                break;
            case 21:
                traitType = TraitType.AflameToSunderStackOnEarthSpell;
                traitBonus = 0.1f;
                break;
            case 22:
                traitType = TraitType.SunderFurtherDecreasesFireResist;
                traitBonus = 1f;
                break;
            case 23:
                traitType = TraitType.AflameSunderCritsSummonFireballs;
                traitBonus = 1f;
                break;
            case 24:
                traitType = TraitType.AflameWindshearWindAttacksGainCritOnBurningTarget;
                traitBonus = 0.02f;
                break;
            case 25:
                traitType = TraitType.AflameWindshearSummonFirePillarsOnHit;
                traitBonus = 1f;
                break;
            case 26:
                traitType = TraitType.AflameWindshearWindSpellsAddFireStacks;
                traitBonus = 0.2f;
                break;
            case 27:
                traitType = TraitType.AflamePhysicalAddFireStacksOnHit;
                traitBonus = 1f;
                break;
            case 28:
                traitType = TraitType.AflamePhysicalDamageAmpOnBurningTarget;
                traitBonus = 0.05f;
                break;
            case 29:
                traitType = TraitType.AflamePhysicalBladeExplosionOnKill;
                traitBonus = 1f;
                break;
            case 30:
                traitType = TraitType.AflamePhysicalBigHitsAddAflame;
                traitBonus = 0.5f;
                break;
            case 31:
                traitType = TraitType.AflameBleedIncreasesFlameCritChance;
                traitBonus = 0.03f;
                break;
            case 32:
                traitType = TraitType.AflameBleedFireDamageAmpOnBleedThreshold;
                traitBonus = 0.1f;
                break;
            case 33:
                traitType = TraitType.AflameBleedAflameAddsBleedAtThreshhold;
                traitBonus = 2f;
                break;
            case 34:
                traitType = TraitType.AflameBleedAflameRemovesBleedResist;
                traitBonus = -0.01f;
                break;
            case 35:
                traitType = TraitType.AflameBleedDamageAmpOnDoubleThreshhold;
                traitBonus = 0.5f;
                break;
            case 36:
                traitType = TraitType.AflamePoisonBurningEnemySpreadPoisonStacksOnDeath;
                traitBonus = 0.5f;
                break;
            case 37:
                traitType = TraitType.AflamePoisonGreviousWoundsOnStackThreshold;
                traitBonus = 4;
                break;
            case 38:
                traitType = TraitType.AflamePoisonPoisonReducesFireResist;
                traitBonus = 0.01f;
                break;
            case 39:
                traitType = TraitType.AflamePoisonFireSpellsSummonsPoisonBurst;
                traitBonus = 5f;
                break;
            case 40:
                traitType = TraitType.AflamePoisonFireAmpsPoison;
                traitBonus = 0.01f;
                break;
            case 41:
                traitType = TraitType.AflamePoisonPoisonCloudOnFireKill;
                traitBonus = 0.1f;
                break;
            case 42:
                traitType = TraitType.AflameStunPeriodBurnStun;
                traitBonus = 1f;
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
                    traitBonus = 0.2f;
                    break;
                case 1:
                    traitType = TraitType.FrostbiteResistance;
                    traitBonus = 0.2f;
                    break;
                case 2:
                    traitType = TraitType.WindshearResistance;
                    traitBonus = 0.2f;
                    break;
                case 3:
                    traitType = TraitType.SunderResistance;
                    traitBonus = 0.2f;
                    break;
                case 4:
                    traitType = TraitType.OverchargeResistance;
                    traitBonus = 0.2f;
                    break;
                case 5:
                    traitType = TraitType.OvergrowthResistance;
                    traitBonus = 0.2f;
                    break;
                case 6:
                    traitType = TraitType.BleedResistance;
                    traitBonus = 0.2f;
                    break;
                case 7:
                    traitType = TraitType.PoisonResistance;
                    traitBonus = 0.2f;
                    break;
                case 8:
                    traitType = TraitType.AsleepResistance;
                    traitBonus = 0.2f;
                    break;
                case 9:
                    traitType = TraitType.StunResistance;
                    traitBonus = 0.2f;
                    break;
                case 10:
                    traitType = TraitType.KnockbackResistance;
                    traitBonus = 0.2f;
                    break;
                default:
                    break;
            }
        }
        
    }

    /*
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
            case TraitType.FireExplosionOnKill:
                switch (rarity)
                {
                    case Item.ItemRarity.Common:
                        traitBonus = Random.Range(5, 10);
                        break;
                    case Item.ItemRarity.Uncommon:
                        traitBonus = Random.Range(10, 15);
                        break;
                    case Item.ItemRarity.Rare:
                        traitBonus = Random.Range(15, 20);
                        break;
                    case Item.ItemRarity.Legendary:
                        traitBonus = Random.Range(20, 25);
                        break;
                    case Item.ItemRarity.Masterwork:
                        traitBonus = Random.Range(25, 30);
                        break;
                    default:
                        break;
                }
                break;
            case TraitType.MoreAflameStacksOnHitThreshold:
                switch (rarity)
                {
                    case Item.ItemRarity.Common:
                        traitBonus = Random.Range(3, 6);
                        break;
                    case Item.ItemRarity.Uncommon:
                        traitBonus = Random.Range(6, 9);
                        break;
                    case Item.ItemRarity.Rare:
                        traitBonus = Random.Range(9, 12);
                        break;
                    case Item.ItemRarity.Legendary:
                        traitBonus = Random.Range(12, 15);
                        break;
                    case Item.ItemRarity.Masterwork:
                        traitBonus = Random.Range(15, 18);
                        break;
                    default:
                        break;
                }
                break;
            case TraitType.BurnDoesMaxHpDamageAtThreshold:
                switch (rarity)
                {
                    case Item.ItemRarity.Common:
                        traitBonus = Random.Range(0.5f, 0.8f);
                        break;
                    case Item.ItemRarity.Uncommon:
                        traitBonus = Random.Range(0.8f, 1.2f);
                        break;
                    case Item.ItemRarity.Rare:
                        traitBonus = Random.Range(1.2f, 1.7f);
                        break;
                    case Item.ItemRarity.Legendary:
                        traitBonus = Random.Range(1.7f, 2.2f);
                        break;
                    case Item.ItemRarity.Masterwork:
                        traitBonus = Random.Range(2.2f, 3f);
                        break;
                    default:
                        break;
                }
                break;
            case TraitType.BasicAttacksShredArmorOnAflame:
                switch (rarity)
                {
                    case Item.ItemRarity.Common:
                        traitBonus = Random.Range(2, 5);
                        break;
                    case Item.ItemRarity.Uncommon:
                        traitBonus = Random.Range(4, 7);
                        break;
                    case Item.ItemRarity.Rare:
                        traitBonus = Random.Range(6, 9);
                        break;
                    case Item.ItemRarity.Legendary:
                        traitBonus = Random.Range(8, 11);
                        break;
                    case Item.ItemRarity.Masterwork:
                        traitBonus = Random.Range(10, 15);
                        break;
                    default:
                        break;
                }
                break;
            case TraitType.FlameVamperism:
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
            case TraitType.RingOfFireOnHit:
                switch (rarity)
                {
                    case Item.ItemRarity.Common:
                        traitBonus = (int)Random.Range(0.1f, 0.2f);
                        break;
                    case Item.ItemRarity.Uncommon:
                        traitBonus = (int)Random.Range(0.2f, 0.6f);
                        break;
                    case Item.ItemRarity.Rare:
                        traitBonus = (int)Random.Range(0.6f, 1.2f);
                        break;
                    case Item.ItemRarity.Legendary:
                        traitBonus = (int)Random.Range(1.2f, 1.75f);
                        break;
                    case Item.ItemRarity.Masterwork:
                        traitBonus = (int)Random.Range(1.75f, 2f);
                        break;
                    default:
                        break;
                }
                break;

            default:
                break;
        }
    }
    */
}
