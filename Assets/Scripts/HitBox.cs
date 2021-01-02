using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public enum DamageType {Physical, Magical, True, Healing, PhysicalCrit, MagicalCrit, HealingCrit}
    public DamageType damageType;

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
        //Debug.Log("we have colldied with collided with: " + other.name);
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

                if (enemyStats.ephemeral)
                    return;
                else if (enemyStats.invulnerable)
                    return;

                bool attackCrit = false;

                float damageDealt = damage;
                if (!damageOverload)
                {
                    // Check to see if the attack hit.
                    damageDealt = myStats.baseDamage * (myStats.baseDamageScaling + ((float)myStats.Str * myStats.weaponStrScaling) + ((float)myStats.Dex * myStats.weaponDexScaling) + ((float)myStats.Vit * myStats.weaponVitScaling) + ((float)myStats.Spd * myStats.weaponSpdScaling) + ((float)myStats.Int * myStats.weaponIntScaling) + ((float)myStats.Wis * myStats.weaponWisScaling) + ((float)myStats.Cha * myStats.weaponChaScaling));
                }
                // Debug.Log("we should check the on hits here.");

                if (projectile && procsOnHits || projectileAOE && procsOnHits)
                    myStats.GetComponent<BuffsManager>().ProcOnHits(other.gameObject, this);
                else if (procsOnHits)
                    transform.root.GetComponent<BuffsManager>().ProcOnHits(other.gameObject, this);

                /*
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
                */
                if (bypassCrit)
                {
                    attackCrit = true;
                    damageDealt *= 2;
                    bypassCrit = false;
                }

                switch (damageType)
                {
                    case DamageType.Physical:
                        damageDealt -= enemyStats.armor;
                        break;
                    case DamageType.Magical:
                        damageDealt -= enemyStats.magicResist;
                        break;
                    case DamageType.True:
                        break;
                    case DamageType.PhysicalCrit:
                        damageDealt -= enemyStats.armor;
                        break;
                    case DamageType.MagicalCrit:
                        damageDealt -= enemyStats.magicResist;
                        break;
                    default:
                        break;
                }

                //if(!forceChangeDamageColor)
                if (damageType != DamageType.Healing && damageType != DamageType.HealingCrit)
                    enemyStats.TakeDamage(damageDealt, attackCrit, damageType);
                else
                    enemyStats.HealHealth(damageDealt, attackCrit, damageType);
                //else
                    //enemyStats.TakeDamage(damageDealt, attackCrit, damageColorOverride);
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
                if (!damageOverload)
                {
                    // Check to see if the attack hit.
                    //Debug.Log(myStats);
                    damageDealt = myStats.baseDamage * (myStats.baseDamageScaling + ((float)myStats.Str * myStats.weaponStrScaling) + ((float)myStats.Dex * myStats.weaponDexScaling) + ((float)myStats.Vit * myStats.weaponVitScaling) + ((float)myStats.Spd * myStats.weaponSpdScaling) + ((float)myStats.Int * myStats.weaponIntScaling) + ((float)myStats.Wis * myStats.weaponWisScaling) + ((float)myStats.Cha * myStats.weaponChaScaling));
                }

                /*
                if (!bypassCrit && Random.Range(0, 100) >= 100 - myStats.weaponCritChance - myStats.weaponBonusCritChance)
                {
                    damageDealt *= (myStats.weaponCritMod + myStats.weaponBonusCritMod);
                    attackCrit = true;
                }
                */
                if (bypassCrit)
                {
                    attackCrit = true;
                    damageDealt *= 2;
                    bypassCrit = false;
                }

                switch (damageType)
                {
                    case DamageType.Physical:
                        damageDealt -= enemyStats.armor;
                        break;
                    case DamageType.Magical:
                        damageDealt -= enemyStats.magicResist;
                        break;
                    case DamageType.True:
                        break;
                    case DamageType.Healing:
                        break;
                    case DamageType.PhysicalCrit:
                        damageDealt -= enemyStats.armor;
                        break;
                    case DamageType.MagicalCrit:
                        damageDealt -= enemyStats.magicResist;
                        break;
                    case DamageType.HealingCrit:
                        break;
                    default:
                        break;
                }
                //Debug.Log("the player shall take: " + damageDealt + " damage");
                //if (!forceChangeDamageColor)
                if (damageType != DamageType.Healing && damageType != DamageType.HealingCrit)
                    enemyStats.TakeDamage(damageDealt, attackCrit, damageType);
                else
                    enemyStats.HealHealth(damageDealt, attackCrit, damageType);
                //else
                //enemyStats.TakeDamage(damageDealt, attackCrit, damageColorOverride);
            }
        }
    }
}
