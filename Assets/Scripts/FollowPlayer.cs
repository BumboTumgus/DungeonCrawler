using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform playerTarget;
    
    // Update is called once per frame
    void Update()
    {
        // Quickly lerp towards the player.
        transform.position = playerTarget.position;
    }
}
