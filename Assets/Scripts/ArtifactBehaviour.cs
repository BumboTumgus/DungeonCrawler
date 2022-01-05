using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactBehaviour : MonoBehaviour
{
    [SerializeField] GameObject dissapearParticles;

    // Used to activate this artifact. Detroy this object and let the manager know one of the artifacts was gathered.
    public void ActivateArtifact()
    {
        Debug.Log("The artifact was activated");
        GameManager.instance.UpdateObjectiveCount(GameManager.instance.objectiveCurrentProgress + 1);

        Instantiate(dissapearParticles, transform.position, transform.rotation);

        Destroy(gameObject);
    }
}
