using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollManager : MonoBehaviour
{
    public FollowPlayer cameraFollow;

    public bool ragdollEnabled = false;
    public bool getUpFaceUp = false;

    public float lerpTime = 0.33f;

    [SerializeField] private Collider[] colliders;
    private Animator animator;
    private CharacterController controller;
    private EnemyMovementManager enemyController;
    private GameObject entityModel;
    private Transform rootTransform;
    private Transform playerHips;

    private bool playerEntity = true;

    [SerializeField] private Transform[] bonesToLerp = new Transform[20];

    private Vector3 facedownGetUpHipsPosition = new Vector3(-4.09f, 41.17f, 0.1f);
    private Vector3 facedownGetUpHipsRotation = new Vector3(9.9f, -116.3f, -137.2f);
    private Vector3 facedownGetUpUpperLegLPosition = new Vector3(4.1f, -2.7f, 9.9f);
    private Vector3 facedownGetUpUpperLegLRotation = new Vector3(-35.1f, -118.1f, 48.7f);
    private Vector3 facedownGetUpUpperLegRPosition = new Vector3(4.1f, -2.7f, -9.9f);
    private Vector3 facedownGetUpUpperLegRRotation = new Vector3(56.1f, 52.3f, -136.3f);
    private Vector3 facedownGetUpLowerLegLPosition = new Vector3(39.9f, -3.6f, 3.3f);
    private Vector3 facedownGetUpLowerLegLRotation = new Vector3(74.9f, 90.9f, -22.9f);
    private Vector3 facedownGetUpLowerLegRPosition = new Vector3(-39.9f, 1.2f, 5.3f);
    private Vector3 facedownGetUpLowerLegRRotation = new Vector3(112.1f, -28.2f, -113.2f);
    private Vector3 facedownGetUpAnkleLPosition = new Vector3(37.7f, 2.1f, -7.4f);
    private Vector3 facedownGetUpAnkleLRotation = new Vector3(-177.3f, 10.4f, -79.8f);
    private Vector3 facedownGetUpAnkleRPosition = new Vector3(-37.7f, 6f, -5.4f);
    private Vector3 facedownGetUpAnkleRRotation = new Vector3(-169.1f, 0.45f, 5.3f);
    private Vector3 facedownGetUpSpine1Position = new Vector3(-10.4f, 0f, -6.5f);
    private Vector3 facedownGetUpSpine1Rotation = new Vector3(-169.2f, 10.7f, 53.9f);
    private Vector3 facedownGetUpSpine2Position = new Vector3(-18.2f, -1.5f, 3.4f);
    private Vector3 facedownGetUpSpine2Rotation = new Vector3(-172.9f, 25.3f, -11.1f);
    private Vector3 facedownGetUpSpine3Position = new Vector3(-17.9f, -1.5f, -1.9f);
    private Vector3 facedownGetUpSpine3Rotation = new Vector3(-180f, -1.5f, 12.8f);
    private Vector3 facedownGetUpClavicleLPosition = new Vector3(-5.8f, -4.2f, -7.5f);
    private Vector3 facedownGetUpClavicleLRotation = new Vector3(115f, 11.2f, 89.2f);
    private Vector3 facedownGetUpClavicleRPosition = new Vector3(-5.8f, -4.2f, 7.5f);
    private Vector3 facedownGetUpClavicleRRotation = new Vector3(-97.1f, -85f, 10.6f);
    private Vector3 facedownGetUpNeckPosition = new Vector3(-11.2f, -1.5f, 4.6f);
    private Vector3 facedownGetUpNeckRotation = new Vector3(18.2f, 11.6f, -38f);
    private Vector3 facedownGetUpHeadPosition = new Vector3(-12.2f, -3f, 1.7f);
    private Vector3 facedownGetUpHeadRotation = new Vector3(50f, -56.6f, 64.9f);
    private Vector3 facedownGetUpShoulderLPosition = new Vector3(-13.2f, -1.1f, -5.8f);
    private Vector3 facedownGetUpShoulderLRotation = new Vector3(-42.2f, 4.3f, 54.4f);
    private Vector3 facedownGetUpShoulderRPosition = new Vector3(13.2f, 0f, 0f);
    private Vector3 facedownGetUpShoulderRRotation = new Vector3(-31.1f, 25.1f, 34.9f);
    private Vector3 facedownGetUpElbowLPosition = new Vector3(-33.9f, 0.1f, 0.1f);
    private Vector3 facedownGetUpElbowLRotation = new Vector3(-10.3f, 84f, -51.3f);
    private Vector3 facedownGetUpElbowRPosition = new Vector3(33.9f, 0.1f, 0.1f);
    private Vector3 facedownGetUpElbowRRotation = new Vector3(15.7f, 106.7f, -28.9f);
    private Vector3 facedownGetUpHandLPosition = new Vector3(-27.1f, -3.95f, 0f);
    private Vector3 facedownGetUpHandLRotation = new Vector3(-0.6f, -29.5f, 13.2f);
    private Vector3 facedownGetUpHandRPosition = new Vector3(27.1f, 3.8f, 0f);
    private Vector3 facedownGetUpHandRRotation = new Vector3(9.2f, -32.2f, 20f);

    private Vector3 faceupGetUpHipsPosition = new Vector3(0.1f, 14.1f, 1.3f);
    private Vector3 faceupGetUpHipsRotation = new Vector3(14.7f, -54.3f, -21.3f);
    private Vector3 faceupGetUpUpperLegLPosition = new Vector3(4.1f, -2.7f, 9.9f);
    private Vector3 faceupGetUpUpperLegLRotation = new Vector3(-37.7f, -67f, 45f);
    private Vector3 faceupGetUpUpperLegRPosition = new Vector3(4.1f, -2.7f, -9.9f);
    private Vector3 faceupGetUpUpperLegRRotation = new Vector3(35.4f, 92.7f, -101.1f);
    private Vector3 faceupGetUpLowerLegLPosition = new Vector3(39.9f, -3.6f, 3.3f);
    private Vector3 faceupGetUpLowerLegLRotation = new Vector3(112.4f, -21.4f, -76f);
    private Vector3 faceupGetUpLowerLegRPosition = new Vector3(-39.9f, 1.2f, 5.3f);
    private Vector3 faceupGetUpLowerLegRRotation = new Vector3(76.1f, 85.7f, 8.4f);
    private Vector3 faceupGetUpAnkleLPosition = new Vector3(37.7f, 2.1f, -7.4f);
    private Vector3 faceupGetUpAnkleLRotation = new Vector3(-168.6f, -8.7f, -49.5f);
    private Vector3 faceupGetUpAnkleRPosition = new Vector3(-37.7f, 6f, -5.4f);
    private Vector3 faceupGetUpAnkleRRotation = new Vector3(-172f, 11.6f, -45.5f);
    private Vector3 faceupGetUpSpine1Position = new Vector3(-10.4f, 0f, -6.5f);
    private Vector3 faceupGetUpSpine1Rotation = new Vector3(-178.6f, -2.2f, 17.6f);
    private Vector3 faceupGetUpSpine2Position = new Vector3(-18.2f, -1.5f, 3.4f);
    private Vector3 faceupGetUpSpine2Rotation = new Vector3(-177.4f, -11.4f, -8.3f);
    private Vector3 faceupGetUpSpine3Position = new Vector3(-17.9f, -0f, -1.9f);
    private Vector3 faceupGetUpSpine3Rotation = new Vector3(-180f, -1.5f, 12.8f);
    private Vector3 faceupGetUpClavicleLPosition = new Vector3(-5.8f, -4.2f, -7.5f);
    private Vector3 faceupGetUpClavicleLRotation = new Vector3(124.8f, 5.6f, 100.2f);
    private Vector3 faceupGetUpClavicleRPosition = new Vector3(-5.8f, -4.2f, 7.5f);
    private Vector3 faceupGetUpClavicleRRotation = new Vector3(-129.2f, -11.4f, -100f);
    private Vector3 faceupGetUpNeckPosition = new Vector3(-11.2f, -1.5f, 4.7f);
    private Vector3 faceupGetUpNeckRotation = new Vector3(-0.5f, 3f, -3.9f);
    private Vector3 faceupGetUpHeadPosition = new Vector3(-12.2f, -3f, 1.7f);
    private Vector3 faceupGetUpHeadRotation = new Vector3(119.5f, -30.2f, 66.2f);
    private Vector3 faceupGetUpShoulderLPosition = new Vector3(-13.2f, -1.1f, -5.8f);
    private Vector3 faceupGetUpShoulderLRotation = new Vector3(-37.5f, 36.7f, 34.1f);
    private Vector3 faceupGetUpShoulderRPosition = new Vector3(13.2f, 0f, 0f);
    private Vector3 faceupGetUpShoulderRRotation = new Vector3(11.4f, 9.1f, 39.7f);
    private Vector3 faceupGetUpElbowLPosition = new Vector3(-33.9f, -0.1f, -0.1f);
    private Vector3 faceupGetUpElbowLRotation = new Vector3(3.5f, 71.1f, -29.2f);
    private Vector3 faceupGetUpElbowRPosition = new Vector3(33.9f, 0.1f, 0.1f);
    private Vector3 faceupGetUpElbowRRotation = new Vector3(-0.3f, 97.5f, -10.6f);
    private Vector3 faceupGetUpHandLPosition = new Vector3(-27.1f, -3.95f, 0f);
    private Vector3 faceupGetUpHandLRotation = new Vector3(12.8f, -42.3f, 6.8f);
    private Vector3 faceupGetUpHandRPosition = new Vector3(27.1f, 3.8f, 0f);
    private Vector3 faceupGetUpHandRRotation = new Vector3(-3.4f, -18.3f, 4.5f);

    private Vector3 originalHipsPosition = Vector3.zero;
    private Vector3 originalHipsRotation = Vector3.zero;
    private Vector3 originalUpperLegLPosition = Vector3.zero;
    private Vector3 originalUpperLegLRotation = Vector3.zero;
    private Vector3 originalUpperLegRPosition = Vector3.zero;
    private Vector3 originalUpperLegRRotation = Vector3.zero;
    private Vector3 originalLowerLegLPosition = Vector3.zero;
    private Vector3 originalLowerLegLRotation = Vector3.zero;
    private Vector3 originalLowerLegRPosition = Vector3.zero;
    private Vector3 originalLowerLegRRotation = Vector3.zero;
    private Vector3 originalAnkleLPosition = Vector3.zero;
    private Vector3 originalAnkleLRotation = Vector3.zero;
    private Vector3 originalAnkleRPosition = Vector3.zero;
    private Vector3 originalAnkleRRotation = Vector3.zero;
    private Vector3 originalSpine1Position = Vector3.zero;
    private Vector3 originalSpine1Rotation = Vector3.zero;
    private Vector3 originalSpine2Position = Vector3.zero;
    private Vector3 originalSpine2Rotation = Vector3.zero;
    private Vector3 originalSpine3Position = Vector3.zero;
    private Vector3 originalSpine3Rotation = Vector3.zero;
    private Vector3 originalClavicleLPosition = Vector3.zero;
    private Vector3 originalClavicleLRotation = Vector3.zero;
    private Vector3 originalClavicleRPosition = Vector3.zero;
    private Vector3 originalClavicleRRotation = Vector3.zero;
    private Vector3 originalNeckPosition = Vector3.zero;
    private Vector3 originalNeckRotation = Vector3.zero;
    private Vector3 originalHeadPosition = Vector3.zero;
    private Vector3 originalHeadRotation = Vector3.zero;
    private Vector3 originalShoulderLPosition = Vector3.zero;
    private Vector3 originalShoulderLRotation = Vector3.zero;
    private Vector3 originalShoulderRPosition = Vector3.zero;
    private Vector3 originalShoulderRRotation = Vector3.zero;
    private Vector3 originalElbowLPosition = Vector3.zero;
    private Vector3 originalElbowLRotation = Vector3.zero;
    private Vector3 originalElbowRPosition = Vector3.zero;
    private Vector3 originalElbowRRotation = Vector3.zero;
    private Vector3 originalHandLPosition = Vector3.zero;
    private Vector3 originalHandLRotation = Vector3.zero;
    private Vector3 originalHandRPosition = Vector3.zero;
    private Vector3 originalHandRRotation = Vector3.zero;

    private const float KNOCKBACK_MULTIPLIER = 45f;
    private const float BREAKOUT_MIN_VELOCITY = 0.025f;
    private const float LERP_STRENGTH = 0.035f;
    private const string GETTING_UP_START_POSITION_ANIM_NAME = "PlayerGettingUpStartPosition";

    // Start is called before the first frame update
    void Start()
    {
        if (CompareTag("Enemy"))
            playerEntity = false;
        else
            playerEntity = true;

        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        enemyController = GetComponent<EnemyMovementManager>();
        entityModel = transform.Find("EntityModel").gameObject;
        rootTransform = entityModel.transform.Find("Root");
        playerHips = entityModel.transform.Find("Root").Find("Hips").transform;
        colliders = entityModel.transform.Find("Root").GetComponentsInChildren<Collider>();

        IntializeRagdoll();
    }

    private void Update()
    {
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


        // Rotates the priamry transform to face the direction of the hips.
        if (Vector3.Angle(Vector3.up, colliders[0].transform.up) > 90)
        {
            Debug.DrawRay(transform.position, colliders[0].transform.right * -4, Color.blue);

            Quaternion rotationChange = Quaternion.FromToRotation(transform.forward, colliders[0].transform.right * -1);
            transform.rotation = rotationChange * transform.rotation;

            // Zero out the x and z on the quaternion
            Vector3 newRotation = transform.rotation.eulerAngles;
            newRotation.x = 0;
            newRotation.z = 0;

            transform.rotation = Quaternion.Euler(newRotation);

            Debug.DrawRay(transform.position, transform.forward * 4, Color.green);
        }
        else
        {
            Debug.DrawRay(transform.position, colliders[0].transform.right * 4, Color.blue);

            Quaternion rotationChange = Quaternion.FromToRotation(transform.forward, colliders[0].transform.right);
            transform.rotation = rotationChange * transform.rotation;

            // Zero out the x and z on the quaternion
            Vector3 newRotation = transform.rotation.eulerAngles;
            newRotation.x = 0;
            newRotation.z = 0;

            transform.rotation = Quaternion.Euler(newRotation);

            Debug.DrawRay(transform.position, transform.forward * 4, Color.green);
        }

        // Move the root outside the player model so they dont rotate with it.
        playerHips.parent = null;

        // Sets the player model to rotate and follow the primary transform.
        entityModel.transform.parent = transform;
        entityModel.transform.localPosition = Vector3.zero;
        entityModel.transform.localRotation = Quaternion.identity;

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

    public void LerpBonesToGetUpAnim()
    {
        //Debug.DrawRay(colliders[0].transform.position, colliders[0].transform.right, Color.blue);
        float angleOfdifference = Vector3.Angle(Vector3.up, colliders[0].transform.up);
        //Debug.Log(angleOfdifference);

        SetInitialBonePositionRotation();

        if(angleOfdifference > 90)
            StartCoroutine(LerpBonesToFrontGetUp());
        else
            StartCoroutine(LerpBonesToBackGetUp());

    }

    IEnumerator LerpBonesToFrontGetUp()
    {
        getUpFaceUp = true;

        //Debug.Log("all bones are gonna be lerped. to FRONT getup position");
        float currentTimer = 0f;
        float targetTimer = lerpTime;

        while(currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;

            for(int index = 0; index < bonesToLerp.Length; index++)
            {
                Transform boneToLerp = bonesToLerp[index];
                Vector3 targetPosition = Vector3.zero;
                Vector3 initialPosition = Vector3.zero;
                Quaternion targetRotation = Quaternion.identity;
                Quaternion initialRotation = Quaternion.identity;

                // Set our target
                switch (index)
                {
                    case 0:
                        targetPosition = facedownGetUpHipsPosition;
                        targetRotation = Quaternion.Euler(facedownGetUpHipsRotation);
                        initialPosition = originalHipsPosition;
                        initialRotation = Quaternion.Euler(originalHipsRotation);
                        break;
                    case 1:
                        targetPosition = facedownGetUpUpperLegLPosition;
                        targetRotation = Quaternion.Euler(facedownGetUpUpperLegLRotation);
                        initialPosition = originalUpperLegLPosition;
                        initialRotation = Quaternion.Euler(originalUpperLegLRotation);
                        break;
                    case 2:
                        targetPosition = facedownGetUpUpperLegRPosition;
                        targetRotation = Quaternion.Euler(facedownGetUpUpperLegRRotation);
                        initialPosition = originalUpperLegRPosition;
                        initialRotation = Quaternion.Euler(originalUpperLegRRotation);
                        break;
                    case 3:
                        targetPosition = facedownGetUpLowerLegLPosition;
                        targetRotation = Quaternion.Euler(facedownGetUpLowerLegLRotation);
                        initialPosition = originalLowerLegLPosition;
                        initialRotation = Quaternion.Euler(originalLowerLegLRotation);
                        break;
                    case 4:
                        targetPosition = facedownGetUpLowerLegRPosition;
                        targetRotation = Quaternion.Euler(facedownGetUpLowerLegRRotation);
                        initialPosition = originalLowerLegRPosition;
                        initialRotation = Quaternion.Euler(originalLowerLegRRotation);
                        break;
                    case 5:
                        targetPosition = facedownGetUpAnkleLPosition;
                        targetRotation = Quaternion.Euler(facedownGetUpAnkleLRotation);
                        initialPosition = originalAnkleLPosition;
                        initialRotation = Quaternion.Euler(originalAnkleLRotation);
                        break;
                    case 6:
                        targetPosition = facedownGetUpAnkleRPosition;
                        targetRotation = Quaternion.Euler(facedownGetUpAnkleRRotation);
                        initialPosition = originalAnkleRPosition;
                        initialRotation = Quaternion.Euler(originalAnkleRRotation);
                        break;
                    case 7:
                        targetPosition = facedownGetUpSpine1Position;
                        targetRotation = Quaternion.Euler(facedownGetUpSpine1Rotation);
                        initialPosition = originalSpine1Position;
                        initialRotation = Quaternion.Euler(originalSpine1Rotation);
                        break;
                    case 8:
                        targetPosition = facedownGetUpSpine2Position;
                        targetRotation = Quaternion.Euler(facedownGetUpSpine2Rotation);
                        initialPosition = originalSpine2Position;
                        initialRotation = Quaternion.Euler(originalSpine2Rotation);
                        break;
                    case 9:
                        targetPosition = facedownGetUpSpine3Position;
                        targetRotation = Quaternion.Euler(facedownGetUpSpine3Rotation);
                        initialPosition = originalSpine3Position;
                        initialRotation = Quaternion.Euler(originalSpine3Rotation);
                        break;
                    case 10:
                        targetPosition = facedownGetUpClavicleLPosition;
                        targetRotation = Quaternion.Euler(facedownGetUpClavicleLRotation);
                        initialPosition = originalClavicleLPosition;
                        initialRotation = Quaternion.Euler(originalClavicleLRotation);
                        break;
                    case 11:
                        targetPosition = facedownGetUpClavicleRPosition;
                        targetRotation = Quaternion.Euler(facedownGetUpClavicleRRotation);
                        initialPosition = originalClavicleRPosition;
                        initialRotation = Quaternion.Euler(originalClavicleRRotation);
                        break;
                    case 12:
                        targetPosition = facedownGetUpNeckPosition;
                        targetRotation = Quaternion.Euler(facedownGetUpNeckRotation);
                        initialPosition = originalNeckPosition;
                        initialRotation = Quaternion.Euler(originalNeckRotation);
                        break;
                    case 13:
                        targetPosition = facedownGetUpHeadPosition;
                        targetRotation = Quaternion.Euler(facedownGetUpHeadRotation);
                        initialPosition = originalHeadPosition;
                        initialRotation = Quaternion.Euler(originalHeadRotation);
                        break;
                    case 14:
                        targetPosition = facedownGetUpShoulderLPosition;
                        targetRotation = Quaternion.Euler(facedownGetUpShoulderLRotation);
                        initialPosition = originalShoulderLPosition;
                        initialRotation = Quaternion.Euler(originalShoulderLRotation);
                        break;
                    case 15:
                        targetPosition = facedownGetUpShoulderRPosition;
                        targetRotation = Quaternion.Euler(facedownGetUpShoulderRRotation);
                        initialPosition = originalShoulderRPosition;
                        initialRotation = Quaternion.Euler(originalShoulderRRotation);
                        break;
                    case 16:
                        targetPosition = facedownGetUpElbowLPosition;
                        targetRotation = Quaternion.Euler(facedownGetUpElbowLRotation);
                        initialPosition = originalElbowLPosition;
                        initialRotation = Quaternion.Euler(originalElbowLRotation);
                        break;
                    case 17:
                        targetPosition = facedownGetUpElbowRPosition;
                        targetRotation = Quaternion.Euler(facedownGetUpElbowRRotation);
                        initialPosition = originalElbowRPosition;
                        initialRotation = Quaternion.Euler(originalElbowRRotation);
                        break;
                    case 18:
                        targetPosition = facedownGetUpHandLPosition;
                        targetRotation = Quaternion.Euler(facedownGetUpHandLRotation);
                        initialPosition = originalHandLPosition;
                        initialRotation = Quaternion.Euler(originalHandLRotation);
                        break;
                    case 19:
                        targetPosition = facedownGetUpHandRPosition;
                        targetRotation = Quaternion.Euler(facedownGetUpHandRRotation);
                        initialPosition = originalHandRPosition;
                        initialRotation = Quaternion.Euler(originalHandRRotation);
                        break;
                    default:
                        break;
                }

                // Begin the lerp 

                boneToLerp.localPosition = Vector3.Lerp(initialPosition, targetPosition, currentTimer / targetTimer);
                boneToLerp.localRotation = Quaternion.Slerp(initialRotation, targetRotation, currentTimer / targetTimer);
            }

            yield return null;
        }
    }

    IEnumerator LerpBonesToBackGetUp()
    {
        getUpFaceUp = false;

        //Debug.Log("all bones are gonna be lerped. to BACK getup position");
        float currentTimer = 0f;
        float targetTimer = lerpTime;

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;

            for (int index = 0; index < bonesToLerp.Length; index++)
            {
                Transform boneToLerp = bonesToLerp[index];
                Vector3 targetPosition = Vector3.zero;
                Vector3 initialPosition = Vector3.zero;
                Quaternion targetRotation = Quaternion.identity;
                Quaternion initialRotation = Quaternion.identity;

                // Set our target
                switch (index)
                {
                    case 0:
                        targetPosition = faceupGetUpHipsPosition;
                        targetRotation = Quaternion.Euler(faceupGetUpHipsRotation);
                        initialPosition = originalHipsPosition;
                        initialRotation = Quaternion.Euler(originalHipsRotation);
                        break;
                    case 1:
                        targetPosition = faceupGetUpUpperLegLPosition;
                        targetRotation = Quaternion.Euler(faceupGetUpUpperLegLRotation);
                        initialPosition = originalUpperLegLPosition;
                        initialRotation = Quaternion.Euler(originalUpperLegLRotation);
                        break;
                    case 2:
                        targetPosition = faceupGetUpUpperLegRPosition;
                        targetRotation = Quaternion.Euler(faceupGetUpUpperLegRRotation);
                        initialPosition = originalUpperLegRPosition;
                        initialRotation = Quaternion.Euler(originalUpperLegRRotation);
                        break;
                    case 3:
                        targetPosition = faceupGetUpLowerLegLPosition;
                        targetRotation = Quaternion.Euler(faceupGetUpLowerLegLRotation);
                        initialPosition = originalLowerLegLPosition;
                        initialRotation = Quaternion.Euler(originalLowerLegLRotation);
                        break;
                    case 4:
                        targetPosition = faceupGetUpLowerLegRPosition;
                        targetRotation = Quaternion.Euler(faceupGetUpLowerLegRRotation);
                        initialPosition = originalLowerLegRPosition;
                        initialRotation = Quaternion.Euler(originalLowerLegRRotation);
                        break;
                    case 5:
                        targetPosition = faceupGetUpAnkleLPosition;
                        targetRotation = Quaternion.Euler(faceupGetUpAnkleLRotation);
                        initialPosition = originalAnkleLPosition;
                        initialRotation = Quaternion.Euler(originalAnkleLRotation);
                        break;
                    case 6:
                        targetPosition = faceupGetUpAnkleRPosition;
                        targetRotation = Quaternion.Euler(faceupGetUpAnkleRRotation);
                        initialPosition = originalAnkleRPosition;
                        initialRotation = Quaternion.Euler(originalAnkleRRotation);
                        break;
                    case 7:
                        targetPosition = faceupGetUpSpine1Position;
                        targetRotation = Quaternion.Euler(faceupGetUpSpine1Rotation);
                        initialPosition = originalSpine1Position;
                        initialRotation = Quaternion.Euler(originalSpine1Rotation);
                        break;
                    case 8:
                        targetPosition = faceupGetUpSpine2Position;
                        targetRotation = Quaternion.Euler(faceupGetUpSpine2Rotation);
                        initialPosition = originalSpine2Position;
                        initialRotation = Quaternion.Euler(originalSpine2Rotation);
                        break;
                    case 9:
                        targetPosition = faceupGetUpSpine3Position;
                        targetRotation = Quaternion.Euler(faceupGetUpSpine3Rotation);
                        initialPosition = originalSpine3Position;
                        initialRotation = Quaternion.Euler(originalSpine3Rotation);
                        break;
                    case 10:
                        targetPosition = faceupGetUpClavicleLPosition;
                        targetRotation = Quaternion.Euler(faceupGetUpClavicleLRotation);
                        initialPosition = originalClavicleLPosition;
                        initialRotation = Quaternion.Euler(originalClavicleLRotation);
                        break;
                    case 11:
                        targetPosition = faceupGetUpClavicleRPosition;
                        targetRotation = Quaternion.Euler(faceupGetUpClavicleRRotation);
                        initialPosition = originalClavicleRPosition;
                        initialRotation = Quaternion.Euler(originalClavicleRRotation);
                        break;
                    case 12:
                        targetPosition = faceupGetUpNeckPosition;
                        targetRotation = Quaternion.Euler(faceupGetUpNeckRotation);
                        initialPosition = originalNeckPosition;
                        initialRotation = Quaternion.Euler(originalNeckRotation);
                        break;
                    case 13:
                        targetPosition = faceupGetUpHeadPosition;
                        targetRotation = Quaternion.Euler(faceupGetUpHeadRotation);
                        initialPosition = originalHeadPosition;
                        initialRotation = Quaternion.Euler(originalHeadRotation);
                        break;
                    case 14:
                        targetPosition = faceupGetUpShoulderLPosition;
                        targetRotation = Quaternion.Euler(faceupGetUpShoulderLRotation);
                        initialPosition = originalShoulderLPosition;
                        initialRotation = Quaternion.Euler(originalShoulderLRotation);
                        break;
                    case 15:
                        targetPosition = faceupGetUpShoulderRPosition;
                        targetRotation = Quaternion.Euler(faceupGetUpShoulderRRotation);
                        initialPosition = originalShoulderRPosition;
                        initialRotation = Quaternion.Euler(originalShoulderRRotation);
                        break;
                    case 16:
                        targetPosition = faceupGetUpElbowLPosition;
                        targetRotation = Quaternion.Euler(faceupGetUpElbowLRotation);
                        initialPosition = originalElbowLPosition;
                        initialRotation = Quaternion.Euler(originalElbowLRotation);
                        break;
                    case 17:
                        targetPosition = faceupGetUpElbowRPosition;
                        targetRotation = Quaternion.Euler(faceupGetUpElbowRRotation);
                        initialPosition = originalElbowRPosition;
                        initialRotation = Quaternion.Euler(originalElbowRRotation);
                        break;
                    case 18:
                        targetPosition = faceupGetUpHandLPosition;
                        targetRotation = Quaternion.Euler(faceupGetUpHandLRotation);
                        initialPosition = originalHandLPosition;
                        initialRotation = Quaternion.Euler(originalHandLRotation);
                        break;
                    case 19:
                        targetPosition = faceupGetUpHandRPosition;
                        targetRotation = Quaternion.Euler(faceupGetUpHandRRotation);
                        initialPosition = originalHandRPosition;
                        initialRotation = Quaternion.Euler(originalHandRRotation);
                        break;
                    default:
                        break;
                }

                // Begin the lerp 

                boneToLerp.localPosition = Vector3.Lerp(initialPosition, targetPosition, currentTimer / targetTimer);
                boneToLerp.localRotation = Quaternion.Slerp(initialRotation, targetRotation, currentTimer / targetTimer);
            }

            yield return null;
        }
    }

    // USed to set the initial bone location and rotations for the slerp.
    private void SetInitialBonePositionRotation()
    {
        for (int index = 0; index < bonesToLerp.Length; index++)
        {
            Transform boneToMarkDown = bonesToLerp[index];

            // Ste our target
            switch (index)
            {
                case 0:
                    originalHipsPosition = boneToMarkDown.localPosition;
                    originalHipsRotation = boneToMarkDown.localRotation.eulerAngles;
                    break;
                case 1:
                    originalUpperLegLPosition = boneToMarkDown.localPosition;
                    originalUpperLegLRotation = boneToMarkDown.localRotation.eulerAngles;
                    break;
                case 2:
                    originalUpperLegRPosition = boneToMarkDown.localPosition;
                    originalUpperLegRRotation = boneToMarkDown.localRotation.eulerAngles;
                    break;
                case 3:
                    originalLowerLegLPosition = boneToMarkDown.localPosition;
                    originalLowerLegLRotation = boneToMarkDown.localRotation.eulerAngles;
                    break;
                case 4:
                    originalLowerLegRPosition = boneToMarkDown.localPosition;
                    originalLowerLegRRotation = boneToMarkDown.localRotation.eulerAngles;
                    break;
                case 5:
                    originalAnkleLPosition = boneToMarkDown.localPosition;
                    originalAnkleLRotation = boneToMarkDown.localRotation.eulerAngles;
                    break;
                case 6:
                    originalAnkleRPosition = boneToMarkDown.localPosition;
                    originalAnkleRRotation = boneToMarkDown.localRotation.eulerAngles;
                    break;
                case 7:
                    originalSpine1Position = boneToMarkDown.localPosition;
                    originalSpine1Rotation = boneToMarkDown.localRotation.eulerAngles;
                    break;
                case 8:
                    originalSpine2Position = boneToMarkDown.localPosition;
                    originalSpine2Rotation = boneToMarkDown.localRotation.eulerAngles;
                    break;
                case 9:
                    originalSpine3Position = boneToMarkDown.localPosition;
                    originalSpine3Rotation = boneToMarkDown.localRotation.eulerAngles;
                    break;
                case 10:
                    originalClavicleLPosition = boneToMarkDown.localPosition;
                    originalClavicleLRotation = boneToMarkDown.localRotation.eulerAngles;
                    break;
                case 11:
                    originalClavicleRPosition = boneToMarkDown.localPosition;
                    originalClavicleRRotation = boneToMarkDown.localRotation.eulerAngles;
                    break;
                case 12:
                    originalNeckPosition = boneToMarkDown.localPosition;
                    originalNeckRotation = boneToMarkDown.localRotation.eulerAngles;
                    break;
                case 13:
                    originalHeadPosition = boneToMarkDown.localPosition;
                    originalHeadRotation = boneToMarkDown.localRotation.eulerAngles;
                    break;
                case 14:
                    originalShoulderLPosition = boneToMarkDown.localPosition;
                    originalShoulderLRotation = boneToMarkDown.localRotation.eulerAngles;
                    break;
                case 15:
                    originalShoulderRPosition = boneToMarkDown.localPosition;
                    originalShoulderRRotation = boneToMarkDown.localRotation.eulerAngles;
                    break;
                case 16:
                    originalElbowLPosition = boneToMarkDown.localPosition;
                    originalElbowLRotation = boneToMarkDown.localRotation.eulerAngles;
                    break;
                case 17:
                    originalElbowRPosition = boneToMarkDown.localPosition;
                    originalElbowRRotation = boneToMarkDown.localRotation.eulerAngles;
                    break;
                case 18:
                    originalHandLPosition = boneToMarkDown.localPosition;
                    originalHandLRotation = boneToMarkDown.localRotation.eulerAngles;
                    break;
                case 19:
                    originalHandRPosition = boneToMarkDown.localPosition;
                    originalHandRRotation = boneToMarkDown.localRotation.eulerAngles;
                    break;
                default:
                    break;
            }
        }
    }

    private void OnDestroy()
    {
        entityModel.transform.parent = transform;
    }
}
