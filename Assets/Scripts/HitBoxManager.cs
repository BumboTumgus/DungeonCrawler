using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxManager : MonoBehaviour
{
    public List<GameObject> hitboxes = new List<GameObject>();
    public List<GameObject> buffboxes = new List<GameObject>();
    private PlayerStats playerStats;

    // USed to disable all hitboxes at the start of the game.
    private void Start()
    {
        playerStats = GetComponent<PlayerStats>();

        foreach (GameObject hitbox in hitboxes)
            hitbox.GetComponent<SphereCollider>().enabled = false;
        foreach (GameObject hitbox in buffboxes)
            hitbox.GetComponent<SphereCollider>().enabled = false;
    }

    // A public method soley used to launch the hit box from another class.
    public void LaunchHitBox(int index)
    {
        //Debug.Log("we are launching a hitbox");
        if (index == 0 && playerStats.bleeding)
            playerStats.TakeDamage(playerStats.healthMax * 0.1f, false, GetComponent<BuffsManager>().damageColors[2]);
        StartCoroutine(HitBoxFlicker(index));
    }

    // A public method soley used to launch the buff box from another class.
    public void LaunchBuffBox(int index)
    {
        //Debug.Log("launching buff box");
        StartCoroutine(BuffBoxFlicker(index));
    }

    // Used to launch an attack
    IEnumerator HitBoxFlicker(int index)
    {
        //Debug.Log("we are flickering hitbox " + index);
        hitboxes[index].GetComponent<SphereCollider>().enabled = true;
        yield return new WaitForFixedUpdate();
        hitboxes[index].GetComponent<SphereCollider>().enabled = false;
    }

    // Used to launch a buff
    IEnumerator BuffBoxFlicker(int index)
    {
        //Debug.Log("flciekred on");
        buffboxes[index].GetComponent<SphereCollider>().enabled = true;
        yield return new WaitForFixedUpdate();
        buffboxes[index].GetComponent<SphereCollider>().enabled = false;
        //Debug.Log("flciekred off");
    }
}
