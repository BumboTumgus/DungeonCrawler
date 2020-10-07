using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScalingIconInitializer : MonoBehaviour
{
    public Sprite[] scalingSprites;
    public Color[] scalingColors;

    public Image scalingImage;

    // Called after we instantiated by the UiItemPopUpresizer to switch us to the proper attribute.
    public void SetScalingImage(int index)
    {
        scalingImage.sprite = scalingSprites[index];
        scalingImage.color = scalingColors[index];
    }
}
