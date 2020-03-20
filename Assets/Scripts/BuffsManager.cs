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

    public enum BuffType { Aflame, Asleep, Stunned, Cursed, Bleeding, Poisoned, Corrosion, Frostbite, EmboldeningEmbers };

    [SerializeField] private Sprite[] buffIcons;
    [SerializeField] private Color[] damageColors;
    [SerializeField] private ParticleSystem[] psSystems;
    private PlayerStats stats;
    private AfflictionManager afflictionManager;
    private EffectsManager effects;


    // Grabs the players stats for use when calculating buff strength.
    private void Start()
    {
        stats = GetComponent<PlayerStats>();
        afflictionManager = GetComponent<AfflictionManager>();
        effects = GetComponent<EffectsManager>();
        foreach (ParticleSystem ps in psSystems)
            ps.Stop();
    }

    // Used to add a new buff to oiur player, if they already have it from this source, we refresh it instead.
    public void NewBuff(BuffType buff)
    {
        Debug.Log("Addding buffs");
        bool buffDealtWith = false;

        // Check to see if any of our buffs match this buff, and if the source matches then we reset the duration.
        foreach(Buff activeBuff in activeBuffs)
        {
            if (activeBuff.myType == buff)
            {
                buffDealtWith = true;
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
                    Buff asleep = transform.Find("BuffContainer").gameObject.AddComponent<Buff>();
                    asleep.connectedIcon = buffIcon;
                    buffIcon.GetComponent<Image>().sprite = buffIcons[1];

                    activeBuffs.Add(asleep);

                    asleep.myType = buff;
                    asleep.connectedPlayer = stats;
                    asleep.infiniteDuration = false;
                    asleep.duration = 5;
                    asleep.DPS = 0;
                    GetComponent<PlayerController>().AsleepLaunch(5);
                    asleep.effectParticleSystem.Add(psSystems[1]);
                    psSystems[1].Play();

                    break;
                case BuffType.Stunned:

                    Debug.Log("Adding stun debuff");
                    Buff stunned = transform.Find("BuffContainer").gameObject.AddComponent<Buff>();
                    stunned.connectedIcon = buffIcon;
                    buffIcon.GetComponent<Image>().sprite = buffIcons[2];

                    activeBuffs.Add(stunned);

                    stunned.myType = buff;
                    stunned.connectedPlayer = stats;
                    stunned.infiniteDuration = false;
                    stunned.duration = 2;
                    stunned.DPS = 0;
                    GetComponent<PlayerController>().StunLaunch(2);
                    stunned.effectParticleSystem.Add(psSystems[2]);
                    psSystems[2].Play();

                    break;
                case BuffType.Cursed:

                    Debug.Log("Adding cursed debuff");
                    Buff cursed = transform.Find("BuffContainer").gameObject.AddComponent<Buff>();
                    cursed.connectedIcon = buffIcon;
                    buffIcon.GetComponent<Image>().sprite = buffIcons[3];

                    activeBuffs.Add(cursed);

                    cursed.myType = buff;
                    cursed.connectedPlayer = stats;
                    cursed.ChangeCoreStats(Mathf.RoundToInt(stats.Vit * -0.5f), Mathf.RoundToInt(stats.Str * -0.5f), Mathf.RoundToInt(stats.Dex * -0.5f),
                        Mathf.RoundToInt(stats.Spd * -0.5f), Mathf.RoundToInt(stats.Int * -0.5f), Mathf.RoundToInt(stats.Wis * -0.5f), Mathf.RoundToInt(stats.Cha * -0.5f));
                    stats.TakeDamage(stats.healthMax / 2, false, damageColors[1]);
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
                    GetComponent<PlayerController>().bleeding = true;
                    stats.TakeDamage(stats.healthMax * 0.25f, false, damageColors[2]);
                    bleed.effectParticleSystem.Add(psSystems[6]);
                    psSystems[6].Play();
                    psSystems[5].Play();

                    break;
                case BuffType.Poisoned:

                    Debug.Log("Adding poison debuff");
                    Buff poisoned = transform.Find("BuffContainer").gameObject.AddComponent<Buff>();
                    poisoned.connectedIcon = buffIcon;
                    buffIcon.GetComponent<Image>().sprite = buffIcons[5];

                    activeBuffs.Add(poisoned);

                    poisoned.myType = buff;
                    poisoned.connectedPlayer = stats;
                    poisoned.infiniteDuration = false;
                    poisoned.duration = 20;
                    poisoned.DPS = stats.healthMax * 0.05f;
                    poisoned.damageColor = damageColors[3];
                    poisoned.effectParticleSystem.Add(psSystems[8]);
                    poisoned.effectParticleSystem.Add(psSystems[9]);
                    psSystems[8].Play();
                    psSystems[9].Play();

                    break;
                case BuffType.Corrosion:

                    Debug.Log("Adding corrosion debuff");
                    Buff corrosion = transform.Find("BuffContainer").gameObject.AddComponent<Buff>();
                    corrosion.connectedIcon = buffIcon;
                    buffIcon.GetComponent<Image>().sprite = buffIcons[6];

                    activeBuffs.Add(corrosion);

                    corrosion.myType = buff;
                    corrosion.connectedPlayer = stats;
                    corrosion.infiniteDuration = false;
                    corrosion.duration = 20;
                    corrosion.ChangeDefensiveStats(0, 0, 0, 0, stats.armor * -0.8f, stats.magicResist * -0.8f);
                    corrosion.effectParticleSystem.Add(psSystems[10]);
                    corrosion.effectParticleSystem.Add(psSystems[11]);
                    psSystems[10].Play();
                    psSystems[11].Play();
                    psSystems[12].Play();

                    break;
                case BuffType.Frostbite:

                    Debug.Log("Adding frostbite debuff");
                    Buff frostbite = transform.Find("BuffContainer").gameObject.AddComponent<Buff>();
                    frostbite.connectedIcon = buffIcon;
                    buffIcon.GetComponent<Image>().sprite = buffIcons[7];

                    activeBuffs.Add(frostbite);

                    frostbite.myType = buff;
                    frostbite.connectedPlayer = stats;
                    frostbite.infiniteDuration = false;
                    frostbite.duration = 10;
                    frostbite.ChangeCoreStats(0, 0, 0, -stats.Spd - 15, 0, 0, 0);
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
                    embers.ChangeCoreStats(5, 5, 0, 0, 0, 0, 0);
                    embers.effectParticleSystem.Add(psSystems[17]);
                    embers.effectParticleSystem.Add(psSystems[18]);
                    psSystems[17].Play();
                    psSystems[18].Play();
                    psSystems[19].Play();

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
        for (int index = 0; index < activeIcons.Count; index++)
            activeIcons[index].transform.localPosition = new Vector3(-90 + (index * 25), 15, 0);
    }
}
