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

                if(canCrit && !critRolled)
                {
                    float bonusCritChance = 0;
                    if (damageType == DamageType.Wind && myStats.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.AflameWindshearWindAttacksGainCritOnBurningTarget) > 0)
                        bonusCritChance += other.GetComponent<BuffsManager>().PollForBuffStacks(BuffsManager.BuffType.Aflame) * myStats.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.AflameWindshearWindAttacksGainCritOnBurningTarget);
                    if (damageType == DamageType.Fire && myStats.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.AflameBleedIncreasesFlameCritChance) > 0)
                        bonusCritChance += other.GetComponent<BuffsManager>().PollForBuffStacks(BuffsManager.BuffType.Bleeding) * myStats.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.AflameBleedIncreasesFlameCritChance);

                    critRolled = true;
                    if (Random.Range(0f,100f) <= (myStats.critChance + bonusCritChance) * 100)
                        crit = true;
                }

                if (damageType == DamageType.Physical && myStats.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.AflamePhysicalDamageAmpOnBurningTarget) > 0)
                    damageDealt *= 1 + (myStats.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.AflamePhysicalDamageAmpOnBurningTarget) * enemyStats.GetComponent<BuffsManager>().PollForBuffStacks(BuffsManager.BuffType.Aflame)); 

                if(damageType == DamageType.Fire && myStats.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.AflameBleedFireDamageAmpOnBleedThreshold) > 0 && enemyStats.GetComponent<BuffsManager>().PollForBuffStacks(BuffsManager.BuffType.Bleeding) >= 25)
                {
                    Debug.Log("damagedealt is: " + damageDealt);
                    damageDealt *= 1 + (myStats.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.AflameBleedFireDamageAmpOnBleedThreshold) * enemyStats.GetComponent<BuffsManager>().PollForBuffStacks(BuffsManager.BuffType.Bleeding));
                    enemyStats.GetComponent<BuffsManager>().AttemptRemovalOfBuff(BuffsManager.BuffType.Bleeding);
                    Debug.Log("damagedealt is: " + damageDealt);
                }

                if (crit)
                    damageDealt *= myStats.critDamageMultiplier;

                if (projectile && procsOnHits || projectileAOE && procsOnHits)
                    myStats.GetComponent<BuffsManager>().ProcOnHits(other.gameObject, this);
                else if (procsOnHits)
                    transform.root.GetComponent<BuffsManager>().ProcOnHits(other.gameObject, this);

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

                if (projectile && procsOnHits || projectileAOE && procsOnHits)
                    myStats.GetComponent<BuffsManager>().ProcOnHits(other.gameObject, this);
                else if (procsOnHits)
                    transform.root.GetComponent<BuffsManager>().ProcOnHits(other.gameObject, this);


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
