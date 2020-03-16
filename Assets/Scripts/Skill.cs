using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public SkillsManager.SkillNames skillName;
    public Sprite skillIcon;

    public float currentCooldown = 0;
    public float targetCooldown = 10;
    public bool skillReady = false;

    public BarManager connectedBar;

    public int skillIndex = 0;

    public SkillsManager myManager;


    // Update is called once per frame
    void Update()
    {
        if (!skillReady)
        {
            currentCooldown += Time.deltaTime;
            connectedBar.SetValue(targetCooldown - currentCooldown);


            if (currentCooldown > targetCooldown)
            {
                currentCooldown = targetCooldown;
                skillReady = true;
                connectedBar.gameObject.SetActive(false);
            }
        }
    }

    // Called When the skill is going to be used.
    public void UseSkill()
    {
        if (skillReady)
        {
            Debug.Log("The skill " + skillName + " has been used");
            currentCooldown = 0;
            skillReady = false;
            connectedBar.gameObject.SetActive(true);
        }
        // else
            // Debug.Log("Skill not ready");
    }
}
