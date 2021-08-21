﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class HitBoxFlickerer : MonoBehaviour
{
    private Collider hitbox;
    private bool flickerHitBox = true;
    [SerializeField] private float targetTimer = 0.1f;
    [SerializeField] private bool playConnectedAudio = false;

    // Start is called before the first frame update
    void Start()
    {
        hitbox = GetComponent<Collider>();
        StartCoroutine(FlickerHitBox());
    }

    public void StartFlicker()
    {
        if(hitbox == null)
            hitbox = GetComponent<Collider>();
        StopAllCoroutines();
        StartCoroutine(FlickerHitBox());
    }

    IEnumerator FlickerHitBox()
    {
        hitbox.enabled = false;

        while(flickerHitBox)
        {
            float currentTimer = 0;
            while(currentTimer < targetTimer)
            {
                currentTimer += Time.deltaTime;
                if(currentTimer > targetTimer)
                {
                    if (playConnectedAudio)
                        GetComponent<AudioSource>().Play();

                    hitbox.enabled = true;
                    yield return new WaitForFixedUpdate();
                    hitbox.enabled = false;
                }
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
