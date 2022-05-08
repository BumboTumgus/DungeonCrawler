using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrineBehaviour : MonoBehaviour
{
    public bool interactable = true;

    [SerializeField] GameObject[] gameObjectsToHideOnUse;
    [SerializeField] ParticleSystem[] particleSystemsToPlayOnUse;
    [SerializeField] bool reusable = false;

    private const float REUSE_COOLDOWN = 1f;

    public virtual void OnInteract(GameObject player)
    {
        if (reusable)
            StartCoroutine(ReuseCooldown());

        if (gameObjectsToHideOnUse.Length > 0)
        {
            foreach (GameObject go in gameObjectsToHideOnUse)
                go.SetActive(false);
        }

        if(particleSystemsToPlayOnUse.Length > 0)
        {
            foreach (ParticleSystem ps in particleSystemsToPlayOnUse)
                ps.Play();
        }

        Collider[] cols = GetComponentsInChildren<Collider>();
        foreach (Collider col in cols)
            if (col.isTrigger)
                col.enabled = false;
    }

    private IEnumerator ReuseCooldown()
    {
        reusable = false;
        yield return new WaitForSeconds(REUSE_COOLDOWN);
        reusable = true;

        Collider[] cols = GetComponentsInChildren<Collider>();
        foreach (Collider col in cols)
            if (col.isTrigger)
                col.enabled = true;
    }

}
