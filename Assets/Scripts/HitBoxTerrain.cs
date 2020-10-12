using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HitBoxTerrain : MonoBehaviour
{
    public HitBox.DamageType damageType;
    public List<ParticleSystem> particlesWindup = new List<ParticleSystem>();
    public List<ParticleSystem> particlesAppear = new List<ParticleSystem>();
    public List<ParticleSystem> particlesDissapear = new List<ParticleSystem>();
    public float damage = 5;
    public bool crit = false;
    public bool procsOnHits = false;
    public float dissapearParticleDuration = 1f;
    public float terrainDuration = 10;
    public float hitBoxLaunchTime = 0.5f;
    // public float rebakeNavMeshTime = 1f;

    [SerializeField] private bool hitEnemies = false;
    [SerializeField] private bool hitPlayers = false;
    
    public PlayerStats myStats;
    public PlayerStats enemyStats;
    public bool bypassCrit = false;

    private Animator anim;

    // Used to Launch the lifetime coroutine.
    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        Collider[] hitboxes = GetComponents<Collider>();
        foreach (Collider hitbox in hitboxes)
            hitbox.enabled = false;
        foreach (ParticleSystem ps in particlesWindup)
            ps.Stop();
        foreach (ParticleSystem ps in particlesAppear)
            ps.Stop();
        foreach (ParticleSystem ps in particlesDissapear)
            ps.Stop();
        StartCoroutine(TerrainTimeOut());
    }

    // Used to laucnh the hitbox 
    public void LaunchHitBox()
    {
        StartCoroutine(HitBoxFlicker());
    }

    // Used to rebake the nav mesh so eneies will path around this object.
    public void RebakeNavMesh()
    {
        Debug.Log("Rebaking ther navmesh BAYBE");
        FindObjectOfType<NavMeshSurface>().BuildNavMesh();
    }

    // Used to check the terrains lifetime then play the dissapearing animation after it times out.
    IEnumerator TerrainTimeOut()
    {
        float currentTimer = 0;
        bool launchedHitBox = false;
        foreach (ParticleSystem ps in particlesWindup)
            ps.Play();
        // bool rebakedNavMesh = false;

        while(currentTimer < terrainDuration)
        {
            if(!launchedHitBox && currentTimer > hitBoxLaunchTime)
            {
                foreach (ParticleSystem ps in particlesWindup)
                    ps.Stop();
                foreach (ParticleSystem ps in particlesAppear)
                    ps.Play();
                launchedHitBox = true;
                LaunchHitBox();
            }
            /**
            if(!rebakedNavMesh && currentTimer > rebakeNavMeshTime)
            {
                rebakedNavMesh = true;
                RebakeNavMesh();
            }
            */
            currentTimer += Time.deltaTime;
            yield return null;
        }

        anim.SetTrigger("HideObject");
        currentTimer = 0;
        terrainDuration = 4;
        foreach (ParticleSystem ps in particlesDissapear)
            ps.Play();
        launchedHitBox = false;

        while (currentTimer < terrainDuration)
        {
            if(!launchedHitBox && currentTimer > dissapearParticleDuration)
            {
                foreach (ParticleSystem ps in particlesDissapear)
                    ps.Stop();
            }
            currentTimer += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }

    // Used to launch an attack
    IEnumerator HitBoxFlicker()
    {
        Collider[] hitboxes = GetComponents<Collider>();

        foreach (Collider hitbox in hitboxes)
            hitbox.enabled = true;

        yield return new WaitForFixedUpdate();
        
        foreach (Collider hitbox in hitboxes)
            hitbox.enabled = false;
    }

    // The hit logic.
    private void OnTriggerEnter(Collider other)
    {
        // Enemy Logic
        if (other.CompareTag("Enemy") && hitEnemies)
        {
            enemyStats = other.GetComponent<PlayerStats>();
            bool attackCrit = false;

            float damageDealt = damage;

            if (procsOnHits)
                myStats.transform.root.GetComponent<BuffsManager>().ProcOnHits(other.gameObject, this);

            /*
            if (Random.Range(0, 100) >= 100 - myStats.weaponCritChance - myStats.weaponBonusCritMod || bypassCrit)
            {
                damageDealt *= (myStats.weaponCritMod + myStats.weaponBonusCritMod);
                attackCrit = true;
                bypassCrit = false;
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
                case HitBox.DamageType.Physical:
                    damageDealt -= enemyStats.armor;
                    break;
                case HitBox.DamageType.Magical:
                    damageDealt -= enemyStats.magicResist;
                    break;
                case HitBox.DamageType.True:
                    break;
                case HitBox.DamageType.Healing:
                    break;
                case HitBox.DamageType.PhysicalCrit:
                    damageDealt -= enemyStats.armor;
                    break;
                case HitBox.DamageType.MagicalCrit:
                    damageDealt -= enemyStats.magicResist;
                    break;
                case HitBox.DamageType.HealingCrit:
                    break;
                default:
                    break;
            }
            if (damageType != HitBox.DamageType.Healing && damageType != HitBox.DamageType.HealingCrit)
                enemyStats.TakeDamage(damageDealt, attackCrit, damageType);
            else
                enemyStats.HealHealth(damageDealt, attackCrit, damageType);
        }
        // Player Logic
        else if (other.CompareTag("Player") && hitPlayers)
        {
            enemyStats = other.GetComponent<PlayerStats>();
            bool attackCrit = false;

            float damageDealt = damage;

            /*
            if (myStats.weaponHitspeeds.Count > 0)
            {
                if (Random.Range(0, 100) >= 100 - myStats.weaponCritChance - myStats.weaponBonusCritMod || bypassCrit)
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
                case HitBox.DamageType.Physical:
                    damageDealt -= enemyStats.armor;
                    break;
                case HitBox.DamageType.Magical:
                    damageDealt -= enemyStats.magicResist;
                    break;
                case HitBox.DamageType.True:
                    break;
                case HitBox.DamageType.Healing:
                    break;
                case HitBox.DamageType.PhysicalCrit:
                    damageDealt -= enemyStats.armor;
                    break;
                case HitBox.DamageType.MagicalCrit:
                    damageDealt -= enemyStats.magicResist;
                    break;
                case HitBox.DamageType.HealingCrit:
                    break;
                default:
                    break;
            }
            if (damageType != HitBox.DamageType.Healing && damageType != HitBox.DamageType.HealingCrit)
                enemyStats.TakeDamage(damageDealt, attackCrit, damageType);
            else
                enemyStats.HealHealth(damageDealt, attackCrit, damageType);
        }
    }
   
}
