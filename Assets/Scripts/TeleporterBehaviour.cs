using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterBehaviour : MonoBehaviour
{
    public bool teleporterActive = false;
    [SerializeField] ParticleSystem teleporterParticles;
    [SerializeField] GameObject teleporterTriggerBox;

    private void Start()
    {
        teleporterParticles.Stop();
        teleporterTriggerBox.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha9))
            StartTeleporter();
    }


    // Called by the manager when we are allowed to teleport. Enable the collider and start the particles.
    public void StartTeleporter()
    {
        teleporterActive = true;
        teleporterTriggerBox.SetActive(true);
        teleporterParticles.Play();
    }

}
