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

    public enum MaterialOverrides { Invisible, GiantStrength}
    public List<Material> materials = new List<Material>();

    private BuffsManager weaponBuffs;
    private bool materialOverriden = false;

    [SerializeField] private Material[] previousMaterials = new Material[25];

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

        // Here I show the base chest items, the onme we decide are the "base values".
        torsos[0].SetActive(true);
        previousMaterials[2] = torsos[0].GetComponent<SkinnedMeshRenderer>().material;

        leftUpperArms[0].SetActive(true);
        previousMaterials[4] = leftUpperArms[0].GetComponent<SkinnedMeshRenderer>().material;
        rightUpperArms[0].SetActive(true);
        previousMaterials[3] = rightUpperArms[0].GetComponent<SkinnedMeshRenderer>().material;

        leftLowerArms[0].SetActive(true);
        previousMaterials[6] = leftLowerArms[0].GetComponent<SkinnedMeshRenderer>().material;
        rightLowerArms[0].SetActive(true);
        previousMaterials[5] = rightLowerArms[0].GetComponent<SkinnedMeshRenderer>().material;

        leftHands[0].SetActive(true);
        previousMaterials[7] = leftHands[0].GetComponent<SkinnedMeshRenderer>().material;
        rightHands[0].SetActive(true);
        previousMaterials[8] = rightHands[0].GetComponent<SkinnedMeshRenderer>().material;

        facialHairs[facialHairsStartingIndex].SetActive(true);
        previousMaterials[16] = facialHairs[facialHairsStartingIndex].GetComponent<SkinnedMeshRenderer>().material;
        eyeBrows[eyeBrowsStartingIndex].SetActive(true);
        previousMaterials[15] = eyeBrows[eyeBrowsStartingIndex].GetComponent<SkinnedMeshRenderer>().material;
        heads[headStartingIndex].SetActive(true);
        previousMaterials[13] = heads[headStartingIndex].GetComponent<SkinnedMeshRenderer>().material;
        hairStyles[hairStyleStartingIndex].SetActive(true);
        previousMaterials[14] = hairStyles[hairStyleStartingIndex].GetComponent<SkinnedMeshRenderer>().material;

        hips[0].SetActive(true);
        previousMaterials[20] = hips[0].GetComponent<SkinnedMeshRenderer>().material;
        rightLegs[0].SetActive(true);
        previousMaterials[21] = rightLegs[0].GetComponent<SkinnedMeshRenderer>().material;
        leftLegs[0].SetActive(true);
        previousMaterials[22] = leftLegs[0].GetComponent<SkinnedMeshRenderer>().material;

        rightKnees[0].SetActive(true);
        leftKnees[0].SetActive(true);
    }
    public void HideItem(Item item)
    {
        switch (item.itemType)
        {
            case Item.ItemType.TrinketCape:
                break;
            case Item.ItemType.TrinketRing:
                break;
            case Item.ItemType.TrinketNecklace:
                break;
            case Item.ItemType.TrinketWaistItem:
                break;
            case Item.ItemType.Weapon:
                if (item.equippedToRightHand)
                {
                    rightHandWeapons[item.itemGearID].SetActive(false);
                    rightHandWeapons[item.itemGearID].GetComponent<MeshRenderer>().material = previousMaterials[1];
                    weaponBuffs.UpdateWeaponEffectsRight(Vector3.zero, Vector3.zero, Quaternion.identity, null);
                }
                else
                {
                    leftHandWeapons[item.itemGearID].SetActive(false);
                    leftHandWeapons[item.itemGearID].GetComponent<MeshRenderer>().material = previousMaterials[0];
                    weaponBuffs.UpdateWeaponEffectsLeft(Vector3.zero, Vector3.zero, Quaternion.identity, null);
                }
                break;
            case Item.ItemType.TwoHandWeapon:
                rightHandWeapons[item.itemGearID].SetActive(false);
                rightHandWeapons[item.itemGearID].GetComponent<MeshRenderer>().material = previousMaterials[1];
                weaponBuffs.UpdateWeaponEffectsRight(Vector3.zero, Vector3.zero, Quaternion.identity, null);
                break;
            case Item.ItemType.Helmet:
                switch (item.helmetType)
                {
                    case Item.HelmetType.NoFeatures:
                        helmetNoFeatures[item.itemGearID].SetActive(false);
                        helmetNoFeatures[item.itemGearID].GetComponent<SkinnedMeshRenderer>().material = previousMaterials[17];
                        break;
                    case Item.HelmetType.NoHair:
                        helmetNoHair[item.itemGearID].SetActive(false);
                        helmetNoHair[item.itemGearID].GetComponent<SkinnedMeshRenderer>().material = previousMaterials[19];
                        break;
                    case Item.HelmetType.AllFeatures:
                        helmetAllFeatures[item.itemGearID].SetActive(false);
                        helmetAllFeatures[item.itemGearID].GetComponent<SkinnedMeshRenderer>().material = previousMaterials[18];
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
                Debug.Log("we are hiding the leggings");
                hips[item.itemtorsoID].SetActive(false);
                hips[item.itemtorsoID].GetComponent<SkinnedMeshRenderer>().material = previousMaterials[20];
                rightLegs[item.itemLowerRightArmID].SetActive(false);
                rightLegs[item.itemLowerRightArmID].GetComponent<SkinnedMeshRenderer>().material = previousMaterials[21];
                leftLegs[item.itemLowerLeftArmID].SetActive(false);
                leftLegs[item.itemLowerLeftArmID].GetComponent<SkinnedMeshRenderer>().material = previousMaterials[22];
                rightKnees[item.itemRightElbowID].SetActive(false);
                if(rightKnees[item.itemRightElbowID].GetComponent<SkinnedMeshRenderer>() != null)
                    rightKnees[item.itemRightElbowID].GetComponent<SkinnedMeshRenderer>().material = previousMaterials[23];
                leftKnees[item.itemLeftElbowID].SetActive(false);
                if (leftKnees[item.itemLeftElbowID].GetComponent<SkinnedMeshRenderer>() != null)
                    leftKnees[item.itemLeftElbowID].GetComponent<SkinnedMeshRenderer>().material = previousMaterials[24];

                hips[0].SetActive(true);
                previousMaterials[20] = hips[0].GetComponent<SkinnedMeshRenderer>().material;
                rightLegs[0].SetActive(true);
                previousMaterials[21] = rightLegs[0].GetComponent<SkinnedMeshRenderer>().material;
                leftLegs[0].SetActive(true);
                previousMaterials[22] = leftLegs[0].GetComponent<SkinnedMeshRenderer>().material;
                rightKnees[0].SetActive(true);
                previousMaterials[23] = null;
                leftKnees[0].SetActive(true);
                previousMaterials[24] = null;
                break;
            case Item.ItemType.Armor:
                Debug.Log("we are hiding the armor");
                torsos[item.itemtorsoID].SetActive(false);
                torsos[item.itemtorsoID].GetComponent<SkinnedMeshRenderer>().material = previousMaterials[2];
                rightUpperArms[item.itemUpperRightArmID].SetActive(false);
                rightUpperArms[item.itemUpperRightArmID].GetComponent<SkinnedMeshRenderer>().material = previousMaterials[3];
                leftUpperArms[item.itemUpperLeftArmID].SetActive(false);
                leftUpperArms[item.itemUpperLeftArmID].GetComponent<SkinnedMeshRenderer>().material = previousMaterials[4];
                rightLowerArms[item.itemLowerRightArmID].SetActive(false);
                rightLowerArms[item.itemLowerRightArmID].GetComponent<SkinnedMeshRenderer>().material = previousMaterials[5];
                leftLowerArms[item.itemLowerLeftArmID].SetActive(false);
                leftLowerArms[item.itemLowerLeftArmID].GetComponent<SkinnedMeshRenderer>().material = previousMaterials[6];
                rightHands[item.itemRightHandID].SetActive(false);
                rightHands[item.itemRightHandID].GetComponent<SkinnedMeshRenderer>().material = previousMaterials[7];
                leftHands[item.itemLeftHandID].SetActive(false);
                leftHands[item.itemLeftHandID].GetComponent<SkinnedMeshRenderer>().material = previousMaterials[8];
                rightElbows[item.itemRightElbowID].SetActive(false);
                if (rightElbows[item.itemRightElbowID].GetComponent<SkinnedMeshRenderer>() != null)
                    rightElbows[item.itemRightElbowID].GetComponent<SkinnedMeshRenderer>().material = previousMaterials[9];
                leftElbows[item.itemLeftElbowID].SetActive(false);
                if (leftElbows[item.itemLeftElbowID].GetComponent<SkinnedMeshRenderer>() != null)
                    leftElbows[item.itemLeftElbowID].GetComponent<SkinnedMeshRenderer>().material = previousMaterials[10];
                rightShoulders[item.itemRightShoulderID].SetActive(false);
                if (rightShoulders[item.itemRightShoulderID].GetComponent<SkinnedMeshRenderer>() != null)
                    rightShoulders[item.itemRightShoulderID].GetComponent<SkinnedMeshRenderer>().material = previousMaterials[11];
                leftShoulders[item.itemLeftShoulderID].SetActive(false);
                if (leftShoulders[item.itemLeftShoulderID].GetComponent<SkinnedMeshRenderer>() != null)
                    leftShoulders[item.itemLeftShoulderID].GetComponent<SkinnedMeshRenderer>().material = previousMaterials[12];

                torsos[0].SetActive(true);
                previousMaterials[2] = torsos[0].GetComponent<SkinnedMeshRenderer>().material;
                leftUpperArms[0].SetActive(true);
                previousMaterials[4] = leftUpperArms[0].GetComponent<SkinnedMeshRenderer>().material;
                rightUpperArms[0].SetActive(true);
                previousMaterials[3] = rightUpperArms[0].GetComponent<SkinnedMeshRenderer>().material;
                leftLowerArms[0].SetActive(true);
                previousMaterials[6] = leftLowerArms[0].GetComponent<SkinnedMeshRenderer>().material;
                rightLowerArms[0].SetActive(true);
                previousMaterials[5] = rightLowerArms[0].GetComponent<SkinnedMeshRenderer>().material;
                leftHands[0].SetActive(true);
                previousMaterials[8] = leftHands[0].GetComponent<SkinnedMeshRenderer>().material;
                rightHands[0].SetActive(true);
                previousMaterials[7] = rightHands[0].GetComponent<SkinnedMeshRenderer>().material;
                break;
            default:
                break;
        }
    }

    // Used to show the specific weapon in question based on that weapons index.
    public void ShowItem(Item item)
    {
        switch (item.itemType)
        {
            case Item.ItemType.TrinketCape:
                break;
            case Item.ItemType.TrinketRing:
                break;
            case Item.ItemType.TrinketNecklace:
                break;
            case Item.ItemType.TrinketWaistItem:
                break;
            case Item.ItemType.Weapon:
                if (item.equippedToRightHand)
                {
                    rightHandWeapons[item.itemGearID].SetActive(true);
                    if(rightHandWeapons[item.itemGearID].GetComponent<MeshRenderer>() != null)
                        previousMaterials[1] = rightHandWeapons[item.itemGearID].GetComponent<MeshRenderer>().material;
                    weaponBuffs.UpdateWeaponEffectsRight(rightHandWeapons[item.itemGearID].transform.position, rightHandWeapons[item.itemGearID].transform.localScale, rightHandWeapons[item.itemGearID].transform.rotation, rightHandWeapons[item.itemGearID].GetComponent<MeshFilter>().mesh);
                }
                else
                {
                    leftHandWeapons[item.itemGearID].SetActive(true);
                    if (leftHandWeapons[item.itemGearID].GetComponent<MeshRenderer>() != null)
                        previousMaterials[0] = leftHandWeapons[item.itemGearID].GetComponent<MeshRenderer>().material;
                    weaponBuffs.UpdateWeaponEffectsLeft(leftHandWeapons[item.itemGearID].transform.position, leftHandWeapons[item.itemGearID].transform.localScale, leftHandWeapons[item.itemGearID].transform.rotation, leftHandWeapons[item.itemGearID].GetComponent<MeshFilter>().mesh);
                }
                break;
            case Item.ItemType.TwoHandWeapon:
                rightHandWeapons[item.itemGearID].SetActive(true);
                if (rightHandWeapons[item.itemGearID].GetComponent<MeshRenderer>() != null)
                    previousMaterials[1] = rightHandWeapons[item.itemGearID].GetComponent<MeshRenderer>().material;
                weaponBuffs.UpdateWeaponEffectsRight(rightHandWeapons[item.itemGearID].transform.position, rightHandWeapons[item.itemGearID].transform.localScale, rightHandWeapons[item.itemGearID].transform.rotation, rightHandWeapons[item.itemGearID].GetComponent<MeshFilter>().mesh);
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
                        previousMaterials[17] = helmetNoFeatures[item.itemGearID].GetComponent<SkinnedMeshRenderer>().material;
                        break;
                    case Item.HelmetType.NoHair:
                        heads[headStartingIndex].SetActive(true);
                        eyeBrows[eyeBrowsStartingIndex].SetActive(true);
                        hairStyles[hairStyleStartingIndex].SetActive(false);
                        facialHairs[facialHairsStartingIndex].SetActive(true);

                        helmetNoHair[item.itemGearID].SetActive(true);
                        previousMaterials[18] = helmetNoHair[item.itemGearID].GetComponent<SkinnedMeshRenderer>().material;
                        break;
                    case Item.HelmetType.AllFeatures:
                        heads[headStartingIndex].SetActive(true);
                        eyeBrows[eyeBrowsStartingIndex].SetActive(true);
                        hairStyles[hairStyleStartingIndex].SetActive(true);
                        facialHairs[facialHairsStartingIndex].SetActive(true);

                        helmetAllFeatures[item.itemGearID].SetActive(true);
                        previousMaterials[19] = helmetAllFeatures[item.itemGearID].GetComponent<SkinnedMeshRenderer>().material;
                        break;
                    default:
                        break;
                }
                break;
            case Item.ItemType.Legs:
                //Debug.Log("we are showing the leggings");
                hips[0].SetActive(false);
                rightLegs[0].SetActive(false);
                leftLegs[0].SetActive(false);
                rightKnees[0].SetActive(false);
                leftKnees[0].SetActive(false);

                hips[item.itemtorsoID].SetActive(true);
                previousMaterials[20] = hips[item.itemtorsoID].GetComponent<SkinnedMeshRenderer>().material;

                rightLegs[item.itemLowerRightArmID].SetActive(true);
                previousMaterials[21] = rightLegs[item.itemLowerRightArmID].GetComponent<SkinnedMeshRenderer>().material;
                leftLegs[item.itemLowerLeftArmID].SetActive(true);
                previousMaterials[22] = leftLegs[item.itemLowerLeftArmID].GetComponent<SkinnedMeshRenderer>().material;

                rightKnees[item.itemRightElbowID].SetActive(true);
                if (rightKnees[item.itemRightElbowID].GetComponent<SkinnedMeshRenderer>() != null)
                    previousMaterials[23] = rightKnees[item.itemRightElbowID].GetComponent<SkinnedMeshRenderer>().material;
                leftKnees[item.itemLeftElbowID].SetActive(true);
                if (leftKnees[item.itemLeftElbowID].GetComponent<SkinnedMeshRenderer>() != null)
                    previousMaterials[24] = leftKnees[item.itemLeftElbowID].GetComponent<SkinnedMeshRenderer>().material;
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
                previousMaterials[2] = torsos[item.itemtorsoID].GetComponent<SkinnedMeshRenderer>().material;

                rightUpperArms[item.itemUpperRightArmID].SetActive(true);
                previousMaterials[3] = rightUpperArms[item.itemUpperRightArmID].GetComponent<SkinnedMeshRenderer>().material;
                leftUpperArms[item.itemUpperLeftArmID].SetActive(true);
                previousMaterials[4] = leftUpperArms[item.itemUpperLeftArmID].GetComponent<SkinnedMeshRenderer>().material;

                rightLowerArms[item.itemLowerRightArmID].SetActive(true);
                previousMaterials[5] = rightLowerArms[item.itemLowerRightArmID].GetComponent<SkinnedMeshRenderer>().material;
                leftLowerArms[item.itemLowerLeftArmID].SetActive(true);
                previousMaterials[6] = leftLowerArms[item.itemLowerLeftArmID].GetComponent<SkinnedMeshRenderer>().material;

                rightHands[item.itemRightHandID].SetActive(true);
                previousMaterials[7] = rightHands[item.itemRightHandID].GetComponent<SkinnedMeshRenderer>().material;
                leftHands[item.itemLeftHandID].SetActive(true);
                previousMaterials[8] = leftHands[item.itemLeftHandID].GetComponent<SkinnedMeshRenderer>().material;

                rightElbows[item.itemRightElbowID].SetActive(true);
                if(rightElbows[item.itemRightElbowID].GetComponent<SkinnedMeshRenderer>() != null)
                    previousMaterials[9] = rightElbows[item.itemRightElbowID].GetComponent<SkinnedMeshRenderer>().material;
                leftElbows[item.itemLeftElbowID].SetActive(true);
                if (leftElbows[item.itemLeftElbowID].GetComponent<SkinnedMeshRenderer>() != null)
                    previousMaterials[10] = leftElbows[item.itemLeftElbowID].GetComponent<SkinnedMeshRenderer>().material;

                rightShoulders[item.itemRightShoulderID].SetActive(true);
                if (rightShoulders[item.itemRightShoulderID].GetComponent<SkinnedMeshRenderer>() != null)
                    previousMaterials[11] = rightShoulders[item.itemRightShoulderID].GetComponent<SkinnedMeshRenderer>().material;
                leftShoulders[item.itemLeftShoulderID].SetActive(true);
                if (leftShoulders[item.itemLeftShoulderID].GetComponent<SkinnedMeshRenderer>() != null)
                    previousMaterials[12] = leftShoulders[item.itemLeftShoulderID].GetComponent<SkinnedMeshRenderer>().material;
                break;
            default:
                break;
        }
    }

    // Used to apply the a new material to all gear, or none.
    public void ChangeMaterialToNewMaterial(MaterialOverrides materialChoice)
    {
        materialOverriden = true;

        Material materialToChangeTo = null;

        switch (materialChoice)
        {
            case MaterialOverrides.Invisible:
                materialToChangeTo = materials[0];
                break;
            case MaterialOverrides.GiantStrength:
                materialToChangeTo = materials[1];
                break;
            default:
                break;
        }


        foreach (GameObject go in leftHandWeapons)
            if (go.activeInHierarchy)
                go.GetComponent<MeshRenderer>().material = materialToChangeTo;
        foreach (GameObject go in rightHandWeapons)
            if (go.activeInHierarchy)
                go.GetComponent<MeshRenderer>().material = materialToChangeTo;

        foreach (GameObject go in torsos)
            if (go.activeInHierarchy)
                go.GetComponent<SkinnedMeshRenderer>().material = materialToChangeTo;

        foreach (GameObject go in rightUpperArms)
            if (go.activeInHierarchy)
                go.GetComponent<SkinnedMeshRenderer>().material = materialToChangeTo;
        foreach (GameObject go in leftUpperArms)
            if (go.activeInHierarchy)
                go.GetComponent<SkinnedMeshRenderer>().material = materialToChangeTo;

        foreach (GameObject go in rightLowerArms)
            if (go.activeInHierarchy)
                go.GetComponent<SkinnedMeshRenderer>().material = materialToChangeTo;
        foreach (GameObject go in leftLowerArms)
            if (go.activeInHierarchy)
                go.GetComponent<SkinnedMeshRenderer>().material = materialToChangeTo;

        foreach (GameObject go in rightHands)
            if (go.activeInHierarchy)
                go.GetComponent<SkinnedMeshRenderer>().material = materialToChangeTo;
        foreach (GameObject go in leftHands)
            if (go.activeInHierarchy)
                go.GetComponent<SkinnedMeshRenderer>().material = materialToChangeTo;

        foreach (GameObject go in rightElbows)
            if (go.activeInHierarchy && go.GetComponent<SkinnedMeshRenderer>() != null)
                go.GetComponent<SkinnedMeshRenderer>().material = materialToChangeTo;
        foreach (GameObject go in leftElbows)
            if (go.activeInHierarchy && go.GetComponent<SkinnedMeshRenderer>() != null)
                go.GetComponent<SkinnedMeshRenderer>().material = materialToChangeTo;

        foreach (GameObject go in rightShoulders)
            if (go.activeInHierarchy && go.GetComponent<SkinnedMeshRenderer>() != null)
                go.GetComponent<SkinnedMeshRenderer>().material = materialToChangeTo;
        foreach (GameObject go in leftShoulders)
            if (go.activeInHierarchy && go.GetComponent<SkinnedMeshRenderer>() != null)
                go.GetComponent<SkinnedMeshRenderer>().material = materialToChangeTo;

        foreach (GameObject go in heads)
            if (go.activeInHierarchy)
                go.GetComponent<SkinnedMeshRenderer>().material = materialToChangeTo;
        foreach (GameObject go in hairStyles)
            if (go.activeInHierarchy && go.GetComponent<SkinnedMeshRenderer>() != null)
                go.GetComponent<SkinnedMeshRenderer>().material = materialToChangeTo;
        foreach (GameObject go in eyeBrows)
            if (go.activeInHierarchy)
                go.GetComponent<SkinnedMeshRenderer>().material = materialToChangeTo;
        foreach (GameObject go in facialHairs)
            if (go.activeInHierarchy)
                go.GetComponent<SkinnedMeshRenderer>().material = materialToChangeTo;
        foreach (GameObject go in helmetNoFeatures)
            if (go.activeInHierarchy)
                go.GetComponent<SkinnedMeshRenderer>().material = materialToChangeTo;
        foreach (GameObject go in helmetAllFeatures)
            if (go.activeInHierarchy)
                go.GetComponent<SkinnedMeshRenderer>().material = materialToChangeTo;
        foreach (GameObject go in helmetNoHair)
            if (go.activeInHierarchy)
                go.GetComponent<SkinnedMeshRenderer>().material = materialToChangeTo;

        foreach (GameObject go in hips)
            if (go.activeInHierarchy)
                go.GetComponent<SkinnedMeshRenderer>().material = materialToChangeTo;

        foreach (GameObject go in rightLegs)
            if (go.activeInHierarchy)
                go.GetComponent<SkinnedMeshRenderer>().material = materialToChangeTo;
        foreach (GameObject go in leftLegs)
            if (go.activeInHierarchy)
                go.GetComponent<SkinnedMeshRenderer>().material = materialToChangeTo;

        foreach (GameObject go in rightKnees)
            if (go.activeInHierarchy && go.GetComponent<SkinnedMeshRenderer>() != null)
                go.GetComponent<SkinnedMeshRenderer>().material = materialToChangeTo;
        foreach (GameObject go in leftKnees)
            if (go.activeInHierarchy && go.GetComponent<SkinnedMeshRenderer>() != null)
                go.GetComponent<SkinnedMeshRenderer>().material = materialToChangeTo;
    }

    public void ResetToOriginalMaterial()
    {
        materialOverriden = false;

        foreach (GameObject go in leftHandWeapons)
            if (go.activeInHierarchy)
                go.GetComponent<MeshRenderer>().material = previousMaterials[0];
        foreach (GameObject go in rightHandWeapons)
            if (go.activeInHierarchy)
                go.GetComponent<MeshRenderer>().material = previousMaterials[1];

        foreach (GameObject go in torsos)
            if (go.activeInHierarchy)
                go.GetComponent<SkinnedMeshRenderer>().material = previousMaterials[2];
        foreach (GameObject go in rightUpperArms)
            if (go.activeInHierarchy)
                go.GetComponent<SkinnedMeshRenderer>().material = previousMaterials[3];
        foreach (GameObject go in leftUpperArms)
            if (go.activeInHierarchy)
                go.GetComponent<SkinnedMeshRenderer>().material = previousMaterials[4];
        foreach (GameObject go in rightLowerArms)
            if (go.activeInHierarchy)
                go.GetComponent<SkinnedMeshRenderer>().material = previousMaterials[5];
        foreach (GameObject go in leftLowerArms)
            if (go.activeInHierarchy)
                go.GetComponent<SkinnedMeshRenderer>().material = previousMaterials[6];
        foreach (GameObject go in rightHands)
            if (go.activeInHierarchy)
                go.GetComponent<SkinnedMeshRenderer>().material = previousMaterials[7];
        foreach (GameObject go in leftHands)
            if (go.activeInHierarchy)
                go.GetComponent<SkinnedMeshRenderer>().material = previousMaterials[8];
        foreach (GameObject go in rightElbows)
            if (go.activeInHierarchy && go.GetComponent<SkinnedMeshRenderer>() != null)
                go.GetComponent<SkinnedMeshRenderer>().material = previousMaterials[9];
        foreach (GameObject go in leftElbows)
            if (go.activeInHierarchy && go.GetComponent<SkinnedMeshRenderer>() != null)
                go.GetComponent<SkinnedMeshRenderer>().material = previousMaterials[10];
        foreach (GameObject go in rightShoulders)
            if (go.activeInHierarchy && go.GetComponent<SkinnedMeshRenderer>() != null)
                go.GetComponent<SkinnedMeshRenderer>().material = previousMaterials[11];
        foreach (GameObject go in leftShoulders)
            if (go.activeInHierarchy && go.GetComponent<SkinnedMeshRenderer>() != null)
                go.GetComponent<SkinnedMeshRenderer>().material = previousMaterials[12];

        foreach (GameObject go in heads)
            if (go.activeInHierarchy)
                go.GetComponent<SkinnedMeshRenderer>().material = previousMaterials[13];
        foreach (GameObject go in hairStyles)
            if (go.activeInHierarchy && go.GetComponent<SkinnedMeshRenderer>() != null)
                go.GetComponent<SkinnedMeshRenderer>().material = previousMaterials[14];
        foreach (GameObject go in eyeBrows)
            if (go.activeInHierarchy)
                go.GetComponent<SkinnedMeshRenderer>().material = previousMaterials[15];
        foreach (GameObject go in facialHairs)
            if (go.activeInHierarchy)
                go.GetComponent<SkinnedMeshRenderer>().material = previousMaterials[16];
        foreach (GameObject go in helmetNoFeatures)
            if (go.activeInHierarchy)
                go.GetComponent<SkinnedMeshRenderer>().material = previousMaterials[17];
        foreach (GameObject go in helmetAllFeatures)
            if (go.activeInHierarchy)
                go.GetComponent<SkinnedMeshRenderer>().material = previousMaterials[18];
        foreach (GameObject go in helmetNoHair)
            if (go.activeInHierarchy)
                go.GetComponent<SkinnedMeshRenderer>().material = previousMaterials[19];

        foreach (GameObject go in hips)
            if (go.activeInHierarchy)
                go.GetComponent<SkinnedMeshRenderer>().material = previousMaterials[20];
        foreach (GameObject go in rightLegs)
            if (go.activeInHierarchy)
                go.GetComponent<SkinnedMeshRenderer>().material = previousMaterials[21];
        foreach (GameObject go in leftLegs)
            if (go.activeInHierarchy)
                go.GetComponent<SkinnedMeshRenderer>().material = previousMaterials[22];
        foreach (GameObject go in rightKnees)
            if (go.activeInHierarchy && go.GetComponent<SkinnedMeshRenderer>() != null)
                go.GetComponent<SkinnedMeshRenderer>().material = previousMaterials[23];
        foreach (GameObject go in leftKnees)
            if (go.activeInHierarchy && go.GetComponent<SkinnedMeshRenderer>() != null)
                go.GetComponent<SkinnedMeshRenderer>().material = previousMaterials[24];
    }
}
