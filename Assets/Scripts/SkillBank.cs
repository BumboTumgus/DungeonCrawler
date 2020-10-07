﻿using System.Collections;
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

    public float GrabSkillCooldown(SkillsManager.SkillNames skillName)
    {
        float cooldown = 0f;
        switch (skillName)
        {
            case SkillsManager.SkillNames.BlinkStrike:
                cooldown = 6f;
                break;
            case SkillsManager.SkillNames.EmboldeningEmbers:
                cooldown = 15f;
                break;
            case SkillsManager.SkillNames.FlameStrike:
                cooldown = 40f;
                break;
            case SkillsManager.SkillNames.SeveringStrike:
                cooldown = 10f;
                break;
            case SkillsManager.SkillNames.ShatteredEarth:
                cooldown = 90f;
                break;
            case SkillsManager.SkillNames.AspectOfRage:
                cooldown = 70f;
                break;
            case SkillsManager.SkillNames.Rampage:
                cooldown = 0f;
                break;
            case SkillsManager.SkillNames.BlessingOfFlames:
                cooldown = 20f;
                break;
            case SkillsManager.SkillNames.GiantStrength:
                cooldown = 30f;
                break;
            case SkillsManager.SkillNames.EarthernPlateau:
                cooldown = 26f;
                break;
            case SkillsManager.SkillNames.BoulderFist:
                cooldown = 6f;
                break;
            case SkillsManager.SkillNames.EarthernSpear:
                cooldown = 10f;
                break;
            case SkillsManager.SkillNames.CausticEdge:
                cooldown = 12f;
                break;
            case SkillsManager.SkillNames.ToxicRipple:
                cooldown = 60f;
                break;
            case SkillsManager.SkillNames.KillerInstinct:
                cooldown = 30f;
                break;
            case SkillsManager.SkillNames.NaturePulse:
                cooldown = 3f;
                break;
            case SkillsManager.SkillNames.Revitalize:
                cooldown = 0f;
                break;
            case SkillsManager.SkillNames.PoisonedMud:
                cooldown = 40f;
                break;
            case SkillsManager.SkillNames.StrangleThorn:
                cooldown = 10f;
                break;
            case SkillsManager.SkillNames.SoothingStone:
                cooldown = 80f;
                break;
            case SkillsManager.SkillNames.Deadeye:
                cooldown = 0f;
                break;
            case SkillsManager.SkillNames.ShearingCyclone:
                cooldown = 18f;
                break;
            case SkillsManager.SkillNames.WindHarpoon:
                cooldown = 18f;
                break;
            case SkillsManager.SkillNames.SplitswordStrikes:
                cooldown = 14f;
                break;
            case SkillsManager.SkillNames.ThunderLance:
                cooldown = 8f;
                break;
            case SkillsManager.SkillNames.LightningStorm:
                cooldown = 8f;
                break;
            case SkillsManager.SkillNames.WrathOfTheRagingWinds:
                cooldown = 40f;
                break;
            case SkillsManager.SkillNames.BladeBarrage:
                cooldown = 16f;
                break;
            case SkillsManager.SkillNames.ViolentZephyr:
                cooldown = 40f;
                break;
            case SkillsManager.SkillNames.Permafrost:
                cooldown = 26f;
                break;
            case SkillsManager.SkillNames.IceBarrage:
                cooldown = 9f;
                break;
            case SkillsManager.SkillNames.FrozenBarrier:
                cooldown = 30f;
                break;
            case SkillsManager.SkillNames.SoothingStream:
                cooldown = 22f;
                break;
            case SkillsManager.SkillNames.SwirlingVortex:
                cooldown = 75f;
                break;
            default:
                break;
        }
        return cooldown;
    }

    public float GrabSkillCost(SkillsManager.SkillNames skillName)
    {
        float cost = 0f;
        switch (skillName)
        {
            case SkillsManager.SkillNames.BlinkStrike:
                cost = 20f;
                break;
            case SkillsManager.SkillNames.EmboldeningEmbers:
                cost = 25f;
                break;
            case SkillsManager.SkillNames.FlameStrike:
                cost = 40f;
                break;
            case SkillsManager.SkillNames.SeveringStrike:
                cost = 50f;
                break;
            case SkillsManager.SkillNames.ShatteredEarth:
                cost = 100f;
                break;
            case SkillsManager.SkillNames.AspectOfRage:
                cost = 70f;
                break;
            case SkillsManager.SkillNames.Rampage:
                cost = 0f;
                break;
            case SkillsManager.SkillNames.BlessingOfFlames:
                cost = 65f;
                break;
            case SkillsManager.SkillNames.GiantStrength:
                cost = 85f;
                break;
            case SkillsManager.SkillNames.EarthernPlateau:
                cost = 115f;
                break;
            case SkillsManager.SkillNames.BoulderFist:
                cost = 45f;
                break;
            case SkillsManager.SkillNames.EarthernSpear:
                cost = 75f;
                break;
            case SkillsManager.SkillNames.CausticEdge:
                cost = 50f;
                break;
            case SkillsManager.SkillNames.ToxicRipple:
                cost = 180f;
                break;
            case SkillsManager.SkillNames.KillerInstinct:
                cost = 80f;
                break;
            case SkillsManager.SkillNames.NaturePulse:
                cost = 50f;
                break;
            case SkillsManager.SkillNames.Revitalize:
                cost = 0f;
                break;
            case SkillsManager.SkillNames.PoisonedMud:
                cost = 160f;
                break;
            case SkillsManager.SkillNames.StrangleThorn:
                cost = 40f;
                break;
            case SkillsManager.SkillNames.SoothingStone:
                cost = 110f;
                break;
            case SkillsManager.SkillNames.Deadeye:
                cost = 0f;
                break;
            case SkillsManager.SkillNames.ShearingCyclone:
                cost = 100f;
                break;
            case SkillsManager.SkillNames.WindHarpoon:
                cost = 60f;
                break;
            case SkillsManager.SkillNames.SplitswordStrikes:
                cost = 60f;
                break;
            case SkillsManager.SkillNames.ThunderLance:
                cost = 40f;
                break;
            case SkillsManager.SkillNames.LightningStorm:
                cost = 40f;
                break;
            case SkillsManager.SkillNames.WrathOfTheRagingWinds:
                cost = 100f;
                break;
            case SkillsManager.SkillNames.BladeBarrage:
                cost = 90f;
                break;
            case SkillsManager.SkillNames.ViolentZephyr:
                cost = 120f;
                break;
            case SkillsManager.SkillNames.Permafrost:
                cost = 80f;
                break;
            case SkillsManager.SkillNames.IceBarrage:
                cost = 35f;
                break;
            case SkillsManager.SkillNames.FrozenBarrier:
                cost = 60f;
                break;
            case SkillsManager.SkillNames.SoothingStream:
                cost = 75f;
                break;
            case SkillsManager.SkillNames.SwirlingVortex:
                cost = 190f;
                break;
            default:
                break;
        }
        return cost;
    }
}
