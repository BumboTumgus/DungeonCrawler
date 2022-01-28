using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    public float projectileSpeed = 5;
    public float projectileSpeedDecay = 0f;
    public float projectileSizeIncrease = 0f;
    public float projectileLifetime = 10f;
    public bool piercesTargets = false;
    public bool piercesWalls = false;
    public bool hitAOE = false;
    public bool trackTarget = false;
    public float YSpin = 0;
    public float gravModifier = 0;
    private float currentGravMultiplier = 0;
    public float trackingStrength = 0.1f;
    public Transform target = null;
    public List<ParticleSystem> particleTrails = new List<ParticleSystem>();
    public GameObject particleSpawnEffects;
    public GameObject particleHitEffects;

    private float currentLifetime = 0;

    // Start is called before the first frame update
    void Start()
    {
        foreach (ParticleSystem ps in particleTrails)
            ps.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if(trackTarget && target != null)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation((target.position + Vector3.up) - transform.position), trackingStrength);

        transform.position += transform.forward * projectileSpeed * Time.deltaTime + Vector3.down * currentGravMultiplier;

        if(YSpin != 0)
        {
            Vector3 rotation = transform.rotation.eulerAngles;
            rotation.y += YSpin * Time.deltaTime;
            transform.rotation = Quaternion.Euler(rotation);
            YSpin *= 0.995f;
        }

        if(gravModifier != 0 && !trackTarget)
        {
            currentGravMultiplier += gravModifier * Time.deltaTime;
        }

        if (projectileSpeedDecay > 0)
        {
            projectileSpeed = Mathf.Lerp(projectileSpeed, 0, projectileSpeedDecay);

            if(projectileSpeed <= 0.1f)
            {
                projectileSpeed = 0;
                projectileSpeedDecay = 0;
            }
        }

        if(projectileSizeIncrease != 0)
        {
            transform.localScale *= 1 + (projectileSizeIncrease * Time.deltaTime);
        }

        currentLifetime += Time.deltaTime;
        if (currentLifetime > projectileLifetime)
            DestroyProjectile();
    }

    // Used to destroy this projectile.
    public void DestroyProjectile()
    {
        if (particleHitEffects)
        {
            GameObject hitEffects = Instantiate(particleHitEffects, transform.position, transform.rotation);
            if (hitAOE && hitEffects.GetComponent<DestroyAfterTime>().attachedHitBox == true)
            {
                hitEffects.GetComponent<HitBox>().damage = GetComponent<HitBox>().damage;
                hitEffects.GetComponent<HitBox>().myStats = GetComponent<HitBox>().myStats;

                if (hitEffects.GetComponent<HitBoxBuff>() != null)
                    hitEffects.GetComponent<HitBoxBuff>().buffOrigin = GetComponent<HitBox>().myStats;
            }
        }
        Destroy(gameObject);
    }
}
