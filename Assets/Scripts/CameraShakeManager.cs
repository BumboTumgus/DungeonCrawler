using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeManager : MonoBehaviour
{
    public float cameraShakeAmount = 0;

    enum CameraPositions {Center, TopLeft, TopRight, BottomLeft, BottomRight};
    Transform cameraToShake;
    bool coroutineEnabled = false;


    // Start is called before the first frame update
    void Start()
    {
        cameraToShake = Camera.main.transform;
    }

    IEnumerator CameraShake()
    {
        coroutineEnabled = true;
        while (cameraShakeAmount > 0)
        {
            SnapCameraToPosition(CameraPositions.TopRight);
            yield return new WaitForEndOfFrame();
            SnapCameraToPosition(CameraPositions.TopLeft);
            yield return new WaitForEndOfFrame();
            SnapCameraToPosition(CameraPositions.Center);
            yield return new WaitForEndOfFrame();

            cameraShakeAmount *= 0.8f;
            if (cameraShakeAmount <= 0.05f)
                cameraShakeAmount = 0;


            SnapCameraToPosition(CameraPositions.BottomRight);
            yield return new WaitForEndOfFrame();
            SnapCameraToPosition(CameraPositions.BottomLeft);
            yield return new WaitForEndOfFrame();
            SnapCameraToPosition(CameraPositions.Center);
            yield return new WaitForEndOfFrame();

            cameraShakeAmount *= 0.8f;
            if (cameraShakeAmount <= 0.05f)
                cameraShakeAmount = 0;
        }
        cameraToShake.localPosition = Vector3.zero;

        coroutineEnabled = false;
    }

    public void AddCameraShake(float value)
    {
        cameraShakeAmount += value;
        if (!coroutineEnabled)
            StartCoroutine(CameraShake());
    }

    private void SnapCameraToPosition(CameraPositions camPos)
    {
        switch (camPos)
        {
            case CameraPositions.Center:
                cameraToShake.localPosition = Vector3.zero;
                break;
            case CameraPositions.TopLeft:
                cameraToShake.localPosition = new Vector3(cameraShakeAmount * -1, cameraShakeAmount);
                break;
            case CameraPositions.TopRight:
                cameraToShake.localPosition = new Vector3(cameraShakeAmount, cameraShakeAmount);
                break;
            case CameraPositions.BottomLeft:
                cameraToShake.localPosition = new Vector3(cameraShakeAmount * -1, cameraShakeAmount * -1);
                break;
            case CameraPositions.BottomRight:
                cameraToShake.localPosition = new Vector3(cameraShakeAmount, cameraShakeAmount * -1);
                break;
            default:
                break;
        }
    }
}
