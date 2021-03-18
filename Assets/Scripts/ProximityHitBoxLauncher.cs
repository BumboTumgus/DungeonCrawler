using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityHitBoxLauncher : MonoBehaviour
{
    public GameObject explosionToInstantiate;
    public float triggerDelay = 1f;

    private HitBox myHitbox;


    private void Start()
    {
        myHitbox = GetComponent<HitBox>();
        GetComponent<Collider>().enabled = false;
        StartCoroutine(TriggerDelay());
    }

    IEnumerator TriggerDelay()
    {
        yield return new WaitForSeconds(triggerDelay);
        GetComponent<Collider>().enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (myHitbox.hitEnemies && other.gameObject.CompareTag("Enemy"))
            LaunchExplosion();
        else if (myHitbox.hitPlayers && other.gameObject.CompareTag("Player"))
            LaunchExplosion();
    }

    public void LaunchExplosion()
    {
        GameObject explosion = Instantiate(explosionToInstantiate, transform.position + Vector3.up, Quaternion.identity);
        explosion.GetComponent<HitBox>().damage = myHitbox.damage;
        explosion.GetComponent<HitBox>().myStats = myHitbox.myStats;

        Destroy(gameObject);
    }
}
