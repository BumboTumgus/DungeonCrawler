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
                skillToSet.skillCost = 20f;
                skillToSet.skillIcon = skillIcons[0];
                break;
            case SkillsManager.SkillNames.EmboldeningEmbers:
                skillToSet.targetCooldown = 15f;
                skillToSet.skillCost = 25f;
                skillToSet.skillIcon = skillIcons[1];
                break;
            case SkillsManager.SkillNames.FlameStrike:
                skillToSet.targetCooldown = 40f;
                skillToSet.skillCost = 40f;
                skillToSet.skillIcon = skillIcons[2];
                break;
            case SkillsManager.SkillNames.SeveringStrike:
                skillToSet.targetCooldown = 10f;
                skillToSet.skillCost = 50f;
                skillToSet.skillIcon = skillIcons[3];
                break;
            case SkillsManager.SkillNames.AspectOfRage:
                skillToSet.targetCooldown = 90f;
                skillToSet.skillCost = 100f;
                skillToSet.skillIcon = skillIcons[4];
                break;
            case SkillsManager.SkillNames.BlessingOfFlames:
                skillToSet.targetCooldown = 70f;
                skillToSet.skillCost = 70f;
                skillToSet.skillIcon = skillIcons[5];
                break;
            case SkillsManager.SkillNames.Rampage:
                skillToSet.targetCooldown = 0f;
                skillToSet.skillCost = 0f;
                skillToSet.passive = true;
                skillToSet.skillIcon = skillIcons[6];
                break;
            case SkillsManager.SkillNames.ShatteredEarth:
                skillToSet.targetCooldown = 20f;
                skillToSet.skillCost = 65f;
                skillToSet.skillIcon = skillIcons[7];
                break;
            case SkillsManager.SkillNames.GiantStrength:
                skillToSet.targetCooldown = 30f;
                skillToSet.skillCost = 85f;
                skillToSet.skillIcon = skillIcons[8];
                break;
            case SkillsManager.SkillNames.EarthernPlateau:
                skillToSet.targetCooldown = 26f;
                skillToSet.skillCost = 115f;
                skillToSet.skillIcon = skillIcons[9];
                break;
            case SkillsManager.SkillNames.BoulderFist:
                skillToSet.targetCooldown = 6f;
                skillToSet.skillCost = 45f;
                skillToSet.skillIcon = skillIcons[10];
                break;
            case SkillsManager.SkillNames.EarthernSpear:
                skillToSet.targetCooldown = 10f;
                skillToSet.skillCost = 75f;
                skillToSet.skillIcon = skillIcons[11];
                break;
            case SkillsManager.SkillNames.CausticEdge:
                skillToSet.targetCooldown = 12;
                skillToSet.skillCost = 50;
                skillToSet.skillIcon = skillIcons[12];
                break;
            case SkillsManager.SkillNames.ToxicRipple:
                skillToSet.targetCooldown = 60f;
                skillToSet.skillCost = 150f;
                skillToSet.skillIcon = skillIcons[13];
                break;
            case SkillsManager.SkillNames.KillerInstinct:
                skillToSet.targetCooldown = 30f;
                skillToSet.skillCost = 80f;
                skillToSet.skillIcon = skillIcons[14];
                break;
            case SkillsManager.SkillNames.NaturePulse:
                skillToSet.targetCooldown = 3f;
                skillToSet.skillCost = 50f;
                skillToSet.skillIcon = skillIcons[15];
                break;
            case SkillsManager.SkillNames.Revitalize:
                skillToSet.targetCooldown = 0f;
                skillToSet.skillCost = 0f;
                skillToSet.passive = true;
                skillToSet.skillIcon = skillIcons[16];
                break;
            case SkillsManager.SkillNames.PoisonedMud:
                skillToSet.targetCooldown = 40f;
                skillToSet.skillCost = 60f;
                skillToSet.skillIcon = skillIcons[17];
                break;
            case SkillsManager.SkillNames.StrangleThorn:
                skillToSet.targetCooldown = 10f;
                skillToSet.skillCost = 40f;
                skillToSet.skillIcon = skillIcons[18];
                break;
            case SkillsManager.SkillNames.SoothingStone:
                skillToSet.targetCooldown = 80f;
                skillToSet.skillCost = 110f;
                skillToSet.skillIcon = skillIcons[19];
                break;
            case SkillsManager.SkillNames.ShearingCyclone:
                skillToSet.targetCooldown = 18f;
                skillToSet.skillCost = 100f;
                skillToSet.skillIcon = skillIcons[21];
                break;
            case SkillsManager.SkillNames.WindHarpoon:
                skillToSet.targetCooldown = 18f;
                skillToSet.skillCost = 60f;
                skillToSet.skillIcon = skillIcons[22];
                break;
            case SkillsManager.SkillNames.SplitswordStrikes:
                skillToSet.targetCooldown = 14f;
                skillToSet.skillCost = 60f;
                skillToSet.skillIcon = skillIcons[23];
                break;
            case SkillsManager.SkillNames.ThunderLance:
                skillToSet.targetCooldown = 8f;
                skillToSet.skillCost = 40f;
                skillToSet.skillIcon = skillIcons[24];
                break;
            case SkillsManager.SkillNames.LightningStorm:
                skillToSet.targetCooldown = 8f;
                skillToSet.skillCost = 40f;
                skillToSet.skillIcon = skillIcons[25];
                break;
            case SkillsManager.SkillNames.WrathOfTheRagingWinds:
                skillToSet.targetCooldown = 40f;
                skillToSet.skillCost = 100f;
                skillToSet.skillIcon = skillIcons[26];
                break;
            case SkillsManager.SkillNames.BladeBarrage:
                skillToSet.targetCooldown = 16f;
                skillToSet.skillCost = 90f;
                skillToSet.skillIcon = skillIcons[27];
                break;
            case SkillsManager.SkillNames.ViolentZephyr:
                skillToSet.targetCooldown = 40f;
                skillToSet.skillCost = 120f;
                skillToSet.skillIcon = skillIcons[28];
                break;
            case SkillsManager.SkillNames.Permafrost:
                skillToSet.targetCooldown = 26f;
                skillToSet.skillCost = 80f;
                skillToSet.skillIcon = skillIcons[29];
                break;
            case SkillsManager.SkillNames.IceBarrage:
                skillToSet.targetCooldown = 9f;
                skillToSet.skillCost = 35f;
                skillToSet.skillIcon = skillIcons[30];
                break;
            case SkillsManager.SkillNames.FrozenBarrier:
                skillToSet.targetCooldown = 30f;
                skillToSet.skillCost = 60f;
                skillToSet.skillIcon = skillIcons[31];
                break;
            case SkillsManager.SkillNames.SoothingStream:
                skillToSet.targetCooldown = 22f;
                skillToSet.skillCost = 75f;
                skillToSet.skillIcon = skillIcons[32];
                break;
            case SkillsManager.SkillNames.SwirlingVortex:
                skillToSet.targetCooldown = 75f;
                skillToSet.skillCost = 190f;
                skillToSet.skillIcon = skillIcons[33];
                break;
            default:
                break;
        }
    }
}
