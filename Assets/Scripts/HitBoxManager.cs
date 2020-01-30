using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxManager : MonoBehaviour
{
    public List<GameObject> hitboxes = new List<GameObject>();

    // USed to disable all hitboxes at the start of the game.
    private void Start()
    {
        foreach (GameObject hitbox in hitboxes)
            hitbox.GetComponent<SphereCollider>().enabled = false;
    }

    // A public method soley used to launch the hit box from another class.
    public void LaunchHitBox(int index)
    {
        StartCoroutine(HitBoxFlicker(index));
    }

    // Used to launch an attack
    IEnumerator HitBoxFlicker(int index)
    {
        hitboxes[index].GetComponent<SphereCollider>().enabled = true;
        yield return new WaitForFixedUpdate();
        hitboxes[index].GetComponent<SphereCollider>().enabled = false;
    }
}
