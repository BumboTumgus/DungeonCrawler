﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public float damage = 5;
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
    public bool forceChangeDamageColor = false;
    public Color damageColorOverride;

    private void Start()
    {
        // Grab our player stats from our parent.
        if(!projectile && !projectileAOE && !disjointedHitbox)
            myStats = transform.root.GetComponent<PlayerStats>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Projectile logic, we are a projectile and hit an object on the collidable envorioment or interactable layer.
        if ((projectile && other.gameObject.layer == 10) || (projectile && other.gameObject.layer == 9) || (projectile && other.gameObject.layer == 14 && hitEnemies) || (projectile && other.gameObject.layer == 13 && hitPlayers))
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
                bool attackCrit = false;

                float damageDealt = damage;
                if (!damageOverload)
                {
                    // Check to see if the attack hit.
                    damageDealt = myStats.weaponHitbase + myStats.weaponBonusHitBase + Random.Range(0, myStats.weaponHitMax + myStats.weaponBonusHitMax + 1)
                        + myStats.Str * myStats.weaponStrScaling + myStats.Dex * myStats.weaponDexScaling + myStats.Vit * myStats.weaponVitScaling + myStats.Spd * myStats.weaponSpdScaling
                        + myStats.Int * myStats.weaponIntScaling + myStats.Wis * myStats.weaponWisScaling + myStats.Cha * myStats.weaponChaScaling;
                }
                // Debug.Log("we should check the on hits here.");

                if (projectile && procsOnHits || projectileAOE && procsOnHits)
                    myStats.GetComponent<BuffsManager>().ProcOnHits(other.gameObject, this);
                else if (procsOnHits)
                    transform.root.GetComponent<BuffsManager>().ProcOnHits(other.gameObject, this);

                if (myStats.weaponHitspeeds.Count > 0)
                {
                    if (Random.Range(0, 100) >= 100 - myStats.weaponCritChance - myStats.weaponBonusCritChance|| bypassCrit)
                    {
                        damageDealt *= (myStats.weaponCritMod + myStats.weaponBonusCritMod);
                        attackCrit = true;
                        bypassCrit = false;
                    }
                }
                else
                {
                    if (Random.Range(0, 100) >= 95 || bypassCrit)
                    {
                        damageDealt *= 2;
                        attackCrit = true;
                        bypassCrit = false;
                    }
                }


                damageDealt -= enemyStats.armor;
                if(!forceChangeDamageColor)
                    enemyStats.TakeDamage(damageDealt, attackCrit);
                else
                    enemyStats.TakeDamage(damageDealt, attackCrit, damageColorOverride);
            }
            // Player Logic
            else if (other.CompareTag("Player") && hitPlayers)
            {
                //Debug.Log("we have hit a player");
                enemyStats = other.GetComponent<PlayerStats>();
                bool attackCrit = false;

                float damageDealt = damage;
                if (!damageOverload)
                {
                    // Check to see if the attack hit.
                    damageDealt = myStats.weaponHitbase + myStats.weaponBonusHitBase + Random.Range(0, myStats.weaponHitMax + myStats.weaponBonusHitMax + 1)
                        + myStats.Str * myStats.weaponStrScaling + myStats.Dex * myStats.weaponDexScaling + myStats.Vit * myStats.weaponVitScaling + myStats.Spd * myStats.weaponSpdScaling
                        + myStats.Int * myStats.weaponIntScaling + myStats.Wis * myStats.weaponWisScaling + myStats.Cha * myStats.weaponChaScaling;
                }

                if (Random.Range(0, 100) >= 100 - myStats.weaponCritChance - myStats.weaponBonusCritChance)
                {
                    damageDealt *= (myStats.weaponCritMod + myStats.weaponBonusCritMod);
                    attackCrit = true;
                }
                damageDealt -= enemyStats.armor;
                //Debug.Log("the player shall take: " + damageDealt + " damage");
                if (!forceChangeDamageColor)
                    enemyStats.TakeDamage(damageDealt, attackCrit);
                else
                    enemyStats.TakeDamage(damageDealt, attackCrit, damageColorOverride);
            }
        }
    }
}
