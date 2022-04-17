using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class BloodyScreenController : MonoBehaviour
{
    private Image bloodyScreen;
    private Color screenTint;

    private void Awake()
    {
        bloodyScreen = GetComponent<Image>();
        screenTint = bloodyScreen.color;
        screenTint.a = 0;
        bloodyScreen.color = screenTint;
    }

    public void SetScreenAlpha(float percentageOfHp)
    {
        if (percentageOfHp > 0.6f)
            screenTint.a = 0;
        else if (percentageOfHp > 0.3f)
        {
            screenTint.a = 1 - ((percentageOfHp - 0.3f) / 0.3f);
        }
        else
            screenTint.a = 1;

        bloodyScreen.color = screenTint;
    }
}
