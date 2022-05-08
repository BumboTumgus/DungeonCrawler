using UnityEngine;
using UnityEngine.UI;

public class ThreatLevelUIManager : MonoBehaviour
{
    [SerializeField] Sprite[] threatLevelSprites;
    [SerializeField] string[] threatLevelStrings;
    [SerializeField] Color[] threatLevelColors;
    [SerializeField] Image threatLevelImage;
    [SerializeField] Text threatLevelText;

    private int currentThreatLevel = -1;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        SetThreatLevel();
    }


    public void SetThreatLevel()
    {
        Debug.Log("Threat Level Was Set here");
        currentThreatLevel++;
        if (currentThreatLevel >= threatLevelSprites.Length)
            currentThreatLevel = threatLevelSprites.Length - 1;

        anim.SetTrigger("ThreatLevelChange");
    }

    public void ChangeAssetsToThreatLevel()
    {
        threatLevelImage.sprite = threatLevelSprites[currentThreatLevel];
        threatLevelText.text = threatLevelStrings[currentThreatLevel];
        threatLevelText.color = threatLevelColors[currentThreatLevel];
    }
}
