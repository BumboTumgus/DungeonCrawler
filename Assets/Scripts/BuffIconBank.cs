using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffIconBank : MonoBehaviour
{
    public static BuffIconBank instance;

    public Color[] buffColors;
    public Sprite[] buffIcons;

    // Start is called before the first frame update
    void Start()
    {
        if (!instance)
            instance = this;
        else
            Destroy(this);
    }

}
