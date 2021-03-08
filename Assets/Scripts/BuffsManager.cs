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

    public enum BuffType { Aflame, Frostbite, Overcharge, Overgrown, Sunder, Windshear, Knockback, Asleep, Stunned, Bleeding, Poisoned, Frozen, ArmorBroken, EmboldeningEmbers, FlameStrike, FlameWalker, AspectOfRage, BlessingOfFlames, Rampage,
                            GiantStrength, ToxicRipple, KillerInstinct, PoisonedMud, StrangleThorn, SoothingStone, Deadeye, WrathOfTheRagingWind, FrozenBarrier, SoothingStream,
                            NaturePulse, Revitalize};
    
    [SerializeField] private ParticleSystem[] psSystems;
    private PlayerStats stats;
    private EffectsManager effects;
    private SkillsManager skillManager;
    private DamageNumberManager uiPopupManager;

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

        foreach (ParticleSystem ps in psSystems)
            if(ps != null)
                ps.Stop();

        foreach (ParticleSystem ps in weaponEffectsLeft)
            ps.Stop();
        foreach (ParticleSystem ps in weaponEffectsRight)
            ps.Stop();
    }

    // USed tyo see if our player resists the buff being added
    public void CheckResistanceToBuff(BuffType buff, int stackCount, float baseDamage)
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
                    NewBuff(buff, baseDamage);
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
    public void NewBuff(BuffType buff, float baseDamage)
    {
        // Debug.Log("Addding buffs");
        bool buffDealtWith = false;

        // Check to see if any of our buffs match this buff, and if the source matches then we reset the duration.
        for(int index = 0; index < activeBuffs.Count; index++)
        {
            Buff activeBuff = activeBuffs[index];
            if (activeBuff.myType == buff)
            {
                buffDealtWith = true;
                if (activeBuff.stackable)
                {
                    if (activeBuff.myType == BuffType.FlameStrike)
                        activeBuff.AddStack(5);
                    else
                        activeBuff.AddStack(1);
                }
                else
                    activeBuff.AddTime(0, true);
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
        if(!buffDealtWith)
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
                    aflame.infiniteDuration = false;
                    aflame.duration = 10;

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
                    frostbite.infiniteDuration = false;
                    frostbite.duration = 10;

                    frostbite.DPS = baseDamage * 0.05f;
                    frostbite.damageType = HitBox.DamageType.Ice;
                    frostbite.ChangeOffensiveStats(true, -0.01f, -0.01f);

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
                    windshear.infiniteDuration = false;
                    windshear.duration = 10;

                    windshear.ChangeDefensiveStats(true, 0f, 0f, -0.01f, 0f);

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
                    sundered.infiniteDuration = false;
                    sundered.duration = 10;

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
                    poisoned.infiniteDuration = false;
                    poisoned.duration = 20;

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
                    stunned.infiniteDuration = false;
                    stunned.duration = 2;
                    stunned.DPS = 0;

                    if (CompareTag("Player"))
                        GetComponent<PlayerMovementController>().StunLaunch();
                    else
                        GetComponent<EnemyCrowdControlManager>().StunLaunch();

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
                    armorBroken.infiniteDuration = false;
                    armorBroken.duration = 10;

                    armorBroken.ChangeDefensiveStats(true, 0f, 0f, -0.4f, 0f);

                    psSystems[17].Play();

                    break;
                case BuffType.EmboldeningEmbers:

                    //Debug.Log("Adding emboldening embers buff");
                    Buff embers = transform.Find("BuffContainer").gameObject.AddComponent<Buff>();
                    embers.connectedIcon = buffIcon;
                    buffIcon.GetComponent<Image>().sprite = BuffIconBank.instance.buffIcons[12];
                    buffIcon.GetComponent<Image>().color = BuffIconBank.instance.buffColors[0];

                    activeBuffs.Add(embers);

                    embers.myType = buff;
                    embers.connectedPlayer = stats;
                    embers.infiniteDuration = false;
                    embers.ChangeOffensiveStats(true, 0.5f, 0.25f);
                    embers.ChangeDefensiveStats(true, stats.healthMax * 0.2f, 0, 0, 0);
                    embers.duration = 15;

                    embers.effectParticleSystem.Add(psSystems[19]);
                    psSystems[18].Play();
                    psSystems[19].Play();

                    break;

                case BuffType.FlameStrike:

                    //Debug.Log("Adding flame strike buff");
                    Buff flameStrike = transform.Find("BuffContainer").gameObject.AddComponent<Buff>();
                    flameStrike.connectedIcon = buffIcon;
                    flameStrike.iconStacks = buffIcon.GetComponentInChildren<Text>();
                    buffIcon.GetComponent<Image>().sprite = BuffIconBank.instance.buffIcons[13];
                    buffIcon.GetComponent<Image>().color = BuffIconBank.instance.buffColors[0];

                    activeBuffs.Add(flameStrike);

                    flameStrike.myType = buff;
                    flameStrike.maxStacks = 5;
                    flameStrike.currentStacks = 4;
                    flameStrike.stackable = true;
                    flameStrike.stackSingleFalloff = false;
                    flameStrike.connectedPlayer = stats;
                    flameStrike.infiniteDuration = false;
                    flameStrike.duration = 15;

                    flameStrike.AddStack(1);

                    flameStrike.effectParticleSystem.Add(weaponEffectsLeft[0]);
                    flameStrike.effectParticleSystem.Add(weaponEffectsRight[0]);

                    weaponEffectsLeft[0].Play();
                    weaponEffectsRight[0].Play();

                    break;
                case BuffType.FlameWalker:

                    //Debug.Log("Adding flame strike buff");
                    Buff flameWalker = transform.Find("BuffContainer").gameObject.AddComponent<Buff>();
                    flameWalker.connectedIcon = buffIcon;
                    buffIcon.GetComponent<Image>().sprite = BuffIconBank.instance.buffIcons[14];
                    buffIcon.GetComponent<Image>().color = BuffIconBank.instance.buffColors[0];

                    activeBuffs.Add(flameWalker);

                    flameWalker.myType = buff;
                    flameWalker.connectedPlayer = stats;
                    flameWalker.infiniteDuration = false;
                    flameWalker.duration = 15;
                    flameWalker.ChangeOffensiveStats(true, 0, 0.5f);

                    stats.flameWalkerEnabled = true;

                    flameWalker.effectParticleSystem.Add(psSystems[20]);
                    flameWalker.effectParticleSystem.Add(psSystems[21]);
                    flameWalker.effectParticleSystem.Add(psSystems[22]);

                    psSystems[20].Play();
                    psSystems[21].Play();
                    psSystems[22].Play();

                    break;
                //----------------------------------------------------------------------------------------------------------------------------------

                case BuffType.AspectOfRage:

                    //Debug.Log("Adding aspect of Rage buff");
                    Buff aspectOfRage = transform.Find("BuffContainer").gameObject.AddComponent<Buff>();
                    aspectOfRage.connectedIcon = buffIcon;
                    buffIcon.GetComponent<Image>().sprite = BuffIconBank.instance.buffIcons[0];

                    activeBuffs.Add(aspectOfRage);

                    aspectOfRage.myType = buff;
                    aspectOfRage.connectedPlayer = stats;
                    aspectOfRage.infiniteDuration = false;
                    aspectOfRage.duration = 15;
                    aspectOfRage.ChangeOffensiveStats(true, 0.4f, 0);
                    aspectOfRage.ChangeDefensiveStats(true, 0, 0, 0, -50);
                    aspectOfRage.effectParticleSystem.Add(psSystems[27]);
                    aspectOfRage.effectParticleSystem.Add(psSystems[28]);
                    aspectOfRage.effectParticleSystem.Add(psSystems[29]);
                    psSystems[27].Play();
                    psSystems[28].Play();
                    psSystems[29].Play();

                    break;
                case BuffType.BlessingOfFlames:
                    //Debug.Log("Adding blessing of flames buff");
                    Buff blessingOfFlames = transform.Find("BuffContainer").gameObject.AddComponent<Buff>();
                    blessingOfFlames.connectedIcon = buffIcon;
                    buffIcon.GetComponent<Image>().sprite = BuffIconBank.instance.buffIcons[0];

                    activeBuffs.Add(blessingOfFlames);

                    blessingOfFlames.myType = buff;
                    blessingOfFlames.connectedPlayer = stats;
                    blessingOfFlames.infiniteDuration = false;
                    blessingOfFlames.duration = 15;
                    //blessingOfFlames.ChangeResistanceStats(true, 0.5f, 0, 0, 0.1f, 0.1f, 0.1f, 0.3f, 0.1f, 0);
                    blessingOfFlames.effectParticleSystem.Add(psSystems[30]);
                    psSystems[30].Play();

                    break;
                case BuffType.Rampage:
                    //Debug.Log("adding rampage buff");
                    Buff rampage = transform.Find("BuffContainer").gameObject.AddComponent<Buff>();
                    rampage.connectedIcon = buffIcon;
                    rampage.iconStacks = buffIcon.GetComponentInChildren<Text>();
                    buffIcon.GetComponent<Image>().sprite = BuffIconBank.instance.buffIcons[0];

                    activeBuffs.Add(rampage);

                    rampage.myType = buff;
                    rampage.infiniteDuration = false;
                    rampage.duration = 3;
                    rampage.connectedPlayer = stats;
                    rampage.stackable = true;
                    rampage.stackSingleFalloff = true;
                    rampage.stackfalloffTime = 0.5f;
                    rampage.maxStacks = 25;
                    rampage.currentStacks = 1;
                    rampage.stacktargetTimer = rampage.duration;
                    rampage.ChangeOffensiveStats(true, 0.04f, 0);
                    //rampage.effectParticleSystem.Add(psSystems[30]);
                    //psSystems[30].Play();

                    break;
                case BuffType.GiantStrength:
                    //Debug.Log("adding giant Strength buff");
                    Buff giantStrength = transform.Find("BuffContainer").gameObject.AddComponent<Buff>();
                    giantStrength.connectedIcon = buffIcon;
                    giantStrength.iconStacks = buffIcon.GetComponentInChildren<Text>();
                    buffIcon.GetComponent<Image>().sprite = BuffIconBank.instance.buffIcons[0];

                    activeBuffs.Add(giantStrength);

                    giantStrength.myType = buff;
                    giantStrength.infiniteDuration = false;
                    giantStrength.duration = 12f;
                    giantStrength.connectedPlayer = stats;
                    giantStrength.ChangeSize(true, 0.25f);
                    giantStrength.ChangeDefensiveStats(true, 0, 0, 0, 0.25f);
                    giantStrength.effectParticleSystem.Add(psSystems[31]);
                    psSystems[31].Play();

                    break;
                case BuffType.ToxicRipple:
                    //Debug.Log("adding Toxic Ripple buff");

                    Buff toxicRipple = transform.Find("BuffContainer").gameObject.AddComponent<Buff>();
                    toxicRipple.connectedIcon = buffIcon;
                    toxicRipple.iconStacks = buffIcon.GetComponentInChildren<Text>();
                    buffIcon.GetComponent<Image>().sprite = BuffIconBank.instance.buffIcons[0];

                    activeBuffs.Add(toxicRipple);

                    toxicRipple.myType = buff;
                    toxicRipple.infiniteDuration = false;
                    toxicRipple.duration = 10f;
                    toxicRipple.connectedPlayer = stats;
                    //toxicRipple.ChangeAfflictionStats(true, 0, 0, 0, 0, 0.7f, 0, 0, 0.7f, 0);

                    break;
                case BuffType.KillerInstinct:
                    //Debug.Log("adding Killer instinct buff");

                    Buff killerInstinct = transform.Find("BuffContainer").gameObject.AddComponent<Buff>();
                    killerInstinct.connectedIcon = buffIcon;
                    killerInstinct.iconStacks = buffIcon.GetComponentInChildren<Text>();
                    buffIcon.GetComponent<Image>().sprite = BuffIconBank.instance.buffIcons[0];

                    activeBuffs.Add(killerInstinct);

                    killerInstinct.myType = buff;
                    killerInstinct.infiniteDuration = false;
                    killerInstinct.duration = 15f;
                    killerInstinct.connectedPlayer = stats;
                    killerInstinct.ChangePlayerStatusLocks(true, 0, 1, 0);
                    killerInstinct.effectParticleSystem.Add(psSystems[32]);
                    killerInstinct.endOfBuffParticleSystem.Add(psSystems[33]);
                    psSystems[32].Play();

                    break;
                case BuffType.NaturePulse:
                    //Debug.Log("adding nature pulse debuff");

                    Buff naturePulse = transform.Find("BuffContainer").gameObject.AddComponent<Buff>();
                    naturePulse.connectedIcon = buffIcon;
                    naturePulse.iconStacks = buffIcon.GetComponentInChildren<Text>();
                    buffIcon.GetComponent<Image>().sprite = BuffIconBank.instance.buffIcons[0];

                    activeBuffs.Add(naturePulse);

                    naturePulse.myType = buff;
                    naturePulse.infiniteDuration = false;
                    naturePulse.maxStacks = 5;
                    naturePulse.currentStacks = 1;
                    naturePulse.stackable = true;
                    naturePulse.duration = 6f;
                    naturePulse.connectedPlayer = stats;
                    //naturePulse.effectParticleSystem.Add(psSystems[32]);
                    //naturePulse.endOfBuffParticleSystem.Add(psSystems[33]);
                    //psSystems[32].Play();
                    break;
                case BuffType.Revitalize:
                    //Debug.Log("adding revitalize buff");

                    Buff revitalize = transform.Find("BuffContainer").gameObject.AddComponent<Buff>();
                    revitalize.connectedIcon = buffIcon;
                    revitalize.iconStacks = buffIcon.GetComponentInChildren<Text>();
                    buffIcon.GetComponent<Image>().sprite = BuffIconBank.instance.buffIcons[0];

                    activeBuffs.Add(revitalize);

                    revitalize.myType = buff;
                    revitalize.infiniteDuration = true;
                    
                    if (stats == null)
                        stats = GetComponent<PlayerStats>();

                    stats.revitalizeBuff = true;
                    revitalize.stackable = true;
                    revitalize.maxStacks = 10;
                    revitalize.connectedPlayer = stats;
                    revitalize.AddStack(1);
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
    public void AttemptRemovalOfBuff (BuffType buffTypeToCheckFor)
    {
        //Debug.Log("attmpting removal of buff:");
        Buff buffToRemove = null;
        foreach(Buff buff in activeBuffs)
        {
            if(buff.myType == buffTypeToCheckFor)
            {
                buffToRemove = buff;
                break;
            }
        }

        if (buffToRemove != null)
            buffToRemove.EndBuff();
    }
    
    // Used to position all the icons based on their index.
    private void UpdateIconLocations()
    {
        if (gameObject.CompareTag("Player"))
        {
            for (int index = 0; index < activeIcons.Count; index++)
                activeIcons[index].transform.localPosition = new Vector3(-90 + (index * 25), 15, 0);
        }
        else if(gameObject.CompareTag("Enemy"))
        {
            for (int index = 0; index < activeIcons.Count; index++)
                activeIcons[index].transform.localPosition = new Vector3(-40 + (index * 25), 8, 0);
        }
    }

    // USed to proc my onhit effects onto an enemy
    public void ProcOnHits(GameObject target , HitBox hitbox)
    {
        // Debug.Log("proccing on hits");
        // Check if we have any skills that apply buffs on hit.
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

        foreach(Buff buff in activeBuffs)
        {
            switch (buff.myType)
            {
                case BuffType.FlameStrike:
                    GameObject flamestrikeHit = Instantiate(GetComponent<SkillsManager>().skillProjectiles[4], target.transform.position + Vector3.up + new Vector3(Random.Range(-0.25f, 0.25f), Random.Range(-0.25f, 0.25f), Random.Range(-0.25f, 0.25f)), Quaternion.identity);
                    flamestrikeHit.GetComponent<HitBox>().myStats = stats;
                    flamestrikeHit.GetComponent<HitBox>().damage = stats.baseDamage * 0.7f;
                    buff.RemoveStacks(1, false);
                    break;
                case BuffType.KillerInstinct:
                    //Debug.Log("killer instinct hit has been procced");
                    hitbox.bypassCrit = true;
                    /*
                    switch (hitbox.damageType)
                    {
                        case HitBox.DamageType.Physical:
                            hitbox.damageType = HitBox.DamageType.PhysicalCrit;
                            break;
                        case HitBox.DamageType.Magical:
                            hitbox.damageType = HitBox.DamageType.MagicalCrit;
                            break;
                        case HitBox.DamageType.Healing:
                            hitbox.damageType = HitBox.DamageType.HealingCrit;
                            break;
                    }
                    */
                    buff.currentTimer = buff.duration;
                    break;
                default:
                    break;
            }
        }
        BuffsManager targetBM = target.GetComponent<BuffsManager>();

        // Does the enemy have a buff that can proc as an onhit.
        for(int index = 0; index < targetBM.activeBuffs.Count; index++)
        {
            Buff buff = targetBM.activeBuffs[index];
            switch (buff.myType)
            {
                case BuffType.Aflame:
                    break;
                case BuffType.Frostbite:
                    break;
                case BuffType.Overcharge:
                    if(hitbox.damageType != HitBox.DamageType.Lightning && hitbox.damageType != HitBox.DamageType.Nature)
                    {
                        // proc the lightning blast.
                        targetBM.psSystems[4].Play();
                        target.GetComponent<PlayerStats>().TakeDamage(0.4f * stats.baseDamage * buff.currentStacks, false, HitBox.DamageType.Lightning);
                        // remove the buff
                        buff.EndBuff();
                        index --;
                    }
                    break;
                case BuffType.Overgrown:
                    if (hitbox.damageType != HitBox.DamageType.Lightning && hitbox.damageType != HitBox.DamageType.Nature)
                    {
                        // proc the nature blast
                        targetBM.psSystems[6].Play();
                        if(target.GetComponent<PlayerStats>().healthMax / 100 >= stats.baseDamage * 0.8f)
                            target.GetComponent<PlayerStats>().TakeDamage(0.8f * stats.baseDamage * buff.currentStacks, false, HitBox.DamageType.Nature);
                        else
                            target.GetComponent<PlayerStats>().TakeDamage(target.GetComponent<PlayerStats>().healthMax / 100 * buff.currentStacks, false, HitBox.DamageType.Nature);
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
                default:
                    break;
            }
        }
    }
    
    // USed to proc my onhit effects onto an enemy
    public void ProcOnHits(GameObject target, HitBoxTerrain hitbox)
    {
        //Debug.Log("proccing on hits");
        // Check if we have any skills that apply buffs on hit.
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

        foreach (Buff buff in activeBuffs)
        {
            switch (buff.myType)
            {
                case BuffType.FlameStrike:
                    target.GetComponent<PlayerStats>().TakeDamage(buff.onHitDamageAmount, false, HitBox.DamageType.Physical);
                    psSystems[23].Play();
                    psSystems[24].Play();
                    psSystems[25].Play();
                    psSystems[26].Play();
                    break;
                case BuffType.KillerInstinct:
                    Debug.Log("killer instinct hit has been procced");
                    hitbox.bypassCrit = true;
                    /*
                    switch (hitbox.damageType)
                    {
                        case HitBox.DamageType.Physical:
                            hitbox.damageType = HitBox.DamageType.PhysicalCrit;
                            break;
                        case HitBox.DamageType.Magical:
                            hitbox.damageType = HitBox.DamageType.MagicalCrit;
                            break;
                        case HitBox.DamageType.Healing:
                            hitbox.damageType = HitBox.DamageType.HealingCrit;
                            break;
                    }
                    */
                    buff.currentTimer = buff.duration;
                    break;
                default:
                    break;
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
                    stats.TakeDamage(buff.DPS * buff.currentStacks, false, buff.damageType);
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
                    stats.TakeDamage(buff.DPS * buff.currentStacks, false, buff.damageType);
                    break;
                default:
                    break;
            }
        }
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
        for(int index = 0; index < weaponEffectsRight.Count; index++)
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
}
