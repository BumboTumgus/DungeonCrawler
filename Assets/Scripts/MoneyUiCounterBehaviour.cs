using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyUiCounterBehaviour : MonoBehaviour
{
    [SerializeField] Text moneyText;

    public float currentTextValue = 0;
    public float targetTextValue = 0;

    Coroutine LerpingToValue;

    private void Start()
    {
        moneyText.text = "0";
        currentTextValue = 0;
        targetTextValue = 0;
    }

    // Called when we want to change the gold value
    public void ChangeGoldValue(float value)
    {
        targetTextValue = value;

        // Stop the previous lerp if we gotta move our value again.
        if (LerpingToValue != null)
            StopCoroutine(LerpingToValue);

        LerpingToValue = StartCoroutine(LerpToValue());
    }

    // Lerps us to the target value over 1.5 second.
    IEnumerator LerpToValue()
    {
        float currentTimer = 0;
        float targetTimer = 1;
        float originalValue = currentTextValue;

        while(currentTimer < targetTimer)
        {
            currentTextValue = Mathf.Lerp(originalValue, targetTextValue, currentTimer / targetTimer);
            moneyText.text = (int)currentTextValue + "";

            currentTimer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        currentTextValue = targetTextValue;
        moneyText.text = (int)currentTextValue + "";
    }
}
