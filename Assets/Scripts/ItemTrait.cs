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
    AflamePoisonFireAmpsPoison, AflamePoisonPoisonCloudOnFireKill, AflameStunPeriodBurnStun, AflameStunStunOnThreshold, AflameStunStunReducesFireResistance, AflameStunStunAmpsBurnDamage, AflameKnockbackAflameReducesKnockbackResist, AflameKnockbackAflameSpellsOnKnockbackedTargetExplode, AflameKnockbackKnockbackAmpsFireDamage, IceFreezeAtStackThreshold, IceAmpAllDamageAtThreshold,
    IceBasicAttacksConsumeStacksAtThreshold, IceEnemyAttacksWeakendAtThreshold, IceEnemiesGainFrostbiteOnStrikingYou, IceAddStacksToNearbyEnemies, IceAmpFrostbiteDamage, IceEarthFrostToEarthBonusDamage, IceEarthSunderAmpsIceDamage, IceEarthIceDOTAtThreshold, IceEarthEarthSpellBonusCritDamage, IceWindWindAmpsFrostbiteDamage, IceWindWindSpellsDamageAmp, IceWindIncreaseArmorShredPerFrostbite,
    IceWindSummonTornadoOnHit, IcePhysicalFrostbiteAmpsPhysicalCritDamage, IcePhysicalPhysicalVampOnFrostbite, IcePhysicalBladeVortexOnHit, IceBleedFrostbiteAmpsBleed, IceBleedBleedDoesDamageInstantlyOnThreshold, IcePoisonFreezingPoison, IcePoisonFrostbiteResetsPoisonAndAmps, IcePoisonSummonPoisonPillarOnThreshold, IceStunRudeAwakening, IceStunIceRefreshesStun, IceKnockbackFrostbiteIncreasesKnockbackForce,
    IceKnockbackSnowEruptionOnKnockback, IceKnockbackBonusStacksOnDownedTargets, EarthMaxHpDamageAtThreshold, EarthAmpAllAfflictionsOnThreshhold, EarthSunderedEnemiesDealLessDamage, EarthRockRingExplosionOnKill, EarthTrueDamageAtThreshold, EarthSunderFurtherReducesResistances, EarthIncreasedDamageToLowerArmorTargets, EarthAmpDamageOnHealthyTargets, EarthHealOnCritAtSunderThreshold,
    EarthPhysicalBonusSunderStacksOnThreshold, EarthPhysicalSunderAmpsCrits, EarthPhysicalSunderAmpsDamage, EarthBleedBonusCritChanceOnBleedingTarget, EarthBleedSunderAddsPercentageOfBleed, EarthBleedBonusEarthDamageToBleeding, EarthBleedBloodExplosionOnBleed, EarthPoisonAddSunderedOnPoisonTick, EarthPoisonSummonPillarOnThreshold, EarthPoisonSunderToPoisonConversion, EarthPoisonSunderToPoisonOnCrit,
    EarthStunStunOnThreshold, EarthStunBonusDamageOnStun, EarthStunKillingStunnedWithEarthRefundsCooldowns, EarthStunStunningAddsSunder, EarthStunSunderAmpsStunDamageLength, EarthKnockbackTremorsOnKnockback, EarthKnockbackSunderReducesKnockbackResistance, EarthKnockbackSummonRocksOnRecentKnockbackTarget, WindAmpsDamageTaken, WindAmpsComboArmorShred, WindTargetGainsBleedOnAttack,
    WindSummonAerobladesOnThreshold, WindWindshearAmpsTrueDamage, WindAddMoreStacksOnInitialStack, WindMoreDamageOnMaximumStacks, WindPhysicalSummonWhirlwindOnSkillHit, WindPhysicalWindshearAmpsBasicAttacks, WindPhysicalCritsDealArmorAsDamage, WindBleedAmpBleedAtThreshold, WindBleedMoreBleedStacksAThreshold, WindBleedBleedGrantsWindCritChance, WindBleedAddBleedOnWindCrit, WindPoisonTransferPoisonStacksOnKill,
    WindPoisonWindAddsPercentageOfPoisonOnHit, WindPoisonPoisonBurstAtWindThreshold, WindStunStunDealsTrueDamageAtThreshold, WindStunWindblastOnStun, WindStunStunAmpsWindshearGain, WindKnockbackKnockbackSummonsMiniCyclone, WindKnockbackLoseKnockbackResistanceOnThreshold, WindKnockbackWindshearDoesDamageIfKnockedBack, PhysicalPhysicalAmpsCritChance, PhysicalPhysicalSkillsComboAmp, PhysicalLifestealAmp,
    PhysicalSkillAmpArmorOnKill, PhysicalAmpDamageBelowHalfHp, PhysicalBleedBleedAmpsPhysicalDamage, PhysicalBleedPhysicalSkillsAddBleed, PhysicalBleedSkillsDoTrueDamageAtThreshold, PhysicalPoisonPhysicalAmpsPoisonDamage, PhysicalPoisonPlayerMaxHpDamageOnThreshold, PhysicalPoisonPoisonAmpsPhysicalDamage, PhysicalStunAmpDamageOnStunned, PhysicalStunBladeRiftOnStun, PhysicalKnockbackKnockbackKillAmpsPhysicalDamage,
    PhysicalKnockbackPhysicalAttacksGainInnateKnockback, PhysicalKnockbackSummonKnivesOnKnockbackHit, BleedReducesResistances, BleedAmpsCritHitsAddsBleedToNearby, BleedSlowsTargets, BleedAmpDamageTakenOnAttack, BleedHealOnBleedingEnemyKill, BleedCritsConsumeBleed, BleedAmpDamageAtThreshold, BleedBloodWellAtThreshold, BleedExpungeBleedAtThreshold, BleedPoisonReduceOtherResistances, BleedPoisonChanceOfPoisonCloudOnBleed,
    BleedStunStunReducesBleedResistance, BleedStunStunAtThresholdBelowHalfHP, BleedStunStunAddsBleed, BleedKnockbackBonusBleed, BleedKnockbackKnockbackExposionOfBlood, BleedKnockbackKnockbackAmpsDamage, PoisonSpreadStacksOnDeath, PoisonAmpsLifesteal, PoisonAmpNextAttackAfterPoisonKill, PoisonAmpDamageOnFirstStack, PoisonPrimaryTraitsAmpPoison, PoisonShredArmorOnThreshold, PoisonEnemiesAmpPoisonOnKill,
    PoisonTrueDamageAtThreshold, PoisonVamperism, PoisonStunPoisonReducesStunResist, PoisonStunPoisonSpreadOnThreshold, PoisonKnockbackPoisonReducesKnockbackResistance, PoisonKnockbackConsumePoisonStacksForTrueDamage, StunReducesArmor, StunAmpsDamageTaken, StunAmpsAfflictionGain, StunAmpDuration, StunShockwaveOnStunningStunned, StunOnStunDealsAdditionalBaseDamage, StunAmpDurationOnThreshold,
    StunAmpCritDamage, StunAmpsMovespeed, StunKnockbackStunReduceResistance, KnockbackReducesSpellCooldowns, KnockBackAmpsBasicAttacks, KnockbackAmpKnockbackForce, KnockbackAmpsDamageTaken, KnockbackAmpsArmor, KnockbackDoesBonusDamageOnKnockbacked, KnockbackReducesResistances, SpellDamage, Luck, BasicAttackAmp, BonusStacksOnHit
    };
    
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
        int randomTrait = Random.Range(0, 192);
        switch (randomTrait)
        {
            case 0:
                traitType = TraitType.HealthFlat;
                traitBonus = 20;
                break;
            case 1:
                traitType = TraitType.HealthPercent;
                traitBonus = 0.05f;
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
                traitBonus = 5;
                break;
            case 5:
                traitType = TraitType.Armor;
                traitBonus = 10;
                break;
            case 6:
                traitType = TraitType.CooldownReduction;
                traitBonus = 0.05f;
                break;
            case 7:
                traitType = TraitType.AttackSpeed;
                traitBonus = 0.10f;
                break;
            case 8:
                traitType = TraitType.MoveSpeed;
                traitBonus = 0.03f;
                break;
            case 9:
                traitType = TraitType.CritChance;
                traitBonus = 0.05f;
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
                traitBonus = 2;
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
                traitBonus = 1.2f;
                break;
            case 24:
                traitType = TraitType.AflameWindshearWindAttacksGainCritOnBurningTarget;
                traitBonus = 0.005f;
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
                traitBonus = 0.005f;
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
                traitBonus = 0.01f;
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
            case 43:
                traitType = TraitType.AflameStunStunOnThreshold;
                traitBonus = 1f;
                break;
            case 44:
                traitType = TraitType.AflameStunStunReducesFireResistance;
                traitBonus = 0.2f;
                break;
            case 45:
                traitType = TraitType.AflameStunStunAmpsBurnDamage;
                traitBonus = 2f;
                break;
            case 46:
                traitType = TraitType.AflameKnockbackAflameReducesKnockbackResist;
                traitBonus = 0.01f;
                break;
            case 47:
                traitType = TraitType.AflameKnockbackAflameSpellsOnKnockbackedTargetExplode;
                traitBonus = 0.25f;
                break;
            case 48:
                traitType = TraitType.AflameKnockbackKnockbackAmpsFireDamage;
                traitBonus = 0.25f;
                break;
            case 49:
                traitType = TraitType.IceFreezeAtStackThreshold;
                traitBonus = 1f;
                break;
            case 50:
                traitType = TraitType.IceAmpAllDamageAtThreshold;
                traitBonus = 0.25f;
                break;
            case 51:
                traitType = TraitType.IceBasicAttacksConsumeStacksAtThreshold;
                traitBonus = 0.1f;
                break;
            case 52:
                traitType = TraitType.IceEnemyAttacksWeakendAtThreshold;
                traitBonus = 2f;
                break;
            case 53:
                traitType = TraitType.IceEnemiesGainFrostbiteOnStrikingYou;
                traitBonus = 3f;
                break;
            case 54:
                traitType = TraitType.IceAddStacksToNearbyEnemies;
                traitBonus = 1;
                break;
            case 55:
                traitType = TraitType.IceAmpFrostbiteDamage;
                traitBonus = 0.25f;
                break;
            case 56:
                traitType = TraitType.IceEarthFrostToEarthBonusDamage;
                traitBonus = 0.02f;
                break;
            case 57:
                traitType = TraitType.IceEarthSunderAmpsIceDamage;
                traitBonus = 0.04f;
                break;
            case 58:
                traitType = TraitType.IceEarthIceDOTAtThreshold;
                traitBonus = 0.25f;
                break;
            case 59:
                traitType = TraitType.IceEarthEarthSpellBonusCritDamage;
                traitBonus = 0.025f;
                break;
            case 60:
                traitType = TraitType.IceWindWindAmpsFrostbiteDamage;
                traitBonus = 0.1f;
                break;
            case 61:
                traitType = TraitType.IceWindWindSpellsDamageAmp;
                traitBonus = 0.03f;
                break;
            case 62:
                traitType = TraitType.IceWindIncreaseArmorShredPerFrostbite;
                traitBonus = 0.025f;
                break;
            case 63:
                traitType = TraitType.IceWindSummonTornadoOnHit;
                traitBonus = 0.25f;
                break;
            case 64:
                traitType = TraitType.IcePhysicalFrostbiteAmpsPhysicalCritDamage;
                traitBonus = 0.02f;
                break;
            case 65:
                traitType = TraitType.IcePhysicalPhysicalVampOnFrostbite;
                traitBonus = 0.01f;
                break;
            case 66:
                traitType = TraitType.IcePhysicalBladeVortexOnHit;
                traitBonus = 0.33f;
                break;
            case 67:
                traitType = TraitType.IceBleedFrostbiteAmpsBleed;
                traitBonus = 0.02f;
                break;
            case 68:
                traitType = TraitType.IceBleedBleedDoesDamageInstantlyOnThreshold;
                traitBonus = 3f;
                break;
            case 69:
                traitType = TraitType.IcePoisonFreezingPoison;
                traitBonus = 0.05f;
                break;
            case 70:
                traitType = TraitType.IcePoisonFrostbiteResetsPoisonAndAmps;
                traitBonus = 1f;
                break;
            case 71:
                traitType = TraitType.IcePoisonSummonPoisonPillarOnThreshold;
                traitBonus = 1f;
                break;
            case 72:
                traitType = TraitType.IceStunRudeAwakening;
                traitBonus = 1f;
                break;
            case 73:
                traitType = TraitType.IceStunIceRefreshesStun;
                traitBonus = 0.25f;
                break;
            case 74:
                traitType = TraitType.IceKnockbackFrostbiteIncreasesKnockbackForce;
                traitBonus = 0.05f;
                break;
            case 75:
                traitType = TraitType.IceKnockbackSnowEruptionOnKnockback;
                traitBonus = 0.1f;
                break;
            case 76:
                traitType = TraitType.IceKnockbackBonusStacksOnDownedTargets;
                traitBonus = 1f;
                break;
            case 77:
                traitType = TraitType.EarthMaxHpDamageAtThreshold;
                traitBonus = 0.01f;
                break;
            case 78:
                traitType = TraitType.EarthAmpAllAfflictionsOnThreshhold;
                traitBonus = 0.05f;
                break;
            case 79:
                traitType = TraitType.EarthSunderedEnemiesDealLessDamage;
                traitBonus = 0.02f;
                break;
            case 80:
                traitType = TraitType.EarthRockRingExplosionOnKill;
                traitBonus = 0.25f;
                break;
            case 81:
                traitType = TraitType.EarthTrueDamageAtThreshold;
                traitBonus = 1f;
                break;
            case 82:
                traitType = TraitType.EarthSunderFurtherReducesResistances;
                traitBonus = 0.05f;
                break;
            case 83:
                traitType = TraitType.EarthIncreasedDamageToLowerArmorTargets;
                traitBonus = 0.1f;
                break;
            case 84:
                traitType = TraitType.EarthAmpDamageOnHealthyTargets;
                traitBonus = 0.25f;
                break;
            case 85:
                traitType = TraitType.EarthHealOnCritAtSunderThreshold;
                traitBonus = 0.05f;
                break;
            case 86:
                traitType = TraitType.EarthPhysicalBonusSunderStacksOnThreshold;
                traitBonus = 1f;
                break;
            case 87:
                traitType = TraitType.EarthPhysicalSunderAmpsCrits;
                traitBonus = 0.02f;
                break;
            case 88:
                traitType = TraitType.EarthPhysicalSunderAmpsDamage;
                traitBonus = 0.01f;
                break;
            case 89:
                traitType = TraitType.EarthBleedBonusCritChanceOnBleedingTarget;
                traitBonus = 0.01f;
                break;
            case 90:
                traitType = TraitType.EarthBleedSunderAddsPercentageOfBleed;
                traitBonus = 0.05f;
                break;
            case 91:
                traitType = TraitType.EarthBleedBonusEarthDamageToBleeding;
                traitBonus = 0.03f;
                break;
            case 92:
                traitType = TraitType.EarthBleedBloodExplosionOnBleed;
                traitBonus = 0.25f;
                break;
            case 93:
                traitType = TraitType.EarthPoisonAddSunderedOnPoisonTick;
                traitBonus = 0.05f;
                break;
            case 94:
                traitType = TraitType.EarthPoisonSummonPillarOnThreshold;
                traitBonus = 1f;
                break;
            case 95:
                traitType = TraitType.EarthPoisonSunderToPoisonConversion;
                traitBonus = 1f;
                break;
            case 96:
                traitType = TraitType.EarthPoisonSunderToPoisonOnCrit;
                traitBonus = 1f;
                break;
            case 97:
                traitType = TraitType.EarthStunStunOnThreshold;
                traitBonus = 1f;
                break;
            case 98:
                traitType = TraitType.EarthStunBonusDamageOnStun;
                traitBonus = 0.25f;
                break;
            case 99:
                traitType = TraitType.EarthStunKillingStunnedWithEarthRefundsCooldowns;
                traitBonus = 0.05f;
                break;
            case 100:
                traitType = TraitType.EarthStunStunningAddsSunder;
                traitBonus = 1f;
                break;
            case 101:
                traitType = TraitType.EarthStunSunderAmpsStunDamageLength;
                traitBonus = 0.01f;
                break;
            case 102:
                traitType = TraitType.EarthKnockbackTremorsOnKnockback;
                traitBonus = 0.1f;
                break;
            case 103:
                traitType = TraitType.EarthKnockbackSunderReducesKnockbackResistance;
                traitBonus = 2f;
                break;
            case 104:
                traitType = TraitType.EarthKnockbackSummonRocksOnRecentKnockbackTarget;
                traitBonus = 1f;
                break;
            case 105:
                traitType = TraitType.WindAmpsDamageTaken;
                traitBonus = 0.005f;
                break;
            case 106:
                traitType = TraitType.WindAmpsComboArmorShred;
                traitBonus = 0.2f;
                break;
            case 107:
                traitType = TraitType.WindTargetGainsBleedOnAttack;
                traitBonus = 1f;
                break;
            case 108:
                traitType = TraitType.WindSummonAerobladesOnThreshold;
                traitBonus = 0.33f;
                break;
            case 109:
                traitType = TraitType.WindWindshearAmpsTrueDamage;
                traitBonus = 0.01f;
                break;
            case 110:
                traitType = TraitType.WindAddMoreStacksOnInitialStack;
                traitBonus = 3f;
                break;
            case 111:
                traitType = TraitType.WindMoreDamageOnMaximumStacks;
                traitBonus = 2f;
                break;
            case 112:
                traitType = TraitType.WindPhysicalSummonWhirlwindOnSkillHit;
                traitBonus = 0.4f;
                break;
            case 113:
                traitType = TraitType.WindPhysicalWindshearAmpsBasicAttacks;
                traitBonus = 0.03f;
                break;
            case 114:
                traitType = TraitType.WindPhysicalCritsDealArmorAsDamage;
                traitBonus = 0.05f;
                break;
            case 115:
                traitType = TraitType.WindBleedAmpBleedAtThreshold;
                traitBonus = 0.33f;
                break;
            case 116:
                traitType = TraitType.WindBleedMoreBleedStacksAThreshold;
                traitBonus = 0.5f;
                break;
            case 117:
                traitType = TraitType.WindBleedBleedGrantsWindCritChance;
                traitBonus = 0.01f;
                break;
            case 118:
                traitType = TraitType.WindBleedAddBleedOnWindCrit;
                traitBonus = 2f;
                break;
            case 119:
                traitType = TraitType.WindPoisonTransferPoisonStacksOnKill;
                traitBonus = 2f;
                break;
            case 120:
                traitType = TraitType.WindPoisonWindAddsPercentageOfPoisonOnHit;
                traitBonus = 0.05f;
                break;
            case 121:
                traitType = TraitType.WindPoisonPoisonBurstAtWindThreshold;
                traitBonus = 1f;
                break;
            case 122:
                traitType = TraitType.WindStunStunDealsTrueDamageAtThreshold;
                traitBonus = 1f;
                break;
            case 123:
                traitType = TraitType.WindStunWindblastOnStun;
                traitBonus = 1f;
                break;
            case 124:
                traitType = TraitType.WindStunStunAmpsWindshearGain;
                traitBonus = 1f;
                break;
            case 125:
                traitType = TraitType.WindKnockbackKnockbackSummonsMiniCyclone;
                traitBonus = 0.33f;
                break;
            case 126:
                traitType = TraitType.WindKnockbackLoseKnockbackResistanceOnThreshold;
                traitBonus = 0.25f;
                break;
            case 127:
                traitType = TraitType.WindKnockbackWindshearDoesDamageIfKnockedBack;
                traitBonus = 0.2f;
                break;
            case 128:
                traitType = TraitType.PhysicalPhysicalAmpsCritChance;
                traitBonus = 0.02f;
                break;
            case 129:
                traitType = TraitType.PhysicalPhysicalSkillsComboAmp;
                traitBonus = 0.33f;
                break;
            case 130:
                traitType = TraitType.PhysicalLifestealAmp;
                traitBonus = 0.25f;
                break;
            case 131:
                traitType = TraitType.PhysicalSkillAmpArmorOnKill;
                traitBonus = 2f;
                break;
            case 132:
                traitType = TraitType.PhysicalAmpDamageBelowHalfHp;
                traitBonus = 0.5f;
                break;
            case 133:
                traitType = TraitType.PhysicalBleedBleedAmpsPhysicalDamage;
                traitBonus = 0.01f;
                break;
            case 134:
                traitType = TraitType.PhysicalBleedPhysicalSkillsAddBleed;
                traitBonus = 1f;
                break;
            case 135:
                traitType = TraitType.PhysicalBleedSkillsDoTrueDamageAtThreshold;
                traitBonus = 4f;
                break;
            case 136:
                traitType = TraitType.PhysicalPoisonPhysicalAmpsPoisonDamage;
                traitBonus = 3f;
                break;
            case 137:
                traitType = TraitType.PhysicalPoisonPlayerMaxHpDamageOnThreshold;
                traitBonus = 0.05f;
                break;
            case 138:
                traitType = TraitType.PhysicalPoisonPoisonAmpsPhysicalDamage;
                traitBonus = 3f;
                break;
            case 139:
                traitType = TraitType.PhysicalStunAmpDamageOnStunned;
                traitBonus = 1f;
                break;
            case 140:
                traitType = TraitType.PhysicalStunBladeRiftOnStun;
                traitBonus = 0.25f;
                break;
            case 141:
                traitType = TraitType.PhysicalKnockbackKnockbackKillAmpsPhysicalDamage;
                traitBonus = 3f;
                break;
            case 142:
                traitType = TraitType.PhysicalKnockbackPhysicalAttacksGainInnateKnockback;
                traitBonus = 0.02f;
                break;
            case 143:
                traitType = TraitType.PhysicalKnockbackSummonKnivesOnKnockbackHit;
                traitBonus = 2f;
                break;
            case 144:
                traitType = TraitType.BleedReducesResistances;
                traitBonus = 0.002f;
                break;
            case 145:
                traitType = TraitType.BleedAmpsCritHitsAddsBleedToNearby;
                traitBonus = 0.025f;
                break;
            case 146:
                traitType = TraitType.BleedSlowsTargets;
                traitBonus = 0.02f;
                break;
            case 147:
                traitType = TraitType.BleedAmpDamageTakenOnAttack;
                traitBonus = 0.5f;
                break;
            case 148:
                traitType = TraitType.BleedHealOnBleedingEnemyKill;
                traitBonus = 1f;
                break;
            case 149:
                traitType = TraitType.BleedCritsConsumeBleed;
                traitBonus = 0.05f;
                break;
            case 150:
                traitType = TraitType.BleedAmpDamageAtThreshold;
                traitBonus = 0.25f;
                break;
            case 151:
                traitType = TraitType.BleedBloodWellAtThreshold;
                traitBonus = 0.5f;
                break;
            case 152:
                traitType = TraitType.BleedExpungeBleedAtThreshold;
                traitBonus = 5f;
                break;
            case 153:
                traitType = TraitType.BleedPoisonReduceOtherResistances;
                traitBonus = 0.01f;
                break;
            case 154:
                traitType = TraitType.BleedPoisonChanceOfPoisonCloudOnBleed;
                traitBonus = 2f;
                break;
            case 155:
                traitType = TraitType.BleedStunStunReducesBleedResistance;
                traitBonus = 0.2f;
                break;
            case 156:
                traitType = TraitType.BleedStunStunAtThresholdBelowHalfHP;
                traitBonus = 1f;
                break;
            case 157:
                traitType = TraitType.BleedStunStunAddsBleed;
                traitBonus = 2f;
                break;
            case 158:
                traitType = TraitType.BleedKnockbackBonusBleed;
                traitBonus = 1f;
                break;
            case 159:
                traitType = TraitType.BleedKnockbackKnockbackExposionOfBlood;
                traitBonus = 1.5f;
                break;
            case 160:
                traitType = TraitType.BleedKnockbackKnockbackAmpsDamage;
                traitBonus = 0.05f;
                break;
            case 161:
                traitType = TraitType.PoisonSpreadStacksOnDeath;
                traitBonus = 0.05f;
                break;
            case 162:
                traitType = TraitType.PoisonSpreadStacksOnDeath;
                traitBonus = 0.25f;
                break;
            case 163:
                traitType = TraitType.PoisonAmpNextAttackAfterPoisonKill;
                traitBonus = 1f;
                break;
            case 164:
                traitType = TraitType.PoisonAmpDamageOnFirstStack;
                traitBonus = 0.5f;
                break;
            case 165:
                traitType = TraitType.PoisonPrimaryTraitsAmpPoison;
                traitBonus = 0.5f;
                break;
            case 167:
                traitType = TraitType.PoisonShredArmorOnThreshold;
                traitBonus = 2f;
                break;
            case 168:
                traitType = TraitType.PoisonEnemiesAmpPoisonOnKill;
                traitBonus = 1f;
                break;
            case 169:
                traitType = TraitType.PoisonTrueDamageAtThreshold;
                traitBonus = 0.025f;
                break;
            case 170:
                traitType = TraitType.PoisonVamperism;
                traitBonus = 0.05f;
                break;
            case 171:
                traitType = TraitType.PoisonStunPoisonReducesStunResist;
                traitBonus = 0.02f;
                break;
            case 172:
                traitType = TraitType.PoisonStunPoisonSpreadOnThreshold;
                traitBonus = 0.25f;
                break;
            case 173:
                traitType = TraitType.PoisonKnockbackPoisonReducesKnockbackResistance;
                traitBonus = 0.02f;
                break;
            case 174:
                traitType = TraitType.PoisonKnockbackConsumePoisonStacksForTrueDamage;
                traitBonus = 0.5f;
                break;
            case 175:
                traitType = TraitType.StunReducesArmor;
                traitBonus = 0.05f;
                break;
            case 176:
                traitType = TraitType.StunAmpsDamageTaken;
                traitBonus = 0.1f;
                break;
            case 177:
                traitType = TraitType.StunAmpsAfflictionGain;
                traitBonus = 0.25f;
                break;
            case 178:
                traitType = TraitType.StunAmpDuration;
                traitBonus = 0.25f;
                break;
            case 179:
                traitType = TraitType.StunShockwaveOnStunningStunned;
                traitBonus = 0.8f;
                break;
            case 180:
                traitType = TraitType.StunOnStunDealsAdditionalBaseDamage;
                traitBonus = 0.25f;
                break;
            case 181:
                traitType = TraitType.StunAmpDurationOnThreshold;
                traitBonus = 0.66f;
                break;
            case 182:
                traitType = TraitType.StunAmpCritDamage;
                traitBonus = 0.25f;
                break;
            case 183:
                traitType = TraitType.StunAmpsMovespeed;
                traitBonus = 0.15f;
                break;
            case 184:
                traitType = TraitType.StunKnockbackStunReduceResistance;
                traitBonus = 0.5f;
                break;
            case 185:
                traitType = TraitType.KnockbackReducesSpellCooldowns;
                traitBonus = 0.025f;
                break;
            case 186:
                traitType = TraitType.KnockBackAmpsBasicAttacks;
                traitBonus = 1f;
                break;
            case 187:
                traitType = TraitType.KnockbackAmpKnockbackForce;
                traitBonus = 0.1f;
                break;
            case 188:
                traitType = TraitType.KnockbackAmpsDamageTaken;
                traitBonus = 0.25f;
                break;
            case 189:
                traitType = TraitType.KnockbackAmpsArmor;
                traitBonus = 0.05f;
                break;
            case 190:
                traitType = TraitType.KnockbackDoesBonusDamageOnKnockbacked;
                traitBonus = 0.33f;
                break;
            case 191:
                traitType = TraitType.KnockbackReducesResistances;
                traitBonus = 0.33f;
                break;
            case 192:
                traitType = TraitType.SpellDamage;
                traitBonus = 0.05f;
                break;
            case 193:
                traitType = TraitType.Luck;
                traitBonus = 1;
                break;
            case 194:
                traitType = TraitType.BasicAttackAmp;
                traitBonus = 0.1f;
                break;
            case 195:
                traitType = TraitType.BonusStacksOnHit;
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

    // USed to grab a trait of a certain type based on the modifier type.
    public void GetTraitForModifier(Item.ModifierType modifier)
    {
        switch (modifier)
        {
            case Item.ModifierType.Devastating:
                switch (Random.Range(0,2))
                {
                    case 0:
                        traitType = TraitType.CritChance;
                        traitBonus = 0.05f;
                        break;
                    case 1:
                        traitType = TraitType.CritDamage;
                        traitBonus = 0.1f;
                        break;
                    default:
                        break;
                }
                break;
            case Item.ModifierType.Dull:
                switch (Random.Range(0, 2))
                {
                    case 0:
                        traitType = TraitType.CritChance;
                        traitBonus = -0.05f;
                        break;
                    case 1:
                        traitType = TraitType.CritDamage;
                        traitBonus = -0.1f;
                        break;
                    default:
                        break;
                }
                break;
            case Item.ModifierType.Hardened:
                switch (Random.Range(0, 2))
                {
                    case 0:
                        traitType = TraitType.Armor;
                        traitBonus = 10f;
                        break;
                    case 1:
                        traitType = TraitType.FlatDamageReduction;
                        traitBonus = 1f;
                        break;
                    default:
                        break;
                }
                break;
            case Item.ModifierType.Cracked:
                switch (Random.Range(0, 2))
                {
                    case 0:
                        traitType = TraitType.Armor;
                        traitBonus = -10f;
                        break;
                    case 1:
                        traitType = TraitType.FlatDamageReduction;
                        traitBonus = -1f;
                        break;
                    default:
                        break;
                }
                break;
            case Item.ModifierType.Nimble:
                switch (Random.Range(0, 3))
                {
                    case 0:
                        traitType = TraitType.AttackSpeed;
                        traitBonus = 0.1f;
                        break;
                    case 1:
                        traitType = TraitType.MoveSpeed;
                        traitBonus = 0.03f;
                        break;
                    case 2:
                        traitType = TraitType.Jumps;
                        traitBonus = 1f;
                        break;
                    default:
                        break;
                }
                break;
            case Item.ModifierType.Sluggish:
                switch (Random.Range(0, 3))
                {
                    case 0:
                        traitType = TraitType.AttackSpeed;
                        traitBonus = -0.1f;
                        break;
                    case 1:
                        traitType = TraitType.MoveSpeed;
                        traitBonus = -0.03f;
                        break;
                    case 2:
                        traitType = TraitType.Jumps;
                        traitBonus = -1f;
                        break;
                    default:
                        break;
                }
                break;
            case Item.ModifierType.Vamperic:
                switch (Random.Range(0, 2))
                {
                    case 0:
                        traitType = TraitType.HealingOnHit;
                        traitBonus = 1f;
                        break;
                    case 1:
                        traitType = TraitType.HealingOnKill;
                        traitBonus = 10f;
                        break;
                    default:
                        break;
                }
                break;
            case Item.ModifierType.Cursed:
                switch (Random.Range(0, 2))
                {
                    case 0:
                        traitType = TraitType.HealingOnHit;
                        traitBonus = -1f;
                        break;
                    case 1:
                        traitType = TraitType.HealingOnKill;
                        traitBonus = -10f;
                        break;
                    default:
                        break;
                }
                break;
            case Item.ModifierType.Magic:
                switch (Random.Range(0, 2))
                {
                    case 0:
                        traitType = TraitType.SpellDamage;
                        traitBonus = 0.05f;
                        break;
                    case 1:
                        traitType = TraitType.CooldownReduction;
                        traitBonus = 0.05f;
                        break;
                    default:
                        break;
                }
                break;
            case Item.ModifierType.Mundane:
                switch (Random.Range(0, 2))
                {
                    case 0:
                        traitType = TraitType.SpellDamage;
                        traitBonus = -0.05f;
                        break;
                    case 1:
                        traitType = TraitType.CooldownReduction;
                        traitBonus = -0.05f;
                        break;
                    default:
                        break;
                }
                break;
            case Item.ModifierType.Lucky:
                traitType = TraitType.Luck;
                traitBonus = 1f;
                break;
            case Item.ModifierType.Unfavoured:
                traitType = TraitType.Luck;
                traitBonus = -1f;
                break;
            case Item.ModifierType.Strong:
                traitType = TraitType.BasicAttackAmp;
                traitBonus = 0.05f;
                break;
            case Item.ModifierType.Brittle:
                traitType = TraitType.BasicAttackAmp;
                traitBonus = -0.05f;
                break;
            case Item.ModifierType.Illustrious:
                traitType = TraitType.BonusStacksOnHit;
                traitBonus = 1f;
                break;
            case Item.ModifierType.Rusty:
                traitType = TraitType.BonusStacksOnHit;
                traitBonus = -1f;
                break;
            case Item.ModifierType.Brawny:
                switch (Random.Range(0, 3))
                {
                    case 0:
                        traitType = TraitType.HealthFlat;
                        traitBonus = 20f;
                        break;
                    case 1:
                        traitType = TraitType.HealthPercent;
                        traitBonus = 0.05f;
                        break;
                    case 2:
                        traitType = TraitType.HealthRegen;
                        traitBonus = 2f;
                        break;
                    default:
                        break;
                }
                break;
            case Item.ModifierType.Meager:
                switch (Random.Range(0, 3))
                {
                    case 0:
                        traitType = TraitType.HealthFlat;
                        traitBonus = -20f;
                        break;
                    case 1:
                        traitType = TraitType.HealthPercent;
                        traitBonus = -0.05f;
                        break;
                    case 2:
                        traitType = TraitType.HealthRegen;
                        traitBonus = -2f;
                        break;
                    default:
                        break;
                }
                break;
            case Item.ModifierType.Resistant:
                switch (Random.Range(0, 9))
                {
                    case 0:
                        traitType = TraitType.AflameResistance;
                        traitBonus = 0.1f;
                        break;
                    case 1:
                        traitType = TraitType.FrostbiteResistance;
                        traitBonus = 0.1f;
                        break;
                    case 2:
                        traitType = TraitType.WindshearResistance;
                        traitBonus = 0.1f;
                        break;
                    case 3:
                        traitType = TraitType.SunderResistance;
                        traitBonus = 0.1f;
                        break;
                    case 4:
                        traitType = TraitType.BleedResistance;
                        traitBonus = 0.1f;
                        break;
                    case 5:
                        traitType = TraitType.PoisonResistance;
                        traitBonus = 0.1f;
                        break;
                    case 6:
                        traitType = TraitType.AsleepResistance;
                        traitBonus = 0.1f;
                        break;
                    case 7:
                        traitType = TraitType.StunResistance;
                        traitBonus = 0.1f;
                        break;
                    case 8:
                        traitType = TraitType.KnockbackResistance;
                        traitBonus = 0.1f;
                        break;
                    case 9:
                        traitType = TraitType.OverchargeResistance;
                        traitBonus = 0.1f;
                        break;
                    case 10:
                        traitType = TraitType.OvergrowthResistance;
                        traitBonus = 0.1f;
                        break;
                }
                break;
            case Item.ModifierType.Vulnerable:
                switch (Random.Range(0, 9))
                {
                    case 0:
                        traitType = TraitType.AflameResistance;
                        traitBonus = -0.1f;
                        break;
                    case 1:
                        traitType = TraitType.FrostbiteResistance;
                        traitBonus = -0.1f;
                        break;
                    case 2:
                        traitType = TraitType.WindshearResistance;
                        traitBonus = -0.1f;
                        break;
                    case 3:
                        traitType = TraitType.SunderResistance;
                        traitBonus = -0.1f;
                        break;
                    case 4:
                        traitType = TraitType.BleedResistance;
                        traitBonus = -0.1f;
                        break;
                    case 5:
                        traitType = TraitType.PoisonResistance;
                        traitBonus = -0.1f;
                        break;
                    case 6:
                        traitType = TraitType.AsleepResistance;
                        traitBonus = -0.1f;
                        break;
                    case 7:
                        traitType = TraitType.StunResistance;
                        traitBonus = -0.1f;
                        break;
                    case 8:
                        traitType = TraitType.KnockbackResistance;
                        traitBonus = -0.1f;
                        break;
                    case 9:
                        traitType = TraitType.OverchargeResistance;
                        traitBonus = -0.1f;
                        break;
                    case 10:
                        traitType = TraitType.OvergrowthResistance;
                        traitBonus = -0.1f;
                        break;
                }
                break;
            default:
                break;
        }

    }

    // Grabs a trait based on the affinity types.
    public void GetTraitForAffinity(Item.AffinityType affinity, Item.AffinityType affinitySplit)
    {
        // These are for affinities with no split between other elements
        if(affinitySplit == Item.AffinityType.None)
        {
            switch (affinity)
            {
                case Item.AffinityType.Fire:
                    switch (Random.Range(0, 4))
                    {
                        case 0:
                            traitType = TraitType.MoreAflameStacksOnHitThreshold;
                            traitBonus = 3;
                            break;
                        case 1:
                            traitType = TraitType.BasicAttacksShredArmorOnAflame;
                            traitBonus = 3;
                            break;
                        case 2:
                            traitType = TraitType.FlameVamperism;
                            traitBonus = 1;
                            break;
                        case 3:
                            traitType = TraitType.RingOfFireOnHit;
                            traitBonus = 0.5f;
                            break;
                    }
                    break;
                case Item.AffinityType.Ice:
                    switch (Random.Range(0, 7))
                    {
                        case 0:
                            traitType = TraitType.IceFreezeAtStackThreshold;
                            traitBonus = 1f;
                            break;
                        case 1:
                            traitType = TraitType.IceAmpAllDamageAtThreshold;
                            traitBonus = 0.25f;
                            break;
                        case 2:
                            traitType = TraitType.IceBasicAttacksConsumeStacksAtThreshold;
                            traitBonus = 0.1f;
                            break;
                        case 3:
                            traitType = TraitType.IceEnemyAttacksWeakendAtThreshold;
                            traitBonus = 2f;
                            break;
                        case 4:
                            traitType = TraitType.IceEnemiesGainFrostbiteOnStrikingYou;
                            traitBonus = 3f;
                            break;
                        case 5:
                            traitType = TraitType.IceAddStacksToNearbyEnemies;
                            traitBonus = 1;
                            break;
                        case 6:
                            traitType = TraitType.IceAmpFrostbiteDamage;
                            traitBonus = 0.25f;
                            break;
                    }
                    break;
                case Item.AffinityType.Earth:
                    switch (Random.Range(0, 9))
                    {
                        case 0:
                            traitType = TraitType.EarthMaxHpDamageAtThreshold;
                            traitBonus = 0.01f;
                            break;
                        case 1:
                            traitType = TraitType.EarthAmpAllAfflictionsOnThreshhold;
                            traitBonus = 0.05f;
                            break;
                        case 2:
                            traitType = TraitType.EarthSunderedEnemiesDealLessDamage;
                            traitBonus = 0.02f;
                            break;
                        case 3:
                            traitType = TraitType.EarthRockRingExplosionOnKill;
                            traitBonus = 0.25f;
                            break;
                        case 4:
                            traitType = TraitType.EarthTrueDamageAtThreshold;
                            traitBonus = 1f;
                            break;
                        case 5:
                            traitType = TraitType.EarthSunderFurtherReducesResistances;
                            traitBonus = 0.05f;
                            break;
                        case 6:
                            traitType = TraitType.EarthIncreasedDamageToLowerArmorTargets;
                            traitBonus = 0.1f;
                            break;
                        case 7:
                            traitType = TraitType.EarthAmpDamageOnHealthyTargets;
                            traitBonus = 0.25f;
                            break;
                        case 8:
                            traitType = TraitType.EarthHealOnCritAtSunderThreshold;
                            traitBonus = 0.05f;
                            break;
                    }
                    break;
                case Item.AffinityType.Wind:
                    switch (Random.Range(0, 7))
                    {
                        case 0:
                            traitType = TraitType.WindAmpsDamageTaken;
                            traitBonus = 0.005f;
                            break;
                        case 1:
                            traitType = TraitType.WindAmpsComboArmorShred;
                            traitBonus = 0.2f;
                            break;
                        case 2:
                            traitType = TraitType.WindTargetGainsBleedOnAttack;
                            traitBonus = 1f;
                            break;
                        case 3:
                            traitType = TraitType.WindSummonAerobladesOnThreshold;
                            traitBonus = 0.33f;
                            break;
                        case 4:
                            traitType = TraitType.WindWindshearAmpsTrueDamage;
                            traitBonus = 0.01f;
                            break;
                        case 5:
                            traitType = TraitType.WindAddMoreStacksOnInitialStack;
                            traitBonus = 3f;
                            break;
                        case 6:
                            traitType = TraitType.WindMoreDamageOnMaximumStacks;
                            traitBonus = 2f;
                            break;
                    }
                    break;
                case Item.AffinityType.Physical:
                    switch (Random.Range(0, 5))
                    {
                        case 0:
                            traitType = TraitType.PhysicalPhysicalAmpsCritChance;
                            traitBonus = 0.02f;
                            break;
                        case 1:
                            traitType = TraitType.PhysicalPhysicalSkillsComboAmp;
                            traitBonus = 0.33f;
                            break;
                        case 2:
                            traitType = TraitType.PhysicalLifestealAmp;
                            traitBonus = 0.25f;
                            break;
                        case 3:
                            traitType = TraitType.PhysicalSkillAmpArmorOnKill;
                            traitBonus = 2f;
                            break;
                        case 4:
                            traitType = TraitType.PhysicalAmpDamageBelowHalfHp;
                            traitBonus = 0.5f;
                            break;
                    }
                    break;
                case Item.AffinityType.Bleed:
                    switch (Random.Range(0, 9))
                    {
                        case 0:
                            traitType = TraitType.BleedReducesResistances;
                            traitBonus = 0.002f;
                            break;
                        case 1:
                            traitType = TraitType.BleedAmpsCritHitsAddsBleedToNearby;
                            traitBonus = 0.025f;
                            break;
                        case 2:
                            traitType = TraitType.BleedSlowsTargets;
                            traitBonus = 0.02f;
                            break;
                        case 3:
                            traitType = TraitType.BleedAmpDamageTakenOnAttack;
                            traitBonus = 0.5f;
                            break;
                        case 4:
                            traitType = TraitType.BleedHealOnBleedingEnemyKill;
                            traitBonus = 1f;
                            break;
                        case 5:
                            traitType = TraitType.BleedCritsConsumeBleed;
                            traitBonus = 0.05f;
                            break;
                        case 6:
                            traitType = TraitType.BleedAmpDamageAtThreshold;
                            traitBonus = 0.25f;
                            break;
                        case 7:
                            traitType = TraitType.BleedBloodWellAtThreshold;
                            traitBonus = 0.5f;
                            break;
                        case 8:
                            traitType = TraitType.BleedExpungeBleedAtThreshold;
                            traitBonus = 5f;
                            break;
                    }
                    break;
                case Item.AffinityType.Poison:
                    switch (Random.Range(0, 9))
                    {
                        case 0:
                            traitType = TraitType.PoisonSpreadStacksOnDeath;
                            traitBonus = 0.05f;
                            break;
                        case 1:
                            traitType = TraitType.PoisonSpreadStacksOnDeath;
                            traitBonus = 0.25f;
                            break;
                        case 2:
                            traitType = TraitType.PoisonAmpNextAttackAfterPoisonKill;
                            traitBonus = 1f;
                            break;
                        case 3:
                            traitType = TraitType.PoisonAmpDamageOnFirstStack;
                            traitBonus = 0.5f;
                            break;
                        case 4:
                            traitType = TraitType.PoisonPrimaryTraitsAmpPoison;
                            traitBonus = 0.5f;
                            break;
                        case 5:
                            traitType = TraitType.PoisonShredArmorOnThreshold;
                            traitBonus = 2f;
                            break;
                        case 6:
                            traitType = TraitType.PoisonEnemiesAmpPoisonOnKill;
                            traitBonus = 1f;
                            break;
                        case 7:
                            traitType = TraitType.PoisonTrueDamageAtThreshold;
                            traitBonus = 0.025f;
                            break;
                        case 8:
                            traitType = TraitType.PoisonVamperism;
                            traitBonus = 0.05f;
                            break;
                    }
                    break;
                case Item.AffinityType.Stun:
                    switch (Random.Range(0, 10))
                    {
                        case 1:
                            traitType = TraitType.StunReducesArmor;
                            traitBonus = 0.05f;
                            break;
                        case 2:
                            traitType = TraitType.StunAmpsDamageTaken;
                            traitBonus = 0.1f;
                            break;
                        case 3:
                            traitType = TraitType.StunAmpsAfflictionGain;
                            traitBonus = 0.25f;
                            break;
                        case 4:
                            traitType = TraitType.StunAmpDuration;
                            traitBonus = 0.25f;
                            break;
                        case 5:
                            traitType = TraitType.StunShockwaveOnStunningStunned;
                            traitBonus = 0.8f;
                            break;
                        case 6:
                            traitType = TraitType.StunOnStunDealsAdditionalBaseDamage;
                            traitBonus = 0.25f;
                            break;
                        case 7:
                            traitType = TraitType.StunAmpDurationOnThreshold;
                            traitBonus = 0.66f;
                            break;
                        case 8:
                            traitType = TraitType.StunAmpCritDamage;
                            traitBonus = 0.25f;
                            break;
                        case 9:
                            traitType = TraitType.StunAmpsMovespeed;
                            traitBonus = 0.15f;
                            break;
                    }
                    break;
                case Item.AffinityType.Knockback:
                    switch (Random.Range(0, 7))
                    {
                        case 0:
                            traitType = TraitType.KnockbackReducesSpellCooldowns;
                            traitBonus = 0.025f;
                            break;
                        case 1:
                            traitType = TraitType.KnockBackAmpsBasicAttacks;
                            traitBonus = 1f;
                            break;
                        case 2:
                            traitType = TraitType.KnockbackAmpKnockbackForce;
                            traitBonus = 0.1f;
                            break;
                        case 3:
                            traitType = TraitType.KnockbackAmpsDamageTaken;
                            traitBonus = 0.25f;
                            break;
                        case 4:
                            traitType = TraitType.KnockbackAmpsArmor;
                            traitBonus = 0.05f;
                            break;
                        case 5:
                            traitType = TraitType.KnockbackDoesBonusDamageOnKnockbacked;
                            traitBonus = 0.33f;
                            break;
                        case 6:
                            traitType = TraitType.KnockbackReducesResistances;
                            traitBonus = 0.33f;
                            break;
                    }
                    break;
                default:
                    break;
            }
        }
        // These are for elements that double dip into two elements
        else
        {
            switch (affinitySplit)
            {
                case Item.AffinityType.Fire:
                    switch (affinity)
                    {
                        case Item.AffinityType.Earth:
                            switch (Random.Range(0, 3))
                            {
                                case 0:
                                    traitType = TraitType.AflameToSunderStackOnEarthSpell;
                                    traitBonus = 0.1f;
                                    break;
                                case 1:
                                    traitType = TraitType.SunderFurtherDecreasesFireResist;
                                    traitBonus = 1f;
                                    break;
                                case 2:
                                    traitType = TraitType.AflameSunderCritsSummonFireballs;
                                    traitBonus = 1f;
                                    break;
                            }
                            break;
                        case Item.AffinityType.Wind:
                            switch (Random.Range(0, 3))
                            {
                                case 0:
                                    traitType = TraitType.AflameWindshearWindAttacksGainCritOnBurningTarget;
                                    traitBonus = 0.02f;
                                    break;
                                case 1:
                                    traitType = TraitType.AflameWindshearSummonFirePillarsOnHit;
                                    traitBonus = 1f;
                                    break;
                                case 2:
                                    traitType = TraitType.AflameWindshearWindSpellsAddFireStacks;
                                    traitBonus = 0.2f;
                                    break;
                            }

                            break;
                        case Item.AffinityType.Physical:
                            switch (Random.Range(0, 4))
                            {
                                case 0:
                                    traitType = TraitType.AflamePhysicalAddFireStacksOnHit;
                                    traitBonus = 1f;
                                    break;
                                case 1:
                                    traitType = TraitType.AflamePhysicalDamageAmpOnBurningTarget;
                                    traitBonus = 0.05f;
                                    break;
                                case 2:
                                    traitType = TraitType.AflamePhysicalBladeExplosionOnKill;
                                    traitBonus = 1f;
                                    break;
                                case 3:
                                    traitType = TraitType.AflamePhysicalBigHitsAddAflame;
                                    traitBonus = 0.5f;
                                    break;
                            }
                            break;
                        case Item.AffinityType.Bleed:
                            switch (Random.Range(0, 5))
                            {
                                case 0:
                                    traitType = TraitType.AflameBleedIncreasesFlameCritChance;
                                    traitBonus = 0.03f;
                                    break;
                                case 1:
                                    traitType = TraitType.AflameBleedFireDamageAmpOnBleedThreshold;
                                    traitBonus = 0.1f;
                                    break;
                                case 2:
                                    traitType = TraitType.AflameBleedAflameAddsBleedAtThreshhold;
                                    traitBonus = 2f;
                                    break;
                                case 3:
                                    traitType = TraitType.AflameBleedAflameRemovesBleedResist;
                                    traitBonus = 0.01f;
                                    break;
                                case 4:
                                    traitType = TraitType.AflameBleedDamageAmpOnDoubleThreshhold;
                                    traitBonus = 0.5f;
                                    break;
                            }
                            break;
                        case Item.AffinityType.Poison:
                            switch (Random.Range(0, 6))
                            {
                                case 0:
                                    traitType = TraitType.AflamePoisonBurningEnemySpreadPoisonStacksOnDeath;
                                    traitBonus = 0.5f;
                                    break;
                                case 1:
                                    traitType = TraitType.AflamePoisonGreviousWoundsOnStackThreshold;
                                    traitBonus = 4;
                                    break;
                                case 2:
                                    traitType = TraitType.AflamePoisonPoisonReducesFireResist;
                                    traitBonus = 0.01f;
                                    break;
                                case 3:
                                    traitType = TraitType.AflamePoisonFireSpellsSummonsPoisonBurst;
                                    traitBonus = 5f;
                                    break;
                                case 4:
                                    traitType = TraitType.AflamePoisonFireAmpsPoison;
                                    traitBonus = 0.01f;
                                    break;
                                case 5:
                                    traitType = TraitType.AflamePoisonPoisonCloudOnFireKill;
                                    traitBonus = 0.1f;
                                    break;
                            }
                            break;
                        case Item.AffinityType.Stun:
                            switch (Random.Range(0, 4))
                            {
                                case 0:
                                    traitType = TraitType.AflameStunPeriodBurnStun;
                                    traitBonus = 1f;
                                    break;
                                case 1:
                                    traitType = TraitType.AflameStunStunOnThreshold;
                                    traitBonus = 1f;
                                    break;
                                case 2:
                                    traitType = TraitType.AflameStunStunReducesFireResistance;
                                    traitBonus = 0.2f;
                                    break;
                                case 3:
                                    traitType = TraitType.AflameStunStunAmpsBurnDamage;
                                    traitBonus = 2f;
                                    break;
                            }
                            break;
                        case Item.AffinityType.Knockback:
                            switch (Random.Range(0, 3))
                            {
                                case 0:
                                    traitType = TraitType.AflameKnockbackAflameReducesKnockbackResist;
                                    traitBonus = 0.01f;
                                    break;
                                case 1:
                                    traitType = TraitType.AflameKnockbackAflameSpellsOnKnockbackedTargetExplode;
                                    traitBonus = 0.25f;
                                    break;
                                case 2:
                                    traitType = TraitType.AflameKnockbackKnockbackAmpsFireDamage;
                                    traitBonus = 0.25f;
                                    break;
                            }
                            break;
                        default:
                            break;
                    }
                    break;
                case Item.AffinityType.Ice:
                    switch (affinity)
                    {
                        case Item.AffinityType.Earth:
                            switch (Random.Range(0, 4))
                            {
                                case 0:
                                    traitType = TraitType.IceEarthFrostToEarthBonusDamage;
                                    traitBonus = 0.02f;
                                    break;
                                case 1:
                                    traitType = TraitType.IceEarthSunderAmpsIceDamage;
                                    traitBonus = 0.04f;
                                    break;
                                case 2:
                                    traitType = TraitType.IceEarthIceDOTAtThreshold;
                                    traitBonus = 0.25f;
                                    break;
                                case 3:
                                    traitType = TraitType.IceEarthEarthSpellBonusCritDamage;
                                    traitBonus = 0.025f;
                                    break;
                            }
                            break;
                        case Item.AffinityType.Wind:
                            switch (Random.Range(0, 4))
                            {
                                case 0:
                                    traitType = TraitType.IceWindWindAmpsFrostbiteDamage;
                                    traitBonus = 0.1f;
                                    break;
                                case 1:
                                    traitType = TraitType.IceWindWindSpellsDamageAmp;
                                    traitBonus = 0.03f;
                                    break;
                                case 2:
                                    traitType = TraitType.IceWindIncreaseArmorShredPerFrostbite;
                                    traitBonus = 0.025f;
                                    break;
                                case 3:
                                    traitType = TraitType.IceWindSummonTornadoOnHit;
                                    traitBonus = 0.25f;
                                    break;
                            }
                            break;
                        case Item.AffinityType.Physical:
                            switch (Random.Range(0, 3))
                            {
                                case 0:
                                    traitType = TraitType.IcePhysicalFrostbiteAmpsPhysicalCritDamage;
                                    traitBonus = 0.02f;
                                    break;
                                case 1:
                                    traitType = TraitType.IcePhysicalPhysicalVampOnFrostbite;
                                    traitBonus = 0.01f;
                                    break;
                                case 2:
                                    traitType = TraitType.IcePhysicalBladeVortexOnHit;
                                    traitBonus = 0.33f;
                                    break;
                            }
                            break;
                        case Item.AffinityType.Bleed:
                            switch (Random.Range(0, 2))
                            {
                                case 0:
                                    traitType = TraitType.IceBleedFrostbiteAmpsBleed;
                                    traitBonus = 0.02f;
                                    break;
                                case 1:
                                    traitType = TraitType.IceBleedBleedDoesDamageInstantlyOnThreshold;
                                    traitBonus = 3f;
                                    break;
                            }
                            break;
                        case Item.AffinityType.Poison:
                            switch (Random.Range(0, 3))
                            {
                                case 0:
                                    traitType = TraitType.IcePoisonFreezingPoison;
                                    traitBonus = 0.05f;
                                    break;
                                case 1:
                                    traitType = TraitType.IcePoisonFrostbiteResetsPoisonAndAmps;
                                    traitBonus = 1f;
                                    break;
                                case 2:
                                    traitType = TraitType.IcePoisonSummonPoisonPillarOnThreshold;
                                    traitBonus = 1f;
                                    break;
                            }
                            break;
                        case Item.AffinityType.Stun:
                            switch (Random.Range(0, 2))
                            {
                                case 0:
                                    traitType = TraitType.IceStunRudeAwakening;
                                    traitBonus = 1f;
                                    break;
                                case 1:
                                    traitType = TraitType.IceStunIceRefreshesStun;
                                    traitBonus = 0.25f;
                                    break;
                            }
                            break;
                        case Item.AffinityType.Knockback:
                            switch (Random.Range(0, 3))
                            {
                                case 0:
                                    traitType = TraitType.IceKnockbackFrostbiteIncreasesKnockbackForce;
                                    traitBonus = 0.05f;
                                    break;
                                case 1:
                                    traitType = TraitType.IceKnockbackSnowEruptionOnKnockback;
                                    traitBonus = 0.1f;
                                    break;
                                case 2:
                                    traitType = TraitType.IceKnockbackBonusStacksOnDownedTargets;
                                    traitBonus = 1f;
                                    break;
                            }
                            break;
                        default:
                            break;
                    }
                    break;
                case Item.AffinityType.Earth:
                    switch (affinity)
                    {
                        case Item.AffinityType.Fire:
                            switch (Random.Range(0, 3))
                            {
                                case 0:
                                    traitType = TraitType.AflameToSunderStackOnEarthSpell;
                                    traitBonus = 0.1f;
                                    break;
                                case 1:
                                    traitType = TraitType.SunderFurtherDecreasesFireResist;
                                    traitBonus = 1f;
                                    break;
                                case 2:
                                    traitType = TraitType.AflameSunderCritsSummonFireballs;
                                    traitBonus = 1f;
                                    break;
                            }
                            break;
                        case Item.AffinityType.Ice:
                            switch (Random.Range(0, 4))
                            {
                                case 0:
                                    traitType = TraitType.IceEarthFrostToEarthBonusDamage;
                                    traitBonus = 0.02f;
                                    break;
                                case 1:
                                    traitType = TraitType.IceEarthSunderAmpsIceDamage;
                                    traitBonus = 0.04f;
                                    break;
                                case 2:
                                    traitType = TraitType.IceEarthIceDOTAtThreshold;
                                    traitBonus = 0.25f;
                                    break;
                                case 3:
                                    traitType = TraitType.IceEarthEarthSpellBonusCritDamage;
                                    traitBonus = 0.025f;
                                    break;
                            }
                            break;
                        case Item.AffinityType.Physical:
                            switch (Random.Range(0, 3))
                            {
                                case 0:
                                    traitType = TraitType.EarthPhysicalBonusSunderStacksOnThreshold;
                                    traitBonus = 1f;
                                    break;
                                case 1:
                                    traitType = TraitType.EarthPhysicalSunderAmpsCrits;
                                    traitBonus = 0.02f;
                                    break;
                                case 2:
                                    traitType = TraitType.EarthPhysicalSunderAmpsDamage;
                                    traitBonus = 0.01f;
                                    break;
                            }
                            break;
                        case Item.AffinityType.Bleed:
                            switch (Random.Range(0, 4))
                            {
                                case 0:
                                    traitType = TraitType.EarthBleedBonusCritChanceOnBleedingTarget;
                                    traitBonus = 0.01f;
                                    break;
                                case 1:
                                    traitType = TraitType.EarthBleedSunderAddsPercentageOfBleed;
                                    traitBonus = 0.05f;
                                    break;
                                case 2:
                                    traitType = TraitType.EarthBleedBonusEarthDamageToBleeding;
                                    traitBonus = 0.03f;
                                    break;
                                case 3:
                                    traitType = TraitType.EarthBleedBloodExplosionOnBleed;
                                    traitBonus = 0.25f;
                                    break;
                            }
                            break;
                        case Item.AffinityType.Poison:
                            switch (Random.Range(0, 4))
                            {
                                case 0:
                                    traitType = TraitType.EarthPoisonAddSunderedOnPoisonTick;
                                    traitBonus = 0.05f;
                                    break;
                                case 1:
                                    traitType = TraitType.EarthPoisonSummonPillarOnThreshold;
                                    traitBonus = 1f;
                                    break;
                                case 2:
                                    traitType = TraitType.EarthPoisonSunderToPoisonConversion;
                                    traitBonus = 1f;
                                    break;
                                case 3:
                                    traitType = TraitType.EarthPoisonSunderToPoisonOnCrit;
                                    traitBonus = 1f;
                                    break;
                            }
                            break;
                        case Item.AffinityType.Stun:
                            switch (Random.Range(0, 5))
                            {
                                case 0:
                                    traitType = TraitType.EarthStunStunOnThreshold;
                                    traitBonus = 1f;
                                    break;
                                case 1:
                                    traitType = TraitType.EarthStunBonusDamageOnStun;
                                    traitBonus = 0.25f;
                                    break;
                                case 2:
                                    traitType = TraitType.EarthStunKillingStunnedWithEarthRefundsCooldowns;
                                    traitBonus = 0.05f;
                                    break;
                                case 3:
                                    traitType = TraitType.EarthStunStunningAddsSunder;
                                    traitBonus = 1f;
                                    break;
                                case 4:
                                    traitType = TraitType.EarthStunSunderAmpsStunDamageLength;
                                    traitBonus = 0.01f;
                                    break;
                            }
                            break;
                        case Item.AffinityType.Knockback:
                            switch (Random.Range(0, 3))
                            {
                                case 0:
                                    traitType = TraitType.EarthKnockbackTremorsOnKnockback;
                                    traitBonus = 0.1f;
                                    break;
                                case 1:
                                    traitType = TraitType.EarthKnockbackSunderReducesKnockbackResistance;
                                    traitBonus = 2f;
                                    break;
                                case 2:
                                    traitType = TraitType.EarthKnockbackSummonRocksOnRecentKnockbackTarget;
                                    traitBonus = 1f;
                                    break;
                            }
                            break;
                        default:
                            break;
                    }
                    break;
                case Item.AffinityType.Wind:
                    switch (affinity)
                    {
                        case Item.AffinityType.Fire:
                            switch (Random.Range(0, 3))
                            {
                                case 0:
                                    traitType = TraitType.AflameWindshearWindAttacksGainCritOnBurningTarget;
                                    traitBonus = 0.02f;
                                    break;
                                case 1:
                                    traitType = TraitType.AflameWindshearSummonFirePillarsOnHit;
                                    traitBonus = 1f;
                                    break;
                                case 2:
                                    traitType = TraitType.AflameWindshearWindSpellsAddFireStacks;
                                    traitBonus = 0.2f;
                                    break;
                            }
                            break;
                        case Item.AffinityType.Ice:
                            switch (Random.Range(0, 4))
                            {
                                case 0:
                                    traitType = TraitType.IceWindWindAmpsFrostbiteDamage;
                                    traitBonus = 0.1f;
                                    break;
                                case 1:
                                    traitType = TraitType.IceWindWindSpellsDamageAmp;
                                    traitBonus = 0.03f;
                                    break;
                                case 2:
                                    traitType = TraitType.IceWindIncreaseArmorShredPerFrostbite;
                                    traitBonus = 0.025f;
                                    break;
                                case 3:
                                    traitType = TraitType.IceWindSummonTornadoOnHit;
                                    traitBonus = 0.25f;
                                    break;
                            }
                            break;
                        case Item.AffinityType.Physical:
                            switch (Random.Range(0, 3))
                            {
                                case 0:
                                    traitType = TraitType.WindPhysicalSummonWhirlwindOnSkillHit;
                                    traitBonus = 0.4f;
                                    break;
                                case 1:
                                    traitType = TraitType.WindPhysicalWindshearAmpsBasicAttacks;
                                    traitBonus = 0.03f;
                                    break;
                                case 2:
                                    traitType = TraitType.WindPhysicalCritsDealArmorAsDamage;
                                    traitBonus = 0.05f;
                                    break;
                            }
                            break;
                        case Item.AffinityType.Bleed:
                            switch (Random.Range(0, 4))
                            {
                                case 0:
                                    traitType = TraitType.WindBleedAmpBleedAtThreshold;
                                    traitBonus = 0.33f;
                                    break;
                                case 1:
                                    traitType = TraitType.WindBleedMoreBleedStacksAThreshold;
                                    traitBonus = 0.5f;
                                    break;
                                case 2:
                                    traitType = TraitType.WindBleedBleedGrantsWindCritChance;
                                    traitBonus = 0.01f;
                                    break;
                                case 3:
                                    traitType = TraitType.WindBleedAddBleedOnWindCrit;
                                    traitBonus = 2f;
                                    break;
                            }
                            break;
                        case Item.AffinityType.Poison:
                            switch (Random.Range(0, 3))
                            {
                                case 0:
                                    traitType = TraitType.WindPoisonTransferPoisonStacksOnKill;
                                    traitBonus = 2f;
                                    break;
                                case 1:
                                    traitType = TraitType.WindPoisonWindAddsPercentageOfPoisonOnHit;
                                    traitBonus = 0.05f;
                                    break;
                                case 2:
                                    traitType = TraitType.WindPoisonPoisonBurstAtWindThreshold;
                                    traitBonus = 1f;
                                    break;
                            }
                            break;
                        case Item.AffinityType.Stun:
                            switch (Random.Range(0, 3))
                            {
                                case 0:
                                    traitType = TraitType.WindStunStunDealsTrueDamageAtThreshold;
                                    traitBonus = 1f;
                                    break;
                                case 1:
                                    traitType = TraitType.WindStunWindblastOnStun;
                                    traitBonus = 1f;
                                    break;
                                case 2:
                                    traitType = TraitType.WindStunStunAmpsWindshearGain;
                                    traitBonus = 1f;
                                    break;
                            }
                            break;
                        case Item.AffinityType.Knockback:
                            switch (Random.Range(0, 3))
                            {
                                case 0:
                                    traitType = TraitType.WindKnockbackKnockbackSummonsMiniCyclone;
                                    traitBonus = 0.33f;
                                    break;
                                case 1:
                                    traitType = TraitType.WindKnockbackLoseKnockbackResistanceOnThreshold;
                                    traitBonus = 0.25f;
                                    break;
                                case 2:
                                    traitType = TraitType.WindKnockbackWindshearDoesDamageIfKnockedBack;
                                    traitBonus = 0.2f;
                                    break;
                            }
                            break;
                        default:
                            break;
                    }
                    break;
                case Item.AffinityType.Physical:
                    switch (affinity)
                    {
                        case Item.AffinityType.Fire:
                            switch (Random.Range(0, 4))
                            {
                                case 0:
                                    traitType = TraitType.AflamePhysicalAddFireStacksOnHit;
                                    traitBonus = 1f;
                                    break;
                                case 1:
                                    traitType = TraitType.AflamePhysicalDamageAmpOnBurningTarget;
                                    traitBonus = 0.05f;
                                    break;
                                case 2:
                                    traitType = TraitType.AflamePhysicalBladeExplosionOnKill;
                                    traitBonus = 1f;
                                    break;
                                case 3:
                                    traitType = TraitType.AflamePhysicalBigHitsAddAflame;
                                    traitBonus = 0.5f;
                                    break;
                            }
                            break;
                        case Item.AffinityType.Ice:
                            switch (Random.Range(0, 3))
                            {
                                case 0:
                                    traitType = TraitType.IcePhysicalFrostbiteAmpsPhysicalCritDamage;
                                    traitBonus = 0.02f;
                                    break;
                                case 1:
                                    traitType = TraitType.IcePhysicalPhysicalVampOnFrostbite;
                                    traitBonus = 0.01f;
                                    break;
                                case 2:
                                    traitType = TraitType.IcePhysicalBladeVortexOnHit;
                                    traitBonus = 0.33f;
                                    break;
                            }
                            break;
                        case Item.AffinityType.Earth:
                            switch (Random.Range(0, 3))
                            {
                                case 0:
                                    traitType = TraitType.EarthPhysicalBonusSunderStacksOnThreshold;
                                    traitBonus = 1f;
                                    break;
                                case 1:
                                    traitType = TraitType.EarthPhysicalSunderAmpsCrits;
                                    traitBonus = 0.02f;
                                    break;
                                case 2:
                                    traitType = TraitType.EarthPhysicalSunderAmpsDamage;
                                    traitBonus = 0.01f;
                                    break;
                            }
                            break;
                        case Item.AffinityType.Wind:
                            switch (Random.Range(0, 3))
                            {
                                case 0:
                                    traitType = TraitType.WindPhysicalSummonWhirlwindOnSkillHit;
                                    traitBonus = 0.4f;
                                    break;
                                case 1:
                                    traitType = TraitType.WindPhysicalWindshearAmpsBasicAttacks;
                                    traitBonus = 0.03f;
                                    break;
                                case 2:
                                    traitType = TraitType.WindPhysicalCritsDealArmorAsDamage;
                                    traitBonus = 0.05f;
                                    break;
                            }
                            break;
                        case Item.AffinityType.Bleed:
                            switch (Random.Range(0, 3))
                            {
                                case 0:
                                    traitType = TraitType.PhysicalBleedBleedAmpsPhysicalDamage;
                                    traitBonus = 0.01f;
                                    break;
                                case 1:
                                    traitType = TraitType.PhysicalBleedPhysicalSkillsAddBleed;
                                    traitBonus = 1f;
                                    break;
                                case 2:
                                    traitType = TraitType.PhysicalBleedSkillsDoTrueDamageAtThreshold;
                                    traitBonus = 4f;
                                    break;
                            }
                            break;
                        case Item.AffinityType.Poison:
                            switch (Random.Range(0, 3))
                            {
                                case 0:
                                    traitType = TraitType.PhysicalPoisonPhysicalAmpsPoisonDamage;
                                    traitBonus = 3f;
                                    break;
                                case 1:
                                    traitType = TraitType.PhysicalPoisonPlayerMaxHpDamageOnThreshold;
                                    traitBonus = 0.05f;
                                    break;
                                case 2:
                                    traitType = TraitType.PhysicalPoisonPoisonAmpsPhysicalDamage;
                                    traitBonus = 3f;
                                    break;
                            }
                            break;
                        case Item.AffinityType.Stun:
                            switch (Random.Range(0, 2))
                            {
                                case 0:
                                    traitType = TraitType.PhysicalStunAmpDamageOnStunned;
                                    traitBonus = 1f;
                                    break;
                                case 1:
                                    traitType = TraitType.PhysicalStunBladeRiftOnStun;
                                    traitBonus = 0.25f;
                                    break;
                            }
                            break;
                        case Item.AffinityType.Knockback:
                            switch (Random.Range(0, 3))
                            {
                                case 0:
                                    traitType = TraitType.PhysicalKnockbackKnockbackKillAmpsPhysicalDamage;
                                    traitBonus = 3f;
                                    break;
                                case 1:
                                    traitType = TraitType.PhysicalKnockbackPhysicalAttacksGainInnateKnockback;
                                    traitBonus = 0.02f;
                                    break;
                                case 2:
                                    traitType = TraitType.PhysicalKnockbackSummonKnivesOnKnockbackHit;
                                    traitBonus = 2f;
                                    break;
                            }
                            break;
                        default:
                            break;
                    }
                    break;
                case Item.AffinityType.Bleed:
                    switch (affinity)
                    {
                        case Item.AffinityType.Fire:
                            switch (Random.Range(0, 5))
                            {
                                case 0:
                                    traitType = TraitType.AflameBleedIncreasesFlameCritChance;
                                    traitBonus = 0.03f;
                                    break;
                                case 1:
                                    traitType = TraitType.AflameBleedFireDamageAmpOnBleedThreshold;
                                    traitBonus = 0.1f;
                                    break;
                                case 2:
                                    traitType = TraitType.AflameBleedAflameAddsBleedAtThreshhold;
                                    traitBonus = 2f;
                                    break;
                                case 3:
                                    traitType = TraitType.AflameBleedAflameRemovesBleedResist;
                                    traitBonus = 0.01f;
                                    break;
                                case 4:
                                    traitType = TraitType.AflameBleedDamageAmpOnDoubleThreshhold;
                                    traitBonus = 0.5f;
                                    break;
                            }
                            break;
                        case Item.AffinityType.Ice:
                            switch (Random.Range(0, 2))
                            {
                                case 0:
                                    traitType = TraitType.IceBleedFrostbiteAmpsBleed;
                                    traitBonus = 0.02f;
                                    break;
                                case 1:
                                    traitType = TraitType.IceBleedBleedDoesDamageInstantlyOnThreshold;
                                    traitBonus = 3f;
                                    break;
                            }
                            break;
                        case Item.AffinityType.Earth:
                            switch (Random.Range(0, 4))
                            {
                                case 0:
                                    traitType = TraitType.EarthBleedBonusCritChanceOnBleedingTarget;
                                    traitBonus = 0.01f;
                                    break;
                                case 1:
                                    traitType = TraitType.EarthBleedSunderAddsPercentageOfBleed;
                                    traitBonus = 0.05f;
                                    break;
                                case 2:
                                    traitType = TraitType.EarthBleedBonusEarthDamageToBleeding;
                                    traitBonus = 0.03f;
                                    break;
                                case 3:
                                    traitType = TraitType.EarthBleedBloodExplosionOnBleed;
                                    traitBonus = 0.25f;
                                    break;
                            }
                            break;
                        case Item.AffinityType.Wind:
                            switch (Random.Range(0, 4))
                            {
                                case 0:
                                    traitType = TraitType.WindBleedAmpBleedAtThreshold;
                                    traitBonus = 0.33f;
                                    break;
                                case 1:
                                    traitType = TraitType.WindBleedMoreBleedStacksAThreshold;
                                    traitBonus = 0.5f;
                                    break;
                                case 2:
                                    traitType = TraitType.WindBleedBleedGrantsWindCritChance;
                                    traitBonus = 0.01f;
                                    break;
                                case 3:
                                    traitType = TraitType.WindBleedAddBleedOnWindCrit;
                                    traitBonus = 2f;
                                    break;
                            }
                            break;
                        case Item.AffinityType.Physical:
                            switch (Random.Range(0, 3))
                            {
                                case 0:
                                    traitType = TraitType.PhysicalBleedBleedAmpsPhysicalDamage;
                                    traitBonus = 0.01f;
                                    break;
                                case 1:
                                    traitType = TraitType.PhysicalBleedPhysicalSkillsAddBleed;
                                    traitBonus = 1f;
                                    break;
                                case 2:
                                    traitType = TraitType.PhysicalBleedSkillsDoTrueDamageAtThreshold;
                                    traitBonus = 4f;
                                    break;
                            }
                            break;
                        case Item.AffinityType.Poison:
                            switch (Random.Range(0, 2))
                            {
                                case 0:
                                    traitType = TraitType.BleedPoisonReduceOtherResistances;
                                    traitBonus = 0.01f;
                                    break;
                                case 1:
                                    traitType = TraitType.BleedPoisonChanceOfPoisonCloudOnBleed;
                                    traitBonus = 2f;
                                    break;
                            }
                            break;
                        case Item.AffinityType.Stun:
                            switch (Random.Range(0, 3))
                            {
                                case 0:
                                    traitType = TraitType.BleedStunStunReducesBleedResistance;
                                    traitBonus = 0.2f;
                                    break;
                                case 1:
                                    traitType = TraitType.BleedStunStunAtThresholdBelowHalfHP;
                                    traitBonus = 1f;
                                    break;
                                case 2:
                                    traitType = TraitType.BleedStunStunAddsBleed;
                                    traitBonus = 2f;
                                    break;
                            }
                            break;
                        case Item.AffinityType.Knockback:
                            switch (Random.Range(0, 3))
                            {
                                case 0:
                                    traitType = TraitType.BleedKnockbackBonusBleed;
                                    traitBonus = 1f;
                                    break;
                                case 1:
                                    traitType = TraitType.BleedKnockbackKnockbackExposionOfBlood;
                                    traitBonus = 1.5f;
                                    break;
                                case 2:
                                    traitType = TraitType.BleedKnockbackKnockbackAmpsDamage;
                                    traitBonus = 0.05f;
                                    break;
                            }
                            break;
                        default:
                            break;
                    }
                    break;
                case Item.AffinityType.Poison:
                    switch (affinity)
                    {
                        case Item.AffinityType.Fire:
                            switch (Random.Range(0, 6))
                            {
                                case 0:
                                    traitType = TraitType.AflamePoisonBurningEnemySpreadPoisonStacksOnDeath;
                                    traitBonus = 0.5f;
                                    break;
                                case 1:
                                    traitType = TraitType.AflamePoisonGreviousWoundsOnStackThreshold;
                                    traitBonus = 4;
                                    break;
                                case 2:
                                    traitType = TraitType.AflamePoisonPoisonReducesFireResist;
                                    traitBonus = 0.01f;
                                    break;
                                case 3:
                                    traitType = TraitType.AflamePoisonFireSpellsSummonsPoisonBurst;
                                    traitBonus = 5f;
                                    break;
                                case 4:
                                    traitType = TraitType.AflamePoisonFireAmpsPoison;
                                    traitBonus = 0.01f;
                                    break;
                                case 5:
                                    traitType = TraitType.AflamePoisonPoisonCloudOnFireKill;
                                    traitBonus = 0.1f;
                                    break;
                            }
                            break;
                        case Item.AffinityType.Ice:
                            switch (Random.Range(0, 3))
                            {
                                case 0:
                                    traitType = TraitType.IcePoisonFreezingPoison;
                                    traitBonus = 0.05f;
                                    break;
                                case 1:
                                    traitType = TraitType.IcePoisonFrostbiteResetsPoisonAndAmps;
                                    traitBonus = 1f;
                                    break;
                                case 2:
                                    traitType = TraitType.IcePoisonSummonPoisonPillarOnThreshold;
                                    traitBonus = 1f;
                                    break;
                            }
                            break;
                        case Item.AffinityType.Earth:
                            switch (Random.Range(0, 4))
                            {
                                case 0:
                                    traitType = TraitType.EarthPoisonAddSunderedOnPoisonTick;
                                    traitBonus = 0.05f;
                                    break;
                                case 1:
                                    traitType = TraitType.EarthPoisonSummonPillarOnThreshold;
                                    traitBonus = 1f;
                                    break;
                                case 2:
                                    traitType = TraitType.EarthPoisonSunderToPoisonConversion;
                                    traitBonus = 1f;
                                    break;
                                case 3:
                                    traitType = TraitType.EarthPoisonSunderToPoisonOnCrit;
                                    traitBonus = 1f;
                                    break;
                            }
                            break;
                        case Item.AffinityType.Wind:
                            switch (Random.Range(0, 3))
                            {
                                case 0:
                                    traitType = TraitType.WindPoisonTransferPoisonStacksOnKill;
                                    traitBonus = 2f;
                                    break;
                                case 1:
                                    traitType = TraitType.WindPoisonWindAddsPercentageOfPoisonOnHit;
                                    traitBonus = 0.05f;
                                    break;
                                case 2:
                                    traitType = TraitType.WindPoisonPoisonBurstAtWindThreshold;
                                    traitBonus = 1f;
                                    break;
                            }
                            break;
                        case Item.AffinityType.Physical:
                            switch (Random.Range(0, 3))
                            {
                                case 0:
                                    traitType = TraitType.PhysicalPoisonPhysicalAmpsPoisonDamage;
                                    traitBonus = 3f;
                                    break;
                                case 1:
                                    traitType = TraitType.PhysicalPoisonPlayerMaxHpDamageOnThreshold;
                                    traitBonus = 0.05f;
                                    break;
                                case 2:
                                    traitType = TraitType.PhysicalPoisonPoisonAmpsPhysicalDamage;
                                    traitBonus = 3f;
                                    break;
                            }
                            break;
                        case Item.AffinityType.Bleed:
                            switch (Random.Range(0, 2))
                            {
                                case 0:
                                    traitType = TraitType.BleedPoisonReduceOtherResistances;
                                    traitBonus = 0.01f;
                                    break;
                                case 1:
                                    traitType = TraitType.BleedPoisonChanceOfPoisonCloudOnBleed;
                                    traitBonus = 2f;
                                    break;
                            }
                            break;
                        case Item.AffinityType.Stun:
                            switch (Random.Range(0, 2))
                            {
                                case 0:
                                    traitType = TraitType.PoisonStunPoisonReducesStunResist;
                                    traitBonus = 0.02f;
                                    break;
                                case 1:
                                    traitType = TraitType.PoisonStunPoisonSpreadOnThreshold;
                                    traitBonus = 0.25f;
                                    break;
                            }
                            break;
                        case Item.AffinityType.Knockback:
                            switch (Random.Range(0, 2))
                            {
                                case 0:
                                    traitType = TraitType.PoisonKnockbackPoisonReducesKnockbackResistance;
                                    traitBonus = 0.02f;
                                    break;
                                case 1:
                                    traitType = TraitType.PoisonKnockbackConsumePoisonStacksForTrueDamage;
                                    traitBonus = 0.5f;
                                    break;
                            }
                            break;
                        default:
                            break;
                    }
                    break;
                case Item.AffinityType.Stun:
                    switch (affinity)
                    {
                        case Item.AffinityType.Fire:
                            switch (Random.Range(0, 4))
                            {
                                case 0:
                                    traitType = TraitType.AflameStunPeriodBurnStun;
                                    traitBonus = 1f;
                                    break;
                                case 1:
                                    traitType = TraitType.AflameStunStunOnThreshold;
                                    traitBonus = 1f;
                                    break;
                                case 2:
                                    traitType = TraitType.AflameStunStunReducesFireResistance;
                                    traitBonus = 0.2f;
                                    break;
                                case 3:
                                    traitType = TraitType.AflameStunStunAmpsBurnDamage;
                                    traitBonus = 2f;
                                    break;
                            }
                            break;
                        case Item.AffinityType.Ice:
                            switch (Random.Range(0, 2))
                            {
                                case 0:
                                    traitType = TraitType.IceStunRudeAwakening;
                                    traitBonus = 1f;
                                    break;
                                case 1:
                                    traitType = TraitType.IceStunIceRefreshesStun;
                                    traitBonus = 0.25f;
                                    break;
                            }
                            break;
                        case Item.AffinityType.Earth:
                            switch (Random.Range(0, 5))
                            {
                                case 0:
                                    traitType = TraitType.EarthStunStunOnThreshold;
                                    traitBonus = 1f;
                                    break;
                                case 1:
                                    traitType = TraitType.EarthStunBonusDamageOnStun;
                                    traitBonus = 0.25f;
                                    break;
                                case 2:
                                    traitType = TraitType.EarthStunKillingStunnedWithEarthRefundsCooldowns;
                                    traitBonus = 0.05f;
                                    break;
                                case 3:
                                    traitType = TraitType.EarthStunStunningAddsSunder;
                                    traitBonus = 1f;
                                    break;
                                case 4:
                                    traitType = TraitType.EarthStunSunderAmpsStunDamageLength;
                                    traitBonus = 0.01f;
                                    break;
                            }
                            break;
                        case Item.AffinityType.Wind:
                            switch (Random.Range(0, 3))
                            {
                                case 0:
                                    traitType = TraitType.WindStunStunDealsTrueDamageAtThreshold;
                                    traitBonus = 1f;
                                    break;
                                case 1:
                                    traitType = TraitType.WindStunWindblastOnStun;
                                    traitBonus = 1f;
                                    break;
                                case 2:
                                    traitType = TraitType.WindStunStunAmpsWindshearGain;
                                    traitBonus = 1f;
                                    break;
                            }
                            break;
                        case Item.AffinityType.Physical:
                            switch (Random.Range(0, 2))
                            {
                                case 0:
                                    traitType = TraitType.PhysicalStunAmpDamageOnStunned;
                                    traitBonus = 1f;
                                    break;
                                case 1:
                                    traitType = TraitType.PhysicalStunBladeRiftOnStun;
                                    traitBonus = 0.25f;
                                    break;
                            }
                            break;
                        case Item.AffinityType.Bleed:
                            switch (Random.Range(0, 3))
                            {
                                case 0:
                                    traitType = TraitType.BleedStunStunReducesBleedResistance;
                                    traitBonus = 0.2f;
                                    break;
                                case 1:
                                    traitType = TraitType.BleedStunStunAtThresholdBelowHalfHP;
                                    traitBonus = 1f;
                                    break;
                                case 2:
                                    traitType = TraitType.BleedStunStunAddsBleed;
                                    traitBonus = 2f;
                                    break;
                            }
                            break;
                        case Item.AffinityType.Poison:
                            switch (Random.Range(0, 2))
                            {
                                case 0:
                                    traitType = TraitType.PoisonStunPoisonReducesStunResist;
                                    traitBonus = 0.02f;
                                    break;
                                case 1:
                                    traitType = TraitType.PoisonStunPoisonSpreadOnThreshold;
                                    traitBonus = 0.25f;
                                    break;
                            }
                            break;
                        case Item.AffinityType.Knockback:
                            switch (Random.Range(0, 1))
                            {
                                case 0:
                                    traitType = TraitType.StunKnockbackStunReduceResistance;
                                    traitBonus = 0.5f;
                                    break;
                            }
                            break;
                        default:
                            break;
                    }
                    break;
                case Item.AffinityType.Knockback:
                    switch (affinity)
                    {
                        case Item.AffinityType.Fire:
                            switch (Random.Range(0, 3))
                            {
                                case 0:
                                    traitType = TraitType.AflameKnockbackAflameReducesKnockbackResist;
                                    traitBonus = 0.01f;
                                    break;
                                case 1:
                                    traitType = TraitType.AflameKnockbackAflameSpellsOnKnockbackedTargetExplode;
                                    traitBonus = 0.25f;
                                    break;
                                case 2:
                                    traitType = TraitType.AflameKnockbackKnockbackAmpsFireDamage;
                                    traitBonus = 0.25f;
                                    break;
                            }
                            break;
                        case Item.AffinityType.Ice:
                            switch (Random.Range(0, 3))
                            {
                                case 0:
                                    traitType = TraitType.IceKnockbackFrostbiteIncreasesKnockbackForce;
                                    traitBonus = 0.05f;
                                    break;
                                case 1:
                                    traitType = TraitType.IceKnockbackSnowEruptionOnKnockback;
                                    traitBonus = 0.1f;
                                    break;
                                case 2:
                                    traitType = TraitType.IceKnockbackBonusStacksOnDownedTargets;
                                    traitBonus = 1f;
                                    break;
                            }
                            break;
                        case Item.AffinityType.Earth:
                            switch (Random.Range(0, 3))
                            {
                                case 0:
                                    traitType = TraitType.EarthKnockbackTremorsOnKnockback;
                                    traitBonus = 0.1f;
                                    break;
                                case 1:
                                    traitType = TraitType.EarthKnockbackSunderReducesKnockbackResistance;
                                    traitBonus = 2f;
                                    break;
                                case 2:
                                    traitType = TraitType.EarthKnockbackSummonRocksOnRecentKnockbackTarget;
                                    traitBonus = 1f;
                                    break;
                            }
                            break;
                        case Item.AffinityType.Wind:
                            switch (Random.Range(0, 3))
                            {
                                case 0:
                                    traitType = TraitType.WindKnockbackKnockbackSummonsMiniCyclone;
                                    traitBonus = 0.33f;
                                    break;
                                case 1:
                                    traitType = TraitType.WindKnockbackLoseKnockbackResistanceOnThreshold;
                                    traitBonus = 0.25f;
                                    break;
                                case 2:
                                    traitType = TraitType.WindKnockbackWindshearDoesDamageIfKnockedBack;
                                    traitBonus = 0.2f;
                                    break;
                            }
                            break;
                        case Item.AffinityType.Physical:
                            switch (Random.Range(0, 3))
                            {
                                case 0:
                                    traitType = TraitType.PhysicalKnockbackKnockbackKillAmpsPhysicalDamage;
                                    traitBonus = 3f;
                                    break;
                                case 1:
                                    traitType = TraitType.PhysicalKnockbackPhysicalAttacksGainInnateKnockback;
                                    traitBonus = 0.02f;
                                    break;
                                case 2:
                                    traitType = TraitType.PhysicalKnockbackSummonKnivesOnKnockbackHit;
                                    traitBonus = 2f;
                                    break;
                            }
                            break;
                        case Item.AffinityType.Bleed:
                            switch (Random.Range(0, 3))
                            {
                                case 0:
                                    traitType = TraitType.BleedKnockbackBonusBleed;
                                    traitBonus = 1f;
                                    break;
                                case 1:
                                    traitType = TraitType.BleedKnockbackKnockbackExposionOfBlood;
                                    traitBonus = 1.5f;
                                    break;
                                case 2:
                                    traitType = TraitType.BleedKnockbackKnockbackAmpsDamage;
                                    traitBonus = 0.05f;
                                    break;
                            }
                            break;
                        case Item.AffinityType.Poison:
                            switch (Random.Range(0, 2))
                            {
                                case 0:
                                    traitType = TraitType.PoisonKnockbackPoisonReducesKnockbackResistance;
                                    traitBonus = 0.02f;
                                    break;
                                case 1:
                                    traitType = TraitType.PoisonKnockbackConsumePoisonStacksForTrueDamage;
                                    traitBonus = 0.5f;
                                    break;
                            }
                            break;
                        case Item.AffinityType.Stun:
                            switch (Random.Range(0, 1))
                            {
                                case 0:
                                    traitType = TraitType.StunKnockbackStunReduceResistance;
                                    traitBonus = 0.5f;
                                    break;
                            }
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

}
