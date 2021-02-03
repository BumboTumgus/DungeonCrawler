using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatUpdater : MonoBehaviour
{
    float healthMax;
    float healthRegen;
    float armor;
    float attackSpeed;
    float baseDamage;
    float aflameResistance;
    float asleepResistance;
    float stunResistance;
    float bleedResistance;
    float poisonResistance;
    float frostbiteResistance;
    float knockbackResistance;
    float cooldownReduction;
    List<Item> weaponsToHitWith = new List<Item>();
    public BarManager expBar;
    public BarManager healthBar;

    public Text[] tooltips;
    public Color[] uiColors;

    public bool mouseWithItemHovered = false;

    // This method is called at the start of the game to set up all the player's stats. It is also called whenever a player switches gear or the stats would ahve to change.
    public void SetStatValues(PlayerStats stats)
    {
        transform.Find("Playername").GetComponent<Text>().text = stats.playerName + " the " + stats.playerTitle;
        transform.Find("Level_Value").GetComponent<Text>().text = stats.level + "";
        transform.Find("EXP_Value").GetComponent<Text>().text = stats.exp + " / " + stats.expTarget;
        SetExpBarsValues(stats);
        SetHealthManaBarValues(stats);

        transform.Find("Health_Value").GetComponent<Text>().text = string.Format("{0:0}",stats.health) + " / " + stats.healthMax;
        transform.Find("HealthRegen_Value").GetComponent<Text>().text = string.Format("{0:0.0}", stats.healthRegen);

        transform.Find("Armor_Value").GetComponent<Text>().text = string.Format("{0:0}", stats.armor);

        switch (stats.weaponsToHitWith.Count)
        {
            case 0:
                transform.Find("AttackDamage_Value").GetComponent<Text>().text = string.Format("{0:0}", stats.baseDamage);
                break;
            case 1:
                transform.Find("AttackDamage_Value").GetComponent<Text>().text = string.Format("{0:0}", stats.weaponsToHitWith[0].baseDamageScaling * stats.baseDamage);
                break;
            case 2:
                transform.Find("AttackDamage_Value").GetComponent<Text>().text = string.Format("{0:0} / {1:0}", stats.weaponsToHitWith[0].baseDamageScaling * stats.baseDamage, stats.weaponsToHitWith[1].baseDamageScaling * stats.baseDamage);
                break;
            default:
                break;
        }

        transform.Find("AttackSpeed_Value").GetComponent<Text>().text = string.Format("{0:0}%", stats.attackSpeed * 100 );
        transform.Find("CooldownReduction_Value").GetComponent<Text>().text = string.Format("{0:0}%", stats.cooldownReduction * 100);

        transform.Find("AflameResistance_Value").GetComponent<Text>().text = string.Format("{0:0}%", stats.aflameResistance * 100);
        transform.Find("AsleepResistance_Value").GetComponent<Text>().text = string.Format("{0:0}%", stats.sleepResistance * 100);
        transform.Find("StunResistance_Value").GetComponent<Text>().text = string.Format("{0:0}%", stats.stunResistance * 100);
        transform.Find("BleedResistance_Value").GetComponent<Text>().text = string.Format("{0:0}%", stats.bleedResistance * 100);
        transform.Find("PoisonResistance_Value").GetComponent<Text>().text = string.Format("{0:0}%", stats.poisonResistance * 100);
        transform.Find("FrostbiteResistance_Value").GetComponent<Text>().text = string.Format("{0:0}%", stats.frostbiteResistance * 100);
        transform.Find("KnockbackResistance_Value").GetComponent<Text>().text = string.Format("{0:0}%", stats.knockbackResistance * 100);

        UpdateTooltips(stats);
    }

    // This method is used to update the health and mana values of the player.
    public void SetHealthManaBarValues(PlayerStats stats)
    {
        healthBar.Initialize(stats.healthMax, false, true, stats.health);
        healthBar.SetValue(stats.health, true);
        healthBar.transform.Find("HealthBarFill").Find("Value").GetComponent<Text>().text = string.Format("{0:0} / {1:0}", stats.health, stats.healthMax);
        healthBar.transform.Find("HealthBarFill").Find("RegenValue").GetComponent<Text>().text = string.Format("+{0:0.0} hp/5", stats.healthRegen);

        transform.Find("Health_Value").GetComponent<Text>().text = string.Format("{0:0}", stats.health) + " / " + stats.healthMax;
    }

    public void UpdateHealthManaBarValues(PlayerStats stats)
    {
        healthBar.SetValue(stats.health, false);

        if (!mouseWithItemHovered)
        {
            healthBar.transform.Find("HealthBarFill").Find("Value").GetComponent<Text>().text = string.Format("{0:0} / {1:0}", stats.health, stats.healthMax);
        }
        else
        {
            if (healthMax - stats.healthMax < 0)
                healthBar.transform.Find("HealthBarFill").Find("Value").GetComponent<Text>().text = string.Format("{0:0} / {1:0} <color=#ea4553>{2:0}</color>", stats.health, stats.healthMax, healthMax - stats.healthMax);
            else if (healthMax - stats.healthMax > 0)
                healthBar.transform.Find("HealthBarFill").Find("Value").GetComponent<Text>().text = string.Format("{0:0} / {1:0} <color=#60d46e>+{2:0}</color>", stats.health, stats.healthMax, healthMax - stats.healthMax);
        }
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

        if(healthMax - stats.healthMax < 0)
            healthBar.transform.Find("HealthBarFill").Find("Value").GetComponent<Text>().text = string.Format("{0:0} / {1:0} <color=#ea4553>{2:0}</color>", stats.health, stats.healthMax, healthMax - stats.healthMax);
        else if (healthMax - stats.healthMax > 0)
            healthBar.transform.Find("HealthBarFill").Find("Value").GetComponent<Text>().text = string.Format("{0:0} / {1:0} <color=#60d46e>+{2:0}</color>", stats.health, stats.healthMax, healthMax - stats.healthMax);

        DrawTextPlusStatChange(transform.Find("Armor_Value").GetComponent<Text>(), stats.armor, armor, 0);

        //Debug.Log("the local weapons to hit with count comaprwe to the actual one is: " + weaponsToHitWith.Count + " | " + stats.weaponsToHitWith.Count);
        switch (weaponsToHitWith.Count)
        {
            case 0:
                transform.Find("AttackDamage_Value").GetComponent<Text>().text = string.Format("{0:0}", baseDamage);
                break;
            case 1:
                transform.Find("AttackDamage_Value").GetComponent<Text>().text = string.Format("{0:0}", weaponsToHitWith[0].baseDamageScaling * baseDamage);
                break;
            case 2:
                transform.Find("AttackDamage_Value").GetComponent<Text>().text = string.Format("{0:0} / {1:0}", weaponsToHitWith[0].baseDamageScaling * baseDamage, weaponsToHitWith[1].baseDamageScaling * baseDamage);
                break;
            default:
                break;
        }

        DrawTextPlusStatChangePercentage(transform.Find("AttackSpeed_Value").GetComponent<Text>(), stats.attackSpeed * 100f, attackSpeed * 100f);
        DrawTextPlusStatChangePercentage(transform.Find("CooldownReduction_Value").GetComponent<Text>(), stats.cooldownReduction * 100f, cooldownReduction * 100f);

        DrawTextPlusStatChangePercentage(transform.Find("AflameResistance_Value").GetComponent<Text>(), stats.aflameResistance * 100, aflameResistance * 100);
        DrawTextPlusStatChangePercentage(transform.Find("AsleepResistance_Value").GetComponent<Text>(), stats.sleepResistance * 100, asleepResistance * 100);
        DrawTextPlusStatChangePercentage(transform.Find("StunResistance_Value").GetComponent<Text>(), stats.stunResistance * 100, stunResistance * 100);
        DrawTextPlusStatChangePercentage(transform.Find("BleedResistance_Value").GetComponent<Text>(), stats.bleedResistance * 100, bleedResistance * 100);
        DrawTextPlusStatChangePercentage(transform.Find("PoisonResistance_Value").GetComponent<Text>(), stats.poisonResistance * 100, poisonResistance * 100);
        DrawTextPlusStatChangePercentage(transform.Find("FrostbiteResistance_Value").GetComponent<Text>(), stats.frostbiteResistance * 100, frostbiteResistance * 100);
        DrawTextPlusStatChangePercentage(transform.Find("KnockbackResistance_Value").GetComponent<Text>(), stats.knockbackResistance * 100, knockbackResistance * 100);
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

        attackSpeed = stats.attackSpeed;
        baseDamage = stats.baseDamage;

        aflameResistance = stats.aflameResistance;
        asleepResistance = stats.sleepResistance;
        stunResistance = stats.stunResistance;
        bleedResistance = stats.bleedResistance;
        poisonResistance = stats.poisonResistance;
        frostbiteResistance = stats.frostbiteResistance;
        knockbackResistance = stats.knockbackResistance;

        weaponsToHitWith = stats.weaponsToHitWith;

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
        for (int index = 0; index < tooltips.Length; index ++)
        {
            switch (index) 
            {
                case 0:
                    tooltips[index].text = string.Format("You are currently taking <color=#F3FC56>{0:0} reduced damage</color> from <color=#FFBE2F>physical attacks</color>", stats.armor);
                    break;
                case 2:
                    tooltips[index].text = string.Format("You deal <color=#E45B5B>{0:0}</color> <color=#FFBE2F>physical damage</color>, before multipliers and effects, on your <color=#E45B5B>basic attacks</color> and <color=#E45B5B>roll attacks</color>.", stats.baseDamage * 1);
                    break;
                case 3:
                    tooltips[index].text = string.Format("The current percent speed at which we <color=#E45B5B>attack</color> and complete animations is <color=#E5BE5D>{0:0}%</color>", stats.attackSpeed * 100);
                    break;
                case 4:
                    tooltips[index].text = string.Format("The percentage we've reduced our <color=#5DB1E5>ability cooldowns</color> by is <color=#5DB1E5>{0:0.0}</color>%. Cooldown Reduction scales multiplicatively with itself.", stats.cooldownReduction * 100);
                    break;
                case 13:
                    tooltips[index].text = string.Format("Reduce the amount of <color=#7B6A91>sleep affliction</color> you recieve by <color=#7B6A91>{0:0.0}%</color>. Being put to sleep <color=#D9B529>stuns</color> you for a long time. The next source of damage you take is increased, but breaks the effect.", stats.sleepResistance * 100);
                    break;
                case 14:
                    tooltips[index].text = string.Format("Reduce the amount of <color=#D9B529>stun affliction</color> you recieve by <color=#D9B529>{0:0.0}%</color>. Being stunned renders you <color=#D9B529>unable to take action</color> for a short period of time.", stats.stunResistance * 100);
                    break;
                case 16:
                    tooltips[index].text = string.Format("Reduce the amount <color=#C0201E> bleed affliction</color> you recieve by <color=#C0201E>{0:0.0}%</color>. Being bled means you will take a small amount of <color=#FFBE2F>physical damage</color> over time, but a large amount of <color=#FFBE2F>physical damage</color> when you attack or roll.", stats.bleedResistance * 100);
                    break;
                case 17:
                    tooltips[index].text = string.Format("Reduce the amount of <color=#7ECE1E>poison affliction</color> you recieve by <color=#7ECE1E>{0:0.0}%</color>. Being poisoned makes you take a large amount of <color=#B756FA>magic damage</color> over a long duration.", stats.poisonResistance * 100);
                    break;
                case 19:
                    tooltips[index].text = string.Format("Reduce the amount of <color=#0DB9C6>frostbite affliction</color> you recieve by <color=#0DB9C6>{0:0.0}%</color>. Being frostbitten reduces your <color=#A1FF7A>movespeed</color> and <color=#E5BE5D>attack speed</color> to nearly zero for a few seconds.", stats.frostbiteResistance * 100);
                    break;
                case 20:
                    tooltips[index].text = string.Format("Reduce the amount of <color=#7B8ADC>knockback</color> you recieve by <color=#7B8ADC>{0:0.0}%</color>. Being knocked back <color=#7B8ADC>launches you</color> in the direction of the attack.", stats.knockbackResistance * 100);
                    break;
                default:
                    break;
            }
        }
    }
}
