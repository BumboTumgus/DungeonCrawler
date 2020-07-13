using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    public float cameraCurrentZoom = -3;
    public float cameraTargetZoom = 0;
    public float cameraScrollTargetZoom = 0;
    public bool menuOpen = false;
    public LayerMask rayMask;

    private Transform cameraRotationParent;
    private float localX;
    private float currentY;

    private const float CAMERA_LOWER_Y = 1.5f;
    private const float CAMERA_UPPER_Y = 3.5f;

    private const float CAMERA_ZOOM_MIN = -0.5f;
    private const float CAMERA_ZOOM_MAX = -5;
    private const float CAMERA_ZOOM_SENSITIVTY = 6;

    private const float CAMERA_PITCH_MIN = 40;
    private const float CAMERA_PITCH_MAX = 40;
    private const float CAMERA_PITCH_SENSITIVITY = 2;
    private const float CAMERA_YAW_SENSITIVITY = 2;
    
    // Start is called before the first frame update
    void Start()
    {
        cameraRotationParent = transform.parent;
        localX = transform.localPosition.x;
        currentY = transform.localPosition.y;
        cameraCurrentZoom = transform.localPosition.z;
        cameraTargetZoom = transform.localPosition.z;
        cameraScrollTargetZoom = transform.localPosition.z;
    }

    // Update is called once per frame
    void Update()
    {
        if(!menuOpen)
            CameraMovement();
    }

    // Used to rotate the camera based on mouse position.
    private void CameraMovement()
    {
        // Grab the movement of the mouse and multiply it by the change in time and our camera sensitivity.
        float horizontalMovement = Input.GetAxis("Mouse X") * CAMERA_YAW_SENSITIVITY;
        float verticalMovement = Input.GetAxis("Mouse Y")* -CAMERA_PITCH_SENSITIVITY;
        
        // Grab our rotation and add the new values to it.
        Vector3 cameraRotation = cameraRotationParent.rotation.eulerAngles + new Vector3(verticalMovement, horizontalMovement, 0);
        
        if (cameraRotation.x > CAMERA_PITCH_MAX && cameraRotation.x < 180)
            cameraRotation.x = CAMERA_PITCH_MAX;

        if(cameraRotation.x < 360 - CAMERA_PITCH_MIN && cameraRotation.x > 180)
            cameraRotation.x = 360 - CAMERA_PITCH_MIN;

        if (cameraRotation.y > 360)
            cameraRotation.y -= 360;
        if (cameraRotation.y < 0)
            cameraRotation.y += 360;

        // Set the rotation to the camera's parent.
        cameraRotationParent.rotation = Quaternion.Euler(cameraRotation);
        
        // If we have an input from the scrollwheel, zoom in or out based on the amount scrolled.
        if(Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            cameraScrollTargetZoom += (Input.GetAxis("Mouse ScrollWheel") * CAMERA_ZOOM_SENSITIVTY);

            if (cameraScrollTargetZoom < CAMERA_ZOOM_MAX)
                cameraScrollTargetZoom = CAMERA_ZOOM_MAX;
            if (cameraScrollTargetZoom > CAMERA_ZOOM_MIN)
                cameraScrollTargetZoom = CAMERA_ZOOM_MIN;
        }

        // Shoot a ray from the characetr to the camera, if it hits somwthing in the mask that is going to be set as the new target position, otherwise the target is set to the scroll target
        Ray raytoShoot = new Ray(cameraRotationParent.transform.position + Vector3.up, transform.forward * -1);
        RaycastHit rayhit = new RaycastHit();
        // Debug.DrawRay(cameraRotationParent.transform.position + Vector3.up, transform.forward * -5, Color.red);

        if(Physics.Raycast(raytoShoot,out rayhit, CAMERA_ZOOM_MAX * -1, rayMask))
        {
            //Debug.Log(rayhit.point);
            //Debug.Log(transform.InverseTransformPoint(rayhit.point));
            cameraTargetZoom = transform.InverseTransformPoint(rayhit.point).z;
            if (cameraTargetZoom < CAMERA_ZOOM_MAX)
                cameraTargetZoom = CAMERA_ZOOM_MAX;
            if (cameraTargetZoom > CAMERA_ZOOM_MIN)
                cameraTargetZoom = CAMERA_ZOOM_MIN;
        }
        else
        {
            cameraTargetZoom = cameraScrollTargetZoom;
        }
        cameraCurrentZoom = Mathf.Lerp(cameraCurrentZoom, cameraTargetZoom, 0.1f);

        // using the current zoom, get a value for the y between the lower and upper bound based on how far we are zoomed in.
        currentY = Mathf.Lerp(CAMERA_LOWER_Y, CAMERA_UPPER_Y, (cameraCurrentZoom - CAMERA_ZOOM_MIN) / (CAMERA_ZOOM_MAX - CAMERA_ZOOM_MIN));

        //Set the camera's local z position based on the current zoom parameter.
        transform.localPosition = new Vector3(localX, currentY, cameraCurrentZoom);
    }
}
