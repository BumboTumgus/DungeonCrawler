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

    public enum BuffType { Aflame, Asleep, Stunned, Cursed, Bleeding, Poisoned, Corrosion, Frostbite, EmboldeningEmbers, FlameStrike, AspectOfRage, BlessingOfFlames, Rampage,
                            GiantStrength, ToxicRipple, KillerInstinct, PoisonedMud, StrangleThorn, SoothingStone, Deadeye, WrathOfTheRagingWind, FrozenBarrier, SoothingStream,
                            NaturePulse, Revitalize};
    
    [SerializeField] private Sprite[] buffIcons;
    public Color[] damageColors;
    [SerializeField] private ParticleSystem[] psSystems;
    private PlayerStats stats;
    private AfflictionManager afflictionManager;
    private EffectsManager effects;
    private SkillsManager skillManager;
    private DamageNumberManager uiPopupManager;


    // Grabs the players stats for use when calculating buff strength.
    private void Start()
    {
        stats = GetComponent<PlayerStats>();
        uiPopupManager = GetComponent<DamageNumberManager>();
        afflictionManager = GetComponent<AfflictionManager>();
        effects = GetComponent<EffectsManager>();
        skillManager = GetComponent<SkillsManager>();
        foreach (ParticleSystem ps in psSystems)
            if(ps != null)
                ps.Stop();
    }

    // Used to add a new buff to oiur player, if they already have it from this source, we refresh it instead.
    public void NewBuff(BuffType buff)
    {
        // Debug.Log("Addding buffs");
        bool buffDealtWith = false;

        // Check to see if any of our buffs match this buff, and if the source matches then we reset the duration.
        foreach(Buff activeBuff in activeBuffs)
        {
            if (activeBuff.myType == buff)
            {
                buffDealtWith = true;
                if(activeBuff.stackable)
                    activeBuff.AddStack(1);
                else
                    activeBuff.AddTime(0, true);
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
                    
                    Debug.Log("Addding aflame buff");
                    uiPopupManager.SpawnFlavorText("Aflame!", damageColors[0]);
                    Buff aflame = transform.Find("BuffContainer").gameObject.AddComponent<Buff>();
                    aflame.connectedIcon = buffIcon;
                    buffIcon.GetComponent<Image>().sprite = buffIcons[0];
                    Debug.Log(aflame);

                    activeBuffs.Add(aflame);

                    aflame.myType = buff;
                    aflame.connectedPlayer = stats;
                    aflame.infiniteDuration = false;
                    aflame.duration = 20;
                    aflame.DPS = stats.healthMax * 0.1f;
                    aflame.damageColor = damageColors[0];
                    aflame.effectParticleSystem.Add(psSystems[0]);
                    aflame.effectParticleSystem.Add(psSystems[15]);
                    aflame.effectParticleSystem.Add(psSystems[16]);
                    psSystems[0].Play();
                    psSystems[15].Play();
                    psSystems[16].Play();

                    Debug.Log("aflame buff has been added");
                    break;
                case BuffType.Asleep:

                    Debug.Log("Adding asleep debuff");
                    uiPopupManager.SpawnFlavorText("Asleep!", damageColors[5]);
                    Buff asleep = transform.Find("BuffContainer").gameObject.AddComponent<Buff>();
                    asleep.connectedIcon = buffIcon;
                    buffIcon.GetComponent<Image>().sprite = buffIcons[1];

                    activeBuffs.Add(asleep);

                    asleep.myType = buff;
                    asleep.connectedPlayer = stats;
                    asleep.infiniteDuration = false;
                    asleep.duration = 5;
                    asleep.DPS = 0;
                    GetComponent<PlayerMovementController>().AsleepLaunch();
                    asleep.effectParticleSystem.Add(psSystems[1]);
                    psSystems[1].Play();

                    break;
                case BuffType.Stunned:

                    Debug.Log("Adding stun debuff");
                    uiPopupManager.SpawnFlavorText("Cursed!", damageColors[6]);
                    Buff stunned = transform.Find("BuffContainer").gameObject.AddComponent<Buff>();
                    stunned.connectedIcon = buffIcon;
                    buffIcon.GetComponent<Image>().sprite = buffIcons[2];

                    activeBuffs.Add(stunned);

                    stunned.myType = buff;
                    stunned.connectedPlayer = stats;
                    stunned.infiniteDuration = false;
                    stunned.duration = 2;
                    stunned.DPS = 0;
                    GetComponent<PlayerMovementController>().StunLaunch();
                    stunned.effectParticleSystem.Add(psSystems[2]);
                    psSystems[2].Play();

                    break;
                case BuffType.Cursed:

                    Debug.Log("Adding cursed debuff");
                    uiPopupManager.SpawnFlavorText("Cursed!", damageColors[1]);
                    Buff cursed = transform.Find("BuffContainer").gameObject.AddComponent<Buff>();
                    cursed.connectedIcon = buffIcon;
                    buffIcon.GetComponent<Image>().sprite = buffIcons[3];

                    activeBuffs.Add(cursed);

                    cursed.myType = buff;
                    cursed.connectedPlayer = stats;
                    cursed.ChangeCoreStats(true, Mathf.RoundToInt(stats.Vit * -0.5f), Mathf.RoundToInt(stats.Str * -0.5f), Mathf.RoundToInt(stats.Dex * -0.5f),
                        Mathf.RoundToInt(stats.Spd * -0.5f), Mathf.RoundToInt(stats.Int * -0.5f), Mathf.RoundToInt(stats.Wis * -0.5f), Mathf.RoundToInt(stats.Cha * -0.5f));
                    stats.TakeDamage(stats.healthMax / 2, false, HitBox.DamageType.True);
                    cursed.infiniteDuration = false;
                    cursed.duration = 20;
                    cursed.DPS = 0;
                    cursed.effectParticleSystem.Add(psSystems[4]);
                    cursed.effectParticleSystem.Add(psSystems[7]);
                    psSystems[4].Play();
                    psSystems[3].Play();
                    psSystems[7].Play();

                    break;
                case BuffType.Bleeding:

                    Debug.Log("Adding bleed debuff");
                    uiPopupManager.SpawnFlavorText("Bleeding!", damageColors[2]);
                    Buff bleed = transform.Find("BuffContainer").gameObject.AddComponent<Buff>();
                    bleed.connectedIcon = buffIcon;
                    buffIcon.GetComponent<Image>().sprite = buffIcons[4];

                    activeBuffs.Add(bleed);

                    bleed.myType = buff;
                    bleed.connectedPlayer = stats;
                    bleed.infiniteDuration = false;
                    bleed.duration = 10;
                    bleed.DPS = stats.healthMax * 0.025f;
                    bleed.damageColor = damageColors[2];
                    stats.bleeding = true;
                    stats.TakeDamage(stats.healthMax * 0.25f, false, HitBox.DamageType.Physical);
                    bleed.effectParticleSystem.Add(psSystems[6]);
                    psSystems[6].Play();
                    psSystems[5].Play();

                    break;
                case BuffType.Poisoned:

                    Debug.Log("Adding poison debuff");
                    uiPopupManager.SpawnFlavorText("Poisoned!", damageColors[3]);
                    Buff poisoned = transform.Find("BuffContainer").gameObject.AddComponent<Buff>();
                    poisoned.connectedIcon = buffIcon;
                    buffIcon.GetComponent<Image>().sprite = buffIcons[5];

                    activeBuffs.Add(poisoned);

                    poisoned.myType = buff;
                    poisoned.connectedPlayer = stats;
                    poisoned.infiniteDuration = false;
                    poisoned.duration = 20;
                    poisoned.DPS = stats.healthMax * 0.05f;
                    poisoned.damageType = HitBox.DamageType.Magical;
                    poisoned.effectParticleSystem.Add(psSystems[8]);
                    poisoned.effectParticleSystem.Add(psSystems[9]);
                    psSystems[8].Play();
                    psSystems[9].Play();

                    break;
                case BuffType.Corrosion:

                    Debug.Log("Adding corrosion debuff");
                    uiPopupManager.SpawnFlavorText("Corroded!", damageColors[4]);
                    Buff corrosion = transform.Find("BuffContainer").gameObject.AddComponent<Buff>();
                    corrosion.connectedIcon = buffIcon;
                    buffIcon.GetComponent<Image>().sprite = buffIcons[6];

                    activeBuffs.Add(corrosion);

                    corrosion.myType = buff;
                    corrosion.connectedPlayer = stats;
                    corrosion.infiniteDuration = false;
                    corrosion.duration = 20;
                    corrosion.ChangeDefensiveStats(true, 0, 0, 0, 0, stats.armor * -0.8f, stats.magicResist * -0.8f, 0);
                    corrosion.effectParticleSystem.Add(psSystems[10]);
                    corrosion.effectParticleSystem.Add(psSystems[11]);
                    psSystems[10].Play();
                    psSystems[11].Play();
                    psSystems[12].Play();

                    break;
                case BuffType.Frostbite:

                    Debug.Log("Adding frostbite debuff");
                    uiPopupManager.SpawnFlavorText("Frostbitten!", damageColors[5]);
                    Buff frostbite = transform.Find("BuffContainer").gameObject.AddComponent<Buff>();
                    frostbite.connectedIcon = buffIcon;
                    buffIcon.GetComponent<Image>().sprite = buffIcons[7];

                    activeBuffs.Add(frostbite);

                    frostbite.myType = buff;
                    frostbite.connectedPlayer = stats;
                    frostbite.infiniteDuration = false;
                    frostbite.duration = 10;
                    frostbite.ChangeCoreStats(true, 0, 0, 0, -stats.Spd - 15, 0, 0, 0);
                    frostbite.effectParticleSystem.Add(psSystems[13]);
                    frostbite.effectParticleSystem.Add(psSystems[14]);
                    psSystems[13].Play();
                    psSystems[14].Play();

                    break;
                case BuffType.EmboldeningEmbers:

                    Debug.Log("Adding emboldening embers buff");
                    Buff embers = transform.Find("BuffContainer").gameObject.AddComponent<Buff>();
                    embers.connectedIcon = buffIcon;
                    buffIcon.GetComponent<Image>().sprite = buffIcons[8];

                    activeBuffs.Add(embers);

                    embers.myType = buff;
                    embers.connectedPlayer = stats;
                    embers.infiniteDuration = false;
                    embers.duration = 10;
                    embers.ChangeCoreStats(true, Mathf.RoundToInt(stats.Vit * 0.2f), 0, Mathf.RoundToInt(stats.Dex * 0.2f), Mathf.RoundToInt(stats.Spd * 0.2f), 0, 0, 0);
                    embers.effectParticleSystem.Add(psSystems[17]);
                    embers.effectParticleSystem.Add(psSystems[18]);
                    psSystems[17].Play();
                    psSystems[18].Play();
                    psSystems[19].Play();

                    break;
                case BuffType.FlameStrike:

                    Debug.Log("Adding flame strike buff");
                    Buff flameStrike = transform.Find("BuffContainer").gameObject.AddComponent<Buff>();
                    flameStrike.connectedIcon = buffIcon;
                    buffIcon.GetComponent<Image>().sprite = buffIcons[9];

                    activeBuffs.Add(flameStrike);

                    flameStrike.myType = buff;
                    flameStrike.connectedPlayer = stats;
                    flameStrike.infiniteDuration = false;
                    flameStrike.duration = 15;
                    flameStrike.ChangeOffensiveStats(true, 0, 0.05f * stats.Int);
                    flameStrike.damageColor = damageColors[0];
                    flameStrike.onHitDamageAmount = stats.baseDamage * (stats.Int * 0.2f);
                    flameStrike.effectParticleSystem.Add(psSystems[20]);
                    flameStrike.effectParticleSystem.Add(psSystems[21]);
                    flameStrike.effectParticleSystem.Add(psSystems[22]);
                    psSystems[20].Play();
                    psSystems[21].Play();
                    psSystems[22].Play();

                    break;
                case BuffType.AspectOfRage:

                    Debug.Log("Adding aspect of Rage buff");
                    Buff aspectOfRage = transform.Find("BuffContainer").gameObject.AddComponent<Buff>();
                    aspectOfRage.connectedIcon = buffIcon;
                    buffIcon.GetComponent<Image>().sprite = buffIcons[10];

                    activeBuffs.Add(aspectOfRage);

                    aspectOfRage.myType = buff;
                    aspectOfRage.connectedPlayer = stats;
                    aspectOfRage.infiniteDuration = false;
                    aspectOfRage.duration = 15;
                    aspectOfRage.ChangeOffensiveStats(true, 1f, 0.4f);
                    aspectOfRage.ChangeDefensiveStats(true, 0, 0, 0, 0, 0, 0, -50);
                    aspectOfRage.effectParticleSystem.Add(psSystems[27]);
                    aspectOfRage.effectParticleSystem.Add(psSystems[28]);
                    aspectOfRage.effectParticleSystem.Add(psSystems[29]);
                    psSystems[27].Play();
                    psSystems[28].Play();
                    psSystems[29].Play();

                    break;
                case BuffType.BlessingOfFlames:
                    Debug.Log("Adding blessing of flames buff");
                    Buff blessingOfFlames = transform.Find("BuffContainer").gameObject.AddComponent<Buff>();
                    blessingOfFlames.connectedIcon = buffIcon;
                    buffIcon.GetComponent<Image>().sprite = buffIcons[11];

                    activeBuffs.Add(blessingOfFlames);

                    blessingOfFlames.myType = buff;
                    blessingOfFlames.connectedPlayer = stats;
                    blessingOfFlames.infiniteDuration = false;
                    blessingOfFlames.duration = 15;
                    blessingOfFlames.ChangeDefensiveStats(true, 0, 0, stats.healthRegen, 0, stats.armor * 0.25f, stats.magicResist * 0.25f, 0);
                    blessingOfFlames.ChangeAfflictionStats(true, 0.5f, 0, 0, 0.1f, 0.1f, 0.1f, 0.3f, 0.1f, 0);
                    blessingOfFlames.effectParticleSystem.Add(psSystems[30]);
                    psSystems[30].Play();

                    break;
                case BuffType.Rampage:
                    Debug.Log("adding rampage buff");
                    Buff rampage = transform.Find("BuffContainer").gameObject.AddComponent<Buff>();
                    rampage.connectedIcon = buffIcon;
                    rampage.iconStacks = buffIcon.GetComponentInChildren<Text>();
                    buffIcon.GetComponent<Image>().sprite = buffIcons[12];

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
                    rampage.ChangeOffensiveStats(true, 0, 0.04f);
                    //rampage.effectParticleSystem.Add(psSystems[30]);
                    //psSystems[30].Play();

                    break;
                case BuffType.GiantStrength:
                    Debug.Log("adding giant Strength buff");
                    Buff giantStrength = transform.Find("BuffContainer").gameObject.AddComponent<Buff>();
                    giantStrength.connectedIcon = buffIcon;
                    giantStrength.iconStacks = buffIcon.GetComponentInChildren<Text>();
                    buffIcon.GetComponent<Image>().sprite = buffIcons[13];

                    activeBuffs.Add(giantStrength);

                    giantStrength.myType = buff;
                    giantStrength.infiniteDuration = false;
                    giantStrength.duration = 12f;
                    giantStrength.connectedPlayer = stats;
                    giantStrength.ChangeCoreStats(true, stats.Vit, stats.Str, -stats.Dex / 2, -stats.Spd / 2, -stats.Int / 2, -stats.Wis / 2, -stats.Cha / 2);
                    giantStrength.ChangeSize(true, 0.25f);
                    giantStrength.ChangeDefensiveStats(true, 0, 0, 0, 0, 0, 0, 0.25f);
                    giantStrength.effectParticleSystem.Add(psSystems[31]);
                    psSystems[31].Play();

                    break;
                case BuffType.ToxicRipple:
                    Debug.Log("adding Toxic Ripple buff");

                    Buff toxicRipple = transform.Find("BuffContainer").gameObject.AddComponent<Buff>();
                    toxicRipple.connectedIcon = buffIcon;
                    toxicRipple.iconStacks = buffIcon.GetComponentInChildren<Text>();
                    buffIcon.GetComponent<Image>().sprite = buffIcons[14];

                    activeBuffs.Add(toxicRipple);

                    toxicRipple.myType = buff;
                    toxicRipple.infiniteDuration = false;
                    toxicRipple.duration = 10f;
                    toxicRipple.connectedPlayer = stats;
                    toxicRipple.ChangeAfflictionStats(true, 0, 0, 0, 0, 0.7f, 0, 0, 0.7f, 0);

                    break;
                case BuffType.KillerInstinct:
                    Debug.Log("adding Killer instinct buff");

                    Buff killerInstinct = transform.Find("BuffContainer").gameObject.AddComponent<Buff>();
                    killerInstinct.connectedIcon = buffIcon;
                    killerInstinct.iconStacks = buffIcon.GetComponentInChildren<Text>();
                    buffIcon.GetComponent<Image>().sprite = buffIcons[15];

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
                    Debug.Log("adding nature pulse debuff");

                    Buff naturePulse = transform.Find("BuffContainer").gameObject.AddComponent<Buff>();
                    naturePulse.connectedIcon = buffIcon;
                    naturePulse.iconStacks = buffIcon.GetComponentInChildren<Text>();
                    buffIcon.GetComponent<Image>().sprite = buffIcons[16];

                    activeBuffs.Add(naturePulse);

                    naturePulse.myType = buff;
                    naturePulse.infiniteDuration = false;
                    naturePulse.maxStacks = 5;
                    naturePulse.currentStacks = 1;
                    naturePulse.stackable = true;
                    naturePulse.duration = 6f;
                    naturePulse.connectedPlayer = stats;
                    naturePulse.ChangeDefensiveStats(true, 0, 0, 0, 0, -stats.armor / 10, -stats.magicResist / 10, 0);
                    //naturePulse.effectParticleSystem.Add(psSystems[32]);
                    //naturePulse.endOfBuffParticleSystem.Add(psSystems[33]);
                    //psSystems[32].Play();
                    break;
                case BuffType.Revitalize:
                    Debug.Log("adding revitalize buff");

                    Buff revitalize = transform.Find("BuffContainer").gameObject.AddComponent<Buff>();
                    revitalize.connectedIcon = buffIcon;
                    revitalize.iconStacks = buffIcon.GetComponentInChildren<Text>();
                    buffIcon.GetComponent<Image>().sprite = buffIcons[17];

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
    public void RemoveBuff(Buff buffToRemove)
    {
        activeBuffs.Remove(buffToRemove);
        activeIcons.Remove(buffToRemove.connectedIcon);
        UpdateIconLocations();
        // Call the icon reshuffle here.
        afflictionManager.RemoveBar(buffToRemove.myType);
        Destroy(buffToRemove.connectedIcon);
        Destroy(buffToRemove);
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
                    NewBuff(BuffType.Rampage);
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
                    target.GetComponent<PlayerStats>().TakeDamage(buff.onHitDamageAmount, false, HitBox.DamageType.Magical);
                    psSystems[23].Play();
                    psSystems[24].Play();
                    psSystems[25].Play();
                    psSystems[26].Play();
                    break;
                case BuffType.KillerInstinct:
                    Debug.Log("killer instinct hit has been procced");
                    hitbox.bypassCrit = true;
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
                    buff.currentTimer = buff.duration;
                    break;
                default:
                    break;
            }
        }
    }
    
    // USed to proc my onhit effects onto an enemy
    public void ProcOnHits(GameObject target, HitBoxTerrain hitbox)
    {
        Debug.Log("proccing on hits");
        // Check if we have any skills that apply buffs on hit.
        foreach (Skill skill in skillManager.mySkills)
        {
            switch (skill.skillName)
            {
                case SkillsManager.SkillNames.Rampage:
                    NewBuff(BuffType.Rampage);
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
                    target.GetComponent<PlayerStats>().TakeDamage(buff.onHitDamageAmount, false, HitBox.DamageType.Magical);
                    psSystems[23].Play();
                    psSystems[24].Play();
                    psSystems[25].Play();
                    psSystems[26].Play();
                    break;
                case BuffType.KillerInstinct:
                    Debug.Log("killer instinct hit has been procced");
                    hitbox.bypassCrit = true;
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
                    buff.currentTimer = buff.duration;
                    break;
                default:
                    break;
            }
        }
    }
}
