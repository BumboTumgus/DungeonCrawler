using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboManager : MonoBehaviour
{
    public Animator comboAnim;

    public float currentcombo = 0f;

    private Text comboCounterText;
    private float currentComboTimer = 0f;
    private float targetComboTimer = 5f;

    private void Start()
    {
        comboCounterText = comboAnim.transform.Find("ComboNumber").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if(currentcombo > 0)
        {
            currentComboTimer += Time.deltaTime;
            if(currentComboTimer > targetComboTimer)
            {
                currentComboTimer = 0f;
                currentcombo = 0;
                comboAnim.Play("ComboFadeOut");
            }
        }
    }

    public void AddComboCounter(float amount)
    {
        currentcombo += amount;
        currentComboTimer = 0;
        comboAnim.Play("ComboPopIn", 0, 0f);
        comboCounterText.text = Mathf.RoundToInt(currentcombo) + "";
    }
}
