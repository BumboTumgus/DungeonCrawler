using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiFollowTarget : MonoBehaviour
{
    public Transform target;

    private Camera mainCamera;

    // Grab the camera reference
    private void Start()
    {
        mainCamera = Camera.main;
    }

    // check where we shoudl be in regards to the camera.
    private void Update()
    {
        transform.position = mainCamera.WorldToScreenPoint(target.position);
    }
}
