using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffsManager : MonoBehaviour
{
    public List<Buff> activeBuffs = new List<Buff>();
    public List<GameObject> activeIcons = new List<GameObject>();
    public Transform canvasParent;
    public GameObject buffIconPrefab;

    public List<ParticleSystem> weaponEffectsLeft = new List<ParticleSystem>();
    public List<ParticleSystem> weaponEffectsRight = new List<ParticleSystem>();

    public enum BuffType
    {
        Aflame, Frostbite, Overcharge, Overgrown, Sunder, Windshear, Knockback, Asleep, Stunned, Bleeding, Poisoned, Frozen, ArmorBroken, EmboldeningEmbers, FlameStrike, FlameWalker, BlessingOfFlames, Immolation, Glacier, FrostsKiss, IceArmor, StoneStrike,
        GiantStrength, StonePrison, SecondWind, WrathOftheWind, Multislash, PressureDrop, BasicAttacksShredArmorOnAflame, GreviousWounds
    };

    [SerializeField] private ParticleSystem[] psSystems;
    private PlayerStats stats;
    private EffectsManager effects;
    private SkillsManager skillManager;
    private DamageNumberManager uiPopupManager;
    private PlayerTraitManager playerTraitManager;

    private List<ParticleSystem.MinMaxCurve> weaponEffectRateOverTime = new List<ParticleSystem.MinMaxCurve>();


    // Grabs the players stats for use when calculating buff strength.
    private void Start()
    {
        foreach (ParticleSystem ps in weaponEffectsRight)
            weaponEffectRateOverTime.Add(ps.emission.rateOverTime);

        stats = GetComponent<PlayerStats>();
        uiPopupManager = GetComponent<DamageNumberManager>();
        effects = GetComponent<EffectsManager>();
        skillManager = GetComponent<SkillsManager>();

        if (CompareTag("Player"))
            playerTraitManager = GetComponent<PlayerTraitManager>();

        foreach (ParticleSystem ps in psSystems)
            if (ps != null)
                ps.Stop();

        foreach (ParticleSystem ps in weaponEffectsLeft)
            ps.Stop();
        foreach (ParticleSystem ps in weaponEffectsRight)
            ps.Stop();
    }

    // USed tyo see if our player resists the buff being added
    public void CheckResistanceToBuff(BuffType buff, int stackCount, float baseDamage, PlayerStats buffInflictor)
    {
        float resistance = 0;
        switch (buff)
        {
            case BuffType.Aflame:
                resistance = stats.aflameResistance;
                break;
            case BuffType.Frostbite:
                resistance = stats.frostbiteResistance;
                break;
            case BuffType.Frozen:
                resistance = stats.frostbiteResistance;
                break;
            case BuffType.Overcharge:
                resistance = stats.overchargeResistance;
                break;
            case BuffType.Overgrown:
                resistance = stats.overgrowthResistance;
                break;
            case BuffType.Sunder:
                resistance = stats.sunderResistance;
                break;
            case BuffType.Windshear:
                resistance = stats.windshearResistance;
                break;
            case BuffType.Asleep:
                resistance = stats.sleepResistance;
                break;
            case BuffType.Stunned:
                resistance = stats.stunResistance;
                break;
            case BuffType.Bleeding:
                resistance = stats.bleedResistance;
                break;
            case BuffType.Poisoned:
                resistance = stats.poisonResistance;
                break;
            default:
                break;
        }

        if (resistance < 100)
        {
            for (int index = 0; index < stackCount; index++)
            {
                if (Random.Range(0, 100) > resistance * 100)
                    NewBuff(buff, baseDamage, buffInflictor);
                else
                {
                    switch (buff)
                    {
                        case BuffType.Aflame:
                            break;
                        case BuffType.Frostbite:
                            break;
                        case BuffType.Frozen:
                            break;
                        case BuffType.Overcharge:
                            break;
                        case BuffType.Overgrown:
                            break;
                        case BuffType.Sunder:
                            break;
                        case BuffType.Windshear:
                            break;
                        case BuffType.Knockback:
                            break;
                        case BuffType.Asleep:
                            break;
                        case BuffType.Stunned:
                            break;
                        case BuffType.Bleeding:
                            break;
                        case BuffType.Poisoned:
                            break;
                        default:
                            break;
                    }
                }
                //here i would spawn a resisted 
                //Debug.Log("resisted");
            }
        }
        else
        {
            for (int index = 0; index < stackCount; index++)
            {
                //here i would spawn a resisted 
                //Debug.Log("resisted");
            }
        }
    }

    // Used to add a new buff to oiur player, if they already have it from this source, we refresh it instead.
    public void NewBuff(BuffType buff, float baseDamage, PlayerStats buffInflictor)
    {
        // Debug.Log("Addding buffs");
        bool buffDealtWith = false;

        // Check to see if any of our buffs match this buff, and if the source matches then we reset the duration.
        for (int index = 0; index < activeBuffs.Count; index++)
        {
            Buff activeBuff = activeBuffs[index];
            if (activeBuff.myType == buff)
            {
                buffDealtWith = true;
                if (activeBuff.stackable)
                {
                    activeBuff.AddStack(1);
                    if (buffInflictor.CompareTag("Player") && buff == BuffType.Aflame && activeBuff.currentStacks >= 32 - buffInflictor.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.AflameBleedAflameAddsBleedAtThreshhold) && buffInflictor.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.AflameBleedAflameAddsBleedAtThreshhold) > 0)
                        CheckResistanceToBuff(BuffType.Bleeding, 1, buffInflictor.baseDamage, buffInflictor);
                }
                else
                {
                    activeBuff.AddTime(0, true);

                    if (CompareTag("Enemy"))
                        if (buff == BuffType.Asleep || buff == BuffType.Stunned || buff == BuffType.Frozen)
                            GetComponent<EnemyMovementManager>().StopMovement();
                }
                break;
            }

            switch (buff)
            {
                case BuffType.Aflame:
                    if (activeBuff.myType == BuffType.Frostbite)
                    {
                        activeBuff.RemoveStacks(1, false);
                        buffDealtWith = true;
                    }
                    break;
                case BuffType.Frostbite:
                    if (activeBuff.myType == BuffType.Aflame)
                    {
                        activeBuff.RemoveStacks(1, false);
                        buffDealtWith = true;
                    }
                    break;
                case BuffType.Frozen:
                    if (activeBuff.myType == BuffType.Aflame)
                    {
                        activeBuff.RemoveStacks(5, false);
                        buffDealtWith = true;
                    }
                    break;
                case BuffType.Overcharge:
                    if (activeBuff.myType == BuffType.Overgrown)
                    {
                        activeBuff.RemoveStacks(1, false);
                        buffDealtWith = true;
                    }
                    break;
                case BuffType.Overgrown:
                    if (activeBuff.myType == BuffType.Overcharge)
                    {
                        activeBuff.RemoveStacks(1, false);
                        buffDealtWith = true;
                    }
                    break;
                case BuffType.Sunder:
                    if (activeBuff.myType == BuffType.Windshear)
                    {
                        activeBuff.RemoveStacks(1, false);
                        buffDealtWith = true;
                    }
                    break;
                case BuffType.Windshear:
                    if (activeBuff.myType == BuffType.Sunder)
                    {
                        activeBuff.RemoveStacks(1, false);
                        buffDealtWith = true;
                    }
                    break;
                default:
                    break;
            }
        }

        // If the buff was not dealt with above, begin the activation process of instantiated a new buff.
        if (!buffDealtWith)
        {
            GameObject buffIcon = Instantiate(buffIconPrefab, canvasParent);
            activeIcons.Add(buffIcon);
            UpdateIconLocations();

            // Look through to see what the buff is and tack it on to our new buff.
            switch (buff)
            {
                case BuffType.Aflame:

                    Buff aflame = transform.Find("BuffContainer").gameObject.AddComponent<Buff>();
                    aflame.connectedIcon = buffIcon;
                    aflame.iconStacks = buffIcon.GetComponentInChildren<Text>();
                    buffIcon.GetComponent<Image>().sprite = BuffIconBank.instance.buffIcons[0];
                    buffIcon.GetComponent<Image>().color = BuffIconBank.instance.buffColors[0];

                    activeBuffs.Add(aflame);

                    aflame.myType = buff;
                    aflame.maxStacks = 100;
                    aflame.currentStacks = 1;
                    aflame.stackable = true;
                    aflame.stackSingleFalloff = false;
                    aflame.connectedPlayer = stats;
                    aflame.playerDamageSource = buffInflictor;
                    aflame.infiniteDuration = false;
                    aflame.duration = 10;

                    if (buffInflictor.CompareTag("Player") && buffInflictor.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.AflameBleedAflameRemovesBleedResist) > 0)
                        aflame.ChangeResistanceStats(true, 0, 0, 0, 0, 0, 0, 0, 0, buffInflictor.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.AflameBleedAflameRemovesBleedResist) * -1, 0, 0);

                    if (buffInflictor.CompareTag("Player") && buffInflictor.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.AflameKnockbackAflameReducesKnockbackResist) > 0)
                        aflame.ChangeResistanceStats(true, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, buffInflictor.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.AflameKnockbackAflameReducesKnockbackResist) * -1);

                    if (PollForBuffStacks(BuffType.Poisoned) > 0 && buffInflictor.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.AflamePoisonFireAmpsPoison) > 0)
                        PollForBuff(BuffType.Poisoned).DPSMultiplier += buffInflictor.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.AflamePoisonFireAmpsPoison);

                    aflame.DPS = baseDamage * 0.2f;
                    aflame.damageType = HitBox.DamageType.Fire;

                    aflame.effectParticleSystem.Add(psSystems[0]);
                    psSystems[0].Play();
                    break;
                case BuffType.Frostbite:

                    Buff frostbite = transform.Find("BuffContainer").gameObject.AddComponent<Buff>();
                    frostbite.connectedIcon = buffIcon;
                    frostbite.iconStacks = buffIcon.GetComponentInChildren<Text>();
                    buffIcon.GetComponent<Image>().sprite = BuffIconBank.instance.buffIcons[1];
                    buffIcon.GetComponent<Image>().color = BuffIconBank.instance.buffColors[1];

                    activeBuffs.Add(frostbite);

                    frostbite.myType = buff;
                    frostbite.maxStacks = 100;
                    frostbite.currentStacks = 1;
                    frostbite.stackable = true;
                    frostbite.stackSingleFalloff = false;
                    frostbite.connectedPlayer = stats;
                    frostbite.playerDamageSource = buffInflictor;
                    frostbite.infiniteDuration = false;
                    frostbite.duration = 10;

                    frostbite.DPS = baseDamage * 0.05f;
                    frostbite.damageType = HitBox.DamageType.Ice;
                    frostbite.ChangeOffensiveStats(true, -0.01f, -0.01f, 0);

                    frostbite.effectParticleSystem.Add(psSystems[1]);
                    frostbite.effectParticleSystem.Add(psSystems[2]);
                    psSystems[1].Play();
                    psSystems[2].Play();

                    break;
                case BuffType.Frozen:

                    Buff frozen = transform.Find("BuffContainer").gameObject.AddComponent<Buff>();
                    frozen.connectedIcon = buffIcon;
                    buffIcon.GetComponent<Image>().sprite = BuffIconBank.instance.buffIcons[7];
                    buffIcon.GetComponent<Image>().color = BuffIconBank.instance.buffColors[1];

                    activeBuffs.Add(frozen);

                    frozen.myType = buff;
                    frozen.connectedPlayer = stats;
                    frozen.playerDamageSource = buffInflictor;
                    frozen.infiniteDuration = false;
                    frozen.duration = 5f;

                    if (CompareTag("Player"))
                        GetComponent<PlayerMovementController>().FrozenLaunch();
                    else
                        GetComponent<EnemyCrowdControlManager>().FrozenLaunch();

                    frozen.effectParticleSystem.Add(psSystems[12]);
                    psSystems[12].Play();

                    break;
                case BuffType.Overcharge:

                    Buff overcharge = transform.Find("BuffContainer").gameObject.AddComponent<Buff>();
                    overcharge.connectedIcon = buffIcon;
                    overcharge.iconStacks = buffIcon.GetComponentInChildren<Text>();
                    buffIcon.GetComponent<Image>().sprite = BuffIconBank.instance.buffIcons[2];
                    buffIcon.GetComponent<Image>().color = BuffIconBank.instance.buffColors[2];

                    activeBuffs.Add(overcharge);

                    overcharge.myType = buff;
                    overcharge.maxStacks = 100;
                    overcharge.currentStacks = 1;
                    overcharge.stackable = true;
                    overcharge.stackSingleFalloff = false;
                    overcharge.connectedPlayer = stats;
                    overcharge.playerDamageSource = buffInflictor;
                    overcharge.infiniteDuration = false;
                    overcharge.duration = 10;

                    overcharge.effectParticleSystem.Add(psSystems[3]);
                    psSystems[3].Play();

                    break;
                case BuffType.Overgrown:

                    Buff overgrown = transform.Find("BuffContainer").gameObject.AddComponent<Buff>();
                    overgrown.connectedIcon = buffIcon;
                    overgrown.iconStacks = buffIcon.GetComponentInChildren<Text>();
                    buffIcon.GetComponent<Image>().sprite = BuffIconBank.instance.buffIcons[3];
                    buffIcon.GetComponent<Image>().color = BuffIconBank.instance.buffColors[3];

                    activeBuffs.Add(overgrown);

                    overgrown.myType = buff;
                    overgrown.maxStacks = 100;
                    overgrown.currentStacks = 1;
                    overgrown.stackable = true;
                    overgrown.stackSingleFalloff = false;
                    overgrown.connectedPlayer = stats;
                    overgrown.playerDamageSource = buffInflictor;
                    overgrown.infiniteDuration = false;
                    overgrown.duration = 10;

                    overgrown.effectParticleSystem.Add(psSystems[5]);
                    psSystems[5].Play();

                    break;
                case BuffType.Windshear:

                    Buff windshear = transform.Find("BuffContainer").gameObject.AddComponent<Buff>();
                    windshear.connectedIcon = buffIcon;
                    windshear.iconStacks = buffIcon.GetComponentInChildren<Text>();
                    buffIcon.GetComponent<Image>().sprite = BuffIconBank.instance.buffIcons[4];
                    buffIcon.GetComponent<Image>().color = BuffIconBank.instance.buffColors[4];

                    activeBuffs.Add(windshear);

                    windshear.myType = buff;
                    windshear.maxStacks = 100;
                    windshear.currentStacks = 1;
                    windshear.stackable = true;
                    windshear.stackSingleFalloff = false;
                    windshear.connectedPlayer = stats;
                    windshear.playerDamageSource = buffInflictor;
                    windshear.infiniteDuration = false;
                    windshear.duration = 10;

                    windshear.ChangeDefensiveStats(true, 0f, 0f, -0.01f, 0f, 0f);

                    windshear.effectParticleSystem.Add(psSystems[7]);
                    psSystems[7].Play();

                    break;
                case BuffType.Sunder:

                    Buff sundered = transform.Find("BuffContainer").gameObject.AddComponent<Buff>();
                    sundered.connectedIcon = buffIcon;
                    sundered.iconStacks = buffIcon.GetComponentInChildren<Text>();
                    buffIcon.GetComponent<Image>().sprite = BuffIconBank.instance.buffIcons[5];
                    buffIcon.GetComponent<Image>().color = BuffIconBank.instance.buffColors[5];

                    activeBuffs.Add(sundered);

                    sundered.myType = buff;
                    sundered.maxStacks = 100;
                    sundered.currentStacks = 1;
                    sundered.stackable = true;
                    sundered.stackSingleFalloff = false;
                    sundered.connectedPlayer = stats;
                    sundered.playerDamageSource = buffInflictor;
                    sundered.infiniteDuration = false;
                    sundered.duration = 10;

                    if(buffInflictor.CompareTag("Player"))
                        sundered.ChangeResistanceStats(true, -0.01f * (buffInflictor.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.SunderFurtherDecreasesFireResist) + 1), -0.01f, -0.01f, -0.01f, -0.01f, -0.01f, -0.01f, -0.01f, -0.01f, -0.01f, -0.01f);
                    else
                        sundered.ChangeResistanceStats(true, -0.01f, -0.01f, -0.01f, -0.01f, -0.01f, -0.01f, -0.01f, -0.01f, -0.01f, -0.01f, -0.01f);

                    sundered.effectParticleSystem.Add(psSystems[8]);
                    psSystems[8].Play();

                    break;
                case BuffType.Bleeding:

                    Buff bleeding = transform.Find("BuffContainer").gameObject.AddComponent<Buff>();
                    bleeding.connectedIcon = buffIcon;
                    bleeding.iconStacks = buffIcon.GetComponentInChildren<Text>();
                    buffIcon.GetComponent<Image>().sprite = BuffIconBank.instance.buffIcons[6];
                    buffIcon.GetComponent<Image>().color = BuffIconBank.instance.buffColors[6];

                    activeBuffs.Add(bleeding);

                    bleeding.myType = buff;
                    bleeding.maxStacks = 100;
                    bleeding.currentStacks = 1;
                    bleeding.stackable = true;
                    bleeding.stackSingleFalloff = false;
                    bleeding.connectedPlayer = stats;
                    bleeding.playerDamageSource = buffInflictor;
                    bleeding.infiniteDuration = false;
                    bleeding.duration = 5;

                    bleeding.DPS = baseDamage * 0.3f;
                    bleeding.damageType = HitBox.DamageType.Bleed;

                    stats.bleeding = true;

                    bleeding.effectParticleSystem.Add(psSystems[9]);
                    psSystems[9].Play();
                    psSystems[10].Play();

                    break;
                case BuffType.Poisoned:
                    Buff poisoned = transform.Find("BuffContainer").gameObject.AddComponent<Buff>();
                    poisoned.connectedIcon = buffIcon;
                    poisoned.iconStacks = buffIcon.GetComponentInChildren<Text>();
                    buffIcon.GetComponent<Image>().sprite = BuffIconBank.instance.buffIcons[6];
                    buffIcon.GetComponent<Image>().color = BuffIconBank.instance.buffColors[7];

                    activeBuffs.Add(poisoned);

                    poisoned.myType = buff;
                    poisoned.maxStacks = 100;
                    poisoned.currentStacks = 1;
                    poisoned.stackable = true;
                    poisoned.stackSingleFalloff = false;
                    poisoned.connectedPlayer = stats;
                    poisoned.playerDamageSource = buffInflictor;
                    poisoned.infiniteDuration = false;
                    poisoned.duration = 20;

                    if (buffInflictor.CompareTag("Player") && buffInflictor.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.AflamePoisonPoisonReducesFireResist) > 0)
                        poisoned.ChangeResistanceStats(true, buffInflictor.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.AflamePoisonPoisonReducesFireResist) * -1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);

                    if (PollForBuffStacks(BuffType.Aflame) > 0 && buffInflictor.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.AflamePoisonFireAmpsPoison) > 0)
                        poisoned.DPSMultiplier += buffInflictor.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.AflamePoisonFireAmpsPoison) * PollForBuffStacks(BuffType.Aflame);

                    if (stats.healthMax * 0.001f < baseDamage * 0.1f)
                        poisoned.DPS = baseDamage * 0.1f;
                    else if (stats.healthMax * 0.001f > baseDamage * 5)
                        poisoned.DPS = baseDamage * 5f;
                    else
                        poisoned.DPS = stats.healthMax * 0.001f;

                    poisoned.damageType = HitBox.DamageType.Poison;

                    poisoned.effectParticleSystem.Add(psSystems[11]);
                    psSystems[11].Play();
                    break;
                case BuffType.Asleep:

                    Buff asleep = transform.Find("BuffContainer").gameObject.AddComponent<Buff>();
                    asleep.connectedIcon = buffIcon;
                    buffIcon.GetComponent<Image>().sprite = BuffIconBank.instance.buffIcons[8];
                    buffIcon.GetComponent<Image>().color = BuffIconBank.instance.buffColors[8];

                    activeBuffs.Add(asleep);

                    asleep.myType = buff;
                    asleep.connectedPlayer = stats;
                    asleep.playerDamageSource = buffInflictor;
                    asleep.infiniteDuration = false;
                    asleep.duration = 5;
                    asleep.DPS = 0;

                    if (CompareTag("Player"))
                        GetComponent<PlayerMovementController>().AsleepLaunch();
                    else
                        GetComponent<EnemyCrowdControlManager>().AsleepLaunch();

                    asleep.effectParticleSystem.Add(psSystems[13]);
                    asleep.effectParticleSystem.Add(psSystems[14]);
                    psSystems[13].Play();
                    psSystems[14].Play();

                    break;
                case BuffType.Stunned:

                    Buff stunned = transform.Find("BuffContainer").gameObject.AddComponent<Buff>();
                    stunned.connectedIcon = buffIcon;
                    buffIcon.GetComponent<Image>().sprite = BuffIconBank.instance.buffIcons[9];
                    buffIcon.GetComponent<Image>().color = BuffIconBank.instance.buffColors[9];

                    activeBuffs.Add(stunned);
                    stunned.myType = buff;
                    stunned.connectedPlayer = stats;
                    stunned.playerDamageSource = buffInflictor;
                    stunned.infiniteDuration = false;
                    stunned.duration = 2;
                    stunned.DPS = 0;

                    if (CompareTag("Player"))
                        GetComponent<PlayerMovementController>().StunLaunch();
                    else
                        GetComponent<EnemyCrowdControlManager>().StunLaunch();


                    if (buffInflictor.CompareTag("Player") && buffInflictor.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.AflameStunStunReducesFireResistance) > 0)
                        stunned.ChangeResistanceStats(true, buffInflictor.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.AflameStunStunReducesFireResistance) * -1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);

                    if (buffInflictor.CompareTag("Player") && buffInflictor.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.AflameStunStunAmpsBurnDamage) > 0 && PollForBuff(BuffType.Aflame))
                        PollForBuff(BuffType.Aflame).bonusDPS += buffInflictor.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.AflameStunStunAmpsBurnDamage);

                    stunned.effectParticleSystem.Add(psSystems[15]);
                    stunned.effectParticleSystem.Add(psSystems[16]);
                    psSystems[15].Play();
                    psSystems[16].Play();

                    break;
                case BuffType.Knockback:

                    Buff knockback = transform.Find("BuffContainer").gameObject.AddComponent<Buff>();
                    knockback.connectedIcon = buffIcon;
                    buffIcon.GetComponent<Image>().sprite = BuffIconBank.instance.buffIcons[10];
                    buffIcon.GetComponent<Image>().color = BuffIconBank.instance.buffColors[10];

                    activeBuffs.Add(knockback);

                    knockback.myType = buff;
                    knockback.connectedPlayer = stats;
                    knockback.playerDamageSource = buffInflictor;
                    knockback.infiniteDuration = true;
                    knockback.DPS = 0;

                    //knockback.effectParticleSystem.Add(psSystems[15]);
                    //knockback.effectParticleSystem.Add(psSystems[16]);
                    //psSystems[15].Play();
                    //psSystems[16].Play();

                    break;
                case BuffType.ArmorBroken:

                    Buff armorBroken = transform.Find("BuffContainer").gameObject.AddComponent<Buff>();
                    armorBroken.connectedIcon = buffIcon;
                    buffIcon.GetComponent<Image>().sprite = BuffIconBank.instance.buffIcons[11];
                    buffIcon.GetComponent<Image>().color = BuffIconBank.instance.buffColors[9];

                    activeBuffs.Add(armorBroken);

                    armorBroken.myType = buff;
                    armorBroken.connectedPlayer = stats;
                    armorBroken.playerDamageSource = buffInflictor;
                    armorBroken.infiniteDuration = false;
                    armorBroken.duration = 10;

                    armorBroken.ChangeDefensiveStats(true, 0f, 0f, -0.4f, 0f, 0f);

                    psSystems[17].Play();

                    break;
                case BuffType.EmboldeningEmbers:
                    Buff embers = transform.Find("BuffContainer").gameObject.AddComponent<Buff>();
                    embers.connectedIcon = buffIcon;
                    buffIcon.GetComponent<Image>().sprite = BuffIconBank.instance.buffIcons[12];
                    buffIcon.GetComponent<Image>().color = BuffIconBank.instance.buffColors[0];

                    activeBuffs.Add(embers);

                    embers.myType = buff;
                    embers.connectedPlayer = stats;
                    embers.playerDamageSource = buffInflictor;
                    embers.infiniteDuration = false;
                    embers.ChangeOffensiveStats(true, 0.5f, 0.25f, 0);
                    embers.ChangeDefensiveStats(true, stats.healthMax * 0.2f, 0, 0, 0, 0);
                    embers.duration = 15;

                    embers.effectParticleSystem.Add(psSystems[19]);
                    psSystems[18].Play();
                    psSystems[19].Play();

                    break;

                case BuffType.FlameStrike:
                    Buff flameStrike = transform.Find("BuffContainer").gameObject.AddComponent<Buff>();
                    flameStrike.connectedIcon = buffIcon;
                    buffIcon.GetComponent<Image>().sprite = BuffIconBank.instance.buffIcons[13];
                    buffIcon.GetComponent<Image>().color = BuffIconBank.instance.buffColors[0];

                    activeBuffs.Add(flameStrike);

                    flameStrike.myType = buff;
                    flameStrike.connectedPlayer = stats;
                    flameStrike.playerDamageSource = buffInflictor;
                    flameStrike.infiniteDuration = false;
                    flameStrike.duration = 15;

                    flameStrike.effectParticleSystem.Add(weaponEffectsLeft[0]);
                    flameStrike.effectParticleSystem.Add(weaponEffectsRight[0]);

                    weaponEffectsLeft[0].Play();
                    weaponEffectsRight[0].Play();

                    break;
                case BuffType.FlameWalker:
                    Buff flameWalker = transform.Find("BuffContainer").gameObject.AddComponent<Buff>();
                    flameWalker.connectedIcon = buffIcon;
                    buffIcon.GetComponent<Image>().sprite = BuffIconBank.instance.buffIcons[14];
                    buffIcon.GetComponent<Image>().color = BuffIconBank.instance.buffColors[0];

                    activeBuffs.Add(flameWalker);

                    flameWalker.myType = buff;
                    flameWalker.connectedPlayer = stats;
                    flameWalker.playerDamageSource = buffInflictor;
                    flameWalker.infiniteDuration = false;
                    flameWalker.duration = 15;
                    flameWalker.ChangeOffensiveStats(true, 0, 0.5f, 0);

                    stats.flameWalkerEnabled = true;

                    flameWalker.effectParticleSystem.Add(psSystems[20]);
                    flameWalker.effectParticleSystem.Add(psSystems[21]);
                    flameWalker.effectParticleSystem.Add(psSystems[22]);

                    psSystems[20].Play();
                    psSystems[21].Play();
                    psSystems[22].Play();

                    break;
                case BuffType.BlessingOfFlames:
                    Buff blessingOfFlames = transform.Find("BuffContainer").gameObject.AddComponent<Buff>();
                    blessingOfFlames.connectedIcon = buffIcon;
                    buffIcon.GetComponent<Image>().sprite = BuffIconBank.instance.buffIcons[15];
                    buffIcon.GetComponent<Image>().color = BuffIconBank.instance.buffColors[0];

                    activeBuffs.Add(blessingOfFlames);

                    blessingOfFlames.myType = buff;
                    blessingOfFlames.connectedPlayer = stats;
                    blessingOfFlames.playerDamageSource = buffInflictor;
                    blessingOfFlames.infiniteDuration = false;
                    blessingOfFlames.duration = 15;
                    blessingOfFlames.ChangeDefensiveStats(true, 0, 5, 0, 0.25f, 0);
                    blessingOfFlames.ChangeResistanceStats(true, 0.25f, 0.25f, 0.25f, 0.25f, 0.25f, 0.25f, 0.25f, 0.25f, 0.25f, 0.25f, 0.25f);

                    blessingOfFlames.effectParticleSystem.Add(psSystems[23]);

                    psSystems[23].Play();

                    break;
                case BuffType.Immolation:
                    Buff immolation = transform.Find("BuffContainer").gameObject.AddComponent<Buff>();
                    immolation.connectedIcon = buffIcon;
                    buffIcon.GetComponent<Image>().sprite = BuffIconBank.instance.buffIcons[16];
                    buffIcon.GetComponent<Image>().color = BuffIconBank.instance.buffColors[0];

                    activeBuffs.Add(immolation);

                    immolation.myType = buff;
                    immolation.connectedPlayer = stats;
                    immolation.playerDamageSource = buffInflictor;
                    immolation.infiniteDuration = false;
                    immolation.duration = 10;
                    immolation.ChangeResistanceStats(true, -10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);

                    for (int index = 0; index < 30; index++)
                        NewBuff(BuffType.Aflame, stats.baseDamage, stats);

                    stats.immolationEnabled = true;

                    immolation.effectParticleSystem.Add(psSystems[24]);

                    psSystems[24].Play();

                    break;

                case BuffType.Glacier:
                    Buff glacier = transform.Find("BuffContainer").gameObject.AddComponent<Buff>();
                    glacier.connectedIcon = buffIcon;
                    buffIcon.GetComponent<Image>().sprite = BuffIconBank.instance.buffIcons[17];
                    buffIcon.GetComponent<Image>().color = BuffIconBank.instance.buffColors[1];

                    activeBuffs.Add(glacier);

                    glacier.myType = buff;
                    glacier.connectedPlayer = stats;
                    glacier.playerDamageSource = buffInflictor;
                    glacier.infiniteDuration = false;
                    glacier.duration = 5;
                    glacier.ChangeDefensiveStats(true, 0, stats.healthMax / 10, 0, 0, 0);
                    glacier.ChangePlayerStatusLocks(true, 1, 0, 0);

                    NewBuff(BuffType.Frozen, 0, stats);

                    break;

                case BuffType.FrostsKiss:
                    Buff frostsKiss = transform.Find("BuffContainer").gameObject.AddComponent<Buff>();
                    frostsKiss.connectedIcon = buffIcon;
                    buffIcon.GetComponent<Image>().sprite = BuffIconBank.instance.buffIcons[18];
                    buffIcon.GetComponent<Image>().color = BuffIconBank.instance.buffColors[1];

                    activeBuffs.Add(frostsKiss);

                    frostsKiss.myType = buff;
                    frostsKiss.connectedPlayer = stats;
                    frostsKiss.playerDamageSource = buffInflictor;
                    frostsKiss.infiniteDuration = false;
                    frostsKiss.duration = 15;

                    frostsKiss.effectParticleSystem.Add(weaponEffectsLeft[3]);
                    frostsKiss.effectParticleSystem.Add(weaponEffectsRight[3]);

                    weaponEffectsLeft[3].Play();
                    weaponEffectsRight[3].Play();

                    break;

                case BuffType.IceArmor:
                    Buff iceArmor = transform.Find("BuffContainer").gameObject.AddComponent<Buff>();
                    iceArmor.connectedIcon = buffIcon;
                    buffIcon.GetComponent<Image>().sprite = BuffIconBank.instance.buffIcons[19];
                    buffIcon.GetComponent<Image>().color = BuffIconBank.instance.buffColors[1];

                    activeBuffs.Add(iceArmor);

                    iceArmor.myType = buff;
                    iceArmor.connectedPlayer = stats;
                    iceArmor.playerDamageSource = buffInflictor;
                    iceArmor.infiniteDuration = false;
                    iceArmor.duration = 15;
                    iceArmor.ChangeDefensiveStats(true, 0, 0, stats.armor * 0.3f, 0.25f, 0);
                    iceArmor.ChangeOffensiveStats(true, 0, -0.25f, 0);

                    iceArmor.effectParticleSystem.Add(psSystems[25]);

                    psSystems[25].Play();

                    break;

                case BuffType.StoneStrike:
                    Buff stoneStrike = transform.Find("BuffContainer").gameObject.AddComponent<Buff>();
                    stoneStrike.connectedIcon = buffIcon;
                    buffIcon.GetComponent<Image>().sprite = BuffIconBank.instance.buffIcons[20];
                    buffIcon.GetComponent<Image>().color = BuffIconBank.instance.buffColors[5];

                    activeBuffs.Add(stoneStrike);

                    stoneStrike.myType = buff;
                    stoneStrike.connectedPlayer = stats;
                    stoneStrike.playerDamageSource = buffInflictor;
                    stoneStrike.infiniteDuration = false;
                    stoneStrike.duration = 15;

                    stoneStrike.effectParticleSystem.Add(weaponEffectsLeft[7]);
                    stoneStrike.effectParticleSystem.Add(weaponEffectsRight[7]);

                    weaponEffectsLeft[7].Play();
                    weaponEffectsRight[7].Play();

                    break;

                case BuffType.GiantStrength:
                    Buff giantStrength = transform.Find("BuffContainer").gameObject.AddComponent<Buff>();
                    giantStrength.connectedIcon = buffIcon;
                    buffIcon.GetComponent<Image>().sprite = BuffIconBank.instance.buffIcons[21];
                    buffIcon.GetComponent<Image>().color = BuffIconBank.instance.buffColors[5];

                    activeBuffs.Add(giantStrength);

                    giantStrength.myType = buff;
                    giantStrength.infiniteDuration = false;
                    giantStrength.duration = 15f;
                    giantStrength.connectedPlayer = stats;
                    giantStrength.playerDamageSource = buffInflictor;
                    giantStrength.ChangeSize(true, 0.5f);
                    giantStrength.ChangeDefensiveStats(true, 0, 0, 0, -0.25f, 0);
                    giantStrength.ChangeOffensiveStats(true, stats.attackSpeed * -0.4f, stats.movespeedPercentMultiplier * -0.4f, stats.damageIncreaseMultiplier * 0.5f);

                    giantStrength.effectParticleSystem.Add(psSystems[26]);

                    psSystems[26].Play();

                    GetComponent<PlayerGearManager>().AddMaterialOverride(PlayerGearManager.MaterialOverrideCode.GiantStrength);

                    break;

                case BuffType.StonePrison:
                    Buff stonePrison = transform.Find("BuffContainer").gameObject.AddComponent<Buff>();
                    stonePrison.connectedIcon = buffIcon;
                    buffIcon.GetComponent<Image>().sprite = BuffIconBank.instance.buffIcons[22];
                    buffIcon.GetComponent<Image>().color = BuffIconBank.instance.buffColors[5];

                    activeBuffs.Add(stonePrison);

                    stonePrison.myType = buff;
                    stonePrison.infiniteDuration = false;
                    stonePrison.duration = 1f;
                    stonePrison.connectedPlayer = stats;
                    stonePrison.playerDamageSource = buffInflictor;
                    stonePrison.ChangeDefensiveStats(true, 0, 0, 0, 0.5f, 0);

                    break;

                case BuffType.SecondWind:
                    Buff secondWind = transform.Find("BuffContainer").gameObject.AddComponent<Buff>();
                    secondWind.connectedIcon = buffIcon;
                    buffIcon.GetComponent<Image>().sprite = BuffIconBank.instance.buffIcons[23];
                    buffIcon.GetComponent<Image>().color = BuffIconBank.instance.buffColors[4];

                    activeBuffs.Add(secondWind);

                    secondWind.myType = buff;
                    secondWind.infiniteDuration = false;
                    secondWind.duration = 6f;
                    secondWind.connectedPlayer = stats;
                    secondWind.playerDamageSource = buffInflictor;
                    secondWind.ChangeOffensiveStats(true, 0.15f, 0.5f, 0);

                    secondWind.effectParticleSystem.Add(psSystems[27]);
                    secondWind.effectParticleSystem.Add(psSystems[28]);

                    psSystems[27].Play();
                    psSystems[28].Play();

                    break;

                case BuffType.WrathOftheWind:
                    Buff wrathOfTheWind = transform.Find("BuffContainer").gameObject.AddComponent<Buff>();
                    wrathOfTheWind.connectedIcon = buffIcon;
                    buffIcon.GetComponent<Image>().sprite = BuffIconBank.instance.buffIcons[24];
                    buffIcon.GetComponent<Image>().color = BuffIconBank.instance.buffColors[4];

                    activeBuffs.Add(wrathOfTheWind);

                    wrathOfTheWind.myType = buff;
                    wrathOfTheWind.infiniteDuration = false;
                    wrathOfTheWind.duration = 15f;
                    wrathOfTheWind.connectedPlayer = stats;
                    wrathOfTheWind.playerDamageSource = buffInflictor;
                    wrathOfTheWind.ChangeOffensiveStats(true, stats.attackSpeed, 0, stats.damageIncreaseMultiplier * -0.5f);

                    wrathOfTheWind.effectParticleSystem.Add(weaponEffectsLeft[11]);
                    wrathOfTheWind.effectParticleSystem.Add(weaponEffectsRight[11]);

                    weaponEffectsLeft[11].Play();
                    weaponEffectsRight[11].Play();

                    break;

                case BuffType.Multislash:
                    Buff multislash = transform.Find("BuffContainer").gameObject.AddComponent<Buff>();
                    multislash.connectedIcon = buffIcon;
                    buffIcon.GetComponent<Image>().sprite = BuffIconBank.instance.buffIcons[25];
                    buffIcon.GetComponent<Image>().color = BuffIconBank.instance.buffColors[4];

                    activeBuffs.Add(multislash);

                    multislash.myType = buff;
                    multislash.infiniteDuration = false;
                    multislash.duration = 10f;
                    multislash.connectedPlayer = stats;
                    multislash.playerDamageSource = buffInflictor;

                    multislash.effectParticleSystem.Add(psSystems[29]);

                    psSystems[29].Play();

                    break;

                case BuffType.PressureDrop:
                    Buff pressureDrop = transform.Find("BuffContainer").gameObject.AddComponent<Buff>();
                    pressureDrop.connectedIcon = buffIcon;
                    buffIcon.GetComponent<Image>().sprite = BuffIconBank.instance.buffIcons[26];
                    buffIcon.GetComponent<Image>().color = BuffIconBank.instance.buffColors[4];

                    activeBuffs.Add(pressureDrop);

                    pressureDrop.myType = buff;
                    pressureDrop.infiniteDuration = false;
                    pressureDrop.duration = 10f;
                    pressureDrop.connectedPlayer = stats;
                    pressureDrop.playerDamageSource = buffInflictor;
                    pressureDrop.ChangeDefensiveStats(true, 0, 0, 0, 1.5f, 0);
                    pressureDrop.ChangeOffensiveStats(true, 0, stats.movespeedPercentMultiplier * -0.5f, 0);

                    break;

                case BuffType.BasicAttacksShredArmorOnAflame:
                    Buff basicAttacksShredArmorOnAflame = transform.Find("BuffContainer").gameObject.AddComponent<Buff>();
                    basicAttacksShredArmorOnAflame.connectedIcon = buffIcon;
                    basicAttacksShredArmorOnAflame.iconStacks = buffIcon.GetComponentInChildren<Text>();
                    buffIcon.GetComponent<Image>().sprite = BuffIconBank.instance.buffIcons[11];
                    buffIcon.GetComponent<Image>().color = BuffIconBank.instance.buffColors[0];

                    activeBuffs.Add(basicAttacksShredArmorOnAflame);

                    basicAttacksShredArmorOnAflame.myType = buff;
                    basicAttacksShredArmorOnAflame.maxStacks = buffInflictor.GetComponent<PlayerTraitManager>().CheckForOnHitValue(ItemTrait.TraitType.BasicAttacksShredArmorOnAflame);
                    basicAttacksShredArmorOnAflame.currentStacks = 1;
                    basicAttacksShredArmorOnAflame.stackable = true;
                    basicAttacksShredArmorOnAflame.stackSingleFalloff = false;
                    basicAttacksShredArmorOnAflame.infiniteDuration = false;
                    basicAttacksShredArmorOnAflame.duration = 10f;
                    basicAttacksShredArmorOnAflame.connectedPlayer = stats;
                    basicAttacksShredArmorOnAflame.playerDamageSource = buffInflictor;
                    basicAttacksShredArmorOnAflame.ChangeDefensiveStats(true, 0, 0,-0.05f, 0, 0);

                    break;
                    
                case BuffType.GreviousWounds:
                    Buff greviousWounds = transform.Find("BuffContainer").gameObject.AddComponent<Buff>();
                    greviousWounds.connectedIcon = buffIcon;
                    buffIcon.GetComponent<Image>().sprite = BuffIconBank.instance.buffIcons[27];
                    buffIcon.GetComponent<Image>().color = BuffIconBank.instance.buffColors[11];

                    activeBuffs.Add(greviousWounds);

                    greviousWounds.myType = buff;
                    greviousWounds.duration = 10f;
                    greviousWounds.connectedPlayer = stats;
                    greviousWounds.playerDamageSource = buffInflictor;
                    greviousWounds.ChangeDefensiveStats(true, 0, 0, 0, 0, stats.healingMultiplier * -0.75f);

                    break;

                default:
                    break;
            }
        }
    }

    // Used to remove an active buff. 
    // DO NOT CALL THIS TO REMVOE A BUFF CALL THE ENDBUFF() on the buff itself.
    public void RemoveBuff(Buff buffToRemove)
    {
        //Debug.Log("removing buff: " + buffToRemove);
        activeBuffs.Remove(buffToRemove);
        activeIcons.Remove(buffToRemove.connectedIcon);
        UpdateIconLocations();
        // Call the icon reshuffle here.
        Destroy(buffToRemove.connectedIcon);
        Destroy(buffToRemove);
    }

    // Checks through all our buffs, if we find one that buff will be removed.
    public void AttemptRemovalOfBuff(BuffType buffTypeToCheckFor)
    {
        //Debug.Log("attmpting removal of buff:");
        Buff buffToRemove = null;
        foreach (Buff buff in activeBuffs)
        {
            if (buff.myType == buffTypeToCheckFor)
            {
                buffToRemove = buff;
                break;
            }
        }

        if (buffToRemove != null)
            buffToRemove.EndBuff();
    }

    //USed to check to see ho many stacks of an active buff is on this target
    public float PollForBuffStacks(BuffType buffType)
    {
        float buffStacks = 0;

        foreach(Buff buff in activeBuffs)
        {
            if (buff.myType == buffType)
                buffStacks = buff.currentStacks;
        }

        return buffStacks;
    }

    //USed to grab the buff of a certain type so we can edit it, usually from another buff because of traits
    public Buff PollForBuff(BuffType buffType)
    {
        Buff buffToReturn = null;

        foreach (Buff buff in activeBuffs)
        {
            if (buff.myType == buffType)
                buffToReturn = buff;
        }

        return buffToReturn;
    }

    // Used to position all the icons based on their index.
    private void UpdateIconLocations()
    {
        if (gameObject.CompareTag("Player"))
        {
            for (int index = 0; index < activeIcons.Count; index++)
                activeIcons[index].transform.localPosition = new Vector3(-90 + (index * 25), 15, 0);
        }
        else if (gameObject.CompareTag("Enemy"))
        {
            for (int index = 0; index < activeIcons.Count; index++)
                activeIcons[index].transform.localPosition = new Vector3(-40 + (index * 25), 8, 0);
        }
    }

    // USed to proc my onkill efffects after slaying an enemy
    public void ProcOnKill(GameObject target, HitBox.DamageType damageType)
    {
        if (CompareTag("Player"))
        {
            Debug.Log("proccing thre players on kills now");
            // DO we have a trait that procs an onhit.
            for (int traitIndex = 0; traitIndex < playerTraitManager.OnKillEffects.Count; traitIndex++)
            {
                PlayerTraitManager.TraitSource trait = playerTraitManager.OnKillEffects[traitIndex];

                switch (trait.traitType)
                {
                    case ItemTrait.TraitType.HealingOnKill:
                        stats.HealHealth(trait.traitValue, HitBox.DamageType.Healing);
                        break;
                    case ItemTrait.TraitType.FireExplosionOnKill:
                        GameObject fireExplosionOnKill = Instantiate(GetComponent<SkillsManager>().skillProjectiles[48], target.transform.position + Vector3.up, Quaternion.identity);
                        fireExplosionOnKill.GetComponent<HitBox>().myStats = stats;
                        fireExplosionOnKill.GetComponent<HitBox>().damage = stats.baseDamage * 1f + (stats.baseDamage * trait.traitValue * target.GetComponent<BuffsManager>().PollForBuffStacks(BuffType.Aflame));
                        break;
                    case ItemTrait.TraitType.AflamePhysicalBladeExplosionOnKill:
                        if (target.GetComponent<BuffsManager>().PollForBuffStacks(BuffType.Aflame) > 25)
                        {
                            GameObject bladeExplosionOnKill = Instantiate(GetComponent<SkillsManager>().skillProjectiles[53], target.transform.position + Vector3.up, Quaternion.identity);
                            bladeExplosionOnKill.GetComponent<HitBox>().myStats = stats;
                            bladeExplosionOnKill.GetComponent<HitBox>().damage = stats.baseDamage * trait.traitValue;
                        }
                        break;
                    case ItemTrait.TraitType.AflamePoisonBurningEnemySpreadPoisonStacksOnDeath:

                        if (target.GetComponent<BuffsManager>().PollForBuffStacks(BuffType.Aflame) > 20 && target.GetComponent<BuffsManager>().PollForBuffStacks(BuffType.Poisoned) > 1)
                        {
                            GameObject poisonBurstOnDeath = Instantiate(GetComponent<SkillsManager>().skillProjectiles[54], target.transform.position + Vector3.up, Quaternion.identity);
                            poisonBurstOnDeath.GetComponent<HitBoxBuff>().buffOrigin = stats;
                            poisonBurstOnDeath.GetComponent<HitBoxBuff>().poisonValue = Mathf.RoundToInt(trait.traitValue * target.GetComponent<BuffsManager>().PollForBuffStacks(BuffType.Poisoned));
                        }
                        break;
                    case ItemTrait.TraitType.AflamePoisonPoisonCloudOnFireKill:

                        if (damageType == HitBox.DamageType.Fire && target.GetComponent<BuffsManager>().PollForBuffStacks(BuffType.Poisoned) > 0)
                        {
                            GameObject poisonBurstOnDeath = Instantiate(GetComponent<SkillsManager>().skillProjectiles[56], target.transform.position + Vector3.up, Quaternion.identity);
                            poisonBurstOnDeath.GetComponent<HitBox>().myStats = stats;
                            poisonBurstOnDeath.GetComponent<HitBox>().damage = stats.baseDamage * trait.traitValue;
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }

    // USed to proc my onhit effects onto an enemy
    public void ProcOnHits(GameObject target, HitBox hitbox)
    {
        // Check if we have any skills that apply buffs on hit.
        /*
        if (CompareTag("Player"))
        {
            foreach (Skill skill in skillManager.mySkills)
            {
                switch (skill.skillName)
                {
                    case SkillsManager.SkillNames.Rampage:
                        NewBuff(BuffType.Rampage, 0);
                        break;
                    default:
                        break;
                }
            }
        }
        */

        // Do we have a buff that procs as a onhit.
        for (int index = 0; index < activeBuffs.Count; index++)
        {
            Buff buff = activeBuffs[index];
            switch (buff.myType)
            {
                case BuffType.FlameStrike:
                    GameObject flamestrikeHit = Instantiate(GetComponent<SkillsManager>().skillProjectiles[4], target.transform.position + Vector3.up + new Vector3(Random.Range(-0.25f, 0.25f), Random.Range(-0.25f, 0.25f), Random.Range(-0.25f, 0.25f)), Quaternion.identity);
                    flamestrikeHit.GetComponent<HitBox>().myStats = stats;
                    flamestrikeHit.GetComponent<HitBox>().damage = stats.baseDamage * 0.7f;
                    break;

                case BuffType.FrostsKiss:
                    GameObject frostsKissProjectile = Instantiate(GetComponent<SkillsManager>().skillProjectiles[18], transform.position + Vector3.up * 2, Quaternion.Euler(Random.Range(-75, -90), Random.Range(0, 360), 0));
                    frostsKissProjectile.GetComponent<HitBox>().myStats = stats;
                    frostsKissProjectile.GetComponent<HitBox>().damage = stats.baseDamage * 1f;
                    frostsKissProjectile.GetComponent<ProjectileBehaviour>().target = target.transform;

                    if (skillManager.spellMirrors.Count > 0)
                    {
                        foreach (SpellMirrorManager spellMirror in skillManager.spellMirrors)
                        {
                            GameObject frostsKissProjectileMirror = Instantiate(GetComponent<SkillsManager>().skillProjectiles[18], spellMirror.transform.position, spellMirror.transform.rotation);
                            frostsKissProjectileMirror.GetComponent<HitBox>().myStats = stats;
                            frostsKissProjectileMirror.GetComponent<HitBox>().damage = stats.baseDamage * 0.5f;
                            frostsKissProjectileMirror.GetComponent<ProjectileBehaviour>().target = target.transform;
                        }
                    }
                    break;

                case BuffType.StoneStrike:
                    target.GetComponent<EnemyCrowdControlManager>().KnockbackLaunch((transform.forward + Vector3.up * 0.5f) * 15, stats);
                    target.GetComponent<PlayerStats>().TakeDamage(stats.baseDamage, false, HitBox.DamageType.Earth, stats.comboManager.currentcombo, stats);
                    StartCoroutine(RemoveBuffNextFrame(buff));
                    break;
                case BuffType.Multislash:
                    StartCoroutine(MultislashDamage(hitbox.damage / 2, hitbox.crit, target.GetComponentInChildren<PlayerStats>()));
                    Instantiate(GetComponent<SkillsManager>().skillProjectiles[42], target.transform.position + Vector3.up, Quaternion.identity); 
                    StartCoroutine(RemoveBuffNextFrame(buff));
                    break;
                default:
                    break;
            }

        }
        BuffsManager targetBM = target.GetComponent<BuffsManager>();

        // Does the enemy have a buff that can proc as an onhit.
        for (int index = 0; index < targetBM.activeBuffs.Count; index++)
        {
            Buff buff = targetBM.activeBuffs[index];
            switch (buff.myType)
            {
                case BuffType.Aflame:
                    break;
                case BuffType.Frostbite:
                    break;
                case BuffType.Overcharge:
                    if (hitbox.damageType != HitBox.DamageType.Lightning && hitbox.damageType != HitBox.DamageType.Nature)
                    {
                        // proc the lightning blast.
                        targetBM.psSystems[4].Play();
                        target.GetComponent<PlayerStats>().TakeDamage(0.4f * stats.baseDamage * buff.currentStacks, false, HitBox.DamageType.Lightning, stats.comboManager.currentcombo, stats);
                        // remove the buff
                        buff.EndBuff();
                        index--;
                    }
                    break;
                case BuffType.Overgrown:
                    if (hitbox.damageType != HitBox.DamageType.Lightning && hitbox.damageType != HitBox.DamageType.Nature)
                    {
                        // proc the nature blast
                        targetBM.psSystems[6].Play();
                        if (target.GetComponent<PlayerStats>().healthMax / 100 >= stats.baseDamage * 0.8f)
                            target.GetComponent<PlayerStats>().TakeDamage(0.8f * stats.baseDamage * buff.currentStacks, false, HitBox.DamageType.Nature, stats.comboManager.currentcombo, stats);
                        else
                            target.GetComponent<PlayerStats>().TakeDamage(target.GetComponent<PlayerStats>().healthMax / 100 * buff.currentStacks, false, HitBox.DamageType.Nature, stats.comboManager.currentcombo, stats);
                        // remove the buff
                        buff.EndBuff();
                        index--;
                    }
                    break;
                case BuffType.Sunder:
                    break;
                case BuffType.Windshear:
                    break;
                case BuffType.Knockback:
                    break;
                case BuffType.Asleep:
                    break;
                case BuffType.Stunned:
                    break;
                case BuffType.Bleeding:
                    break;
                case BuffType.Poisoned:
                    break;
                case BuffType.IceArmor:
                    if (target.CompareTag("Player"))
                    {
                        target.GetComponent<SkillsManager>().SpawnDisjointedSkillEffect(SkillsManager.SkillNames.IceArmor);
                    }
                    break;
                case BuffType.PressureDrop:
                    targetBM.StartCoroutine(RemoveBuffNextFrame(buff));
                    break;
                default:
                    break;
            }
        }

        if (CompareTag("Player"))
        {
            // DO we have a trait that procs an onhit.
            for (int traitIndex = 0; traitIndex < playerTraitManager.OnHitEffects.Count; traitIndex++)
            {
                PlayerTraitManager.TraitSource trait = playerTraitManager.OnHitEffects[traitIndex];

                float randomChance = Random.Range(0f, 100f);

                switch (trait.traitType)
                {
                    case ItemTrait.TraitType.HealingOnHit:
                        stats.HealHealth(trait.traitValue, HitBox.DamageType.Healing);
                        break;
                    case ItemTrait.TraitType.MoreAflameStacksOnHitThreshold:
                        if(target.GetComponent<BuffsManager>().PollForBuffStacks(BuffType.Aflame) >= 15 && !target.GetComponent<PlayerStats>().traitMoreAflameStacksOnHitThresholdFatigue)
                        {
                            target.GetComponent<PlayerStats>().traitMoreAflameStacksOnHitThresholdFatigue = true;
                            GameObject moreAflameStacksPulse = Instantiate(GetComponent<SkillsManager>().skillProjectiles[49], target.transform.position + Vector3.up, Quaternion.identity);
                            moreAflameStacksPulse.GetComponent<HitBoxBuff>().buffOrigin = stats;
                            moreAflameStacksPulse.GetComponent<HitBoxBuff>().aflameValue = (int) trait.traitValue;
                        }
                        break;
                    case ItemTrait.TraitType.BasicAttacksShredArmorOnAflame:
                        if (hitbox.CompareTag("BasicAttack"))
                            target.GetComponent<BuffsManager>().NewBuff(BuffType.BasicAttacksShredArmorOnAflame, stats.baseDamage, stats);
                        break;
                    case ItemTrait.TraitType.RingOfFireOnHit:
                        if(randomChance < 20f)
                        {
                            GameObject ringOfFireTraitOnHit = Instantiate(GetComponent<SkillsManager>().skillProjectiles[50], target.transform.position, Quaternion.identity);
                            ringOfFireTraitOnHit.GetComponent<HitBox>().myStats = stats;
                            ringOfFireTraitOnHit.GetComponent<HitBox>().damage = (stats.baseDamage + (trait.traitValue * stats.baseDamage)) / 2;
                        }
                        break;
                    case ItemTrait.TraitType.AflameToSunderStackOnEarthSpell:
                        bool earthSpell = false;

                        if (hitbox.GetComponent<HitBox>())
                            if (hitbox.GetComponent<HitBox>().damageType == HitBox.DamageType.Earth)
                                earthSpell = true;
                        if (hitbox.GetComponent<HitBoxBuff>())
                            if (hitbox.GetComponent<HitBoxBuff>().sunderValue > 0)
                                earthSpell = true;

                        if (earthSpell)
                            target.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffType.Sunder, Mathf.RoundToInt(target.GetComponent<BuffsManager>().PollForBuffStacks(BuffType.Aflame) * trait.traitValue), stats.baseDamage, stats);
                        break;
                    case ItemTrait.TraitType.AflameSunderCritsSummonFireballs:
                        BuffsManager targetBuffManager = target.GetComponent<BuffsManager>();
                        if (hitbox.GetComponent<HitBox>().crit && (targetBuffManager.PollForBuffStacks(BuffType.Aflame) + targetBuffManager.PollForBuffStacks(BuffType.Sunder)) >= 50 && targetBuffManager.PollForBuffStacks(BuffType.Aflame) > 0 && targetBuffManager.PollForBuffStacks(BuffType.Sunder) > 0)
                        {
                            GameObject aflameSunderCritFireball = Instantiate(GetComponent<SkillsManager>().skillProjectiles[51], target.transform.position + Vector3.up * 2, Quaternion.Euler(new Vector3(Random.Range(-60,-90),Random.Range(0,360),0)));
                            aflameSunderCritFireball.GetComponent<HitBox>().myStats = stats;
                            aflameSunderCritFireball.GetComponent<HitBox>().damage = stats.baseDamage * trait.traitValue;
                        }
                        break;
                    case ItemTrait.TraitType.AflameWindshearSummonFirePillarsOnHit:
                        if (hitbox.damageType==HitBox.DamageType.Wind && randomChance < 20f && target.GetComponent<BuffsManager>().PollForBuffStacks(BuffType.Aflame) > 30)
                        {
                            GameObject flameGeyserOnHit = Instantiate(GetComponent<SkillsManager>().skillProjectiles[52], target.transform.position, Quaternion.identity);
                            flameGeyserOnHit.GetComponent<HitBox>().myStats = stats;
                            flameGeyserOnHit.GetComponent<HitBox>().damage = stats.baseDamage * trait.traitValue;
                        }
                        break;
                    case ItemTrait.TraitType.AflameWindshearWindSpellsAddFireStacks:
                        if (hitbox.damageType==HitBox.DamageType.Wind && target.GetComponent<BuffsManager>().PollForBuffStacks(BuffType.Aflame) > 0 && randomChance < 10f)
                        {
                            GameObject moreAflameStacksPulse = Instantiate(GetComponent<SkillsManager>().skillProjectiles[49], target.transform.position + Vector3.up, Quaternion.identity);
                            moreAflameStacksPulse.GetComponent<HitBoxBuff>().buffOrigin = stats;
                            moreAflameStacksPulse.GetComponent<HitBoxBuff>().aflameValue = Mathf.RoundToInt(target.GetComponent<BuffsManager>().PollForBuffStacks(BuffType.Aflame) * trait.traitValue);
                        }
                        break;
                    case ItemTrait.TraitType.AflamePhysicalAddFireStacksOnHit:
                        if (hitbox.damageType == HitBox.DamageType.Physical)
                            target.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffType.Aflame, Mathf.RoundToInt(trait.traitValue), stats.baseDamage, stats);
                        break;
                    case ItemTrait.TraitType.AflamePhysicalBigHitsAddAflame:
                        if (hitbox.damageType == HitBox.DamageType.Physical && hitbox.damage / stats.baseDamage > 4 && target.GetComponent<BuffsManager>().PollForBuffStacks(BuffType.Aflame) > 0)
                        {
                            target.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffType.Aflame, Mathf.RoundToInt(target.GetComponent<BuffsManager>().PollForBuffStacks(BuffType.Aflame) * trait.traitValue), stats.baseDamage, stats);
                        }
                        break;
                    case ItemTrait.TraitType.AflamePoisonFireSpellsSummonsPoisonBurst:
                        if (hitbox.damageType == HitBox.DamageType.Fire && target.GetComponent<BuffsManager>().PollForBuffStacks(BuffType.Poisoned) > 30 && stats.traitPoisonFireSpellOnHitReady)
                        {
                            stats.traitPoisonFireSpellOnHitReady = false;
                            GameObject poisonBurst = Instantiate(GetComponent<SkillsManager>().skillProjectiles[55], target.transform.position + Vector3.up, Quaternion.identity);
                            poisonBurst.GetComponent<HitBoxBuff>().buffOrigin = stats;
                            poisonBurst.GetComponent<HitBoxBuff>().poisonValue = Mathf.RoundToInt(trait.traitValue);
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }

    public void ProcOnAttack()
    {

        foreach (Buff buff in activeBuffs)
        {
            switch (buff.myType)
            {
                case BuffType.Bleeding:
                    psSystems[10].Play();
                    stats.TakeDamage(buff.DPS * buff.currentStacks, false, buff.damageType, 0, stats);
                    break;
                default:
                    break;
            }
        }
    }


    public void ProcOnRoll()
    {
        foreach (Buff buff in activeBuffs)
        {
            switch (buff.myType)
            {
                case BuffType.Bleeding:
                    psSystems[10].Play();
                    stats.TakeDamage(buff.DPS * buff.currentStacks, false, buff.damageType, 0, stats);
                    break;
                default:
                    break;
            }
        }
    }

    IEnumerator RemoveBuffNextFrame(Buff buffToRemove)
    {
        yield return new WaitForEndOfFrame();
        if (buffToRemove != null)
            buffToRemove.EndBuff();
    }


    // USed to update the location and mesh shape of all our weapon particles.
    public void UpdateWeaponEffectsLeft(Vector3 position, Vector3 scale, Quaternion rotation, Mesh mesh)
    {
        for (int index = 0; index < weaponEffectsLeft.Count; index++)
        {
            ParticleSystem ps = weaponEffectsLeft[index];

            if (mesh != null)
            {
                ps.transform.position = position;
                ps.transform.rotation = rotation;

                var shapeMod = ps.shape;
                shapeMod.shapeType = ParticleSystemShapeType.Mesh;
                shapeMod.scale = new Vector3(scale.x / 100, scale.y / 100, scale.z / 100);
                shapeMod.mesh = mesh;

                var emmissionMod = ps.emission;
                emmissionMod.rateOverTime = weaponEffectRateOverTime[index];
            }
            else
            {
                ps.transform.localPosition = position;
                ps.transform.rotation = rotation;

                var shapeMod = ps.shape;
                shapeMod.shapeType = ParticleSystemShapeType.Sphere;
                shapeMod.scale = new Vector3(1, 1, 1);
                shapeMod.mesh = mesh;

                var emmissionMod = ps.emission;
                emmissionMod.rateOverTime = 0;
            }
        }
    }
    // USed to update the location and mesh shape of all our weapon particles.
    public void UpdateWeaponEffectsRight(Vector3 position, Vector3 scale, Quaternion rotation, Mesh mesh)
    {
        for (int index = 0; index < weaponEffectsRight.Count; index++)
        {
            ParticleSystem ps = weaponEffectsRight[index];

            if (mesh != null)
            {
                ps.transform.position = position;
                ps.transform.rotation = rotation;

                var shapeMod = ps.shape;
                shapeMod.shapeType = ParticleSystemShapeType.Mesh;
                shapeMod.scale = new Vector3(scale.x / 100, scale.y / 100, scale.z / 100);
                shapeMod.mesh = mesh;

                var emmissionMod = ps.emission;
                emmissionMod.rateOverTime = weaponEffectRateOverTime[index];
            }
            else
            {
                ps.transform.localPosition = position;
                ps.transform.rotation = rotation;

                var shapeMod = ps.shape;
                shapeMod.shapeType = ParticleSystemShapeType.Sphere;
                shapeMod.scale = new Vector3(1, 1, 1);
                shapeMod.mesh = mesh;

                var emmissionMod = ps.emission;
                emmissionMod.rateOverTime = 0;
            }
        }
    }

    private void OnDestroy()
    {
        for (int index = activeBuffs.Count - 1; index >= 0; index--)
        {
            //Debug.Log("Destroying the buff: " + activeBuffs[index].myType);
            Destroy(activeBuffs[index]);
        }
    }

    IEnumerator MultislashDamage(float damage, bool crit, PlayerStats target)
    {
        for (int index = 0; index < 3; index++)
        {
            stats.comboManager.AddComboCounter(1);
            target.TakeDamage(damage, crit, HitBox.DamageType.Wind, stats.comboManager.currentcombo, stats);
            yield return new WaitForSeconds(0.33f);
        }
    }
}