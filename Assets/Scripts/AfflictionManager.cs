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
    private const float CURSE_DECAY_RATE = 2;
    private const float BLEED_DECAY_RATE = 10;
    private const float POISON_DECAY_RATE = 5;
    private const float CORROSION_DECAY_RATE = 5;
    private const float FROSTBITE_DECAY_RATE = 10;

    // Start is called before the first frame update
    void Start()
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
            barToUpdate.targetValue = originalValue;
        }
        return originalValue;
    }

    // Used to add a value to the different bars.
    public void AddAffliction(AfflictionTypes affliction, float value)
    {
        Debug.Log(affliction + " has had " + value + " added.");
        switch (affliction)
        {
            case AfflictionTypes.Aflame:
                currentAflameValue += value * (1 - aflameResist);
                if (currentAflameValue >= 100)
                {
                    currentAflameValue = 100;
                    AflameActivate();
                }
                if (!aflameBar.transform.parent.gameObject.activeSelf)
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
                    AsleepActivate();
                }
                if (!sleepBar.transform.parent.gameObject.activeSelf)
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
                    StunActivate();
                }
                if (!stunBar.transform.parent.gameObject.activeSelf)
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
                    CurseActivate();
                }
                if (!curseBar.transform.parent.gameObject.activeSelf)
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
                    BleedActivate();
                }
                if (!bleedBar.transform.parent.gameObject.activeSelf)
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
                    PoisonActivate();
                }
                if (!poisonBar.transform.parent.gameObject.activeSelf)
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
                    CorrosionActivate();
                }
                if (!corrosionBar.transform.parent.gameObject.activeSelf)
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
                    FrostbiteActivate();
                }
                if (!frostbiteBar.transform.parent.gameObject.activeSelf)
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
        Debug.Log("Updating the bar locations");
        for(int index = 0; index < activeBars.Count; index++)
            activeBars[index].transform.position = new Vector3(0, index * 56f, 0);
    }

    // USed to set the player on fire.
    private void AflameActivate()
    {
        Debug.Log("The player has been set on fire");
    }

    // USed to make the player fall asleep
    private void AsleepActivate()
    {
        Debug.Log("The player was put to sleep");
    }

    // USed to stun the player
    private void StunActivate()
    {
        Debug.Log("The player has been stunned");
    }

    // USed to curse the player
    private void CurseActivate()
    {
        Debug.Log("The player has been cursed");
    }

    // USed to make the player start bleeding
    private void BleedActivate()
    {
        Debug.Log("The player has started to bleed");
    }

    // USed to make the player poisoned
    private void PoisonActivate()
    {
        Debug.Log("The player has been poisoned");
    }

    // USed to make the player Corroded, reducing their armor
    private void CorrosionActivate()
    {
        Debug.Log("The player has had their armor corroded");
    }

    // USed to make the player frostbitten
    private void FrostbiteActivate()
    {
        Debug.Log("The player was Frostbitten");
    }
}
