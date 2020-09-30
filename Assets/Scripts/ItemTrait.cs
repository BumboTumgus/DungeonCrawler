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
}
