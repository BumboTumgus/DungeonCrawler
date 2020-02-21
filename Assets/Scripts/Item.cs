﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName;
    public string description;
    public int itemGearID;

    public int itemtorsoID;
    public int itemUpperRightArmID;
    public int itemUpperLeftArmID;
    public int itemLowerRightArmID;
    public int itemLowerLeftArmID;
    public int itemRightHandID;
    public int itemLeftHandID;
    public int itemRightElbowID;
    public int itemLeftElbowID;
    public int itemRightShoulderID;
    public int itemLeftShoulderID;


    public enum HelmetType {NoFeatures, NoHair, AllFeatures};
    public HelmetType helmetType;

    public bool equippedToRightHand = false;
    public bool stackable;
    public bool instantPickup;
    public GameObject previousOwner = null;
    public int currentStack;
    public int maxStack;

    public Sprite artwork;

    public int inventoryIndex = 0;

    public int goldValue;

    public enum ItemType { Consumable, Gold, Trinket, Weapon, TwoHandWeapon, Helmet, Legs, Armor};
    public ItemType itemType;

    // Equipable Stats (weapons armor and trinkets)
    public int vitMod;
    public int strMod;
    public int dexMod;
    public int spdMod;
    public int intMod;
    public int wisMod;
    public int chaMod;
    
    public float baseAttackDelay;
    public int hitMin;
    public int hitMax;
    public int hitBase;
    public int staggerBase;
    public float critChance;
    public float critMod;
    
    public float vitScaling = 0;
    public float strScaling = 0;
    public float dexScaling = 0;
    public float spdScaling = 0;
    public float intScaling = 0;
    public float wisScaling = 0;
    public float chaScaling = 0;

    public int health;
    public int mana;
    public int armor;
    public int resistance;
    public int poise;
    public float healthRegen;
    public float manaRegen;

    private const float STAT_MODIFIER_RANGE = 0.25f;

    // Rerolls all the base stats plus or minus a percentage based on the stat modifier range.
    public void RollStatModifiers()
    {
        vitMod += Mathf.RoundToInt(Random.Range(-vitMod * STAT_MODIFIER_RANGE, vitMod * STAT_MODIFIER_RANGE));
        strMod += Mathf.RoundToInt(Random.Range(-strMod * STAT_MODIFIER_RANGE, strMod * STAT_MODIFIER_RANGE));
        dexMod += Mathf.RoundToInt(Random.Range(-dexMod * STAT_MODIFIER_RANGE, dexMod * STAT_MODIFIER_RANGE));
        spdMod += Mathf.RoundToInt(Random.Range(-spdMod * STAT_MODIFIER_RANGE, spdMod * STAT_MODIFIER_RANGE));
        intMod += Mathf.RoundToInt(Random.Range(-intMod * STAT_MODIFIER_RANGE, intMod * STAT_MODIFIER_RANGE));
        wisMod += Mathf.RoundToInt(Random.Range(-wisMod * STAT_MODIFIER_RANGE, wisMod * STAT_MODIFIER_RANGE));
        chaMod += Mathf.RoundToInt(Random.Range(-chaMod * STAT_MODIFIER_RANGE, chaMod * STAT_MODIFIER_RANGE));

        baseAttackDelay += Mathf.RoundToInt(Random.Range(-baseAttackDelay * STAT_MODIFIER_RANGE, baseAttackDelay * STAT_MODIFIER_RANGE));
        hitMin += Mathf.RoundToInt(Random.Range(-hitMin * STAT_MODIFIER_RANGE, hitMin * STAT_MODIFIER_RANGE));
        hitMax += Mathf.RoundToInt(Random.Range(-hitMax * STAT_MODIFIER_RANGE, hitMax * STAT_MODIFIER_RANGE));
        hitBase += Mathf.RoundToInt(Random.Range(-hitBase * STAT_MODIFIER_RANGE, hitBase * STAT_MODIFIER_RANGE));
        staggerBase += Mathf.RoundToInt(Random.Range(-staggerBase * STAT_MODIFIER_RANGE, staggerBase * STAT_MODIFIER_RANGE));
        critChance += Mathf.RoundToInt(Random.Range(-critChance * STAT_MODIFIER_RANGE, critChance * STAT_MODIFIER_RANGE));
        critMod += Mathf.RoundToInt(Random.Range(-critMod * STAT_MODIFIER_RANGE, critMod * STAT_MODIFIER_RANGE));

        armor += Mathf.RoundToInt(Random.Range(-armor * STAT_MODIFIER_RANGE, armor * STAT_MODIFIER_RANGE));
        resistance += Mathf.RoundToInt(Random.Range(-resistance * STAT_MODIFIER_RANGE, resistance * STAT_MODIFIER_RANGE));
        poise += Mathf.RoundToInt(Random.Range(-poise * STAT_MODIFIER_RANGE, poise * STAT_MODIFIER_RANGE));
        healthRegen += Mathf.RoundToInt(Random.Range(-healthRegen * STAT_MODIFIER_RANGE, healthRegen * STAT_MODIFIER_RANGE));
        manaRegen += Mathf.RoundToInt(Random.Range(-manaRegen * STAT_MODIFIER_RANGE, manaRegen * STAT_MODIFIER_RANGE));
    }

    // This is called when the item can be picked up. The item will set it's parent as the player's ivnentory, hide itself and disable collisions.
    public void ComfirmPickup(Transform targetParent, int index)
    {
        GetComponent<MeshRenderer>().enabled = false;

        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer renderer in meshRenderers)
            renderer.enabled = false;

        GetComponent<SphereCollider>().enabled = false;
        transform.SetParent(targetParent);
        transform.localPosition = Vector3.zero;
        inventoryIndex = index;
    }

    // USed to drop this item back on the ground.
    public void ComfirmDrop()
    {
        transform.SetParent(null);
        GetComponent<MeshRenderer>().enabled = true;

        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer renderer in meshRenderers)
            renderer.enabled = true;

        GetComponent<SphereCollider>().enabled = true;
    }
}
