using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollManager : MonoBehaviour
{
    public FollowPlayer cameraFollow;

    public bool ragdollEnabled = false;

    private Collider[] colliders;
    private Animator animator;
    private CharacterController controller;
    private GameObject playerModel;
    private Transform playerHips;

    private const float KNOCKBACK_MULTIPLIER = 50f;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        playerModel = transform.Find("PlayerModel").gameObject;
        playerHips = playerModel.transform.Find("Root").Find("Hips").transform;
        colliders = playerModel.transform.Find("Root").GetComponentsInChildren<Collider>();

        DisableRagDollState();
    }

    private void Update()
    {
        if (ragdollEnabled)
            transform.position = playerHips.position;
    }

    // USed to enable or disable the ragdoll behaviour from the knockback effect.
    public void EnableRagDollState(Vector3 knockbackDirection)
    {
        ragdollEnabled = true;
        animator.enabled = false;
        playerModel.transform.parent = null;
        cameraFollow.playerTarget = playerHips;
        Debug.Log("the force we are adding to everything is: " + knockbackDirection * KNOCKBACK_MULTIPLIER);

        foreach (Collider col in colliders)
        {
            col.enabled = true;
            col.GetComponent<Rigidbody>().velocity = Vector3.zero;
            col.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            col.GetComponent<Rigidbody>().AddForce(knockbackDirection * KNOCKBACK_MULTIPLIER, ForceMode.Impulse);
        }
    }

    public void DisableRagDollState()
    {
        ragdollEnabled = false;
        animator.enabled = true;
        playerModel.transform.parent = transform;
        playerModel.transform.localPosition = Vector3.zero;
        cameraFollow.playerTarget = transform;

        foreach (Collider col in colliders)
        {
            col.enabled = false;
            col.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }
}
