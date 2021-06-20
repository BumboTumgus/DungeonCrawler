using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public enum DamageType {Physical, Fire, Ice, Lightning, Nature, Earth, Wind, Poison, Bleed, True, Healing}
    public DamageType damageType;

    public float damage = 5;
    public int stacksToAdd = 0;
    public bool crit = false;
    public bool canCrit = true;
    public bool critRolled = false;
    public bool procsOnHits = false;

    public bool hitEnemies = false;
    public bool hitPlayers = false;

    public PlayerStats myStats;
    public PlayerStats enemyStats;

    public bool projectile = false;
    public bool projectileAOE = false;
    public bool disjointedHitbox = false;
    public bool trap = false;


    public float screenShakeAmount = 0;

    private void Start()
    {
        // Grab our player stats from our parent.
        if(!projectile && !projectileAOE && !disjointedHitbox)
            myStats = transform.root.GetComponent<PlayerStats>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("we have colldied with collided with: " + other.name);
        // Projectile logic, we are a projectile and hit an object on the collidable envorioment or interactable layer.
        if ((projectile && other.gameObject.layer == 14 && hitEnemies) || (projectile && other.gameObject.layer == 13 && hitPlayers))
        {
            //Debug.Log("we hit an object that is in the collidable or interable layer");
            if(GetComponent<ProjectileBehaviour>() && !GetComponent<ProjectileBehaviour>().piercesTargets)
                GetComponent<ProjectileBehaviour>().DestroyProjectile();
        }
        else if ((projectile && other.gameObject.layer == 10))
        {
            //Debug.Log("we hit an object that is in the collidable or interable layer");
            if (GetComponent<ProjectileBehaviour>() && !GetComponent<ProjectileBehaviour>().piercesWalls)
                GetComponent<ProjectileBehaviour>().DestroyProjectile();
        }

        if (projectile && !GetComponent<ProjectileBehaviour>().hitAOE || !projectile)
        {
            // Enemy Logic
            if (other.CompareTag("Enemy") && hitEnemies)
            {
                enemyStats = other.GetComponent<PlayerStats>();

                if (enemyStats.ephemeral)
                    return;
                else if (enemyStats.invulnerable)
                    return;

                float damageDealt = damage;

                if (enemyStats.traitEarthTrueDamageConversion)
                    damageType = DamageType.True;

                if (damageType == DamageType.Physical && myStats.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.PhysicalBleedSkillsDoTrueDamageAtThreshold) > 0 && enemyStats.GetComponent<BuffsManager>().PollForBuffStacks(BuffsManager.BuffType.Bleeding) >= 54 - myStats.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.PhysicalBleedSkillsDoTrueDamageAtThreshold))
                    damageType = DamageType.True;

                if (canCrit && !critRolled)
                {
                    float bonusCritChance = 0;
                    if (damageType == DamageType.Wind && myStats.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.AflameWindshearWindAttacksGainCritOnBurningTarget) > 0)
                        bonusCritChance += other.GetComponent<BuffsManager>().PollForBuffStacks(BuffsManager.BuffType.Aflame) * myStats.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.AflameWindshearWindAttacksGainCritOnBurningTarget);
                    if (damageType == DamageType.Fire && myStats.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.AflameBleedIncreasesFlameCritChance) > 0)
                        bonusCritChance += other.GetComponent<BuffsManager>().PollForBuffStacks(BuffsManager.BuffType.Bleeding) * myStats.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.AflameBleedIncreasesFlameCritChance);
                    if (myStats.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.EarthBleedBonusCritChanceOnBleedingTarget) > 0 && other.GetComponent<BuffsManager>().PollForBuff(BuffsManager.BuffType.Bleeding) && other.GetComponent<BuffsManager>().PollForBuff(BuffsManager.BuffType.Sunder))
                        bonusCritChance += other.GetComponent<BuffsManager>().PollForBuffStacks(BuffsManager.BuffType.Sunder) * myStats.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.EarthBleedBonusCritChanceOnBleedingTarget);
                    if (damageType == DamageType.Wind && myStats.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.WindBleedBleedGrantsWindCritChance) > 0 && other.GetComponent<BuffsManager>().PollForBuff(BuffsManager.BuffType.Bleeding))
                        bonusCritChance += other.GetComponent<BuffsManager>().PollForBuffStacks(BuffsManager.BuffType.Bleeding) * myStats.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.WindBleedBleedGrantsWindCritChance);
                    if (damageType == DamageType.Poison && myStats.GetComponent<BuffsManager>().PollForBuff(BuffsManager.BuffType.PoisonInstantCrit))
                        bonusCritChance += 100;

                    critRolled = true;
                    if (Random.Range(0f,100f) <= (myStats.critChance + bonusCritChance) * 100)
                        crit = true;
                }

                if (GetComponent<HitBoxBuff>() && GetComponent<HitBoxBuff>().stun && other.GetComponent<PlayerStats>().stunned && myStats.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.StunOnStunDealsAdditionalBaseDamage) > 0)
                    damageDealt += myStats.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.StunOnStunDealsAdditionalBaseDamage) * myStats.baseDamage;




                if (GetComponent<HitBoxBuff>() && GetComponent<HitBoxBuff>().stun && myStats.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.WindStunStunDealsTrueDamageAtThreshold) > 0 && other.GetComponent<BuffsManager>().PollForBuffStacks(BuffsManager.BuffType.Windshear) >= 50)
                {
                    damageDealt *= 1f + myStats.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.WindStunStunDealsTrueDamageAtThreshold);
                    other.GetComponent<BuffsManager>().AttemptRemovalOfBuff(BuffsManager.BuffType.Windshear, true);
                    damageType = DamageType.True;
                }


                if (damageType == DamageType.Physical && myStats.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.AflamePhysicalDamageAmpOnBurningTarget) > 0)
                    damageDealt *= 1 + (myStats.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.AflamePhysicalDamageAmpOnBurningTarget) * enemyStats.GetComponent<BuffsManager>().PollForBuffStacks(BuffsManager.BuffType.Aflame));

                if (damageType == DamageType.Fire && myStats.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.AflameKnockbackKnockbackAmpsFireDamage) > 0 && other.GetComponent<PlayerStats>().knockedBack)
                    damageDealt *= 1 + myStats.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.AflameKnockbackKnockbackAmpsFireDamage);


                if (damageType == DamageType.Fire && myStats.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.AflameBleedFireDamageAmpOnBleedThreshold) > 0 && enemyStats.GetComponent<BuffsManager>().PollForBuffStacks(BuffsManager.BuffType.Bleeding) >= 25)
                {
                    damageDealt *= 1 + (myStats.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.AflameBleedFireDamageAmpOnBleedThreshold) * enemyStats.GetComponent<BuffsManager>().PollForBuffStacks(BuffsManager.BuffType.Bleeding));
                    enemyStats.GetComponent<BuffsManager>().AttemptRemovalOfBuff(BuffsManager.BuffType.Bleeding, true);
                }

                if (damageType == DamageType.Earth && myStats.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.IceEarthFrostToEarthBonusDamage) > 0 && other.GetComponent<BuffsManager>().PollForBuff(BuffsManager.BuffType.Frostbite))
                {
                    damageDealt *= 1 + myStats.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.IceEarthFrostToEarthBonusDamage) * other.GetComponent<BuffsManager>().PollForBuffStacks(BuffsManager.BuffType.Frostbite);
                    other.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Sunder, Mathf.RoundToInt(other.GetComponent<BuffsManager>().PollForBuffStacks(BuffsManager.BuffType.Frostbite) / 2), myStats.baseDamage, myStats);
                    other.GetComponent<BuffsManager>().AttemptRemovalOfBuff(BuffsManager.BuffType.Frostbite,true);
                }

                if (damageType == DamageType.Ice && myStats.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.IceEarthSunderAmpsIceDamage) > 0 && other.GetComponent<BuffsManager>().PollForBuff(BuffsManager.BuffType.Sunder))
                {
                    damageDealt *= 1 + myStats.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.IceEarthSunderAmpsIceDamage) * other.GetComponent<BuffsManager>().PollForBuffStacks(BuffsManager.BuffType.Sunder);
                    other.GetComponent<BuffsManager>().AttemptRemovalOfBuff(BuffsManager.BuffType.Sunder,true);
                }

                if (damageType == DamageType.Wind && myStats.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.IceWindWindSpellsDamageAmp) > 0 && other.GetComponent<BuffsManager>().PollForBuff(BuffsManager.BuffType.Frostbite))
                    damageDealt *= 1 + myStats.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.IceWindWindSpellsDamageAmp) * other.GetComponent<BuffsManager>().PollForBuffStacks(BuffsManager.BuffType.Frostbite);

                if (damageType == DamageType.Ice && myStats.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.IceStunIceRefreshesStun) > 0 && other.GetComponent<PlayerStats>().stunned && other.GetComponent<PlayerStats>().traitFreezeRefreshesStunReady)
                {
                    other.GetComponent<PlayerStats>().traitFreezeRefreshesStunReady = false;
                    damageDealt *= 1.5f + myStats.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.IceStunIceRefreshesStun);
                    other.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Stunned, 1, myStats.baseDamage, myStats);
                }

                if (damageType == DamageType.Earth && myStats.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.EarthIncreasedDamageToLowerArmorTargets) > 0 && (myStats.armor * myStats.armorReductionMultiplier) / 3 >= (other.GetComponent<PlayerStats>().armor * other.GetComponent<PlayerStats>().armorReductionMultiplier))
                    damageDealt *= 1.8f + myStats.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.EarthIncreasedDamageToLowerArmorTargets);

                if (damageType == DamageType.Earth && myStats.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.EarthAmpDamageOnHealthyTargets) > 0 && other.GetComponent<PlayerStats>().health / other.GetComponent<PlayerStats>().healthMax >= 0.9f)
                    damageDealt *= 1.25f + myStats.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.EarthAmpDamageOnHealthyTargets);

                if (damageType == DamageType.Physical && myStats.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.EarthPhysicalSunderAmpsDamage) > 0 && other.GetComponent<BuffsManager>().PollForBuff(BuffsManager.BuffType.Sunder))
                    damageDealt *= 1f + myStats.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.EarthPhysicalSunderAmpsDamage) * other.GetComponent<BuffsManager>().PollForBuffStacks(BuffsManager.BuffType.Sunder);

                if (damageType == DamageType.Earth && myStats.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.EarthBleedBonusEarthDamageToBleeding) > 0 && other.GetComponent<BuffsManager>().PollForBuff(BuffsManager.BuffType.Bleeding))
                    damageDealt *= 1f + myStats.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.EarthBleedBonusEarthDamageToBleeding) * other.GetComponent<BuffsManager>().PollForBuffStacks(BuffsManager.BuffType.Bleeding);

                if (GetComponent<HitBoxBuff>() && GetComponent<HitBoxBuff>().stun && myStats.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.EarthStunSunderAmpsStunDamageLength) > 0 && other.GetComponent<BuffsManager>().PollForBuff(BuffsManager.BuffType.Sunder))
                {
                    damageDealt *= 1f + myStats.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.EarthStunSunderAmpsStunDamageLength) * other.GetComponent<BuffsManager>().PollForBuffStacks(BuffsManager.BuffType.Sunder);
                    other.GetComponent<BuffsManager>().AttemptRemovalOfBuff(BuffsManager.BuffType.Sunder, true);
                }

                if(damageType == DamageType.True && other.GetComponent<BuffsManager>().PollForBuff(BuffsManager.BuffType.Windshear) && myStats.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.WindWindshearAmpsTrueDamage) > 0)
                    damageDealt *= 1f + myStats.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.WindWindshearAmpsTrueDamage) * other.GetComponent<BuffsManager>().PollForBuffStacks(BuffsManager.BuffType.Windshear);

                if (CompareTag("BasicAttack") && myStats.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.WindPhysicalWindshearAmpsBasicAttacks) > 0)
                    damageDealt *= 1f + myStats.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.WindPhysicalWindshearAmpsBasicAttacks) * other.GetComponent<BuffsManager>().PollForBuffStacks(BuffsManager.BuffType.Windshear);

                if (damageType == DamageType.Physical && myStats.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.PhysicalAmpDamageBelowHalfHp) > 0 && myStats.health / myStats.healthMax <= 0.5f)
                    damageDealt *= 1f + myStats.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.PhysicalAmpDamageBelowHalfHp);

                if (damageType == DamageType.Physical && myStats.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.PhysicalBleedBleedAmpsPhysicalDamage) > 0 && other.GetComponent<BuffsManager>().PollForBuffStacks(BuffsManager.BuffType.Bleeding) > 0)
                    damageDealt *= 1f + myStats.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.PhysicalBleedBleedAmpsPhysicalDamage) * other.GetComponent<BuffsManager>().PollForBuffStacks(BuffsManager.BuffType.Bleeding);

                if(damageType == DamageType.Physical && other.GetComponent<BuffsManager>().PollForBuff(BuffsManager.BuffType.PhysicalPhysicalAmp))
                    damageDealt *= 1f + 0.1f * other.GetComponent<BuffsManager>().PollForBuffStacks(BuffsManager.BuffType.PhysicalPhysicalAmp);

                if (damageType == DamageType.Physical && myStats.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.PhysicalStunAmpDamageOnStunned) > 0 && other.GetComponent<PlayerStats>().stunned)
                    damageDealt *= 1f + myStats.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.PhysicalStunAmpDamageOnStunned);

                if (damageType == DamageType.Physical && myStats.GetComponent<BuffsManager>().PollForBuff(BuffsManager.BuffType.PhysicalKnockbackAmpNextAttack))
                {
                    damageDealt *= 1f + myStats.GetComponent<PlayerTraitManager>().CheckForOnKillValue(ItemTrait.TraitType.PhysicalKnockbackKnockbackKillAmpsPhysicalDamage);
                    myStats.GetComponent<BuffsManager>().AttemptRemovalOfBuff(BuffsManager.BuffType.PhysicalKnockbackAmpNextAttack, true);
                }
                if (CompareTag("BasicAttack") && myStats.GetComponent<BuffsManager>().PollForBuff(BuffsManager.BuffType.KnockbackAmpBasicAttacks))
                {
                    damageDealt *= 1f + myStats.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.KnockBackAmpsBasicAttacks);
                    myStats.GetComponent<BuffsManager>().PollForBuff(BuffsManager.BuffType.KnockbackAmpBasicAttacks).RemoveStacks(1, false);
                }








                if (crit)
                {
                    float bonusCritDamage = 0;
                    if (damageType == DamageType.Earth && myStats.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.IceEarthEarthSpellBonusCritDamage) > 0)
                        bonusCritDamage += other.GetComponent<BuffsManager>().PollForBuffStacks(BuffsManager.BuffType.Frostbite) * myStats.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.IceEarthEarthSpellBonusCritDamage);
                    if (damageType == DamageType.Physical && myStats.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.IcePhysicalFrostbiteAmpsPhysicalCritDamage) > 0)
                        bonusCritDamage += other.GetComponent<BuffsManager>().PollForBuffStacks(BuffsManager.BuffType.Frostbite) * myStats.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.IcePhysicalFrostbiteAmpsPhysicalCritDamage);
                    if (damageType == DamageType.Physical && myStats.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.EarthPhysicalSunderAmpsCrits) > 0)
                        bonusCritDamage += other.GetComponent<BuffsManager>().PollForBuffStacks(BuffsManager.BuffType.Sunder) * myStats.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.EarthPhysicalSunderAmpsCrits);
                    if (myStats.GetComponent<PlayerTraitManager>().CheckForOnHitValue(ItemTrait.TraitType.BleedAmpsCritHitsAddsBleedToNearby) > 0)
                        bonusCritDamage += other.GetComponent<BuffsManager>().PollForBuffStacks(BuffsManager.BuffType.Bleeding) * myStats.GetComponent<PlayerTraitManager>().CheckForOnHitValue(ItemTrait.TraitType.BleedAmpsCritHitsAddsBleedToNearby);
                    if (myStats.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.BleedCritsConsumeBleed) > 0)
                    {
                        bonusCritDamage += other.GetComponent<BuffsManager>().PollForBuffStacks(BuffsManager.BuffType.Bleeding) * myStats.GetComponent<PlayerTraitManager>().CheckForOnHitValue(ItemTrait.TraitType.BleedCritsConsumeBleed);
                        other.GetComponent<BuffsManager>().AttemptRemovalOfBuff(BuffsManager.BuffType.Bleeding, true);
                    }

                    if (!CompareTag("BasicAttack") && damageType == DamageType.Physical && myStats.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.WindPhysicalCritsDealArmorAsDamage) > 0)
                    {
                        damage *= 1 + other.GetComponent<PlayerStats>().armor * myStats.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.WindPhysicalCritsDealArmorAsDamage) * other.GetComponent<BuffsManager>().PollForBuffStacks(BuffsManager.BuffType.Windshear);
                        other.GetComponent<BuffsManager>().AttemptRemovalOfBuff(BuffsManager.BuffType.Windshear, true);
                    }
                    if (damageType == DamageType.Poison && myStats.GetComponent<BuffsManager>().PollForBuff(BuffsManager.BuffType.PoisonInstantCrit))
                    {
                        bonusCritDamage += myStats.GetComponent<PlayerTraitManager>().CheckForOnKillValue(ItemTrait.TraitType.PoisonAmpNextAttackAfterPoisonKill);
                        myStats.GetComponent<BuffsManager>().AttemptRemovalOfBuff(BuffsManager.BuffType.PoisonInstantCrit, true);
                    }
                    if (myStats.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.StunAmpCritDamage) > 0 && other.GetComponent<PlayerStats>().stunned)
                        bonusCritDamage += myStats.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.StunAmpCritDamage);


                    damageDealt *= myStats.critDamageMultiplier + bonusCritDamage;
                }

                if (projectile && procsOnHits || projectileAOE && procsOnHits)
                    myStats.GetComponent<BuffsManager>().ProcOnHits(other.gameObject, this, damageDealt);
                else if (procsOnHits)
                    transform.root.GetComponent<BuffsManager>().ProcOnHits(other.gameObject, this, damageDealt);


                if (damageType != DamageType.Healing)
                {
                    if(damageType != DamageType.Physical && damageType != DamageType.True)
                    {
                        switch (damageType)
                        {
                            case DamageType.Fire:
                                enemyStats.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Aflame, stacksToAdd, myStats.baseDamage, myStats);
                                break;
                            case DamageType.Ice:
                                enemyStats.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Frostbite, stacksToAdd, myStats.baseDamage, myStats);
                                break;
                            case DamageType.Lightning:
                                enemyStats.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Overcharge, stacksToAdd, myStats.baseDamage, myStats);
                                break;
                            case DamageType.Nature:
                                enemyStats.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Overgrown, stacksToAdd, myStats.baseDamage, myStats);
                                break;
                            case DamageType.Earth:
                                enemyStats.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Sunder, stacksToAdd, myStats.baseDamage, myStats);
                                break;
                            case DamageType.Wind:
                                enemyStats.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Windshear, stacksToAdd, myStats.baseDamage, myStats);
                                break;
                            case DamageType.Poison:
                                enemyStats.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Poisoned, stacksToAdd, myStats.baseDamage, myStats);
                                break;
                            case DamageType.Bleed:
                                enemyStats.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Bleeding, stacksToAdd, myStats.baseDamage, myStats);
                                break;
                            default:
                                break;
                        }
                    }
                    enemyStats.TakeDamage(damageDealt * myStats.damageIncreaseMultiplier, crit, damageType, myStats.comboManager.currentcombo, myStats);

                    myStats.comboManager.AddComboCounter(1);
                }
                else
                    enemyStats.HealHealth(damageDealt, damageType);
            }
            // Player Logic
            else if (other.CompareTag("Player") && hitPlayers)
            {

                //Debug.Log("we have hit a player");
                enemyStats = other.GetComponent<PlayerStats>();
                
                if (enemyStats.ephemeral)
                    return;
                else if (enemyStats.invulnerable)
                    return;

                float damageDealt = damage;

                if (canCrit && !critRolled)
                {
                    critRolled = true;
                    if (Random.Range(0f, 100f) <= myStats.critChance)
                    {
                        crit = true;
                        damageDealt *= myStats.critDamageMultiplier;
                    }
                }

                if (!trap)
                {
                    if (projectile && procsOnHits || projectileAOE && procsOnHits)
                        myStats.GetComponent<BuffsManager>().ProcOnHits(other.gameObject, this, damageDealt);
                    else if (procsOnHits)
                        transform.root.GetComponent<BuffsManager>().ProcOnHits(other.gameObject, this, damageDealt);

                    if (other.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.IceEnemyAttacksWeakendAtThreshold) > 0 && myStats.GetComponent<BuffsManager>().PollForBuffStacks(BuffsManager.BuffType.Frostbite) >= 31 - other.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.IceEnemyAttacksWeakendAtThreshold))
                        damageDealt *= 0.5f;
                }

                if (damageType != DamageType.Healing)
                {
                    if (damageType != DamageType.Physical && damageType != DamageType.True)
                    {
                        switch (damageType)
                        {
                            case DamageType.Fire:
                                enemyStats.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Aflame, stacksToAdd, myStats.baseDamage, myStats);
                                break;
                            case DamageType.Ice:
                                enemyStats.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Frostbite, stacksToAdd, myStats.baseDamage, myStats);
                                break;
                            case DamageType.Lightning:
                                enemyStats.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Overcharge, stacksToAdd, myStats.baseDamage, myStats);
                                break;
                            case DamageType.Nature:
                                enemyStats.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Overgrown, stacksToAdd, myStats.baseDamage, myStats);
                                break;
                            case DamageType.Earth:
                                enemyStats.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Sunder, stacksToAdd, myStats.baseDamage, myStats);
                                break;
                            case DamageType.Wind:
                                enemyStats.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Windshear, stacksToAdd, myStats.baseDamage, myStats);
                                break;
                            case DamageType.Poison:
                                enemyStats.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Poisoned, stacksToAdd, myStats.baseDamage, myStats);
                                break;
                            case DamageType.Bleed:
                                enemyStats.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Bleeding, stacksToAdd, myStats.baseDamage, myStats);
                                break;
                            default:
                                break;
                        }
                    }
                    enemyStats.TakeDamage(damageDealt * myStats.damageIncreaseMultiplier, crit, damageType, 0, myStats);
                }
                else
                    enemyStats.HealHealth(damageDealt, damageType);
            }
        }
    }
}
