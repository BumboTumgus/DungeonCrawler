using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBank : MonoBehaviour
{
    public Sprite[] skillIcons;

    // USed to return a skill.
    public void SetSkill(SkillsManager.SkillNames skillName, Skill skillToSet)
    {
        skillToSet.skillName = skillName;

        switch (skillName)
        {
            case SkillsManager.SkillNames.BlinkStrike:
                skillToSet.targetCooldown = 6f;
                skillToSet.skillIcon = skillIcons[0];
                break;
            case SkillsManager.SkillNames.EmboldeningEmbers:
                skillToSet.targetCooldown = 15f;
                skillToSet.skillIcon = skillIcons[1];
                break;
            case SkillsManager.SkillNames.FlameStrike:
                skillToSet.targetCooldown = 20f;
                skillToSet.skillIcon = skillIcons[2];
                break;
            case SkillsManager.SkillNames.SeveringStrike:
                skillToSet.targetCooldown = 10f;
                skillToSet.skillIcon = skillIcons[3];
                break;
            default:
                break;
        }
    }
}
