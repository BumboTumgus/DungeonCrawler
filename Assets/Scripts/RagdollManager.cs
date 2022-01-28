using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollManager : MonoBehaviour
{
    public FollowPlayer cameraFollow;

    public enum BoneProfile {Humanoid, Bee, Snek, Wolf, Golem, Dragon};
    public BoneProfile myBoneProfile = BoneProfile.Humanoid;

    public bool ragdollEnabled = false;
    public bool getUpFaceUp = false;
    public bool canGetUpFromBothSides = true;

    [SerializeField] private Collider[] colliders;
    private Animator animator;
    [SerializeField] public GameObject entityModel;
    private Transform rootTransform;
    private Transform playerHips;
    private Vector3 startingEntityModelRotation = Vector3.zero;

    private bool playerEntity = true;

    private const float KNOCKBACK_MULTIPLIER = 45f;
    private const float BREAKOUT_MIN_VELOCITY = 0.025f;

    // Start is called before the first frame update
    void Start()
    {
        if (CompareTag("Enemy"))
            playerEntity = false;
        else
            playerEntity = true;

        animator = GetComponent<Animator>();
        entityModel = transform.Find("EntityModel").gameObject;
        startingEntityModelRotation = entityModel.transform.localRotation.eulerAngles;

        switch (myBoneProfile)
        {
            case BoneProfile.Humanoid:
                rootTransform = entityModel.transform.Find("Root");
                playerHips = entityModel.transform.Find("Root").Find("Hips").transform;
                colliders = entityModel.transform.Find("Root").GetComponentsInChildren<Collider>();
                break;
            case BoneProfile.Bee:
                rootTransform = entityModel.transform.Find("Root");
                playerHips = entityModel.transform.Find("Root").Find("RigBody").transform;
                colliders = entityModel.transform.Find("Root").GetComponentsInChildren<Collider>();
                break;
            case BoneProfile.Snek:
                rootTransform = entityModel.transform.Find("Root");
                playerHips = entityModel.transform.Find("Root").Find("RigPelvis").transform;
                colliders = entityModel.transform.Find("Root").GetComponentsInChildren<Collider>();
                break;
            case BoneProfile.Wolf:
                rootTransform = entityModel.transform.Find("Root");
                playerHips = entityModel.transform.Find("Root").Find("RigPelvis").transform;
                colliders = entityModel.transform.Find("Root").GetComponentsInChildren<Collider>();
                break;
            case BoneProfile.Golem:
                rootTransform = entityModel.transform.Find("Root");
                playerHips = entityModel.transform.Find("Root").Find("Hips").transform;
                colliders = entityModel.transform.Find("Root").GetComponentsInChildren<Collider>();
                break;
            case BoneProfile.Dragon:
                rootTransform = entityModel.transform.Find("Root");
                playerHips = entityModel.transform.Find("Root").Find("RigPelvis").transform;
                colliders = entityModel.transform.Find("Root").GetComponentsInChildren<Collider>();
                break;
            default:
                break;
        }

        IntializeRagdoll();
    }

    private void Update()
    {
        Debug.DrawRay(transform.position, colliders[0].transform.right * -4, Color.blue);
        Debug.DrawRay(transform.position, colliders[0].transform.forward * -4, Color.red);
        Debug.DrawRay(transform.position, colliders[0].transform.up * -4, Color.green);
        if (ragdollEnabled)
            transform.position = playerHips.position;
    }

    // USed to enable or disable the ragdoll behaviour from the knockback effect.
    public void EnableRagDollState(Vector3 knockbackDirection)
    {
        StopAllCoroutines();
        ragdollEnabled = true;
        animator.enabled = false;
        entityModel.transform.parent = null;

        if (playerEntity)
        {
            cameraFollow.playerTarget = playerHips;
            //GetComponent<CharacterController>().enabled = false;
        }

        //Debug.Log("the force we are adding to everything is: " + knockbackDirection * KNOCKBACK_MULTIPLIER);

        foreach (Collider col in colliders)
        {
            col.enabled = true;
            col.GetComponent<Rigidbody>().velocity = Vector3.zero;
            col.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            col.GetComponent<Rigidbody>().isKinematic = false;
            //col.GetComponent<Rigidbody>().AddForce(knockbackDirection * KNOCKBACK_MULTIPLIER, ForceMode.Impulse);
        }

        colliders[0].GetComponent<Rigidbody>().AddForce(knockbackDirection * KNOCKBACK_MULTIPLIER, ForceMode.Impulse);

        if(!playerEntity)
        {
            GetComponent<Rigidbody>().isKinematic = true;
            //GetComponent<CapsuleCollider>().enabled = false;
        }
    }

    private void IntializeRagdoll()
    {
        //Debug.Log("initializing ragdoll");
        ragdollEnabled = false;

        if (playerEntity)
        {
            cameraFollow.playerTarget = transform;
            //GetComponent<CharacterController>().enabled = true;
        }

        foreach (Collider col in colliders)
        {
            col.enabled = false;
            col.GetComponent<Rigidbody>().velocity = Vector3.zero;
            col.GetComponent<Rigidbody>().isKinematic = true;
        }

        if(!playerEntity)
        {
            GetComponent<Rigidbody>().isKinematic = false;
            //GetComponent<CapsuleCollider>().enabled = true;
        }

    }

    public void DisableRagDollState()
    {
        ragdollEnabled = false;

        if (playerEntity)
        {
            cameraFollow.playerTarget = transform;
            //GetComponent<CharacterController>().enabled = true;
            // Set the playercharacter to face the direction of the hips, and we set our height to the grounds height.
            GetComponent<PlayerMovementController>().SnapToFloor();
        }


        if (myBoneProfile == BoneProfile.Golem)
        {
            Debug.Log("The angular difference is " + Vector3.Angle(Vector3.up, colliders[0].transform.forward));
            // Rotates the priamry transform to face the direction of the hips.
            if (Vector3.Angle(Vector3.up, colliders[0].transform.forward) > 90)
            {
                Debug.DrawRay(transform.position, colliders[0].transform.right * -4, Color.blue, 2f);

                Quaternion rotationChange = Quaternion.FromToRotation(transform.forward, colliders[0].transform.up);
                transform.rotation = rotationChange * transform.rotation;

                // Zero out the x and z on the quaternion
                Vector3 newRotation = transform.rotation.eulerAngles;
                newRotation.x = 0;
                newRotation.z = 0;

                transform.rotation = Quaternion.Euler(newRotation);

                if (canGetUpFromBothSides)
                    getUpFaceUp = true;

                Debug.DrawRay(transform.position, transform.forward * 4, Color.green, 2f);
            }
            else
            {
                Debug.DrawRay(transform.position, colliders[0].transform.right * 4, Color.blue, 2f);

                Quaternion rotationChange = Quaternion.FromToRotation(transform.forward, colliders[0].transform.up* -1);
                transform.rotation = rotationChange * transform.rotation;

                // Zero out the x and z on the quaternion
                Vector3 newRotation = transform.rotation.eulerAngles;
                newRotation.x = 0;
                newRotation.z = 0;

                transform.rotation = Quaternion.Euler(newRotation);

                if (canGetUpFromBothSides)
                    getUpFaceUp = false;

                Debug.DrawRay(transform.position, transform.forward * 4, Color.green, 2f);
            }
        }
        else
        {
            // Rotates the priamry transform to face the direction of the hips.
            if (Vector3.Angle(Vector3.up, colliders[0].transform.up) > 90)
            {
                Debug.DrawRay(transform.position, colliders[0].transform.right * -4, Color.blue, 2f);

                Quaternion rotationChange = Quaternion.FromToRotation(transform.forward, colliders[0].transform.right * -1);
                transform.rotation = rotationChange * transform.rotation;

                // Zero out the x and z on the quaternion
                Vector3 newRotation = transform.rotation.eulerAngles;
                newRotation.x = 0;
                newRotation.z = 0;

                transform.rotation = Quaternion.Euler(newRotation);

                if (canGetUpFromBothSides)
                    getUpFaceUp = true;

                Debug.DrawRay(transform.position, transform.forward * 4, Color.green, 2f);
            }
            else
            {
                Debug.DrawRay(transform.position, colliders[0].transform.right * 4, Color.blue, 2f);

                Quaternion rotationChange = Quaternion.FromToRotation(transform.forward, colliders[0].transform.right);
                transform.rotation = rotationChange * transform.rotation;

                // Zero out the x and z on the quaternion
                Vector3 newRotation = transform.rotation.eulerAngles;
                newRotation.x = 0;
                newRotation.z = 0;

                transform.rotation = Quaternion.Euler(newRotation);

                if (canGetUpFromBothSides)
                    getUpFaceUp = false;

                Debug.DrawRay(transform.position, transform.forward * 4, Color.green, 2f);
            }
        }

        // Move the root outside the player model so they dont rotate with it.
        playerHips.parent = null;

        // Sets the player model to rotate and follow the primary transform.
        entityModel.transform.parent = transform;
        entityModel.transform.localPosition = Vector3.zero;
        entityModel.transform.localRotation = Quaternion.Euler(startingEntityModelRotation);

        // Move the root back in now that the rotation is done.
        playerHips.parent = rootTransform;

        // Moves the hips to be in the same place in the player model.
        Vector3 positionalDifference = Vector3.zero;
        positionalDifference = entityModel.transform.position - colliders[0].transform.position;
        colliders[0].transform.position += positionalDifference + Vector3.up * 0.16f;

        // Zero out all the velocities of the rigidbodies
        foreach (Collider col in colliders)
        {
            col.enabled = false;
            col.GetComponent<Rigidbody>().velocity = Vector3.zero;
            col.GetComponent<Rigidbody>().isKinematic = true;
        }

        if (!playerEntity)
        {
            GetComponent<Rigidbody>().isKinematic = false;
            //GetComponent<CapsuleCollider>().enabled = true;
        }
    }

    // USed to poll to see our current velocity and if it's magnitude is low wnough that our character could get up.
    public bool CanWeGetUpVelocityPoll()
    {
        bool canWeGetUp = false;

        //Debug.Log("our current speed is: " + colliders[0].GetComponent<Rigidbody>().velocity.sqrMagnitude);
        if (colliders[0].GetComponent<Rigidbody>().velocity.sqrMagnitude < BREAKOUT_MIN_VELOCITY * BREAKOUT_MIN_VELOCITY)
        {
            //Debug.Log("we got out.");
            canWeGetUp = true;
        }

        return canWeGetUp;
    }

    private void OnDestroy()
    {
        entityModel.transform.parent = transform;
    }
}
