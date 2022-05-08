using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiCurrentTimeBehaviour : MonoBehaviour
{
    [SerializeField] Text timerText;
    [SerializeField] float currentTime = 0;


    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.L))
            //currentTime += UnityEngine.Random.Range(1000f, 10000f);

        currentTime += Time.deltaTime;

        TimeSpan ts = TimeSpan.FromSeconds(currentTime);
        string result = "";

        if(currentTime < 600)
            result = ts.ToString("m\\:ss\\.ff");
        else if (currentTime < 3600)
            result = ts.ToString("mm\\:ss\\.ff");
        else if (currentTime < 36000)
            result = ts.ToString("h\\:mm\\:ss\\.ff");
        else
            result = ts.ToString("hh\\:mm\\:ss\\.ff");

        timerText.text = result;
    }
}
