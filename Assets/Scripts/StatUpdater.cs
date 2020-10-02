using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatUpdater : MonoBehaviour
{
    int Vit;
    int Str;
    int Dex;
    int Spd;
    int Int;
    int Wis;
    int Cha;
    float healthMax;
    float manaMax;
    float healthRegen;
    float manaRegen;
    float armor;
    float resistance;
    float baseDamageMod;
    float vitMod;
    float strMod;
    float dexMod;
    float spdMod;
    float intMod;
    float wisMod;
    float chaMod;
    float attackSpeed;
    float baseDamage;
    float aflameResistance;
    float asleepResistance;
    float stunResistance;
    float curseResistance;
    float bleedResistance;
    float poisonResistance;
    float corrosionResistance;
    float frostbiteResistance;
    float knockbackResistance;
    public BarManager expBar;
    public BarManager healthBar;
    public BarManager manaBar;
    int weaponCount;

    // This method is called at the start of the game to set up all the player's stats. It is also called whenever a player switches gear or the stats would ahve to change.
    public void SetStatValues(PlayerStats stats)
    {
        transform.Find("Playername").GetComponent<Text>().text = stats.playerName + " the " + stats.playerTitle;
        transform.Find("Level_Value").GetComponent<Text>().text = stats.level + "";
        //transform.Find("EXP_Value").GetComponent<Text>().text = stats.exp + " / " + stats.expTarget;
        SetExpBarsValues(stats);
        SetHealthManaBarValues(stats);

        transform.Find("Vitality_Value").GetComponent<Text>().text = stats.Vit + "";
        transform.Find("Strength_Value").GetComponent<Text>().text = stats.Str + "";
        transform.Find("Dexterity_Value").GetComponent<Text>().text = stats.Dex + "";
        transform.Find("Speed_Value").GetComponent<Text>().text = stats.Spd + "";
        transform.Find("Intelligence_Value").GetComponent<Text>().text = stats.Int + "";
        transform.Find("Wisdom_Value").GetComponent<Text>().text = stats.Wis + "";
        transform.Find("Charisma_Value").GetComponent<Text>().text = stats.Cha + "";

        //transform.Find("Health_Value").GetComponent<Text>().text = string.Format("{0:0}",stats.health) + " / " + stats.healthMax;
        //transform.Find("Mana_Value").GetComponent<Text>().text = string.Format("{0:0}", stats.mana) + " / " + stats.manaMax;
        //transform.Find("HealthRegen_Value").GetComponent<Text>().text = string.Format("{0:0.0}", stats.healthRegen);
        //transform.Find("ManaRegen_Value").GetComponent<Text>().text = string.Format("{0:0.0}", stats.manaRegen);
        transform.Find("Armor_Value").GetComponent<Text>().text = string.Format("{0:0}", stats.armor);
        transform.Find("Resistance_Value").GetComponent<Text>().text = string.Format("{0:0}", stats.magicResist);
        // transform.Find("Poise_Value").GetComponent<Text>().text = string.Format("{0:0.0}", stats.poise);

        transform.Find("AttackDamage_Value").GetComponent<Text>().text = string.Format("{0:0}", stats.baseDamage * (stats.baseDamageScaling + ((float)stats.Str * stats.weaponStrScaling) + ((float)stats.Dex * stats.weaponDexScaling) + ((float)stats.Vit * stats.weaponVitScaling) + ((float)stats.Spd * stats.weaponSpdScaling) + ((float)stats.Int * stats.weaponIntScaling) + ((float)stats.Wis * stats.weaponWisScaling) + ((float)stats.Cha * stats.weaponChaScaling)));
        transform.Find("AttackSpeed_Value").GetComponent<Text>().text = string.Format("{0:0}%", stats.attackSpeed * 100 );
        
        AfflictionManager am = stats.GetComponent<AfflictionManager>();
        transform.Find("AflameResistance_Value").GetComponent<Text>().text = string.Format("{0:0}%", am.aflameResist * 100);
        transform.Find("AsleepResistance_Value").GetComponent<Text>().text = string.Format("{0:0}%", am.sleepResist * 100);
        transform.Find("StunResistance_Value").GetComponent<Text>().text = string.Format("{0:0}%", am.stunResist * 100);
        transform.Find("CurseResistance_Value").GetComponent<Text>().text = string.Format("{0:0}%", am.curseResist * 100);
        transform.Find("BleedResistance_Value").GetComponent<Text>().text = string.Format("{0:0}%", am.bleedResist * 100);
        transform.Find("PoisonResistance_Value").GetComponent<Text>().text = string.Format("{0:0}%", am.poisonResist * 100);
        transform.Find("CorrosionResistance_Value").GetComponent<Text>().text = string.Format("{0:0}%", am.corrosionResist * 100);
        transform.Find("FrostbiteResistance_Value").GetComponent<Text>().text = string.Format("{0:0}%", am.frostbiteResist * 100);
        transform.Find("KnockbackResistance_Value").GetComponent<Text>().text = string.Format("{0:0}%", am.knockBackResist * 100);
    }

    // This method is used to update the health and mana values of the player.
    public void SetHealthManaBarValues(PlayerStats stats)
    {
        healthBar.Initialize(stats.healthMax, true);
        healthBar.SetValue(stats.health);
        healthBar.transform.Find("HealthBarFill").GetComponentInChildren<Text>().text = string.Format("{0:0} / {1:0} | +{2:0.0} hp/5", stats.health, stats.healthMax, stats.healthRegen);

        manaBar.Initialize(stats.manaMax, true);
        manaBar.SetValue(stats.mana);
        manaBar.transform.Find("HealthBarFill").GetComponentInChildren<Text>().text = string.Format("{0:0} / {1:0} | +{2:0.0} mp/5", stats.mana, stats.manaMax, stats.manaRegen);

        //transform.Find("Health_Value").GetComponent<Text>().text = string.Format("{0:0}", stats.health) + " / " + stats.healthMax;
        //transform.Find("Mana_Value").GetComponent<Text>().text = string.Format("{0:0}", stats.mana) + " / " + stats.manaMax;
    }

    public void UpdateHealthManaBarValues(PlayerStats stats)
    {
        healthBar.SetValue(stats.health);
        manaBar.SetValue(stats.mana);

        healthBar.transform.Find("HealthBarFill").GetComponentInChildren<Text>().text = string.Format("{0:0} / {1:0} | +{2:0.0} hp/5", stats.health, stats.healthMax, stats.healthRegen);
        manaBar.transform.Find("HealthBarFill").GetComponentInChildren<Text>().text = string.Format("{0:0} / {1:0} | +{2:0.0} mp/5", stats.mana, stats.manaMax, stats.manaRegen);
    }

    //USed to set the exp bar's value and its text
    public void SetExpBarsValues(PlayerStats stats)
    {
        expBar.Initialize(stats.expTarget, true);
        expBar.SetValue(stats.exp);
        expBar.transform.Find("HealthBarFill").GetComponentInChildren<Text>().text = string.Format("{0} / {1}", stats.exp, stats.expTarget);
    }

    // Used to compare stat values between the player stats.
    public void CompareStatValues(PlayerStats stats)
    {
        DrawTextPlusStatChange(transform.Find("Vitality_Value").GetComponent<Text>(), stats.Vit, Vit);
        DrawTextPlusStatChange(transform.Find("Strength_Value").GetComponent<Text>(), stats.Str, Str);
        DrawTextPlusStatChange(transform.Find("Dexterity_Value").GetComponent<Text>(), stats.Dex, Dex);
        DrawTextPlusStatChange(transform.Find("Speed_Value").GetComponent<Text>(), stats.Spd, Spd);
        DrawTextPlusStatChange(transform.Find("Intelligence_Value").GetComponent<Text>(), stats.Int, Int);
        DrawTextPlusStatChange(transform.Find("Wisdom_Value").GetComponent<Text>(), stats.Wis, Wis);
        DrawTextPlusStatChange(transform.Find("Charisma_Value").GetComponent<Text>(), stats.Cha, Cha);

        if(healthMax - stats.healthMax < 0)
            transform.Find("Health_Value").GetComponent<Text>().text = string.Format("{0:0} / {1:0} <color=red>{2:0}</color>", stats.health, stats.healthMax, healthMax - stats.healthMax);
        else if (healthMax - stats.healthMax > 0)
            transform.Find("Health_Value").GetComponent<Text>().text = string.Format("{0:0} / {1:0} <color=green>+{2:0}</color>", stats.health, stats.healthMax, healthMax - stats.healthMax);
        else
            transform.Find("Health_Value").GetComponent<Text>().text = string.Format("{0:0}", stats.health) + " / " + stats.healthMax;

        if (manaMax - stats.manaMax < 0)
            transform.Find("Mana_Value").GetComponent<Text>().text = string.Format("{0:0} / {1:0}<color=red>{2:0}</color>", stats.mana, stats.manaMax, manaMax - stats.manaMax);
        else if (manaMax - stats.manaMax > 0)
            transform.Find("Mana_Value").GetComponent<Text>().text = string.Format("{0:0} / {1:0}<color=green>+{2:0}</color>", stats.mana, stats.manaMax, manaMax - stats.manaMax);
        else
            transform.Find("Mana_Value").GetComponent<Text>().text = string.Format("{0:0}", stats.mana) + " / " + stats.manaMax;

        DrawTextPlusStatChange(transform.Find("HealthRegen_Value").GetComponent<Text>(), stats.healthRegen, healthRegen, 1);
        DrawTextPlusStatChange(transform.Find("ManaRegen_Value").GetComponent<Text>(), stats.manaRegen, manaRegen, 1);
        DrawTextPlusStatChange(transform.Find("Armor_Value").GetComponent<Text>(), stats.armor, armor, 0);
        DrawTextPlusStatChange(transform.Find("Resistance_Value").GetComponent<Text>(), stats.magicResist, resistance, 0);

        float originalDamage = stats.baseDamage * (stats.baseDamageScaling + ((float)stats.Str * stats.weaponStrScaling) + ((float)stats.Dex * stats.weaponDexScaling) + ((float)stats.Vit * stats.weaponVitScaling) + ((float)stats.Spd * stats.weaponSpdScaling) + ((float)stats.Int * stats.weaponIntScaling) + ((float)stats.Wis * stats.weaponWisScaling) + ((float)stats.Cha * stats.weaponChaScaling));
        float newDamage = baseDamage * (baseDamageMod + ((float)Str * strMod) + ((float)Vit * vitMod) + ((float)Dex * dexMod) + ((float)Spd * spdMod) + ((float)Int * intMod) + ((float)Wis * wisMod) + ((float)Cha * chaMod));

        DrawTextPlusStatChange(transform.Find("AttackDamage_Value").GetComponent<Text>(), originalDamage , newDamage, 0);

        Debug.Log("curernt attack speed| " + stats.attackSpeed + "  proposed attack speed| " + attackSpeed);
        DrawTextPlusStatChangePercentage(transform.Find("AttackSpeed_Value").GetComponent<Text>(), stats.attackSpeed * 100f, attackSpeed * 100f);
        
        AfflictionManager am = stats.GetComponent<AfflictionManager>();
        DrawTextPlusStatChangePercentage(transform.Find("AflameResistance_Value").GetComponent<Text>(), am.aflameResist * 100, aflameResistance * 100);
        DrawTextPlusStatChangePercentage(transform.Find("AsleepResistance_Value").GetComponent<Text>(), am.sleepResist * 100, asleepResistance * 100);
        DrawTextPlusStatChangePercentage(transform.Find("StunResistance_Value").GetComponent<Text>(), am.stunResist * 100, stunResistance * 100);
        DrawTextPlusStatChangePercentage(transform.Find("CurseResistance_Value").GetComponent<Text>(), am.curseResist * 100, curseResistance * 100);
        DrawTextPlusStatChangePercentage(transform.Find("BleedResistance_Value").GetComponent<Text>(), am.bleedResist * 100, bleedResistance * 100);
        DrawTextPlusStatChangePercentage(transform.Find("PoisonResistance_Value").GetComponent<Text>(), am.poisonResist * 100, poisonResistance * 100);
        DrawTextPlusStatChangePercentage(transform.Find("CorrosionResistance_Value").GetComponent<Text>(), am.corrosionResist * 100, corrosionResistance * 100);
        DrawTextPlusStatChangePercentage(transform.Find("FrostbiteResistance_Value").GetComponent<Text>(), am.frostbiteResist * 100, frostbiteResistance * 100);
        DrawTextPlusStatChangePercentage(transform.Find("KnockbackResistance_Value").GetComponent<Text>(), am.knockBackResist * 100, knockbackResistance * 100);
    }

    // Used to draw the stat changes of percentage based Calculations
    public void DrawTextPlusStatChangePercentage(Text text, float value, float newValue)
    {
        if (newValue - value < 0)
            text.text = string.Format("{0:0}%<color=red>{1:0}%</color>", value, newValue - value);
        else if (newValue - value > 0)
            text.text = string.Format("{0:0}%<color=green>+{1:0}%</color>", value, newValue - value);
        else
            text.text = string.Format("{0:0}%", value);
    }

    // USed to check if our value is above or below the original, and to write it down as such.
    public void DrawTextPlusStatChange(Text text,float value, float newValue, int decimalPlaces)
    {
        if (decimalPlaces == 0)
        {
            if (newValue - value < 0)
                text.text = string.Format("{0:0}<color=red>{1:0}</color>", value, (newValue - value));
            else if (newValue - value > 0)
                text.text = string.Format("{0:0}<color=green>+{1:0}</color>", value, (newValue - value));
            else
                text.text = string.Format("{0:0}", value);
        }
        else if(decimalPlaces == 1)
        {
            if (newValue - value < 0)
                text.text = string.Format("{0:0.0}<color=red>{1:0.0}</color>", value, (newValue - value));
            else if (newValue - value > 0)
                text.text = string.Format("{0:0.0}<color=green>+{1:0.0}</color>", value, (newValue - value));
            else
                text.text = string.Format("{0:0.0}", value);
        }
        else if (decimalPlaces == 2)
        {
            if (newValue - value < 0)
                text.text = string.Format("{0:0.00}<color=red>{1:0.00}</color>", value, (newValue - value));
            else if (newValue - value > 0)
                text.text = string.Format("{0:0.00}<color=green>+{1:0.00}</color>", value, (newValue - value));
            else
                text.text = string.Format("{0:0.00}", value);
        }
    }

    // USed to check if our value is above or below the original, and to write it down as such.
    public void DrawTextPlusStatChange(Text text, int value, int newValue)
    {
        if (newValue - value < 0)
            text.text = value + "<color=red>" + (newValue - value) + "</color>";
        else if (newValue - value > 0)
            text.text = value + "<color=green>+" + (newValue - value) + "</color>";
        else
            text.text = value + "";
    }

    // Used to assign what oru stats would like with the changed stats from the item.
    public void AssignPotentialStats(PlayerStats stats)
    {
        Vit = stats.Vit;
        Str = stats.Str;
        Dex = stats.Dex;
        Spd = stats.Spd;
        Int = stats.Int;
        Wis = stats.Wis;
        Cha = stats.Cha;
        healthMax = 20 + 3 * stats.level + 5 * Vit + 2 *  Str + Dex + Spd + Int + Wis + Cha + stats.bonusHealth;
        manaMax = 20 + 3 * stats.level + 5 * Wis + Int + stats.bonusMana;
        healthRegen = Vit * 0.2f + stats.bonusHealthRegen;
        manaRegen = Wis * 0.4f + Int * 0.1f + stats.bonusManaRegen;
        armor = stats.armor;
        resistance = stats.magicResist;

        attackSpeed = 1 + 0.1f * stats.Spd + 0.05f * stats.Dex + stats.bonusAttackSpeed + (stats.weaponAttackSpeed - 1);
        baseDamage = stats.baseDamage;

        baseDamageMod = stats.baseDamageScaling;
        vitMod = stats.weaponVitScaling;
        strMod = stats.weaponStrScaling;
        dexMod = stats.weaponDexScaling;
        spdMod = stats.weaponSpdScaling;
        intMod = stats.weaponIntScaling;
        wisMod = stats.weaponWisScaling;
        chaMod = stats.weaponChaScaling;
        weaponCount = stats.weaponAttackSpeeds.Count;

        AfflictionManager am = stats.GetComponent<AfflictionManager>();
        aflameResistance = am.aflameResist;
        asleepResistance = am.sleepResist;
        stunResistance = am.stunResist;
        curseResistance = am.curseResist;
        bleedResistance = am.bleedResist;
        poisonResistance = am.poisonResist;
        corrosionResistance = am.corrosionResist;
        frostbiteResistance = am.frostbiteResist;
        knockbackResistance = am.knockBackResist;
    }
}
