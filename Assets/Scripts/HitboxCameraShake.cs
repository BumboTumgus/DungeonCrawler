using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxCameraShake : MonoBehaviour
{
    public float shakeAmount = 0;
    public float flickerDelay = 0;

    private void Awake()
    {
        GetComponent<Collider>().enabled = false;
        StartCoroutine(FlickerCamShakeBox());
    }

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<CameraShakeManager>().AddCameraShake(shakeAmount);
    }

    IEnumerator FlickerCamShakeBox()
    {
        yield return new WaitForSeconds(flickerDelay);

        GetComponent<Collider>().enabled = true;
        yield return new WaitForFixedUpdate();
        GetComponent<Collider>().enabled = false;

    }


}
