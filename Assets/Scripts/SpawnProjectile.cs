using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnProjectile : MonoBehaviour
{
    public GameObject projectileToSpawn;
    public float spawnCount = 0;
    public float spawnDelay = 0;
    public enum SpawnPattern { Targeted, Ring};
    public SpawnPattern spawnPattern = SpawnPattern.Targeted;

    // Start is called before the first frame update
    void Start()
    {
        if (projectileToSpawn != null)
            StartCoroutine(SpawnProjectiles());
    }

    // Used to satrt the timer to spawn the projectiles.
    IEnumerator SpawnProjectiles()
    {
        yield return new WaitForSeconds(spawnDelay);
        switch (spawnPattern)
        {
            case SpawnPattern.Targeted:
                break;
            case SpawnPattern.Ring:
                for(int index = 0; index < spawnCount; index++)
                {
                    GameObject projectile = Instantiate(projectileToSpawn, transform.position, Quaternion.identity);

                    Vector3 rotation = projectile.transform.rotation.eulerAngles;
                    rotation.y = ((index + 1) / spawnCount) * 360;
                    projectile.transform.rotation = Quaternion.Euler(rotation);

                    projectile.GetComponent<HitBox>().damage = GetComponent<HitBox>().damage;
                    projectile.GetComponent<HitBox>().myStats = GetComponent<HitBox>().myStats;
                }
                break;
            default:
                break;
        }
    }
}
