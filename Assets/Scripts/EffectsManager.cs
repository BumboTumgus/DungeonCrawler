using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsManager : MonoBehaviour
{
    public enum ParticleType { Affliction, Spell, Buff, Basic}

    public ParticleSystem[] afflictions;
    public ParticleSystem[] spells;
    public ParticleSystem[] buffs;
    public ParticleSystem[] basics;

    // Start is called before the first frame update
    void Start()
    {
        foreach (ParticleSystem ps in afflictions)
            HideParticles(ps);
        foreach (ParticleSystem ps in spells)
            HideParticles(ps);
        foreach (ParticleSystem ps in buffs)
            HideParticles(ps);
        foreach (ParticleSystem ps in basics)
            HideParticles(ps);
    }

    // Used to start the particles at a certain index.
    public void Play(int index, ParticleType psType)
    {
        switch (psType)
        {
            case ParticleType.Affliction:
                LaunchParticles(afflictions[index]);
                break;
            case ParticleType.Spell:
                LaunchParticles(spells[index]);
                break;
            case ParticleType.Buff:
                LaunchParticles(buffs[index]);
                break;
            case ParticleType.Basic:
                LaunchParticles(basics[index]);
                break;
            default:
                break;
        }
    }

    // Used to hide the particles at a certain index.
    public void Stop(int index, ParticleType psType)
    {
        switch (psType)
        {
            case ParticleType.Affliction:
                HideParticles(afflictions[index]);
                break;
            case ParticleType.Spell:
                HideParticles(spells[index]);
                break;
            case ParticleType.Buff:
                HideParticles(buffs[index]);
                break;
            case ParticleType.Basic:
                HideParticles(basics[index]);
                break;
            default:
                break;
        }
    }

    // USed to Launch the particles
    private void LaunchParticles(ParticleSystem ps)
    {
        ps.Play();
        foreach(Transform child in ps.transform)
            GetComponent<ParticleSystem>().Play();
    }

    // USed to Hide the particles.
    private void HideParticles(ParticleSystem ps)
    {
        ps.Stop();
        foreach (Transform child in ps.transform)
            GetComponent<ParticleSystem>().Stop();
    }
}
