using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGearManager : MonoBehaviour
{
    public Transform leftHandWeaponParent;
    public Transform rightHandWeaponParent;

    public Transform torsoParent;
    public Transform rightUpperArmParent;
    public Transform leftUpperArmParent;
    public Transform rightLowerArmParent;
    public Transform leftLowerArmParent;
    public Transform rightHandParent;
    public Transform leftHandParent;
    public Transform rightElbowParent;
    public Transform leftElbowParent;
    public Transform rightShoulderParent;
    public Transform leftShoulderParent;
    
    public Transform headParent;
    public Transform hairStylesParent;
    public Transform facialHairParent;
    public Transform eyeBrowsParent;
    public Transform helmetNoFeaturesParent;
    public Transform helmetNoHairParent;
    public Transform helmetAllFeauresParent;

    public Transform hipParent;
    public Transform rightLegParent;
    public Transform leftLegParent;
    public Transform rightKneeParent;
    public Transform leftKneeParent;

    public Transform ringParent;
    public Transform braceletParent;
    public Transform capeParent;
    public Transform waistParent;
    
    public GameObject[] leftHandWeapons;
    public GameObject[] rightHandWeapons;

    public GameObject[] torsos;
    public GameObject[] rightUpperArms;
    public GameObject[] leftUpperArms;
    public GameObject[] rightLowerArms;
    public GameObject[] leftLowerArms;
    public GameObject[] rightHands;
    public GameObject[] leftHands;
    public GameObject[] rightElbows;
    public GameObject[] leftElbows;
    public GameObject[] rightShoulders;
    public GameObject[] leftShoulders;

    public GameObject[] heads;
    public GameObject[] hairStyles;
    public GameObject[] eyeBrows;
    public GameObject[] facialHairs;
    public GameObject[] helmetAllFeatures;
    public GameObject[] helmetNoHair;
    public GameObject[] helmetNoFeatures;

    public GameObject[] hips;
    public GameObject[] rightLegs;
    public GameObject[] leftLegs;
    public GameObject[] rightKnees;
    public GameObject[] leftKnees;

    public GameObject[] rings;
    public GameObject[] bracelets;
    public GameObject[] capes;
    public GameObject[] waistTrinkets;

    public enum MaterialOverrideCode { Invisible, GiantStrength}
    public List<Material> materials = new List<Material>();

    public class MaterialOverride
    {
        public float priority = 0;
        public Material mat = null;
        public MaterialOverrideCode matCode;
    }
    public List<MaterialOverride> matOverrides = new List<MaterialOverride>();
    private Material currentMaterialOverride = null;

    private BuffsManager weaponBuffs;

    private Material[] previousMaterial_RightWeapon;
    private Material[] previousMaterial_LeftWeapon;

    private Material[] previousMaterial_Torso;
    private Material[] previousMaterial_RightUpper;
    private Material[] previousMaterial_LeftUpper;
    private Material[] previousMaterial_RightLower;
    private Material[] previousMaterial_LeftLower;
    private Material[] previousMaterial_RightHand;
    private Material[] previousMaterial_LeftHand;
    private Material[] previousMaterial_RightElbow;
    private Material[] previousMaterial_LeftElbow;
    private Material[] previousMaterial_RightShoulder;
    private Material[] previousMaterial_LeftShoulder;

    private Material[] previousMaterial_Head;
    private Material[] previousMaterial_Hair;
    private Material[] previousMaterial_Eyebrows;
    private Material[] previousMaterial_FacialHair;
    private Material[] previousMaterial_HelmetAllFeatures;
    private Material[] previousMaterial_HelmetNoHair;
    private Material[] previousMaterial_HelmetNoFeatures;

    private Material[] previousMaterial_Hips;
    private Material[] previousMaterial_RightLegs;
    private Material[] previousMaterial_LeftLegs;
    private Material[] previousMaterial_RightKnee;
    private Material[] previousMaterial_LeftKnee;

    private Material[] previousMaterial_Rings;
    private Material[] previousMaterial_Bracelet;
    private Material[] previousMaterial_Capes;
    private Material[] previousMaterial_WaistTrinkets;

    int headStartingIndex;
    int hairStyleStartingIndex;
    int eyeBrowsStartingIndex;
    int facialHairsStartingIndex;

    private void Start()
    {
        weaponBuffs = GetComponent<BuffsManager>();

        leftHandWeapons = new GameObject[leftHandWeaponParent.childCount];
        rightHandWeapons = new GameObject[rightHandWeaponParent.childCount];

        torsos = new GameObject[torsoParent.childCount];
        rightUpperArms = new GameObject[rightUpperArmParent.childCount];
        leftUpperArms = new GameObject[leftUpperArmParent.childCount];
        rightLowerArms = new GameObject[rightLowerArmParent.childCount];
        leftLowerArms = new GameObject[leftLowerArmParent.childCount];
        rightHands = new GameObject[rightHandParent.childCount];
        leftHands = new GameObject[leftHandParent.childCount];
        rightElbows = new GameObject[rightElbowParent.childCount];
        leftElbows = new GameObject[leftElbowParent.childCount];
        rightShoulders = new GameObject[rightShoulderParent.childCount];
        leftShoulders = new GameObject[leftShoulderParent.childCount];

        hairStyles = new GameObject[hairStylesParent.childCount];
        eyeBrows = new GameObject[eyeBrowsParent.childCount];
        heads = new GameObject[headParent.childCount];
        facialHairs = new GameObject[facialHairParent.childCount];
        helmetNoFeatures = new GameObject[helmetNoFeaturesParent.childCount];
        helmetNoHair = new GameObject[helmetNoHairParent.childCount];
        helmetAllFeatures = new GameObject[helmetAllFeauresParent.childCount];

        hips = new GameObject[hipParent.childCount];
        rightLegs = new GameObject[rightLegParent.childCount];
        leftLegs = new GameObject[leftLegParent.childCount];
        rightKnees = new GameObject[rightKneeParent.childCount];
        leftKnees = new GameObject[leftKneeParent.childCount];

        rings = new GameObject[ringParent.childCount];
        bracelets = new GameObject[braceletParent.childCount];
        capes = new GameObject[capeParent.childCount];
        waistTrinkets = new GameObject[waistParent.childCount];

        // grab all the weapons for setting and hiding purposes.
        for (int index = 0; index < leftHandWeaponParent.childCount; index++)
            leftHandWeapons[index] = leftHandWeaponParent.GetChild(index).gameObject;
        for (int index = 0; index < rightHandWeaponParent.childCount; index++)
            rightHandWeapons[index] = rightHandWeaponParent.GetChild(index).gameObject;

        // grab all the armors for setting and hiding purposes
        for (int index = 0; index < torsoParent.childCount; index++)
            torsos[index] = torsoParent.GetChild(index).gameObject;
        for (int index = 0; index < rightUpperArmParent.childCount; index++)
            rightUpperArms[index] = rightUpperArmParent.GetChild(index).gameObject;
        for (int index = 0; index < leftUpperArmParent.childCount; index++)
            leftUpperArms[index] = leftUpperArmParent.GetChild(index).gameObject;
        for (int index = 0; index < rightLowerArmParent.childCount; index++)
            rightLowerArms[index] = rightLowerArmParent.GetChild(index).gameObject;
        for (int index = 0; index < leftLowerArmParent.childCount; index++)
            leftLowerArms[index] = leftLowerArmParent.GetChild(index).gameObject;
        for (int index = 0; index < rightHandParent.childCount; index++)
            rightHands[index] = rightHandParent.GetChild(index).gameObject;
        for (int index = 0; index < leftHandParent.childCount; index++)
            leftHands[index] = leftHandParent.GetChild(index).gameObject;
        for (int index = 0; index < rightElbowParent.childCount; index++)
            rightElbows[index] = rightElbowParent.GetChild(index).gameObject;
        for (int index = 0; index < leftElbowParent.childCount; index++)
            leftElbows[index] = leftElbowParent.GetChild(index).gameObject;
        for (int index = 0; index < rightShoulderParent.childCount; index++)
            rightShoulders[index] = rightShoulderParent.GetChild(index).gameObject;
        for (int index = 0; index < leftShoulderParent.childCount; index++)
            leftShoulders[index] = leftShoulderParent.GetChild(index).gameObject;

        // grab the faces, eyebrows, haiurs, and helmets for setting and hiding purposes.
        for (int index = 0; index < hairStylesParent.childCount; index++)
            hairStyles[index] = hairStylesParent.GetChild(index).gameObject;
        for (int index = 0; index < eyeBrowsParent.childCount; index++)
            eyeBrows[index] = eyeBrowsParent.GetChild(index).gameObject;
        for (int index = 0; index < headParent.childCount; index++)
            heads[index] = headParent.GetChild(index).gameObject;
        for (int index = 0; index < facialHairParent.childCount; index++)
            facialHairs[index] = facialHairParent.GetChild(index).gameObject;
        for (int index = 0; index < helmetNoFeaturesParent.childCount; index++)
            helmetNoFeatures[index] = helmetNoFeaturesParent.GetChild(index).gameObject;
        for (int index = 0; index < helmetNoHairParent.childCount; index++)
            helmetNoHair[index] = helmetNoHairParent.GetChild(index).gameObject;
        for (int index = 0; index < helmetAllFeauresParent.childCount; index++)
            helmetAllFeatures[index] = helmetAllFeauresParent.GetChild(index).gameObject;

        // Grab all the legs, hips and knee armor availible.
        for (int index = 0; index < hipParent.childCount; index++)
            hips[index] = hipParent.GetChild(index).gameObject;
        for (int index = 0; index < rightLegParent.childCount; index++)
            rightLegs[index] = rightLegParent.GetChild(index).gameObject;
        for (int index = 0; index < leftLegParent.childCount; index++)
            leftLegs[index] = leftLegParent.GetChild(index).gameObject;
        for (int index = 0; index < rightKneeParent.childCount; index++)
            rightKnees[index] = rightKneeParent.GetChild(index).gameObject;
        for (int index = 0; index < leftKneeParent.childCount; index++)
            leftKnees[index] = leftKneeParent.GetChild(index).gameObject;

        //Grab all the rings and other trinkets for setting and hiding purposes.
        for (int index = 0; index < ringParent.childCount; index++)
            rings[index] = ringParent.GetChild(index).gameObject;
        for (int index = 0; index < braceletParent.childCount; index++)
            bracelets[index] = braceletParent.GetChild(index).gameObject;
        for (int index = 0; index < capeParent.childCount; index++)
            capes[index] = capeParent.GetChild(index).gameObject;
        for (int index = 0; index < waistParent.childCount; index++)
            waistTrinkets[index] = waistParent.GetChild(index).gameObject;

        HideAllItems();

    }

    // Used to hide all the weapons, or a specific one at an index.
    public void HideAllItems()
    { 
        foreach (GameObject weapon in rightHandWeapons)
            weapon.SetActive(false);
        weaponBuffs.UpdateWeaponEffectsRight(Vector3.zero, Vector3.zero, Quaternion.identity, null);

        foreach (GameObject weapon in leftHandWeapons)
            weapon.SetActive(false);
        weaponBuffs.UpdateWeaponEffectsLeft(Vector3.zero, Vector3.zero, Quaternion.identity, null);

        foreach (GameObject armor in torsos)
            armor.SetActive(false);
        foreach (GameObject armor in rightUpperArms)
            armor.SetActive(false);
        foreach (GameObject armor in leftUpperArms)
            armor.SetActive(false);
        foreach (GameObject armor in rightLowerArms)
            armor.SetActive(false);
        foreach (GameObject armor in leftLowerArms)
            armor.SetActive(false);
        foreach (GameObject armor in rightHands)
            armor.SetActive(false);
        foreach (GameObject armor in leftHands)
            armor.SetActive(false);
        foreach (GameObject armor in rightElbows)
            armor.SetActive(false);
        foreach (GameObject armor in leftElbows)
            armor.SetActive(false);
        foreach (GameObject armor in rightShoulders)
            armor.SetActive(false);
        foreach (GameObject armor in leftShoulders)
            armor.SetActive(false);
        
        foreach (GameObject helmet in helmetAllFeatures)
            helmet.SetActive(false);
        foreach (GameObject helmet in helmetNoFeatures)
            helmet.SetActive(false);
        foreach (GameObject helmet in helmetNoHair)
            helmet.SetActive(false);
        for (int index = 0; index < facialHairParent.childCount; index++)
            if (facialHairParent.GetChild(index).gameObject.activeSelf)
                facialHairsStartingIndex = index;
        for (int index = 0; index < headParent.childCount; index++)
            if (headParent.GetChild(index).gameObject.activeSelf)
                headStartingIndex = index;
        for (int index = 0; index < eyeBrowsParent.childCount; index++)
            if (eyeBrowsParent.GetChild(index).gameObject.activeSelf)
                eyeBrowsStartingIndex = index;
        for (int index = 0; index < hairStylesParent.childCount; index++)
            if (hairStylesParent.GetChild(index).gameObject.activeSelf)
                hairStyleStartingIndex = index;

        foreach (GameObject armor in hips)
            armor.SetActive(false);
        foreach (GameObject armor in rightLegs)
            armor.SetActive(false);
        foreach (GameObject armor in leftLegs)
            armor.SetActive(false);
        foreach (GameObject armor in rightKnees)
            armor.SetActive(false);
        foreach (GameObject armor in leftKnees)
            armor.SetActive(false);

        foreach (GameObject ring in rings)
            ring.SetActive(false);
        foreach (GameObject bracelet in bracelets)
            bracelet.SetActive(false);
        foreach (GameObject cape in capes)
            cape.SetActive(false);
        foreach (GameObject trinket in waistTrinkets)
            trinket.SetActive(false);

        // Here I show the base items, the one we decide are the "base values".
        torsos[0].SetActive(true);
        previousMaterial_Torso = torsos[0].GetComponent<SkinnedMeshRenderer>().materials;

        leftUpperArms[0].SetActive(true);
        previousMaterial_LeftUpper = leftUpperArms[0].GetComponent<SkinnedMeshRenderer>().materials;
        rightUpperArms[0].SetActive(true);
        previousMaterial_RightUpper = rightUpperArms[0].GetComponent<SkinnedMeshRenderer>().materials;

        leftLowerArms[0].SetActive(true);
        previousMaterial_LeftLower = leftLowerArms[0].GetComponent<SkinnedMeshRenderer>().materials;
        rightLowerArms[0].SetActive(true);
        previousMaterial_RightLower = rightLowerArms[0].GetComponent<SkinnedMeshRenderer>().materials;

        leftHands[0].SetActive(true);
        previousMaterial_LeftHand = leftHands[0].GetComponent<SkinnedMeshRenderer>().materials;
        rightHands[0].SetActive(true);
        previousMaterial_RightHand = rightHands[0].GetComponent<SkinnedMeshRenderer>().materials;

        facialHairs[facialHairsStartingIndex].SetActive(true);
        previousMaterial_FacialHair = facialHairs[facialHairsStartingIndex].GetComponent<SkinnedMeshRenderer>().materials;
        eyeBrows[eyeBrowsStartingIndex].SetActive(true);
        previousMaterial_Eyebrows = eyeBrows[eyeBrowsStartingIndex].GetComponent<SkinnedMeshRenderer>().materials;
        heads[headStartingIndex].SetActive(true);
        previousMaterial_Head = heads[headStartingIndex].GetComponent<SkinnedMeshRenderer>().materials;
        hairStyles[hairStyleStartingIndex].SetActive(true);
        previousMaterial_Hair = hairStyles[hairStyleStartingIndex].GetComponent<SkinnedMeshRenderer>().materials;

        hips[0].SetActive(true);
        previousMaterial_Hips = hips[0].GetComponent<SkinnedMeshRenderer>().materials;
        rightLegs[0].SetActive(true);
        previousMaterial_RightLegs = rightLegs[0].GetComponent<SkinnedMeshRenderer>().materials;
        leftLegs[0].SetActive(true);
        previousMaterial_LeftLegs = leftLegs[0].GetComponent<SkinnedMeshRenderer>().materials;

        rightKnees[0].SetActive(true);
        leftKnees[0].SetActive(true);
    }
    public void HideItem(Item item)
    {
        //Debug.Log($"Hiding the item {item.itemName}");
        switch (item.itemType)
        {
            case Item.ItemType.TrinketCape:
                capes[item.itemGearID].SetActive(false);
                if(capes[item.itemGearID].GetComponent<SkinnedMeshRenderer>())
                    capes[item.itemGearID].GetComponent<SkinnedMeshRenderer>().materials = previousMaterial_Capes;
                else
                    capes[item.itemGearID].GetComponent<MeshRenderer>().materials = previousMaterial_Capes;
                break;
            case Item.ItemType.TrinketRing:
                rings[item.itemGearID].SetActive(false);
                rings[item.itemGearID].GetComponent<MeshRenderer>().materials = previousMaterial_Rings;
                break;
            case Item.ItemType.TrinketBracelet:
                bracelets[item.itemGearID].SetActive(false);
                bracelets[item.itemGearID].GetComponent<MeshRenderer>().materials = previousMaterial_Bracelet;
                break;
            case Item.ItemType.TrinketWaistItem:
                waistTrinkets[item.itemGearID].SetActive(false);
                waistTrinkets[item.itemGearID].GetComponent<SkinnedMeshRenderer>().materials = previousMaterial_WaistTrinkets;
                break;
            case Item.ItemType.Weapon:
                if (item.equippedToRightHand)
                {
                    rightHandWeapons[item.itemGearID].SetActive(false);
                    rightHandWeapons[item.itemGearID].GetComponent<MeshRenderer>().materials = previousMaterial_RightWeapon;
                    weaponBuffs.UpdateWeaponEffectsRight(Vector3.zero, Vector3.zero, Quaternion.identity, null);
                }
                else
                {
                    leftHandWeapons[item.itemGearID].SetActive(false);
                    leftHandWeapons[item.itemGearID].GetComponent<MeshRenderer>().materials = previousMaterial_LeftWeapon;
                    weaponBuffs.UpdateWeaponEffectsLeft(Vector3.zero, Vector3.zero, Quaternion.identity, null);
                }
                break;
            case Item.ItemType.Shield:
                if (item.equippedToRightHand)
                {
                    rightHandWeapons[item.itemGearID].SetActive(false);
                    rightHandWeapons[item.itemGearID].GetComponent<MeshRenderer>().materials = previousMaterial_RightWeapon;
                    weaponBuffs.UpdateWeaponEffectsRight(Vector3.zero, Vector3.zero, Quaternion.identity, null);
                }
                else
                {
                    leftHandWeapons[item.itemGearID].SetActive(false);
                    leftHandWeapons[item.itemGearID].GetComponent<MeshRenderer>().materials = previousMaterial_LeftWeapon;
                    weaponBuffs.UpdateWeaponEffectsLeft(Vector3.zero, Vector3.zero, Quaternion.identity, null);
                }
                break;
            case Item.ItemType.MagicBooster:
                if (item.equippedToRightHand)
                {
                    rightHandWeapons[item.itemGearID].SetActive(false);
                    rightHandWeapons[item.itemGearID].GetComponent<MeshRenderer>().materials = previousMaterial_RightWeapon;
                    weaponBuffs.UpdateWeaponEffectsRight(Vector3.zero, Vector3.zero, Quaternion.identity, null);
                }
                else
                {
                    leftHandWeapons[item.itemGearID].SetActive(false);
                    leftHandWeapons[item.itemGearID].GetComponent<MeshRenderer>().materials = previousMaterial_LeftWeapon;
                    weaponBuffs.UpdateWeaponEffectsLeft(Vector3.zero, Vector3.zero, Quaternion.identity, null);
                }
                break;
            case Item.ItemType.TwoHandWeapon:
                rightHandWeapons[item.itemGearID].SetActive(false);
                rightHandWeapons[item.itemGearID].GetComponent<MeshRenderer>().materials = previousMaterial_RightWeapon;
                weaponBuffs.UpdateWeaponEffectsRight(Vector3.zero, Vector3.zero, Quaternion.identity, null);
                break;
            case Item.ItemType.Helmet:
                switch (item.helmetType)
                {
                    case Item.HelmetType.NoFeatures:
                        helmetNoFeatures[item.itemGearID].SetActive(false);
                        helmetNoFeatures[item.itemGearID].GetComponent<SkinnedMeshRenderer>().materials = previousMaterial_HelmetNoFeatures;
                        break;
                    case Item.HelmetType.NoHair:
                        helmetNoHair[item.itemGearID].SetActive(false);
                        helmetNoHair[item.itemGearID].GetComponent<SkinnedMeshRenderer>().materials = previousMaterial_HelmetNoHair;
                        break;
                    case Item.HelmetType.AllFeatures:
                        helmetAllFeatures[item.itemGearID].SetActive(false);
                        helmetAllFeatures[item.itemGearID].GetComponent<SkinnedMeshRenderer>().materials = previousMaterial_HelmetAllFeatures;
                        break;
                    default:
                        break;
                }
                heads[headStartingIndex].SetActive(true);
                eyeBrows[eyeBrowsStartingIndex].SetActive(true);
                hairStyles[hairStyleStartingIndex].SetActive(true);
                facialHairs[facialHairsStartingIndex].SetActive(true);
                break;
            case Item.ItemType.Legs:
                //Debug.Log("we are hiding the leggings");
                hips[item.itemtorsoID].SetActive(false);
                hips[item.itemtorsoID].GetComponent<SkinnedMeshRenderer>().materials = previousMaterial_Hips;
                rightLegs[item.itemLowerRightArmID].SetActive(false);
                rightLegs[item.itemLowerRightArmID].GetComponent<SkinnedMeshRenderer>().materials = previousMaterial_RightLegs;
                leftLegs[item.itemLowerLeftArmID].SetActive(false);
                leftLegs[item.itemLowerLeftArmID].GetComponent<SkinnedMeshRenderer>().materials = previousMaterial_LeftLegs;

                rightKnees[item.itemRightElbowID].SetActive(false);
                if(rightKnees[item.itemRightElbowID].GetComponent<SkinnedMeshRenderer>() != null)
                    rightKnees[item.itemRightElbowID].GetComponent<SkinnedMeshRenderer>().materials = previousMaterial_RightKnee;

                leftKnees[item.itemLeftElbowID].SetActive(false);
                if (leftKnees[item.itemLeftElbowID].GetComponent<SkinnedMeshRenderer>() != null)
                    leftKnees[item.itemLeftElbowID].GetComponent<SkinnedMeshRenderer>().materials = previousMaterial_LeftKnee;

                hips[0].SetActive(true);
                previousMaterial_Hips = hips[0].GetComponent<SkinnedMeshRenderer>().materials;
                rightLegs[0].SetActive(true);
                previousMaterial_RightLegs = rightLegs[0].GetComponent<SkinnedMeshRenderer>().materials;
                leftLegs[0].SetActive(true);
                previousMaterial_LeftLegs = leftLegs[0].GetComponent<SkinnedMeshRenderer>().materials;
                rightKnees[0].SetActive(true);
                previousMaterial_RightKnee = null;
                leftKnees[0].SetActive(true);
                previousMaterial_LeftKnee = null;
                break;
            case Item.ItemType.Armor:
                //Debug.Log("we are hiding the armor");
                torsos[item.itemtorsoID].SetActive(false);
                torsos[item.itemtorsoID].GetComponent<SkinnedMeshRenderer>().materials = previousMaterial_Torso;
                rightUpperArms[item.itemUpperRightArmID].SetActive(false);
                rightUpperArms[item.itemUpperRightArmID].GetComponent<SkinnedMeshRenderer>().materials = previousMaterial_RightUpper;
                leftUpperArms[item.itemUpperLeftArmID].SetActive(false);
                leftUpperArms[item.itemUpperLeftArmID].GetComponent<SkinnedMeshRenderer>().materials = previousMaterial_LeftUpper;
                rightLowerArms[item.itemLowerRightArmID].SetActive(false);
                rightLowerArms[item.itemLowerRightArmID].GetComponent<SkinnedMeshRenderer>().materials = previousMaterial_RightLower;
                leftLowerArms[item.itemLowerLeftArmID].SetActive(false);
                leftLowerArms[item.itemLowerLeftArmID].GetComponent<SkinnedMeshRenderer>().materials = previousMaterial_LeftLower;
                rightHands[item.itemRightHandID].SetActive(false);
                rightHands[item.itemRightHandID].GetComponent<SkinnedMeshRenderer>().materials = previousMaterial_RightHand;
                leftHands[item.itemLeftHandID].SetActive(false);
                leftHands[item.itemLeftHandID].GetComponent<SkinnedMeshRenderer>().materials = previousMaterial_LeftHand;
                rightElbows[item.itemRightElbowID].SetActive(false);
                if (rightElbows[item.itemRightElbowID].GetComponent<SkinnedMeshRenderer>() != null)
                    rightElbows[item.itemRightElbowID].GetComponent<SkinnedMeshRenderer>().materials = previousMaterial_RightElbow;
                leftElbows[item.itemLeftElbowID].SetActive(false);
                if (leftElbows[item.itemLeftElbowID].GetComponent<SkinnedMeshRenderer>() != null)
                    leftElbows[item.itemLeftElbowID].GetComponent<SkinnedMeshRenderer>().materials = previousMaterial_LeftElbow;
                rightShoulders[item.itemRightShoulderID].SetActive(false);
                if (rightShoulders[item.itemRightShoulderID].GetComponent<SkinnedMeshRenderer>() != null)
                    rightShoulders[item.itemRightShoulderID].GetComponent<SkinnedMeshRenderer>().materials = previousMaterial_RightShoulder;
                leftShoulders[item.itemLeftShoulderID].SetActive(false);
                if (leftShoulders[item.itemLeftShoulderID].GetComponent<SkinnedMeshRenderer>() != null)
                    leftShoulders[item.itemLeftShoulderID].GetComponent<SkinnedMeshRenderer>().materials = previousMaterial_LeftShoulder;

                torsos[0].SetActive(true);
                previousMaterial_Torso = torsos[0].GetComponent<SkinnedMeshRenderer>().materials;
                leftUpperArms[0].SetActive(true);
                previousMaterial_LeftUpper = leftUpperArms[0].GetComponent<SkinnedMeshRenderer>().materials;
                rightUpperArms[0].SetActive(true);
                previousMaterial_RightUpper = rightUpperArms[0].GetComponent<SkinnedMeshRenderer>().materials;
                leftLowerArms[0].SetActive(true);
                previousMaterial_LeftLower = leftLowerArms[0].GetComponent<SkinnedMeshRenderer>().materials;
                rightLowerArms[0].SetActive(true);
                previousMaterial_RightLower = rightLowerArms[0].GetComponent<SkinnedMeshRenderer>().materials;
                leftHands[0].SetActive(true);
                previousMaterial_LeftHand = leftHands[0].GetComponent<SkinnedMeshRenderer>().materials;
                rightHands[0].SetActive(true);
                previousMaterial_RightHand = rightHands[0].GetComponent<SkinnedMeshRenderer>().materials;
                break;
            default:
                break;
        }
    }

    // Used to show the specific weapon in question based on that weapons index.
    public void ShowItem(Item item)
    {
        //Debug.Log($"Shwoing the item {item.itemName}");
        switch (item.itemType)
        {
            case Item.ItemType.TrinketCape:
                capes[item.itemGearID].SetActive(true);
                if (capes[item.itemGearID].GetComponent<SkinnedMeshRenderer>())
                    previousMaterial_Capes = capes[item.itemGearID].GetComponent<SkinnedMeshRenderer>().materials;
                else
                    previousMaterial_Capes = capes[item.itemGearID].GetComponent<MeshRenderer>().materials;

                if(currentMaterialOverride != null)
                {
                    if (capes[item.itemGearID].GetComponent<SkinnedMeshRenderer>())
                        ChangeGameobjectsMaterialArray(capes[item.itemGearID], currentMaterialOverride);
                    else
                        ChangeGameobjectsMaterialArrayNotSkinned(capes[item.itemGearID], currentMaterialOverride);
                }
                break;
            case Item.ItemType.TrinketRing:
                rings[item.itemGearID].SetActive(true);
                previousMaterial_Rings = rings[item.itemGearID].GetComponent<MeshRenderer>().materials;
                if (currentMaterialOverride != null)
                    ChangeGameobjectsMaterialArrayNotSkinned(rings[item.itemGearID], currentMaterialOverride);
                break;
            case Item.ItemType.TrinketBracelet:
                bracelets[item.itemGearID].SetActive(true);
                previousMaterial_Bracelet = bracelets[item.itemGearID].GetComponent<MeshRenderer>().materials;
                if (currentMaterialOverride != null)
                    ChangeGameobjectsMaterialArrayNotSkinned(bracelets[item.itemGearID], currentMaterialOverride);
                break;
            case Item.ItemType.TrinketWaistItem:
                waistTrinkets[item.itemGearID].SetActive(true);
                previousMaterial_WaistTrinkets = waistTrinkets[item.itemGearID].GetComponent<SkinnedMeshRenderer>().materials;
                if (currentMaterialOverride != null)
                    ChangeGameobjectsMaterialArray(waistTrinkets[item.itemGearID], currentMaterialOverride);
                break;
            case Item.ItemType.Weapon:
                if (item.equippedToRightHand)
                {
                    rightHandWeapons[item.itemGearID].SetActive(true);
                    if(rightHandWeapons[item.itemGearID].GetComponent<MeshRenderer>() != null)
                        previousMaterial_RightWeapon = rightHandWeapons[item.itemGearID].GetComponent<MeshRenderer>().materials;
                    weaponBuffs.UpdateWeaponEffectsRight(rightHandWeapons[item.itemGearID].transform.position, rightHandWeapons[item.itemGearID].transform.localScale, rightHandWeapons[item.itemGearID].transform.rotation, rightHandWeapons[item.itemGearID].GetComponent<MeshFilter>().mesh);

                    if (currentMaterialOverride != null)
                        ChangeGameobjectsMaterialArrayNotSkinned(rightHandWeapons[item.itemGearID], currentMaterialOverride);
                }
                else
                {
                    leftHandWeapons[item.itemGearID].SetActive(true);
                    if (leftHandWeapons[item.itemGearID].GetComponent<MeshRenderer>() != null)
                        previousMaterial_LeftWeapon = leftHandWeapons[item.itemGearID].GetComponent<MeshRenderer>().materials;
                    weaponBuffs.UpdateWeaponEffectsLeft(leftHandWeapons[item.itemGearID].transform.position, leftHandWeapons[item.itemGearID].transform.localScale, leftHandWeapons[item.itemGearID].transform.rotation, leftHandWeapons[item.itemGearID].GetComponent<MeshFilter>().mesh);

                    if (currentMaterialOverride != null)
                        ChangeGameobjectsMaterialArrayNotSkinned(leftHandWeapons[item.itemGearID], currentMaterialOverride);
                }
                break;
            case Item.ItemType.Shield:
                if (item.equippedToRightHand)
                {
                    rightHandWeapons[item.itemGearID].SetActive(true);
                    if (rightHandWeapons[item.itemGearID].GetComponent<MeshRenderer>() != null)
                        previousMaterial_RightWeapon = rightHandWeapons[item.itemGearID].GetComponent<MeshRenderer>().materials;
                    weaponBuffs.UpdateWeaponEffectsRight(rightHandWeapons[item.itemGearID].transform.position, rightHandWeapons[item.itemGearID].transform.localScale, rightHandWeapons[item.itemGearID].transform.rotation, rightHandWeapons[item.itemGearID].GetComponent<MeshFilter>().mesh);

                    if (currentMaterialOverride != null)
                        ChangeGameobjectsMaterialArrayNotSkinned(rightHandWeapons[item.itemGearID], currentMaterialOverride);
                }
                else
                {
                    leftHandWeapons[item.itemGearID].SetActive(true);
                    if (leftHandWeapons[item.itemGearID].GetComponent<MeshRenderer>() != null)
                        previousMaterial_LeftWeapon = leftHandWeapons[item.itemGearID].GetComponent<MeshRenderer>().materials;
                    weaponBuffs.UpdateWeaponEffectsLeft(leftHandWeapons[item.itemGearID].transform.position, leftHandWeapons[item.itemGearID].transform.localScale, leftHandWeapons[item.itemGearID].transform.rotation, leftHandWeapons[item.itemGearID].GetComponent<MeshFilter>().mesh);

                    if (currentMaterialOverride != null)
                        ChangeGameobjectsMaterialArrayNotSkinned(leftHandWeapons[item.itemGearID], currentMaterialOverride);
                }
                break;
            case Item.ItemType.MagicBooster:
                if (item.equippedToRightHand)
                {
                    rightHandWeapons[item.itemGearID].SetActive(true);
                    if (rightHandWeapons[item.itemGearID].GetComponent<MeshRenderer>() != null)
                        previousMaterial_RightWeapon = rightHandWeapons[item.itemGearID].GetComponent<MeshRenderer>().materials;
                    weaponBuffs.UpdateWeaponEffectsRight(rightHandWeapons[item.itemGearID].transform.position, rightHandWeapons[item.itemGearID].transform.localScale, rightHandWeapons[item.itemGearID].transform.rotation, rightHandWeapons[item.itemGearID].GetComponent<MeshFilter>().mesh);

                    if (currentMaterialOverride != null)
                        ChangeGameobjectsMaterialArrayNotSkinned(rightHandWeapons[item.itemGearID], currentMaterialOverride);
                }
                else
                {
                    leftHandWeapons[item.itemGearID].SetActive(true);
                    if (leftHandWeapons[item.itemGearID].GetComponent<MeshRenderer>() != null)
                        previousMaterial_LeftWeapon = leftHandWeapons[item.itemGearID].GetComponent<MeshRenderer>().materials;
                    weaponBuffs.UpdateWeaponEffectsLeft(leftHandWeapons[item.itemGearID].transform.position, leftHandWeapons[item.itemGearID].transform.localScale, leftHandWeapons[item.itemGearID].transform.rotation, leftHandWeapons[item.itemGearID].GetComponent<MeshFilter>().mesh);

                    if (currentMaterialOverride != null)
                        ChangeGameobjectsMaterialArrayNotSkinned(leftHandWeapons[item.itemGearID], currentMaterialOverride);
                }
                break;
            case Item.ItemType.TwoHandWeapon:
                rightHandWeapons[item.itemGearID].SetActive(true);
                if (rightHandWeapons[item.itemGearID].GetComponent<MeshRenderer>() != null)
                    previousMaterial_RightWeapon = rightHandWeapons[item.itemGearID].GetComponent<MeshRenderer>().materials;
                weaponBuffs.UpdateWeaponEffectsRight(rightHandWeapons[item.itemGearID].transform.position, rightHandWeapons[item.itemGearID].transform.localScale, rightHandWeapons[item.itemGearID].transform.rotation, rightHandWeapons[item.itemGearID].GetComponent<MeshFilter>().mesh);

                if (currentMaterialOverride != null)
                    ChangeGameobjectsMaterialArrayNotSkinned(rightHandWeapons[item.itemGearID], currentMaterialOverride);
                break;
            case Item.ItemType.Helmet:
                switch (item.helmetType)
                {
                    case Item.HelmetType.NoFeatures:
                        heads[headStartingIndex].SetActive(false);
                        eyeBrows[eyeBrowsStartingIndex].SetActive(false);
                        hairStyles[hairStyleStartingIndex].SetActive(false);
                        facialHairs[facialHairsStartingIndex].SetActive(false);

                        helmetNoFeatures[item.itemGearID].SetActive(true);
                        previousMaterial_HelmetNoFeatures = helmetNoFeatures[item.itemGearID].GetComponent<SkinnedMeshRenderer>().materials;

                        if (currentMaterialOverride != null)
                            ChangeGameobjectsMaterialArray(helmetNoFeatures[item.itemGearID], currentMaterialOverride);
                        break;
                    case Item.HelmetType.NoHair:
                        heads[headStartingIndex].SetActive(true);
                        eyeBrows[eyeBrowsStartingIndex].SetActive(true);
                        hairStyles[hairStyleStartingIndex].SetActive(false);
                        facialHairs[facialHairsStartingIndex].SetActive(true);

                        helmetNoHair[item.itemGearID].SetActive(true);
                        previousMaterial_HelmetNoHair = helmetNoHair[item.itemGearID].GetComponent<SkinnedMeshRenderer>().materials;

                        if (currentMaterialOverride != null)
                        {
                            ChangeGameobjectsMaterialArray(helmetNoHair[item.itemGearID], currentMaterialOverride);

                            ChangeGameobjectsMaterialArray(eyeBrows[eyeBrowsStartingIndex], currentMaterialOverride);
                            ChangeGameobjectsMaterialArray(heads[headStartingIndex], currentMaterialOverride);
                            ChangeGameobjectsMaterialArray(facialHairs[facialHairsStartingIndex], currentMaterialOverride);
                        }
                        break;
                    case Item.HelmetType.AllFeatures:
                        heads[headStartingIndex].SetActive(true);
                        eyeBrows[eyeBrowsStartingIndex].SetActive(true);
                        hairStyles[hairStyleStartingIndex].SetActive(true);
                        facialHairs[facialHairsStartingIndex].SetActive(true);

                        helmetAllFeatures[item.itemGearID].SetActive(true);
                        previousMaterial_HelmetAllFeatures = helmetAllFeatures[item.itemGearID].GetComponent<SkinnedMeshRenderer>().materials;

                        if (currentMaterialOverride != null)
                        {
                            ChangeGameobjectsMaterialArray(helmetAllFeatures[item.itemGearID], currentMaterialOverride);

                            ChangeGameobjectsMaterialArray(eyeBrows[eyeBrowsStartingIndex], currentMaterialOverride);
                            ChangeGameobjectsMaterialArray(heads[headStartingIndex], currentMaterialOverride);
                            ChangeGameobjectsMaterialArray(facialHairs[facialHairsStartingIndex], currentMaterialOverride);
                            ChangeGameobjectsMaterialArray(hairStyles[hairStyleStartingIndex], currentMaterialOverride);
                        }
                        break;
                    default:
                        break;
                }
                break;
            case Item.ItemType.Legs:
                hips[0].SetActive(false);
                rightLegs[0].SetActive(false);
                leftLegs[0].SetActive(false);
                rightKnees[0].SetActive(false);
                leftKnees[0].SetActive(false);

                hips[item.itemtorsoID].SetActive(true);
                previousMaterial_Hips = hips[item.itemtorsoID].GetComponent<SkinnedMeshRenderer>().materials;

                rightLegs[item.itemLowerRightArmID].SetActive(true);
                previousMaterial_RightLegs = rightLegs[item.itemLowerRightArmID].GetComponent<SkinnedMeshRenderer>().materials;
                leftLegs[item.itemLowerLeftArmID].SetActive(true);
                previousMaterial_LeftLegs = leftLegs[item.itemLowerLeftArmID].GetComponent<SkinnedMeshRenderer>().materials;

                rightKnees[item.itemRightElbowID].SetActive(true);
                if (rightKnees[item.itemRightElbowID].GetComponent<SkinnedMeshRenderer>() != null)
                    previousMaterial_RightKnee = rightKnees[item.itemRightElbowID].GetComponent<SkinnedMeshRenderer>().materials;
                leftKnees[item.itemLeftElbowID].SetActive(true);
                if (leftKnees[item.itemLeftElbowID].GetComponent<SkinnedMeshRenderer>() != null)
                    previousMaterial_LeftKnee = leftKnees[item.itemLeftElbowID].GetComponent<SkinnedMeshRenderer>().materials;

                if (currentMaterialOverride != null)
                {
                    ChangeGameobjectsMaterialArray(hips[item.itemtorsoID], currentMaterialOverride);
                    ChangeGameobjectsMaterialArray(rightLegs[item.itemLowerRightArmID], currentMaterialOverride);
                    ChangeGameobjectsMaterialArray(leftLegs[item.itemLowerLeftArmID], currentMaterialOverride);
                    if (rightKnees[item.itemRightElbowID].GetComponent<SkinnedMeshRenderer>() != null)
                        ChangeGameobjectsMaterialArray(rightKnees[item.itemRightElbowID], currentMaterialOverride);
                    if (leftKnees[item.itemLeftElbowID].GetComponent<SkinnedMeshRenderer>() != null)
                        ChangeGameobjectsMaterialArray(leftKnees[item.itemLeftElbowID], currentMaterialOverride);
                }
                break;
            case Item.ItemType.Armor:
                //Debug.Log("we are showing the armor");
                torsos[0].SetActive(false);
                leftUpperArms[0].SetActive(false);
                rightUpperArms[0].SetActive(false);
                leftLowerArms[0].SetActive(false);
                rightLowerArms[0].SetActive(false);
                leftHands[0].SetActive(false);
                rightHands[0].SetActive(false);

                torsos[item.itemtorsoID].SetActive(true);
                previousMaterial_Torso = torsos[item.itemtorsoID].GetComponent<SkinnedMeshRenderer>().materials;

                rightUpperArms[item.itemUpperRightArmID].SetActive(true);
                previousMaterial_RightUpper = rightUpperArms[item.itemUpperRightArmID].GetComponent<SkinnedMeshRenderer>().materials;
                leftUpperArms[item.itemUpperLeftArmID].SetActive(true);
                previousMaterial_LeftUpper = leftUpperArms[item.itemUpperLeftArmID].GetComponent<SkinnedMeshRenderer>().materials;

                rightLowerArms[item.itemLowerRightArmID].SetActive(true);
                previousMaterial_RightLower = rightLowerArms[item.itemLowerRightArmID].GetComponent<SkinnedMeshRenderer>().materials;
                leftLowerArms[item.itemLowerLeftArmID].SetActive(true);
                previousMaterial_LeftLower = leftLowerArms[item.itemLowerLeftArmID].GetComponent<SkinnedMeshRenderer>().materials;

                rightHands[item.itemRightHandID].SetActive(true);
                previousMaterial_RightHand = rightHands[item.itemRightHandID].GetComponent<SkinnedMeshRenderer>().materials;
                leftHands[item.itemLeftHandID].SetActive(true);
                previousMaterial_LeftHand = leftHands[item.itemLeftHandID].GetComponent<SkinnedMeshRenderer>().materials;

                rightElbows[item.itemRightElbowID].SetActive(true);
                if(rightElbows[item.itemRightElbowID].GetComponent<SkinnedMeshRenderer>() != null)
                    previousMaterial_RightElbow = rightElbows[item.itemRightElbowID].GetComponent<SkinnedMeshRenderer>().materials;
                leftElbows[item.itemLeftElbowID].SetActive(true);
                if (leftElbows[item.itemLeftElbowID].GetComponent<SkinnedMeshRenderer>() != null)
                    previousMaterial_LeftElbow = leftElbows[item.itemLeftElbowID].GetComponent<SkinnedMeshRenderer>().materials;

                rightShoulders[item.itemRightShoulderID].SetActive(true);
                if (rightShoulders[item.itemRightShoulderID].GetComponent<SkinnedMeshRenderer>() != null)
                    previousMaterial_RightShoulder = rightShoulders[item.itemRightShoulderID].GetComponent<SkinnedMeshRenderer>().materials;
                leftShoulders[item.itemLeftShoulderID].SetActive(true);
                if (leftShoulders[item.itemLeftShoulderID].GetComponent<SkinnedMeshRenderer>() != null)
                    previousMaterial_LeftShoulder = leftShoulders[item.itemLeftShoulderID].GetComponent<SkinnedMeshRenderer>().materials;


                if (currentMaterialOverride != null)
                {
                    ChangeGameobjectsMaterialArray(torsos[item.itemtorsoID], currentMaterialOverride);
                    ChangeGameobjectsMaterialArray(rightUpperArms[item.itemUpperRightArmID], currentMaterialOverride);
                    ChangeGameobjectsMaterialArray(leftUpperArms[item.itemUpperLeftArmID], currentMaterialOverride);
                    ChangeGameobjectsMaterialArray(rightLowerArms[item.itemLowerRightArmID], currentMaterialOverride);
                    ChangeGameobjectsMaterialArray(leftLowerArms[item.itemLowerLeftArmID], currentMaterialOverride);
                    ChangeGameobjectsMaterialArray(rightHands[item.itemRightHandID], currentMaterialOverride);
                    ChangeGameobjectsMaterialArray(leftHands[item.itemLeftHandID], currentMaterialOverride);

                    if (rightElbows[item.itemRightElbowID].GetComponent<SkinnedMeshRenderer>() != null)
                        ChangeGameobjectsMaterialArray(rightElbows[item.itemRightElbowID], currentMaterialOverride);
                    if (leftElbows[item.itemLeftElbowID].GetComponent<SkinnedMeshRenderer>() != null)
                        ChangeGameobjectsMaterialArray(leftElbows[item.itemLeftElbowID], currentMaterialOverride);

                    if (rightShoulders[item.itemRightShoulderID].GetComponent<SkinnedMeshRenderer>() != null)
                        ChangeGameobjectsMaterialArray(rightShoulders[item.itemRightShoulderID], currentMaterialOverride);
                    if (leftShoulders[item.itemLeftShoulderID].GetComponent<SkinnedMeshRenderer>() != null)
                        ChangeGameobjectsMaterialArray(leftShoulders[item.itemLeftShoulderID], currentMaterialOverride);
                }
                break;
            default:
                break;
        }
    }

    // USed to add a ew material Ovveride
    public void AddMaterialOverride(MaterialOverrideCode materialChoice)
    {
        MaterialOverride matOverride = new MaterialOverride();

        switch (materialChoice)
        {
            case MaterialOverrideCode.Invisible:
                matOverride.mat = materials[0];
                matOverride.priority = 0;
                break;
            case MaterialOverrideCode.GiantStrength:
                matOverride.mat = materials[1];
                matOverride.priority = 1;
                break;
            default:
                break;
        }
        matOverride.matCode = materialChoice;

        matOverrides.Add(matOverride);

        EvaluateMaterialOverrides();
    }

    // Used to Remove a material ovveride
    public void RemoveMaterialOverride(MaterialOverrideCode materialChoice)
    {
        MaterialOverride matToRemove = null;

        foreach(MaterialOverride matOverride in matOverrides)
        {
            if(matOverride.matCode == materialChoice)
            {
                matToRemove = matOverride;
                break;
            }
        }

        matOverrides.Remove(matToRemove);

        EvaluateMaterialOverrides();
    }

    // Used to check tyhe hierarchy to find the material that has the highest priorty and apply it.
    public void EvaluateMaterialOverrides()
    {
        if (matOverrides.Count > 0)
        {
            float highestPriority = 1000;

            foreach (MaterialOverride matOverride in matOverrides)
            {
                if (matOverride.priority < highestPriority)
                {
                    highestPriority = matOverride.priority;
                    currentMaterialOverride = matOverride.mat;
                }
            }

            ChangeMaterialToNewMaterial(currentMaterialOverride);
        }
        else
        {
            currentMaterialOverride = null;
            ResetToOriginalMaterial();
        }
    }

    private void ChangeGameobjectsMaterialArray(GameObject go, Material newMaterial)
    {
        Material[] materialArrayOverride = new Material[go.GetComponent<SkinnedMeshRenderer>().materials.Length];
        for (int index = 0; index < go.GetComponent<SkinnedMeshRenderer>().materials.Length; index++)
        {
            materialArrayOverride[index] = newMaterial;
            go.GetComponent<SkinnedMeshRenderer>().materials[index] = newMaterial;
        }
        go.GetComponent<SkinnedMeshRenderer>().materials = materialArrayOverride;
    }
    private void ChangeGameobjectsMaterialArrayNotSkinned(GameObject go, Material newMaterial)
    {
        Material[] materialArrayOverride = new Material[go.GetComponent<MeshRenderer>().materials.Length];
        for (int index = 0; index < go.GetComponent<MeshRenderer>().materials.Length; index++)
        {
            materialArrayOverride[index] = newMaterial;
            go.GetComponent<MeshRenderer>().materials[index] = newMaterial;
        }
        go.GetComponent<MeshRenderer>().materials = materialArrayOverride;
    }

    // Used to apply the a new material to all gear, or none.
    public void ChangeMaterialToNewMaterial(Material mat)
    {
        Debug.Log("Changing material override to " + mat);
        Material materialToChangeTo = mat;

        foreach (GameObject go in leftHandWeapons)
            if (go.activeInHierarchy)
                ChangeGameobjectsMaterialArrayNotSkinned(go, materialToChangeTo);
        foreach (GameObject go in rightHandWeapons)
            if (go.activeInHierarchy)
                ChangeGameobjectsMaterialArrayNotSkinned(go, materialToChangeTo);

        foreach (GameObject go in torsos)
            if (go.activeInHierarchy)
                ChangeGameobjectsMaterialArray(go, materialToChangeTo);

        foreach (GameObject go in rightUpperArms)
            if (go.activeInHierarchy)
                ChangeGameobjectsMaterialArray(go, materialToChangeTo);
        foreach (GameObject go in leftUpperArms)
            if (go.activeInHierarchy)
                ChangeGameobjectsMaterialArray(go, materialToChangeTo);

        foreach (GameObject go in rightLowerArms)
            if (go.activeInHierarchy)
                ChangeGameobjectsMaterialArray(go, materialToChangeTo);
        foreach (GameObject go in leftLowerArms)
            if (go.activeInHierarchy)
                ChangeGameobjectsMaterialArray(go, materialToChangeTo);

        foreach (GameObject go in rightHands)
            if (go.activeInHierarchy)
                ChangeGameobjectsMaterialArray(go, materialToChangeTo);
        foreach (GameObject go in leftHands)
            if (go.activeInHierarchy)
                ChangeGameobjectsMaterialArray(go, materialToChangeTo);

        foreach (GameObject go in rightElbows)
            if (go.activeInHierarchy && go.GetComponent<SkinnedMeshRenderer>() != null)
                ChangeGameobjectsMaterialArray(go, materialToChangeTo);
        foreach (GameObject go in leftElbows)
            if (go.activeInHierarchy && go.GetComponent<SkinnedMeshRenderer>() != null)
                ChangeGameobjectsMaterialArray(go, materialToChangeTo);

        foreach (GameObject go in rightShoulders)
            if (go.activeInHierarchy && go.GetComponent<SkinnedMeshRenderer>() != null)
                ChangeGameobjectsMaterialArray(go, materialToChangeTo);
        foreach (GameObject go in leftShoulders)
            if (go.activeInHierarchy && go.GetComponent<SkinnedMeshRenderer>() != null)
                ChangeGameobjectsMaterialArray(go, materialToChangeTo);

        foreach (GameObject go in heads)
            if (go.activeInHierarchy)
                ChangeGameobjectsMaterialArray(go, materialToChangeTo);
        foreach (GameObject go in hairStyles)
            if (go.activeInHierarchy)
                ChangeGameobjectsMaterialArray(go, materialToChangeTo);
        foreach (GameObject go in eyeBrows)
            if (go.activeInHierarchy)
                ChangeGameobjectsMaterialArray(go, materialToChangeTo);
        foreach (GameObject go in facialHairs)
            if (go.activeInHierarchy)
                ChangeGameobjectsMaterialArray(go, materialToChangeTo);
        foreach (GameObject go in helmetNoFeatures)
            if (go.activeInHierarchy)
                ChangeGameobjectsMaterialArray(go, materialToChangeTo);
        foreach (GameObject go in helmetAllFeatures)
            if (go.activeInHierarchy)
                ChangeGameobjectsMaterialArray(go, materialToChangeTo);
        foreach (GameObject go in helmetNoHair)
            if (go.activeInHierarchy)
                ChangeGameobjectsMaterialArray(go, materialToChangeTo);

        foreach (GameObject go in hips)
            if (go.activeInHierarchy)
                ChangeGameobjectsMaterialArray(go, materialToChangeTo);

        foreach (GameObject go in rightLegs)
            if (go.activeInHierarchy)
                ChangeGameobjectsMaterialArray(go, materialToChangeTo);
        foreach (GameObject go in leftLegs)
            if (go.activeInHierarchy)
                ChangeGameobjectsMaterialArray(go, materialToChangeTo);

        foreach (GameObject go in rightKnees)
            if (go.activeInHierarchy && go.GetComponent<SkinnedMeshRenderer>() != null)
                ChangeGameobjectsMaterialArray(go, materialToChangeTo);
        foreach (GameObject go in leftKnees)
            if (go.activeInHierarchy && go.GetComponent<SkinnedMeshRenderer>() != null)
                ChangeGameobjectsMaterialArray(go, materialToChangeTo);

        foreach (GameObject go in capes)
            if (go.activeInHierarchy && go.GetComponent<SkinnedMeshRenderer>())
                ChangeGameobjectsMaterialArray(go, materialToChangeTo);
            else if (go.activeInHierarchy && go.GetComponent<MeshRenderer>())
                ChangeGameobjectsMaterialArrayNotSkinned(go, materialToChangeTo);
        foreach (GameObject go in rings)
            if (go.activeInHierarchy)
                ChangeGameobjectsMaterialArrayNotSkinned(go, materialToChangeTo);
        foreach (GameObject go in bracelets)
            if (go.activeInHierarchy)
                ChangeGameobjectsMaterialArrayNotSkinned(go, materialToChangeTo);
        foreach (GameObject go in waistTrinkets)
            if (go.activeInHierarchy)
                ChangeGameobjectsMaterialArray(go, materialToChangeTo);
    }

    public void ResetToOriginalMaterial()
    {

        foreach (GameObject go in leftHandWeapons)
            if (go.activeInHierarchy)
                go.GetComponent<MeshRenderer>().materials = previousMaterial_LeftWeapon;
        foreach (GameObject go in rightHandWeapons)
            if (go.activeInHierarchy)
                go.GetComponent<MeshRenderer>().materials = previousMaterial_RightWeapon;

        foreach (GameObject go in torsos)
            if (go.activeInHierarchy)
                go.GetComponent<SkinnedMeshRenderer>().materials = previousMaterial_Torso;
        foreach (GameObject go in rightUpperArms)
            if (go.activeInHierarchy)
                go.GetComponent<SkinnedMeshRenderer>().materials = previousMaterial_RightUpper;
        foreach (GameObject go in leftUpperArms)
            if (go.activeInHierarchy)
                go.GetComponent<SkinnedMeshRenderer>().materials = previousMaterial_LeftUpper;
        foreach (GameObject go in rightLowerArms)
            if (go.activeInHierarchy)
                go.GetComponent<SkinnedMeshRenderer>().materials = previousMaterial_RightLower;
        foreach (GameObject go in leftLowerArms)
            if (go.activeInHierarchy)
                go.GetComponent<SkinnedMeshRenderer>().materials = previousMaterial_LeftLower;
        foreach (GameObject go in rightHands)
            if (go.activeInHierarchy)
                go.GetComponent<SkinnedMeshRenderer>().materials = previousMaterial_RightHand;
        foreach (GameObject go in leftHands)
            if (go.activeInHierarchy)
                go.GetComponent<SkinnedMeshRenderer>().materials = previousMaterial_LeftHand;
        foreach (GameObject go in rightElbows)
            if (go.activeInHierarchy && go.GetComponent<SkinnedMeshRenderer>() != null)
                go.GetComponent<SkinnedMeshRenderer>().materials = previousMaterial_RightElbow;
        foreach (GameObject go in leftElbows)
            if (go.activeInHierarchy && go.GetComponent<SkinnedMeshRenderer>() != null)
                go.GetComponent<SkinnedMeshRenderer>().materials = previousMaterial_LeftElbow;
        foreach (GameObject go in rightShoulders)
            if (go.activeInHierarchy && go.GetComponent<SkinnedMeshRenderer>() != null)
                go.GetComponent<SkinnedMeshRenderer>().materials = previousMaterial_RightShoulder;
        foreach (GameObject go in leftShoulders)
            if (go.activeInHierarchy && go.GetComponent<SkinnedMeshRenderer>() != null)
                go.GetComponent<SkinnedMeshRenderer>().materials = previousMaterial_LeftShoulder;

        foreach (GameObject go in heads)
            if (go.activeInHierarchy)
                go.GetComponent<SkinnedMeshRenderer>().materials = previousMaterial_Head;
        foreach (GameObject go in hairStyles)
            if (go.activeInHierarchy && go.GetComponent<SkinnedMeshRenderer>() != null)
                go.GetComponent<SkinnedMeshRenderer>().materials = previousMaterial_Hair;
        foreach (GameObject go in eyeBrows)
            if (go.activeInHierarchy)
                go.GetComponent<SkinnedMeshRenderer>().materials = previousMaterial_Eyebrows;
        foreach (GameObject go in facialHairs)
            if (go.activeInHierarchy)
                go.GetComponent<SkinnedMeshRenderer>().materials = previousMaterial_FacialHair;
        foreach (GameObject go in helmetNoFeatures)
            if (go.activeInHierarchy)
                go.GetComponent<SkinnedMeshRenderer>().materials = previousMaterial_HelmetNoFeatures;
        foreach (GameObject go in helmetAllFeatures)
            if (go.activeInHierarchy)
                go.GetComponent<SkinnedMeshRenderer>().materials = previousMaterial_HelmetAllFeatures;
        foreach (GameObject go in helmetNoHair)
            if (go.activeInHierarchy)
                go.GetComponent<SkinnedMeshRenderer>().materials = previousMaterial_HelmetNoHair;

        foreach (GameObject go in hips)
            if (go.activeInHierarchy)
                go.GetComponent<SkinnedMeshRenderer>().materials = previousMaterial_Hips;
        foreach (GameObject go in rightLegs)
            if (go.activeInHierarchy)
                go.GetComponent<SkinnedMeshRenderer>().materials = previousMaterial_RightLegs;
        foreach (GameObject go in leftLegs)
            if (go.activeInHierarchy)
                go.GetComponent<SkinnedMeshRenderer>().materials = previousMaterial_LeftLegs;
        foreach (GameObject go in rightKnees)
            if (go.activeInHierarchy && go.GetComponent<SkinnedMeshRenderer>() != null)
                go.GetComponent<SkinnedMeshRenderer>().materials = previousMaterial_RightKnee;
        foreach (GameObject go in leftKnees)
            if (go.activeInHierarchy && go.GetComponent<SkinnedMeshRenderer>() != null)
                go.GetComponent<SkinnedMeshRenderer>().materials = previousMaterial_LeftKnee;

        foreach (GameObject go in capes)
            if (go.activeInHierarchy && go.GetComponent<SkinnedMeshRenderer>())
                go.GetComponent<SkinnedMeshRenderer>().materials = previousMaterial_Capes;
            else if (go.activeInHierarchy && go.GetComponent<MeshRenderer>())
                go.GetComponent<MeshRenderer>().materials = previousMaterial_Capes;
        foreach (GameObject go in rings)
            if (go.activeInHierarchy)
                go.GetComponent<MeshRenderer>().materials = previousMaterial_Rings;
        foreach (GameObject go in bracelets)
            if (go.activeInHierarchy)
                go.GetComponent<MeshRenderer>().materials = previousMaterial_Bracelet;
        foreach (GameObject go in waistTrinkets)
            if (go.activeInHierarchy)
                go.GetComponent<SkinnedMeshRenderer>().materials = previousMaterial_WaistTrinkets;
    }
}
