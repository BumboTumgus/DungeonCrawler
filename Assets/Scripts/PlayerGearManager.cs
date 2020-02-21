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

    int headStartingIndex;
    int hairStyleStartingIndex;
    int eyeBrowsStartingIndex;
    int facialHairsStartingIndex;

    private void Start()
    {
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
        foreach (GameObject weapon in leftHandWeapons)
            weapon.SetActive(false);
        
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
        leftUpperArms[0].SetActive(true);
        rightUpperArms[0].SetActive(true);
        leftLowerArms[0].SetActive(true);
        rightLowerArms[0].SetActive(true);
        leftHands[0].SetActive(true);
        rightHands[0].SetActive(true);
        facialHairs[facialHairsStartingIndex].SetActive(true);
        eyeBrows[eyeBrowsStartingIndex].SetActive(true);
        heads[headStartingIndex].SetActive(true);
        hairStyles[hairStyleStartingIndex].SetActive(true);
        hips[0].SetActive(true);
        rightLegs[0].SetActive(true);
        leftLegs[0].SetActive(true);
        rightKnees[0].SetActive(true);
        leftKnees[0].SetActive(true);
    }
    public void HideItem(Item item)
    {
        switch (item.itemType)
        {
            case Item.ItemType.Trinket:
                break;
            case Item.ItemType.Weapon:
                if (item.equippedToRightHand)
                    rightHandWeapons[item.itemGearID].SetActive(false);
                else
                    leftHandWeapons[item.itemGearID].SetActive(false);
                break;
            case Item.ItemType.TwoHandWeapon:
                rightHandWeapons[item.itemGearID].SetActive(false);
                break;
            case Item.ItemType.Helmet:
                switch (item.helmetType)
                {
                    case Item.HelmetType.NoFeatures:
                        helmetNoFeatures[item.itemGearID].SetActive(false);
                        break;
                    case Item.HelmetType.NoHair:
                        helmetNoHair[item.itemGearID].SetActive(false);
                        break;
                    case Item.HelmetType.AllFeatures:
                        helmetAllFeatures[item.itemGearID].SetActive(false);
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
                rightLegs[item.itemLowerRightArmID].SetActive(false);
                leftLegs[item.itemLowerLeftArmID].SetActive(false);
                rightKnees[item.itemRightElbowID].SetActive(false);
                leftKnees[item.itemLeftElbowID].SetActive(false);

                hips[0].SetActive(true);
                rightLegs[0].SetActive(true);
                leftLegs[0].SetActive(true);
                rightKnees[0].SetActive(true);
                leftKnees[0].SetActive(true);
                break;
            case Item.ItemType.Armor:
                Debug.Log("we are hiding the armor");
                torsos[item.itemtorsoID].SetActive(false);
                rightUpperArms[item.itemUpperRightArmID].SetActive(false);
                leftUpperArms[item.itemUpperLeftArmID].SetActive(false);
                rightLowerArms[item.itemLowerRightArmID].SetActive(false);
                leftLowerArms[item.itemLowerLeftArmID].SetActive(false);
                rightHands[item.itemRightHandID].SetActive(false);
                leftHands[item.itemLeftHandID].SetActive(false);
                rightElbows[item.itemRightElbowID].SetActive(false);
                leftElbows[item.itemLeftElbowID].SetActive(false);
                rightShoulders[item.itemRightShoulderID].SetActive(false);
                leftShoulders[item.itemLeftShoulderID].SetActive(false);

                torsos[0].SetActive(true);
                leftUpperArms[0].SetActive(true);
                rightUpperArms[0].SetActive(true);
                leftLowerArms[0].SetActive(true);
                rightLowerArms[0].SetActive(true);
                leftHands[0].SetActive(true);
                rightHands[0].SetActive(true);
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
            case Item.ItemType.Trinket:
                break;
            case Item.ItemType.Weapon:
                if (item.equippedToRightHand)
                    rightHandWeapons[item.itemGearID].SetActive(true);
                else
                    leftHandWeapons[item.itemGearID].SetActive(true);
                break;
            case Item.ItemType.TwoHandWeapon:
                rightHandWeapons[item.itemGearID].SetActive(true);
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
                        break;
                    case Item.HelmetType.NoHair:
                        heads[headStartingIndex].SetActive(true);
                        eyeBrows[eyeBrowsStartingIndex].SetActive(true);
                        hairStyles[hairStyleStartingIndex].SetActive(false);
                        facialHairs[facialHairsStartingIndex].SetActive(true);

                        helmetNoHair[item.itemGearID].SetActive(true);
                        break;
                    case Item.HelmetType.AllFeatures:
                        heads[headStartingIndex].SetActive(true);
                        eyeBrows[eyeBrowsStartingIndex].SetActive(true);
                        hairStyles[hairStyleStartingIndex].SetActive(true);
                        facialHairs[facialHairsStartingIndex].SetActive(true);

                        helmetAllFeatures[item.itemGearID].SetActive(true);
                        break;
                    default:
                        break;
                }
                break;
            case Item.ItemType.Legs:
                Debug.Log("we are showing the leggings");
                hips[0].SetActive(false);
                rightLegs[0].SetActive(false);
                leftLegs[0].SetActive(false);
                rightKnees[0].SetActive(false);
                leftKnees[0].SetActive(false);

                hips[item.itemtorsoID].SetActive(true);
                rightLegs[item.itemLowerRightArmID].SetActive(true);
                leftLegs[item.itemLowerLeftArmID].SetActive(true);
                rightKnees[item.itemRightElbowID].SetActive(true);
                leftKnees[item.itemLeftElbowID].SetActive(true);
                break;
            case Item.ItemType.Armor:
                Debug.Log("we are showing the armor");
                torsos[0].SetActive(false);
                leftUpperArms[0].SetActive(false);
                rightUpperArms[0].SetActive(false);
                leftLowerArms[0].SetActive(false);
                rightLowerArms[0].SetActive(false);
                leftHands[0].SetActive(false);
                rightHands[0].SetActive(false);

                torsos[item.itemtorsoID].SetActive(true);
                rightUpperArms[item.itemUpperRightArmID].SetActive(true);
                leftUpperArms[item.itemUpperLeftArmID].SetActive(true);
                rightLowerArms[item.itemLowerRightArmID].SetActive(true);
                leftLowerArms[item.itemLowerLeftArmID].SetActive(true);
                rightHands[item.itemRightHandID].SetActive(true);
                leftHands[item.itemLeftHandID].SetActive(true);
                rightElbows[item.itemRightElbowID].SetActive(true);
                leftElbows[item.itemLeftElbowID].SetActive(true);
                rightShoulders[item.itemRightShoulderID].SetActive(true);
                leftShoulders[item.itemLeftShoulderID].SetActive(true);
                break;
            default:
                break;
        }
    }
}
