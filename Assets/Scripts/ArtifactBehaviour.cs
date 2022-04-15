using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactBehaviour : MonoBehaviour
{
    [SerializeField] GameObject dissapearParticles;
    [SerializeField] GameObject waypoint;
    GameObject waypointReference;

    private void Start()
    {
        waypointReference = Instantiate(waypoint, new Vector3(9999, 9999, 9999), Quaternion.identity, GameManager.instance.playerUis[0].transform.Find("TemporaryUi"));
        waypointReference.GetComponent<UiFollowTarget>().target = transform.Find("ArtifactWaypointTarget");
        waypointReference.transform.SetAsFirstSibling();
    }

    // Used to activate this artifact. Detroy this object and let the manager know one of the artifacts was gathered.
    public void ActivateArtifact()
    {
        Debug.Log("The artifact was activated");
        GameManager.instance.UpdateObjectiveCount(GameManager.instance.objectiveCurrentProgress + 1);

        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        waypointReference.GetComponent<UiFollowTarget>().RemoveFromCullList();
        Instantiate(dissapearParticles, transform.position, transform.rotation);
        Destroy(waypointReference);
    }
}
