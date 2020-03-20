using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarResizer : MonoBehaviour
{
    public List<Image> barsToSize = new List<Image>();
    public Text textValue;
    public GameObject rightBarCap;

    // Used to resize the bars to a certain length
    public void ResizeBar(float value)
    {
        foreach (Image bar in barsToSize)
            bar.rectTransform.sizeDelta = new Vector2(value, 30);

        textValue.transform.localPosition = new Vector3((value / 2) -150, textValue.transform.localPosition.y, 0);
        rightBarCap.transform.localPosition = new Vector3(value - 143, rightBarCap.transform.localPosition.y, 0);
    }
}
