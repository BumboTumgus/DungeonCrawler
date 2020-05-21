using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapOscillation : MonoBehaviour
{
    public float trapArmedTimer = 2;
    public float trapDisarmedTimer = 2;
    public float trapArmedOffset = 0;

    public ParticleSystem ps;
    public GameObject hitbox;

    private float currentTimer = 0;
    private bool trapArmed = false;
    // Start is called before the first frame update
    void Start()
    {
        currentTimer = trapArmedOffset;
        hitbox.SetActive(false);
        ps.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        currentTimer += Time.deltaTime;
        if(currentTimer > trapArmedTimer && !trapArmed)
        {
            currentTimer = 0;
            LaunchTrap();
            trapArmed = true;
        }
        else if(currentTimer > trapArmedTimer && trapArmed)
        {
            currentTimer = 0;
            DisableTrap();
            trapArmed = false;
        }
    }

    // Used to enable the traps
    private void LaunchTrap()
    {
        ps.Play();
        hitbox.SetActive(true);
        if (hitbox.GetComponent<HitBoxFlickerer>() != null)
            hitbox.GetComponent<HitBoxFlickerer>().StartFlicker();
    }

    // Used to disable the traps
    private void DisableTrap()
    {
        ps.Stop();
        hitbox.SetActive(false);
    }
}
