using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollManager : MonoBehaviour
{
    public FollowPlayer cameraFollow;

    public enum BoneProfile {Humanoid, Bee, Snek, Wolf};
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

    private Vector3 faceupGetUpSnakePelvisPosition = new Vector3(0, 0.2617419f, 0.1515051f);
    private Vector3 faceupGetUpSnakePelvisRotation = new Vector3(0, -65, -117.055f);
    private Vector3 faceupGetUpSnakeSpine1Position = new Vector3(0, 0, 0);
    private Vector3 faceupGetUpSnakeSpine1Rotation = new Vector3(0.6810001f, -0.224f, 8.787001f);
    private Vector3 faceupGetUpSnakeSpine2Position = new Vector3(-0.1523528f, 0, 0);
    private Vector3 faceupGetUpSnakeSpine2Rotation = new Vector3(7.1f, -1.291f, 60.744f);
    private Vector3 faceupGetUpSnakeSpine3Position = new Vector3(-0.1376112f, 0, 0);
    private Vector3 faceupGetUpSnakeSpine3Rotation = new Vector3(19.489f, -5.439f, 15.749f);
    private Vector3 faceupGetUpSnakeSpine4Position = new Vector3(-0.1314502f, 0, 0);
    private Vector3 faceupGetUpSnakeSpine4Rotation = new Vector3(16.383f, -12.416f, 34.548f);
    private Vector3 faceupGetUpSnakeSpine5Position = new Vector3(-0.1471077f, 0, 0);
    private Vector3 faceupGetUpSnakeSpine5Rotation = new Vector3(11.677f, -12.441f, 20.64f);
    private Vector3 faceupGetUpSnakeSpine6Position = new Vector3(-0.1320765f, 0, 0);
    private Vector3 faceupGetUpSnakeSpine6Rotation = new Vector3(-0.504f, -8.497001f, 27.047f);
    private Vector3 faceupGetUpSnakeSpine7Position = new Vector3(-0.1110411f, 0, 0);
    private Vector3 faceupGetUpSnakeSpine7Rotation = new Vector3(-12.972f, 5.622f, -3.518f);
    private Vector3 faceupGetUpSnakeRibcagePosition = new Vector3(-0.1340093f, 0f, 0);
    private Vector3 faceupGetUpSnakeRibcageRotation = new Vector3(-10.783f, 7.900001f, -2.785f);
    private Vector3 faceupGetUpSnakeNeck1Position = new Vector3(0, 0, 0);
    private Vector3 faceupGetUpSnakeNeck1Rotation = new Vector3(-0.207f, 0.093f, -19.741f);
    private Vector3 faceupGetUpSnakeNeck2Position = new Vector3(-0.1700639f, 0, 0);
    private Vector3 faceupGetUpSnakeNeck2Rotation = new Vector3(-1.096f, 1.12f, -16.621f);
    private Vector3 faceupGetUpSnakeNeck3Position = new Vector3(-0.2244231f, 0, 0);
    private Vector3 faceupGetUpSnakeNeck3Rotation = new Vector3(-1.908f, 1.197f, -8.268001f);
    private Vector3 faceupGetUpSnakeNeck4Position = new Vector3(-0.1486681f, 0, 0);
    private Vector3 faceupGetUpSnakeNeck4Rotation = new Vector3(-2.366f, -1.766f, 17.605f);
    private Vector3 faceupGetUpSnakeNeck5Position = new Vector3(-0.1236803f, 0, 0);
    private Vector3 faceupGetUpSnakeNeck5Rotation = new Vector3(-0.167f, -5.747f, 3.77f);
    private Vector3 faceupGetUpSnakeHeadPosition = new Vector3(-0.1153349f, 0, 0);
    private Vector3 faceupGetUpSnakeHeadRotation = new Vector3(2.879f, -2.954f, 91.05f);
    private Vector3 faceupGetUpSnakeLArm1Position = new Vector3(-0.1073348f, -0.132188f, 0.2238881f);
    private Vector3 faceupGetUpSnakeLArm1Rotation = new Vector3(69.69701f, -112.221f, 158.004f);
    private Vector3 faceupGetUpSnakeLArm2Position = new Vector3(-0.162615f, 0, 0);
    private Vector3 faceupGetUpSnakeLArm2Rotation = new Vector3(-88.285f, 87.27901f, -85.27901f);
    private Vector3 faceupGetUpSnakeRArm1Position = new Vector3(-0.1073347f, -0.132188f, -0.223888f);
    private Vector3 faceupGetUpSnakeRArm1Rotation = new Vector3(-75.8f, 128.01f, 142.221f);
    private Vector3 faceupGetUpSnakeRArm2Position = new Vector3(-0.162615f, 0, 0);
    private Vector3 faceupGetUpSnakeRArm2Rotation = new Vector3(88.414f, -127.96f, -118.532f);
    private Vector3 faceupGetUpSnakeTail1Position = new Vector3(0.0702024f, 0, 0);
    private Vector3 faceupGetUpSnakeTail1Rotation = new Vector3(0, 0, 131.472f);
    private Vector3 faceupGetUpSnakeTail2Position = new Vector3(-0.1888614f, 0, 0);
    private Vector3 faceupGetUpSnakeTail2Rotation = new Vector3(1.269f, 4.945f, -10.644f);
    private Vector3 faceupGetUpSnakeTail3Position = new Vector3(-0.2580396f, 0, 0);
    private Vector3 faceupGetUpSnakeTail3Rotation = new Vector3(0.942f, 14.651f, 1.317f);
    private Vector3 faceupGetUpSnakeTail4Position = new Vector3(-0.2777665f, 0, 0);
    private Vector3 faceupGetUpSnakeTail4Rotation = new Vector3(3.918f, 37.119f, 3.663f);
    private Vector3 faceupGetUpSnakeTail5Position = new Vector3(-0.2743517f, 0, 0);
    private Vector3 faceupGetUpSnakeTail5Rotation = new Vector3(3.207f, 41.637f, -7.87f);
    private Vector3 faceupGetUpSnakeTail6Position = new Vector3(-0.2657467f, 0, 0);
    private Vector3 faceupGetUpSnakeTail6Rotation = new Vector3(8.038f, 51.813f, 2.263f);
    private Vector3 faceupGetUpSnakeTail7Position = new Vector3(-0.2142581f, 0, 0);
    private Vector3 faceupGetUpSnakeTail7Rotation = new Vector3(-0.37f, 16.845f, -2.339f);
    private Vector3 faceupGetUpSnakeTail8Position = new Vector3(-0.1965372f, 0, 0);
    private Vector3 faceupGetUpSnakeTail8Rotation = new Vector3(-18.144f, 39.318f, -5.501f);
    private Vector3 faceupGetUpSnakeTail9Position = new Vector3(-0.2189824f, 0, 0);
    private Vector3 faceupGetUpSnakeTail9Rotation = new Vector3(5.939f, 19.012f, 6.876f);
    private Vector3 faceupGetUpSnakeTail10Position = new Vector3(-0.2215247f, 0, 0);
    private Vector3 faceupGetUpSnakeTail10Rotation = new Vector3(11.898f, 6.235f, 11.159f);
    private Vector3 faceupGetUpSnakeTail11Position = new Vector3(-0.1450966f, 0, 0);
    private Vector3 faceupGetUpSnakeTail11Rotation = new Vector3(-5.37f, 17.647f, -13.656f);

    private Vector3 originalSnakePelvisPosition = Vector3.zero;
    private Vector3 originalSnakePelvisRotation = Vector3.zero;
    private Vector3 originalSnakeSpine1Position = Vector3.zero;
    private Vector3 originalSnakeSpine1Rotation = Vector3.zero;
    private Vector3 originalSnakeSpine2Position = Vector3.zero;
    private Vector3 originalSnakeSpine2Rotation = Vector3.zero;
    private Vector3 originalSnakeSpine3Position = Vector3.zero;
    private Vector3 originalSnakeSpine3Rotation = Vector3.zero;
    private Vector3 originalSnakeSpine4Position = Vector3.zero;
    private Vector3 originalSnakeSpine4Rotation = Vector3.zero;
    private Vector3 originalSnakeSpine5Position = Vector3.zero;
    private Vector3 originalSnakeSpine5Rotation = Vector3.zero;
    private Vector3 originalSnakeSpine6Position = Vector3.zero;
    private Vector3 originalSnakeSpine6Rotation = Vector3.zero;
    private Vector3 originalSnakeSpine7Position = Vector3.zero;
    private Vector3 originalSnakeSpine7Rotation = Vector3.zero;
    private Vector3 originalSnakeRibcagePosition = Vector3.zero;
    private Vector3 originalSnakeRibcageRotation = Vector3.zero;
    private Vector3 originalSnakeNeck1Position = Vector3.zero;
    private Vector3 originalSnakeNeck1Rotation = Vector3.zero;
    private Vector3 originalSnakeNeck2Position = Vector3.zero;
    private Vector3 originalSnakeNeck2Rotation = Vector3.zero;
    private Vector3 originalSnakeNeck3Position = Vector3.zero;
    private Vector3 originalSnakeNeck3Rotation = Vector3.zero;
    private Vector3 originalSnakeNeck4Position = Vector3.zero;
    private Vector3 originalSnakeNeck4Rotation = Vector3.zero;
    private Vector3 originalSnakeNeck5Position = Vector3.zero;
    private Vector3 originalSnakeNeck5Rotation = Vector3.zero;
    private Vector3 originalSnakeHeadPosition = Vector3.zero;
    private Vector3 originalSnakeHeadRotation = Vector3.zero;
    private Vector3 originalSnakeLArm1Position = Vector3.zero;
    private Vector3 originalSnakeLArm1Rotation = Vector3.zero;
    private Vector3 originalSnakeLArm2Position = Vector3.zero;
    private Vector3 originalSnakeLArm2Rotation = Vector3.zero;
    private Vector3 originalSnakeRArm1Position = Vector3.zero;
    private Vector3 originalSnakeRArm1Rotation = Vector3.zero;
    private Vector3 originalSnakeRArm2Position = Vector3.zero;
    private Vector3 originalSnakeRArm2Rotation = Vector3.zero;
    private Vector3 originalSnakeTail1Position = Vector3.zero;
    private Vector3 originalSnakeTail1Rotation = Vector3.zero;
    private Vector3 originalSnakeTail2Position = Vector3.zero;
    private Vector3 originalSnakeTail2Rotation = Vector3.zero;
    private Vector3 originalSnakeTail3Position = Vector3.zero;
    private Vector3 originalSnakeTail3Rotation = Vector3.zero;
    private Vector3 originalSnakeTail4Position = Vector3.zero;
    private Vector3 originalSnakeTail4Rotation = Vector3.zero;
    private Vector3 originalSnakeTail5Position = Vector3.zero;
    private Vector3 originalSnakeTail5Rotation = Vector3.zero;
    private Vector3 originalSnakeTail6Position = Vector3.zero;
    private Vector3 originalSnakeTail6Rotation = Vector3.zero;
    private Vector3 originalSnakeTail7Position = Vector3.zero;
    private Vector3 originalSnakeTail7Rotation = Vector3.zero;
    private Vector3 originalSnakeTail8Position = Vector3.zero;
    private Vector3 originalSnakeTail8Rotation = Vector3.zero;
    private Vector3 originalSnakeTail9Position = Vector3.zero;
    private Vector3 originalSnakeTail9Rotation = Vector3.zero;
    private Vector3 originalSnakeTail10Position = Vector3.zero;
    private Vector3 originalSnakeTail10Rotation = Vector3.zero;
    private Vector3 originalSnakeTail11Position = Vector3.zero;
    private Vector3 originalSnakeTail11Rotation = Vector3.zero;

    private Vector3 faceupGetUpWolfPelvisPosition = new Vector3(0.3408259f, 0.3058549f, -0.3246979f);
    private Vector3 faceupGetUpWolfPelvisRotation = new Vector3(-22.048f, -82.305f, -163.5f);
    private Vector3 faceupGetUpWolfBLLeg1Position = new Vector3(0.01755646f, 0.05833435f, 0.1093739f);
    private Vector3 faceupGetUpWolfBLLeg1Rotation = new Vector3(-39.802f, 151.896f, -62.17f);
    private Vector3 faceupGetUpWolfBLLeg2Position = new Vector3(-0.2878836f, 0, 0);
    private Vector3 faceupGetUpWolfBLLeg2Rotation = new Vector3(2.889f, 1.606f, 94.061f);
    private Vector3 faceupGetUpWolfBLLeg3Position = new Vector3(-0.1862568f, 0, 0);
    private Vector3 faceupGetUpWolfBLLeg3Rotation = new Vector3(21.569f, 8.668f, -102.142f);
    private Vector3 faceupGetUpWolfBLAnklePosition = new Vector3(-0.1960851f, 0, 0);
    private Vector3 faceupGetUpWolfBLAnkleRotation = new Vector3(-6.914001f, -9.423f, -12.337f);
    private Vector3 faceupGetUpWolfBLFoot1Position = new Vector3(-0.1072237f, 0, 0);
    private Vector3 faceupGetUpWolfBLFoot1Rotation = new Vector3(0, 0.002f, -61.372f);
    private Vector3 faceupGetUpWolfBLFoot2Position = new Vector3(-0.0726627f, 0, 0);
    private Vector3 faceupGetUpWolfBLFoot2Rotation = new Vector3(0, 0, 0);
    private Vector3 faceupGetUpWolfBRLeg1Position = new Vector3(0.01755648f, 0.05833435f, -0.109374f);
    private Vector3 faceupGetUpWolfBRLeg1Rotation = new Vector3(-24.71f, 146.86f, -39.907f);
    private Vector3 faceupGetUpWolfBRLeg2Position = new Vector3(-0.2878835f, 0, 0);
    private Vector3 faceupGetUpWolfBRLeg2Rotation = new Vector3(2.805f, -2.014f, 79.288f);
    private Vector3 faceupGetUpWolfBRLeg3Position = new Vector3(-0.1862568f, 0, 0);
    private Vector3 faceupGetUpWolfBRLeg3Rotation = new Vector3(-12.985f, -8.22f, -86.997f);
    private Vector3 faceupGetUpWolfBRAnklePosition = new Vector3(-0.1960851f, 0, 0);
    private Vector3 faceupGetUpWolfBRAnkleRotation = new Vector3(-53.162f, 20.171f, -5.252f);
    private Vector3 faceupGetUpWolfBRFoot1Position = new Vector3(-0.1072235f, 0, 0);
    private Vector3 faceupGetUpWolfBRFoot1Rotation = new Vector3(0, -0.008f, -61.353f);
    private Vector3 faceupGetUpWolfBRFoot2Position = new Vector3(-0.07266266f, 0, 0);
    private Vector3 faceupGetUpWolfBRFoot2Rotation = new Vector3(0, 0, 0);
    private Vector3 faceupGetUpWolfTail1Position = new Vector3(0.001383886f, 0.001607361f, 0);
    private Vector3 faceupGetUpWolfTail1Rotation = new Vector3(0, 0, 155.849f);
    private Vector3 faceupGetUpWolfTail2Position = new Vector3(-0.09901333f, 0, 0);
    private Vector3 faceupGetUpWolfTail2Rotation = new Vector3(-12.934f, -12.235f, 49.305f);
    private Vector3 faceupGetUpWolfTail3Position = new Vector3(-0.123868f, 0, 0);
    private Vector3 faceupGetUpWolfTail3Rotation = new Vector3(-14.098f, 9.696f, 13.397f);
    private Vector3 faceupGetUpWolfTail4Position = new Vector3(-0.1256545f, 0, 0);
    private Vector3 faceupGetUpWolfTail4Rotation = new Vector3(-12.132f, 11.211f, -9.976001f);
    private Vector3 faceupGetUpWolfTail5Position = new Vector3(-0.1816296f, 0, 0);
    private Vector3 faceupGetUpWolfTail5Rotation = new Vector3(-10.835f, 7.996f, -36.673f);
    private Vector3 faceupGetUpWolfTail6Position = new Vector3(-0.08322937f, 0, 0);
    private Vector3 faceupGetUpWolfTail6Rotation = new Vector3(-8.911f, 1.389f, -17.425f);
    private Vector3 faceupGetUpWolfSpine1Position = new Vector3(0, 0, 0);
    private Vector3 faceupGetUpWolfSpine1Rotation = new Vector3(-0.6950001f, -3.242f, -6.34f);
    private Vector3 faceupGetUpWolfSpine2Position = new Vector3(-0.2346027f, 0, 0);
    private Vector3 faceupGetUpWolfSpine2Rotation = new Vector3(0.997f, 9.243f, -10.374f);
    private Vector3 faceupGetUpWolfSpine3Position = new Vector3(-0.2344154f, 0, 0);
    private Vector3 faceupGetUpWolfSpine3Rotation = new Vector3(1.314f, 12.158f, -11.959f);
    private Vector3 faceupGetUpWolfRibcagePosition = new Vector3(-0.2344155f, 0, 0);
    private Vector3 faceupGetUpWolfRibcageRotation = new Vector3(0.034f, 4.536f, 4.248f);
    private Vector3 faceupGetUpWolfNeck1Position = new Vector3(0, 0, 0);
    private Vector3 faceupGetUpWolfNeck1Rotation = new Vector3(0.369f, 3.673f, -13.077f);
    private Vector3 faceupGetUpWolfNeck2Position = new Vector3(-0.1701972f, 0, 0);
    private Vector3 faceupGetUpWolfNeck2Rotation = new Vector3(3.506f, 16.652f, -27.561f);
    private Vector3 faceupGetUpWolfHeadPosition = new Vector3(-0.1701973f, 0, 0);
    private Vector3 faceupGetUpWolfHeadRotation = new Vector3(-5.115f, 5.916f, -0.421f);
    private Vector3 faceupGetUpWolfFLLeg1Position = new Vector3(0.08744419f, 0.07312885f, 0.1658689f);
    private Vector3 faceupGetUpWolfFLLeg1Rotation = new Vector3(-148.674f, 10.27299f, 89.28699f);
    private Vector3 faceupGetUpWolfFLLeg2Position = new Vector3(-0.3627603f, 0, 0);
    private Vector3 faceupGetUpWolfFLLeg2Rotation = new Vector3(5.989f, 1.03f, -87.746f);
    private Vector3 faceupGetUpWolfFLAnklePosition = new Vector3(-0.2677539f, 0, 0);
    private Vector3 faceupGetUpWolfFLAnkleRotation = new Vector3(1.65f, 3.832f, 53.296f);
    private Vector3 faceupGetUpWolfFLFoot1Position = new Vector3(-0.1072236f, 0, 0);
    private Vector3 faceupGetUpWolfFLFoot1Rotation = new Vector3(0, -0.002f, -61.332f);
    private Vector3 faceupGetUpWolfFLFoot2Position = new Vector3(-0.0867643f, 0, 0);
    private Vector3 faceupGetUpWolfFLFoot2Rotation = new Vector3(0, 0, 0);
    private Vector3 faceupGetUpWolfFRLeg1Position = new Vector3(0.08744425f, 0.07312877f, -0.1658689f);
    private Vector3 faceupGetUpWolfFRLeg1Rotation = new Vector3(-129.681f, -4.713013f, 118.991f);
    private Vector3 faceupGetUpWolfFRLeg2Position = new Vector3(-0.3627605f, 0, 0);
    private Vector3 faceupGetUpWolfFRLeg2Rotation = new Vector3(-5.538f, -0.904f, -61.394f);
    private Vector3 faceupGetUpWolfFRAnklePosition = new Vector3(-0.2677533f, 0, 0);
    private Vector3 faceupGetUpWolfFRAnkleRotation = new Vector3(7.151f, 7.679f, -8.174001f);
    private Vector3 faceupGetUpWolfFRFoot1Position = new Vector3(-0.1072236f, 0, 0);
    private Vector3 faceupGetUpWolfFRFoot1Rotation = new Vector3(0, 0.002f, -61.332f);
    private Vector3 faceupGetUpWolfFRFoot2Position = new Vector3(-0.0867643f, 0, 0);
    private Vector3 faceupGetUpWolfFRFoot2Rotation = new Vector3(0, 0, 0);

    private Vector3 originalWolfPelvisPosition = Vector3.zero;
    private Vector3 originalWolfPelvisRotation = Vector3.zero;
    private Vector3 originalWolfBLLeg1Position = Vector3.zero;
    private Vector3 originalWolfBLLeg1Rotation = Vector3.zero;
    private Vector3 originalWolfBLLeg2Position = Vector3.zero;
    private Vector3 originalWolfBLLeg2Rotation = Vector3.zero;
    private Vector3 originalWolfBLLeg3Position = Vector3.zero;
    private Vector3 originalWolfBLLeg3Rotation = Vector3.zero;
    private Vector3 originalWolfBLAnklePosition = Vector3.zero;
    private Vector3 originalWolfBLAnkleRotation = Vector3.zero;
    private Vector3 originalWolfBLFoot1Position = Vector3.zero;
    private Vector3 originalWolfBLFoot1Rotation = Vector3.zero;
    private Vector3 originalWolfBLFoot2Position = Vector3.zero;
    private Vector3 originalWolfBLFoot2Rotation = Vector3.zero;
    private Vector3 originalWolfBRLeg1Position = Vector3.zero;
    private Vector3 originalWolfBRLeg1Rotation = Vector3.zero;
    private Vector3 originalWolfBRLeg2Position = Vector3.zero;
    private Vector3 originalWolfBRLeg2Rotation = Vector3.zero;
    private Vector3 originalWolfBRLeg3Position = Vector3.zero;
    private Vector3 originalWolfBRLeg3Rotation = Vector3.zero;
    private Vector3 originalWolfBRAnklePosition = Vector3.zero;
    private Vector3 originalWolfBRAnkleRotation = Vector3.zero;
    private Vector3 originalWolfBRFoot1Position = Vector3.zero;
    private Vector3 originalWolfBRFoot1Rotation = Vector3.zero;
    private Vector3 originalWolfBRFoot2Position = Vector3.zero;
    private Vector3 originalWolfBRFoot2Rotation = Vector3.zero;
    private Vector3 originalWolfTail1Position = Vector3.zero;
    private Vector3 originalWolfTail1Rotation = Vector3.zero;
    private Vector3 originalWolfTail2Position = Vector3.zero;
    private Vector3 originalWolfTail2Rotation = Vector3.zero;
    private Vector3 originalWolfTail3Position = Vector3.zero;
    private Vector3 originalWolfTail3Rotation = Vector3.zero;
    private Vector3 originalWolfTail4Position = Vector3.zero;
    private Vector3 originalWolfTail4Rotation = Vector3.zero;
    private Vector3 originalWolfTail5Position = Vector3.zero;
    private Vector3 originalWolfTail5Rotation = Vector3.zero;
    private Vector3 originalWolfTail6Position = Vector3.zero;
    private Vector3 originalWolfTail6Rotation = Vector3.zero;
    private Vector3 originalWolfSpine1Position = Vector3.zero;
    private Vector3 originalWolfSpine1Rotation = Vector3.zero;
    private Vector3 originalWolfSpine2Position = Vector3.zero;
    private Vector3 originalWolfSpine2Rotation = Vector3.zero;
    private Vector3 originalWolfSpine3Position = Vector3.zero;
    private Vector3 originalWolfSpine3Rotation = Vector3.zero;
    private Vector3 originalWolfRibcagePosition = Vector3.zero;
    private Vector3 originalWolfRibcageRotation = Vector3.zero;
    private Vector3 originalWolfNeck1Position = Vector3.zero;
    private Vector3 originalWolfNeck1Rotation = Vector3.zero;
    private Vector3 originalWolfNeck2Position = Vector3.zero;
    private Vector3 originalWolfNeck2Rotation = Vector3.zero;
    private Vector3 originalWolfHeadPosition = Vector3.zero;
    private Vector3 originalWolfHeadRotation = Vector3.zero;
    private Vector3 originalWolfFLLeg1Position = Vector3.zero;
    private Vector3 originalWolfFLLeg1Rotation = Vector3.zero;
    private Vector3 originalWolfFLLeg2Position = Vector3.zero;
    private Vector3 originalWolfFLLeg2Rotation = Vector3.zero;
    private Vector3 originalWolfFLAnklePosition = Vector3.zero;
    private Vector3 originalWolfFLAnkleRotation = Vector3.zero;
    private Vector3 originalWolfFLFoot1Position = Vector3.zero;
    private Vector3 originalWolfFLFoot1Rotation = Vector3.zero;
    private Vector3 originalWolfFLFoot2Position = Vector3.zero;
    private Vector3 originalWolfFLFoot2Rotation = Vector3.zero;
    private Vector3 originalWolfFRLeg1Position = Vector3.zero;
    private Vector3 originalWolfFRLeg1Rotation = Vector3.zero;
    private Vector3 originalWolfFRLeg2Position = Vector3.zero;
    private Vector3 originalWolfFRLeg2Rotation = Vector3.zero;
    private Vector3 originalWolfFRAnklePosition = Vector3.zero;
    private Vector3 originalWolfFRAnkleRotation = Vector3.zero;
    private Vector3 originalWolfFRFoot1Position = Vector3.zero;
    private Vector3 originalWolfFRFoot1Rotation = Vector3.zero;
    private Vector3 originalWolfFRFoot2Position = Vector3.zero;
    private Vector3 originalWolfFRFoot2Rotation = Vector3.zero;

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
                case BoneProfile.Snek:
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
                                targetPosition = faceupGetUpSnakePelvisPosition;
                                targetRotation = Quaternion.Euler(faceupGetUpSnakePelvisRotation);
                                initialPosition = originalSnakePelvisPosition;
                                initialRotation = Quaternion.Euler(originalSnakePelvisRotation);
                                break;
                            case 1:
                                targetPosition = faceupGetUpSnakeSpine1Position;
                                targetRotation = Quaternion.Euler(faceupGetUpSnakeSpine1Rotation);
                                initialPosition = originalSnakeSpine1Position;
                                initialRotation = Quaternion.Euler(originalSnakeSpine1Rotation);
                                break;
                            case 2:
                                targetPosition = faceupGetUpSnakeSpine2Position;
                                targetRotation = Quaternion.Euler(faceupGetUpSnakeSpine2Rotation);
                                initialPosition = originalSnakeSpine2Position;
                                initialRotation = Quaternion.Euler(originalSnakeSpine2Rotation);
                                break;
                            case 3:
                                targetPosition = faceupGetUpSnakeSpine3Position;
                                targetRotation = Quaternion.Euler(faceupGetUpSnakeSpine3Rotation);
                                initialPosition = originalSnakeSpine3Position;
                                initialRotation = Quaternion.Euler(originalSnakeSpine3Rotation);
                                break;
                            case 4:
                                targetPosition = faceupGetUpSnakeSpine4Position;
                                targetRotation = Quaternion.Euler(faceupGetUpSnakeSpine4Rotation);
                                initialPosition = originalSnakeSpine4Position;
                                initialRotation = Quaternion.Euler(originalSnakeSpine4Rotation);
                                break;
                            case 5:
                                targetPosition = faceupGetUpSnakeSpine5Position;
                                targetRotation = Quaternion.Euler(faceupGetUpSnakeSpine5Rotation);
                                initialPosition = originalSnakeSpine5Position;
                                initialRotation = Quaternion.Euler(originalSnakeSpine5Rotation);
                                break;
                            case 6:
                                targetPosition = faceupGetUpSnakeSpine6Position;
                                targetRotation = Quaternion.Euler(faceupGetUpSnakeSpine6Rotation);
                                initialPosition = originalSnakeSpine6Position;
                                initialRotation = Quaternion.Euler(originalSnakeSpine6Rotation);
                                break;
                            case 7:
                                targetPosition = faceupGetUpSnakeSpine7Position;
                                targetRotation = Quaternion.Euler(faceupGetUpSnakeSpine7Rotation);
                                initialPosition = originalSnakeSpine7Position;
                                initialRotation = Quaternion.Euler(originalSnakeSpine7Rotation);
                                break;
                            case 8:
                                targetPosition = faceupGetUpSnakeRibcagePosition;
                                targetRotation = Quaternion.Euler(faceupGetUpSnakeRibcageRotation);
                                initialPosition = originalSnakeRibcagePosition;
                                initialRotation = Quaternion.Euler(originalSnakeRibcageRotation);
                                break;
                            case 9:
                                targetPosition = faceupGetUpSnakeNeck1Position;
                                targetRotation = Quaternion.Euler(faceupGetUpSnakeNeck1Rotation);
                                initialPosition = originalSnakeNeck1Position;
                                initialRotation = Quaternion.Euler(originalSnakeNeck1Rotation);
                                break;
                            case 10:
                                targetPosition = faceupGetUpSnakeNeck2Position;
                                targetRotation = Quaternion.Euler(faceupGetUpSnakeNeck2Rotation);
                                initialPosition = originalSnakeNeck2Position;
                                initialRotation = Quaternion.Euler(originalSnakeNeck2Rotation);
                                break;
                            case 11:
                                targetPosition = faceupGetUpSnakeNeck3Position;
                                targetRotation = Quaternion.Euler(faceupGetUpSnakeNeck3Rotation);
                                initialPosition = originalSnakeNeck3Position;
                                initialRotation = Quaternion.Euler(originalSnakeNeck3Rotation);
                                break;
                            case 12:
                                targetPosition = faceupGetUpSnakeNeck4Position;
                                targetRotation = Quaternion.Euler(faceupGetUpSnakeNeck4Rotation);
                                initialPosition = originalSnakeNeck4Position;
                                initialRotation = Quaternion.Euler(originalSnakeNeck4Rotation);
                                break;
                            case 13:
                                targetPosition = faceupGetUpSnakeNeck5Position;
                                targetRotation = Quaternion.Euler(faceupGetUpSnakeNeck5Rotation);
                                initialPosition = originalSnakeNeck5Position;
                                initialRotation = Quaternion.Euler(originalSnakeNeck5Rotation);
                                break;
                            case 14:
                                targetPosition = faceupGetUpSnakeHeadPosition;
                                targetRotation = Quaternion.Euler(faceupGetUpSnakeHeadRotation);
                                initialPosition = originalSnakeHeadPosition;
                                initialRotation = Quaternion.Euler(originalSnakeHeadRotation);
                                break;
                            case 15:
                                targetPosition = faceupGetUpSnakeLArm1Position;
                                targetRotation = Quaternion.Euler(faceupGetUpSnakeLArm1Rotation);
                                initialPosition = originalSnakeLArm1Position;
                                initialRotation = Quaternion.Euler(originalSnakeLArm1Rotation);
                                break;
                            case 16:
                                targetPosition = faceupGetUpSnakeLArm2Position;
                                targetRotation = Quaternion.Euler(faceupGetUpSnakeLArm2Rotation);
                                initialPosition = originalSnakeLArm2Position;
                                initialRotation = Quaternion.Euler(originalSnakeLArm2Rotation);
                                break;
                            case 17:
                                targetPosition = faceupGetUpSnakeRArm1Position;
                                targetRotation = Quaternion.Euler(faceupGetUpSnakeRArm1Rotation);
                                initialPosition = originalSnakeRArm1Position;
                                initialRotation = Quaternion.Euler(originalSnakeRArm1Rotation);
                                break;
                            case 18:
                                targetPosition = faceupGetUpSnakeRArm2Position;
                                targetRotation = Quaternion.Euler(faceupGetUpSnakeRArm2Rotation);
                                initialPosition = originalSnakeRArm2Position;
                                initialRotation = Quaternion.Euler(originalSnakeRArm2Rotation);
                                break;
                            case 19:
                                targetPosition = faceupGetUpSnakeTail1Position;
                                targetRotation = Quaternion.Euler(faceupGetUpSnakeTail1Rotation);
                                initialPosition = originalSnakeTail1Position;
                                initialRotation = Quaternion.Euler(originalSnakeTail1Rotation);
                                break;
                            case 20:
                                targetPosition = faceupGetUpSnakeTail2Position;
                                targetRotation = Quaternion.Euler(faceupGetUpSnakeTail2Rotation);
                                initialPosition = originalSnakeTail2Position;
                                initialRotation = Quaternion.Euler(originalSnakeTail2Rotation);
                                break;
                            case 21:
                                targetPosition = faceupGetUpSnakeTail3Position;
                                targetRotation = Quaternion.Euler(faceupGetUpSnakeTail3Rotation);
                                initialPosition = originalSnakeTail3Position;
                                initialRotation = Quaternion.Euler(originalSnakeTail3Rotation);
                                break;
                            case 22:
                                targetPosition = faceupGetUpSnakeTail4Position;
                                targetRotation = Quaternion.Euler(faceupGetUpSnakeTail4Rotation);
                                initialPosition = originalSnakeTail4Position;
                                initialRotation = Quaternion.Euler(originalSnakeTail4Rotation);
                                break;
                            case 23:
                                targetPosition = faceupGetUpSnakeTail5Position;
                                targetRotation = Quaternion.Euler(faceupGetUpSnakeTail5Rotation);
                                initialPosition = originalSnakeTail5Position;
                                initialRotation = Quaternion.Euler(originalSnakeTail5Rotation);
                                break;
                            case 24:
                                targetPosition = faceupGetUpSnakeTail6Position;
                                targetRotation = Quaternion.Euler(faceupGetUpSnakeTail6Rotation);
                                initialPosition = originalSnakeTail6Position;
                                initialRotation = Quaternion.Euler(originalSnakeTail6Rotation);
                                break;
                            case 25:
                                targetPosition = faceupGetUpSnakeTail7Position;
                                targetRotation = Quaternion.Euler(faceupGetUpSnakeTail7Rotation);
                                initialPosition = originalSnakeTail7Position;
                                initialRotation = Quaternion.Euler(originalSnakeTail7Rotation);
                                break;
                            case 26:
                                targetPosition = faceupGetUpSnakeTail8Position;
                                targetRotation = Quaternion.Euler(faceupGetUpSnakeTail8Rotation);
                                initialPosition = originalSnakeTail8Position;
                                initialRotation = Quaternion.Euler(originalSnakeTail8Rotation);
                                break;
                            case 27:
                                targetPosition = faceupGetUpSnakeTail9Position;
                                targetRotation = Quaternion.Euler(faceupGetUpSnakeTail9Rotation);
                                initialPosition = originalSnakeTail9Position;
                                initialRotation = Quaternion.Euler(originalSnakeTail9Rotation);
                                break;
                            case 28:
                                targetPosition = faceupGetUpSnakeTail10Position;
                                targetRotation = Quaternion.Euler(faceupGetUpSnakeTail10Rotation);
                                initialPosition = originalSnakeTail10Position;
                                initialRotation = Quaternion.Euler(originalSnakeTail10Rotation);
                                break;
                            case 29:
                                targetPosition = faceupGetUpSnakeTail11Position;
                                targetRotation = Quaternion.Euler(faceupGetUpSnakeTail11Rotation);
                                initialPosition = originalSnakeTail11Position;
                                initialRotation = Quaternion.Euler(originalSnakeTail11Rotation);
                                break;
                            default:
                                break;
                        }

                        // Begin the lerp 

                        boneToLerp.localPosition = Vector3.Lerp(initialPosition, targetPosition, currentTimer / targetTimer);
                        boneToLerp.localRotation = Quaternion.Slerp(initialRotation, targetRotation, currentTimer / targetTimer);
                    }
                    break;
                case BoneProfile.Wolf:
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
                                targetPosition = faceupGetUpWolfPelvisPosition;
                                targetRotation = Quaternion.Euler(faceupGetUpWolfPelvisRotation);
                                initialPosition = originalWolfPelvisPosition;
                                initialRotation = Quaternion.Euler(originalWolfPelvisRotation);
                                break;
                            case 1:
                                targetPosition = faceupGetUpWolfBLLeg1Position;
                                targetRotation = Quaternion.Euler(faceupGetUpWolfBLLeg1Rotation);
                                initialPosition = originalWolfBLLeg1Position;
                                initialRotation = Quaternion.Euler(originalWolfBLLeg1Rotation);
                                break;
                            case 2:
                                targetPosition = faceupGetUpWolfBLLeg2Position;
                                targetRotation = Quaternion.Euler(faceupGetUpWolfBLLeg2Rotation);
                                initialPosition = originalWolfBLLeg2Position;
                                initialRotation = Quaternion.Euler(originalWolfBLLeg2Rotation);
                                break;
                            case 3:
                                targetPosition = faceupGetUpWolfBLLeg3Position;
                                targetRotation = Quaternion.Euler(faceupGetUpWolfBLLeg3Rotation);
                                initialPosition = originalWolfBLLeg3Position;
                                initialRotation = Quaternion.Euler(originalWolfBLLeg3Rotation);
                                break;
                            case 4:
                                targetPosition = faceupGetUpWolfBLAnklePosition;
                                targetRotation = Quaternion.Euler(faceupGetUpWolfBLAnkleRotation);
                                initialPosition = originalWolfBLAnklePosition;
                                initialRotation = Quaternion.Euler(originalWolfBLAnkleRotation);
                                break;
                            case 5:
                                targetPosition = faceupGetUpWolfBLFoot1Position;
                                targetRotation = Quaternion.Euler(faceupGetUpWolfBLFoot1Rotation);
                                initialPosition = originalWolfBLFoot1Position;
                                initialRotation = Quaternion.Euler(originalWolfBLFoot1Rotation);
                                break;
                            case 6:
                                targetPosition = faceupGetUpWolfBLFoot2Position;
                                targetRotation = Quaternion.Euler(faceupGetUpWolfBLFoot2Rotation);
                                initialPosition = originalWolfBLFoot2Position;
                                initialRotation = Quaternion.Euler(originalWolfBLFoot2Rotation);
                                break;
                            case 7:
                                targetPosition = faceupGetUpWolfBRLeg1Position;
                                targetRotation = Quaternion.Euler(faceupGetUpWolfBRLeg1Rotation);
                                initialPosition = originalWolfBRLeg1Position;
                                initialRotation = Quaternion.Euler(originalWolfBRLeg1Rotation);
                                break;
                            case 8:
                                targetPosition = faceupGetUpWolfBRLeg2Position;
                                targetRotation = Quaternion.Euler(faceupGetUpWolfBRLeg2Rotation);
                                initialPosition = originalWolfBRLeg2Position;
                                initialRotation = Quaternion.Euler(originalWolfBRLeg2Rotation);
                                break;
                            case 9:
                                targetPosition = faceupGetUpWolfBRLeg3Position;
                                targetRotation = Quaternion.Euler(faceupGetUpWolfBRLeg3Rotation);
                                initialPosition = originalWolfBRLeg3Position;
                                initialRotation = Quaternion.Euler(originalWolfBRLeg3Rotation);
                                break;
                            case 10:
                                targetPosition = faceupGetUpWolfBRAnklePosition;
                                targetRotation = Quaternion.Euler(faceupGetUpWolfBRAnkleRotation);
                                initialPosition = originalWolfBRAnklePosition;
                                initialRotation = Quaternion.Euler(originalWolfBRAnkleRotation);
                                break;
                            case 11:
                                targetPosition = faceupGetUpWolfBRFoot1Position;
                                targetRotation = Quaternion.Euler(faceupGetUpWolfBRFoot1Rotation);
                                initialPosition = originalWolfBRFoot1Position;
                                initialRotation = Quaternion.Euler(originalWolfBRFoot1Rotation);
                                break;
                            case 12:
                                targetPosition = faceupGetUpWolfBRFoot2Position;
                                targetRotation = Quaternion.Euler(faceupGetUpWolfBRFoot2Rotation);
                                initialPosition = originalWolfBRFoot2Position;
                                initialRotation = Quaternion.Euler(originalWolfBRFoot2Rotation);
                                break;
                            case 13:
                                targetPosition = faceupGetUpWolfTail1Position;
                                targetRotation = Quaternion.Euler(faceupGetUpWolfTail1Rotation);
                                initialPosition = originalWolfTail1Position;
                                initialRotation = Quaternion.Euler(originalWolfTail1Rotation);
                                break;
                            case 14:
                                targetPosition = faceupGetUpWolfTail2Position;
                                targetRotation = Quaternion.Euler(faceupGetUpWolfTail2Rotation);
                                initialPosition = originalWolfTail2Position;
                                initialRotation = Quaternion.Euler(originalWolfTail2Rotation);
                                break;
                            case 15:
                                targetPosition = faceupGetUpWolfTail3Position;
                                targetRotation = Quaternion.Euler(faceupGetUpWolfTail3Rotation);
                                initialPosition = originalWolfTail3Position;
                                initialRotation = Quaternion.Euler(originalWolfTail3Rotation);
                                break;
                            case 16:
                                targetPosition = faceupGetUpWolfTail4Position;
                                targetRotation = Quaternion.Euler(faceupGetUpWolfTail4Rotation);
                                initialPosition = originalWolfTail4Position;
                                initialRotation = Quaternion.Euler(originalWolfTail4Rotation);
                                break;
                            case 17:
                                targetPosition = faceupGetUpWolfTail5Position;
                                targetRotation = Quaternion.Euler(faceupGetUpWolfTail5Rotation);
                                initialPosition = originalWolfTail5Position;
                                initialRotation = Quaternion.Euler(originalWolfTail5Rotation);
                                break;
                            case 18:
                                targetPosition = faceupGetUpWolfTail6Position;
                                targetRotation = Quaternion.Euler(faceupGetUpWolfTail6Rotation);
                                initialPosition = originalWolfTail6Position;
                                initialRotation = Quaternion.Euler(originalWolfTail6Rotation);
                                break;
                            case 19:
                                targetPosition = faceupGetUpWolfSpine1Position;
                                targetRotation = Quaternion.Euler(faceupGetUpWolfSpine1Rotation);
                                initialPosition = originalWolfSpine1Position;
                                initialRotation = Quaternion.Euler(originalWolfSpine1Rotation);
                                break;
                            case 20:
                                targetPosition = faceupGetUpWolfSpine2Position;
                                targetRotation = Quaternion.Euler(faceupGetUpWolfSpine2Rotation);
                                initialPosition = originalWolfSpine2Position;
                                initialRotation = Quaternion.Euler(originalWolfSpine2Rotation);
                                break;
                            case 21:
                                targetPosition = faceupGetUpWolfSpine3Position;
                                targetRotation = Quaternion.Euler(faceupGetUpWolfSpine3Rotation);
                                initialPosition = originalWolfSpine3Position;
                                initialRotation = Quaternion.Euler(originalWolfSpine3Rotation);
                                break;
                            case 22:
                                targetPosition = faceupGetUpWolfRibcagePosition;
                                targetRotation = Quaternion.Euler(faceupGetUpWolfRibcageRotation);
                                initialPosition = originalWolfRibcagePosition;
                                initialRotation = Quaternion.Euler(originalWolfRibcageRotation);
                                break;
                            case 23:
                                targetPosition = faceupGetUpWolfNeck1Position;
                                targetRotation = Quaternion.Euler(faceupGetUpWolfNeck1Rotation);
                                initialPosition = originalWolfNeck1Position;
                                initialRotation = Quaternion.Euler(originalWolfNeck1Rotation);
                                break;
                            case 24:
                                targetPosition = faceupGetUpWolfNeck2Position;
                                targetRotation = Quaternion.Euler(faceupGetUpWolfNeck2Rotation);
                                initialPosition = originalWolfNeck2Position;
                                initialRotation = Quaternion.Euler(originalWolfNeck2Rotation);
                                break;
                            case 25:
                                targetPosition = faceupGetUpWolfHeadPosition;
                                targetRotation = Quaternion.Euler(faceupGetUpWolfHeadRotation);
                                initialPosition = originalWolfHeadPosition;
                                initialRotation = Quaternion.Euler(originalWolfHeadRotation);
                                break;
                            case 26:
                                targetPosition = faceupGetUpWolfFLLeg1Position;
                                targetRotation = Quaternion.Euler(faceupGetUpWolfFLLeg1Rotation);
                                initialPosition = originalWolfFLLeg1Position;
                                initialRotation = Quaternion.Euler(originalWolfFLLeg1Rotation);
                                break;
                            case 27:
                                targetPosition = faceupGetUpWolfFLLeg2Position;
                                targetRotation = Quaternion.Euler(faceupGetUpWolfFLLeg2Rotation);
                                initialPosition = originalWolfFLLeg2Position;
                                initialRotation = Quaternion.Euler(originalWolfFLLeg2Rotation);
                                break;
                            case 28:
                                targetPosition = faceupGetUpWolfFLAnklePosition;
                                targetRotation = Quaternion.Euler(faceupGetUpWolfFLAnkleRotation);
                                initialPosition = originalWolfFLAnklePosition;
                                initialRotation = Quaternion.Euler(originalWolfFLAnkleRotation);
                                break;
                            case 29:
                                targetPosition = faceupGetUpWolfFLFoot1Position;
                                targetRotation = Quaternion.Euler(faceupGetUpWolfFLFoot1Rotation);
                                initialPosition = originalWolfFLFoot1Position;
                                initialRotation = Quaternion.Euler(originalWolfFLFoot1Rotation);
                                break;
                            case 30:
                                targetPosition = faceupGetUpWolfFLFoot2Position;
                                targetRotation = Quaternion.Euler(faceupGetUpWolfFLFoot2Rotation);
                                initialPosition = originalWolfFLFoot2Position;
                                initialRotation = Quaternion.Euler(originalWolfFLFoot2Rotation);
                                break;
                            case 31:
                                targetPosition = faceupGetUpWolfFRLeg1Position;
                                targetRotation = Quaternion.Euler(faceupGetUpWolfFRLeg1Rotation);
                                initialPosition = originalWolfFRLeg1Position;
                                initialRotation = Quaternion.Euler(originalWolfFRLeg1Rotation);
                                break;
                            case 32:
                                targetPosition = faceupGetUpWolfFRLeg2Position;
                                targetRotation = Quaternion.Euler(faceupGetUpWolfFRLeg2Rotation);
                                initialPosition = originalWolfFRLeg2Position;
                                initialRotation = Quaternion.Euler(originalWolfFRLeg2Rotation);
                                break;
                            case 33:
                                targetPosition = faceupGetUpWolfFRAnklePosition;
                                targetRotation = Quaternion.Euler(faceupGetUpWolfFRAnkleRotation);
                                initialPosition = originalWolfFRAnklePosition;
                                initialRotation = Quaternion.Euler(originalWolfFRAnkleRotation);
                                break;
                            case 34:
                                targetPosition = faceupGetUpWolfFRFoot1Position;
                                targetRotation = Quaternion.Euler(faceupGetUpWolfFRFoot1Rotation);
                                initialPosition = originalWolfFRFoot1Position;
                                initialRotation = Quaternion.Euler(originalWolfFRFoot1Rotation);
                                break;
                            case 35:
                                targetPosition = faceupGetUpWolfFRFoot2Position;
                                targetRotation = Quaternion.Euler(faceupGetUpWolfFRFoot2Rotation);
                                initialPosition = originalWolfFRFoot2Position;
                                initialRotation = Quaternion.Euler(originalWolfFRFoot2Rotation);
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
            case BoneProfile.Snek:
                for (int index = 0; index < bonesToLerp.Length; index++)
                {
                    Transform boneToMarkDown = bonesToLerp[index];

                    // Ste our target
                    switch (index)
                    {
                        case 0:
                            originalSnakePelvisPosition = boneToMarkDown.localPosition;
                            originalSnakePelvisRotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 1:
                            originalSnakeSpine1Position = boneToMarkDown.localPosition;
                            originalSnakeSpine1Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 2:
                            originalSnakeSpine2Position = boneToMarkDown.localPosition;
                            originalSnakeSpine2Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 3:
                            originalSnakeSpine3Position = boneToMarkDown.localPosition;
                            originalSnakeSpine3Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 4:
                            originalSnakeSpine4Position = boneToMarkDown.localPosition;
                            originalSnakeSpine4Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 5:
                            originalSnakeSpine5Position = boneToMarkDown.localPosition;
                            originalSnakeSpine5Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 6:
                            originalSnakeSpine6Position = boneToMarkDown.localPosition;
                            originalSnakeSpine6Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 7:
                            originalSnakeSpine7Position = boneToMarkDown.localPosition;
                            originalSnakeSpine7Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 8:
                            originalSnakeRibcagePosition = boneToMarkDown.localPosition;
                            originalSnakeRibcageRotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 9:
                            originalSnakeNeck1Position = boneToMarkDown.localPosition;
                            originalSnakeNeck1Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 10:
                            originalSnakeNeck2Position = boneToMarkDown.localPosition;
                            originalSnakeNeck2Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 11:
                            originalSnakeNeck3Position = boneToMarkDown.localPosition;
                            originalSnakeNeck3Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 12:
                            originalSnakeNeck4Position = boneToMarkDown.localPosition;
                            originalSnakeNeck4Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 13:
                            originalSnakeNeck5Position = boneToMarkDown.localPosition;
                            originalSnakeNeck5Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 14:
                            originalSnakeHeadPosition = boneToMarkDown.localPosition;
                            originalSnakeHeadRotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 15:
                            originalSnakeLArm1Position = boneToMarkDown.localPosition;
                            originalSnakeLArm1Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 16:
                            originalSnakeLArm2Position = boneToMarkDown.localPosition;
                            originalSnakeLArm2Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 17:
                            originalSnakeRArm1Position = boneToMarkDown.localPosition;
                            originalSnakeRArm1Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 18:
                            originalSnakeRArm2Position = boneToMarkDown.localPosition;
                            originalSnakeRArm2Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 19:
                            originalSnakeTail1Position = boneToMarkDown.localPosition;
                            originalSnakeTail1Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 20:
                            originalSnakeTail2Position = boneToMarkDown.localPosition;
                            originalSnakeTail2Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 21:
                            originalSnakeTail3Position = boneToMarkDown.localPosition;
                            originalSnakeTail3Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 22:
                            originalSnakeTail4Position = boneToMarkDown.localPosition;
                            originalSnakeTail4Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 23:
                            originalSnakeTail5Position = boneToMarkDown.localPosition;
                            originalSnakeTail5Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 24:
                            originalSnakeTail6Position = boneToMarkDown.localPosition;
                            originalSnakeTail6Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 25:
                            originalSnakeTail7Position = boneToMarkDown.localPosition;
                            originalSnakeTail7Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 26:
                            originalSnakeTail8Position = boneToMarkDown.localPosition;
                            originalSnakeTail8Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 27:
                            originalSnakeTail9Position = boneToMarkDown.localPosition;
                            originalSnakeTail9Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 28:
                            originalSnakeTail10Position = boneToMarkDown.localPosition;
                            originalSnakeTail10Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 29:
                            originalSnakeTail11Position = boneToMarkDown.localPosition;
                            originalSnakeTail11Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                    }
                }
                break;
            case BoneProfile.Wolf:
                for (int index = 0; index < bonesToLerp.Length; index++)
                {
                    Transform boneToMarkDown = bonesToLerp[index];

                    // Ste our target
                    switch (index)
                    {
                        case 0:
                            originalWolfPelvisPosition = boneToMarkDown.localPosition;
                            originalWolfPelvisRotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 1:
                            originalWolfBLLeg1Position = boneToMarkDown.localPosition;
                            originalWolfBLLeg1Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 2:
                            originalWolfBLLeg2Position = boneToMarkDown.localPosition;
                            originalWolfBLLeg2Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 3:
                            originalWolfBLLeg3Position = boneToMarkDown.localPosition;
                            originalWolfBLLeg3Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 4:
                            originalWolfBLAnklePosition = boneToMarkDown.localPosition;
                            originalWolfBLAnkleRotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 5:
                            originalWolfBLFoot1Position = boneToMarkDown.localPosition;
                            originalWolfBLFoot1Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 6:
                            originalWolfBLFoot2Position = boneToMarkDown.localPosition;
                            originalWolfBLFoot2Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 7:
                            originalWolfBRLeg1Position = boneToMarkDown.localPosition;
                            originalWolfBRLeg1Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 8:
                            originalWolfBRLeg2Position = boneToMarkDown.localPosition;
                            originalWolfBRLeg2Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 9:
                            originalWolfBRLeg3Position = boneToMarkDown.localPosition;
                            originalWolfBRLeg3Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 10:
                            originalWolfBRAnklePosition = boneToMarkDown.localPosition;
                            originalWolfBRAnkleRotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 11:
                            originalWolfBRFoot1Position = boneToMarkDown.localPosition;
                            originalWolfBRFoot1Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 12:
                            originalWolfBRFoot2Position = boneToMarkDown.localPosition;
                            originalWolfBRFoot2Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 13:
                            originalWolfTail1Position = boneToMarkDown.localPosition;
                            originalWolfTail1Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 14:
                            originalWolfTail2Position = boneToMarkDown.localPosition;
                            originalWolfTail2Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 15:
                            originalWolfTail3Position = boneToMarkDown.localPosition;
                            originalWolfTail3Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 16:
                            originalWolfTail4Position = boneToMarkDown.localPosition;
                            originalWolfTail4Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 17:
                            originalWolfTail5Position = boneToMarkDown.localPosition;
                            originalWolfTail5Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 18:
                            originalWolfTail6Position = boneToMarkDown.localPosition;
                            originalWolfTail6Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 19:
                            originalWolfSpine1Position = boneToMarkDown.localPosition;
                            originalWolfSpine1Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 20:
                            originalWolfSpine2Position = boneToMarkDown.localPosition;
                            originalWolfSpine2Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 21:
                            originalWolfSpine3Position = boneToMarkDown.localPosition;
                            originalWolfSpine3Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 22:
                            originalWolfRibcagePosition = boneToMarkDown.localPosition;
                            originalWolfRibcageRotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 23:
                            originalWolfNeck1Position = boneToMarkDown.localPosition;
                            originalWolfNeck1Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 24:
                            originalWolfNeck2Position = boneToMarkDown.localPosition;
                            originalWolfNeck2Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 25:
                            originalWolfHeadPosition = boneToMarkDown.localPosition;
                            originalWolfHeadRotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 26:
                            originalWolfFLLeg1Position = boneToMarkDown.localPosition;
                            originalWolfFLLeg1Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 27:
                            originalWolfFLLeg2Position = boneToMarkDown.localPosition;
                            originalWolfFLLeg2Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 28:
                            originalWolfFLAnklePosition = boneToMarkDown.localPosition;
                            originalWolfFLAnkleRotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 29:
                            originalWolfFLFoot1Position = boneToMarkDown.localPosition;
                            originalWolfFLFoot1Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 30:
                            originalWolfFLFoot2Position = boneToMarkDown.localPosition;
                            originalWolfFLFoot2Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 31:
                            originalWolfFRLeg1Position = boneToMarkDown.localPosition;
                            originalWolfFRLeg1Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 32:
                            originalWolfFRLeg2Position = boneToMarkDown.localPosition;
                            originalWolfFRLeg2Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 33:
                            originalWolfFRAnklePosition = boneToMarkDown.localPosition;
                            originalWolfFRAnkleRotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 34:
                            originalWolfFRFoot1Position = boneToMarkDown.localPosition;
                            originalWolfFRFoot1Rotation = boneToMarkDown.localRotation.eulerAngles;
                            break;
                        case 35:
                            originalWolfFRFoot2Position = boneToMarkDown.localPosition;
                            originalWolfFRFoot2Rotation = boneToMarkDown.localRotation.eulerAngles;
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
