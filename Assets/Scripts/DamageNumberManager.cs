using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageNumberManager : MonoBehaviour
{
    public Transform numberTarget;
    public GameObject damageNumberPrefab;
    public GameObject resistedShieldPrefab;
    public GameObject expNumberPrefab;
    public GameObject flavorTextPrefab;
    public Color xpColor;
    
    private Transform primaryCanvas;

    // Grab the canvas we spawn the damage numebrs to.
    private void Start()
    {
        primaryCanvas = GameObject.Find("PrimaryCanvas").transform;
    }

    // USed to spawn the damage numbers and forcibly change it's color.
    public void SpawnNumber(float value, bool crit, Color colorOveride)
    {
        if (value <= 0)
        {
            GameObject resistShield = Instantiate(resistedShieldPrefab, new Vector3(1000, 1000, 1000), new Quaternion(0, 0, 0, 0), primaryCanvas);
            resistShield.GetComponent<UiFollowTarget>().target = numberTarget;
            resistShield.GetComponent<DamageNumber>().SetDamageNumber("", colorOveride, 1);
        }
        else
        {
            // Spawn the damage number offscreen.
            GameObject damageNumber = Instantiate(damageNumberPrefab, new Vector3(1000, 1000, 1000), new Quaternion(0, 0, 0, 0), primaryCanvas);
            damageNumber.GetComponent<UiFollowTarget>().target = numberTarget;

            // Check what kind of damage was dealt and set the target text color accordingly;

            if (crit)
                damageNumber.GetComponent<DamageNumber>().SetDamageNumber(Mathf.Round(value) + "!", colorOveride, 1.5f);
            else
                damageNumber.GetComponent<DamageNumber>().SetDamageNumber(Mathf.Round(value) + "", colorOveride, 1);
        }
    }

    //USed to sapwn an exp value for the target when they Die.
    public void SpawnEXPValue(float value)
    {
        GameObject expNumber = Instantiate(expNumberPrefab, new Vector3(1000, 1000, 1000), new Quaternion(0, 0, 0, 0), primaryCanvas);
        expNumber.GetComponent<UiFollowTarget>().target = numberTarget;

        string xpText = "+" + string.Format("{0:0}", value) + " XP";
        expNumber.GetComponent<DamageNumber>().SetDamageNumber(xpText, xpColor, 1);
    }

    // Used to spawn a flavor text of some sort from this target.
    public void SpawnFlavorText(string text, Color color)
    {
        GameObject expNumber = Instantiate(flavorTextPrefab, new Vector3(1000, 1000, 1000), new Quaternion(0, 0, 0, 0), primaryCanvas);
        expNumber.GetComponent<UiFollowTarget>().target = numberTarget;
        
        expNumber.GetComponent<DamageNumber>().SetDamageNumber(text, color, 1);
    }
}
