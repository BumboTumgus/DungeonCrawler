using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public float damage = 5;
    public bool crit = false;
    public float stagger = 5;

    [SerializeField] private bool hitEnemies = false;
    [SerializeField] private bool hitPlayers = false;

    private PlayerStats myStats;
    private PlayerStats enemyStats;

    private void Start()
    {
        // Grab our player stats from our parent.
        myStats = transform.root.GetComponent<PlayerStats>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Enemy Logic
        if (other.CompareTag("Enemy") && hitEnemies)
        {
            enemyStats = other.GetComponent<PlayerStats>();
            bool attackCrit = false;

            // Check to see if the attack hit.
            float damageDealt = myStats.weaponHitbase + Random.Range(myStats.weaponHitMin, myStats.weaponHitMax + 1) 
                + myStats.Str * myStats.weaponStrScaling + myStats.Dex * myStats.weaponDexScaling + myStats.Vit * myStats.weaponVitScaling + myStats.Spd * myStats.weaponSpdScaling
                + myStats.Int * myStats.weaponIntScaling + myStats.Wis * myStats.weaponWisScaling + myStats.Cha * myStats.weaponChaScaling;

            if (Random.Range(0, 100) >= 100 - myStats.weaponCritChance)
            {
                damageDealt *= myStats.weaponCritMod;
                attackCrit = true;
            }

            damageDealt -= enemyStats.armor;
            enemyStats.TakeDamage(damageDealt, attackCrit);
        }
        // Player Logic
        else if (other.CompareTag("Player") && hitPlayers)
        {
            enemyStats = other.GetComponent<PlayerStats>();
            bool attackCrit = false;

            // Check to see if the attack hit.
            float damageDealt = myStats.weaponHitbase + Random.Range(myStats.weaponHitMin, myStats.weaponHitMax + 1) +
                + myStats.Str * myStats.weaponStrScaling + myStats.Dex * myStats.weaponDexScaling + myStats.Vit * myStats.weaponVitScaling + myStats.Spd * myStats.weaponSpdScaling
                + myStats.Int * myStats.weaponIntScaling + myStats.Wis * myStats.weaponWisScaling + myStats.Cha * myStats.weaponChaScaling;

            if (Random.Range(0, 100) >= 100 - myStats.weaponCritChance)
            {
                damageDealt *= myStats.weaponCritMod;
                attackCrit = true;
            }

            damageDealt -= enemyStats.armor;
            enemyStats.TakeDamage(damageDealt, attackCrit);
        }
    }
}
