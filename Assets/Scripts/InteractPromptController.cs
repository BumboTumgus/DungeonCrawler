using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractPromptController : MonoBehaviour
{
    Text text;

    // Start is called before the first frame update
    void Awake()
    {
        text = GetComponent<Text>();
        text.text = "";
    }
    
    // Used to set the text to something
    public void SetText(string value)
    {
        text.text = value;
    }

    public void SetColor(Color color)
    {
        text.color = color;
    }
}
