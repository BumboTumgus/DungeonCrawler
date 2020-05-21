using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfflictionManager : MonoBehaviour
{
    public List<GameObject> activeBars;

    public float currentAflameValue = 0;
    public float currentAsleepValue = 0;
    public float currentStunValue = 0;
    public float currentCurseValue = 0;
    public float currentBleedValue = 0;
    public float currentPoisonValue = 0;
    public float currentCorrosionValue = 0;
    public float currentFrostbiteValue = 0;

    public float aflameResist = 0;
    public float sleepResist = 0;
    public float stunResist = 0;
    public float curseResist = 0;
    public float bleedResist = 0;
    public float poisonResist = 0;
    public float corrosionResist = 0;
    public float frostbiteResist = 0;
    public float knockBackResist = 0;

    public BarManager aflameBar;
    public BarManager sleepBar;
    public BarManager stunBar;
    public BarManager curseBar;
    public BarManager bleedBar;
    public BarManager poisonBar;
    public BarManager corrosionBar;
    public BarManager frostbiteBar;

    public enum AfflictionTypes { Aflame, Asleep, Stun, Curse, Bleed, Poison, Corrosion, Frostbite };

    private const float AFLAME_DECAY_RATE = 5;
    private const float ASLEEP_DECAY_RATE = 20;
    private const float STUN_DECAY_RATE = 50;
    private const float CURSE_DECAY_RATE = 5;
    private const float BLEED_DECAY_RATE = 10;
    private const float POISON_DECAY_RATE = 5;
    private const float CORROSION_DECAY_RATE = 5;
    private const float FROSTBITE_DECAY_RATE = 10;

    private BuffsManager playerBuffManager;
    private DamageNumberManager uiPopupManager;

    // Start is called before the first frame update
    void Start()
    {
        playerBuffManager = GetComponent<BuffsManager>();
        uiPopupManager = GetComponent<DamageNumberManager>();

        if (gameObject.CompareTag("Player"))
        {
            aflameBar.Initialize(100, false);
            sleepBar.Initialize(100, false);
            stunBar.Initialize(100, false);
            curseBar.Initialize(100, false);
            bleedBar.Initialize(100, false);
            poisonBar.Initialize(100, false);
            corrosionBar.Initialize(100, false);
            frostbiteBar.Initialize(100, false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        currentAflameValue = LowerValue(currentAflameValue, AFLAME_DECAY_RATE, aflameBar);
        currentAsleepValue = LowerValue(currentAsleepValue, ASLEEP_DECAY_RATE, sleepBar);
        currentStunValue = LowerValue(currentStunValue, STUN_DECAY_RATE, stunBar);
        currentCurseValue = LowerValue(currentCurseValue, CURSE_DECAY_RATE, curseBar);
        currentBleedValue = LowerValue(currentBleedValue, BLEED_DECAY_RATE, bleedBar);
        currentPoisonValue = LowerValue(currentPoisonValue, POISON_DECAY_RATE, poisonBar);
        currentCorrosionValue = LowerValue(currentCorrosionValue, CORROSION_DECAY_RATE, corrosionBar);
        currentFrostbiteValue = LowerValue(currentFrostbiteValue, FROSTBITE_DECAY_RATE, frostbiteBar);
    }

    // Used to update a bar and decay a value down.
    private float LowerValue(float originalValue, float decayRate, BarManager barToUpdate)
    {
        if (originalValue != 0)
        {
            originalValue -= decayRate * Time.deltaTime;
            if (originalValue < 0)
            {
                originalValue = 0;
                barToUpdate.transform.parent.gameObject.SetActive(false);
                activeBars.Remove(barToUpdate.transform.parent.gameObject);
                UpdateBarLocations();
            }
            if(barToUpdate != null)
                barToUpdate.SetValue(originalValue);
        }
        return originalValue;
    }

    // Used to add a value to the different bars.
    public void AddAffliction(AfflictionTypes affliction, float value)
    {
        // Debug.Log(affliction + " has had " + value + " added.");
        switch (affliction)
        {
            case AfflictionTypes.Aflame:
                currentAflameValue += value * (1 - aflameResist);
                if (currentAflameValue >= 100)
                {
                    currentAflameValue = 100;
                    playerBuffManager.NewBuff(BuffsManager.BuffType.Aflame);

                }
                if (aflameBar != null && !aflameBar.transform.parent.gameObject.activeSelf && aflameResist < 1)
                {
                    aflameBar.transform.parent.gameObject.SetActive(true);
                    activeBars.Add(aflameBar.transform.parent.gameObject);
                    UpdateBarLocations();
                }
                break;
            case AfflictionTypes.Asleep:
                currentAsleepValue += value * (1 - sleepResist);
                if (currentAsleepValue >= 100)
                {
                    currentAsleepValue = 100;
                    playerBuffManager.NewBuff(BuffsManager.BuffType.Asleep);
                }
                if (sleepBar != null && !sleepBar.transform.parent.gameObject.activeSelf && sleepResist < 1)
                {
                    sleepBar.transform.parent.gameObject.SetActive(true);
                    activeBars.Add(sleepBar.transform.parent.gameObject);
                    UpdateBarLocations();
                }
                break;
            case AfflictionTypes.Stun:
                currentStunValue += value * (1 - stunResist);
                if (currentStunValue >= 100)
                {
                    currentStunValue = 100;
                    playerBuffManager.NewBuff(BuffsManager.BuffType.Stunned);
                }
                if (stunBar != null && !stunBar.transform.parent.gameObject.activeSelf && stunResist < 1)
                {
                    stunBar.transform.parent.gameObject.SetActive(true);
                    activeBars.Add(stunBar.transform.parent.gameObject);
                    UpdateBarLocations();
                }
                break;
            case AfflictionTypes.Curse:
                currentCurseValue += value * (1 - curseResist);
                if (currentCurseValue >= 100)
                {
                    currentCurseValue = 100;
                    playerBuffManager.NewBuff(BuffsManager.BuffType.Cursed);
                }
                if (curseBar != null && !curseBar.transform.parent.gameObject.activeSelf && curseResist < 1)
                {
                    curseBar.transform.parent.gameObject.SetActive(true);
                    activeBars.Add(curseBar.transform.parent.gameObject);
                    UpdateBarLocations();
                }
                break;
            case AfflictionTypes.Bleed:
                currentBleedValue += value * (1 - bleedResist);
                if (currentBleedValue >= 100)
                {
                    currentBleedValue = 100;
                    playerBuffManager.NewBuff(BuffsManager.BuffType.Bleeding);
                }
                if (bleedBar != null && !bleedBar.transform.parent.gameObject.activeSelf && bleedResist < 1)
                {
                    bleedBar.transform.parent.gameObject.SetActive(true);
                    activeBars.Add(bleedBar.transform.parent.gameObject);
                    UpdateBarLocations();
                }
                break;
            case AfflictionTypes.Poison:
                currentPoisonValue += value * (1 - poisonResist);
                if (currentPoisonValue >= 100)
                {
                    currentPoisonValue = 100;
                    playerBuffManager.NewBuff(BuffsManager.BuffType.Poisoned);
                }
                if (poisonBar != null && !poisonBar.transform.parent.gameObject.activeSelf && poisonResist < 1)
                {
                    poisonBar.transform.parent.gameObject.SetActive(true);
                    activeBars.Add(poisonBar.transform.parent.gameObject);
                    UpdateBarLocations();
                }
                break;
            case AfflictionTypes.Corrosion:
                currentCorrosionValue += value * (1 - corrosionResist);
                if (currentCorrosionValue >= 100)
                {
                    currentCorrosionValue = 100;
                    playerBuffManager.NewBuff(BuffsManager.BuffType.Corrosion);
                }
                if (corrosionBar != null && !corrosionBar.transform.parent.gameObject.activeSelf && corrosionResist < 1)
                {
                    corrosionBar.transform.parent.gameObject.SetActive(true);
                    activeBars.Add(corrosionBar.transform.parent.gameObject);
                    UpdateBarLocations();
                }
                break;
            case AfflictionTypes.Frostbite:
                currentFrostbiteValue += value * (1 - frostbiteResist);
                if (currentFrostbiteValue >= 100)
                {
                    currentFrostbiteValue = 100;
                    playerBuffManager.NewBuff(BuffsManager.BuffType.Frostbite);
                }
                if (frostbiteBar != null && !frostbiteBar.transform.parent.gameObject.activeSelf && frostbiteResist < 1)
                {
                    frostbiteBar.transform.parent.gameObject.SetActive(true);
                    activeBars.Add(frostbiteBar.transform.parent.gameObject);
                    UpdateBarLocations();
                }
                break;
            default:
                break;
        }
    }

    // Used to position all the bars based on their indexs.
    private void UpdateBarLocations()
    {
        for(int index = 0; index < activeBars.Count; index++)
            activeBars[index].transform.position = new Vector3(0, index * 56f, 0);
    }

    //USed to hide the bar, and rest it's value to 0, this is in the case the effect is cleansed or removed
    public void RemoveBar(BuffsManager.BuffType buffType)
    {
        switch (buffType)
        {
            case BuffsManager.BuffType.Aflame:
                currentAflameValue = 0.1f;
                break;
            case BuffsManager.BuffType.Asleep:
                currentAsleepValue = 0.1f;
                break;
            case BuffsManager.BuffType.Stunned:
                currentStunValue = 0.1f;
                break;
            case BuffsManager.BuffType.Cursed:
                currentCurseValue = 0.1f;
                break;
            case BuffsManager.BuffType.Bleeding:
                currentBleedValue = 0.1f;
                break;
            case BuffsManager.BuffType.Poisoned:
                currentPoisonValue = 0.1f;
                break;
            case BuffsManager.BuffType.Corrosion:
                currentCorrosionValue = 0.1f;
                break;
            case BuffsManager.BuffType.Frostbite:
                currentFrostbiteValue = 0.1f;
                break;
            default:
                break;
        }
    }
}
