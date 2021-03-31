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
    public HitBox.DamageType damageType;
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

    public float healthSC = 0;
    public float healthRegenSC = 0;
    public float armorSC = 0;
    public float damageReductionSC = 0;
    
    public float atkSpdSC = 0;
    public float movespeedSC = 0;
    public float damagePercentageSC = 0;

    public float aflameResistSC = 0;
    public float frostbiteResistSC = 0;
    public float overchargeResistSC = 0;
    public float overgrowthResistSC = 0;
    public float sunderResistSC = 0;
    public float windshearResistSC = 0;
    public float asleepResistSC = 0;
    public float stunResistSC = 0;
    public float knockBackResistSC = 0;
    public float bleedResistSC = 0;
    public float poisonResistSC = 0;

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
                if (stackable && stackSingleFalloff)
                    RemoveStacks(1, true);
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
               // if(damageType == null)
                    //connectedPlayer.TakeDamage(DPS * targetDamageTickTimer, false, damageColor);
                //else
                if(!stackable)
                    connectedPlayer.TakeDamage(DPS * targetDamageTickTimer, false, damageType, 0);
                else
                    connectedPlayer.TakeDamage(DPS * currentStacks * targetDamageTickTimer, false, damageType, 0);


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

        // reset one time aprticle effects of the target.
        if(reset)
        {
            switch (myType)
            {
                case BuffsManager.BuffType.Frozen:
                    foreach (ParticleSystem ps in effectParticleSystem)
                        ps.Stop();
                    foreach (ParticleSystem ps in effectParticleSystem)
                        ps.Play();
                    break;
                case BuffsManager.BuffType.Asleep:
                    foreach (ParticleSystem ps in effectParticleSystem)
                        ps.Stop();
                    foreach (ParticleSystem ps in effectParticleSystem)
                        ps.Play();
                    break;
                default:
                    break;
            }
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
                //amount = maxStacks = currentStacks;
                currentStacks = maxStacks;
            }
        }
        else
            amount = 0;

        connectedIcon.GetComponent<Animator>().SetBool("AlmostDone", false);

        // Setting the timer.
        currentTimer = 0;
        if (stackSingleFalloff && stacktargetTimer != duration)
            duration = stacktargetTimer;

        // Changing the text on the buff icon.
        iconStacks.text = string.Format("x{0:0}", currentStacks);

        // Adding the stat change.
        if (amount != 0)
        {
            // If we changed the defensive stats, add more stacks
            if (healthSC != 0 || healthRegenSC != 0 || armorSC != 0 )
                ChangeDefensiveStats(false, healthSC * amount, healthRegenSC * amount, armorSC * amount, damageReductionSC * amount);

            // If we changed offensive stats, add more stacks
            if (atkSpdSC != 0 || movespeedSC != 0 || damagePercentageSC != 0)
                ChangeOffensiveStats(false, atkSpdSC * amount, movespeedSC * amount, damagePercentageSC * amount);

            // If we changed our resistance based stats, remove more stacks
            if (aflameResistSC != 0 || stunResistSC != 0 || asleepResistSC != 0 || bleedResistSC != 0 || poisonResistSC != 0 || frostbiteResistSC != 0 || knockBackResistSC != 0 || overchargeResistSC != 0 || overgrowthResistSC != 0 || sunderResistSC != 0 || windshearResistSC != 0)
                ChangeResistanceStats(true, aflameResistSC * amount, frostbiteResistSC * amount, overchargeResistSC * amount, overgrowthResistSC * amount, sunderResistSC * amount, frostbiteResistSC * amount, stunResistSC * amount, asleepResistSC * amount, bleedResistSC * amount, poisonResistSC * amount, knockBackResistSC * amount);

            // If we changed our invunerability, unatrgetability or invisibility
            if (invulnerabilitySC != 0 || untargetabilitySC != 0 || invisibilitySC != 0)
                ChangePlayerStatusLocks(false, invulnerabilitySC * amount, invisibilitySC * amount, untargetabilitySC * amount);

            // If we changed the size, change them back.
            if (sizeSC != 0)
                ChangeSize(false, sizeSC * amount);
        }
    }

    // Used to remove stacks and stats per stat to the target.
    public void RemoveStacks(float amount, bool changeTimer)
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

        if (changeTimer)
        {
            // Setting the timer.
            currentTimer = 0;
            if (stackSingleFalloff && stackfalloffTime != duration)
                duration = stackfalloffTime;
        }

        // Changing the text on the buff icon.
        iconStacks.text = string.Format("x{0:0}", currentStacks);

        // Adding the stat change.

        // If we changed the defensive stats, remove more stacks
        if (healthSC != 0 || healthRegenSC != 0 || armorSC != 0 )
            ChangeDefensiveStats(false, healthSC * amount,  healthRegenSC * amount,  armorSC * amount, damageReductionSC * amount);

        // If we changed offensive stats, remove more stacks
        if (atkSpdSC != 0 || movespeedSC != 0 || damagePercentageSC != 0)
            ChangeOffensiveStats(false, atkSpdSC * amount, movespeedSC * amount, damagePercentageSC * amount);

        // If we changed our resistance based stats, remove more stacks
        if (aflameResistSC != 0 || stunResistSC != 0 || asleepResistSC != 0 || bleedResistSC != 0 || poisonResistSC != 0 || frostbiteResistSC != 0 || knockBackResistSC != 0 || overchargeResistSC != 0 || overgrowthResistSC != 0 || sunderResistSC != 0 || windshearResistSC != 0)
            ChangeResistanceStats(true, aflameResistSC * amount, frostbiteResistSC * amount, overchargeResistSC * amount, overgrowthResistSC * amount, sunderResistSC * amount, frostbiteResistSC * amount, stunResistSC * amount, asleepResistSC * amount, bleedResistSC * amount, poisonResistSC * amount, knockBackResistSC * amount);

        // If we changed our invunerability, unatrgetability or invisibility
        if (invulnerabilitySC != 0 || untargetabilitySC != 0 || invisibilitySC != 0)
            ChangePlayerStatusLocks(false, invulnerabilitySC * amount, invisibilitySC * amount, untargetabilitySC * amount);

        // If we changed the size, change them back.
        if (sizeSC != 0)
            ChangeSize(false, sizeSC * amount);

        if (currentStacks == 0)
            EndBuff();
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
    public void ChangeOffensiveStats(bool changeStatsChangeValue, float atkSpeedGain, float movespeedGain, float damagePercentageGain)
    {
        connectedPlayer.bonusAttackSpeed += atkSpeedGain;
        connectedPlayer.movespeedPercentMultiplier += movespeedGain;
        connectedPlayer.damageIncreaseMultiplier += damagePercentageGain;

        if (changeStatsChangeValue)
        {
            atkSpdSC = atkSpeedGain;
            movespeedSC = movespeedGain;
            damagePercentageSC = damagePercentageGain;
        }

        connectedPlayer.StatSetup(false, true);
    }

    // Used to change the characters health, manan, regens, armor, and mr
    public void ChangeDefensiveStats(bool changeStatsChangeValue, float healthGain, float healthRegenGain, float armorGain, float damageReductionGain)
    {
        connectedPlayer.bonusHealth += healthGain;
        connectedPlayer.bonusHealthRegen += healthRegenGain;
        connectedPlayer.ChangeArmor(armorGain);
        connectedPlayer.damageReductionMultiplier += damageReductionGain;

        if (changeStatsChangeValue)
        {
            healthSC = healthGain;
            healthRegenSC = healthRegenGain;
            armorSC = armorGain;
            damageReductionSC = damageReductionGain;
        }
        
        connectedPlayer.StatSetup(false, true);
    }

    // Used to change the players resistance to certain afflictions
    public void ChangeResistanceStats(bool changeStatsChangeValue, float aflameGain, float frostbiteGain, float overchargeGain, float overgrownGain, float sunderGain, float windshearGain, float stunGain, float asleepGain, float bleedGain, float poisonGain, float knockbackGain)
    {
        connectedPlayer.aflameResistance += aflameGain;
        connectedPlayer.frostbiteResistance += frostbiteGain;
        connectedPlayer.overchargeResistance += overchargeGain;
        connectedPlayer.overgrowthResistance += overgrownGain;
        connectedPlayer.sunderResistance += sunderGain;
        connectedPlayer.windshearResistance += windshearGain;
        connectedPlayer.stunResistance += stunGain;
        connectedPlayer.sleepResistance += asleepGain;
        connectedPlayer.bleedResistance += bleedGain;
        connectedPlayer.poisonResistance += poisonGain;
        connectedPlayer.knockbackResistance += knockbackGain;

        if (changeStatsChangeValue)
        {
            aflameResistSC = aflameGain;
            frostbiteResistSC = frostbiteGain;
            overchargeResistSC = overchargeGain;
            overgrowthResistSC = overgrownGain;
            sunderResistSC = sunderGain;
            windshearResistSC = windshearGain;
            stunResistSC = stunGain;
            asleepResistSC = asleepGain;
            bleedResistSC = bleedGain;
            poisonResistSC = poisonGain;
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
            connectedPlayer.stunned = false;
        else if (myType == BuffsManager.BuffType.Asleep)
            connectedPlayer.asleep = false;
        else if (myType == BuffsManager.BuffType.Frozen)
            connectedPlayer.frozen = false;
        else if (myType == BuffsManager.BuffType.Bleeding)
            connectedPlayer.bleeding = false;
        else if (myType == BuffsManager.BuffType.Knockback)
            connectedPlayer.knockedBack = false;
        /*else if (myType == BuffsManager.BuffType.Revitalize)
        {
            connectedPlayer.revitalizeBuff = false;
            connectedPlayer.healthRegen = connectedPlayer.baseHealthRegen + connectedPlayer.baseHealthRegenGrowth * connectedPlayer.level + connectedPlayer.bonusHealthRegen;
        }
        */
        else if (myType == BuffsManager.BuffType.FlameWalker)
            connectedPlayer.flameWalkerEnabled = false;
        else if (myType == BuffsManager.BuffType.Immolation)
        {
            connectedPlayer.immolationEnabled = false;
            connectedPlayer.GetComponent<BuffsManager>().AttemptRemovalOfBuff(BuffsManager.BuffType.Aflame);
        }
        else if (myType == BuffsManager.BuffType.GiantStrength)
            connectedPlayer.GetComponent<PlayerGearManager>().ResetToOriginalMaterial();

        // We do not change it if its a stackable buff since this method is called after we already remvoed all the stats associated with the buff, this would put us into negatives.
        if (!stackable)
        {

            // If we changed the defensive stats, change them back.
            if (healthSC != 0 || healthRegenSC != 0 || armorSC != 0 )
                ChangeDefensiveStats(true, healthSC * -1, healthRegenSC * -1, armorSC * -1, damageReductionSC * -1);

            // If we changed offensive stats, change em back.
            if (atkSpdSC != 0 || movespeedSC != 0 || damagePercentageSC != 0)
                ChangeOffensiveStats(true,  atkSpdSC * -1, movespeedSC * -1, damagePercentageSC * -1);

            // If we changed our resistance based stats, change em back.
            if (aflameResistSC != 0 || stunResistSC != 0 || asleepResistSC != 0 || bleedResistSC != 0 || poisonResistSC != 0 ||  frostbiteResistSC != 0 || knockBackResistSC != 0 || overchargeResistSC != 0 || overgrowthResistSC != 0|| sunderResistSC != 0 || windshearResistSC != 0)
                ChangeResistanceStats(true, aflameResistSC * -1, frostbiteResistSC * -1, overchargeResistSC * -1, overgrowthResistSC * -1, sunderResistSC * -1, frostbiteResistSC * -1, stunResistSC * -1, asleepResistSC * -1, bleedResistSC * -1, poisonResistSC * -1, knockBackResistSC * -1);

            // If we changed our invunerability, unatrgetability or invisibility
            if (invulnerabilitySC != 0 || untargetabilitySC != 0 || invisibilitySC != 0)
                ChangePlayerStatusLocks(true, invulnerabilitySC * -1, invisibilitySC * -1, untargetabilitySC * -1);

            // If we changed the size, change them back.
            if (sizeSC != 0)
                ChangeSize(true, sizeSC * -1);
        }
        else
        {
            // If we changed the defensive stats, change them back.
            if (healthSC != 0 || healthRegenSC != 0 || armorSC != 0)
                ChangeDefensiveStats(true, healthSC * -1 * currentStacks, healthRegenSC * -1 * currentStacks, armorSC * -1 * currentStacks, damageReductionSC * -1 * currentStacks);

            // If we changed offensive stats, change em back.
            if (atkSpdSC != 0 || movespeedSC != 0 || damagePercentageSC != 0)
                ChangeOffensiveStats(true, atkSpdSC * -1 * currentStacks, movespeedSC * -1 * currentStacks, damagePercentageSC * -1 * currentStacks);

            // If we changed our resistance based stats, change em back.
            if (aflameResistSC != 0 || stunResistSC != 0 || asleepResistSC != 0 || bleedResistSC != 0 || poisonResistSC != 0 || frostbiteResistSC != 0 || knockBackResistSC != 0 || overchargeResistSC != 0 || overgrowthResistSC != 0 || sunderResistSC != 0 || windshearResistSC != 0)
                ChangeResistanceStats(true, aflameResistSC * -1 * currentStacks, frostbiteResistSC * -1 * currentStacks, overchargeResistSC * -1 * currentStacks, overgrowthResistSC * -1 * currentStacks, sunderResistSC * -1 * currentStacks, frostbiteResistSC * -1 * currentStacks, stunResistSC * -1 * currentStacks, asleepResistSC * -1 * currentStacks, bleedResistSC * -1 * currentStacks, poisonResistSC * -1 * currentStacks, knockBackResistSC * -1 * currentStacks);

            // If we changed our invunerability, unatrgetability or invisibility
            if (invulnerabilitySC != 0 || untargetabilitySC != 0 || invisibilitySC != 0)
                ChangePlayerStatusLocks(true, invulnerabilitySC * -1 * currentStacks, invisibilitySC * -1 * currentStacks, untargetabilitySC * -1 * currentStacks);

            // If we changed the size, change them back.
            if (sizeSC != 0)
                ChangeSize(true, sizeSC * -1 * currentStacks);

        }
        // This contacts the buff manager, removi8ng us from their list of active buffs , deleting our icon, then killing this instance of the class. ALL fixes to stats should be done before this.
        connectedPlayer.GetComponent<BuffsManager>().RemoveBuff(this);
    }
}
