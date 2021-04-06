using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBank : MonoBehaviour
{
    public Sprite[] skillIcons;
    public Color[] skillRarityBorderColors;
    public Color[] skillIconElementColors;
    public Color[] skillBackgroundColors;

    // USed to return a skill.
    public void SetSkill(SkillsManager.SkillNames skillName, Skill skillToSet)
    {
        skillToSet.skillName = skillName;

        switch (skillName)
        {
            case SkillsManager.SkillNames.SweepingBlow:
                skillToSet.targetCooldown = 5f;
                skillToSet.skillIcon = skillIcons[0];
                skillToSet.skillIconColor = skillIconElementColors[0];
                skillToSet.rarityBorderColor = skillRarityBorderColors[0];
                skillToSet.skillBackgroundColor = skillBackgroundColors[0];
                break;
            case SkillsManager.SkillNames.Rapislash:
                skillToSet.targetCooldown = 9f;
                skillToSet.skillIcon = skillIcons[1];
                skillToSet.skillIconColor = skillIconElementColors[0];
                skillToSet.rarityBorderColor = skillRarityBorderColors[0];
                skillToSet.skillBackgroundColor = skillBackgroundColors[0];
                break;
            case SkillsManager.SkillNames.SkywardSlash:
                skillToSet.targetCooldown = 12f;
                skillToSet.skillIcon = skillIcons[2];
                skillToSet.skillIconColor = skillIconElementColors[0];
                skillToSet.rarityBorderColor = skillRarityBorderColors[0];
                skillToSet.skillBackgroundColor = skillBackgroundColors[0];
                break;
            case SkillsManager.SkillNames.BladeVolley:
                skillToSet.targetCooldown = 6f;
                skillToSet.skillIcon = skillIcons[3];
                skillToSet.skillIconColor = skillIconElementColors[0];
                skillToSet.rarityBorderColor = skillRarityBorderColors[0];
                skillToSet.skillBackgroundColor = skillBackgroundColors[0];
                break;
            case SkillsManager.SkillNames.BlinkStrike:
                skillToSet.targetCooldown = 10f;
                skillToSet.skillIcon = skillIcons[4];
                skillToSet.skillIconColor = skillIconElementColors[0];
                skillToSet.rarityBorderColor = skillRarityBorderColors[0];
                skillToSet.skillBackgroundColor = skillBackgroundColors[0];
                break;
            case SkillsManager.SkillNames.TremorStab:
                skillToSet.targetCooldown = 16f;
                skillToSet.skillIcon = skillIcons[5];
                skillToSet.skillIconColor = skillIconElementColors[0];
                skillToSet.rarityBorderColor = skillRarityBorderColors[1];
                skillToSet.skillBackgroundColor = skillBackgroundColors[0];
                break;
            case SkillsManager.SkillNames.LeapStrike:
                skillToSet.targetCooldown = 10f;
                skillToSet.skillIcon = skillIcons[6];
                skillToSet.skillIconColor = skillIconElementColors[0];
                skillToSet.rarityBorderColor = skillRarityBorderColors[1];
                skillToSet.skillBackgroundColor = skillBackgroundColors[0];
                break;
            case SkillsManager.SkillNames.Takedown:
                skillToSet.targetCooldown = 20f;
                skillToSet.skillIcon = skillIcons[7];
                skillToSet.skillIconColor = skillIconElementColors[0];
                skillToSet.rarityBorderColor = skillRarityBorderColors[1];
                skillToSet.skillBackgroundColor = skillBackgroundColors[0];
                break;
            case SkillsManager.SkillNames.Impale:
                skillToSet.targetCooldown = 11f;
                skillToSet.skillIcon = skillIcons[8];
                skillToSet.skillIconColor = skillIconElementColors[0];
                skillToSet.rarityBorderColor = skillRarityBorderColors[1];
                skillToSet.skillBackgroundColor = skillBackgroundColors[0];
                break;
            case SkillsManager.SkillNames.Counter:
                skillToSet.targetCooldown = 13f;
                skillToSet.skillIcon = skillIcons[9];
                skillToSet.skillIconColor = skillIconElementColors[0];
                skillToSet.rarityBorderColor = skillRarityBorderColors[2];
                skillToSet.skillBackgroundColor = skillBackgroundColors[0];
                break;
            case SkillsManager.SkillNames.SeveringStrike:
                skillToSet.targetCooldown = 5f;
                skillToSet.skillIcon = skillIcons[10];
                skillToSet.skillIconColor = skillIconElementColors[0];
                skillToSet.rarityBorderColor = skillRarityBorderColors[2];
                skillToSet.skillBackgroundColor = skillBackgroundColors[0];
                break;
            case SkillsManager.SkillNames.Whirlwind:
                skillToSet.targetCooldown = 16f;
                skillToSet.skillIcon = skillIcons[11];
                skillToSet.skillIconColor = skillIconElementColors[0];
                skillToSet.rarityBorderColor = skillRarityBorderColors[2];
                skillToSet.skillBackgroundColor = skillBackgroundColors[0];
                break;
            case SkillsManager.SkillNames.ShatteredEarth:
                skillToSet.targetCooldown = 20f;
                skillToSet.skillIcon = skillIcons[12];
                skillToSet.skillIconColor = skillIconElementColors[0];
                skillToSet.rarityBorderColor = skillRarityBorderColors[3];
                skillToSet.skillBackgroundColor = skillBackgroundColors[0];
                break;
            case SkillsManager.SkillNames.FallingSword:
                skillToSet.targetCooldown = 25f;
                skillToSet.skillIcon = skillIcons[13];
                skillToSet.skillIconColor = skillIconElementColors[0];
                skillToSet.rarityBorderColor = skillRarityBorderColors[3];
                skillToSet.skillBackgroundColor = skillBackgroundColors[0];
                break;
            case SkillsManager.SkillNames.SenateSlash:
                skillToSet.targetCooldown = 16f;
                skillToSet.skillIcon = skillIcons[14];
                skillToSet.skillIconColor = skillIconElementColors[0];
                skillToSet.rarityBorderColor = skillRarityBorderColors[4];
                skillToSet.skillBackgroundColor = skillBackgroundColors[0];
                break;



            case SkillsManager.SkillNames.Firebolt:
                skillToSet.targetCooldown = 5f;
                skillToSet.skillIcon = skillIcons[15];
                skillToSet.skillIconColor = skillIconElementColors[1];
                skillToSet.rarityBorderColor = skillRarityBorderColors[0];
                skillToSet.skillBackgroundColor = skillBackgroundColors[1];
                break;
            case SkillsManager.SkillNames.Ignition:
                skillToSet.targetCooldown = 14f;
                skillToSet.skillIcon = skillIcons[16];
                skillToSet.skillIconColor = skillIconElementColors[1];
                skillToSet.rarityBorderColor = skillRarityBorderColors[0];
                skillToSet.skillBackgroundColor = skillBackgroundColors[1];
                break;
            case SkillsManager.SkillNames.EmboldeningEmbers:
                skillToSet.targetCooldown = 30f;
                skillToSet.skillIcon = skillIcons[17];
                skillToSet.skillIconColor = skillIconElementColors[1];
                skillToSet.rarityBorderColor = skillRarityBorderColors[0];
                skillToSet.skillBackgroundColor = skillBackgroundColors[1];
                break;
            case SkillsManager.SkillNames.Firebeads:
                skillToSet.targetCooldown = 15f;
                skillToSet.skillIcon = skillIcons[18];
                skillToSet.skillIconColor = skillIconElementColors[1];
                skillToSet.rarityBorderColor = skillRarityBorderColors[0];
                skillToSet.skillBackgroundColor = skillBackgroundColors[1];
                break;
            case SkillsManager.SkillNames.HeatPulse:
                skillToSet.targetCooldown = 11f;
                skillToSet.skillIcon = skillIcons[19];
                skillToSet.skillIconColor = skillIconElementColors[1];
                skillToSet.rarityBorderColor = skillRarityBorderColors[0];
                skillToSet.skillBackgroundColor = skillBackgroundColors[1];
                break;
            case SkillsManager.SkillNames.FlameStrike:
                skillToSet.targetCooldown = 15f;
                skillToSet.skillIcon = skillIcons[20];
                skillToSet.skillIconColor = skillIconElementColors[1];
                skillToSet.rarityBorderColor = skillRarityBorderColors[1];
                skillToSet.skillBackgroundColor = skillBackgroundColors[1];
                break;
            case SkillsManager.SkillNames.Flamewalker:
                skillToSet.targetCooldown = 30f;
                skillToSet.skillIcon = skillIcons[21];
                skillToSet.skillIconColor = skillIconElementColors[1];
                skillToSet.rarityBorderColor = skillRarityBorderColors[1];
                skillToSet.skillBackgroundColor = skillBackgroundColors[1];
                break;
            case SkillsManager.SkillNames.WitchPyre:
                skillToSet.targetCooldown = 11f;
                skillToSet.skillIcon = skillIcons[22];
                skillToSet.skillIconColor = skillIconElementColors[1];
                skillToSet.rarityBorderColor = skillRarityBorderColors[1];
                skillToSet.skillBackgroundColor = skillBackgroundColors[1];
                break;
            case SkillsManager.SkillNames.Combustion:
                skillToSet.targetCooldown = 6f;
                skillToSet.skillIcon = skillIcons[23];
                skillToSet.skillIconColor = skillIconElementColors[1];
                skillToSet.rarityBorderColor = skillRarityBorderColors[1];
                skillToSet.skillBackgroundColor = skillBackgroundColors[1];
                break;
            case SkillsManager.SkillNames.RingOfFire:
                skillToSet.targetCooldown = 40f;
                skillToSet.skillIcon = skillIcons[24];
                skillToSet.skillIconColor = skillIconElementColors[1];
                skillToSet.rarityBorderColor = skillRarityBorderColors[2];
                skillToSet.skillBackgroundColor = skillBackgroundColors[1];
                break;
            case SkillsManager.SkillNames.BlessingOfFlames:
                skillToSet.targetCooldown = 30f;
                skillToSet.skillIcon = skillIcons[25];
                skillToSet.skillIconColor = skillIconElementColors[1];
                skillToSet.rarityBorderColor = skillRarityBorderColors[2];
                skillToSet.skillBackgroundColor = skillBackgroundColors[1];
                break;
            case SkillsManager.SkillNames.Immolate:
                skillToSet.targetCooldown = 30f;
                skillToSet.skillIcon = skillIcons[26];
                skillToSet.skillIconColor = skillIconElementColors[1];
                skillToSet.rarityBorderColor = skillRarityBorderColors[2];
                skillToSet.skillBackgroundColor = skillBackgroundColors[1];
                break;
            case SkillsManager.SkillNames.Firestorm:
                skillToSet.targetCooldown = 36f;
                skillToSet.skillIcon = skillIcons[27];
                skillToSet.skillIconColor = skillIconElementColors[1];
                skillToSet.rarityBorderColor = skillRarityBorderColors[3];
                skillToSet.skillBackgroundColor = skillBackgroundColors[1];
                break;
            case SkillsManager.SkillNames.Fireweave:
                skillToSet.targetCooldown = 22f;
                skillToSet.skillIcon = skillIcons[28];
                skillToSet.skillIconColor = skillIconElementColors[1];
                skillToSet.rarityBorderColor = skillRarityBorderColors[3];
                skillToSet.skillBackgroundColor = skillBackgroundColors[1];
                break;
            case SkillsManager.SkillNames.Fireball:
                skillToSet.targetCooldown = 26f;
                skillToSet.skillIcon = skillIcons[29];
                skillToSet.skillIconColor = skillIconElementColors[1];
                skillToSet.rarityBorderColor = skillRarityBorderColors[4];
                skillToSet.skillBackgroundColor = skillBackgroundColors[1];
                break;



            case SkillsManager.SkillNames.IceSpike:
                skillToSet.targetCooldown = 6f;
                skillToSet.skillIcon = skillIcons[30];
                skillToSet.skillIconColor = skillIconElementColors[2];
                skillToSet.rarityBorderColor = skillRarityBorderColors[0];
                skillToSet.skillBackgroundColor = skillBackgroundColors[2];
                break;
            case SkillsManager.SkillNames.IceShards:
                skillToSet.targetCooldown = 10f;
                skillToSet.skillIcon = skillIcons[31];
                skillToSet.skillIconColor = skillIconElementColors[2];
                skillToSet.rarityBorderColor = skillRarityBorderColors[0];
                skillToSet.skillBackgroundColor = skillBackgroundColors[2];
                break;
            case SkillsManager.SkillNames.HarshWinds:
                skillToSet.targetCooldown = 30f;
                skillToSet.skillIcon = skillIcons[32];
                skillToSet.skillIconColor = skillIconElementColors[2];
                skillToSet.rarityBorderColor = skillRarityBorderColors[0];
                skillToSet.skillBackgroundColor = skillBackgroundColors[2];
                break;
            case SkillsManager.SkillNames.IcicleBarrage:
                skillToSet.targetCooldown = 14f;
                skillToSet.skillIcon = skillIcons[33];
                skillToSet.skillIconColor = skillIconElementColors[2];
                skillToSet.rarityBorderColor = skillRarityBorderColors[0];
                skillToSet.skillBackgroundColor = skillBackgroundColors[2];
                break;
            case SkillsManager.SkillNames.FrozenBarricade:
                skillToSet.targetCooldown = 16f;
                skillToSet.skillIcon = skillIcons[34];
                skillToSet.skillIconColor = skillIconElementColors[2];
                skillToSet.rarityBorderColor = skillRarityBorderColors[0];
                skillToSet.skillBackgroundColor = skillBackgroundColors[2];
                break;
            case SkillsManager.SkillNames.IceJavelin:
                skillToSet.targetCooldown = 15f;
                skillToSet.skillIcon = skillIcons[35];
                skillToSet.skillIconColor = skillIconElementColors[2];
                skillToSet.rarityBorderColor = skillRarityBorderColors[1];
                skillToSet.skillBackgroundColor = skillBackgroundColors[2];
                break;
            case SkillsManager.SkillNames.Glacier:
                skillToSet.targetCooldown = 30;
                skillToSet.skillIcon = skillIcons[36];
                skillToSet.skillIconColor = skillIconElementColors[2];
                skillToSet.rarityBorderColor = skillRarityBorderColors[1];
                skillToSet.skillBackgroundColor = skillBackgroundColors[2];
                break;
            case SkillsManager.SkillNames.FrostNova:
                skillToSet.targetCooldown = 12f;
                skillToSet.skillIcon = skillIcons[37];
                skillToSet.skillIconColor = skillIconElementColors[2];
                skillToSet.rarityBorderColor = skillRarityBorderColors[1];
                skillToSet.skillBackgroundColor = skillBackgroundColors[2];
                break;
            case SkillsManager.SkillNames.FrostsKiss:
                skillToSet.targetCooldown = 15f;
                skillToSet.skillIcon = skillIcons[38];
                skillToSet.skillIconColor = skillIconElementColors[2];
                skillToSet.rarityBorderColor = skillRarityBorderColors[1];
                skillToSet.skillBackgroundColor = skillBackgroundColors[2];
                break;
            case SkillsManager.SkillNames.Blizzard:
                skillToSet.targetCooldown = 60f;
                skillToSet.skillIcon = skillIcons[39];
                skillToSet.skillIconColor = skillIconElementColors[2];
                skillToSet.rarityBorderColor = skillRarityBorderColors[2];
                skillToSet.skillBackgroundColor = skillBackgroundColors[2];
                break;
            case SkillsManager.SkillNames.IceArtillery:
                skillToSet.targetCooldown = 22f;
                skillToSet.skillIcon = skillIcons[40];
                skillToSet.skillIconColor = skillIconElementColors[2];
                skillToSet.rarityBorderColor = skillRarityBorderColors[2];
                skillToSet.skillBackgroundColor = skillBackgroundColors[2];
                break;
            case SkillsManager.SkillNames.RayOfIce:
                skillToSet.targetCooldown = 20f;
                skillToSet.skillIcon = skillIcons[41];
                skillToSet.skillIconColor = skillIconElementColors[2];
                skillToSet.rarityBorderColor = skillRarityBorderColors[2];
                skillToSet.skillBackgroundColor = skillBackgroundColors[2];
                break;
            case SkillsManager.SkillNames.IceArmor:
                skillToSet.targetCooldown = 30f;
                skillToSet.skillIcon = skillIcons[42];
                skillToSet.skillIconColor = skillIconElementColors[2];
                skillToSet.rarityBorderColor = skillRarityBorderColors[3];
                skillToSet.skillBackgroundColor = skillBackgroundColors[2];
                break;
            case SkillsManager.SkillNames.AbsoluteZero:
                skillToSet.targetCooldown = 45f;
                skillToSet.skillIcon = skillIcons[43];
                skillToSet.skillIconColor = skillIconElementColors[2];
                skillToSet.rarityBorderColor = skillRarityBorderColors[3];
                skillToSet.skillBackgroundColor = skillBackgroundColors[2];
                break;
            case SkillsManager.SkillNames.SpellMirror:
                skillToSet.targetCooldown = 40f;
                skillToSet.skillIcon = skillIcons[44];
                skillToSet.skillIconColor = skillIconElementColors[2];
                skillToSet.rarityBorderColor = skillRarityBorderColors[4];
                skillToSet.skillBackgroundColor = skillBackgroundColors[2];
                break;



            case SkillsManager.SkillNames.EarthernSpear:
                skillToSet.targetCooldown = 6f;
                skillToSet.skillIcon = skillIcons[45];
                skillToSet.skillIconColor = skillIconElementColors[6];
                skillToSet.rarityBorderColor = skillRarityBorderColors[0];
                skillToSet.skillBackgroundColor = skillBackgroundColors[6];
                break;
            case SkillsManager.SkillNames.EarthernUrchin:
                skillToSet.targetCooldown = 10f;
                skillToSet.skillIcon = skillIcons[46];
                skillToSet.skillIconColor = skillIconElementColors[6];
                skillToSet.rarityBorderColor = skillRarityBorderColors[0];
                skillToSet.skillBackgroundColor = skillBackgroundColors[6];
                break;
            case SkillsManager.SkillNames.IdolOfTremors:
                skillToSet.targetCooldown = 10f;
                skillToSet.skillIcon = skillIcons[47];
                skillToSet.skillIconColor = skillIconElementColors[6];
                skillToSet.rarityBorderColor = skillRarityBorderColors[0];
                skillToSet.skillBackgroundColor = skillBackgroundColors[6];
                break;
            case SkillsManager.SkillNames.BoulderFist:
                skillToSet.targetCooldown = 8f;
                skillToSet.skillIcon = skillIcons[48];
                skillToSet.skillIconColor = skillIconElementColors[6];
                skillToSet.rarityBorderColor = skillRarityBorderColors[0];
                skillToSet.skillBackgroundColor = skillBackgroundColors[6];
                break;
            case SkillsManager.SkillNames.StoneStrike:
                skillToSet.targetCooldown = 5f;
                skillToSet.skillIcon = skillIcons[49];
                skillToSet.skillIconColor = skillIconElementColors[6];
                skillToSet.rarityBorderColor = skillRarityBorderColors[0];
                skillToSet.skillBackgroundColor = skillBackgroundColors[6];
                break;
            case SkillsManager.SkillNames.EarthernPlateau:
                skillToSet.targetCooldown = 20f;
                skillToSet.skillIcon = skillIcons[50];
                skillToSet.skillIconColor = skillIconElementColors[6];
                skillToSet.rarityBorderColor = skillRarityBorderColors[1];
                skillToSet.skillBackgroundColor = skillBackgroundColors[6];
                break;
            case SkillsManager.SkillNames.GiantStrength:
                skillToSet.targetCooldown = 30f;
                skillToSet.skillIcon = skillIcons[51];
                skillToSet.skillIconColor = skillIconElementColors[6];
                skillToSet.rarityBorderColor = skillRarityBorderColors[1];
                skillToSet.skillBackgroundColor = skillBackgroundColors[6];
                break;
            case SkillsManager.SkillNames.RockShot:
                skillToSet.targetCooldown = 9f;
                skillToSet.skillIcon = skillIcons[52];
                skillToSet.skillIconColor = skillIconElementColors[6];
                skillToSet.rarityBorderColor = skillRarityBorderColors[1];
                skillToSet.skillBackgroundColor = skillBackgroundColors[6];
                break;
            case SkillsManager.SkillNames.StalagmiteSmash:
                skillToSet.targetCooldown = 14f;
                skillToSet.skillIcon = skillIcons[53];
                skillToSet.skillIconColor = skillIconElementColors[6];
                skillToSet.rarityBorderColor = skillRarityBorderColors[1];
                skillToSet.skillBackgroundColor = skillBackgroundColors[6];
                break;
            case SkillsManager.SkillNames.UnstableEarth:
                skillToSet.targetCooldown = 20f;
                skillToSet.skillIcon = skillIcons[54];
                skillToSet.skillIconColor = skillIconElementColors[6];
                skillToSet.rarityBorderColor = skillRarityBorderColors[2];
                skillToSet.skillBackgroundColor = skillBackgroundColors[6];
                break;
            case SkillsManager.SkillNames.Tremorfall:
                skillToSet.targetCooldown = 17f;
                skillToSet.skillIcon = skillIcons[55];
                skillToSet.skillIconColor = skillIconElementColors[6];
                skillToSet.rarityBorderColor = skillRarityBorderColors[2];
                skillToSet.skillBackgroundColor = skillBackgroundColors[6];
                break;
            case SkillsManager.SkillNames.GaiasCyclone:
                skillToSet.targetCooldown = 33f;
                skillToSet.skillIcon = skillIcons[56];
                skillToSet.skillIconColor = skillIconElementColors[6];
                skillToSet.rarityBorderColor = skillRarityBorderColors[2];
                skillToSet.skillBackgroundColor = skillBackgroundColors[6];
                break;
            case SkillsManager.SkillNames.CaveIn:
                skillToSet.targetCooldown = 16f;
                skillToSet.skillIcon = skillIcons[57];
                skillToSet.skillIconColor = skillIconElementColors[6];
                skillToSet.rarityBorderColor = skillRarityBorderColors[3];
                skillToSet.skillBackgroundColor = skillBackgroundColors[6];
                break;
            case SkillsManager.SkillNames.StonePrison:
                skillToSet.targetCooldown = 28f;
                skillToSet.skillIcon = skillIcons[58];
                skillToSet.skillIconColor = skillIconElementColors[6];
                skillToSet.rarityBorderColor = skillRarityBorderColors[3];
                skillToSet.skillBackgroundColor = skillBackgroundColors[6];
                break;
            case SkillsManager.SkillNames.Earthquake:
                skillToSet.targetCooldown = 60f;
                skillToSet.skillIcon = skillIcons[59];
                skillToSet.skillIconColor = skillIconElementColors[6];
                skillToSet.rarityBorderColor = skillRarityBorderColors[4];
                skillToSet.skillBackgroundColor = skillBackgroundColors[6];
                break;




            case SkillsManager.SkillNames.Airgust:
                skillToSet.targetCooldown = 6f;
                skillToSet.skillIcon = skillIcons[60];
                skillToSet.skillIconColor = skillIconElementColors[5];
                skillToSet.rarityBorderColor = skillRarityBorderColors[0];
                skillToSet.skillBackgroundColor = skillBackgroundColors[5];
                break;
            case SkillsManager.SkillNames.SecondWind:
                skillToSet.targetCooldown = 10f;
                skillToSet.skillIcon = skillIcons[61];
                skillToSet.skillIconColor = skillIconElementColors[5];
                skillToSet.rarityBorderColor = skillRarityBorderColors[0];
                skillToSet.skillBackgroundColor = skillBackgroundColors[5];
                break;
            case SkillsManager.SkillNames.Airblade:
                skillToSet.targetCooldown = 5f;
                skillToSet.skillIcon = skillIcons[62];
                skillToSet.skillIconColor = skillIconElementColors[5];
                skillToSet.rarityBorderColor = skillRarityBorderColors[0];
                skillToSet.skillBackgroundColor = skillBackgroundColors[5];
                break;
            case SkillsManager.SkillNames.Aeroslash:
                skillToSet.targetCooldown = 12f;
                skillToSet.skillIcon = skillIcons[63];
                skillToSet.skillIconColor = skillIconElementColors[5];
                skillToSet.rarityBorderColor = skillRarityBorderColors[0];
                skillToSet.skillBackgroundColor = skillBackgroundColors[5];
                break;
            case SkillsManager.SkillNames.Aeroburst:
                skillToSet.targetCooldown = 11f;
                skillToSet.skillIcon = skillIcons[64];
                skillToSet.skillIconColor = skillIconElementColors[5];
                skillToSet.rarityBorderColor = skillRarityBorderColors[0];
                skillToSet.skillBackgroundColor = skillBackgroundColors[5];
                break;
            case SkillsManager.SkillNames.WrathOfTheWind:
                skillToSet.targetCooldown = 30f;
                skillToSet.skillIcon = skillIcons[65];
                skillToSet.skillIconColor = skillIconElementColors[5];
                skillToSet.rarityBorderColor = skillRarityBorderColors[1];
                skillToSet.skillBackgroundColor = skillBackgroundColors[5];
                break;
            case SkillsManager.SkillNames.OrbOfShredding:
                skillToSet.targetCooldown = 16f;
                skillToSet.skillIcon = skillIcons[66];
                skillToSet.skillIconColor = skillIconElementColors[5];
                skillToSet.rarityBorderColor = skillRarityBorderColors[1];
                skillToSet.skillBackgroundColor = skillBackgroundColors[5];
                break;
            case SkillsManager.SkillNames.Multislash:
                skillToSet.targetCooldown = 12f;
                skillToSet.skillIcon = skillIcons[67];
                skillToSet.skillIconColor = skillIconElementColors[5];
                skillToSet.rarityBorderColor = skillRarityBorderColors[1];
                skillToSet.skillBackgroundColor = skillBackgroundColors[5];
                break;
            case SkillsManager.SkillNames.Aerolaunch:
                skillToSet.targetCooldown = 15f;
                skillToSet.skillIcon = skillIcons[68];
                skillToSet.skillIconColor = skillIconElementColors[5];
                skillToSet.rarityBorderColor = skillRarityBorderColors[1];
                skillToSet.skillBackgroundColor = skillBackgroundColors[5];
                break;
            case SkillsManager.SkillNames.WhirlwindSlash:
                skillToSet.targetCooldown = 20f;
                skillToSet.skillIcon = skillIcons[69];
                skillToSet.skillIconColor = skillIconElementColors[5];
                skillToSet.rarityBorderColor = skillRarityBorderColors[2];
                skillToSet.skillBackgroundColor = skillBackgroundColors[5];
                break;
            case SkillsManager.SkillNames.Aerobarrage:
                skillToSet.targetCooldown = 18f;
                skillToSet.skillIcon = skillIcons[70];
                skillToSet.skillIconColor = skillIconElementColors[5];
                skillToSet.rarityBorderColor = skillRarityBorderColors[2];
                skillToSet.skillBackgroundColor = skillBackgroundColors[5];
                break;
            case SkillsManager.SkillNames.PressureDrop:
                skillToSet.targetCooldown = 24f;
                skillToSet.skillIcon = skillIcons[71];
                skillToSet.skillIconColor = skillIconElementColors[5];
                skillToSet.rarityBorderColor = skillRarityBorderColors[2];
                skillToSet.skillBackgroundColor = skillBackgroundColors[5];
                break;
            case SkillsManager.SkillNames.TwinTwisters:
                skillToSet.targetCooldown = 28f;
                skillToSet.skillIcon = skillIcons[72];
                skillToSet.skillIconColor = skillIconElementColors[5];
                skillToSet.rarityBorderColor = skillRarityBorderColors[3];
                skillToSet.skillBackgroundColor = skillBackgroundColors[5];
                break;
            case SkillsManager.SkillNames.Vortex:
                skillToSet.targetCooldown = 45f;
                skillToSet.skillIcon = skillIcons[73];
                skillToSet.skillIconColor = skillIconElementColors[5];
                skillToSet.rarityBorderColor = skillRarityBorderColors[3];
                skillToSet.skillBackgroundColor = skillBackgroundColors[5];
                break;
            case SkillsManager.SkillNames.GroundZero:
                skillToSet.targetCooldown = 45f;
                skillToSet.skillIcon = skillIcons[74];
                skillToSet.skillIconColor = skillIconElementColors[5];
                skillToSet.rarityBorderColor = skillRarityBorderColors[4];
                skillToSet.skillBackgroundColor = skillBackgroundColors[5];
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



            case SkillsManager.SkillNames.IceSpike:
                cooldown = 6f;
                break;
            case SkillsManager.SkillNames.IceShards:
                cooldown = 10f;
                break;
            case SkillsManager.SkillNames.HarshWinds:
                cooldown = 30f;
                break;
            case SkillsManager.SkillNames.IcicleBarrage:
                cooldown = 14f;
                break;
            case SkillsManager.SkillNames.FrozenBarricade:
                cooldown = 16f;
                break;
            case SkillsManager.SkillNames.IceJavelin:
                cooldown = 15f;
                break;
            case SkillsManager.SkillNames.Glacier:
                cooldown = 30f;
                break;
            case SkillsManager.SkillNames.FrostNova:
                cooldown = 12f;
                break;
            case SkillsManager.SkillNames.FrostsKiss:
                cooldown = 15f;
                break;
            case SkillsManager.SkillNames.Blizzard:
                cooldown = 60f;
                break;
            case SkillsManager.SkillNames.IceArtillery:
                cooldown = 22f;
                break;
            case SkillsManager.SkillNames.RayOfIce:
                cooldown = 20f;
                break;
            case SkillsManager.SkillNames.IceArmor:
                cooldown = 30f;
                break;
            case SkillsManager.SkillNames.AbsoluteZero:
                cooldown = 45f;
                break;
            case SkillsManager.SkillNames.SpellMirror:
                cooldown = 40f;
                break;



            case SkillsManager.SkillNames.EarthernSpear:
                cooldown = 6f;
                break;
            case SkillsManager.SkillNames.EarthernUrchin:
                cooldown = 10f;
                break;
            case SkillsManager.SkillNames.IdolOfTremors:
                cooldown = 10f;
                break;
            case SkillsManager.SkillNames.BoulderFist:
                cooldown = 8f;
                break;
            case SkillsManager.SkillNames.StoneStrike:
                cooldown = 5f;
                break;
            case SkillsManager.SkillNames.EarthernPlateau:
                cooldown = 20f;
                break;
            case SkillsManager.SkillNames.GiantStrength:
                cooldown = 30f;
                break;
            case SkillsManager.SkillNames.RockShot:
                cooldown = 9f;
                break;
            case SkillsManager.SkillNames.StalagmiteSmash:
                cooldown = 14f;
                break;
            case SkillsManager.SkillNames.UnstableEarth:
                cooldown = 20f;
                break;
            case SkillsManager.SkillNames.Tremorfall:
                cooldown = 17f;
                break;
            case SkillsManager.SkillNames.GaiasCyclone:
                cooldown = 33f;
                break;
            case SkillsManager.SkillNames.CaveIn:
                cooldown = 16f;
                break;
            case SkillsManager.SkillNames.StonePrison:
                cooldown = 28f;
                break;
            case SkillsManager.SkillNames.Earthquake:
                cooldown = 60f;
                break;




            case SkillsManager.SkillNames.Airgust:
                cooldown = 6f;
                break;
            case SkillsManager.SkillNames.SecondWind:
                cooldown = 10f;
                break;
            case SkillsManager.SkillNames.Airblade:
                cooldown = 5f;
                break;
            case SkillsManager.SkillNames.Aeroslash:
                cooldown = 12f;
                break;
            case SkillsManager.SkillNames.Aeroburst:
                cooldown = 11f;
                break;
            case SkillsManager.SkillNames.WrathOfTheWind:
                cooldown = 30f;
                break;
            case SkillsManager.SkillNames.OrbOfShredding:
                cooldown = 16f;
                break;
            case SkillsManager.SkillNames.Multislash:
                cooldown = 12f;
                break;
            case SkillsManager.SkillNames.Aerolaunch:
                cooldown = 15f;
                break;
            case SkillsManager.SkillNames.WhirlwindSlash:
                cooldown = 20f;
                break;
            case SkillsManager.SkillNames.Aerobarrage:
                cooldown = 18f;
                break;
            case SkillsManager.SkillNames.PressureDrop:
                cooldown = 24f;
                break;
            case SkillsManager.SkillNames.TwinTwisters:
                cooldown = 28f;
                break;
            case SkillsManager.SkillNames.Vortex:
                cooldown = 45f;
                break;
            case SkillsManager.SkillNames.GroundZero:
                cooldown = 45f;
                break;
            default:
                break;
        }
        return cooldown;
    }

}
