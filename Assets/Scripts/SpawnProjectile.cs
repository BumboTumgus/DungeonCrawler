using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnProjectile : MonoBehaviour
{
    public GameObject projectileToSpawn;
    public float spawnCount = 0;
    public float spawnDelay = 0;
    public bool repeatShotPattern = false;
    public bool projectilesTrack = false;
    public enum SpawnPattern { Targeted, Ring, Random};
    public SpawnPattern spawnPattern = SpawnPattern.Targeted;

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        if (projectileToSpawn != null)
            StartCoroutine(SpawnProjectiles());
        audioSource = GetComponent<AudioSource>();
    }

    // Used to satrt the timer to spawn the projectiles.
    IEnumerator SpawnProjectiles()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnDelay);
            switch (spawnPattern)
            {
                case SpawnPattern.Targeted:
                    break;
                case SpawnPattern.Ring:
                    for (int index = 0; index < spawnCount; index++)
                    {
                        GameObject projectile = Instantiate(projectileToSpawn, transform.position, Quaternion.identity);

                        Vector3 rotation = projectile.transform.rotation.eulerAngles;
                        rotation.y = ((index + 1) / spawnCount) * 360;
                        projectile.transform.rotation = Quaternion.Euler(rotation);

                        projectile.GetComponent<HitBox>().damage = GetComponent<HitBox>().damage;
                        projectile.GetComponent<HitBox>().myStats = GetComponent<HitBox>().myStats;
                        if (projectilesTrack)
                            projectile.GetComponent<ProjectileBehaviour>().target = GetComponent<HitBox>().enemyStats.transform;
                    }
                    break;
                case SpawnPattern.Random:
                    {
                        for (int index = 0; index < spawnCount; index++)
                        {
                            GameObject projectile = Instantiate(projectileToSpawn, transform.position, Quaternion.identity);

                            Vector3 rotation = projectile.transform.rotation.eulerAngles;
                            rotation = new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
                            projectile.transform.rotation = Quaternion.Euler(rotation);

                            projectile.GetComponent<HitBox>().damage = GetComponent<HitBox>().damage;
                            projectile.GetComponent<HitBox>().myStats = GetComponent<HitBox>().myStats;
                            if (projectilesTrack)
                                projectile.GetComponent<ProjectileBehaviour>().target = GetComponent<HitBox>().enemyStats.transform;
                        }
                    }
                    break;
                default:
                    break;
            }

            if (audioSource)
                audioSource.Play();

            if (!repeatShotPattern)
                break;
        }
    }
}
