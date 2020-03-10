using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarManager : MonoBehaviour
{
    public float currentBarValue;
    public float currentDecayBarValue;
    public float targetValue;
    public float maximumValue = 100;
    public float minimumValue = 0;
    public bool decayEffect = true;
    public bool linkedText = false;
    public float primaryLerpSpeed = 5;
    public float secondaryLerpSpeed = 5;
    public Image primaryContent;
    public Image decayContent;
    public Color decayDownColor;
    public Color decayUpColor;
    public Text valueDisplay;

    // The intial setup of the bar.
    private void Start()
    {
        // Initialize(maximumValue, false);
        // If we are not using the decay effect hide the bar used for it.
        if (!decayEffect && decayContent != null)
            decayContent.gameObject.SetActive(false);
    }

    // reset the bar with a new max Value.
    public void Initialize(float maxValue, bool matchBarValue)
    {
        maximumValue = maxValue;
        primaryContent.fillAmount = currentBarValue / maximumValue;
        if (matchBarValue)
        {
            currentBarValue = maxValue;
            targetValue = maximumValue;
            if(decayEffect)
                currentDecayBarValue = maxValue;
        }
    }

    // Checks to see if our values are close to the target or not and slowly moves towards them if the lerp option is ticked.
    private void Update()
    {
        // Primary bar logic.
        if (currentBarValue != targetValue)
        {
            if (!decayEffect)
                currentBarValue = Mathf.Lerp(currentBarValue, targetValue, primaryLerpSpeed / 100);
            else
            {
                if (currentBarValue - targetValue > 0)
                {
                    decayContent.color = decayDownColor;
                    currentBarValue = Mathf.Lerp(currentBarValue, targetValue, primaryLerpSpeed / 100);
                    currentDecayBarValue = Mathf.Lerp(currentDecayBarValue, targetValue, secondaryLerpSpeed / 100);
                }
                else
                {
                    decayContent.color = decayUpColor;
                    currentBarValue = Mathf.Lerp(currentBarValue, targetValue, secondaryLerpSpeed / 100);
                    currentDecayBarValue = Mathf.Lerp(currentDecayBarValue, targetValue, primaryLerpSpeed / 100);
                }
            }
        }
        SetBarValues();
    }

    // Used to set the bars values to their targets
    private void SetBarValues()
    {
        primaryContent.fillAmount = currentBarValue / maximumValue;
        if (decayEffect)
            decayContent.fillAmount = currentDecayBarValue / maximumValue;
        if (linkedText)
            valueDisplay.text = string.Format("{0:n0}", targetValue);
    }

    // USed by other classes to set what value this bar will display.
    public void SetValue(float target)
    {
        targetValue = target;
        if (targetValue > maximumValue)
            targetValue = maximumValue;
        else if (targetValue < minimumValue)
            targetValue = minimumValue;
    }
}
