using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    public float cameraCurrentZoom = -3;
    public bool menuOpen = false;

    private Transform cameraRotationParent;
    private float localX;
    private float localY;

    private const float CAMERA_ZOOM_MIN = -1;
    private const float CAMERA_ZOOM_MAX = -6;
    private const float CAMERA_ZOOM_SENSITIVTY = 3;

    private const float CAMERA_PITCH_MIN = 17;
    private const float CAMERA_PITCH_MAX = 40;
    private const float CAMERA_PITCH_SENSITIVITY = 2;
    private const float CAMERA_YAW_SENSITIVITY = 2;
    
    // Start is called before the first frame update
    void Start()
    {
        cameraRotationParent = transform.parent;
        localX = transform.localPosition.x;
        localY = transform.localPosition.y;
        cameraCurrentZoom = transform.localPosition.z;
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
            cameraCurrentZoom += (Input.GetAxis("Mouse ScrollWheel") * CAMERA_ZOOM_SENSITIVTY);

            if (cameraCurrentZoom < CAMERA_ZOOM_MAX)
                cameraCurrentZoom = CAMERA_ZOOM_MAX;
            if (cameraCurrentZoom > CAMERA_ZOOM_MIN)
                cameraCurrentZoom = CAMERA_ZOOM_MIN;

            //Set the camera's local z position based on the current zoom parameter.
            transform.localPosition = new Vector3(localX, localY, cameraCurrentZoom);
        }
    }
}
