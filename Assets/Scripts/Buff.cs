using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviour
{
    public BuffsManager.BuffType myType;

    public List<ParticleSystem> effectParticleSystem = new List<ParticleSystem>();
    public List<ParticleSystem> endOfBuffParticleSystem = new List<ParticleSystem>();
    public PlayerStats connectedPlayer;
    public PlayerStats playerDamageSource;
    public GameObject connectedIcon;
    public UnityEngine.UI.Text iconStacks;
    public HitBox.DamageType damageType;
    public bool onHitEffect = false;

    public bool infiniteDuration = false;
    public float duration = 10f;
    public float currentTimer = 0f;
    public float DPS = 0f;
    public float bonusDPS = 0f;
    public float DPSMultiplier = 1;
    public float onHitDamageAmount = 0;

    public bool stackable = false;
    public float currentStacks = 0;
    public float maxStacks = 1;
    public bool stackSingleFalloff = false;
    public float stackfalloffTime = 0.5f;
    public float stacktargetTimer = 0f;

    public float healthSC = 0;
    public float healthRegenSC = 0;
    public float armorSC = 0;
    public float damageReductionSC = 0;
    public float healingMultiplierSC = 0;

    public float armorSCMultiplier = 1;

    public float atkSpdSC = 0;
    public float movespeedSC = 0;
    public float damagePercentageSC = 0;
    public float critChanceSC = 0;
    public float critDamageSC = 0;

    public float aflameResistSC = 0;
    public float frostbiteResistSC = 0;
    public float overchargeResistSC = 0;
    public float overgrowthResistSC = 0;
    public float sunderResistSC = 0;
    public float windshearResistSC = 0;
    public float asleepResistSC = 0;
    public float stunResistSC = 0;
    public float knockBackResistSC = 0;
    public float bleedResistSC = 0;
    public float poisonResistSC = 0;

    public float invulnerabilitySC = 0;
    public float untargetabilitySC = 0;
    public float invisibilitySC = 0;

    public float targetDamageTickTimer = 0.5f;
    public float currentDamageTick = 0;
    public bool almostDone = false;

    public float sizeSC = 0;


    private void Update()
    {
        // If this is not an infinite duration buff, count it down.
        if (!infiniteDuration)
        {
            currentTimer += Time.deltaTime;
            if (!almostDone && currentTimer > duration - 4)
            {
                almostDone = true;
                connectedIcon.GetComponent<Animator>().SetBool("AlmostDone", true);
            }
            if (currentTimer > duration)
            {
                if (stackable && stackSingleFalloff)
                    RemoveStacks(1, true);
                else
                    EndBuff();
            }
        }

        if (DPS != 0)
        {
            currentDamageTick += Time.deltaTime;
            if (currentDamageTick > targetDamageTickTimer)
            {
                currentDamageTick -= targetDamageTickTimer;
                // if(damageType == null)
                //connectedPlayer.TakeDamage(DPS * targetDamageTickTimer, false, damageColor);
                //else
                if (!stackable)
                    connectedPlayer.TakeDamage((DPS * targetDamageTickTimer + bonusDPS) * DPSMultiplier, false, damageType, 0, playerDamageSource, true);
                else if (myType != BuffsManager.BuffType.Poisoned)
                    connectedPlayer.TakeDamage((DPS * currentStacks * targetDamageTickTimer + bonusDPS) * DPSMultiplier, false, damageType, 0, playerDamageSource, true);
                else
                {
                    if(playerDamageSource.CompareTag("Player") && playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.PoisonTrueDamageAtThreshold) > 0 && connectedPlayer.health / connectedPlayer.healthMax <= 0.225f + playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.PoisonTrueDamageAtThreshold))
                        connectedPlayer.TakeDamage((DPS * currentStacks * targetDamageTickTimer + bonusDPS) * DPSMultiplier, false, HitBox.DamageType.True, 0, playerDamageSource, true);
                    else
                        connectedPlayer.TakeDamage((DPS * currentStacks * targetDamageTickTimer + bonusDPS) * DPSMultiplier, false, damageType, 0, playerDamageSource, true);
                }

                if (myType == BuffsManager.BuffType.Aflame && playerDamageSource.CompareTag("Player") && playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.FlameVamperism) > 0 && currentStacks >= 10)
                    playerDamageSource.HealHealth(playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.FlameVamperism) * (int)(currentStacks / 10), HitBox.DamageType.Healing);
                if (myType == BuffsManager.BuffType.Poisoned && playerDamageSource.CompareTag("Player") && playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.PoisonVamperism) > 0 )
                    playerDamageSource.HealHealth(playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.PoisonVamperism) * (DPS * currentStacks * targetDamageTickTimer + bonusDPS) * DPSMultiplier, HitBox.DamageType.Healing);

                float randomChance = Random.Range(0f, 1f); 
                if (myType == BuffsManager.BuffType.Poisoned && playerDamageSource.CompareTag("Player") && randomChance < playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.IcePoisonFreezingPoison))
                    connectedPlayer.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Frostbite, 1, playerDamageSource.baseDamage, playerDamageSource);
                if (myType == BuffsManager.BuffType.Poisoned && playerDamageSource.CompareTag("Player") && randomChance < playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.EarthPoisonAddSunderedOnPoisonTick))
                    connectedPlayer.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Sunder, 1, playerDamageSource.baseDamage, playerDamageSource);


            }
        }

        if (playerDamageSource && playerDamageSource.CompareTag("Player") && myType == BuffsManager.BuffType.Aflame && playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.AflameStunPeriodBurnStun) > 0 && connectedPlayer.traitAflameStunsPeriodicallyReady)
        {
            connectedPlayer.traitAflameStunsPeriodicallyReady = false;
            connectedPlayer.traitAflameStunPeriodicallyTargetTimer = 21 - playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.AflameStunPeriodBurnStun);
            connectedPlayer.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Stunned, 1, playerDamageSource.baseDamage, playerDamageSource);
        }
    }

    // Used to add or reset the buff's timer.
    public void AddTime(float amount, bool reset)
    {
        if (reset)
            currentTimer = 0;
        else
            currentTimer -= amount;

        if (currentTimer < duration - 4)
        {
            almostDone = false;
            connectedIcon.GetComponent<Animator>().SetBool("AlmostDone", false);
        }

        // reset one time aprticle effects of the target.
        if (reset)
        {
            switch (myType)
            {
                case BuffsManager.BuffType.Frozen:
                    foreach (ParticleSystem ps in effectParticleSystem)
                        ps.Stop();
                    foreach (ParticleSystem ps in effectParticleSystem)
                        ps.Play();
                    break;
                case BuffsManager.BuffType.Asleep:
                    foreach (ParticleSystem ps in effectParticleSystem)
                        ps.Stop();
                    foreach (ParticleSystem ps in effectParticleSystem)
                        ps.Play();
                    break;
                default:
                    break;
            }
        }
    }

    // Used to add stacks and stats per stat to the target.
    public void AddStack(float amount)
    {
        if (currentStacks < maxStacks)
        {
            // Add the required stacks, check to see if we exceed the max stacks.
            if (currentStacks + amount <= maxStacks)
                currentStacks += amount;
            else
            {
                //amount = maxStacks = currentStacks;
                currentStacks = maxStacks;
            }
        }
        else
        {
            amount = 0;
        }

        connectedIcon.GetComponent<Animator>().SetBool("AlmostDone", false);

        // Setting the timer.
        currentTimer = 0;
        if (stackSingleFalloff && stacktargetTimer != duration)
            duration = stacktargetTimer;

        // Changing the text on the buff icon.
        iconStacks.text = string.Format("x{0:0}", currentStacks);

        // Adding the stat change.
        if (amount != 0)
        {
            // If we changed the defensive stats, add more stacks
            if (healthSC != 0 || healthRegenSC != 0 || armorSC != 0 || damageReductionSC != 0 || healingMultiplierSC != 0)
                ChangeDefensiveStats(false, healthSC * amount, healthRegenSC * amount, armorSC * amount, damageReductionSC * amount, healingMultiplierSC * amount);

            // If we changed offensive stats, add more stacks
            if (atkSpdSC != 0 || movespeedSC != 0 || damagePercentageSC != 0 || critChanceSC != 0 || critDamageSC != 0)
                ChangeOffensiveStats(false, atkSpdSC * amount, movespeedSC * amount, damagePercentageSC * amount, critChanceSC * amount, critDamageSC * amount);

            // If we changed our resistance based stats, remove more stacks
            if (aflameResistSC != 0 || stunResistSC != 0 || asleepResistSC != 0 || bleedResistSC != 0 || poisonResistSC != 0 || frostbiteResistSC != 0 || knockBackResistSC != 0 || overchargeResistSC != 0 || overgrowthResistSC != 0 || sunderResistSC != 0 || windshearResistSC != 0)
                ChangeResistanceStats(false, aflameResistSC * amount, frostbiteResistSC * amount, overchargeResistSC * amount, overgrowthResistSC * amount, sunderResistSC * amount, frostbiteResistSC * amount, stunResistSC * amount, asleepResistSC * amount, bleedResistSC * amount, poisonResistSC * amount, knockBackResistSC * amount);

            // If we changed our invunerability, unatrgetability or invisibility
            if (invulnerabilitySC != 0 || untargetabilitySC != 0 || invisibilitySC != 0)
                ChangePlayerStatusLocks(false, invulnerabilitySC * amount, invisibilitySC * amount, untargetabilitySC * amount);

            // If we changed the size, change them back.
            if (sizeSC != 0)
                ChangeSize(false, sizeSC * amount);



            if (playerDamageSource.CompareTag("Player"))
            {

                if (myType == BuffsManager.BuffType.Aflame && currentStacks >= 25 && bonusDPS == 0 && playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.BurnDoesMaxHpDamageAtThreshold) > 0)
                {
                    bonusDPS = (connectedPlayer.healthMax / 100) * playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.BurnDoesMaxHpDamageAtThreshold);
                    if (bonusDPS > playerDamageSource.baseDamage * 10)
                        bonusDPS = playerDamageSource.baseDamage * 10;
                }

                if (myType == BuffsManager.BuffType.Aflame && DPSMultiplier == 1 && currentStacks >= 30 && connectedPlayer.GetComponent<BuffsManager>().PollForBuffStacks(BuffsManager.BuffType.Bleeding) >= 30 && playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.AflameBleedDamageAmpOnDoubleThreshhold) > 0)
                    DPSMultiplier += playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.AflameBleedDamageAmpOnDoubleThreshhold);

                if (myType == BuffsManager.BuffType.Bleeding && DPSMultiplier == 1 && currentStacks >= 30 && connectedPlayer.GetComponent<BuffsManager>().PollForBuffStacks(BuffsManager.BuffType.Aflame) >= 30 && playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.AflameBleedDamageAmpOnDoubleThreshhold) > 0)
                    DPSMultiplier += playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.AflameBleedDamageAmpOnDoubleThreshhold);

                if (myType == BuffsManager.BuffType.Aflame && currentStacks + connectedPlayer.GetComponent<BuffsManager>().PollForBuffStacks(BuffsManager.BuffType.Poisoned) >= 40 - playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.AflamePoisonGreviousWoundsOnStackThreshold)
                    && connectedPlayer.GetComponent<BuffsManager>().PollForBuffStacks(BuffsManager.BuffType.Poisoned) > 0 && playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.AflamePoisonGreviousWoundsOnStackThreshold) > 0)
                    connectedPlayer.GetComponent<BuffsManager>().NewBuff(BuffsManager.BuffType.GreviousWounds, playerDamageSource.baseDamage, playerDamageSource);

                if (myType == BuffsManager.BuffType.Poisoned && currentStacks + connectedPlayer.GetComponent<BuffsManager>().PollForBuffStacks(BuffsManager.BuffType.Aflame) >= 40 - playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.AflamePoisonGreviousWoundsOnStackThreshold)
                    && connectedPlayer.GetComponent<BuffsManager>().PollForBuffStacks(BuffsManager.BuffType.Aflame) > 0 && playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.AflamePoisonGreviousWoundsOnStackThreshold) > 0)
                    connectedPlayer.GetComponent<BuffsManager>().NewBuff(BuffsManager.BuffType.GreviousWounds, playerDamageSource.baseDamage, playerDamageSource);

                if (myType == BuffsManager.BuffType.Aflame && connectedPlayer.GetComponent<BuffsManager>().PollForBuffStacks(BuffsManager.BuffType.Poisoned) > 0 && playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.AflamePoisonFireAmpsPoison) > 0)
                    connectedPlayer.GetComponent<BuffsManager>().PollForBuff(BuffsManager.BuffType.Poisoned).DPSMultiplier += playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.AflamePoisonFireAmpsPoison) * amount;

                if (myType == BuffsManager.BuffType.Aflame && connectedPlayer.traitAflameStunStunOnThresholdReady && currentStacks >= 16 - playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.AflameStunStunOnThreshold) && playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.AflameStunStunOnThreshold) > 0)
                {
                    connectedPlayer.traitAflameStunStunOnThresholdReady = false;
                    connectedPlayer.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Stunned, 1, playerDamageSource.baseDamage, playerDamageSource);
                }

                if (myType == BuffsManager.BuffType.Frostbite && connectedPlayer.traitFreezeOnThresholdReady && currentStacks >= 21 - playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.IceFreezeAtStackThreshold) && playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.IceFreezeAtStackThreshold) > 0)
                {
                    connectedPlayer.traitFreezeOnThresholdReady = false;
                    connectedPlayer.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Frozen, 1, playerDamageSource.baseDamage, playerDamageSource);
                }

                if (myType == BuffsManager.BuffType.Frostbite && currentStacks >= 40 && playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.IceAmpAllDamageAtThreshold) > 0)
                    connectedPlayer.GetComponent<BuffsManager>().NewBuff(BuffsManager.BuffType.IceDamageAmp, playerDamageSource.baseDamage, playerDamageSource);

                if (myType == BuffsManager.BuffType.Windshear && playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.IceWindWindAmpsFrostbiteDamage) > 0 && connectedPlayer.GetComponent<BuffsManager>().PollForBuffStacks(BuffsManager.BuffType.Frostbite) > 0)
                    connectedPlayer.GetComponent<BuffsManager>().PollForBuff(BuffsManager.BuffType.Frostbite).DPSMultiplier += playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.IceWindWindAmpsFrostbiteDamage);

                if (myType == BuffsManager.BuffType.Frostbite && playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.IceWindIncreaseArmorShredPerFrostbite) > 0 && connectedPlayer.GetComponent<BuffsManager>().PollForBuffStacks(BuffsManager.BuffType.Windshear) > 0)
                    connectedPlayer.GetComponent<BuffsManager>().PollForBuff(BuffsManager.BuffType.Windshear).ChangeArmorScMultiplier(playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.IceWindIncreaseArmorShredPerFrostbite));

                if (myType == BuffsManager.BuffType.Frostbite && playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.IceBleedFrostbiteAmpsBleed) > 0 && connectedPlayer.GetComponent<BuffsManager>().PollForBuffStacks(BuffsManager.BuffType.Bleeding) > 0)
                    connectedPlayer.GetComponent<BuffsManager>().PollForBuff(BuffsManager.BuffType.Bleeding).DPSMultiplier += playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.IceBleedFrostbiteAmpsBleed);

                if (myType == BuffsManager.BuffType.Sunder && playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.EarthMaxHpDamageAtThreshold) > 0 && connectedPlayer.traitEarthMaxHpDamageReady && currentStacks >= 25)
                {
                    connectedPlayer.TakeDamage(connectedPlayer.healthMax * (0.09f + playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.EarthMaxHpDamageAtThreshold)), false, HitBox.DamageType.True, playerDamageSource.comboManager.currentcombo, playerDamageSource, false);
                    connectedPlayer.traitEarthMaxHpDamageReady = false;
                }
                if (myType == BuffsManager.BuffType.Sunder && playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.EarthAmpAllAfflictionsOnThreshhold) > 0 && currentStacks >= 20 && connectedPlayer.traitEarthAfflictionDamageAmpReady)
                {
                    connectedPlayer.traitEarthAfflictionDamageAmpReady = false;
                    BuffsManager myBuffManager = connectedPlayer.GetComponent<BuffsManager>();
                    float damagePercentageToAdd = 0.15f + playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.EarthAmpAllAfflictionsOnThreshhold);
                    if (myBuffManager.PollForBuffStacks(BuffsManager.BuffType.Aflame) > 0)
                        myBuffManager.PollForBuff(BuffsManager.BuffType.Aflame).DPSMultiplier += damagePercentageToAdd;
                    if (myBuffManager.PollForBuffStacks(BuffsManager.BuffType.Frostbite) > 0)
                        myBuffManager.PollForBuff(BuffsManager.BuffType.Frostbite).DPSMultiplier += damagePercentageToAdd;
                    if (myBuffManager.PollForBuffStacks(BuffsManager.BuffType.Bleeding) > 0)
                        myBuffManager.PollForBuff(BuffsManager.BuffType.Bleeding).DPSMultiplier += damagePercentageToAdd;
                    if (myBuffManager.PollForBuffStacks(BuffsManager.BuffType.Poisoned) > 0)
                        myBuffManager.PollForBuff(BuffsManager.BuffType.Poisoned).DPSMultiplier += damagePercentageToAdd;
                }

                if (myType == BuffsManager.BuffType.Sunder && connectedPlayer.GetComponent<PlayerStats>().traitEarthStunStunOnThresholdReady && currentStacks >= 11 - playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.EarthStunStunOnThreshold) && playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.EarthStunStunOnThreshold) > 0)
                {
                    connectedPlayer.GetComponent<BuffsManager>().NewBuff(BuffsManager.BuffType.Stunned, playerDamageSource.baseDamage, playerDamageSource);
                    connectedPlayer.GetComponent<PlayerStats>().traitEarthStunStunOnThresholdReady = false;
                }

                if (myType == BuffsManager.BuffType.Sunder && playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.EarthKnockbackSunderReducesKnockbackResistance) > 0)
                    connectedPlayer.GetComponent<BuffsManager>().NewBuff(BuffsManager.BuffType.EarthKnockbackResistanceLoss, playerDamageSource.baseDamage, playerDamageSource);

                if (myType == BuffsManager.BuffType.Windshear && currentStacks >= maxStacks && playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.WindMoreDamageOnMaximumStacks) > 0)
                    connectedPlayer.GetComponent<BuffsManager>().NewBuff(BuffsManager.BuffType.WindAmpDamageAtMaxStacks, playerDamageSource.baseDamage, playerDamageSource);

                if (myType == BuffsManager.BuffType.Windshear && currentStacks >= 25 && playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.WindBleedAmpBleedAtThreshold) > 0 && connectedPlayer.GetComponent<BuffsManager>().PollForBuffStacks(BuffsManager.BuffType.Bleeding) > 0 && !connectedPlayer.traitWindBleedBonusDamageAtThresholdEnabled)
                {
                    connectedPlayer.traitWindBleedBonusDamageAtThresholdEnabled = true;
                    connectedPlayer.GetComponent<BuffsManager>().PollForBuff(BuffsManager.BuffType.Bleeding).DPSMultiplier += playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.WindBleedAmpBleedAtThreshold);
                }

                if (myType == BuffsManager.BuffType.PhysicalPoisonAmp && connectedPlayer.GetComponent<BuffsManager>().PollForBuff(BuffsManager.BuffType.Poisoned))
                    connectedPlayer.GetComponent<BuffsManager>().PollForBuff(BuffsManager.BuffType.Poisoned).DPSMultiplier += 0.1f;

                if (myType == BuffsManager.BuffType.Bleeding && playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.BleedSlowsTargets) > 0)
                    connectedPlayer.GetComponent<BuffsManager>().NewBuff(BuffsManager.BuffType.BleedSlow, playerDamageSource.baseDamage, playerDamageSource);

                if (myType == BuffsManager.BuffType.Bleeding && currentStacks >= 10 && playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.BleedAmpDamageAtThreshold) > 0)
                    connectedPlayer.GetComponent<BuffsManager>().NewBuff(BuffsManager.BuffType.BleedAmpDamage, playerDamageSource.baseDamage, playerDamageSource);

                if (myType == BuffsManager.BuffType.Bleeding && connectedPlayer.traitBleedStunStunBelowHalfHPReady && connectedPlayer.health / connectedPlayer.healthMax <= 0.5f && currentStacks >= 11 - playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.BleedStunStunAtThresholdBelowHalfHP) && playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.BleedStunStunAtThresholdBelowHalfHP) > 0)
                {
                    connectedPlayer.traitBleedStunStunBelowHalfHPReady = false;
                    connectedPlayer.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Stunned, 1, playerDamageSource.baseDamage, playerDamageSource);
                }

                if (myType == BuffsManager.BuffType.Poisoned && currentStacks >= 32 - playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.PoisonShredArmorOnThreshold) && playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.PoisonShredArmorOnThreshold) > 0)
                    connectedPlayer.GetComponent<BuffsManager>().NewBuff(BuffsManager.BuffType.ArmorBroken, playerDamageSource.baseDamage, playerDamageSource);

                if (myType == BuffsManager.BuffType.PoisonAmpDamageOnKill && connectedPlayer.GetComponent<BuffsManager>().PollForBuff(BuffsManager.BuffType.Poisoned))
                    connectedPlayer.GetComponent<BuffsManager>().PollForBuff(BuffsManager.BuffType.Poisoned).DPSMultiplier += playerDamageSource.GetComponent<PlayerTraitManager>().CheckForOnKillValue(ItemTrait.TraitType.PoisonEnemiesAmpPoisonOnKill);
            }

        }
    }

    // Used to remove stacks and stats per stat to the target.
    public void RemoveStacks(float amount, bool changeTimer)
    {
        amount *= -1;
        if (currentStacks + amount < 0)
        {
            amount = currentStacks * -1;
            currentStacks = 0;
        }
        else
            currentStacks += amount;
        int amountint = Mathf.RoundToInt(amount);

        if (myType == BuffsManager.BuffType.Aflame && currentStacks < 25)
            bonusDPS = 0;

        if (myType == BuffsManager.BuffType.Aflame && DPSMultiplier >= 1 && (currentStacks < 30 || connectedPlayer.GetComponent<BuffsManager>().PollForBuffStacks(BuffsManager.BuffType.Bleeding) < 30))
            DPSMultiplier = 1;

        if (myType == BuffsManager.BuffType.Bleeding && DPSMultiplier >= 1 && (currentStacks < 30 || connectedPlayer.GetComponent<BuffsManager>().PollForBuffStacks(BuffsManager.BuffType.Aflame) < 30))
            DPSMultiplier = 1;

        if (playerDamageSource.CompareTag("Player"))
        {
            if (myType == BuffsManager.BuffType.Aflame && connectedPlayer.GetComponent<BuffsManager>().PollForBuffStacks(BuffsManager.BuffType.Poisoned) > 0 && playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.AflamePoisonFireAmpsPoison) > 0)
                connectedPlayer.GetComponent<BuffsManager>().PollForBuff(BuffsManager.BuffType.Poisoned).DPSMultiplier -= playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.AflamePoisonFireAmpsPoison) * amount;

            if (myType == BuffsManager.BuffType.Frostbite && currentStacks < 40 && playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.IceAmpAllDamageAtThreshold) > 0)
                connectedPlayer.GetComponent<BuffsManager>().AttemptRemovalOfBuff(BuffsManager.BuffType.IceDamageAmp, false);

            if (myType == BuffsManager.BuffType.Windshear && playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.IceWindWindAmpsFrostbiteDamage) > 0 && connectedPlayer.GetComponent<BuffsManager>().PollForBuffStacks(BuffsManager.BuffType.Frostbite) > 0)
                connectedPlayer.GetComponent<BuffsManager>().PollForBuff(BuffsManager.BuffType.Frostbite).DPSMultiplier -= playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.IceWindWindAmpsFrostbiteDamage);

            if (myType == BuffsManager.BuffType.Frostbite && playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.IceWindIncreaseArmorShredPerFrostbite) > 0 && connectedPlayer.GetComponent<BuffsManager>().PollForBuffStacks(BuffsManager.BuffType.Windshear) > 0)
                connectedPlayer.GetComponent<BuffsManager>().PollForBuff(BuffsManager.BuffType.Windshear).ChangeArmorScMultiplier(playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.IceWindIncreaseArmorShredPerFrostbite) * -1);

            if (myType == BuffsManager.BuffType.Frostbite && playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.IceBleedFrostbiteAmpsBleed) > 0 && connectedPlayer.GetComponent<BuffsManager>().PollForBuffStacks(BuffsManager.BuffType.Bleeding) > 0)
                connectedPlayer.GetComponent<BuffsManager>().PollForBuff(BuffsManager.BuffType.Bleeding).DPSMultiplier -= playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.IceBleedFrostbiteAmpsBleed);
        }
        if (changeTimer)
        {
            // Setting the timer.
            currentTimer = 0;
            if (stackSingleFalloff && stackfalloffTime != duration)
                duration = stackfalloffTime;
        }

        // Changing the text on the buff icon.
        iconStacks.text = string.Format("x{0:0}", currentStacks);

        // Adding the stat change.

        // If we changed the defensive stats, remove more stacks
        if (healthSC != 0 || healthRegenSC != 0 || armorSC != 0 || damageReductionSC != 0 || healingMultiplierSC != 0)
            ChangeDefensiveStats(false, healthSC * amount, healthRegenSC * amount, armorSC * amount, damageReductionSC * amount, healingMultiplierSC * amount);

        // If we changed offensive stats, remove more stacks
        if (atkSpdSC != 0 || movespeedSC != 0 || damagePercentageSC != 0 || critChanceSC != 0 || critDamageSC != 0)
            ChangeOffensiveStats(false, atkSpdSC * amount, movespeedSC * amount, damagePercentageSC * amount, critChanceSC * amount, critDamageSC * amount);

        // If we changed our resistance based stats, remove more stacks
        if (aflameResistSC != 0 || stunResistSC != 0 || asleepResistSC != 0 || bleedResistSC != 0 || poisonResistSC != 0 || frostbiteResistSC != 0 || knockBackResistSC != 0 || overchargeResistSC != 0 || overgrowthResistSC != 0 || sunderResistSC != 0 || windshearResistSC != 0)
            ChangeResistanceStats(false, aflameResistSC * amount, frostbiteResistSC * amount, overchargeResistSC * amount, overgrowthResistSC * amount, sunderResistSC * amount, frostbiteResistSC * amount, stunResistSC * amount, asleepResistSC * amount, bleedResistSC * amount, poisonResistSC * amount, knockBackResistSC * amount);

        // If we changed our invunerability, unatrgetability or invisibility
        if (invulnerabilitySC != 0 || untargetabilitySC != 0 || invisibilitySC != 0)
            ChangePlayerStatusLocks(false, invulnerabilitySC * amount, invisibilitySC * amount, untargetabilitySC * amount);

        // If we changed the size, change them back.
        if (sizeSC != 0)
            ChangeSize(false, sizeSC * amount);

        if (currentStacks == 0)
            EndBuff();
    }

    // Used to change if the player is invisible, invulnerable, etc. by changing their stats bool values.
    public void ChangePlayerStatusLocks(bool changeStatsChangeValue, float invulnerabilityGain, float invisibilityGain, float untargetabilityGain)
    {
        connectedPlayer.AddInvisibilitySource(invisibilityGain);
        connectedPlayer.AddInvulnerablitySource(invulnerabilityGain);
        connectedPlayer.AddUntargetableSource(untargetabilityGain);

        if (changeStatsChangeValue)
        {
            invulnerabilitySC += invulnerabilityGain;
            invisibilitySC += invisibilityGain;
            untargetabilitySC += untargetabilityGain;
        }
    }

    // USed to change the player size.
    public void ChangeSize(bool changeStatsChangeValue, float sizeGain)
    {
        connectedPlayer.ChangeSize(sizeGain);

        if (changeStatsChangeValue)
            sizeSC += sizeGain;
    }

    //USed to add offensive stast to the player
    public void ChangeOffensiveStats(bool changeStatsChangeValue, float atkSpeedGain, float movespeedGain, float damagePercentageGain, float critChanceGain, float critDamageGain)
    {
        connectedPlayer.bonusAttackSpeed += atkSpeedGain;
        connectedPlayer.movespeedPercentMultiplier += movespeedGain;
        connectedPlayer.damageIncreaseMultiplier += damagePercentageGain;
        connectedPlayer.critChance += critChanceGain;
        connectedPlayer.critDamageMultiplier += critDamageGain;

        if (changeStatsChangeValue)
        {
            atkSpdSC += atkSpeedGain;
            movespeedSC += movespeedGain;
            damagePercentageSC += damagePercentageGain;
            critChanceSC += critChanceGain;
            critDamageSC += critDamageGain;
        }

        connectedPlayer.StatSetup(false, true);
    }

    // Used to change the characters health, manan, regens, armor, and mr
    public void ChangeDefensiveStats(bool changeStatsChangeValue, float healthGain, float healthRegenGain, float armorGain, float damageReductionGain, float healingMultiplier)
    {
        connectedPlayer.bonusHealth += healthGain;
        connectedPlayer.bonusHealthRegen += healthRegenGain;
        connectedPlayer.ChangeArmor(armorGain * armorSCMultiplier);
        connectedPlayer.damageReductionMultiplier += damageReductionGain;
        connectedPlayer.healingMultiplier += healingMultiplier;

        if (changeStatsChangeValue)
        {
            healthSC += healthGain;
            healthRegenSC += healthRegenGain;
            armorSC += armorGain;
            damageReductionSC += damageReductionGain;
            healingMultiplierSC += healingMultiplier;
        }

        connectedPlayer.StatSetup(false, true);
    }

    // Used to change the players resistance to certain afflictions
    public void ChangeResistanceStats(bool changeStatsChangeValue, float aflameGain, float frostbiteGain, float overchargeGain, float overgrownGain, float sunderGain, float windshearGain, float stunGain, float asleepGain, float bleedGain, float poisonGain, float knockbackGain)
    {
        connectedPlayer.aflameResistance += aflameGain;
        connectedPlayer.frostbiteResistance += frostbiteGain;
        connectedPlayer.overchargeResistance += overchargeGain;
        connectedPlayer.overgrowthResistance += overgrownGain;
        connectedPlayer.sunderResistance += sunderGain;
        connectedPlayer.windshearResistance += windshearGain;
        connectedPlayer.stunResistance += stunGain;
        connectedPlayer.sleepResistance += asleepGain;
        connectedPlayer.bleedResistance += bleedGain;
        connectedPlayer.poisonResistance += poisonGain;
        connectedPlayer.knockbackResistance += knockbackGain;

        if (changeStatsChangeValue)
        {
            aflameResistSC += aflameGain;
            frostbiteResistSC += frostbiteGain;
            overchargeResistSC += overchargeGain;
            overgrowthResistSC += overgrownGain;
            sunderResistSC += sunderGain;
            windshearResistSC += windshearGain;
            stunResistSC += stunGain;
            asleepResistSC += asleepGain;
            bleedResistSC += bleedGain;
            poisonResistSC += poisonGain;
            knockBackResistSC += knockbackGain;
        }

        connectedPlayer.StatSetup(false, true);
    }

    // Called when we change the multiplier of the armorSC
    public void ChangeArmorScMultiplier(float value)
    {
        float previousValue = armorSCMultiplier;
        armorSCMultiplier += value;

        // Change our armor by the number of stacks times the difference in armor for the original sc to the new sc multiplier.
        if(currentStacks > 0)
        {
            float armorDifference = (currentStacks * armorSC * armorSCMultiplier) - (currentStacks * armorSC * previousValue);


            connectedPlayer.ChangeArmor(armorDifference);
            //connectedPlayer.ChangeArmor(armorGain * armorSCMultiplier);
        }
    }


    // Used when the buff is over
    public void EndBuff()
    {
        foreach (ParticleSystem ps in effectParticleSystem)
            ps.Stop();
        foreach (ParticleSystem ps in endOfBuffParticleSystem)
            ps.Play();

        if (myType == BuffsManager.BuffType.Stunned)
        {
            connectedPlayer.stunned = false;
            connectedPlayer.traitFreezeRefreshesStunReady = true;
            if (playerDamageSource.CompareTag("Player"))
            {
                if (connectedPlayer.GetComponent<BuffsManager>().PollForBuff(BuffsManager.BuffType.Aflame) && playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.AflameStunStunAmpsBurnDamage) > 0)
                    connectedPlayer.GetComponent<BuffsManager>().PollForBuff(BuffsManager.BuffType.Aflame).bonusDPS -= playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.AflameStunStunAmpsBurnDamage);
                if (connectedPlayer.GetComponent<BuffsManager>().PollForBuff(BuffsManager.BuffType.Poisoned) && playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.PoisonPrimaryTraitsAmpPoison) > 0)
                    connectedPlayer.GetComponent<BuffsManager>().PollForBuff(BuffsManager.BuffType.Poisoned).DPSMultiplier -= playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.PoisonPrimaryTraitsAmpPoison);
            }

        }
        else if (myType == BuffsManager.BuffType.Asleep)
            connectedPlayer.asleep = false;
        else if (myType == BuffsManager.BuffType.Frozen)
        {
            connectedPlayer.frozen = false;
            if (playerDamageSource.CompareTag("Player"))
            {
                if (connectedPlayer.GetComponent<BuffsManager>().PollForBuff(BuffsManager.BuffType.Poisoned) && playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.PoisonPrimaryTraitsAmpPoison) > 0)
                    connectedPlayer.GetComponent<BuffsManager>().PollForBuff(BuffsManager.BuffType.Poisoned).DPSMultiplier -= playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.PoisonPrimaryTraitsAmpPoison);
            }
        }
        else if (myType == BuffsManager.BuffType.Bleeding)
        {
            connectedPlayer.traitWindBleedBonusDamageAtThresholdEnabled = false;
            connectedPlayer.traitBleedBloodWellOnThresholdReady = true;
            connectedPlayer.traitBleedStunStunBelowHalfHPReady = true;

            connectedPlayer.bleeding = false;
            if (playerDamageSource.CompareTag("Player"))
            {
                if (currentStacks >= 10 && playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.BleedAmpDamageAtThreshold) > 0)
                    connectedPlayer.GetComponent<BuffsManager>().AttemptRemovalOfBuff(BuffsManager.BuffType.BleedAmpDamage, true);
                if (connectedPlayer.GetComponent<BuffsManager>().PollForBuff(BuffsManager.BuffType.Poisoned) && playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.PoisonPrimaryTraitsAmpPoison) > 0)
                    connectedPlayer.GetComponent<BuffsManager>().PollForBuff(BuffsManager.BuffType.Poisoned).DPSMultiplier -= playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.PoisonPrimaryTraitsAmpPoison);
            }
        }
        else if (myType == BuffsManager.BuffType.Knockback)
        {
            connectedPlayer.knockedBack = false;
            connectedPlayer.traitEarthKnockbackRocksOnSunderReady = false;
            if (playerDamageSource.CompareTag("Player"))
            {
                if (connectedPlayer.GetComponent<BuffsManager>().PollForBuff(BuffsManager.BuffType.Poisoned) && playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.PoisonPrimaryTraitsAmpPoison) > 0)
                    connectedPlayer.GetComponent<BuffsManager>().PollForBuff(BuffsManager.BuffType.Poisoned).DPSMultiplier -= playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.PoisonPrimaryTraitsAmpPoison);
            }
        }
        else if (myType == BuffsManager.BuffType.Aflame)
        {
            connectedPlayer.traitMoreAflameStacksOnHitThresholdFatigue = false;
            connectedPlayer.traitAflameStunStunOnThresholdReady = true;
            if (playerDamageSource.CompareTag("Player"))
            {
                if (connectedPlayer.GetComponent<BuffsManager>().PollForBuff(BuffsManager.BuffType.Poisoned) && playerDamageSource.CompareTag("Player") && playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.PoisonPrimaryTraitsAmpPoison) > 0)
                    connectedPlayer.GetComponent<BuffsManager>().PollForBuff(BuffsManager.BuffType.Poisoned).DPSMultiplier -= playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.PoisonPrimaryTraitsAmpPoison);
            }
        }
        else if (myType == BuffsManager.BuffType.Poisoned)
            connectedPlayer.traitEarthPoisonSummonPillarOnThresholdReady = true;
        else if (myType == BuffsManager.BuffType.Frostbite)
        {
            connectedPlayer.traitFreezeOnThresholdReady = true;
            if (playerDamageSource.CompareTag("Player"))
            {
                if (playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.IceAmpAllDamageAtThreshold) > 0)
                    connectedPlayer.GetComponent<BuffsManager>().AttemptRemovalOfBuff(BuffsManager.BuffType.IceDamageAmp, false);
                if (connectedPlayer.GetComponent<BuffsManager>().PollForBuff(BuffsManager.BuffType.Poisoned) && playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.PoisonPrimaryTraitsAmpPoison) > 0)
                    connectedPlayer.GetComponent<BuffsManager>().PollForBuff(BuffsManager.BuffType.Poisoned).DPSMultiplier -= playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.PoisonPrimaryTraitsAmpPoison);
            }
        }
        else if (myType == BuffsManager.BuffType.Windshear)
        {
            if (playerDamageSource.CompareTag("Player"))
            {
                if (playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.IceWindWindAmpsFrostbiteDamage) > 0 && connectedPlayer.GetComponent<BuffsManager>().PollForBuffStacks(BuffsManager.BuffType.Frostbite) > 0)
                    connectedPlayer.GetComponent<BuffsManager>().PollForBuff(BuffsManager.BuffType.Frostbite).DPSMultiplier -= playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.IceWindWindAmpsFrostbiteDamage) * currentStacks;

                if (playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.WindBleedAmpBleedAtThreshold) > 0 && connectedPlayer.GetComponent<BuffsManager>().PollForBuffStacks(BuffsManager.BuffType.Bleeding) > 0)
                {
                    connectedPlayer.GetComponent<BuffsManager>().PollForBuff(BuffsManager.BuffType.Bleeding).DPSMultiplier -= playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.WindBleedAmpBleedAtThreshold);
                    connectedPlayer.traitWindBleedBonusDamageAtThresholdEnabled = false;
                }
                if (connectedPlayer.GetComponent<BuffsManager>().PollForBuff(BuffsManager.BuffType.Poisoned) && playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.PoisonPrimaryTraitsAmpPoison) > 0)
                    connectedPlayer.GetComponent<BuffsManager>().PollForBuff(BuffsManager.BuffType.Poisoned).DPSMultiplier -= playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.PoisonPrimaryTraitsAmpPoison);
            }

            connectedPlayer.GetComponent<BuffsManager>().AttemptRemovalOfBuff(BuffsManager.BuffType.WindAmpDamageAtMaxStacks, true);
        }
        else if (myType == BuffsManager.BuffType.Sunder)
        {
            connectedPlayer.traitEarthMaxHpDamageReady = true;
            connectedPlayer.traitEarthStunStunOnThresholdReady = true;

            if (playerDamageSource.CompareTag("Player"))
            {
                if (playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.EarthAmpAllAfflictionsOnThreshhold) > 0 && currentStacks >= 20 && !connectedPlayer.traitEarthAfflictionDamageAmpReady)
                {
                    connectedPlayer.traitEarthAfflictionDamageAmpReady = true;
                    BuffsManager myBuffManager = connectedPlayer.GetComponent<BuffsManager>();
                    float damagePercentageToAdd = 0.15f + playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.EarthAmpAllAfflictionsOnThreshhold);
                    if (myBuffManager.PollForBuffStacks(BuffsManager.BuffType.Aflame) > 0)
                        myBuffManager.PollForBuff(BuffsManager.BuffType.Aflame).DPSMultiplier -= damagePercentageToAdd;
                    if (myBuffManager.PollForBuffStacks(BuffsManager.BuffType.Frostbite) > 0)
                        myBuffManager.PollForBuff(BuffsManager.BuffType.Frostbite).DPSMultiplier -= damagePercentageToAdd;
                    if (myBuffManager.PollForBuffStacks(BuffsManager.BuffType.Bleeding) > 0)
                        myBuffManager.PollForBuff(BuffsManager.BuffType.Bleeding).DPSMultiplier -= damagePercentageToAdd;
                    if (myBuffManager.PollForBuffStacks(BuffsManager.BuffType.Poisoned) > 0)
                        myBuffManager.PollForBuff(BuffsManager.BuffType.Poisoned).DPSMultiplier -= damagePercentageToAdd;
                }
                if (connectedPlayer.GetComponent<BuffsManager>().PollForBuff(BuffsManager.BuffType.Poisoned) && playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.PoisonPrimaryTraitsAmpPoison) > 0)
                    connectedPlayer.GetComponent<BuffsManager>().PollForBuff(BuffsManager.BuffType.Poisoned).DPSMultiplier -= playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.PoisonPrimaryTraitsAmpPoison);
            }

            connectedPlayer.GetComponent<BuffsManager>().AttemptRemovalOfBuff(BuffsManager.BuffType.EarthernDecay, false);
            connectedPlayer.GetComponent<BuffsManager>().AttemptRemovalOfBuff(BuffsManager.BuffType.EarthBonusResistanceLoss, false);
        }
        /*else if (myType == BuffsManager.BuffType.Revitalize)
        {
            connectedPlayer.revitalizeBuff = false;
            connectedPlayer.healthRegen = connectedPlayer.baseHealthRegen + connectedPlayer.baseHealthRegenGrowth * connectedPlayer.level + connectedPlayer.bonusHealthRegen;
        }
        */
        else if (myType == BuffsManager.BuffType.FlameWalker)
            connectedPlayer.flameWalkerEnabled = false;
        else if (myType == BuffsManager.BuffType.Immolation)
        {
            connectedPlayer.immolationEnabled = false;
            connectedPlayer.GetComponent<BuffsManager>().AttemptRemovalOfBuff(BuffsManager.BuffType.Aflame, true);
        }
        else if (myType == BuffsManager.BuffType.GiantStrength)
            connectedPlayer.GetComponent<PlayerGearManager>().RemoveMaterialOverride(PlayerGearManager.MaterialOverrideCode.GiantStrength);
        else if (myType == BuffsManager.BuffType.PoisonDamageAmp)
            connectedPlayer.GetComponent<BuffsManager>().PollForBuff(BuffsManager.BuffType.Poisoned).DPSMultiplier -= 0.5f;
        else if (myType == BuffsManager.BuffType.EarthTrueDamageConversion)
            connectedPlayer.traitEarthTrueDamageConversion = false;
        else if (myType == BuffsManager.BuffType.PhysicalPoisonAmp)
        {
            if (connectedPlayer.GetComponent<BuffsManager>().PollForBuff(BuffsManager.BuffType.Poisoned))
                connectedPlayer.GetComponent<BuffsManager>().PollForBuff(BuffsManager.BuffType.Poisoned).DPSMultiplier -= 0.1f * currentStacks;
        }
        else if (myType == BuffsManager.BuffType.PoisonAmpInitialDamage && connectedPlayer.GetComponent<BuffsManager>().PollForBuff(BuffsManager.BuffType.Poisoned))
        {
            if (playerDamageSource.CompareTag("Player"))
                connectedPlayer.GetComponent<BuffsManager>().PollForBuff(BuffsManager.BuffType.Poisoned).DPSMultiplier -= playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.PoisonAmpDamageOnFirstStack);
        }
        else if (myType == BuffsManager.BuffType.PoisonAmpDamageOnKill && connectedPlayer.GetComponent<BuffsManager>().PollForBuff(BuffsManager.BuffType.Poisoned))
        {
            if (playerDamageSource.CompareTag("Player"))
                connectedPlayer.GetComponent<BuffsManager>().PollForBuff(BuffsManager.BuffType.Poisoned).DPSMultiplier -= playerDamageSource.GetComponent<PlayerTraitManager>().CheckForOnKillValue(ItemTrait.TraitType.PoisonEnemiesAmpPoisonOnKill) * currentStacks;
        }


        

        if (playerDamageSource != null && playerDamageSource.CompareTag("Player"))
        {
            if (myType == BuffsManager.BuffType.Frostbite && playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.IceWindIncreaseArmorShredPerFrostbite) > 0 && connectedPlayer.GetComponent<BuffsManager>().PollForBuffStacks(BuffsManager.BuffType.Windshear) > 0)
            {
                connectedPlayer.GetComponent<BuffsManager>().PollForBuff(BuffsManager.BuffType.Windshear).ChangeArmorScMultiplier(playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.IceWindIncreaseArmorShredPerFrostbite) * currentStacks);
            }

            if (myType == BuffsManager.BuffType.Frostbite && playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.IceBleedFrostbiteAmpsBleed) > 0 && connectedPlayer.GetComponent<BuffsManager>().PollForBuffStacks(BuffsManager.BuffType.Bleeding) > 0)
            {
                connectedPlayer.GetComponent<BuffsManager>().PollForBuff(BuffsManager.BuffType.Bleeding).DPSMultiplier -= playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.IceBleedFrostbiteAmpsBleed) * currentStacks;
            }

            if (myType == BuffsManager.BuffType.Aflame && connectedPlayer.GetComponent<BuffsManager>().PollForBuffStacks(BuffsManager.BuffType.Poisoned) > 0 && playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.AflamePoisonFireAmpsPoison) > 0)
            {
                connectedPlayer.GetComponent<BuffsManager>().PollForBuff(BuffsManager.BuffType.Poisoned).DPSMultiplier -= playerDamageSource.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.AflamePoisonFireAmpsPoison) * currentStacks;
            }
        }




        // We do not change it if its a stackable buff since this method is called after we already remvoed all the stats associated with the buff, this would put us into negatives.
        if (!stackable)
        {

            // If we changed the defensive stats, change them back.
            if (healthSC != 0 || healthRegenSC != 0 || armorSC != 0 || damageReductionSC != 0 || healingMultiplierSC != 0)
                ChangeDefensiveStats(true, healthSC * -1, healthRegenSC * -1, armorSC * -1, damageReductionSC * -1, healingMultiplierSC * -1);

            // If we changed offensive stats, change em back.
            if (atkSpdSC != 0 || movespeedSC != 0 || damagePercentageSC != 0 || critChanceSC != 0 || critDamageSC != 0)
                ChangeOffensiveStats(true,  atkSpdSC * -1, movespeedSC * -1, damagePercentageSC * -1, critChanceSC * -1, critDamageSC * -1);

            // If we changed our resistance based stats, change em back.
            if (aflameResistSC != 0 || stunResistSC != 0 || asleepResistSC != 0 || bleedResistSC != 0 || poisonResistSC != 0 ||  frostbiteResistSC != 0 || knockBackResistSC != 0 || overchargeResistSC != 0 || overgrowthResistSC != 0|| sunderResistSC != 0 || windshearResistSC != 0)
                ChangeResistanceStats(true, aflameResistSC * -1, frostbiteResistSC * -1, overchargeResistSC * -1, overgrowthResistSC * -1, sunderResistSC * -1, frostbiteResistSC * -1, stunResistSC * -1, asleepResistSC * -1, bleedResistSC * -1, poisonResistSC * -1, knockBackResistSC * -1);

            // If we changed our invunerability, unatrgetability or invisibility
            if (invulnerabilitySC != 0 || untargetabilitySC != 0 || invisibilitySC != 0)
                ChangePlayerStatusLocks(true, invulnerabilitySC * -1, invisibilitySC * -1, untargetabilitySC * -1);

            // If we changed the size, change them back.
            if (sizeSC != 0)
                ChangeSize(true, sizeSC * -1);
        }
        else
        {
            // If we changed the defensive stats, change them back.
            if (healthSC != 0 || healthRegenSC != 0 || armorSC != 0 || damageReductionSC != 0 || healingMultiplierSC != 0)
                ChangeDefensiveStats(true, healthSC * -1 * currentStacks, healthRegenSC * -1 * currentStacks, armorSC * -1 * currentStacks, damageReductionSC * -1 * currentStacks, healingMultiplierSC * -1 * currentStacks);

            // If we changed offensive stats, change em back.
            if (atkSpdSC != 0 || movespeedSC != 0 || damagePercentageSC != 0 || critChanceSC != 0 || critDamageSC != 0)
                ChangeOffensiveStats(true, atkSpdSC * -1 * currentStacks, movespeedSC * -1 * currentStacks, damagePercentageSC * -1 * currentStacks, critChanceSC * -1 * currentStacks, critDamageSC * -1 * currentStacks);

            // If we changed our resistance based stats, change em back.
            if (aflameResistSC != 0 || stunResistSC != 0 || asleepResistSC != 0 || bleedResistSC != 0 || poisonResistSC != 0 || frostbiteResistSC != 0 || knockBackResistSC != 0 || overchargeResistSC != 0 || overgrowthResistSC != 0 || sunderResistSC != 0 || windshearResistSC != 0)
                ChangeResistanceStats(true, aflameResistSC * -1 * currentStacks, frostbiteResistSC * -1 * currentStacks, overchargeResistSC * -1 * currentStacks, overgrowthResistSC * -1 * currentStacks, sunderResistSC * -1 * currentStacks, frostbiteResistSC * -1 * currentStacks, stunResistSC * -1 * currentStacks, asleepResistSC * -1 * currentStacks, bleedResistSC * -1 * currentStacks, poisonResistSC * -1 * currentStacks, knockBackResistSC * -1 * currentStacks);

            // If we changed our invunerability, unatrgetability or invisibility
            if (invulnerabilitySC != 0 || untargetabilitySC != 0 || invisibilitySC != 0)
                ChangePlayerStatusLocks(true, invulnerabilitySC * -1 * currentStacks, invisibilitySC * -1 * currentStacks, untargetabilitySC * -1 * currentStacks);

            // If we changed the size, change them back.
            if (sizeSC != 0)
                ChangeSize(true, sizeSC * -1 * currentStacks);

        }
        // This contacts the buff manager, removi8ng us from their list of active buffs , deleting our icon, then killing this instance of the class. ALL fixes to stats should be done before this.
        connectedPlayer.GetComponent<BuffsManager>().RemoveBuff(this);
    }
}
