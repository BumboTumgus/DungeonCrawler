using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollManager : MonoBehaviour
{
    public FollowPlayer cameraFollow;

    public enum BoneProfile {Humanoid, Bee};
    public BoneProfile myBoneProfile = BoneProfile.Humanoid;

    public bool ragdollEnabled = false;
    public bool getUpFaceUp = false;
    public bool canGetUpFromBothSides = true;

    public float lerpTime = 0.33f;

    [SerializeField] private Collider[] colliders;
    private Animator animator;
    private GameObject entityModel;
    private Transform rootTransform;
    private Transform playerHips;
    private Vector3 startingEntityModelRotation = Vector3.zero;

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

    private Vector3 faceupGetUpBeeBodyPosition = new Vector3(-0.5322447f, 0.1669079f, 0.1354949f);
    private Vector3 faceupGetUpBeeBodyRotation = new Vector3(73.384f, -150.317f, -224.68f);
    private Vector3 faceupGetUpBeeTail2Position = new Vector3(-0.2027658f, 0, 0);
    private Vector3 faceupGetUpBeeTail2Rotation = new Vector3(-7.18f, 3.003f, 43.37f);
    private Vector3 faceupGetUpBeeTail3Position = new Vector3(-0.2131896f, 0, 0);
    private Vector3 faceupGetUpBeeTail3Rotation = new Vector3(-8.432f, 1.964f, 54.261f);
    private Vector3 faceupGetUpBeeTail4Position = new Vector3(-0.2007263f, 0, 0);
    private Vector3 faceupGetUpBeeTail4Rotation = new Vector3(-7.164001f, 9.583f, 26.026f);
    private Vector3 faceupGetUpBeeHeadPosition = new Vector3(-0.195669f, 0, 0);
    private Vector3 faceupGetUpBeeHeadRotation = new Vector3(-7.174f, 8.717f, 2.136f);
    private Vector3 faceupGetUpBeeLBackArm1Position = new Vector3(0.06990565f, 0.07030716f, 0.08224874f);
    private Vector3 faceupGetUpBeeLBackArm1Rotation = new Vector3(-11.336f, 195.997f, -72.164f);
    private Vector3 faceupGetUpBeeLBackArm2Position = new Vector3(-0.03690737f, 0.08525192f, 0.08224876f);
    private Vector3 faceupGetUpBeeLBackArm2Rotation = new Vector3(10.424f, -172.648f, -98.93201f);
    private Vector3 faceupGetUpBeeLBackArm3Position = new Vector3(-0.08026523f, 0, 0);
    private Vector3 faceupGetUpBeeLBackArm3Rotation = new Vector3(0.148f, -6.668f, 68.493f);
    private Vector3 faceupGetUpBeeLFrontArm1Position = new Vector3(-0.03690737f, 0.08525192f, 0.08224876f);
    private Vector3 faceupGetUpBeeLFrontArm1Rotation = new Vector3(10.424f, -172.648f, -98.93201f);
    private Vector3 faceupGetUpBeeLFrontArm2Position = new Vector3(-0.09344251f, 0f, 0f);
    private Vector3 faceupGetUpBeeLFrontArm2Rotation = new Vector3(2.553f, 8.309f, -77.283f);
    private Vector3 faceupGetUpBeeLFrontArm3Position = new Vector3(-0.1135087f, 0f, 0f);
    private Vector3 faceupGetUpBeeLFrontArm3Rotation = new Vector3(14.642f, 8.544001f, 129.138f);
    private Vector3 faceupGetUpBeeLMidArm1Position = new Vector3(0.01609291f, 0.08335769f, 0.08224876f);
    private Vector3 faceupGetUpBeeLMidArm1Rotation = new Vector3(-8.272f, 172.707f, -89.691f);
    private Vector3 faceupGetUpBeeLMidArm2Position = new Vector3(-0.07806961f, 0, 0);
    private Vector3 faceupGetUpBeeLMidArm2Rotation = new Vector3(1.009162f, -0.9975435f, -49.07458f);
    private Vector3 faceupGetUpBeeLMidArm3Position = new Vector3(-0.07751523f, 0, 0);
    private Vector3 faceupGetUpBeeLMidArm3Rotation = new Vector3(-10.058f, -1.369f, 83.985f);
    private Vector3 faceupGetUpBeeLWing1Position = new Vector3(-0.04646673f, -0.09351684f, 0.09805851f);
    private Vector3 faceupGetUpBeeLWing1Rotation = new Vector3(-9.893001f, -15.206f, 88.30701f);
    private Vector3 faceupGetUpBeeLWing2Position = new Vector3(-0.1681254f, 0, 0);
    private Vector3 faceupGetUpBeeLWing2Rotation = new Vector3(5.719f, 9.744f, -2.173f);
    private Vector3 faceupGetUpBeeLWing3Position = new Vector3(-0.1472564f, 0, 0);
    private Vector3 faceupGetUpBeeLWing3Rotation = new Vector3(0.48f, -12.378f, -5.489f);
    private Vector3 faceupGetUpBeeLWing4Position = new Vector3(-0.1546206f, 0, 0);
    private Vector3 faceupGetUpBeeLWing4Rotation = new Vector3(-5.316f, -17.037f, 3.871f);
    private Vector3 faceupGetUpBeeLWing5Position = new Vector3(0.06990566f, 0.07030716f, -0.08224874f);
    private Vector3 faceupGetUpBeeLWing5Rotation = new Vector3(-14.42f, -170.445f, -85.633f);
    private Vector3 faceupGetUpBeeRBackArm1Position = new Vector3(0.06990566f, 0.07030716f, -0.08224874f);
    private Vector3 faceupGetUpBeeRBackArm1Rotation = new Vector3(-14.42f, -170.445f, -85.633f);
    private Vector3 faceupGetUpBeeRBackArm2Position = new Vector3(-0.08546752f, 0, 0);
    private Vector3 faceupGetUpBeeRBackArm2Rotation = new Vector3(-5.578f, 24.207f, -105.818f);
    private Vector3 faceupGetUpBeeRBackArm3Position = new Vector3(-0.08026527f, 0, 0);
    private Vector3 faceupGetUpBeeRBackArm3Rotation = new Vector3(21.323f, 23.452f, 135.697f);
    private Vector3 faceupGetUpBeeRFrontArm1Position = new Vector3(-0.03690735f, 0.08525199f, -0.08224878f);
    private Vector3 faceupGetUpBeeRFrontArm1Rotation = new Vector3(-19.239f, 163.492f, -106.872f);
    private Vector3 faceupGetUpBeeRFrontArm2Position = new Vector3(-0.09344249f, 0, 0);
    private Vector3 faceupGetUpBeeRFrontArm2Rotation = new Vector3(0.1072707f, -3.92999f, -24.1135f);
    private Vector3 faceupGetUpBeeRFrontArm3Position = new Vector3(-0.1135087f, 0, 0);
    private Vector3 faceupGetUpBeeRFrontArm3Rotation = new Vector3(12.608f, 1.139f, 101.815f);
    private Vector3 faceupGetUpBeeRMidArm1Position = new Vector3(0.01609293f, 0.08335777f, -0.08224876f);
    private Vector3 faceupGetUpBeeRMidArm1Rotation = new Vector3(-36.857f, 147.884f, -82.93301f);
    private Vector3 faceupGetUpBeeRMidArm2Position = new Vector3(-0.07806961f, 0, 0);
    private Vector3 faceupGetUpBeeRMidArm2Rotation = new Vector3(0, 0, -29.53526f);
    private Vector3 faceupGetUpBeeRMidArm3Position = new Vector3(-0.07751521f, 0, 0);
    private Vector3 faceupGetUpBeeRMidArm3Rotation = new Vector3(11.414f, 26.493f, 106.201f);
    private Vector3 faceupGetUpBeeRWing1Position = new Vector3(-0.04646671f, -0.09351684f, -0.09805856f);
    private Vector3 faceupGetUpBeeRWing1Rotation = new Vector3(-14.433f, 3.241f, 113.642f);
    private Vector3 faceupGetUpBeeRWing2Position = new Vector3(-0.1681253f, 0, 0);
    private Vector3 faceupGetUpBeeRWing2Rotation = new Vector3(-4.481f, -6.82f, -2.596f);
    private Vector3 faceupGetUpBeeRWing3Position = new Vector3(-0.1472564f, 0, 0);
    private Vector3 faceupGetUpBeeRWing3Rotation = new Vector3(-4.892f, 5.982f, -7.892f);
    private Vector3 faceupGetUpBeeRWing4Position = new Vector3(-0.1546207f, 0, 0);
    private Vector3 faceupGetUpBeeRWing4Rotation = new Vector3(-17.741f, -29.133f, 2.538f);
    private Vector3 faceupGetUpBeeRWing5Position = new Vector3(-0.2222735f, 0, 0);
    private Vector3 faceupGetUpBeeRWing5Rotation = new Vector3(7.485f, 8.444f, 5.83f);

    private Vector3 originalBeeBodyPosition = Vector3.zero;
    private Vector3 originalBeeBodyRotation = Vector3.zero;
    private Vector3 originalBeeTail2Position = Vector3.zero;
    private Vector3 originalBeeTail2Rotation = Vector3.zero;
    private Vector3 originalBeeTail3Position = Vector3.zero;
    private Vector3 originalBeeTail3Rotation = Vector3.zero;
    private Vector3 originalBeeTail4Position = Vector3.zero;
    private Vector3 originalBeeTail4Rotation = Vector3.zero;
    private Vector3 originalBeeHeadPosition = Vector3.zero;
    private Vector3 originalBeeHeadRotation = Vector3.zero;
    private Vector3 originalBeeLBackArm1Position = Vector3.zero;
    private Vector3 originalBeeLBackArm1Rotation = Vector3.zero;
    private Vector3 originalBeeLBackArm2Position = Vector3.zero;
    private Vector3 originalBeeLBackArm2Rotation = Vector3.zero;
    private Vector3 originalBeeLBackArm3Position = Vector3.zero;
    private Vector3 originalBeeLBackArm3Rotation = Vector3.zero;
    private Vector3 originalBeeLFrontArm1Position = Vector3.zero;
    private Vector3 originalBeeLFrontArm1Rotation = Vector3.zero;
    private Vector3 originalBeeLFrontArm2Position = Vector3.zero;
    private Vector3 originalBeeLFrontArm2Rotation = Vector3.zero;
    private Vector3 originalBeeLFrontArm3Position = Vector3.zero;
    private Vector3 originalBeeLFrontArm3Rotation = Vector3.zero;
    private Vector3 originalBeeLMidArm1Position = Vector3.zero;
    private Vector3 originalBeeLMidArm1Rotation = Vector3.zero;
    private Vector3 originalBeeLMidArm2Position = Vector3.zero;
    private Vector3 originalBeeLMidArm2Rotation = Vector3.zero;
    private Vector3 originalBeeLMidArm3Position = Vector3.zero;
    private Vector3 originalBeeLMidArm3Rotation = Vector3.zero;
    private Vector3 originalBeeLWing1Position = Vector3.zero;
    private Vector3 originalBeeLWing1Rotation = Vector3.zero;
    private Vector3 originalBeeLWing2Position = Vector3.zero;
    private Vector3 originalBeeLWing2Rotation = Vector3.zero;
    private Vector3 originalBeeLWing3Position = Vector3.zero;
    private Vector3 originalBeeLWing3Rotation = Vector3.zero;
    private Vector3 originalBeeLWing4Position = Vector3.zero;
    private Vector3 originalBeeLWing4Rotation = Vector3.zero;
    private Vector3 originalBeeLWing5Position = Vector3.zero;
    private Vector3 originalBeeLWing5Rotation = Vector3.zero;
    private Vector3 originalBeeRBackArm1Position = Vector3.zero;
    private Vector3 originalBeeRBackArm1Rotation = Vector3.zero;
    private Vector3 originalBeeRBackArm2Position = Vector3.zero;
    private Vector3 originalBeeRBackArm2Rotation = Vector3.zero;
    private Vector3 originalBeeRBackArm3Position = Vector3.zero;
    private Vector3 originalBeeRBackArm3Rotation = Vector3.zero;
    private Vector3 originalBeeRFrontArm1Position = Vector3.zero;
    private Vector3 originalBeeRFrontArm1Rotation = Vector3.zero;
    private Vector3 originalBeeRFrontArm2Position = Vector3.zero;
    private Vector3 originalBeeRFrontArm2Rotation = Vector3.zero;
    private Vector3 originalBeeRFrontArm3Position = Vector3.zero;
    private Vector3 originalBeeRFrontArm3Rotation = Vector3.zero;
    private Vector3 originalBeeRMidArm1Position = Vector3.zero;
    private Vector3 originalBeeRMidArm1Rotation = Vector3.zero;
    private Vector3 originalBeeRMidArm2Position = Vector3.zero;
    private Vector3 originalBeeRMidArm2Rotation = Vector3.zero;
    private Vector3 originalBeeRMidArm3Position = Vector3.zero;
    private Vector3 originalBeeRMidArm3Rotation = Vector3.zero;
    private Vector3 originalBeeRWing1Position = Vector3.zero;
    private Vector3 originalBeeRWing1Rotation = Vector3.zero;
    private Vector3 originalBeeRWing2Position = Vector3.zero;
    private Vector3 originalBeeRWing2Rotation = Vector3.zero;
    private Vector3 originalBeeRWing3Position = Vector3.zero;
    private Vector3 originalBeeRWing3Rotation = Vector3.zero;
    private Vector3 originalBeeRWing4Position = Vector3.zero;
    private Vector3 originalBeeRWing4Rotation = Vector3.zero;
    private Vector3 originalBeeRWing5Position = Vector3.zero;
    private Vector3 originalBeeRWing5Rotation = Vector3.zero;

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
        startingEntityModelRotation = entityModel.transform.rotation.eulerAngles;

        switch (myBoneProfile)
        {
            case BoneProfile.Humanoid:
                rootTransform = entityModel.transform.Find("Root");
                playerHips = entityModel.transform.Find("Root").Find("Hips").transform;
                colliders = entityModel.transform.Find("Root").GetComponentsInChildren<Collider>();
                break;
            case BoneProfile.Bee:
                Debug.Log("bee setup");
                rootTransform = entityModel.transform.Find("Root");
                playerHips = entityModel.transform.Find("Root").Find("RigBody").transform;
                colliders = entityModel.transform.Find("Root").GetComponentsInChildren<Collider>();
                Debug.Log(rootTransform);
                Debug.Log(playerHips);
                break;
            default:
                break;
        }

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

    public void LerpBonesToGetUpAnim()
    {
        //Debug.DrawRay(colliders[0].transform.position, colliders[0].transform.right, Color.blue);
        float angleOfdifference = Vector3.Angle(Vector3.up, colliders[0].transform.up);
        //Debug.Log(angleOfdifference);

        SetInitialBonePositionRotation();

        if (canGetUpFromBothSides)
        {
            if (angleOfdifference > 90)
                StartCoroutine(LerpBonesToFrontGetUp());
            else
                StartCoroutine(LerpBonesToBackGetUp());
        }
        else
            StartCoroutine(LerpBonesToFrontGetUp());

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

            switch (myBoneProfile)
            {
                case BoneProfile.Humanoid:
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
                    break;
                case BoneProfile.Bee:
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
                                targetPosition = faceupGetUpBeeBodyPosition;
                                targetRotation = Quaternion.Euler(faceupGetUpBeeBodyRotation);
                                initialPosition = originalBeeBodyPosition;
                                initialRotation = Quaternion.Euler(originalBeeBodyRotation);
                                break;
                            case 1:
                                targetPosition = faceupGetUpBeeTail2Position;
                                targetRotation = Quaternion.Euler(faceupGetUpBeeTail2Rotation);
                                initialPosition = originalBeeTail2Position;
                                initialRotation = Quaternion.Euler(originalBeeTail2Rotation);
                                break;
                            case 2:
                                targetPosition = faceupGetUpBeeTail3Position;
                                targetRotation = Quaternion.Euler(faceupGetUpBeeTail3Rotation);
                                initialPosition = originalBeeTail3Position;
                                initialRotation = Quaternion.Euler(originalBeeTail3Rotation);
                                break;
                            case 3:
                                targetPosition = faceupGetUpBeeTail4Position;
                                targetRotation = Quaternion.Euler(faceupGetUpBeeTail4Rotation);
                                initialPosition = originalBeeTail4Position;
                                initialRotation = Quaternion.Euler(originalBeeTail4Rotation);
                                break;
                            case 4:
                                targetPosition = faceupGetUpBeeHeadPosition;
                                targetRotation = Quaternion.Euler(faceupGetUpBeeHeadRotation);
                                initialPosition = originalBeeHeadPosition;
                                initialRotation = Quaternion.Euler(originalBeeHeadRotation);
                                break;
                            case 5:
                                targetPosition = faceupGetUpBeeLBackArm1Position;
                                targetRotation = Quaternion.Euler(faceupGetUpBeeLBackArm1Rotation);
                                initialPosition = originalBeeLBackArm1Position;
                                initialRotation = Quaternion.Euler(originalBeeLBackArm1Rotation);
                                break;
                            case 6:
                                targetPosition = faceupGetUpBeeLBackArm2Position;
                                targetRotation = Quaternion.Euler(faceupGetUpBeeLBackArm2Rotation);
                                initialPosition = originalBeeLBackArm2Position;
                                initialRotation = Quaternion.Euler(originalBeeLBackArm2Rotation);
                                break;
                            case 7:
                                targetPosition = faceupGetUpBeeLBackArm3Position;
                                targetRotation = Quaternion.Euler(faceupGetUpBeeLBackArm3Rotation);
                                initialPosition = originalBeeLBackArm3Position;
                                initialRotation = Quaternion.Euler(originalBeeLBackArm3Rotation);
                                break;
                            case 8:
                                targetPosition = faceupGetUpBeeLFrontArm1Position;
                                targetRotation = Quaternion.Euler(faceupGetUpBeeLFrontArm1Rotation);
                                initialPosition = originalBeeLFrontArm1Position;
                                initialRotation = Quaternion.Euler(originalBeeLFrontArm1Rotation);
                                break;
                            case 9:
                                targetPosition = faceupGetUpBeeLFrontArm2Position;
                                targetRotation = Quaternion.Euler(faceupGetUpBeeLFrontArm2Rotation);
                                initialPosition = originalBeeLFrontArm2Position;
                                initialRotation = Quaternion.Euler(originalBeeLFrontArm2Rotation);
                                break;
                            case 10:
                                targetPosition = faceupGetUpBeeLFrontArm3Position;
                                targetRotation = Quaternion.Euler(faceupGetUpBeeLFrontArm3Rotation);
                                initialPosition = originalBeeLFrontArm3Position;
                                initialRotation = Quaternion.Euler(originalBeeLFrontArm3Rotation);
                                break;
                            case 11:
                                targetPosition = faceupGetUpBeeLMidArm1Position;
                                targetRotation = Quaternion.Euler(faceupGetUpBeeLMidArm1Rotation);
                                initialPosition = originalBeeLMidArm1Position;
                                initialRotation = Quaternion.Euler(originalBeeLMidArm1Rotation);
                                break;
                            case 12:
                                targetPosition = faceupGetUpBeeLMidArm2Position;
                                targetRotation = Quaternion.Euler(faceupGetUpBeeLMidArm2Rotation);
                                initialPosition = originalBeeLMidArm2Position;
                                initialRotation = Quaternion.Euler(originalBeeLMidArm2Rotation);
                                break;
                            case 13:
                                targetPosition = faceupGetUpBeeLMidArm3Position;
                                targetRotation = Quaternion.Euler(faceupGetUpBeeLMidArm3Rotation);
                                initialPosition = originalBeeLMidArm3Position;
                                initialRotation = Quaternion.Euler(originalBeeLMidArm3Rotation);
                                break;
                            case 14:
                                targetPosition = faceupGetUpBeeLWing1Position;
                                targetRotation = Quaternion.Euler(faceupGetUpBeeLWing1Rotation);
                                initialPosition = originalBeeLWing1Position;
                                initialRotation = Quaternion.Euler(originalBeeLWing1Rotation);
                                break;
                            case 15:
                                targetPosition = faceupGetUpBeeLWing2Position;
                                targetRotation = Quaternion.Euler(faceupGetUpBeeLWing2Rotation);
                                initialPosition = originalBeeLWing2Position;
                                initialRotation = Quaternion.Euler(originalBeeLWing2Rotation);
                                break;
                            case 16:
                                targetPosition = faceupGetUpBeeLWing3Position;
                                targetRotation = Quaternion.Euler(faceupGetUpBeeLWing3Rotation);
                                initialPosition = originalBeeLWing3Position;
                                initialRotation = Quaternion.Euler(originalBeeLWing3Rotation);
                                break;
                            case 17:
                                targetPosition = faceupGetUpBeeLWing4Position;
                                targetRotation = Quaternion.Euler(faceupGetUpBeeLWing4Rotation);
                                initialPosition = originalBeeLWing4Position;
                                initialRotation = Quaternion.Euler(originalBeeLWing4Rotation);
                                break;
                            case 18:
                                targetPosition = faceupGetUpBeeLWing5Position;
                                targetRotation = Quaternion.Euler(faceupGetUpBeeLWing5Rotation);
                                initialPosition = originalBeeLWing5Position;
                                initialRotation = Quaternion.Euler(originalBeeLWing5Rotation);
                                break;
                            case 19:
                                targetPosition = faceupGetUpBeeRBackArm1Position;
                                targetRotation = Quaternion.Euler(faceupGetUpBeeRBackArm1Rotation);
                                initialPosition = originalBeeRBackArm1Position;
                                initialRotation = Quaternion.Euler(originalBeeRBackArm1Rotation);
                                break;
                            case 20:
                                targetPosition = faceupGetUpBeeRBackArm2Position;
                                targetRotation = Quaternion.Euler(faceupGetUpBeeRBackArm2Rotation);
                                initialPosition = originalBeeRBackArm2Position;
                                initialRotation = Quaternion.Euler(originalBeeRBackArm2Rotation);
                                break;
                            case 21:
                                targetPosition = faceupGetUpBeeRBackArm3Position;
                                targetRotation = Quaternion.Euler(faceupGetUpBeeRBackArm3Rotation);
                                initialPosition = originalBeeRBackArm3Position;
                                initialRotation = Quaternion.Euler(originalBeeRBackArm3Rotation);
                                break;
                            case 22:
                                targetPosition = faceupGetUpBeeRFrontArm1Position;
                                targetRotation = Quaternion.Euler(faceupGetUpBeeRFrontArm1Rotation);
                                initialPosition = originalBeeRFrontArm1Position;
                                initialRotation = Quaternion.Euler(originalBeeRFrontArm1Rotation);
                                break;
                            case 23:
                                targetPosition = faceupGetUpBeeRFrontArm2Position;
                                targetRotation = Quaternion.Euler(faceupGetUpBeeRFrontArm2Rotation);
                                initialPosition = originalBeeRFrontArm2Position;
                                initialRotation = Quaternion.Euler(originalBeeRFrontArm2Rotation);
                                break;
                            case 24:
                                targetPosition = faceupGetUpBeeRFrontArm3Position;
                                targetRotation = Quaternion.Euler(faceupGetUpBeeRFrontArm3Rotation);
                                initialPosition = originalBeeRFrontArm3Position;
                                initialRotation = Quaternion.Euler(originalBeeRFrontArm3Rotation);
                                break;
                            case 25:
                                targetPosition = faceupGetUpBeeRMidArm1Position;
                                targetRotation = Quaternion.Euler(faceupGetUpBeeRMidArm1Rotation);
                                initialPosition = originalBeeRMidArm1Position;
                                initialRotation = Quaternion.Euler(originalBeeRMidArm1Rotation);
                                break;
                            case 26:
                                targetPosition = faceupGetUpBeeRMidArm2Position;
                                targetRotation = Quaternion.Euler(faceupGetUpBeeRMidArm2Rotation);
                                initialPosition = originalBeeRMidArm2Position;
                                initialRotation = Quaternion.Euler(originalBeeRMidArm2Rotation);
                                break;
                            case 27:
                                targetPosition = faceupGetUpBeeRMidArm3Position;
                                targetRotation = Quaternion.Euler(faceupGetUpBeeRMidArm3Rotation);
                                initialPosition = originalBeeRMidArm3Position;
                                initialRotation = Quaternion.Euler(originalBeeRMidArm3Rotation);
                                break;
                            case 28:
                                targetPosition = faceupGetUpBeeRWing1Position;
                                targetRotation = Quaternion.Euler(faceupGetUpBeeRWing1Rotation);
                                initialPosition = originalBeeRWing1Position;
                                initialRotation = Quaternion.Euler(originalBeeRWing1Rotation);
                                break;
                            case 29:
                                targetPosition = faceupGetUpBeeRWing2Position;
                                targetRotation = Quaternion.Euler(faceupGetUpBeeRWing2Rotation);
                                initialPosition = originalBeeRWing2Position;
                                initialRotation = Quaternion.Euler(originalBeeRWing2Rotation);
                                break;
                            case 30:
                                targetPosition = faceupGetUpBeeRWing3Position;
                                targetRotation = Quaternion.Euler(faceupGetUpBeeRWing3Rotation);
                                initialPosition = originalBeeRWing3Position;
                                initialRotation = Quaternion.Euler(originalBeeRWing3Rotation);
                                break;
                            case 31:
                                targetPosition = faceupGetUpBeeRWing4Position;
                                targetRotation = Quaternion.Euler(faceupGetUpBeeRWing4Rotation);
                                initialPosition = originalBeeRWing4Position;
                                initialRotation = Quaternion.Euler(originalBeeRWing4Rotation);
                                break;
                            case 32:
                                targetPosition = faceupGetUpBeeRWing5Position;
                                targetRotation = Quaternion.Euler(faceupGetUpBeeRWing5Rotation);
                                initialPosition = originalBeeRWing5Position;
                                initialRotation = Quaternion.Euler(originalBeeRWing5Rotation);
                                break;
                            default:
                                break;
                        }

                        // Begin the lerp 

                        boneToLerp.localPosition = Vector3.Lerp(initialPosition, targetPosition, currentTimer / targetTimer);
                        boneToLerp.localRotation = Quaternion.Slerp(initialRotation, targetRotation, currentTimer / targetTimer);
                    }
                    break;
                default:
                    break;
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
        switch (myBoneProfile)
        {
            case BoneProfile.Humanoid:
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
                break;
            case BoneProfile.Bee:
                for (int index = 0; index < bonesToLerp.Length; index++)
                {
                    Transform boneToMarkDown = bonesToLerp[index];

                    // Ste our target
                    switch (index)
                    {
                        case 0:
                            originalBeeBodyPosition = boneToMarkDown.localPosition;
                            originalBeeBodyRotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 1:
                            originalBeeTail2Position = boneToMarkDown.localPosition;
                            originalBeeTail2Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 2:
                            originalBeeTail3Position = boneToMarkDown.localPosition;
                            originalBeeTail3Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 3:
                            originalBeeTail4Position = boneToMarkDown.localPosition;
                            originalBeeTail4Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 4:
                            originalBeeHeadPosition = boneToMarkDown.localPosition;
                            originalBeeHeadRotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 5:
                            originalBeeLBackArm1Position = boneToMarkDown.localPosition;
                            originalBeeLBackArm1Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 6:
                            originalBeeLBackArm2Position = boneToMarkDown.localPosition;
                            originalBeeLBackArm2Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 7:
                            originalBeeLBackArm3Position = boneToMarkDown.localPosition;
                            originalBeeLBackArm3Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 8:
                            originalBeeLFrontArm1Position = boneToMarkDown.localPosition;
                            originalBeeLFrontArm1Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 9:
                            originalBeeLFrontArm2Position = boneToMarkDown.localPosition;
                            originalBeeLFrontArm2Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 10:
                            originalBeeLFrontArm3Position = boneToMarkDown.localPosition;
                            originalBeeLFrontArm3Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 11:
                            originalBeeLMidArm1Position = boneToMarkDown.localPosition;
                            originalBeeLMidArm1Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 12:
                            originalBeeLMidArm2Position = boneToMarkDown.localPosition;
                            originalBeeLMidArm2Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 13:
                            originalBeeLMidArm3Position = boneToMarkDown.localPosition;
                            originalBeeLMidArm3Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 14:
                            originalBeeLWing1Position = boneToMarkDown.localPosition;
                            originalBeeLWing1Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 15:
                            originalBeeLWing2Position = boneToMarkDown.localPosition;
                            originalBeeLWing2Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 16:
                            originalBeeLWing3Position = boneToMarkDown.localPosition;
                            originalBeeLWing3Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 17:
                            originalBeeLWing4Position = boneToMarkDown.localPosition;
                            originalBeeLWing4Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 18:
                            originalBeeLWing5Position = boneToMarkDown.localPosition;
                            originalBeeLWing5Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 19:
                            originalBeeRBackArm1Position = boneToMarkDown.localPosition;
                            originalBeeRBackArm1Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 20:
                            originalBeeRBackArm2Position = boneToMarkDown.localPosition;
                            originalBeeRBackArm2Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 21:
                            originalBeeRBackArm3Position = boneToMarkDown.localPosition;
                            originalBeeRBackArm3Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 22:
                            originalBeeRFrontArm1Position = boneToMarkDown.localPosition;
                            originalBeeRFrontArm1Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 23:
                            originalBeeRFrontArm2Position = boneToMarkDown.localPosition;
                            originalBeeRFrontArm2Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 24:
                            originalBeeRFrontArm3Position = boneToMarkDown.localPosition;
                            originalBeeRFrontArm3Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 25:
                            originalBeeRMidArm1Position = boneToMarkDown.localPosition;
                            originalBeeRMidArm1Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 26:
                            originalBeeRMidArm2Position = boneToMarkDown.localPosition;
                            originalBeeRMidArm2Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 27:
                            originalBeeRMidArm3Position = boneToMarkDown.localPosition;
                            originalBeeRMidArm3Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 28:
                            originalBeeRWing1Position = boneToMarkDown.localPosition;
                            originalBeeRWing1Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 29:
                            originalBeeRWing2Position = boneToMarkDown.localPosition;
                            originalBeeRWing2Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 30:
                            originalBeeRWing3Position = boneToMarkDown.localPosition;
                            originalBeeRWing3Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 31:
                            originalBeeRWing4Position = boneToMarkDown.localPosition;
                            originalBeeRWing4Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 32:
                            originalBeeRWing5Position = boneToMarkDown.localPosition;
                            originalBeeRWing5Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                    }
                }
                break;
            default:
                break;
        }
    }

    private void OnDestroy()
    {
        entityModel.transform.parent = transform;
    }
}
