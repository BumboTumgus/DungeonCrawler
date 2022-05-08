using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatUpdater : MonoBehaviour
{
    float healthMax;
    float healthRegen;
    float armor;
    float flatDamageReduction;
    float attackSpeed;
    float baseDamage;
    float critChance;
    float critDamage;
    float movespeed;
    float jumps;
    float luck;
    float aflameResistance;
    float overchargeResistance;
    float overgrowthResistance;
    float sunderResistance;
    float windshearResistance;
    float asleepResistance;
    float stunResistance;
    float bleedResistance;
    float poisonResistance;
    float frostbiteResistance;
    float knockbackResistance;
    float cooldownReduction;
    float damageIncreaseMultiplier;
    List<Item> weaponsToHitWith = new List<Item>();
    public BarManager expBar;
    public BarManager healthBar;

    public Text[] tooltips;
    //public Color[] uiColors;

    public bool mouseWithItemHovered = false;

    // This method is called at the start of the game to set up all the player's stats. It is also called whenever a player switches gear or the stats would ahve to change.
    public void SetStatValues(PlayerStats stats)
    {
        transform.Find("Playername").GetComponent<Text>().text = stats.playerName + " the " + stats.playerTitle;
        transform.Find("Level_Value").GetComponent<Text>().text = stats.level + "";
        transform.Find("EXP_Value").GetComponent<Text>().text = stats.exp + " / " + stats.expTarget;
        SetExpBarsValues(stats);
        SetHealthManaBarValues(stats);

        //transform.Find("Health_Value").GetComponent<Text>().text = string.Format("{0:0}",stats.health) + " / " + stats.healthMax;
        //transform.Find("HealthRegen_Value").GetComponent<Text>().text = string.Format("{0:0.0}", stats.healthRegen);

        transform.Find("Armor_Value").GetComponent<Text>().text = string.Format("{0:0}", stats.armor);
        transform.Find("Armor_Value").GetComponent<Text>().fontSize = 26;
        transform.Find("FDR_Value").GetComponent<Text>().text = string.Format("{0:0}", stats.flatDamageReduction);
        transform.Find("FDR_Value").GetComponent<Text>().fontSize = 26;

        switch (stats.weaponsToHitWith.Count)
        {
            case 0:
                transform.Find("AttackDamageRightHand_Value").GetComponent<Text>().text = string.Format("{0:0}", stats.baseDamage * stats.damageIncreaseMultiplier);
                transform.Find("AttackDamageLeftHand_Value").GetComponent<Text>().text = string.Format("{0:0}", 0);
                break;
            case 1:
                transform.Find("AttackDamageRightHand_Value").GetComponent<Text>().text = string.Format("{0:0}", stats.weaponsToHitWith[0].baseDamageScaling * stats.baseDamage * stats.damageIncreaseMultiplier);
                transform.Find("AttackDamageLeftHand_Value").GetComponent<Text>().text = string.Format("{0:0}", 0);
                break;
            case 2:
                if(stats.weaponsToHitWith[0].equippedToRightHand)
                {
                    transform.Find("AttackDamageRightHand_Value").GetComponent<Text>().text = string.Format("{0:0}", stats.weaponsToHitWith[0].baseDamageScaling * stats.baseDamage * stats.damageIncreaseMultiplier);
                    transform.Find("AttackDamageLeftHand_Value").GetComponent<Text>().text = string.Format("{0:0}", stats.weaponsToHitWith[1].baseDamageScaling * stats.baseDamage * stats.damageIncreaseMultiplier);
                }
                else
                {
                    transform.Find("AttackDamageRightHand_Value").GetComponent<Text>().text = string.Format("{0:0}", stats.weaponsToHitWith[1].baseDamageScaling * stats.baseDamage * stats.damageIncreaseMultiplier);
                    transform.Find("AttackDamageLeftHand_Value").GetComponent<Text>().text = string.Format("{0:0}", stats.weaponsToHitWith[0].baseDamageScaling * stats.baseDamage * stats.damageIncreaseMultiplier);
                }

                break;
            default:
                break;
        }
        transform.Find("AttackDamageRightHand_Value").GetComponent<Text>().fontSize = 26;
        transform.Find("AttackDamageLeftHand_Value").GetComponent<Text>().fontSize = 26;

        transform.Find("CritChance_Value").GetComponent<Text>().fontSize = 26;
        transform.Find("CritDamage_Value").GetComponent<Text>().fontSize = 26;
        transform.Find("CritChance_Value").GetComponent<Text>().text = string.Format("{0:0}%", stats.critChance * 100);
        transform.Find("CritDamage_Value").GetComponent<Text>().text = string.Format("{0:0}%", stats.critDamageMultiplier * 100);

        transform.Find("AttackSpeed_Value").GetComponent<Text>().text = string.Format("{0:0}%", stats.attackSpeed * 100 );
        transform.Find("CooldownReduction_Value").GetComponent<Text>().text = string.Format("{0:0}%", stats.cooldownReduction * 100);
        transform.Find("AttackSpeed_Value").GetComponent<Text>().fontSize = 26;
        transform.Find("CooldownReduction_Value").GetComponent<Text>().fontSize = 26;

        transform.Find("Movespeed_Value").GetComponent<Text>().text = string.Format("{0:0}%", stats.movespeedPercentMultiplier * 100);
        transform.Find("Jumps_Value").GetComponent<Text>().text = string.Format("{0:0}", stats.jumps);
        transform.Find("Luck_Value").GetComponent<Text>().text = string.Format("{0:0}%", stats.luck);
        transform.Find("Movespeed_Value").GetComponent<Text>().fontSize = 26;
        transform.Find("Jumps_Value").GetComponent<Text>().fontSize = 26;
        transform.Find("Luck_Value").GetComponent<Text>().fontSize = 26;

        transform.Find("AsleepResistance_Value").GetComponent<Text>().text = string.Format("{0:0}%", stats.sleepResistance * 100);
        transform.Find("StunResistance_Value").GetComponent<Text>().text = string.Format("{0:0}%", stats.stunResistance * 100);
        transform.Find("KnockbackResistance_Value").GetComponent<Text>().text = string.Format("{0:0}%", stats.knockbackResistance * 100);
        transform.Find("BleedResistance_Value").GetComponent<Text>().text = string.Format("{0:0}%", stats.bleedResistance * 100);
        transform.Find("PoisonResistance_Value").GetComponent<Text>().text = string.Format("{0:0}%", stats.poisonResistance * 100);
        transform.Find("AflameResistance_Value").GetComponent<Text>().text = string.Format("{0:0}%", stats.aflameResistance * 100);
        transform.Find("FrostbiteResistance_Value").GetComponent<Text>().text = string.Format("{0:0}%", stats.frostbiteResistance * 100);
        transform.Find("OverchargeResistance_Value").GetComponent<Text>().text = string.Format("{0:0}%", stats.overchargeResistance * 100);
        transform.Find("NatureResistance_Value").GetComponent<Text>().text = string.Format("{0:0}%", stats.overgrowthResistance * 100);
        transform.Find("SunderedResistance_Value").GetComponent<Text>().text = string.Format("{0:0}%", stats.sunderResistance * 100);
        transform.Find("WindshearResistance_Value").GetComponent<Text>().text = string.Format("{0:0}%", stats.windshearResistance * 100);
        transform.Find("AsleepResistance_Value").GetComponent<Text>().fontSize = 24;
        transform.Find("StunResistance_Value").GetComponent<Text>().fontSize = 24;
        transform.Find("KnockbackResistance_Value").GetComponent<Text>().fontSize = 24;
        transform.Find("BleedResistance_Value").GetComponent<Text>().fontSize = 24;
        transform.Find("PoisonResistance_Value").GetComponent<Text>().fontSize = 24;
        transform.Find("AflameResistance_Value").GetComponent<Text>().fontSize = 24;
        transform.Find("FrostbiteResistance_Value").GetComponent<Text>().fontSize = 24;
        transform.Find("OverchargeResistance_Value").GetComponent<Text>().fontSize = 24;
        transform.Find("NatureResistance_Value").GetComponent<Text>().fontSize = 24;
        transform.Find("SunderedResistance_Value").GetComponent<Text>().fontSize = 24;
        transform.Find("WindshearResistance_Value").GetComponent<Text>().fontSize = 24;

        transform.Find("Money_Value").GetComponent<Text>().text = string.Format("{0:0}", stats.gold);
        transform.Find("Money_Value").GetComponent<Text>().fontSize = 24;

        UpdateTooltips(stats);
    }

    // This method is used to update the health and mana values of the player.
    public void SetHealthManaBarValues(PlayerStats stats)
    {
        healthBar.Initialize(stats.healthMax, false, true, stats.health);
        healthBar.SetValue(stats.health, true);
        healthBar.transform.Find("HealthBarFill").Find("Value").GetComponent<Text>().text = string.Format("{0:0} / {1:0}", stats.health, stats.healthMax);
        healthBar.transform.Find("HealthBarFill").Find("RegenValue").GetComponent<Text>().text = string.Format("+{0:0.0} hp/5", stats.healthRegen);
        healthBar.transform.Find("HealthBarFill").Find("Value").GetComponent<Text>().fontSize = 30;
        healthBar.transform.Find("HealthBarFill").Find("RegenValue").GetComponent<Text>().fontSize = 30;

        //transform.Find("Health_Value").GetComponent<Text>().text = string.Format("{0:0}", stats.health) + " / " + stats.healthMax;
    }

    public void UpdateHealthManaBarValues(PlayerStats stats)
    {
        healthBar.SetValue(stats.health, false);

        if (!mouseWithItemHovered)
        {
            healthBar.transform.Find("HealthBarFill").Find("Value").GetComponent<Text>().text = string.Format("{0:0} / {1:0}", stats.health, stats.healthMax);
            healthBar.transform.Find("HealthBarFill").Find("Value").GetComponent<Text>().fontSize = 30;
        }
        else
        {
            if (healthMax - stats.healthMax < 0)
                healthBar.transform.Find("HealthBarFill").Find("Value").GetComponent<Text>().text = string.Format("{0:0} / {1:0} <color=#ea4553>{2:0}</color>", stats.health, stats.healthMax, healthMax - stats.healthMax);

            else if (healthMax - stats.healthMax > 0)
                healthBar.transform.Find("HealthBarFill").Find("Value").GetComponent<Text>().text = string.Format("{0:0} / {1:0} <color=#60d46e>+{2:0}</color>", stats.health, stats.healthMax, healthMax - stats.healthMax);
            
            healthBar.transform.Find("HealthBarFill").Find("Value").GetComponent<Text>().fontSize = 26;
        }
        healthBar.transform.Find("HealthBarFill").Find("RegenValue").GetComponent<Text>().text = string.Format("+{0:0.0} hp/second", stats.healthRegen);
    }

    //USed to set the exp bar's value and its text
    public void SetExpBarsValues(PlayerStats stats)
    {
        expBar.Initialize(stats.expTarget, false, true, stats.exp);
        expBar.SetValue(stats.exp, true);
        expBar.transform.Find("HealthBarFill").GetComponentInChildren<Text>().text = string.Format("{0} / {1}", stats.exp, stats.expTarget);
    }

    // Used to compare stat values between the player stats.
    public void CompareStatValues(PlayerStats stats)
    {

        if(healthMax - stats.healthMax < 0)
            healthBar.transform.Find("HealthBarFill").Find("Value").GetComponent<Text>().text = string.Format("{0:0} / {1:0} <color=#ea4553>{2:0}</color>", stats.health, stats.healthMax, healthMax - stats.healthMax);
        else if (healthMax - stats.healthMax > 0)
            healthBar.transform.Find("HealthBarFill").Find("Value").GetComponent<Text>().text = string.Format("{0:0} / {1:0} <color=#60d46e>+{2:0}</color>", stats.health, stats.healthMax, healthMax - stats.healthMax);

        if (healthMax - stats.healthMax != 0)
            healthBar.transform.Find("HealthBarFill").Find("Value").GetComponent<Text>().fontSize = 26;
        else
            healthBar.transform.Find("HealthBarFill").Find("Value").GetComponent<Text>().fontSize = 30;


        DrawTextPlusStatChange(transform.Find("Armor_Value").GetComponent<Text>(), stats.armor, armor, 0);

        if (armor - stats.armor != 0)
            transform.Find("Armor_Value").GetComponent<Text>().fontSize = 20;
        else
            transform.Find("Armor_Value").GetComponent<Text>().fontSize = 26;

        DrawTextPlusStatChange(transform.Find("FDR_Value").GetComponent<Text>(), stats.flatDamageReduction, flatDamageReduction, 0);

        if (flatDamageReduction - stats.flatDamageReduction != 0)
            transform.Find("FDR_Value").GetComponent<Text>().fontSize = 20;
        else
            transform.Find("FDR_Value").GetComponent<Text>().fontSize = 26;

        float newRightHandWeapon = 0;
        float newLeftHandWeapon = 0;
        float currentRightHandWeapon = 0;
        float currentLeftHandWeapon = 0;

        switch (stats.weaponsToHitWith.Count)
        {
            case 0:
                currentRightHandWeapon = stats.baseDamage * stats.damageIncreaseMultiplier;
                currentLeftHandWeapon = 0;
                break;
            case 1:
                currentRightHandWeapon = stats.weaponsToHitWith[0].baseDamageScaling * stats.baseDamage * stats.damageIncreaseMultiplier;
                currentLeftHandWeapon = 0;
                break;
            case 2:
                if (stats.weaponsToHitWith[0].equippedToRightHand)
                {
                    currentRightHandWeapon = stats.weaponsToHitWith[0].baseDamageScaling * stats.baseDamage * stats.damageIncreaseMultiplier;
                    currentLeftHandWeapon = stats.weaponsToHitWith[1].baseDamageScaling * stats.baseDamage * stats.damageIncreaseMultiplier;
                }
                else
                {
                    currentRightHandWeapon = stats.weaponsToHitWith[1].baseDamageScaling * stats.baseDamage * stats.damageIncreaseMultiplier;
                    currentLeftHandWeapon = stats.weaponsToHitWith[0].baseDamageScaling * stats.baseDamage * stats.damageIncreaseMultiplier;
                }
                break;
            default:
                break;
        }

        //Debug.Log("the new weapons are being evaluated: their count is: " + weaponsToHitWith.Count);
        switch (weaponsToHitWith.Count)
        {
            case 0:
                newRightHandWeapon = stats.baseDamage * damageIncreaseMultiplier;
                newLeftHandWeapon = 0;
                break;
            case 1:
                newRightHandWeapon = weaponsToHitWith[0].baseDamageScaling * stats.baseDamage * damageIncreaseMultiplier;
                newLeftHandWeapon = 0;
                break;
            case 2:
                if (weaponsToHitWith[0].equippedToRightHand)
                {
                    newRightHandWeapon = weaponsToHitWith[0].baseDamageScaling * stats.baseDamage * damageIncreaseMultiplier;
                    newLeftHandWeapon = weaponsToHitWith[1].baseDamageScaling * stats.baseDamage * damageIncreaseMultiplier;
                }
                else
                {
                    newRightHandWeapon = weaponsToHitWith[1].baseDamageScaling * stats.baseDamage * damageIncreaseMultiplier;
                    newLeftHandWeapon = weaponsToHitWith[0].baseDamageScaling * stats.baseDamage * damageIncreaseMultiplier;
                }
                break;
            default:
                break;
        }
        //Debug.Log("new left to current left is: " + newLeftHandWeapon + " | " + currentLeftHandWeapon);
       // Debug.Log("new right to current left is: " + newRightHandWeapon + " | " + currentRightHandWeapon);
        DrawTextPlusStatChange(transform.Find("AttackDamageRightHand_Value").GetComponent<Text>(), currentRightHandWeapon, newRightHandWeapon, 0);
        DrawTextPlusStatChange(transform.Find("AttackDamageLeftHand_Value").GetComponent<Text>(), currentLeftHandWeapon, newLeftHandWeapon, 0);

        if (currentRightHandWeapon - newRightHandWeapon != 0)
            transform.Find("AttackDamageRightHand_Value").GetComponent<Text>().fontSize = 20;
        else
            transform.Find("AttackDamageRightHand_Value").GetComponent<Text>().fontSize = 26;
        if (currentLeftHandWeapon - newLeftHandWeapon != 0)
            transform.Find("AttackDamageLeftHand_Value").GetComponent<Text>().fontSize = 20;
        else
            transform.Find("AttackDamageLeftHand_Value").GetComponent<Text>().fontSize = 26;

        DrawTextPlusStatChangePercentage(transform.Find("AttackSpeed_Value").GetComponent<Text>(), stats.attackSpeed * 100f, attackSpeed * 100f);
        DrawTextPlusStatChangePercentage(transform.Find("CooldownReduction_Value").GetComponent<Text>(), stats.cooldownReduction * 100f, cooldownReduction * 100f);
        if (attackSpeed - stats.attackSpeed != 0)
            transform.Find("AttackSpeed_Value").GetComponent<Text>().fontSize = 14;
        else
            transform.Find("AttackSpeed_Value").GetComponent<Text>().fontSize = 26;
        if (cooldownReduction - stats.cooldownReduction != 0)
            transform.Find("CooldownReduction_Value").GetComponent<Text>().fontSize = 14;
        else
            transform.Find("CooldownReduction_Value").GetComponent<Text>().fontSize = 26;

        DrawTextPlusStatChangePercentage(transform.Find("CritChance_Value").GetComponent<Text>(), stats.critChance * 100f, critChance * 100f);
        DrawTextPlusStatChangePercentage(transform.Find("CritDamage_Value").GetComponent<Text>(), stats.critDamageMultiplier * 100f, critDamage * 100f);
        if (critChance - stats.critChance != 0)
            transform.Find("CritChance_Value").GetComponent<Text>().fontSize = 14;
        else
            transform.Find("CritChance_Value").GetComponent<Text>().fontSize = 26;
        if (critDamage - stats.critDamageMultiplier != 0)
            transform.Find("CritDamage_Value").GetComponent<Text>().fontSize = 14;
        else
            transform.Find("CritDamage_Value").GetComponent<Text>().fontSize = 26;

        DrawTextPlusStatChangePercentage(transform.Find("Movespeed_Value").GetComponent<Text>(), stats.movespeedPercentMultiplier * 100f, movespeed * 100f);
        DrawTextPlusStatChangePercentage(transform.Find("Luck_Value").GetComponent<Text>(), stats.luck, luck);
        if (movespeed - stats.movespeedPercentMultiplier != 0)
            transform.Find("Movespeed_Value").GetComponent<Text>().fontSize = 14;
        else
            transform.Find("Movespeed_Value").GetComponent<Text>().fontSize = 26;
        if (luck - stats.luck != 0)
            transform.Find("Luck_Value").GetComponent<Text>().fontSize = 14;
        else
            transform.Find("Luck_Value").GetComponent<Text>().fontSize = 26;

        DrawTextPlusStatChange(transform.Find("Jumps_Value").GetComponent<Text>(), stats.jumps, jumps, 0);

        if (jumps - stats.jumps != 0)
            transform.Find("Jumps_Value").GetComponent<Text>().fontSize = 20;
        else
            transform.Find("Jumps_Value").GetComponent<Text>().fontSize = 26;

        DrawTextPlusStatChangePercentage(transform.Find("AsleepResistance_Value").GetComponent<Text>(), stats.sleepResistance * 100, asleepResistance * 100);
        DrawTextPlusStatChangePercentage(transform.Find("StunResistance_Value").GetComponent<Text>(), stats.stunResistance * 100, stunResistance * 100);
        DrawTextPlusStatChangePercentage(transform.Find("KnockbackResistance_Value").GetComponent<Text>(), stats.knockbackResistance * 100, knockbackResistance * 100);
        DrawTextPlusStatChangePercentage(transform.Find("BleedResistance_Value").GetComponent<Text>(), stats.bleedResistance * 100, bleedResistance * 100);
        DrawTextPlusStatChangePercentage(transform.Find("PoisonResistance_Value").GetComponent<Text>(), stats.poisonResistance * 100, poisonResistance * 100);
        DrawTextPlusStatChangePercentage(transform.Find("AflameResistance_Value").GetComponent<Text>(), stats.aflameResistance * 100, aflameResistance * 100);
        DrawTextPlusStatChangePercentage(transform.Find("FrostbiteResistance_Value").GetComponent<Text>(), stats.frostbiteResistance * 100, frostbiteResistance * 100);
        DrawTextPlusStatChangePercentage(transform.Find("OverchargeResistance_Value").GetComponent<Text>(), stats.overchargeResistance * 100, overchargeResistance * 100);
        DrawTextPlusStatChangePercentage(transform.Find("NatureResistance_Value").GetComponent<Text>(), stats.overgrowthResistance * 100, overgrowthResistance * 100);
        DrawTextPlusStatChangePercentage(transform.Find("SunderedResistance_Value").GetComponent<Text>(), stats.sunderResistance * 100, sunderResistance * 100);
        DrawTextPlusStatChangePercentage(transform.Find("WindshearResistance_Value").GetComponent<Text>(), stats.windshearResistance * 100, windshearResistance * 100);

        if (asleepResistance - stats.sleepResistance != 0)
            transform.Find("AsleepResistance_Value").GetComponent<Text>().fontSize = 16;
        else
            transform.Find("AsleepResistance_Value").GetComponent<Text>().fontSize = 24;

        if (stunResistance - stats.stunResistance != 0)
            transform.Find("StunResistance_Value").GetComponent<Text>().fontSize = 16;
        else
            transform.Find("StunResistance_Value").GetComponent<Text>().fontSize = 24;

        if (knockbackResistance - stats.knockbackResistance != 0)
            transform.Find("KnockbackResistance_Value").GetComponent<Text>().fontSize = 16;
        else
            transform.Find("KnockbackResistance_Value").GetComponent<Text>().fontSize = 24;

        if (bleedResistance - stats.bleedResistance != 0)
            transform.Find("BleedResistance_Value").GetComponent<Text>().fontSize = 16;
        else
            transform.Find("BleedResistance_Value").GetComponent<Text>().fontSize = 24;

        if (poisonResistance - stats.poisonResistance != 0)
            transform.Find("PoisonResistance_Value").GetComponent<Text>().fontSize = 16;
        else
            transform.Find("PoisonResistance_Value").GetComponent<Text>().fontSize = 24;

        if (aflameResistance - stats.aflameResistance != 0)
            transform.Find("AflameResistance_Value").GetComponent<Text>().fontSize = 16;
        else
            transform.Find("AflameResistance_Value").GetComponent<Text>().fontSize = 24;

        if (frostbiteResistance - stats.frostbiteResistance != 0)
            transform.Find("FrostbiteResistance_Value").GetComponent<Text>().fontSize = 16;
        else
            transform.Find("FrostbiteResistance_Value").GetComponent<Text>().fontSize = 24;

        if (overchargeResistance - stats.overchargeResistance != 0)
            transform.Find("OverchargeResistance_Value").GetComponent<Text>().fontSize = 16;
        else
            transform.Find("OverchargeResistance_Value").GetComponent<Text>().fontSize = 24;

        if (overgrowthResistance - stats.overgrowthResistance != 0)
            transform.Find("NatureResistance_Value").GetComponent<Text>().fontSize = 16;
        else
            transform.Find("NatureResistance_Value").GetComponent<Text>().fontSize = 24;

        if (sunderResistance - stats.sunderResistance != 0)
            transform.Find("SunderedResistance_Value").GetComponent<Text>().fontSize = 16;
        else
            transform.Find("SunderedResistance_Value").GetComponent<Text>().fontSize = 24;

        if (windshearResistance - stats.windshearResistance != 0)
            transform.Find("WindshearResistance_Value").GetComponent<Text>().fontSize = 16;
        else
            transform.Find("WindshearResistance_Value").GetComponent<Text>().fontSize = 24;
    }

    // Used to draw the stat changes of percentage based Calculations
    public void DrawTextPlusStatChangePercentage(Text text, float value, float newValue)
    {
        if (newValue - value < 0)
            text.text = string.Format("{0:0}%<color=#ea4553>{1:0}%</color>", value, newValue - value);
        else if (newValue - value > 0)
            text.text = string.Format("{0:0}%<color=#60d46e>+{1:0}%</color>", value, newValue - value);
        else
            text.text = string.Format("{0:0}%", value);
    }

    // USed to check if our value is above or below the original, and to write it down as such.
    public void DrawTextPlusStatChange(Text text,float value, float newValue, int decimalPlaces)
    {
        if (decimalPlaces == 0)
        {
            if (newValue - value < 0)
                text.text = string.Format("{0:0}<color=#ea4553>{1:0}</color>", value, (newValue - value));
            else if (newValue - value > 0)
                text.text = string.Format("{0:0}<color=#60d46e>+{1:0}</color>", value, (newValue - value));
            else
                text.text = string.Format("{0:0}", value);
        }
        else if(decimalPlaces == 1)
        {
            if (newValue - value < 0)
                text.text = string.Format("{0:0.0}<color=#ea4553>{1:0.0}</color>", value, (newValue - value));
            else if (newValue - value > 0)
                text.text = string.Format("{0:0.0}<color=#60d46e>+{1:0.0}</color>", value, (newValue - value));
            else
                text.text = string.Format("{0:0.0}", value);
        }
        else if (decimalPlaces == 2)
        {
            if (newValue - value < 0)
                text.text = string.Format("{0:0.00}<color=#ea4553>{1:0.00}</color>", value, (newValue - value));
            else if (newValue - value > 0)
                text.text = string.Format("{0:0.00}<color=#60d46e>+{1:0.00}</color>", value, (newValue - value));
            else
                text.text = string.Format("{0:0.00}", value);
        }
    }

    // USed to check if our value is above or below the original, and to write it down as such.
    public void DrawTextPlusStatChange(Text text, int value, int newValue)
    {
        if (newValue - value < 0)
            text.text = value + "<color=#ea4553>" + (newValue - value) + "</color>";
        else if (newValue - value > 0)
            text.text = value + "<color=#60d46e>+" + (newValue - value) + "</color>";
        else
            text.text = value + "";
    }

    // Used to assign what oru stats would like with the changed stats from the item.
    public void AssignPotentialStats(PlayerStats stats)
    {
        healthMax = stats.healthMax;
        healthRegen = stats.healthRegen;
        armor = stats.armor;
        flatDamageReduction = stats.flatDamageReduction;

        attackSpeed = stats.attackSpeed;
        baseDamage = stats.baseDamage;
        critChance = stats.critChance;
        critDamage = stats.critDamageMultiplier;

        movespeed = stats.movespeedPercentMultiplier;
        jumps = stats.jumps;
        luck = stats.luck;

        aflameResistance = stats.aflameResistance;
        asleepResistance = stats.sleepResistance;
        stunResistance = stats.stunResistance;
        bleedResistance = stats.bleedResistance;
        poisonResistance = stats.poisonResistance;
        frostbiteResistance = stats.frostbiteResistance;
        knockbackResistance = stats.knockbackResistance;
        overchargeResistance = stats.overchargeResistance;
        overgrowthResistance = stats.overgrowthResistance;
        sunderResistance = stats.sunderResistance;
        windshearResistance = stats.windshearResistance;

        damageIncreaseMultiplier = stats.damageIncreaseMultiplier;

        //Debug.Log("we are assigning the weapons tyo hit with here. the count of the one pased in is: " + stats.weaponsToHitWith.Count);
        weaponsToHitWith = new List<Item>();

        foreach (Item item in stats.weaponsToHitWith)
            weaponsToHitWith.Add(item);
        //Debug.Log("we have assigned it to our ours should match now. our is " + weaponsToHitWith.Count + " and should match " + stats.weaponsToHitWith.Count);

        cooldownReduction = 0;

        foreach (float cdr in stats.cooldownReductionSources)
        {
            // grab the amount of perentage remaining.
            float totalAmountToReduce = 1 - cooldownReduction;
            // add cdr to that percentage.
            totalAmountToReduce *= cdr;
            // add it back to total
            cooldownReduction += totalAmountToReduce;
        }
    }

    // Used to update just the money Tooltip
    public void UpdateGoldCounter(PlayerStats stats)
    {
        transform.Find("Money_Value").GetComponent<Text>().text = string.Format("{0:0}", stats.gold);
        transform.Find("Money_Value").GetComponent<Text>().fontSize = 24;

        tooltips[16].text = string.Format("Beautiful wonderful money. Use it to buy better gear or upgrade existing gear. You currently have <color=#FFD605>{0:0} gold.</color>", stats.gold);
    }

    // Used to update all the tooltips.
    public void UpdateTooltips(PlayerStats stats)
    {
        //Debug.Log("Tooltips updated");
        for (int index = 0; index < tooltips.Length; index ++)
        {
            switch (index) 
            {
                case 0:
                    tooltips[index].text = string.Format("You are currently taking <color=#F3FC56>{0:0.0}% reduced damage</color>.",  (1 - (100 / (100 + stats.armor))) * 100);
                    break;
                case 1:
                    switch (stats.weaponsToHitWith.Count)
                    {
                        case 0:
                            tooltips[index].text = string.Format("Your basic attacks with your bare fists deal <color=#ffbe2f>{0:0} physical damage</color> before multipliers and effects.", stats.baseDamage);
                            break;
                        case 1:
                            string weaponDamageAndOnHits = "";
                            switch (stats.weaponsToHitWith[0].damageType)
                            {
                                case HitBox.DamageType.Physical:
                                    weaponDamageAndOnHits = string.Format("<color=#ffbe2f>{0:0} physical damage</color> before multipliers and effects.", stats.baseDamage * stats.weaponsToHitWith[0].baseDamageScaling);
                                    break;
                                case HitBox.DamageType.Fire:
                                    weaponDamageAndOnHits = string.Format("<color=#ff932e>{0:0} fire damage</color> before multipliers and effects, and <color=#ff932e>adds {1:0} aflame stacks</color> per strike.", stats.baseDamage * stats.weaponsToHitWith[0].baseDamageScaling, stats.weaponsToHitWith[0].stacksToAddOnHit);
                                    break;
                                case HitBox.DamageType.Ice:
                                    weaponDamageAndOnHits = string.Format("<color=#5ad9f5>{0:0} ice damage</color> before multipliers and effects, and <color=#5ad9f5>adds {1:0} frostbite stacks</color> per strike.", stats.baseDamage * stats.weaponsToHitWith[0].baseDamageScaling, stats.weaponsToHitWith[0].stacksToAddOnHit);
                                    break;
                                case HitBox.DamageType.Lightning:
                                    weaponDamageAndOnHits = string.Format("<color=#ca65ff>{0:0} lightning damage</color> before multipliers and effects, and <color=#ca65ff>adds {1:0} overcharge stacks</color> per strike.", stats.baseDamage * stats.weaponsToHitWith[0].baseDamageScaling, stats.weaponsToHitWith[0].stacksToAddOnHit);
                                    break;
                                case HitBox.DamageType.Nature:
                                    weaponDamageAndOnHits = string.Format("<color=#4ed477>{0:0} nature damage</color> before multipliers and effects, and <color=#4ed477>adds {1:0} overgrowth stacks</color> per strike.", stats.baseDamage * stats.weaponsToHitWith[0].baseDamageScaling, stats.weaponsToHitWith[0].stacksToAddOnHit);
                                    break;
                                case HitBox.DamageType.Earth:
                                    weaponDamageAndOnHits = string.Format("<color=#b0946c>{0:0} earth damage</color> before multipliers and effects, and <color=#b0946c>adds {1:0} sunder stacks</color> per strike.", stats.baseDamage * stats.weaponsToHitWith[0].baseDamageScaling, stats.weaponsToHitWith[0].stacksToAddOnHit);
                                    break;
                                case HitBox.DamageType.Wind:
                                    weaponDamageAndOnHits = string.Format("<color=#abd1e0>{0:0} wind damage</color> before multipliers and effects, and <color=#abd1e0>adds {1:0} windshear stacks</color> per strike.", stats.baseDamage * stats.weaponsToHitWith[0].baseDamageScaling, stats.weaponsToHitWith[0].stacksToAddOnHit);
                                    break;
                                case HitBox.DamageType.Poison:
                                    weaponDamageAndOnHits = string.Format("<color=#ffbe2f>{0:0} physical damage</color> before multipliers and effects, and <color=#7ece1e>adds {1:0} poison stacks</color> per strike.", stats.baseDamage * stats.weaponsToHitWith[0].baseDamageScaling, stats.weaponsToHitWith[0].stacksToAddOnHit);
                                    break;
                                case HitBox.DamageType.Bleed:
                                    weaponDamageAndOnHits = string.Format("<color=#ffbe2f>{0:0} physical damage</color> before multipliers and effects, and <color=#c0201e>adds {1:0} bleed stacks</color> per strike.", stats.baseDamage * stats.weaponsToHitWith[0].baseDamageScaling, stats.weaponsToHitWith[0].stacksToAddOnHit);
                                    break;
                                default:
                                    break;
                            }

                            tooltips[index].text = "Your basic attacks with your right hand weapon deal " + weaponDamageAndOnHits;
                            break;
                        case 2:
                            if (stats.weaponsToHitWith[0].equippedToRightHand)
                            {
                                string weaponDamageAndOnHitsRight = "";
                                switch (stats.weaponsToHitWith[0].damageType)
                                {
                                    case HitBox.DamageType.Physical:
                                        weaponDamageAndOnHitsRight = string.Format("<color=#ffbe2f>{0:0} physical damage</color> before multipliers and effects.", stats.baseDamage * stats.weaponsToHitWith[0].baseDamageScaling);
                                        break;
                                    case HitBox.DamageType.Fire:
                                        weaponDamageAndOnHitsRight = string.Format("<color=#ff932e>{0:0} fire damage</color> before multipliers and effects, and <color=#ff932e>adds {1:0} aflame stacks</color> per strike.", stats.baseDamage * stats.weaponsToHitWith[0].baseDamageScaling, stats.weaponsToHitWith[0].stacksToAddOnHit);
                                        break;
                                    case HitBox.DamageType.Ice:
                                        weaponDamageAndOnHitsRight = string.Format("<color=#5ad9f5>{0:0} ice damage</color> before multipliers and effects, and <color=#5ad9f5>adds {1:0} frostbite stacks</color> per strike.", stats.baseDamage * stats.weaponsToHitWith[0].baseDamageScaling, stats.weaponsToHitWith[0].stacksToAddOnHit);
                                        break;
                                    case HitBox.DamageType.Lightning:
                                        weaponDamageAndOnHitsRight = string.Format("<color=#ca65ff>{0:0} lightning damage</color> before multipliers and effects, and <color=#ca65ff>adds {1:0} overcharge stacks</color> per strike.", stats.baseDamage * stats.weaponsToHitWith[0].baseDamageScaling, stats.weaponsToHitWith[0].stacksToAddOnHit);
                                        break;
                                    case HitBox.DamageType.Nature:
                                        weaponDamageAndOnHitsRight = string.Format("<color=#4ed477>{0:0} nature damage</color> before multipliers and effects, and <color=#4ed477>adds {1:0} overgrowth stacks</color> per strike.", stats.baseDamage * stats.weaponsToHitWith[0].baseDamageScaling, stats.weaponsToHitWith[0].stacksToAddOnHit);
                                        break;
                                    case HitBox.DamageType.Earth:
                                        weaponDamageAndOnHitsRight = string.Format("<color=#b0946c>{0:0} earth damage</color> before multipliers and effects, and <color=#b0946c>adds {1:0} sunder stacks</color> per strike.", stats.baseDamage * stats.weaponsToHitWith[0].baseDamageScaling, stats.weaponsToHitWith[0].stacksToAddOnHit);
                                        break;
                                    case HitBox.DamageType.Wind:
                                        weaponDamageAndOnHitsRight = string.Format("<color=#abd1e0>{0:0} wind damage</color> before multipliers and effects, and <color=#abd1e0>adds {1:0} windshear stacks</color> per strike.", stats.baseDamage * stats.weaponsToHitWith[0].baseDamageScaling, stats.weaponsToHitWith[0].stacksToAddOnHit);
                                        break;
                                    case HitBox.DamageType.Poison:
                                        weaponDamageAndOnHitsRight = string.Format("<color=#ffbe2f>{0:0} physical damage</color> before multipliers and effects, and <color=#7ece1e>adds {1:0} poison stacks</color> per strike.", stats.baseDamage * stats.weaponsToHitWith[0].baseDamageScaling, stats.weaponsToHitWith[0].stacksToAddOnHit);
                                        break;
                                    case HitBox.DamageType.Bleed:
                                        weaponDamageAndOnHitsRight = string.Format("<color=#ffbe2f>{0:0} physical damage</color> before multipliers and effects, and <color=#c0201e>adds {1:0} bleed stacks</color> per strike.", stats.baseDamage * stats.weaponsToHitWith[0].baseDamageScaling, stats.weaponsToHitWith[0].stacksToAddOnHit);
                                        break;
                                    default:
                                        break;
                                }

                                tooltips[index].text = "Your basic attacks with your right hand weapon deal " + weaponDamageAndOnHitsRight;
                            }
                            else
                            {
                                string weaponDamageAndOnHitsLeft = "";
                                switch (stats.weaponsToHitWith[1].damageType)
                                {
                                    case HitBox.DamageType.Physical:
                                        weaponDamageAndOnHitsLeft = string.Format("<color=#ffbe2f>{0:0} physical damage</color> before multipliers and effects.", stats.baseDamage * stats.weaponsToHitWith[1].baseDamageScaling);
                                        break;
                                    case HitBox.DamageType.Fire:
                                        weaponDamageAndOnHitsLeft = string.Format("<color=#ff932e>{0:0} fire damage</color> before multipliers and effects, and <color=#ff932e>adds {1:0} aflame stacks</color> per strike.", stats.baseDamage * stats.weaponsToHitWith[1].baseDamageScaling, stats.weaponsToHitWith[1].stacksToAddOnHit);
                                        break;
                                    case HitBox.DamageType.Ice:
                                        weaponDamageAndOnHitsLeft = string.Format("<color=#5ad9f5>{0:0} ice damage</color> before multipliers and effects, and <color=#5ad9f5>adds {1:0} frostbite stacks</color> per strike.", stats.baseDamage * stats.weaponsToHitWith[1].baseDamageScaling, stats.weaponsToHitWith[1].stacksToAddOnHit);
                                        break;
                                    case HitBox.DamageType.Lightning:
                                        weaponDamageAndOnHitsLeft = string.Format("<color=#ca65ff>{0:0} lightning damage</color> before multipliers and effects, and <color=#ca65ff>adds {1:0} overcharge stacks</color> per strike.", stats.baseDamage * stats.weaponsToHitWith[1].baseDamageScaling, stats.weaponsToHitWith[1].stacksToAddOnHit);
                                        break;
                                    case HitBox.DamageType.Nature:
                                        weaponDamageAndOnHitsLeft = string.Format("<color=#4ed477>{0:0} nature damage</color> before multipliers and effects, and <color=#4ed477>adds {1:0} overgrowth stacks</color> per strike.", stats.baseDamage * stats.weaponsToHitWith[1].baseDamageScaling, stats.weaponsToHitWith[1].stacksToAddOnHit);
                                        break;
                                    case HitBox.DamageType.Earth:
                                        weaponDamageAndOnHitsLeft = string.Format("<color=#b0946c>{0:0} earth damage</color> before multipliers and effects, and <color=#b0946c>adds {1:0} sunder stacks</color> per strike.", stats.baseDamage * stats.weaponsToHitWith[1].baseDamageScaling, stats.weaponsToHitWith[1].stacksToAddOnHit);
                                        break;
                                    case HitBox.DamageType.Wind:
                                        weaponDamageAndOnHitsLeft = string.Format("<color=#abd1e0>{0:0} wind damage</color> before multipliers and effects, and <color=#abd1e0>adds {1:0} windshear stacks</color> per strike.", stats.baseDamage * stats.weaponsToHitWith[1].baseDamageScaling, stats.weaponsToHitWith[1].stacksToAddOnHit);
                                        break;
                                    case HitBox.DamageType.Poison:
                                        weaponDamageAndOnHitsLeft = string.Format("<color=#ffbe2f>{0:0} physical damage</color> before multipliers and effects, and <color=#7ece1e>adds {1:0} poison stacks</color> per strike.", stats.baseDamage * stats.weaponsToHitWith[1].baseDamageScaling, stats.weaponsToHitWith[1].stacksToAddOnHit);
                                        break;
                                    case HitBox.DamageType.Bleed:
                                        weaponDamageAndOnHitsLeft = string.Format("<color=#ffbe2f>{0:0} physical damage</color> before multipliers and effects, and <color=#c0201e>adds {1:0} bleed stacks</color> per strike.", stats.baseDamage * stats.weaponsToHitWith[1].baseDamageScaling, stats.weaponsToHitWith[1].stacksToAddOnHit);
                                        break;
                                    default:
                                        break;
                                }

                                tooltips[index].text = "Your basic attacks with your right hand weapon deal " + weaponDamageAndOnHitsLeft;

                            }
                            break;
                        default:
                            break;
                    }
                    break;
                case 2:
                    switch (stats.weaponsToHitWith.Count)
                    {
                        case 0:
                            tooltips[index].text = string.Format("You do not have a weapon equipped in your left hand slot.", stats.baseDamage);
                            break;
                        case 1:
                            tooltips[index].text = string.Format("You do not have a weapon equipped in your left hand slot.", stats.baseDamage);
                            break;
                        case 2:
                            if (stats.weaponsToHitWith[1].equippedToRightHand)
                            {
                                string weaponDamageAndOnHitsRight = "";
                                switch (stats.weaponsToHitWith[0].damageType)
                                {
                                    case HitBox.DamageType.Physical:
                                        weaponDamageAndOnHitsRight = string.Format("<color=#ffbe2f>{0:0} physical damage</color> before multipliers and effects.", stats.baseDamage * stats.weaponsToHitWith[0].baseDamageScaling);
                                        break;
                                    case HitBox.DamageType.Fire:
                                        weaponDamageAndOnHitsRight = string.Format("<color=#ff932e>{0:0} fire damage</color> before multipliers and effects, and <color=#ff932e>adds {1:0} aflame stacks</color> per strike.", stats.baseDamage * stats.weaponsToHitWith[0].baseDamageScaling, stats.weaponsToHitWith[0].stacksToAddOnHit);
                                        break;
                                    case HitBox.DamageType.Ice:
                                        weaponDamageAndOnHitsRight = string.Format("<color=#5ad9f5>{0:0} ice damage</color> before multipliers and effects, and <color=#5ad9f5>adds {1:0} frostbite stacks</color> per strike.", stats.baseDamage * stats.weaponsToHitWith[0].baseDamageScaling, stats.weaponsToHitWith[0].stacksToAddOnHit);
                                        break;
                                    case HitBox.DamageType.Lightning:
                                        weaponDamageAndOnHitsRight = string.Format("<color=#ca65ff>{0:0} lightning damage</color> before multipliers and effects, and <color=#ca65ff>adds {1:0} overcharge stacks</color> per strike.", stats.baseDamage * stats.weaponsToHitWith[0].baseDamageScaling, stats.weaponsToHitWith[0].stacksToAddOnHit);
                                        break;
                                    case HitBox.DamageType.Nature:
                                        weaponDamageAndOnHitsRight = string.Format("<color=#4ed477>{0:0} nature damage</color> before multipliers and effects, and <color=#4ed477>adds {1:0} overgrowth stacks</color> per strike.", stats.baseDamage * stats.weaponsToHitWith[0].baseDamageScaling, stats.weaponsToHitWith[0].stacksToAddOnHit);
                                        break;
                                    case HitBox.DamageType.Earth:
                                        weaponDamageAndOnHitsRight = string.Format("<color=#b0946c>{0:0} earth damage</color> before multipliers and effects, and <color=#b0946c>adds {1:0} sunder stacks</color> per strike.", stats.baseDamage * stats.weaponsToHitWith[0].baseDamageScaling, stats.weaponsToHitWith[0].stacksToAddOnHit);
                                        break;
                                    case HitBox.DamageType.Wind:
                                        weaponDamageAndOnHitsRight = string.Format("<color=#abd1e0>{0:0} wind damage</color> before multipliers and effects, and <color=#abd1e0>adds {1:0} windshear stacks</color> per strike.", stats.baseDamage * stats.weaponsToHitWith[0].baseDamageScaling, stats.weaponsToHitWith[0].stacksToAddOnHit);
                                        break;
                                    case HitBox.DamageType.Poison:
                                        weaponDamageAndOnHitsRight = string.Format("<color=#ffbe2f>{0:0} physical damage</color> before multipliers and effects, and <color=#7ece1e>adds {1:0} poison stacks</color> per strike.", stats.baseDamage * stats.weaponsToHitWith[0].baseDamageScaling, stats.weaponsToHitWith[0].stacksToAddOnHit);
                                        break;
                                    case HitBox.DamageType.Bleed:
                                        weaponDamageAndOnHitsRight = string.Format("<color=#ffbe2f>{0:0} physical damage</color> before multipliers and effects, and <color=#c0201e>adds {1:0} bleed stacks</color> per strike.", stats.baseDamage * stats.weaponsToHitWith[0].baseDamageScaling, stats.weaponsToHitWith[0].stacksToAddOnHit);
                                        break;
                                    default:
                                        break;
                                }

                                tooltips[index].text = "Your basic attacks with your right hand weapon deal " + weaponDamageAndOnHitsRight;
                            }
                            else
                            {
                                string weaponDamageAndOnHitsLeft = "";
                                switch (stats.weaponsToHitWith[1].damageType)
                                {
                                    case HitBox.DamageType.Physical:
                                        weaponDamageAndOnHitsLeft = string.Format("<color=#ffbe2f>{0:0} physical damage</color> before multipliers and effects.", stats.baseDamage * stats.weaponsToHitWith[1].baseDamageScaling);
                                        break;
                                    case HitBox.DamageType.Fire:
                                        weaponDamageAndOnHitsLeft = string.Format("<color=#ff932e>{0:0} fire damage</color> before multipliers and effects, and <color=#ff932e>adds {1:0} aflame stacks</color> per strike.", stats.baseDamage * stats.weaponsToHitWith[1].baseDamageScaling, stats.weaponsToHitWith[1].stacksToAddOnHit);
                                        break;
                                    case HitBox.DamageType.Ice:
                                        weaponDamageAndOnHitsLeft = string.Format("<color=#5ad9f5>{0:0} ice damage</color> before multipliers and effects, and <color=#5ad9f5>adds {1:0} frostbite stacks</color> per strike.", stats.baseDamage * stats.weaponsToHitWith[1].baseDamageScaling, stats.weaponsToHitWith[1].stacksToAddOnHit);
                                        break;
                                    case HitBox.DamageType.Lightning:
                                        weaponDamageAndOnHitsLeft = string.Format("<color=#ca65ff>{0:0} lightning damage</color> before multipliers and effects, and <color=#ca65ff>adds {1:0} overcharge stacks</color> per strike.", stats.baseDamage * stats.weaponsToHitWith[1].baseDamageScaling, stats.weaponsToHitWith[1].stacksToAddOnHit);
                                        break;
                                    case HitBox.DamageType.Nature:
                                        weaponDamageAndOnHitsLeft = string.Format("<color=#4ed477>{0:0} nature damage</color> before multipliers and effects, and <color=#4ed477>adds {1:0} overgrowth stacks</color> per strike.", stats.baseDamage * stats.weaponsToHitWith[1].baseDamageScaling, stats.weaponsToHitWith[1].stacksToAddOnHit);
                                        break;
                                    case HitBox.DamageType.Earth:
                                        weaponDamageAndOnHitsLeft = string.Format("<color=#b0946c>{0:0} earth damage</color> before multipliers and effects, and <color=#b0946c>adds {1:0} sunder stacks</color> per strike.", stats.baseDamage * stats.weaponsToHitWith[1].baseDamageScaling, stats.weaponsToHitWith[1].stacksToAddOnHit);
                                        break;
                                    case HitBox.DamageType.Wind:
                                        weaponDamageAndOnHitsLeft = string.Format("<color=#abd1e0>{0:0} wind damage</color> before multipliers and effects, and <color=#abd1e0>adds {1:0} windshear stacks</color> per strike.", stats.baseDamage * stats.weaponsToHitWith[1].baseDamageScaling, stats.weaponsToHitWith[1].stacksToAddOnHit);
                                        break;
                                    case HitBox.DamageType.Poison:
                                        weaponDamageAndOnHitsLeft = string.Format("<color=#ffbe2f>{0:0} physical damage</color> before multipliers and effects, and <color=#7ece1e>adds {1:0} poison stacks</color> per strike.", stats.baseDamage * stats.weaponsToHitWith[1].baseDamageScaling, stats.weaponsToHitWith[1].stacksToAddOnHit);
                                        break;
                                    case HitBox.DamageType.Bleed:
                                        weaponDamageAndOnHitsLeft = string.Format("<color=#ffbe2f>{0:0} physical damage</color> before multipliers and effects, and <color=#c0201e>adds {1:0} bleed stacks</color> per strike.", stats.baseDamage * stats.weaponsToHitWith[1].baseDamageScaling, stats.weaponsToHitWith[1].stacksToAddOnHit);
                                        break;
                                    default:
                                        break;
                                }

                                tooltips[index].text = "Your basic attacks with your left hand weapon deal " + weaponDamageAndOnHitsLeft;

                            }
                            break;
                        default:
                            break;
                    }
                    break;
                case 3:
                    tooltips[index].text = string.Format("The current percent speed at which we <color=#E45B5B>attack</color> and complete ability animations is <color=#E5BE5D>{0:0}%</color>", stats.attackSpeed * 100);
                    break;
                case 4:
                    tooltips[index].text = string.Format("The percentage we've reduced our <color=#5DB1E5>ability cooldowns</color> by is <color=#5DB1E5>{0:0.0}%</color>. Cooldown Reduction scales multiplicatively with itself.", stats.cooldownReduction * 100);
                    break;
                case 5:
                    tooltips[index].text = string.Format("Reduces the chance of receiving the <color=#ff932e>aflame debuff by {0:0.0}%</color>. Being set <color=#ff932e>aflame</color> deals <color=#ff932e>20% base damage as fire damage</color> a stack per second for 10 seconds, with a maximum of 100 stacks. <color=#5DB1E5>Rolling</color> reduces the current amount of <color=#ff932e>aflame</color> stacks by half.", stats.aflameResistance * 100);
                    break;
                case 6:
                    tooltips[index].text = string.Format("Reduces the chance of receiving the <color=#5ad9f5>frostbite debuff</color> and <color=#5ad9f5>frozen debuff by {0:0.0}%. Frostbite</color> lowers <color=#E5BE5D>attack speed</color> and <color=#5DB1E5>movespeed</color> by 1% per stack, and deals <color=#5ad9f5>5% base damage as ice damage</color> per stack per second for 10 seconds, with a maximum of 100 stacks. Being <color=#5ad9f5>frozen</color> makes you unable to move for 5 seconds.", stats.frostbiteResistance * 100);
                    break;
                case 7:
                    tooltips[index].text = string.Format("Reduces the chance of receiving the <color=#ca65ff>overcharge debuff by {0:0.0}%</color>. Striking an enemy with stacks of <color=#ca65ff>overcharge</color> with a non-lightning damage attack consumes the <color=#ca65ff>overcharge</color> stacks to deal a bonus <color=#ca65ff>40% base damage as lightning damage</color> per stack, with a maximum of 100 stacks. <color=#ca65ff>Overcharge</color> stacks last 10 seconds.", stats.overchargeResistance * 100);
                    break;
                case 8:
                    tooltips[index].text = string.Format("Reduces the chance of receiving the <color=#4ed477>overgrowth debuff by {0:0.0}%</color>. Striking an enemy with stacks of <color=#4ed477>overgrowth</color> with a non-nature damage attack consumes the <color=#4ed477>overgrowth</color> stacks to deal a bonus 1% of the targets <color=#ad2a2a>maximum health</color> as <color=#4ed477>nature damage</color> per stack, with a maximum of 100 stacks. The maximum damage per stack is capped at 80% base damage. <color=#4ed477>Overgrowth</color> stacks last for 10 seconds.", stats.overgrowthResistance * 100);
                    break;
                case 9:
                    tooltips[index].text = string.Format("Reduces the chance of receiving the <color=#abd1e0>windshear debuff by {0:0.0}%. Windshear</color> <color=#F3FC56>reduces armor by 1%</color> per stack for 10 seconds, with a maximum of 100 stacks.", stats.windshearResistance * 100);
                    break;
                case 10:
                    tooltips[index].text = string.Format("Reduces the chance of receiving the <color=#b0946c>sunder debuff by {0:0.0}%. Sunder</color> reduces <color=#f93dff>resistances</color> to debuffs by 1% per stack for 10 seconds, with a maximum of 100 stacks.", stats.sunderResistance * 100);
                    break;
                case 11:
                    tooltips[index].text = string.Format("Reduces the chance of receiving the <color=#c0201e>bleed debuff by {0:0.0}%. Bleed</color> deals <color=#c0201e>30% base damage</color> a second a stack for 5 seconds, with a maximum of 100 stacks. <color=#ffbe2f>Attacking</color> or <color=#5DB1E5>rolling</color> deals an additional <color=#c0201e>30% base damage</color> a stack to the target", stats.bleedResistance * 100);
                    break;
                case 12:
                    tooltips[index].text = string.Format("Reduces the chance of receiving the <color=#7ece1e>poison debuff by {0:0.0}%. Poison deals 0.1%</color> of the targets <color=#ad2a2a>maximum health</color> per stack per second for 20 seconds, with a maximum of 100 stacks. The minimum damage is equal to <color=#7ece1e>10% base damage</color> per stack. The maximum damage is <color=#7ece1e>500% base damage</color> per stack.", stats.poisonResistance * 100);
                    break;
                case 13:
                    tooltips[index].text = string.Format("Reduces the chance of receiving the <color=#7b6a91>sleep debuff by {0:0.0}%. Sleep</color> renders you unable to take action for 5 seconds. Being struck removes the <color=#7b6a91>sleep debuff</color> but increases the <color=#ffbe2f>damage of the attack that awoke you by 200%</color>", stats.sleepResistance * 100);
                    break;
                case 14:
                    tooltips[index].text = string.Format("Reduces the chance of receiving the <color=#d9b529>stun debuff by {0:0.0}%. Stun</color> renders you unable to take action for 2 seconds.", stats.stunResistance * 100);
                    break;
                case 15:
                    tooltips[index].text = string.Format("Reduces the chance of receiving the <color=#7b8adc>knockback debuff by {0:0.0}%. Knockback</color> launches you in a specified direction and renders you unable to take action.", stats.knockbackResistance * 100);
                    break;
                case 16:
                    tooltips[index].text = string.Format("Beautiful wonderful money. Use it to buy better gear or upgrade existing gear. You currently have <color=#FFD605>{0:0} gold.</color>", stats.gold);
                    break;
                case 17:
                    float minDamage = 0;
                    if ((1 - (100 / (100 + stats.armor))) > 0)
                    {
                        //Debug.Log(string.Format("The percent damage reduction is {0}, the damage percentage we take is: {1}, the multiplier to the FDR is {2}, resulting in {3}", 1 - (100f / (100f + stats.armor)), 100f / (100f + stats.armor), 1f / (100f / (100f + stats.armor)), stats.flatDamageReduction * (1f / (100f / (100 + stats.armor)))));
                        minDamage = stats.flatDamageReduction * (1f / (100f / (100 + stats.armor)));
                    }
                    tooltips[index].text = string.Format("<color=#554A73>Flat Damage reduction.</color> Reduces all damage taken, except for true damage, by {0:0}. Attacks must deal a pre-mitigation minimum of {1:0.0} damage for you to recieve any damage.", stats.flatDamageReduction, minDamage);
                    break;
                case 18:
                    tooltips[index].text = string.Format("The percentage chance you have to <color=#BE2020>critically strike,</color> dealing bonus damage based on you critical damage. <color=#BE2020>{0:0}% of attacks will currently critically strike.</color>", stats.critChance * 100);
                    break;
                case 19:
                    tooltips[index].text = string.Format("The percentage of bonus damage your attacks and spells will deal when you critically strike. <color=#BE2020>Critically striking makes the attack deal {0:0}% of it's original damage.</color>", stats.critDamageMultiplier * 100);
                    break;
                case 20:
                    tooltips[index].text = string.Format("Your current speed multiplier. Currently you will move <color=#5DB1E5>{0:0}% of your base movespeed.</color> Sprinting increase your speed by an <color=#5DB1E5>additional 75%.</color>", stats.movespeedPercentMultiplier * 100);
                    break;
                case 21:
                    tooltips[index].text = string.Format("The number of jumps your player character can perform. Resets upon landing. You can currently <color=#5DB1E5>jump {0:0} times.</color> The minimum amount of jumps will always be 1.", stats.jumps );
                    break;
                case 22:
                    tooltips[index].text = string.Format("The bonus percent chance you have of recieving an item upgrade when opening a chest. Currently, you have a <color=#289F4F>{0:0}% chance of getting an item upgrade.</color>", stats.luck);
                    break;
                default:
                    break;
            }
        }
    }
}
