using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectivePanelController : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private Text titleText;
    [SerializeField] private Text descText;

    /// <summary>
    /// Used to show and setup the objective panel initially.
    /// </summary>
    /// <param name="titleText"></param> The title text (yellow)
    /// <param name="descText"></param> The desc text (white).
    public void SetupObjectivePanel(string TitleText, string DescText)
    {
        titleText.text = TitleText;
        descText.text = DescText;
        anim.SetTrigger("ShowObjective");
    }

    /// <summary>
    /// Used to solely update the description text, useful for 
    /// </summary>
    /// <param name="descText"></param>
    public void UpdateObjectiveDecription(string DescText)
    {
        descText.text = DescText;
    }
}
