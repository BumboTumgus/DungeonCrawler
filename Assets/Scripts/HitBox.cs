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
    public bool damageOverload = false;
    public bool procsOnHits = false;

    [SerializeField] private bool hitEnemies = false;
    [SerializeField] private bool hitPlayers = false;

    public PlayerStats myStats;
    public PlayerStats enemyStats;

    public bool projectile = false;
    public bool projectileAOE = false;
    public bool disjointedHitbox = false;

    public bool bypassCrit = false;

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
        if ((projectile && other.gameObject.layer == 10) || (projectile && other.gameObject.layer == 14 && hitEnemies) || (projectile && other.gameObject.layer == 13 && hitPlayers))
        {
            //Debug.Log("we hit an object that is in the collidable or interable layer");
            if(GetComponent<ProjectileBehaviour>())
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

                bool attackCrit = false;

                float damageDealt = damage;
                /*
                if (!damageOverload)
                {
                    // Check to see if the attack hit.
                    damageDealt = myStats.baseDamage * 1;
                }
                */
                // Debug.Log("we should check the on hits here.");

                if (projectile && procsOnHits || projectileAOE && procsOnHits)
                    myStats.GetComponent<BuffsManager>().ProcOnHits(other.gameObject, this);
                else if (procsOnHits)
                    transform.root.GetComponent<BuffsManager>().ProcOnHits(other.gameObject, this);

                if (bypassCrit)
                {
                    attackCrit = true;
                    damageDealt *= 2;
                    bypassCrit = false;
                }

                if (damageType != DamageType.Healing)
                {
                    if(damageType != DamageType.Physical && damageType != DamageType.True)
                    {
                        switch (damageType)
                        {
                            case DamageType.Fire:
                                enemyStats.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Aflame, stacksToAdd, myStats.baseDamage);
                                break;
                            case DamageType.Ice:
                                enemyStats.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Frostbite, stacksToAdd, myStats.baseDamage);
                                break;
                            case DamageType.Lightning:
                                enemyStats.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Overcharge, stacksToAdd, myStats.baseDamage);
                                break;
                            case DamageType.Nature:
                                enemyStats.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Overgrown, stacksToAdd, myStats.baseDamage);
                                break;
                            case DamageType.Earth:
                                enemyStats.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Sunder, stacksToAdd, myStats.baseDamage);
                                break;
                            case DamageType.Wind:
                                enemyStats.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Windshear, stacksToAdd, myStats.baseDamage);
                                break;
                            case DamageType.Poison:
                                enemyStats.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Poisoned, stacksToAdd, myStats.baseDamage);
                                break;
                            case DamageType.Bleed:
                                enemyStats.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Bleeding, stacksToAdd, myStats.baseDamage);
                                break;
                            default:
                                break;
                        }
                    }
                    enemyStats.TakeDamage(damageDealt, attackCrit, damageType);

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

                bool attackCrit = false;

                float damageDealt = damage;
                /*
                if (!damageOverload)
                {
                    damageDealt = myStats.baseDamage * 1;
                }
                */

                if (bypassCrit)
                {
                    attackCrit = true;
                    damageDealt *= 2;
                    bypassCrit = false;
                }

                if (damageType != DamageType.Healing)
                {
                    if (damageType != DamageType.Physical && damageType != DamageType.True)
                    {
                        switch (damageType)
                        {
                            case DamageType.Fire:
                                enemyStats.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Aflame, stacksToAdd, myStats.baseDamage);
                                break;
                            case DamageType.Ice:
                                enemyStats.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Frostbite, stacksToAdd, myStats.baseDamage);
                                break;
                            case DamageType.Lightning:
                                enemyStats.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Overcharge, stacksToAdd, myStats.baseDamage);
                                break;
                            case DamageType.Nature:
                                break;
                            case DamageType.Earth:
                                break;
                            case DamageType.Wind:
                                break;
                            case DamageType.Poison:
                                break;
                            case DamageType.Bleed:
                                break;
                            default:
                                break;
                        }
                    }
                    enemyStats.TakeDamage(damageDealt, attackCrit, damageType);
                }
                else
                    enemyStats.HealHealth(damageDealt, damageType);
            }
        }
    }
}
