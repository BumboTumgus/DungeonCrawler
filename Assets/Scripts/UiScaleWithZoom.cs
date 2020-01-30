using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiScaleWithZoom : MonoBehaviour
{
    public float maxScalePercent = 1.5f;
    public float minScalePercent = 0.6f;

    private CameraControls cameraControls;
    private float originalScale;
    private float minScale;
    private float maxScale;
    private float targetScale;
    private float currentScale;

    private const float UI_SCALE_LERP_SPEED = 6;

    // Start is called before the first frame update
    void Start()
    {
        // Grabs reference to the camera controls so we know how zoomed in the camera is.
        cameraControls = Camera.main.gameObject.GetComponentInParent<CameraControls>();
        originalScale = transform.localScale.x;

        // Sets up the maximum and minimum scale for each axis.
        minScale = originalScale * minScalePercent;
        maxScale = originalScale * maxScalePercent;
        targetScale = originalScale;
        currentScale = originalScale;
    }

    // Update is called once per frame
    void Update()
    {
        // This gives us a value between the min and max zoom, currently 4 and -16;
        float currentZoom = cameraControls.cameraCurrentZoom;

        // If it is less than zero, interpolate between our max size and our original size.
        if(currentZoom >= 0)
            targetScale = Mathf.Lerp(originalScale, maxScale, currentZoom / 4);
        // If it is greater than zero, interpolate between our original and min size.
        else
            targetScale = Mathf.Lerp(minScale, originalScale, (currentZoom + 16) / 16);
        
        // Lerp towards the target
        if(currentScale != targetScale)
            currentScale = Mathf.Lerp(currentScale, targetScale, UI_SCALE_LERP_SPEED / 100);

        // Set the scale.
        transform.localScale = new Vector3(currentScale, currentScale, 1);

    }
}
