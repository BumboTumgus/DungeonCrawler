using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellMirrorManager : MonoBehaviour
{
    public Vector3 targetToFace = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation((targetToFace + Vector3.up) - transform.position), 0.01f);
    }
}
