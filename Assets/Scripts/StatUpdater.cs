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
    float poise;
    float weaponBaseHit;
    float weaponMaxHit;
    float vitMod;
    float strMod;
    float dexMod;
    float spdMod;
    float intMod;
    float wisMod;
    float chaMod;
    float attackSpeed;
    float stagger;
    float critChance;
    float critMod;
    float aflameResistance;
    float asleepResistance;
    float stunResistance;
    float curseResistance;
    float bleedResistance;
    float poisonResistance;
    float corrosionResistance;
    float frostbiteResistance;
    float knockbackResistance;
    int weaponCount;

    // This method is called at the start of the game to set up all the player's stats. It is also called whenever a player switches gear or the stats would ahve to change.
    public void SetStatValues(PlayerStats stats)
    {
        transform.Find("Playername").GetComponent<Text>().text = stats.playerName + " the " + stats.playerTitle;
        transform.Find("Level_Value").GetComponent<Text>().text = stats.level + "";
        transform.Find("EXP_Value").GetComponent<Text>().text = stats.exp + " / " + stats.expTarget;

        transform.Find("Vitality_Value").GetComponent<Text>().text = stats.Vit + "";
        transform.Find("Strength_Value").GetComponent<Text>().text = stats.Str + "";
        transform.Find("Dexterity_Value").GetComponent<Text>().text = stats.Dex + "";
        transform.Find("Speed_Value").GetComponent<Text>().text = stats.Spd + "";
        transform.Find("Intelligence_Value").GetComponent<Text>().text = stats.Int + "";
        transform.Find("Wisdom_Value").GetComponent<Text>().text = stats.Wis + "";
        transform.Find("Charisma_Value").GetComponent<Text>().text = stats.Cha + "";

        transform.Find("Health_Value").GetComponent<Text>().text = string.Format("{0:0}",stats.health) + " / " + stats.healthMax;
        transform.Find("Mana_Value").GetComponent<Text>().text = string.Format("{0:0}", stats.mana) + " / " + stats.manaMax;
        transform.Find("HealthRegen_Value").GetComponent<Text>().text = string.Format("{0:0.0}", stats.healthRegen);
        transform.Find("ManaRegen_Value").GetComponent<Text>().text = string.Format("{0:0.0}", stats.manaRegen);
        transform.Find("Armor_Value").GetComponent<Text>().text = string.Format("{0:0}", stats.armor);
        transform.Find("Resistance_Value").GetComponent<Text>().text = string.Format("{0:0}", stats.magicResist);
        // transform.Find("Poise_Value").GetComponent<Text>().text = string.Format("{0:0.0}", stats.poise);

        float statBasedDamaged = stats.Str * (stats.weaponStrScaling + stats.weaponBonusStrScaling) + stats.Dex * (stats.weaponDexScaling + stats.weaponBonusDexScaling) + stats.Vit * stats.weaponVitScaling + stats.Spd * stats.weaponSpdScaling
                + stats.Int * stats.weaponIntScaling + stats.Wis * stats.weaponWisScaling + stats.Cha * stats.weaponChaScaling;
        float averageDamage = stats.weaponHitbase + stats.weaponBonusHitBase + (stats.weaponHitMax + stats.weaponBonusHitMax) / 2 + statBasedDamaged;
        transform.Find("AttackDamage_Value").GetComponent<Text>().text = string.Format("{0:0}", (stats.weaponHitbase + stats.weaponBonusHitBase + statBasedDamaged)) + " - " + string.Format("{0:0}", (stats.weaponHitbase +stats.weaponBonusHitBase + stats.weaponHitMax + stats.weaponBonusHitMax + statBasedDamaged));
        transform.Find("AttackSpeed_Value").GetComponent<Text>().text = string.Format("{0:0.00}", stats.attackSpeed);
        // transform.Find("Stagger_Value").GetComponent<Text>().text = string.Format("{0:0.0}", (stats.weaponStaggerBase + stats.Str * stats.weaponStrScaling));
        transform.Find("CritChance_Value").GetComponent<Text>().text = string.Format("{0:0}", (stats.weaponCritChance + stats.weaponBonusCritChance)) + "%";
        transform.Find("CritMod_Value").GetComponent<Text>().text = string.Format("{0:0}", (stats.weaponCritMod + stats.weaponBonusCritMod) * 100) + "%";
        // average attack damage times avergae crit chance and damage times attack per second
        float critValue = stats.weaponCritChance + stats.weaponBonusCritChance;
        if (critValue > 100)
            critValue = 100;
        else if (critValue < 0)
            critValue = 0;

        transform.Find("DPS_Value").GetComponent<Text>().text = string.Format("{0:0.0}", averageDamage *  ( 1 + (critValue / 100 * (stats.weaponCritMod + stats.weaponBonusCritMod))) * stats.attackSpeed);
        // Debug.Log("After equipiing the item, our average damage is " + averageDamage + ", our crit modifier is: " + 1 + (critValue / 100 * (stats.weaponCritMod - stats.weaponHitspeeds.Count))
        //    + ", and our attack speed is: " + stats.attackSpeed);
        // Debug.Log("the weapon hit base is: " + stats.weaponHitbase + ", the weapon hit min and max are " + stats.weaponHitMin + " | " + stats.weaponHitMax + ", and the stats based dmg is " + statBasedDamaged);

        AfflictionManager am = stats.GetComponent<AfflictionManager>();
        transform.Find("AflameResistance_Value").GetComponent<Text>().text = string.Format("{0:0}", am.aflameResist * 100 + "%");
        transform.Find("AsleepResistance_Value").GetComponent<Text>().text = string.Format("{0:0}", am.sleepResist * 100 + "%");
        transform.Find("StunResistance_Value").GetComponent<Text>().text = string.Format("{0:0}", am.stunResist * 100 + "%");
        transform.Find("CurseResistance_Value").GetComponent<Text>().text = string.Format("{0:0}", am.curseResist * 100 + "%");
        transform.Find("BleedResistance_Value").GetComponent<Text>().text = string.Format("{0:0}", am.bleedResist * 100 + "%");
        transform.Find("PoisonResistance_Value").GetComponent<Text>().text = string.Format("{0:0}", am.poisonResist * 100 + "%");
        transform.Find("CorrosionResistance_Value").GetComponent<Text>().text = string.Format("{0:0}", am.corrosionResist * 100 + "%");
        transform.Find("FrostbiteResistance_Value").GetComponent<Text>().text = string.Format("{0:0}", am.frostbiteResist * 100 + "%");
        transform.Find("KnockbackResistance_Value").GetComponent<Text>().text = string.Format("{0:0}", am.knockBackResist * 100 + "%");
    }

    // This method is used to update the health and mana values of the player.
    public void SetHealthManaValues(PlayerStats stats)
    {
        transform.Find("Health_Value").GetComponent<Text>().text = string.Format("{0:0}", stats.health) + " / " + stats.healthMax;
        transform.Find("Mana_Value").GetComponent<Text>().text = string.Format("{0:0}", stats.mana) + " / " + stats.manaMax;
    }

    // This method is used to update the healthregen and mana regen of the player.
    public void SetHealthManaRegenValues(PlayerStats stats)
    {
        transform.Find("HealthRegen_Value").GetComponent<Text>().text = string.Format("{0:0.0}", stats.healthRegen);
        transform.Find("ManaRegen_Value").GetComponent<Text>().text = string.Format("{0:0.0}", stats.manaRegen);
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
        // DrawTextPlusStatChange(transform.Find("Poise_Value").GetComponent<Text>(), stats.poise, poise, 1);

        float oldStatBasedDamaged = stats.Str * (stats.weaponStrScaling + stats.weaponBonusStrScaling) + stats.Dex * (stats.weaponDexScaling + stats.weaponBonusDexScaling) + stats.Vit * stats.weaponVitScaling + stats.Spd * stats.weaponSpdScaling
                + stats.Int * stats.weaponIntScaling + stats.Wis * stats.weaponWisScaling + stats.Cha * stats.weaponChaScaling;
        float oldAverageDamage = stats.weaponHitbase + stats.weaponBonusHitBase + (stats.weaponBonusHitMax + stats.weaponHitMax) / 2 + oldStatBasedDamaged;

        float newStatBasedDamaged = Str * strMod + Dex * dexMod + Vit * vitMod + Spd * spdMod + Int * intMod + Wis * wisMod + Cha * chaMod;
        float newAverageDamage = weaponBaseHit + weaponMaxHit / 2 + newStatBasedDamaged;
        float oldUpperBound = stats.weaponHitbase + stats.weaponBonusHitBase + stats.weaponHitMax + stats.weaponBonusHitMax + oldStatBasedDamaged;
        float oldLowerBound = stats.weaponHitbase + stats.weaponBonusHitBase + oldStatBasedDamaged;
        float newUpperBound = weaponBaseHit + weaponMaxHit + newStatBasedDamaged;
        float newLowerBound = weaponBaseHit + newStatBasedDamaged;
        string lowerBound = "";
        string upperBound = "";

        if (newUpperBound - oldUpperBound < 0)
            upperBound = string.Format("{0:0}<color=red>{1:0}</color>", oldUpperBound, newUpperBound - oldUpperBound);
        else if (newUpperBound - oldUpperBound > 0)
            upperBound = string.Format("{0:0}<color=green>+{1:0}</color>", oldUpperBound, newUpperBound - oldUpperBound);
        else
            upperBound = string.Format("{0:0}", oldUpperBound);

        if (newLowerBound - oldLowerBound < 0)
            lowerBound = string.Format("{0:0}<color=red>{1:0}</color>", oldLowerBound, newLowerBound - oldLowerBound);
        else if (newLowerBound - oldLowerBound > 0)
            lowerBound = string.Format("{0:0}<color=green>+{1:0}</color>", oldLowerBound, newLowerBound - oldLowerBound);
        else
            lowerBound = string.Format("{0:0}", oldLowerBound);

        transform.Find("AttackDamage_Value").GetComponent<Text>().text = lowerBound + " - " + upperBound;

        DrawTextPlusStatChange(transform.Find("AttackSpeed_Value").GetComponent<Text>(), stats.attackSpeed, attackSpeed, 2);
        // DrawTextPlusStatChange(transform.Find("Stagger_Value").GetComponent<Text>(), stats.weaponStaggerBase + stats.Str * stats.weaponStrScaling, stagger + Str * strMod, 1);
        

        float oldCritChance = stats.weaponCritChance + stats.weaponBonusCritChance;
        if ((stats.weaponCritChance + stats.weaponBonusCritChance) > 100)
            oldCritChance = 100;
        else if ((stats.weaponCritChance + stats.weaponBonusCritChance) < 0)
            oldCritChance = 0;
        float newCritChance = critChance;
        if (critChance > 100)
            newCritChance = 100;
        else if (critChance < 0)
            newCritChance = 0;

        DrawTextPlusStatChangePercentage(transform.Find("CritChance_Value").GetComponent<Text>(), oldCritChance, critChance);
        DrawTextPlusStatChangePercentage(transform.Find("CritMod_Value").GetComponent<Text>(), (stats.weaponCritMod + stats.weaponBonusCritMod) * 100, critMod * 100);

        // average attack damage times average crit chance and damage times attack per second
        float oldDPS = oldAverageDamage * (1 + (oldCritChance / 100 * (stats.weaponCritMod + stats.weaponBonusCritMod))) * stats.attackSpeed;
        float newDPS = newAverageDamage * (1 + (newCritChance / 100 * critMod)) * attackSpeed;

        DrawTextPlusStatChange(transform.Find("DPS_Value").GetComponent<Text>(), oldDPS, newDPS, 1);
        // Debug.Log("before equipiing the item, our new average damage is " + newAverageDamage + ", our crit modifier is: " + 1 + (newCritChance / 100 * (critMod - weaponCount))
        //     + ", and our attack speed is: " + attackSpeed);
        // Debug.Log("the weapon hit base is: " + weaponBaseHit + ", the weapon hit min and max are " + weaponMinHit + " | " + weaponMaxHit + ", and the stats based dmg is " + newStatBasedDamaged);
        
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
        // poise = stats.poiseMax;
        weaponBaseHit = stats.weaponHitbase + stats.weaponBonusHitBase;
        weaponMaxHit = stats.weaponHitMax + stats.weaponBonusHitMax;
        attackSpeed = stats.weaponBaseAttackDelay * (1 + 0.025f * Spd + 0.0125f * Dex);
        critChance = stats.weaponCritChance + stats.weaponBonusCritChance;
        critMod = stats.weaponCritMod + stats.weaponBonusCritMod;
        vitMod = stats.weaponVitScaling;
        strMod = stats.weaponStrScaling + stats.weaponBonusStrScaling;
        dexMod = stats.weaponDexScaling + stats.weaponBonusDexScaling;
        spdMod = stats.weaponSpdScaling;
        intMod = stats.weaponIntScaling;
        wisMod = stats.weaponWisScaling;
        chaMod = stats.weaponChaScaling;
        weaponCount = stats.weaponHitspeeds.Count;

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
