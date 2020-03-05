using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiFollowTarget : MonoBehaviour
{
    public Transform target;
    public bool hideIfBehindCamera = true;

    private Camera mainCamera;

    // Grab the camera reference
    private void Start()
    {
        mainCamera = Camera.main;
        mainCamera.transform.parent.GetComponent<UiHideBehindPlayer>().targets.Add(this);
    }

    // check where we shoudl be in regards to the camera.
    private void Update()
    {
        transform.position = mainCamera.WorldToScreenPoint(target.position);
    }

    public void RemoveFromCullList()
    {
        mainCamera.transform.parent.GetComponent<UiHideBehindPlayer>().targets.Remove(this);
    }
}
