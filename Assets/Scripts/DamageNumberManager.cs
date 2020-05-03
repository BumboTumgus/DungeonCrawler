using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageNumberManager : MonoBehaviour
{
    public Transform numberTarget;
    public GameObject damageNumberPrefab;
    public GameObject expNumberPrefab;
    public Color physicalDamageColor;
    public Color physicalDamageCritColor;
    public Color defendedColor;
    public Color xpColor;
    
    private Transform primaryCanvas;

    // Grab the canvas we spawn the damage numebrs to.
    private void Start()
    {
        primaryCanvas = GameObject.Find("PrimaryCanvas").transform;
    }

    // USed to spawn the damage numbers.
    public void SpawnNumber(float value, bool crit)
    {
        // Spawn the damage number offscreen.
        GameObject damageNumber = Instantiate(damageNumberPrefab, new Vector3(1000, 1000, 1000), new Quaternion(0,0,0,0), primaryCanvas);
        damageNumber.GetComponent<UiFollowTarget>().target = numberTarget;

        // Check what kind of damage was dealt and set the target text color accordingly;
        if(value <= 0)
            damageNumber.GetComponent<DamageNumber>().SetDamageNumber("MISS", defendedColor, 0.8f);
        else if (crit)
            damageNumber.GetComponent<DamageNumber>().SetDamageNumber(Mathf.Round(value) + "!", physicalDamageCritColor, 1.2f);
        else
            damageNumber.GetComponent<DamageNumber>().SetDamageNumber(Mathf.Round(value) + "", physicalDamageColor, 1);
    }

    // USed to spawn the damage numbers and forcibly change it's color.
    public void SpawnNumber(float value, bool crit, Color colorOveride)
    {
        // Spawn the damage number offscreen.
        GameObject damageNumber = Instantiate(damageNumberPrefab, new Vector3(1000, 1000, 1000), new Quaternion(0, 0, 0, 0), primaryCanvas);
        damageNumber.GetComponent<UiFollowTarget>().target = numberTarget;

        // Check what kind of damage was dealt and set the target text color accordingly;
        if (value <= 0)
            damageNumber.GetComponent<DamageNumber>().SetDamageNumber("BLOCKED", colorOveride, 0.8f);
        else if (crit)
            damageNumber.GetComponent<DamageNumber>().SetDamageNumber(Mathf.Round(value) + "!", colorOveride, 1.5f);
        else
            damageNumber.GetComponent<DamageNumber>().SetDamageNumber(Mathf.Round(value) + "", colorOveride, 1);
    }

    //USed to sapwn an exp value for the target when they Die.
    public void SpawnEXPValue(float value)
    {
        GameObject expNumber = Instantiate(expNumberPrefab, new Vector3(1000, 1000, 1000), new Quaternion(0, 0, 0, 0), primaryCanvas);
        expNumber.GetComponent<UiFollowTarget>().target = numberTarget;

        string xpText = "+" + string.Format("{0:0}", value) + " XP";
        expNumber.GetComponent<DamageNumber>().SetDamageNumber(xpText, xpColor, 1);
    }
}
