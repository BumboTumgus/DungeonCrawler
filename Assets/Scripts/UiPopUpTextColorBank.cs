using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiPopUpTextColorBank : MonoBehaviour
{
    public static UiPopUpTextColorBank instance;

    public Color[] damageColors;

    // Start is called before the first frame update
    void Start()
    {
        if (!instance)
            instance = this;
        else
            Destroy(this);
    }
}
