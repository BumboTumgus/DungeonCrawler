using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    public float projectileSpeed = 5;
    public float projectileLifetime = 10f;
    public bool hitAOE = false;
    public bool trackTarget = false;
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
        if(trackTarget)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation((target.position + Vector3.up) - transform.position), trackingStrength);
        transform.position += transform.forward * projectileSpeed * Time.deltaTime;

        currentLifetime += Time.deltaTime;
        if (currentLifetime > projectileLifetime)
            DestroyProjectile();
    }

    // Used to destroy this projectile.
    public void DestroyProjectile()
    {
        GameObject hitEffects = Instantiate(particleHitEffects, transform.position, transform.rotation);
        if(hitAOE && hitEffects.GetComponent<DestroyAfterTime>().attachedHitBox == true)
        {
            hitEffects.GetComponent<HitBox>().damage = GetComponent<HitBox>().damage;
            hitEffects.GetComponent<HitBox>().myStats = GetComponent<HitBox>().myStats;
        }
        Destroy(gameObject);
    }
}
