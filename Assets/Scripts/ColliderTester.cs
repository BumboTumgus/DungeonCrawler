using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderTester : MonoBehaviour
{
    Collider col;

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<Collider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(string.Format("A collision was noticed between {0} of layer {1} and {2} of layer {3}.", col.name, col.gameObject.layer, collision.transform.name, collision.transform.gameObject.layer));
    }
}
