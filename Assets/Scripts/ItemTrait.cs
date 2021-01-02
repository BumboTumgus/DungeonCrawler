using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is used as a method way to add certain traits to items. 
public class ItemTrait
{
    public enum TraitType { Vit, Str, Dex, Spd, Int, Wis, Health, Mana, Armor, Resistance, HealthRegen, ManaRegen, CooldownReduction, SpellSlots, AflameResistance, AsleepResistance, StunResistance, CurseResistance, BleedResistance, PoisonResistance, CorrosionResistance,
        FrostbiteResistance, KnockbackResistance, AttackSpeed};
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
        int randomTrait = Random.Range(0, 24);
        switch (randomTrait)
        {
            case 0:
                traitType = TraitType.Vit;
                break;
            case 1:
                traitType = TraitType.Str;
                break;
            case 2:
                traitType = TraitType.Dex;
                break;
            case 3:
                traitType = TraitType.Spd;
                break;
            case 4:
                traitType = TraitType.Int;
                break;
            case 5:
                traitType = TraitType.Wis;
                break;
            case 6:
                traitType = TraitType.Health;
                break;
            case 7:
                traitType = TraitType.Mana;
                break;
            case 8:
                traitType = TraitType.Armor;
                break;
            case 9:
                traitType = TraitType.Resistance;
                break;
            case 10:
                traitType = TraitType.HealthRegen;
                break;
            case 11:
                traitType = TraitType.ManaRegen;
                break;
            case 12:
                traitType = TraitType.CooldownReduction;
                break;
            case 13:
                int randomResistanceIndex = Random.Range(0, 9);
                switch (randomResistanceIndex)
                {
                    case 0:
                        traitType = TraitType.AflameResistance;
                        break;
                    case 1:
                        traitType = TraitType.AsleepResistance;
                        break;
                    case 2:
                        traitType = TraitType.StunResistance;
                        break;
                    case 3:
                        traitType = TraitType.CurseResistance;
                        break;
                    case 4:
                        traitType = TraitType.BleedResistance;
                        break;
                    case 5:
                        traitType = TraitType.PoisonResistance;
                        break;
                    case 6:
                        traitType = TraitType.CorrosionResistance;
                        break;
                    case 7:
                        traitType = TraitType.KnockbackResistance;
                        break;
                    case 8:
                        traitType = TraitType.FrostbiteResistance;
                        break;
                    default:
                        break;
                }
                traitType = TraitType.SpellSlots;
                break;
            case 14:
                traitType = TraitType.AttackSpeed;
                break;
            default:
                break;
        }
        
    }

    // USed to grab a random value for the trait
    public void GetRandomTraitValue(Item.ItemRarity rarity)
    {
        switch (traitType)
        {
            case TraitType.Vit:
                switch (rarity)
                {
                    case Item.ItemRarity.Common:
                        traitBonus = (int) Random.Range(1, 4);
                        break;
                    case Item.ItemRarity.Uncommon:
                        traitBonus = (int)Random.Range(2, 5);
                        break;
                    case Item.ItemRarity.Rare:
                        traitBonus = (int)Random.Range(3, 6);
                        break;
                    case Item.ItemRarity.Legendary:
                        traitBonus = (int)Random.Range(5, 8);
                        break;
                    case Item.ItemRarity.Masterwork:
                        traitBonus = (int)Random.Range(8, 11);
                        break;
                    default:
                        break;
                }
                break;
            case TraitType.Str:
                switch (rarity)
                {
                    case Item.ItemRarity.Common:
                        traitBonus = (int)Random.Range(1, 4);
                        break;
                    case Item.ItemRarity.Uncommon:
                        traitBonus = (int)Random.Range(2, 5);
                        break;
                    case Item.ItemRarity.Rare:
                        traitBonus = (int)Random.Range(3, 6);
                        break;
                    case Item.ItemRarity.Legendary:
                        traitBonus = (int)Random.Range(5, 8);
                        break;
                    case Item.ItemRarity.Masterwork:
                        traitBonus = (int)Random.Range(8, 11);
                        break;
                    default:
                        break;
                }
                break;
            case TraitType.Dex:
                switch (rarity)
                {
                    case Item.ItemRarity.Common:
                        traitBonus = (int)Random.Range(1, 4);
                        break;
                    case Item.ItemRarity.Uncommon:
                        traitBonus = (int)Random.Range(2, 5);
                        break;
                    case Item.ItemRarity.Rare:
                        traitBonus = (int)Random.Range(3, 6);
                        break;
                    case Item.ItemRarity.Legendary:
                        traitBonus = (int)Random.Range(5, 8);
                        break;
                    case Item.ItemRarity.Masterwork:
                        traitBonus = (int)Random.Range(8, 11);
                        break;
                    default:
                        break;
                }
                break;
            case TraitType.Spd:
                switch (rarity)
                {
                    case Item.ItemRarity.Common:
                        traitBonus = (int)Random.Range(1, 4);
                        break;
                    case Item.ItemRarity.Uncommon:
                        traitBonus = (int)Random.Range(2, 5);
                        break;
                    case Item.ItemRarity.Rare:
                        traitBonus = (int)Random.Range(3, 6);
                        break;
                    case Item.ItemRarity.Legendary:
                        traitBonus = (int)Random.Range(5, 8);
                        break;
                    case Item.ItemRarity.Masterwork:
                        traitBonus = (int)Random.Range(8, 11);
                        break;
                    default:
                        break;
                }
                break;
            case TraitType.Int:
                switch (rarity)
                {
                    case Item.ItemRarity.Common:
                        traitBonus = (int)Random.Range(1, 4);
                        break;
                    case Item.ItemRarity.Uncommon:
                        traitBonus = (int)Random.Range(2, 5);
                        break;
                    case Item.ItemRarity.Rare:
                        traitBonus = (int)Random.Range(3, 6);
                        break;
                    case Item.ItemRarity.Legendary:
                        traitBonus = (int)Random.Range(5, 8);
                        break;
                    case Item.ItemRarity.Masterwork:
                        traitBonus = (int)Random.Range(8, 11);
                        break;
                    default:
                        break;
                }
                break;
            case TraitType.Wis:
                switch (rarity)
                {
                    case Item.ItemRarity.Common:
                        traitBonus = (int)Random.Range(1, 4);
                        break;
                    case Item.ItemRarity.Uncommon:
                        traitBonus = (int)Random.Range(2, 5);
                        break;
                    case Item.ItemRarity.Rare:
                        traitBonus = (int)Random.Range(3, 6);
                        break;
                    case Item.ItemRarity.Legendary:
                        traitBonus = (int)Random.Range(5, 8);
                        break;
                    case Item.ItemRarity.Masterwork:
                        traitBonus = (int)Random.Range(8, 11);
                        break;
                    default:
                        break;
                }
                break;
            case TraitType.Health:
                switch (rarity)
                {
                    case Item.ItemRarity.Common:
                        traitBonus = (int)Random.Range(10, 31);
                        break;
                    case Item.ItemRarity.Uncommon:
                        traitBonus = (int)Random.Range(20, 41);
                        break;
                    case Item.ItemRarity.Rare:
                        traitBonus = (int)Random.Range(30, 61);
                        break;
                    case Item.ItemRarity.Legendary:
                        traitBonus = (int)Random.Range(50, 81);
                        break;
                    case Item.ItemRarity.Masterwork:
                        traitBonus = (int)Random.Range(80, 111);
                        break;
                    default:
                        break;
                }
                break;
            case TraitType.Mana:
                switch (rarity)
                {
                    case Item.ItemRarity.Common:
                        traitBonus = (int)Random.Range(10, 31);
                        break;
                    case Item.ItemRarity.Uncommon:
                        traitBonus = (int)Random.Range(20, 41);
                        break;
                    case Item.ItemRarity.Rare:
                        traitBonus = (int)Random.Range(30, 61);
                        break;
                    case Item.ItemRarity.Legendary:
                        traitBonus = (int)Random.Range(50, 81);
                        break;
                    case Item.ItemRarity.Masterwork:
                        traitBonus = (int)Random.Range(80, 111);
                        break;
                    default:
                        break;
                }
                break;
            case TraitType.Armor:
                switch (rarity)
                {
                    case Item.ItemRarity.Common:
                        traitBonus = (int)Random.Range(2, 8);
                        break;
                    case Item.ItemRarity.Uncommon:
                        traitBonus = (int)Random.Range(5, 10);
                        break;
                    case Item.ItemRarity.Rare:
                        traitBonus = (int)Random.Range(8, 16);
                        break;
                    case Item.ItemRarity.Legendary:
                        traitBonus = (int)Random.Range(12, 21);
                        break;
                    case Item.ItemRarity.Masterwork:
                        traitBonus = (int)Random.Range(20, 61);
                        break;
                    default:
                        break;
                }
                break;
            case TraitType.Resistance:
                switch (rarity)
                {
                    case Item.ItemRarity.Common:
                        traitBonus = (int)Random.Range(2, 8);
                        break;
                    case Item.ItemRarity.Uncommon:
                        traitBonus = (int)Random.Range(5, 10);
                        break;
                    case Item.ItemRarity.Rare:
                        traitBonus = (int)Random.Range(8, 16);
                        break;
                    case Item.ItemRarity.Legendary:
                        traitBonus = (int)Random.Range(12, 21);
                        break;
                    case Item.ItemRarity.Masterwork:
                        traitBonus = (int)Random.Range(20, 61);
                        break;
                    default:
                        break;
                }
                break;
            case TraitType.HealthRegen:
                switch (rarity)
                {
                    case Item.ItemRarity.Common:
                        traitBonus = Random.Range(0.2f, 0.8f);
                        break;
                    case Item.ItemRarity.Uncommon:
                        traitBonus = Random.Range(0.4f, 1.4f);
                        break;
                    case Item.ItemRarity.Rare:
                        traitBonus = Random.Range(1.3f, 2.2f);
                        break;
                    case Item.ItemRarity.Legendary:
                        traitBonus = Random.Range(2f, 3.8f);
                        break;
                    case Item.ItemRarity.Masterwork:
                        traitBonus = Random.Range(4f, 8f);
                        break;
                    default:
                        break;
                }
                break;
            case TraitType.ManaRegen:
                switch (rarity)
                {
                    case Item.ItemRarity.Common:
                        traitBonus = Random.Range(0.2f, 0.8f);
                        break;
                    case Item.ItemRarity.Uncommon:
                        traitBonus = Random.Range(0.4f, 1.4f);
                        break;
                    case Item.ItemRarity.Rare:
                        traitBonus = Random.Range(1.3f, 2.2f);
                        break;
                    case Item.ItemRarity.Legendary:
                        traitBonus = Random.Range(2f, 3.8f);
                        break;
                    case Item.ItemRarity.Masterwork:
                        traitBonus = Random.Range(4f, 8f);
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
            case TraitType.SpellSlots:
                switch (rarity)
                {
                    case Item.ItemRarity.Common:
                        traitBonus = 1;
                        break;
                    case Item.ItemRarity.Uncommon:
                        traitBonus = (int) Random.Range(1, 3);
                        break;
                    case Item.ItemRarity.Rare:
                        traitBonus = (int)Random.Range(1, 3);
                        break;
                    case Item.ItemRarity.Legendary:
                        traitBonus = (int)Random.Range(2, 4);
                        break;
                    case Item.ItemRarity.Masterwork:
                        traitBonus = (int)Random.Range(3, 5);
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
            case TraitType.CurseResistance:
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
            case TraitType.CorrosionResistance:
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
            case TraitType.AttackSpeed:
                switch (rarity)
                {
                    case Item.ItemRarity.Common:
                        traitBonus = Random.Range(0.05f, 0.20f);
                        break;
                    case Item.ItemRarity.Uncommon:
                        traitBonus = Random.Range(0.10f, 0.30f);
                        break;
                    case Item.ItemRarity.Rare:
                        traitBonus = Random.Range(0.15f, 0.40f);
                        break;
                    case Item.ItemRarity.Legendary:
                        traitBonus = Random.Range(0.20f, 0.50f);
                        break;
                    case Item.ItemRarity.Masterwork:
                        traitBonus = Random.Range(0.40f, 0.90f);
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
