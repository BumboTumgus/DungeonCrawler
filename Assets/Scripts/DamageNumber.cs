﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageNumber : MonoBehaviour
{
    public bool xMovementEnabled = true;
    public bool shieldIconOverride = false;

    private Text damageText;
    private Image shieldImage;
    private float xMovement;

    private const float MAX_X_MOVEMENT = 3.5f;
    private const float LIFETIME = 1.5f;
    private const float X_DECAY_SPEED = 0.97f;

    // Start is called before the first frame update
    void Start()
    {
        damageText = GetComponentInChildren<Text>();
        xMovement = Random.Range(-MAX_X_MOVEMENT, MAX_X_MOVEMENT);
        transform.SetAsFirstSibling();
    }
    
    // Called by the damage number manager to insatntiate and setup this damage number.
    public void SetDamageNumber(string value, Color textColor, float sizeMod)
    {
        if (!shieldIconOverride)
        {
            damageText = GetComponentInChildren<Text>();
            damageText.text = value;
            damageText.fontSize = (int)Mathf.Round(damageText.fontSize * sizeMod);
            damageText.color = textColor;
        }
        else
        {
            shieldImage = GetComponentInChildren<Image>();
            shieldImage.color = textColor;
        }
        StartCoroutine(NumberMovement());
    }

    // Used to make the numebr fly up
    IEnumerator NumberMovement()
    {
        // Set up the timer and start the vertical movement animation and fade.
        float currentTimer = 0;
        if(!shieldIconOverride)
            damageText.GetComponent<Animator>().Play("DamageNumberPop");
        else
            shieldImage.GetComponent<Animator>().Play("ResistShieldPop");

        if (!shieldIconOverride)
        {
            while (currentTimer < LIFETIME)
            {
                // Increment the timer and move the text side to side.
                currentTimer += Time.deltaTime;
                if (xMovementEnabled)
                {
                    damageText.transform.Translate(xMovement, 0, 0);
                    xMovement *= X_DECAY_SPEED;
                }

                yield return new WaitForEndOfFrame();
            }
        }
        else
        {
            while (currentTimer < LIFETIME)
            {
                // Increment the timer and move the text side to side.
                currentTimer += Time.deltaTime;
                if (xMovementEnabled)
                {
                    shieldImage.transform.Translate(xMovement, 0, 0);
                    xMovement *= X_DECAY_SPEED;
                }

                yield return new WaitForEndOfFrame();
            }
        }


        // Destroy ourselves.
        GetComponent<UiFollowTarget>().RemoveFromCullList();
        Destroy(gameObject);
    }
}
