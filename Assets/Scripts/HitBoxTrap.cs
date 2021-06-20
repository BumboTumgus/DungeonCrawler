using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxTrap : MonoBehaviour
{
    public HitBox.DamageType damageType;

    public float baseDamage = 10;
    public float damageMultiplier = 0;
    public int stacksToAdd = 0;

    public bool hitEnemies = false;
    public bool hitPlayers = false;

    private PlayerStats enemyStats;


    private void OnTriggerEnter(Collider other)
    {
        // Enemy Logic
        if (other.CompareTag("Enemy") && hitEnemies)
        {
        }
        // Player Logic
        else if (other.CompareTag("Player") && hitPlayers)
        {
            enemyStats = other.GetComponent<PlayerStats>();

            if (enemyStats.ephemeral)
                return;
            else if (enemyStats.invulnerable)
                return;

            float damageDealt = baseDamage * damageMultiplier;

            if (damageType != HitBox.DamageType.Healing)
            {
                if (damageType != HitBox.DamageType.Physical && damageType != HitBox.DamageType.True)
                {
                    switch (damageType)
                    {
                        case HitBox.DamageType.Fire:
                            enemyStats.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Aflame, stacksToAdd, baseDamage, null);
                            break;
                        case HitBox.DamageType.Ice:
                            enemyStats.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Frostbite, stacksToAdd, baseDamage, null);
                            break;
                        case HitBox.DamageType.Lightning:
                            enemyStats.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Overcharge, stacksToAdd, baseDamage, null);
                            break;
                        case HitBox.DamageType.Nature:
                            enemyStats.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Overgrown, stacksToAdd, baseDamage, null);
                            break;
                        case HitBox.DamageType.Earth:
                            enemyStats.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Sunder, stacksToAdd, baseDamage, null);
                            break;
                        case HitBox.DamageType.Wind:
                            enemyStats.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Windshear, stacksToAdd, baseDamage, null);
                            break;
                        case HitBox.DamageType.Poison:
                            enemyStats.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Poisoned, stacksToAdd, baseDamage, null);
                            break;
                        case HitBox.DamageType.Bleed:
                            enemyStats.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Bleeding, stacksToAdd, baseDamage, null);
                            break;
                        default:
                            break;
                    }
                }
                enemyStats.TakeDamage(damageDealt, false, damageType, 0, null);
            }
            else
                enemyStats.HealHealth(damageDealt, damageType);
        }
    }
}
