using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxManager : MonoBehaviour
{
    public List<GameObject> hitboxes = new List<GameObject>();
    public List<GameObject> buffboxes = new List<GameObject>();
    public List<ParticleSystem> hiteffects = new List<ParticleSystem>();

    private PlayerStats playerStats;

    // USed to disable all hitboxes at the start of the game.
    private void Start()
    {
        playerStats = GetComponent<PlayerStats>();

        foreach (GameObject hitbox in hitboxes)
            hitbox.GetComponent<Collider>().enabled = false;
        foreach (GameObject hitbox in buffboxes)
            hitbox.GetComponent<Collider>().enabled = false;
        foreach (ParticleSystem ps in hiteffects)
            ps.Stop();
    }

    // A public method soley used to launch the hit box from another class.
    public void LaunchHitBox(int index)
    {
        //Debug.Log("we are launching a hitbox");
        StartCoroutine(HitBoxFlicker(index));
    }

    // A public method soley used to launch the buff box from another class.
    public void LaunchBuffBox(int index)
    {
        //Debug.Log("launching buff box");
        if (buffboxes[index].GetComponent<HitBoxBuff>().hitSelf)
            buffboxes[index].GetComponent<HitBoxBuff>().BuffSelf();

        StartCoroutine(BuffBoxFlicker(index));
    }

    // Used to play the particles in one of our particle systems.
    public void PlayParticles(int index)
    {
        //Debug.Log("Playing particles " + index);
        hiteffects[index].Play();
    }

    // Used to stop the particles in one of our particle systems.
    public void StopParticles(int index)
    {
        //Debug.Log("Stopping particles " + index);
        hiteffects[index].Stop();
    }

    // Used to stop the particles in one of our particle systems.
    public void StopAndClearParticles(int index)
    {
        //Debug.Log("Stopping particles " + index);
        hiteffects[index].Clear();
        hiteffects[index].Stop();
    }

    // Used to launch an attack
    IEnumerator HitBoxFlicker(int index)
    {

        if (hitboxes[index].GetComponent<HitBox>() != null)
        {
            hitboxes[index].GetComponent<HitBox>().critRolled = false;
            hitboxes[index].GetComponent<HitBox>().crit = false;
        }

        //Debug.Log("we are flickering hitbox " + index);
        hitboxes[index].GetComponent<Collider>().enabled = true;
        yield return new WaitForFixedUpdate();
        hitboxes[index].GetComponent<Collider>().enabled = false;
    }

    // Used to launch a buff
    IEnumerator BuffBoxFlicker(int index)
    {
        //Debug.Log("flciekred on");
        buffboxes[index].GetComponent<Collider>().enabled = true;
        yield return new WaitForFixedUpdate();
        buffboxes[index].GetComponent<Collider>().enabled = false;
        //Debug.Log("flciekred off");
    }
}
