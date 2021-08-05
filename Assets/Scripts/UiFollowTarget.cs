using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiFollowTarget : MonoBehaviour
{
    public Transform target;
    public bool hideIfBehindCamera = true;
    public bool ignoreCameraDistanceCull = false;

    private Camera mainCamera;
    private Coroutine ignoreDistanceCullCoroutine;

    // Grab the camera reference
    private void Start()
    {
        mainCamera = GameManager.instance.playerCameras[0].GetComponentInChildren<Camera>();
        mainCamera.GetComponent<UiHideBehindPlayer>().targets.Add(this);
    }

    // check where we shoudl be in regards to the camera.
    private void Update()
    {
        transform.position = mainCamera.WorldToScreenPoint(target.position);
    }

    public void RemoveFromCullList()
    {
        mainCamera.GetComponent<UiHideBehindPlayer>().targets.Remove(this);
    }

    public void TriggerIgnoreCameraDistanceCull()
    {
        if (ignoreDistanceCullCoroutine != null)
            StopCoroutine(ignoreDistanceCullCoroutine);

        gameObject.SetActive(true);
        ignoreDistanceCullCoroutine = StartCoroutine(IgnoreCameraDistanceCull());
    }

    IEnumerator IgnoreCameraDistanceCull()
    {
        ignoreCameraDistanceCull = true;

        yield return new WaitForSeconds(5);

        ignoreCameraDistanceCull = false;
    }
}
