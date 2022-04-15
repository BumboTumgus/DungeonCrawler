using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterBehaviour : MonoBehaviour
{
    public bool teleporterActive = false;
    [SerializeField] ParticleSystem teleporterParticles;
    [SerializeField] GameObject teleporterTriggerBox;
    [SerializeField] GameObject teleporterWaypoint;

    private void Start()
    {
        teleporterParticles.Stop();
        teleporterTriggerBox.SetActive(false);
    }

    /*
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha9))
            StartTeleporter();
    }
    */


    // Called by the manager when we are allowed to teleport. Enable the collider and start the particles. Create our waypoint too.
    public void StartTeleporter()
    {
        teleporterActive = true;
        teleporterTriggerBox.SetActive(true);
        teleporterParticles.Play();
        GetComponent<AudioSource>().Play();

        GameObject waypoint = Instantiate(teleporterWaypoint, new Vector3(9999, 9999, 9999), Quaternion.identity, GameManager.instance.playerUis[0].transform.Find("TemporaryUi"));
        waypoint.GetComponent<UiFollowTarget>().target = transform.Find("TeleporterWaypointTarget");
        waypoint.transform.SetAsFirstSibling();
    }

    // Called by the manager when the teleporter has been interacted with and were teleporting to the next level.
    public void DisableTriggerBox()
    {
        teleporterTriggerBox.SetActive(false);
    }

}
