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
    float cooldownReduction;
    public BarManager expBar;
    public BarManager healthBar;
    public BarManager manaBar;
    int weaponCount;

    public Text[] tooltips;
    public Color[] uiColors;

    public bool mouseWithItemHovered = false;

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
        transform.Find("CooldownReduction_Value").GetComponent<Text>().text = string.Format("{0:0}%", stats.cooldownReduction * 100);

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

        UpdateTooltips(stats);
    }

    // This method is used to update the health and mana values of the player.
    public void SetHealthManaBarValues(PlayerStats stats)
    {
        healthBar.Initialize(stats.healthMax, false, true, stats.health);
        healthBar.SetValue(stats.health, true);
        healthBar.transform.Find("HealthBarFill").Find("Value").GetComponent<Text>().text = string.Format("{0:0} / {1:0}", stats.health, stats.healthMax);
        healthBar.transform.Find("HealthBarFill").Find("RegenValue").GetComponent<Text>().text = string.Format("+{0:0.0} hp/5", stats.healthRegen);

        manaBar.Initialize(stats.manaMax, false, true, stats.health);
        manaBar.SetValue(stats.mana, true);
        manaBar.transform.Find("HealthBarFill").Find("Value").GetComponent<Text>().text = string.Format("{0:0} / {1:0}", stats.mana, stats.manaMax);
        manaBar.transform.Find("HealthBarFill").Find("RegenValue").GetComponent<Text>().text = string.Format("+{0:0.0} mp/5", stats.manaRegen);

        //transform.Find("Health_Value").GetComponent<Text>().text = string.Format("{0:0}", stats.health) + " / " + stats.healthMax;
        //transform.Find("Mana_Value").GetComponent<Text>().text = string.Format("{0:0}", stats.mana) + " / " + stats.manaMax;
    }

    public void UpdateHealthManaBarValues(PlayerStats stats)
    {
        healthBar.SetValue(stats.health, false);
        manaBar.SetValue(stats.mana, false);

        if (!mouseWithItemHovered)
        {
            healthBar.transform.Find("HealthBarFill").Find("Value").GetComponent<Text>().text = string.Format("{0:0} / {1:0}", stats.health, stats.healthMax);
            manaBar.transform.Find("HealthBarFill").Find("Value").GetComponent<Text>().text = string.Format("{0:0} / {1:0}", stats.mana, stats.manaMax);
        }
        else
        {
            if (healthMax - stats.healthMax < 0)
                healthBar.transform.Find("HealthBarFill").Find("Value").GetComponent<Text>().text = string.Format("{0:0} / {1:0} <color=#ea4553>{2:0}</color>", stats.health, stats.healthMax, healthMax - stats.healthMax);
            else if (healthMax - stats.healthMax > 0)
                healthBar.transform.Find("HealthBarFill").Find("Value").GetComponent<Text>().text = string.Format("{0:0} / {1:0} <color=#60d46e>+{2:0}</color>", stats.health, stats.healthMax, healthMax - stats.healthMax);

            if (manaMax - stats.manaMax < 0)
                manaBar.transform.Find("HealthBarFill").Find("Value").GetComponent<Text>().text = string.Format("{0:0} / {1:0} <color=#ea4553>{2:0}</color>", stats.mana, stats.manaMax, manaMax - stats.manaMax);
            else if (manaMax - stats.manaMax > 0)
                manaBar.transform.Find("HealthBarFill").Find("Value").GetComponent<Text>().text = string.Format("{0:0} / {1:0} <color=#60d46e>+{2:0}</color>", stats.mana, stats.manaMax, manaMax - stats.manaMax);
        }
        manaBar.transform.Find("HealthBarFill").Find("RegenValue").GetComponent<Text>().text = string.Format("+{0:0.0} mp/5", stats.manaRegen);
        healthBar.transform.Find("HealthBarFill").Find("RegenValue").GetComponent<Text>().text = string.Format("+{0:0.0} hp/5", stats.healthRegen);
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
        DrawTextPlusStatChange(transform.Find("Vitality_Value").GetComponent<Text>(), stats.Vit, Vit);
        DrawTextPlusStatChange(transform.Find("Strength_Value").GetComponent<Text>(), stats.Str, Str);
        DrawTextPlusStatChange(transform.Find("Dexterity_Value").GetComponent<Text>(), stats.Dex, Dex);
        DrawTextPlusStatChange(transform.Find("Speed_Value").GetComponent<Text>(), stats.Spd, Spd);
        DrawTextPlusStatChange(transform.Find("Intelligence_Value").GetComponent<Text>(), stats.Int, Int);
        DrawTextPlusStatChange(transform.Find("Wisdom_Value").GetComponent<Text>(), stats.Wis, Wis);
        DrawTextPlusStatChange(transform.Find("Charisma_Value").GetComponent<Text>(), stats.Cha, Cha);

        if(healthMax - stats.healthMax < 0)
            healthBar.transform.Find("HealthBarFill").Find("Value").GetComponent<Text>().text = string.Format("{0:0} / {1:0} <color=#ea4553>{2:0}</color>", stats.health, stats.healthMax, healthMax - stats.healthMax);
        else if (healthMax - stats.healthMax > 0)
            healthBar.transform.Find("HealthBarFill").Find("Value").GetComponent<Text>().text = string.Format("{0:0} / {1:0} <color=#60d46e>+{2:0}</color>", stats.health, stats.healthMax, healthMax - stats.healthMax);
       // else
           // transform.Find("Health_Value").GetComponent<Text>().text = string.Format("{0:0}", stats.health) + " / " + stats.healthMax;

        if (manaMax - stats.manaMax < 0)
            manaBar.transform.Find("HealthBarFill").Find("Value").GetComponent<Text>().text = string.Format("{0:0} / {1:0} <color=#ea4553>{2:0}</color>", stats.mana, stats.manaMax, manaMax - stats.manaMax);
        else if (manaMax - stats.manaMax > 0)
            manaBar.transform.Find("HealthBarFill").Find("Value").GetComponent<Text>().text = string.Format("{0:0} / {1:0} <color=#60d46e>+{2:0}</color>", stats.mana, stats.manaMax, manaMax - stats.manaMax);
       // else
        //    transform.Find("Mana_Value").GetComponent<Text>().text = string.Format("{0:0}", stats.mana) + " / " + stats.manaMax;

        //DrawTextPlusStatChange(transform.Find("HealthRegen_Value").GetComponent<Text>(), stats.healthRegen, healthRegen, 1);
        //DrawTextPlusStatChange(transform.Find("ManaRegen_Value").GetComponent<Text>(), stats.manaRegen, manaRegen, 1);
        DrawTextPlusStatChange(transform.Find("Armor_Value").GetComponent<Text>(), stats.armor, armor, 0);
        DrawTextPlusStatChange(transform.Find("Resistance_Value").GetComponent<Text>(), stats.magicResist, resistance, 0);

        float originalDamage = stats.baseDamage * (stats.baseDamageScaling + ((float)stats.Str * stats.weaponStrScaling) + ((float)stats.Dex * stats.weaponDexScaling) + ((float)stats.Vit * stats.weaponVitScaling) + ((float)stats.Spd * stats.weaponSpdScaling) + ((float)stats.Int * stats.weaponIntScaling) + ((float)stats.Wis * stats.weaponWisScaling) + ((float)stats.Cha * stats.weaponChaScaling));
        float newDamage = baseDamage * (baseDamageMod + ((float)Str * strMod) + ((float)Vit * vitMod) + ((float)Dex * dexMod) + ((float)Spd * spdMod) + ((float)Int * intMod) + ((float)Wis * wisMod) + ((float)Cha * chaMod));

        DrawTextPlusStatChange(transform.Find("AttackDamage_Value").GetComponent<Text>(), originalDamage , newDamage, 0);

        //Debug.Log("curernt attack speed| " + stats.attackSpeed + "  proposed attack speed| " + attackSpeed);
        DrawTextPlusStatChangePercentage(transform.Find("AttackSpeed_Value").GetComponent<Text>(), stats.attackSpeed * 100f, attackSpeed * 100f);
        DrawTextPlusStatChangePercentage(transform.Find("CooldownReduction_Value").GetComponent<Text>(), stats.cooldownReduction * 100f, cooldownReduction * 100f);

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
        Vit = stats.Vit;
        Str = stats.Str;
        Dex = stats.Dex;
        Spd = stats.Spd;
        Int = stats.Int;
        Wis = stats.Wis;
        Cha = stats.Cha;
        healthMax = 20 + 3 * stats.level + 5 * stats.Vit + 2 * stats.Str + stats.Dex + stats.Spd + stats.Int + stats.Wis + stats.Cha + stats.bonusHealth;
        manaMax = 20 + 3 * stats.level + 5 * stats.Wis + stats.Int + stats.bonusMana;
        healthRegen = stats.Vit * 0.2f + stats.bonusHealthRegen;
        manaRegen = stats.Wis * 0.4f + stats.Int * 0.1f + stats.bonusManaRegen;
        armor = stats.armor;
        resistance = stats.magicResist;

        attackSpeed = 1 + 0.05f * stats.Spd + 0.02f * stats.Dex + stats.bonusAttackSpeed + (stats.weaponAttackSpeed - 1);
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

        cooldownReduction = stats.Wis * 0.005f;
        if (cooldownReduction > 0.5f)
            cooldownReduction = 0.5f;

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

    // Used to update all the tooltips.
    public void UpdateTooltips(PlayerStats stats)
    {

        AfflictionManager am = stats.GetComponent<AfflictionManager>();
        for (int index = 0; index < tooltips.Length; index ++)
        {
            switch (index)
            {
                case 0:
                    tooltips[index].text = string.Format("You are currently taking <color=#F3FC56>{0:0} reduced damage</color> from <color=#FFBE2F>physical attacks</color>", stats.armor);
                    break;
                case 1:
                    tooltips[index].text = string.Format("You are currently taking <color=#F93DFF>{0:0} reduced damage</color> from <color=#B756FA>magical attacks</color>", stats.magicResist);
                    break;
                case 2:
                    tooltips[index].text = string.Format("You deal <color=#E45B5B>{0:0}</color> <color=#FFBE2F>physical damage</color>, before multipliers and effects, on your <color=#E45B5B>basic attacks</color> and <color=#E45B5B>roll attacks</color>.", stats.baseDamage * (stats.baseDamageScaling + ((float)stats.Str * stats.weaponStrScaling) + ((float)stats.Dex * stats.weaponDexScaling) + ((float)stats.Vit * stats.weaponVitScaling) + ((float)stats.Spd * stats.weaponSpdScaling) + ((float)stats.Int * stats.weaponIntScaling) + ((float)stats.Wis * stats.weaponWisScaling) + ((float)stats.Cha * stats.weaponChaScaling)));
                    break;
                case 3:
                    tooltips[index].text = string.Format("The current percent speed at which we <color=#E45B5B>attack</color> and complete animations is <color=#E5BE5D>{0:0}%</color>", stats.attackSpeed * 100);
                    break;
                case 4:
                    tooltips[index].text = string.Format("The percentage we've reduced our <color=#5DB1E5>ability cooldowns</color> by is <color=#5DB1E5>{0:0.0}</color>%. Cooldown Reduction scales multiplicatively with itself.", stats.cooldownReduction * 100);
                    break;
                case 5:
                    tooltips[index].text = string.Format("Your <color=#F76A6A>vitality of {0:0}</color> affects your characters overall <color=#AD2A2A>health total</color> drastically, as well as your overall resistances to afflictons.", stats.Vit);
                    break;
                case 6:
                    tooltips[index].text = string.Format("Your <color=#EE9149>strength of {0:0}</color> affects your characters overall <color=#AD2A2A>health total</color> moderately, as well as your <color=#E45B5B>damage</color> with larger weapons.", stats.Str);
                    break;
                case 7:
                    tooltips[index].text = string.Format("Your <color=#FFF071>dexterity of {0:0}</color> affects your characters overall <color=#AD2A2A>health total</color> slighty, as well as your <color=#E45B5B>damage</color> with smaller weapons. It also slightly increases your <color=#E5BE5D>attack speed</color> and <color=#A1FF7A>movespeed</color>.", stats.Dex);
                    break;
                case 8:
                    tooltips[index].text = string.Format("Your <color=#A1FF7A>speed of {0:0}</color> affects your characters overall <color=#AD2A2A>health total</color> slighty, as well as increases your <color=#E5BE5D>attack speed</color> and <color=#A1FF7A>movespeed</color> moderately.", stats.Spd);
                    break;
                case 9:
                    tooltips[index].text = string.Format("Your <color=#7CF2F9>intelligence of {0:0}</color> affects your characters overall <color=#AD2A2A>health total</color> slighty, as well as increases your damage dealt with most <color=#B756FA>magical attacks</color>. Also increases <color=#2A60AD>mana</color> and <color=#2A60AD>mana regen</color> slightly.", stats.Int);
                    break;
                case 10:
                    tooltips[index].text = string.Format("Your <color=#8C91FC>wisdom of {0:0}</color> affects your characters overall <color=#AD2A2A>health total</color> slighty, as well as increases <color=#2A60AD>mana, manaregen</color>, and <color=#5DB1E5>cooldown reduction</color> moderately.", stats.Wis);
                    break;
                case 11:
                    tooltips[index].text = string.Format("Your <color=#F27EEB>charima of {0:0}</color> affects your characters overall <color=#AD2A2A>health total</color> slighty, as well as offers you better <color=#F27EEB>luck in random encounters</color>. It also increases the level of <color=#F27EEB>friendliness</color> a npc will regard you with.", stats.Cha);
                    break;
                case 12:
                    tooltips[index].text = string.Format("Reduce the amount of <color=#F49910>fire affliction</color> you recieve by <color=#F49910>{0:0.0}%</color>. Being set on fire does massive <color=#B756FA>magic damage</color> over time. The duration can be reduce by rolling around.", am.aflameResist * 100);
                    break;
                case 13:
                    tooltips[index].text = string.Format("Reduce the amount of <color=#7B6A91>sleep affliction</color> you recieve by <color=#7B6A91>{0:0.0}%</color>. Being put to sleep <color=#D9B529>stuns</color> you for a long time. The next source of damage you take is increased, but breaks the effect.", am.sleepResist * 100);
                    break;
                case 14:
                    tooltips[index].text = string.Format("Reduce the amount of <color=#D9B529>stun affliction</color> you recieve by <color=#D9B529>{0:0.0}%</color>. Being stunned renders you <color=#D9B529>unable to take action</color> for a short period of time.", am.stunResist * 100);
                    break;
                case 15:
                    tooltips[index].text = string.Format("Reduce the amount of <color=#585547>curse affliction</color> you recieve by <color=#585547>{0:0.0}%</color>. Being cursed <color=#585547>reduces yours stats,</color> <color=#E5BE5D>attack speed,</color><color=#FFBE2F> physical damage dealt,</color> and <color=#B756FA>magic damage dealt</color> by 50% for a few seconds.", am.curseResist * 100);
                    break;
                case 16:
                    tooltips[index].text = string.Format("Reduce the amount <color=#C0201E> bleed affliction</color> you recieve by <color=#C0201E>{0:0.0}%</color>. Being bled means you will take a small amount of <color=#FFBE2F>physical damage</color> over time, but a large amount of <color=#FFBE2F>physical damage</color> when you attack or roll.", am.bleedResist * 100);
                    break;
                case 17:
                    tooltips[index].text = string.Format("Reduce the amount of <color=#7ECE1E>poison affliction</color> you recieve by <color=#7ECE1E>{0:0.0}%</color>. Being poisoned makes you take a large amount of <color=#B756FA>magic damage</color> over a long duration.", am.poisonResist * 100);
                    break;
                case 18:
                    tooltips[index].text = string.Format("Reduce the amount of <color=#168523>corrosion affliction</color> you recieve by <color=#168523>{0:0.0}%</color>. Being corroded does a small amount of <color=#B756FA>magic damage</color> and reduces your <color=#F3FC56>armor</color> and resistance significantly for a moderate time.", am.corrosionResist * 100);
                    break;
                case 19:
                    tooltips[index].text = string.Format("Reduce the amount of <color=#0DB9C6>frostbite affliction</color> you recieve by <color=#0DB9C6>{0:0.0}%</color>. Being frostbitten reduces your <color=#A1FF7A>movespeed</color> and <color=#E5BE5D>attack speed</color> to nearly zero for a few seconds.", am.frostbiteResist * 100);
                    break;
                case 20:
                    tooltips[index].text = string.Format("Reduce the amount of <color=#7B8ADC>knockback</color> you recieve by <color=#7B8ADC>{0:0.0}%</color>. Being knocked back <color=#7B8ADC>launches you</color> in the direction of the attack.", am.knockBackResist * 100);
                    break;
                default:
                    break;
            }
        }
    }
}
