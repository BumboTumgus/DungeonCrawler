using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceNearestPlayerBehaviour : MonoBehaviour
{
    public Transform playerToFace;

    private const float ROTATION_SPEED = 2f;
    private const float MAX_UPDATE_RANGE = 75;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FaceTarget());
    }

    IEnumerator FaceTarget()
    {
        while(enabled)
        {
            if ((playerToFace.position - transform.position).sqrMagnitude <= MAX_UPDATE_RANGE * MAX_UPDATE_RANGE)
            {
                Quaternion newRotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(transform.position - (playerToFace.position + Vector3.up)), ROTATION_SPEED * Time.deltaTime);
                transform.rotation = newRotation;
            }

            yield return new WaitForEndOfFrame();
        }
    }
}