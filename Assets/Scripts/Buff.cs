using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviour
{
    public BuffsManager.BuffType myType;
    
    public List<ParticleSystem> effectParticleSystem = new List<ParticleSystem>();
    public List<ParticleSystem> endOfBuffParticleSystem = new List<ParticleSystem>();
    public PlayerStats connectedPlayer;
    public GameObject connectedIcon;
    public UnityEngine.UI.Text iconStacks;
    public Color damageColor;
    public bool onHitEffect = false;
    
    public bool infiniteDuration = false;
    public float duration = 10f;
    public float currentTimer = 0f;
    public float DPS = 0f;
    public float onHitDamageAmount = 0;
    
    public bool stackable = false;
    public float currentStacks = 0;
    public float maxStacks = 1;
    public bool stackSingleFalloff = false;
    public float stackfalloffTime = 0.5f;
    public float stacktargetTimer = 0f;

    public int vitSC = 0;                                   //SC stands for stat change.
    public int strSC = 0;
    public int dexSC = 0;
    public int spdSC = 0;
    public int intSC = 0;
    public int wisSC = 0;
    public int chaSC = 0;

    public float healthSC = 0;
    public float manaSC = 0;
    public float healthRegenSC = 0;
    public float manaRegenSC = 0;
    public float armorSC = 0;
    public float resistanceSC = 0;
    public float damageReductionSC = 0;

    public float minDamageSC = 0;
    public float maxDamageSC = 0;
    public float baseDamageSC = 0;
    public float atkSpdSC = 0;
    public float critChanceSC = 0;
    public float critDamageSC = 0;

    public float aflameResistSC = 0;
    public float asleepResistSC = 0;
    public float stunResistSC = 0;
    public float knockBackResistSC = 0;
    public float curseResistSC = 0;
    public float bleedResistSC = 0;
    public float poisonResistSC = 0;
    public float corrosionResistSC = 0;
    public float frostbiteResistSC = 0;

    public float invulnerabilitySC = 0;
    public float untargetabilitySC = 0;
    public float invisibilitySC = 0;

    public float targetDamageTickTimer = 0.5f;
    public float currentDamageTick = 0;
    public bool almostDone = false;

    public float sizeSC = 0;
    

    private void Update()
    {
        // If this is not an infinite duration buff, count it down.
        if (!infiniteDuration)
        {
            currentTimer += Time.deltaTime;
            if(!almostDone && currentTimer > duration - 4)
            {
                almostDone = true;
                connectedIcon.GetComponent<Animator>().SetBool("AlmostDone", true);
            }
            if (currentTimer > duration)
            {
                if (stackable)
                    RemoveStacks(1);
                else
                    EndBuff();
            }
        }

        if(DPS != 0)
        {
            currentDamageTick += Time.deltaTime;
            if (currentDamageTick > targetDamageTickTimer)
            {
                currentDamageTick -= targetDamageTickTimer;
                connectedPlayer.TakeDamage(DPS * targetDamageTickTimer, false, damageColor);
            }
        }
    }

    // Used to add or reset the buff's timer.
    public void AddTime(float amount, bool reset)
    {
        if (reset)
            currentTimer = 0;
        else
            currentTimer -= amount;

        if (currentTimer < duration - 4)
        {
            almostDone = false;
            connectedIcon.GetComponent<Animator>().SetBool("AlmostDone", false);
        }

    }

    // Used to add stacks and stats per stat to the target.
    public void AddStack(float amount)
    {
        if (currentStacks < maxStacks)
        {
            // Add the required stacks, check to see if we exceed the max stacks.
            if (currentStacks + amount <= maxStacks)
                currentStacks += amount;
            else
            {
                amount = maxStacks = currentStacks;
                currentStacks = maxStacks;
            }
        }
        else
            amount = 0;

        // Setting the timer.
        currentTimer = 0;
        if (stackSingleFalloff && stacktargetTimer != duration)
            duration = stacktargetTimer;

        // Changing the text on the buff icon.
        iconStacks.text = string.Format("x{0:0}", currentStacks);

        // Adding the stat change.
        if (amount != 0)
        {
            // If we changed the core stats, add more stacks
            if (vitSC != 0 || strSC != 0 || dexSC != 0 || spdSC != 0 || intSC != 0 || wisSC != 0 || chaSC != 0)
                ChangeCoreStats(false, vitSC * (int)amount, strSC * (int)amount, dexSC * (int)amount, spdSC * (int)amount, intSC * (int)amount, wisSC * (int)amount, chaSC * (int)amount);

            // If we changed the defensive stats, add more stacks
            if (healthSC != 0 || manaSC != 0 || healthRegenSC != 0 || manaRegenSC != 0 || armorSC != 0 || resistanceSC != 0)
                ChangeDefensiveStats(false, healthSC * amount, manaSC * amount, healthRegenSC * amount, manaRegenSC * amount, armorSC * amount, resistanceSC * amount, damageReductionSC * amount);

            // If we changed offensive stats, add more stacks
            if (baseDamageSC != 0 || minDamageSC != 0 || maxDamageSC != 0 || atkSpdSC != 0 || critDamageSC != 0 || critChanceSC != 0)
                ChangeOffensiveStats(false, baseDamageSC * amount, minDamageSC * amount, maxDamageSC * amount, critChanceSC * amount, critDamageSC * amount, atkSpdSC * amount);

            // If we changed our resistance based stats, add more stacks
            if (aflameResistSC != 0 || stunResistSC != 0 || asleepResistSC != 0 || bleedResistSC != 0 || poisonResistSC != 0 || curseResistSC != 0 || corrosionResistSC != 0 || frostbiteResistSC != 0 || knockBackResistSC != 0)
                ChangeAfflictionStats(false, aflameResistSC * amount, stunResistSC * amount, asleepResistSC * amount, bleedResistSC * amount, poisonResistSC * amount, curseResistSC * amount, frostbiteResistSC * amount, corrosionResistSC * amount, knockBackResistSC * amount);

            // If we changed our invunerability, unatrgetability or invisibility
            if (invulnerabilitySC != 0 || untargetabilitySC != 0 || invisibilitySC != 0)
                ChangePlayerStatusLocks(false, invulnerabilitySC * amount, invisibilitySC * amount, untargetabilitySC * amount);

            // If we changed the size, change them back.
            if (sizeSC != 0)
                ChangeSize(false, sizeSC * amount);
        }
    }

    // Used to remove stacks and stats per stat to the target.
    public void RemoveStacks(float amount)
    {
        amount *= -1;
        if (currentStacks + amount < 0)
        {
            amount = currentStacks * -1;
            currentStacks = 0;
        }
        else
            currentStacks += amount;
        int amountint = Mathf.RoundToInt(amount);

        // Setting the timer.
        currentTimer = 0;
        if (stackSingleFalloff && stackfalloffTime != duration)
            duration = stackfalloffTime;

        // Changing the text on the buff icon.
        iconStacks.text = string.Format("x{0:0}", currentStacks);

        // Adding the stat change.
        // If we changed the core stats, remove more stacks
        if (vitSC != 0 || strSC != 0 || dexSC != 0 || spdSC != 0 || intSC != 0 || wisSC != 0 || chaSC != 0)
            ChangeCoreStats(false, vitSC * amountint, strSC * amountint, dexSC * amountint, spdSC * amountint, intSC * amountint, wisSC * amountint, chaSC * amountint);

        // If we changed the defensive stats, remove more stacks
        if (healthSC != 0 || manaSC != 0 || healthRegenSC != 0 || manaRegenSC != 0 || armorSC != 0 || resistanceSC != 0)
            ChangeDefensiveStats(false, healthSC * amount, manaSC * amount, healthRegenSC * amount, manaRegenSC * amount, armorSC * amount, resistanceSC * amount, damageReductionSC * amount);

        // If we changed offensive stats, remove more stacks
        if (baseDamageSC != 0 || minDamageSC != 0 || maxDamageSC != 0 || atkSpdSC != 0 || critDamageSC != 0 || critChanceSC != 0)
            ChangeOffensiveStats(false, baseDamageSC * amount, minDamageSC * amount, maxDamageSC * amount, critChanceSC * amount, critDamageSC * amount, atkSpdSC * amount);

        // If we changed our resistance based stats, remove more stacks
        if (aflameResistSC != 0 || stunResistSC != 0 || asleepResistSC != 0 || bleedResistSC != 0 || poisonResistSC != 0 || curseResistSC != 0 || corrosionResistSC != 0 || frostbiteResistSC != 0 || knockBackResistSC != 0)
            ChangeAfflictionStats(false, aflameResistSC * amount, stunResistSC * amount, asleepResistSC * amount, bleedResistSC * amount, poisonResistSC * amount, curseResistSC * amount, frostbiteResistSC * amount, corrosionResistSC * amount, knockBackResistSC * amount);

        // If we changed our invunerability, unatrgetability or invisibility
        if (invulnerabilitySC != 0 || untargetabilitySC != 0 || invisibilitySC != 0)
            ChangePlayerStatusLocks(false, invulnerabilitySC * amount, invisibilitySC * amount, untargetabilitySC * amount);

        // If we changed the size, change them back.
        if (sizeSC != 0)
            ChangeSize(false, sizeSC * amount);

        if (currentStacks == 0)
            EndBuff();
    }

    // USed to add core stats to the player
    public void ChangeCoreStats(bool changeStatsChangeValue, int vitGain, int strGain, int dexGain, int spdGain, int intGain, int wisGain, int chaGain)
    {
        connectedPlayer.Vit += vitGain;
        connectedPlayer.Str += strGain;
        connectedPlayer.Dex += dexGain;
        connectedPlayer.Spd += spdGain;
        connectedPlayer.Int += intGain;
        connectedPlayer.Wis += wisGain;
        connectedPlayer.Cha += chaGain;

        if (changeStatsChangeValue)
        {
            vitSC = vitGain;
            strSC = strGain;
            dexSC = dexGain;
            spdSC = spdGain;
            intSC = intGain;
            wisSC = wisGain;
            chaSC = chaGain;
        }

        connectedPlayer.StatSetup(false, true);
    }

    // Used to change if the player is invisible, invulnerable, etc. by changing their stats bool values.
    public void ChangePlayerStatusLocks(bool changeStatsChangeValue, float invulnerabilityGain, float invisibilityGain, float untargetabilityGain)
    {
        connectedPlayer.AddInvisibilitySource(invisibilityGain);
        connectedPlayer.AddInvulnerablitySource(invulnerabilityGain);
        connectedPlayer.AddUntargetableSource(untargetabilityGain);

        if(changeStatsChangeValue)
        {
            invulnerabilitySC = invulnerabilityGain;
            invisibilitySC = invisibilityGain;
            untargetabilitySC = untargetabilityGain;
        }
    }

    // USed to change the player size.
    public void ChangeSize(bool changeStatsChangeValue, float sizeGain)
    {
        connectedPlayer.ChangeSize(sizeGain);

        if (changeStatsChangeValue)
            sizeSC = sizeGain;
    }

    //USed to add offensive stast to the player
    public void ChangeOffensiveStats(bool changeStatsChangeValue, float baseDamageGain, float minDamageGain, float maxDamageGain, float critChanceGain, float critModifierGain, float atkSpeedGain)
    {
        connectedPlayer.weaponHitbase += baseDamageGain;
        connectedPlayer.weaponHitMax += maxDamageGain;
        connectedPlayer.weaponHitMin += minDamageGain;
        connectedPlayer.weaponCritChance += critChanceGain;
        connectedPlayer.weaponCritMod += critModifierGain;
        connectedPlayer.bonusAttackSpeed += atkSpeedGain;

        if (changeStatsChangeValue)
        {
            baseDamageSC = baseDamageGain;
            minDamageSC = minDamageGain;
            maxDamageSC = maxDamageGain;
            atkSpdSC = atkSpeedGain;
            critChanceSC = critChanceGain;
            critDamageSC = critModifierGain;
        }

        connectedPlayer.StatSetup(false, true);
    }

    // Used to change the characters health, manan, regens, armor, and mr
    public void ChangeDefensiveStats(bool changeStatsChangeValue, float healthGain, float manaGain, float healthRegenGain, float manaRegenGain, float armorGain, float resistanceGain, float damageReductionGain)
    {
        connectedPlayer.bonusHealth += healthGain;
        connectedPlayer.bonusMana += manaGain;
        connectedPlayer.bonusHealthRegen += healthRegenGain;
        connectedPlayer.bonusManaRegen += manaRegenGain;
        connectedPlayer.armor += armorGain;
        connectedPlayer.magicResist += resistanceGain;
        connectedPlayer.damageReduction += damageReductionGain;

        if (changeStatsChangeValue)
        {
            healthSC = healthGain;
            manaSC = manaGain;
            healthRegenSC = healthRegenGain;
            manaRegenSC = manaRegenGain;
            armorSC = armorGain;
            resistanceSC = resistanceGain;
            damageReductionSC = damageReductionGain;
        }
        
        connectedPlayer.StatSetup(false, true);
    }

    // Used to change the players resistance to certain afflictions
    public void ChangeAfflictionStats(bool changeStatsChangeValue, float aflameGain, float stunGain, float asleepGain, float bleedGain, float poisonGain, float curseGain, float frostbiteGain, float corrosionGain, float knockbackGain)
    {
        AfflictionManager afflictions = connectedPlayer.GetComponent<AfflictionManager>();

        afflictions.aflameResist += aflameGain;
        afflictions.stunResist += stunGain;
        afflictions.sleepResist += asleepGain;
        afflictions.bleedResist += bleedGain;
        afflictions.poisonResist += poisonGain;
        afflictions.curseResist += curseGain;
        afflictions.frostbiteResist += frostbiteGain;
        afflictions.corrosionResist += corrosionGain;
        afflictions.knockBackResist += knockbackGain;

        if (changeStatsChangeValue)
        {
            aflameResistSC = aflameGain;
            stunResistSC = stunGain;
            asleepResistSC = asleepGain;
            bleedResistSC = bleedGain;
            poisonResistSC = poisonGain;
            curseResistSC = curseGain;
            frostbiteResistSC = frostbiteGain;
            corrosionResistSC = corrosionGain;
            knockBackResistSC = knockbackGain;
        }

        connectedPlayer.StatSetup(false, true);
    }


    // Used when the buff is over
    public void EndBuff()
    {
        foreach (ParticleSystem ps in effectParticleSystem)
            ps.Stop();
        foreach (ParticleSystem ps in endOfBuffParticleSystem)
            ps.Play();

        if (myType == BuffsManager.BuffType.Stunned)
            connectedPlayer.GetComponent<PlayerController>().stunned = false;
        else if (myType == BuffsManager.BuffType.Asleep)
            connectedPlayer.GetComponent<PlayerController>().asleep = false;
        else if (myType == BuffsManager.BuffType.Bleeding)
            connectedPlayer.GetComponent<PlayerController>().bleeding = false;

        // We do not change it if its a stackable buff since this method is called after we already remvoed all the stats associated with the buff, this would put us into negatives.
        if (!stackable)
        {
            // If we changed the core stats, change them back.
            if (vitSC != 0 || strSC != 0 || dexSC != 0 || spdSC != 0 || intSC != 0 || wisSC != 0 || chaSC != 0)
                ChangeCoreStats(true, vitSC * -1, strSC * -1, dexSC * -1, spdSC * -1, intSC * -1, wisSC * -1, chaSC * -1);

            // If we changed the defensive stats, change them back.
            if (healthSC != 0 || manaSC != 0 || healthRegenSC != 0 || manaRegenSC != 0 || armorSC != 0 || resistanceSC != 0)
                ChangeDefensiveStats(true, healthSC * -1, manaSC * -1, healthRegenSC * -1, manaRegenSC * -1, armorSC * -1, resistanceSC * -1, damageReductionSC * -1);

            // If we changed offensive stats, change em back.
            if (baseDamageSC != 0 || minDamageSC != 0 || maxDamageSC != 0 || atkSpdSC != 0 || critDamageSC != 0 || critChanceSC != 0)
                ChangeOffensiveStats(true, baseDamageSC * -1, minDamageSC * -1, maxDamageSC * -1, critChanceSC * -1, critDamageSC * -1, atkSpdSC * -1);

            // If we changed our resistance based stats, change em back.
            if (aflameResistSC != 0 || stunResistSC != 0 || asleepResistSC != 0 || bleedResistSC != 0 || poisonResistSC != 0 || curseResistSC != 0 || corrosionResistSC != 0 || frostbiteResistSC != 0 || knockBackResistSC != 0)
                ChangeAfflictionStats(true, aflameResistSC * -1, stunResistSC * -1, asleepResistSC * -1, bleedResistSC * -1, poisonResistSC * -1, curseResistSC * -1, frostbiteResistSC * -1, corrosionResistSC * -1, knockBackResistSC * -1);

            // If we changed our invunerability, unatrgetability or invisibility
            if (invulnerabilitySC != 0 || untargetabilitySC != 0 || invisibilitySC != 0)
                ChangePlayerStatusLocks(true, invulnerabilitySC * -1, invisibilitySC * -1, untargetabilitySC * -1);

            // If we changed the size, change them back.
            if (sizeSC != 0)
                ChangeSize(true, sizeSC * -1);
        }
        // This contacts the buff manager, removi8ng us from their list of active buffs , deleting our icon, then killing this instance of the class. ALL fixes to stats should be done before this.
        connectedPlayer.GetComponent<BuffsManager>().RemoveBuff(this);
    }
}
