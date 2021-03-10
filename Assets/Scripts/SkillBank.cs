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
            case SkillsManager.SkillNames.SweepingBlow:
                skillToSet.targetCooldown = 5f;
                skillToSet.skillIcon = skillIcons[0];
                break;
            case SkillsManager.SkillNames.Rapislash:
                skillToSet.targetCooldown = 9f;
                skillToSet.skillIcon = skillIcons[1];
                break;
            case SkillsManager.SkillNames.SkywardSlash:
                skillToSet.targetCooldown = 12f;
                skillToSet.skillIcon = skillIcons[2];
                break;
            case SkillsManager.SkillNames.BladeVolley:
                skillToSet.targetCooldown = 6f;
                skillToSet.skillIcon = skillIcons[3];
                break;
            case SkillsManager.SkillNames.BlinkStrike:
                skillToSet.targetCooldown = 10f;
                skillToSet.skillIcon = skillIcons[4];
                break;
            case SkillsManager.SkillNames.TremorStab:
                skillToSet.targetCooldown = 16f;
                skillToSet.skillIcon = skillIcons[5];
                break;
            case SkillsManager.SkillNames.LeapStrike:
                skillToSet.targetCooldown = 10f;
                skillToSet.skillIcon = skillIcons[6];
                break;
            case SkillsManager.SkillNames.Takedown:
                skillToSet.targetCooldown = 20f;
                skillToSet.skillIcon = skillIcons[7];
                break;
            case SkillsManager.SkillNames.Impale:
                skillToSet.targetCooldown = 11f;
                skillToSet.skillIcon = skillIcons[8];
                break;
            case SkillsManager.SkillNames.Counter:
                skillToSet.targetCooldown = 13f;
                skillToSet.skillIcon = skillIcons[9];
                break;
            case SkillsManager.SkillNames.SeveringStrike:
                skillToSet.targetCooldown = 5f;
                skillToSet.skillIcon = skillIcons[10];
                break;
            case SkillsManager.SkillNames.Whirlwind:
                skillToSet.targetCooldown = 16f;
                skillToSet.skillIcon = skillIcons[11];
                break;
            case SkillsManager.SkillNames.ShatteredEarth:
                skillToSet.targetCooldown = 20f;
                skillToSet.skillIcon = skillIcons[12];
                break;
            case SkillsManager.SkillNames.FallingSword:
                skillToSet.targetCooldown = 25f;
                skillToSet.skillIcon = skillIcons[13];
                break;
            case SkillsManager.SkillNames.SenateSlash:
                skillToSet.targetCooldown = 16f;
                skillToSet.skillIcon = skillIcons[14];
                break;
            case SkillsManager.SkillNames.Firebolt:
                skillToSet.targetCooldown = 5f;
                skillToSet.skillIcon = skillIcons[15];
                break;
            case SkillsManager.SkillNames.Ignition:
                skillToSet.targetCooldown = 14f;
                skillToSet.skillIcon = skillIcons[16];
                break;
            case SkillsManager.SkillNames.EmboldeningEmbers:
                skillToSet.targetCooldown = 30f;
                skillToSet.skillIcon = skillIcons[17];
                break;
            case SkillsManager.SkillNames.Firebeads:
                skillToSet.targetCooldown = 15f;
                skillToSet.skillIcon = skillIcons[18];
                break;
            case SkillsManager.SkillNames.HeatPulse:
                skillToSet.targetCooldown = 11f;
                skillToSet.skillIcon = skillIcons[19];
                break;
            case SkillsManager.SkillNames.FlameStrike:
                skillToSet.targetCooldown = 15f;
                skillToSet.skillIcon = skillIcons[20];
                break;
            case SkillsManager.SkillNames.Flamewalker:
                skillToSet.targetCooldown = 30f;
                skillToSet.skillIcon = skillIcons[21];
                break;
            case SkillsManager.SkillNames.WitchPyre:
                skillToSet.targetCooldown = 11f;
                skillToSet.skillIcon = skillIcons[22];
                break;
            case SkillsManager.SkillNames.Combustion:
                skillToSet.targetCooldown = 6f;
                skillToSet.skillIcon = skillIcons[23];
                break;
            case SkillsManager.SkillNames.RingOfFire:
                skillToSet.targetCooldown = 40f;
                skillToSet.skillIcon = skillIcons[24];
                break;
            case SkillsManager.SkillNames.BlessingOfFlames:
                skillToSet.targetCooldown = 30f;
                skillToSet.skillIcon = skillIcons[25];
                break;
            case SkillsManager.SkillNames.Immolate:
                skillToSet.targetCooldown = 30f;
                skillToSet.skillIcon = skillIcons[26];
                break;
            case SkillsManager.SkillNames.Firestorm:
                skillToSet.targetCooldown = 36f;
                skillToSet.skillIcon = skillIcons[27];
                break;
            case SkillsManager.SkillNames.Fireweave:
                skillToSet.targetCooldown = 22f;
                skillToSet.skillIcon = skillIcons[28];
                break;
            case SkillsManager.SkillNames.Fireball:
                skillToSet.targetCooldown = 26f;
                skillToSet.skillIcon = skillIcons[29];
                break;
            //-----------------------------------------------------------------------------------------------------------------------------
            case SkillsManager.SkillNames.AspectOfRage:
                skillToSet.targetCooldown = 90f;
                skillToSet.skillIcon = skillIcons[4];
                break;
            case SkillsManager.SkillNames.Rampage:
                skillToSet.targetCooldown = 0f;
                skillToSet.passive = true;
                skillToSet.skillIcon = skillIcons[6];
                break;
            case SkillsManager.SkillNames.GiantStrength:
                skillToSet.targetCooldown = 30f;
                skillToSet.skillIcon = skillIcons[8];
                break;
            case SkillsManager.SkillNames.EarthernPlateau:
                skillToSet.targetCooldown = 26f;
                skillToSet.skillIcon = skillIcons[9];
                break;
            case SkillsManager.SkillNames.BoulderFist:
                skillToSet.targetCooldown = 6f;
                skillToSet.skillIcon = skillIcons[10];
                break;
            case SkillsManager.SkillNames.EarthernSpear:
                skillToSet.targetCooldown = 10f;
                skillToSet.skillIcon = skillIcons[11];
                break;
            case SkillsManager.SkillNames.CausticEdge:
                skillToSet.targetCooldown = 12;
                skillToSet.skillIcon = skillIcons[12];
                break;
            case SkillsManager.SkillNames.ToxicRipple:
                skillToSet.targetCooldown = 60f;
                skillToSet.skillIcon = skillIcons[13];
                break;
            case SkillsManager.SkillNames.KillerInstinct:
                skillToSet.targetCooldown = 30f;
                skillToSet.skillIcon = skillIcons[14];
                break;
            case SkillsManager.SkillNames.NaturePulse:
                skillToSet.targetCooldown = 3f;
                skillToSet.skillIcon = skillIcons[15];
                break;
            case SkillsManager.SkillNames.Revitalize:
                skillToSet.targetCooldown = 0f;
                skillToSet.passive = true;
                skillToSet.skillIcon = skillIcons[16];
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
            case SkillsManager.SkillNames.SweepingBlow:
                cooldown = 5f;
                break;
            case SkillsManager.SkillNames.Rapislash:
                cooldown = 9f;
                break;
            case SkillsManager.SkillNames.SkywardSlash:
                cooldown = 12f;
                break;
            case SkillsManager.SkillNames.BladeVolley:
                cooldown = 6f;
                break;
            case SkillsManager.SkillNames.BlinkStrike:
                cooldown = 10f;
                break;
            case SkillsManager.SkillNames.TremorStab:
                cooldown = 16f;
                break;
            case SkillsManager.SkillNames.LeapStrike:
                cooldown = 10f;
                break;
            case SkillsManager.SkillNames.Takedown:
                cooldown = 20f;
                break;
            case SkillsManager.SkillNames.Impale:
                cooldown = 11f;
                break;
            case SkillsManager.SkillNames.Counter:
                cooldown = 13f;
                break;
            case SkillsManager.SkillNames.SeveringStrike:
                cooldown = 5f;
                break;
            case SkillsManager.SkillNames.Whirlwind:
                cooldown = 16f;
                break;
            case SkillsManager.SkillNames.ShatteredEarth:
                cooldown = 20f;
                break;
            case SkillsManager.SkillNames.FallingSword:
                cooldown = 25f;
                break;
            case SkillsManager.SkillNames.SenateSlash:
                cooldown = 16f;
                break;
            case SkillsManager.SkillNames.Firebolt:
                cooldown = 5f;
                break;
            case SkillsManager.SkillNames.Ignition:
                cooldown = 14f;
                break;
            case SkillsManager.SkillNames.EmboldeningEmbers:
                cooldown = 30f;
                break;
            case SkillsManager.SkillNames.Firebeads:
                cooldown = 15f;
                break;
            case SkillsManager.SkillNames.HeatPulse:
                cooldown = 11f;
                break;
            case SkillsManager.SkillNames.FlameStrike:
                cooldown = 15f;
                break;
            case SkillsManager.SkillNames.Flamewalker:
                cooldown = 30f;
                break;
            case SkillsManager.SkillNames.WitchPyre:
                cooldown = 11f;
                break;
            case SkillsManager.SkillNames.Combustion:
                cooldown = 6f;
                break;
            case SkillsManager.SkillNames.RingOfFire:
                cooldown = 40f;
                break;
            case SkillsManager.SkillNames.BlessingOfFlames:
                cooldown = 30f;
                break;
            case SkillsManager.SkillNames.Immolate:
                cooldown = 30f;
                break;
            case SkillsManager.SkillNames.Firestorm:
                cooldown = 36f;
                break;
            case SkillsManager.SkillNames.Fireweave:
                cooldown = 22f;
                break;
            case SkillsManager.SkillNames.Fireball:
                cooldown = 26f;
                break;
            //---------------------------------------------------------------------------------------------------------------------------------------------------------
            case SkillsManager.SkillNames.AspectOfRage:
                cooldown = 70f;
                break;
            case SkillsManager.SkillNames.Rampage:
                cooldown = 0f;
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
            default:
                break;
        }
        return cooldown;
    }

}
