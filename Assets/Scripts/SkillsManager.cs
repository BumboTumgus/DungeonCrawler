using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillsManager : MonoBehaviour
{
    public List<Skill> mySkills = new List<Skill>();
    public List<GameObject> mySkillBars = new List<GameObject>();
    public List<GameObject> skillProjectiles = new List<GameObject>();
    public List<SpellMirrorManager> spellMirrors = new List<SpellMirrorManager>();
    public List<Color> damageColors = new List<Color>();
    public GameObject skillIconPrefab;
    public Transform iconParent;
    public int maxSkillNumber = 3;
    public GameObject targetIndicatorCircle;
    public LayerMask targettingRayMask;
    public LayerMask targettingRayMaskHitEnemies;
    public InventoryUiManager inventory;

    public enum SkillNames { SweepingBlow, Rapislash, SkywardSlash, BladeVolley, BlinkStrike, TremorStab, LeapStrike, Takedown, Impale, Counter, SeveringStrike, Whirlwind, ShatteredEarth, FallingSword, SenateSlash, 
        Firebolt, Ignition, EmboldeningEmbers, Firebeads, HeatPulse, FlameStrike, Flamewalker, WitchPyre, Combustion, RingOfFire, BlessingOfFlames, Immolate, Firestorm, Fireweave, Fireball,
        IceSpike, IceShards, HarshWinds, IcicleBarrage, FrozenBarricade, IceJavelin, Glacier, FrostNova, FrostsKiss, Blizzard, IceArtillery, RayOfIce, IceArmor, AbsoluteZero, SpellMirror,
        EarthernSpear, EarthernUrchin, IdolOfTremors, BoulderFist, StoneStrike, EarthernPlateau, GiantStrength, RockShot, StalagmiteSmash, UnstableEarth, Tremorfall, GaiasCyclone, CaveIn, StonePrison, Earthquake,
        Airgust, SecondWind, Airblade, Aeroslash, Aeroburst, WrathOfTheWind, OrbOfShredding, Multislash, Aerolaunch, WhirlwindSlash, Aerobarrage, PressureDrop, TwinTwisters, Vortex, GroundZero,
        AspectOfRage, Rampage, CausticEdge, ToxicRipple, KillerInstinct, NaturePulse, Revitalize };

    public Transform skillsContainer;

    private SkillBank skillBank;
    [HideInInspector] public PlayerInputs inputs;
    [HideInInspector] public HitBoxManager hitBoxes;
    [HideInInspector] public PlayerStats stats;
    [HideInInspector] public PlayerMovementController controller;
    [HideInInspector] public CameraControls cameraControls;
    [HideInInspector] public CharacterController characterController;


    private void Start()
    {
        skillBank = FindObjectOfType<SkillBank>();
        inputs = GetComponent<PlayerInputs>();
        stats = GetComponent<PlayerStats>();
        controller = GetComponent<PlayerMovementController>();
        hitBoxes = GetComponent<HitBoxManager>();
        cameraControls = Camera.main.GetComponent<CameraControls>();
        characterController = GetComponent<CharacterController>();

        StartCoroutine(SpellMirrorTargetUpdater());
    }

    // Used to add a new skill to our player at an index.
    public void AddSkill(int index, SkillNames skillName)
    {
        if (index < maxSkillNumber)
        {
            RemoveSkill(index);

            Skill addedSkill = skillsContainer.gameObject.AddComponent<Skill>();
            GameObject addedIcon = Instantiate(skillIconPrefab, iconParent);
            
            skillBank.SetSkill(skillName, addedSkill);

            // Add this skills bar to the container as well.
            addedSkill.connectedBar = addedIcon.GetComponentInChildren<BarManager>();
            addedSkill.noManaOverlay = addedIcon.transform.Find("NoManaOverlay").gameObject;
            addedSkill.connectedBar.Initialize(addedSkill.targetCooldown, true, false, 0);
            addedSkill.skillIndex = index;
            addedSkill.myManager = this;
            addedSkill.pc = GetComponent<PlayerMovementController>();
            addedSkill.stats = GetComponent<PlayerStats>();
            addedSkill.anim = GetComponent<Animator>();
            addedSkill.currentCooldown = addedSkill.targetCooldown;
            addedIcon.transform.GetChild(1).GetComponent<Image>().sprite = addedSkill.skillIcon;

            mySkills.Add(addedSkill);
            mySkillBars.Add(addedIcon);
            PositionSkillIcons();

            // If this skill is a passive, add this buff.
            if(addedSkill.passive)
                switch (addedSkill.skillName)
                {
                    case SkillNames.Revitalize:
                        GetComponent<BuffsManager>().NewBuff(BuffsManager.BuffType.Revitalize, stats.baseDamage);
                        stats.revitalizeCount++;
                        break;
                }
        }
        else
            Debug.Log("You have too many skills already!");
    }

    // SUed to remove a skill at an index.
    public void RemoveSkill(int index)
    {
        Skill skillToRemove = null;
 
        // If we have a skill that matches the index we want to put a new one at, remove it and destroy it.
        foreach (Skill skill in mySkills)
            if (skill.skillIndex == index)
            {
                skillToRemove = skill;
                break;
            }

        // This is not done in the foreach loop since we modify the collection we are parsing through for a match. This is a big no no
        if (skillToRemove != null)
        {
            //Debug.Log("The skill " + skillToRemove.skillName + " has been removed");

            // If this skill is a passive, remove this buff.
            if (skillToRemove.passive)
                switch (skillToRemove.skillName)
                {
                    case SkillNames.Revitalize:
                        for (int buffIndex = 0; buffIndex < GetComponent<BuffsManager>().activeBuffs.Count; buffIndex++)
                        {
                            Buff buff = GetComponent<BuffsManager>().activeBuffs[buffIndex];
                            if (buff.myType == BuffsManager.BuffType.Revitalize)
                                buff.RemoveStacks(1, true);
                            stats.revitalizeCount--;
                        }
                        break;
                }

            mySkillBars.Remove(skillToRemove.connectedBar.transform.parent.gameObject);
            mySkills.Remove(skillToRemove);
       
            Destroy(skillToRemove.connectedBar.transform.parent.gameObject);
            Destroy(skillToRemove);

            PositionSkillIcons();
            //Debug.Log("checking to see if the error is before or after this.");
        }
    }

    // Used to position the skills icons properly in the UI.
    private void PositionSkillIcons()
    {
        // First we organize the skils in ouyr myskills list absed on their index.
        if (mySkills.Count > 1)
        {
            mySkills.Sort(SortBySkillIndex);
            for(int index =0; index < mySkills.Count; index++)
                mySkillBars[index] = mySkills[index].connectedBar.transform.parent.gameObject;
        }

        // Only do this if we have 
        if (mySkillBars.Count != 0)
        {
            //string test01 = null;
            //for(int index = 0; index < mySkills.Count; index++)
            //    test01 += mySkills[index].skillName + ", ";
            //Debug.Log(test01);

            
            //string test02 = null;
            //for (int index = 0; index < mySkills.Count; index++)
            //    test02 += mySkills[index].skillName + ", ";
            //Debug.Log(test02);

            // If its an odd count well have to position them differently then an even count.
            if (mySkillBars.Count % 2 == 1)
            {
                // Odds Case, the center of the array is found.
                int centerIndex = (int)((float) mySkillBars.Count / 2 - 0.5f);
                // Debug.Log(centerIndex + " | " + mySkillBars.Count);
                mySkillBars[centerIndex].transform.localPosition = new Vector3(0, 6, 0);

                // everything below this index is placed to the left.
                for (int index = centerIndex - 1; index > -1; index--)
                    mySkillBars[index].transform.localPosition = new Vector3(-40 * (centerIndex - index), 6, 0);
                
                // everything above this index is placed to the right.
                for (int index = centerIndex + 1; index < mySkillBars.Count; index++)
                    mySkillBars[index].transform.localPosition = new Vector3(40 * (index - centerIndex), 6, 0);
            }
            else
            {
                // Evens case tehre are two different icons that are the start of the center of the array.
                int centerIndex = (int)((float)mySkillBars.Count / 2);
                
                // everything below this index is placed to the left.
                for (int index = centerIndex - 1; index > -1; index--)
                    mySkillBars[index].transform.localPosition = new Vector3(-20 + -40 * (centerIndex - 1 - index), 6, 0);

                // everything above this index is placed to the right.
                for (int index = centerIndex; index < mySkillBars.Count; index++)
                    mySkillBars[index].transform.localPosition = new Vector3(20 + 40 * (index - centerIndex), 6, 0);
            }
        }
    }

    // My compare method for my list of skills so i can reorganize it based ion thwe individual skills skill indexs.
    static int SortBySkillIndex(Skill skill1, Skill skill2)
    {
        return skill1.skillIndex.CompareTo(skill2.skillIndex);
    }

    // Used to set the iventory ui for the number of max skills allowed
    public void setInventorySkillSlots()
    {
        inventory.CheckActiveSkillSlots();
    }

    public void SpawnDisjointedSkillEffect(SkillNames skill)
    {
        switch (skill)
        {
            case SkillNames.FallingSword:
                GameObject swordCircle = Instantiate(skillProjectiles[1], transform.position + Vector3.up * 0.1f, Quaternion.identity);
                swordCircle.GetComponent<HitBox>().damage = stats.baseDamage * 0.5f;
                swordCircle.GetComponent<HitBox>().myStats = stats;
                break;
            case SkillNames.RingOfFire:
                GameObject ringOfFire = Instantiate(skillProjectiles[8], transform.position, Quaternion.identity);
                ringOfFire.GetComponent<HitBox>().damage = stats.baseDamage * 3f;
                ringOfFire.GetComponent<HitBox>().myStats = stats;
                GameObject ringOfFireDOT = Instantiate(skillProjectiles[9], transform.position, Quaternion.identity);
                ringOfFireDOT.GetComponent<HitBox>().damage = stats.baseDamage * 0.4f;
                ringOfFireDOT.GetComponent<HitBox>().myStats = stats;
                break;
            case SkillNames.Firestorm:
                for(int index = 0; index < 15; index++)
                {
                    bool succesfulLocationSelected = false;
                    Vector3 targetPosition = Vector3.zero;

                    while (!succesfulLocationSelected)
                    {
                        Ray ray = new Ray(transform.position + new Vector3(Random.Range(-8, 8), 5, Random.Range(-8, 8)), Vector3.down);
                        RaycastHit rayhit;

                        if (Physics.Raycast(ray, out rayhit, 15, targettingRayMask))
                        {
                            targetPosition = rayhit.point;
                            succesfulLocationSelected = true;
                            break;
                        }
                    }

                    GameObject firepillar = Instantiate(skillProjectiles[6], targetPosition, Quaternion.identity);

                    float scaleModifier = Random.Range(0.5f, 1f);
                    firepillar.transform.localScale = new Vector3(scaleModifier, scaleModifier, scaleModifier);

                    firepillar.GetComponent<HitBox>().damage = stats.baseDamage * 0.5f * scaleModifier;
                    firepillar.GetComponent<HitBox>().myStats = stats;
                }
                break;
            case SkillNames.Fireweave:
                for (int index = 0; index < 3; index++)
                {
                    GameObject fireweave = Instantiate(skillProjectiles[10], transform.position + Vector3.up * 2, Quaternion.Euler(new Vector3(Random.Range(-45, -90), Random.Range(0, 360), 0)));
                    fireweave.GetComponent<HitBox>().damage = stats.baseDamage * 0.5f;
                    fireweave.GetComponent<HitBox>().myStats = stats;

                    if (EnemyManager.instance.enemyStats.Count > 0)
                        fireweave.GetComponent<ProjectileBehaviour>().target = EnemyManager.instance.enemyStats[Random.Range(0, EnemyManager.instance.enemyStats.Count)].transform;
                }
                break;

            case SkillNames.FrozenBarricade:
                for(int index = 0; index < 5; index++)
                {
                    Vector3 targetBarricadePosition = transform.position + (transform.forward * ((Mathf.Abs(index - 2)) * -2 + 5)) + transform.right * ((index - 2) * 4f);

                    GameObject frozenBarricade = Instantiate(skillProjectiles[16], targetBarricadePosition, Quaternion.identity);
                    frozenBarricade.GetComponent<HitBox>().damage = stats.baseDamage * 3f;
                    frozenBarricade.GetComponent<HitBox>().myStats = stats;
                }
                break;

            case SkillNames.RayOfIce:

                Ray iceRayRay = new Ray(transform.position + Vector3.up, transform.forward);
                RaycastHit iceRayhit = new RaycastHit();
                Vector3 iceRayTargetPosition = Vector3.zero;

                if(Physics.Raycast(iceRayRay, out iceRayhit, 25, targettingRayMaskHitEnemies))
                {
                    iceRayTargetPosition = iceRayhit.point;
                }
                else
                    iceRayTargetPosition = transform.position + Vector3.up + transform.forward * 25;

                GameObject rayOfIceExplosion = Instantiate(skillProjectiles[21], iceRayTargetPosition, Quaternion.identity);
                rayOfIceExplosion.GetComponent<HitBox>().damage = stats.baseDamage * 0.2f;
                rayOfIceExplosion.GetComponent<HitBox>().myStats = stats;
                break;

            case SkillNames.IceArmor:
                GameObject iceArmorPop = Instantiate(skillProjectiles[22], transform.position + Vector3.up, Quaternion.identity);
                iceArmorPop.GetComponent<HitBox>().damage = stats.baseDamage * 0.5f;
                iceArmorPop.GetComponent<HitBox>().myStats = stats;
                break;

            default:
                break;
        }

        if(spellMirrors.Count > 0)
        {
            foreach(SpellMirrorManager spellMirror in spellMirrors)
            {
                Ray rayDownwards = new Ray(spellMirror.transform.position, Vector3.down);
                RaycastHit rayHitDownwards;
                Vector3 downwardsTargetPosition = Vector3.zero;


                if (Physics.Raycast(rayDownwards, out rayHitDownwards, 100, targettingRayMaskHitEnemies))
                {
                    if (rayHitDownwards.transform.gameObject.layer == 14)
                    {
                        // we hit an enemy
                        downwardsTargetPosition = rayHitDownwards.transform.position;
                    }
                    else
                        downwardsTargetPosition = rayHitDownwards.point;
                }
                else
                {
                    // what if we dont hit anything? default to 10 units in front of the player.
                    downwardsTargetPosition = spellMirror.transform.position + Vector3.down * 10;
                }


                switch (skill)
                {
                    case SkillNames.FallingSword:
                        GameObject swordCircle = Instantiate(skillProjectiles[1], downwardsTargetPosition + Vector3.up * 0.1f, Quaternion.identity);
                        swordCircle.GetComponent<HitBox>().damage = stats.baseDamage * 0.5f;
                        swordCircle.GetComponent<HitBox>().myStats = stats;
                        break;
                    case SkillNames.RingOfFire:
                        GameObject ringOfFire = Instantiate(skillProjectiles[8], downwardsTargetPosition, Quaternion.identity);
                        ringOfFire.GetComponent<HitBox>().damage = stats.baseDamage * 1.5f;
                        ringOfFire.GetComponent<HitBox>().myStats = stats;
                        GameObject ringOfFireDOT = Instantiate(skillProjectiles[9], downwardsTargetPosition, Quaternion.identity);
                        ringOfFireDOT.GetComponent<HitBox>().damage = stats.baseDamage * 0.2f;
                        ringOfFireDOT.GetComponent<HitBox>().myStats = stats;
                        break;
                    case SkillNames.Firestorm:
                        for (int index = 0; index < 15; index++)
                        {
                            bool succesfulLocationSelected = false;
                            Vector3 targetPosition = Vector3.zero;

                            while (!succesfulLocationSelected)
                            {
                                Ray ray = new Ray(transform.position + new Vector3(Random.Range(-8, 8), 5, Random.Range(-8, 8)), Vector3.down);
                                RaycastHit rayhit;

                                if (Physics.Raycast(ray, out rayhit, 15, targettingRayMask))
                                {
                                    targetPosition = rayhit.point;
                                    succesfulLocationSelected = true;
                                    break;
                                }
                            }

                            GameObject firepillar = Instantiate(skillProjectiles[6], targetPosition, Quaternion.identity);

                            float scaleModifier = Random.Range(0.25f, 0.5f);
                            firepillar.transform.localScale = new Vector3(scaleModifier, scaleModifier, scaleModifier);

                            firepillar.GetComponent<HitBox>().damage = stats.baseDamage * 0.5f * scaleModifier;
                            firepillar.GetComponent<HitBox>().myStats = stats;
                        }
                        break;
                    case SkillNames.Fireweave:
                        for (int index = 0; index < 3; index++)
                        {
                            GameObject fireweave = Instantiate(skillProjectiles[10], transform.position + transform.forward, Quaternion.Euler(spellMirror.transform.forward + new Vector3(Random.Range(-45, -90), Random.Range(0, 360), 0)));
                            fireweave.GetComponent<HitBox>().damage = stats.baseDamage * 0.25f;
                            fireweave.GetComponent<HitBox>().myStats = stats;

                            if (EnemyManager.instance.enemyStats.Count > 0)
                                fireweave.GetComponent<ProjectileBehaviour>().target = EnemyManager.instance.enemyStats[Random.Range(0, EnemyManager.instance.enemyStats.Count)].transform;
                        }
                        break;

                    case SkillNames.FrozenBarricade:
                        for (int index = 0; index < 5; index++)
                        {
                            Vector3 spellMirrorForward = spellMirror.transform.forward;
                            spellMirrorForward.y = 0;
                            Vector3 targetBarricadePosition = downwardsTargetPosition + (spellMirrorForward * ((Mathf.Abs(index - 2)) * -2 + 5)) + spellMirror.transform.right * ((index - 2) * 4f);

                            GameObject frozenBarricade = Instantiate(skillProjectiles[16], targetBarricadePosition, Quaternion.identity);
                            frozenBarricade.GetComponent<HitBox>().damage = stats.baseDamage * 1.5f;
                            frozenBarricade.GetComponent<HitBox>().myStats = stats;
                        }
                        break;

                    case SkillNames.RayOfIce:

                        Ray iceRayRay = new Ray(spellMirror.transform.position, spellMirror.transform.forward);
                        RaycastHit iceRayhit = new RaycastHit();
                        Vector3 iceRayTargetPosition = Vector3.zero;

                        if (Physics.Raycast(iceRayRay, out iceRayhit, 25, targettingRayMaskHitEnemies))
                        {
                            iceRayTargetPosition = iceRayhit.point;
                        }
                        else
                            iceRayTargetPosition = spellMirror.transform.position + spellMirror.transform.forward * 25;

                        GameObject rayOfIceExplosion = Instantiate(skillProjectiles[21], iceRayTargetPosition, Quaternion.identity);
                        rayOfIceExplosion.GetComponent<HitBox>().damage = stats.baseDamage * 0.1f;
                        rayOfIceExplosion.GetComponent<HitBox>().myStats = stats;
                        break;

                    case SkillNames.IceArmor:
                        GameObject iceArmorPop = Instantiate(skillProjectiles[22], spellMirror.transform.position, Quaternion.identity);
                        iceArmorPop.GetComponent<HitBox>().damage = stats.baseDamage * 0.25f;
                        iceArmorPop.GetComponent<HitBox>().myStats = stats;
                        break;

                    default:
                        break;
                }
            }
        }
    }

    public void SpawnDisjointedSkillEffectAtCursor(SkillNames skill)
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit rayhit = new RaycastHit();
        Vector3 targetPosition = Vector3.zero;

        if(Physics.Raycast(ray, out rayhit, 100, targettingRayMaskHitEnemies))
        {
            if(rayhit.transform.gameObject.layer == 14)
            {
                // we hit an enemy
                targetPosition = rayhit.transform.position;
            }
            else
                targetPosition = rayhit.point;
        }
        else
        {
            // what if we dont hit anything? default to 10 units in front of the player.
            targetPosition = transform.position + transform.forward * 10;
        }

        switch (skill)
        {
            case SkillNames.WitchPyre:
                GameObject witchPyre = Instantiate(skillProjectiles[6], targetPosition, Quaternion.identity);
                witchPyre.transform.localScale = new Vector3(2, 2, 2);
                witchPyre.GetComponent<HitBox>().damage = stats.baseDamage * 0.7f;
                witchPyre.GetComponent<HitBox>().myStats = stats;
                break;
            case SkillNames.Combustion:
                GameObject combustion = Instantiate(skillProjectiles[7], targetPosition + Vector3.up, Quaternion.identity);
                combustion.GetComponent<HitBox>().damage = stats.baseDamage * 3f;
                combustion.GetComponent<HitBox>().myStats = stats;
                break;
            case SkillNames.IceShards:
                GameObject iceShards = Instantiate(skillProjectiles[13], targetPosition + Vector3.up, Quaternion.identity);
                iceShards.GetComponent<HitBox>().damage = stats.baseDamage * 0.5f;
                iceShards.GetComponent<HitBox>().myStats = stats;
                break;
            case SkillNames.HarshWinds:
                GameObject harshWinds = Instantiate(skillProjectiles[14], targetPosition + Vector3.up, Quaternion.identity);

                Vector3 rotation = harshWinds.transform.rotation.eulerAngles;
                rotation.y -= Random.Range(0, 360);
                harshWinds.transform.rotation = Quaternion.Euler(rotation);

                harshWinds.GetComponent<HitBox>().damage = stats.baseDamage * 1f;
                harshWinds.GetComponent<HitBox>().myStats = stats;
                break;
            case SkillNames.Blizzard:
                GameObject blizzard = Instantiate(skillProjectiles[19], targetPosition + Vector3.up, Quaternion.identity);

                Vector3 blizzardRotation = blizzard.transform.rotation.eulerAngles;
                blizzardRotation.y -= Random.Range(0, 360);
                blizzard.transform.rotation = Quaternion.Euler(blizzardRotation);

                blizzard.GetComponent<HitBox>().damage = stats.baseDamage * 2f;
                blizzard.GetComponent<HitBox>().myStats = stats;
                break;
            case SkillNames.IceArtillery:
                GameObject artillery = Instantiate(skillProjectiles[20], targetPosition + Vector3.up * 0.1f, Quaternion.identity);

                artillery.GetComponent<HitBox>().damage = stats.baseDamage * 0.4f;
                artillery.GetComponent<HitBox>().myStats = stats;
                break;

            case SkillNames.SpellMirror:
                GameObject spellMirror = Instantiate(skillProjectiles[23], targetPosition + Vector3.up * 3f, Quaternion.identity);

                StartCoroutine(SpellMirrorLifetime(spellMirror.GetComponent<SpellMirrorManager>()));
                break;

            case SkillNames.EarthernUrchin:
                GameObject earthernUrchin = Instantiate(skillProjectiles[26], targetPosition + Vector3.up, Quaternion.identity);

                earthernUrchin.GetComponent<HitBox>().damage = stats.baseDamage * 3f;
                earthernUrchin.GetComponent<HitBox>().myStats = stats;
                break;

            case SkillNames.IdolOfTremors:
                GameObject idolOfTremors = Instantiate(skillProjectiles[27], targetPosition, Quaternion.identity);

                idolOfTremors.GetComponentInChildren<HitBox>().damage = stats.baseDamage * 1f;
                idolOfTremors.GetComponentInChildren<HitBox>().myStats = stats;
                break;

            case SkillNames.BoulderFist:
                GameObject boulderFist = Instantiate(skillProjectiles[28], targetPosition, transform.rotation);

                boulderFist.GetComponentInChildren<HitBox>().damage = stats.baseDamage * 4f;
                boulderFist.GetComponentInChildren<HitBox>().myStats = stats;
                boulderFist.GetComponent<HitBoxBuff>().knockbackDirection = transform.forward + Vector3.up * 0.5f;
                break;
            default:
                break;
        }

        if(spellMirrors.Count > 0)
        {
            foreach(SpellMirrorManager spellMirror in spellMirrors)
            {
                Ray spellMirrorRay = new Ray(spellMirror.transform.position, spellMirror.transform.forward);
                RaycastHit spellMirrorRayHit = new RaycastHit();
                Vector3 spellMirrorTargetPosition = Vector3.zero;

                if (Physics.Raycast(spellMirrorRay, out spellMirrorRayHit, 100, targettingRayMaskHitEnemies))
                {
                    if (spellMirrorRayHit.transform.gameObject.layer == 14)
                    {
                        // we hit an enemy
                        spellMirrorTargetPosition = spellMirrorRayHit.transform.position;
                    }
                    else
                        spellMirrorTargetPosition = spellMirrorRayHit.point;
                }
                else
                {
                    // what if we dont hit anything? default to 10 units in front of the player.
                    spellMirrorTargetPosition = spellMirror.transform.position + spellMirror.transform.forward * 10;
                }

                switch (skill)
                {
                    case SkillNames.WitchPyre:
                        GameObject witchPyre = Instantiate(skillProjectiles[6], spellMirrorTargetPosition, Quaternion.identity);
                        witchPyre.transform.localScale = new Vector3(1, 1, 1);
                        witchPyre.GetComponent<HitBox>().damage = stats.baseDamage * 0.35f;
                        witchPyre.GetComponent<HitBox>().myStats = stats;
                        break;
                    case SkillNames.Combustion:
                        GameObject combustion = Instantiate(skillProjectiles[7], spellMirrorTargetPosition + Vector3.up, Quaternion.identity);
                        combustion.GetComponent<HitBox>().damage = stats.baseDamage * 1.5f;
                        combustion.GetComponent<HitBox>().myStats = stats;
                        break;
                    case SkillNames.IceShards:
                        GameObject iceShards = Instantiate(skillProjectiles[13], spellMirrorTargetPosition + Vector3.up, Quaternion.identity);
                        iceShards.GetComponent<HitBox>().damage = stats.baseDamage * 0.25f;
                        iceShards.GetComponent<HitBox>().myStats = stats;
                        break;
                    case SkillNames.HarshWinds:
                        GameObject harshWinds = Instantiate(skillProjectiles[14], spellMirrorTargetPosition + Vector3.up, Quaternion.identity);

                        Vector3 rotation = harshWinds.transform.rotation.eulerAngles;
                        rotation.y -= Random.Range(0, 360);
                        harshWinds.transform.rotation = Quaternion.Euler(rotation);

                        harshWinds.GetComponent<HitBox>().damage = stats.baseDamage * 0.5f;
                        harshWinds.GetComponent<HitBox>().myStats = stats;
                        break;
                    case SkillNames.Blizzard:
                        GameObject blizzard = Instantiate(skillProjectiles[19], spellMirrorTargetPosition + Vector3.up, Quaternion.identity);

                        Vector3 blizzardRotation = blizzard.transform.rotation.eulerAngles;
                        blizzardRotation.y -= Random.Range(0, 360);
                        blizzard.transform.rotation = Quaternion.Euler(blizzardRotation);

                        blizzard.GetComponent<HitBox>().damage = stats.baseDamage * 1f;
                        blizzard.GetComponent<HitBox>().myStats = stats;
                        break;
                    case SkillNames.IceArtillery:
                        GameObject artillery = Instantiate(skillProjectiles[20], spellMirrorTargetPosition + Vector3.up * 0.1f, Quaternion.identity);

                        artillery.GetComponent<HitBox>().damage = stats.baseDamage * 0.2f;
                        artillery.GetComponent<HitBox>().myStats = stats;
                        break;

                    case SkillNames.EarthernUrchin:
                        GameObject earthernUrchin = Instantiate(skillProjectiles[26], spellMirrorTargetPosition + Vector3.up, Quaternion.identity);

                        earthernUrchin.GetComponent<HitBox>().damage = stats.baseDamage * 1.5f;
                        earthernUrchin.GetComponent<HitBox>().myStats = stats;
                        break;
                    case SkillNames.IdolOfTremors:
                        GameObject idolOfTremors = Instantiate(skillProjectiles[27], spellMirrorTargetPosition, Quaternion.identity);

                        idolOfTremors.GetComponentInChildren<HitBox>().damage = stats.baseDamage * 0.5f;
                        idolOfTremors.GetComponentInChildren<HitBox>().myStats = stats;
                        break;

                    case SkillNames.BoulderFist:

                        GameObject boulderFist = Instantiate(skillProjectiles[28], spellMirrorTargetPosition, spellMirror.transform.rotation);

                        Vector3 boulderFistRotation = boulderFist.transform.rotation.eulerAngles;
                        boulderFistRotation.x = 0;
                        boulderFistRotation.z = 0;
                        boulderFist.transform.rotation = Quaternion.Euler(boulderFistRotation);

                        boulderFist.GetComponentInChildren<HitBox>().damage = stats.baseDamage * 2f;
                        boulderFist.GetComponentInChildren<HitBox>().myStats = stats;
                        boulderFist.GetComponent<HitBoxBuff>().knockbackDirection = transform.forward + Vector3.up * 0.5f;
                        break;
                    default:
                        break;
                }
            }
        }
    }

    public void ShootProjectileForward(SkillNames skill)
    {
        Vector3 target = Vector3.zero;
        switch (skill)
        {
            case SkillNames.Firebolt:
                target = Camera.main.transform.position + Camera.main.transform.forward * 100;

                GameObject firebolt = Instantiate(skillProjectiles[2], transform.position + Vector3.up + transform.forward, transform.rotation);
                firebolt.transform.LookAt(target);

                Vector3 rotation = firebolt.transform.rotation.eulerAngles;
                rotation.x -= 3;
                firebolt.transform.rotation = Quaternion.Euler(rotation);

                firebolt.GetComponent<HitBox>().damage = stats.baseDamage * 0.33f;
                firebolt.GetComponent<HitBox>().myStats = stats;
                break;

            case SkillNames.Firebeads:
                GameObject firebead1 = Instantiate(skillProjectiles[3], transform.position + Vector3.up + transform.forward, transform.rotation);
                GameObject firebead2 = Instantiate(skillProjectiles[3], transform.position + Vector3.up + transform.right, transform.rotation);
                GameObject firebead3 = Instantiate(skillProjectiles[3], transform.position + Vector3.up + transform.forward * -1, transform.rotation);
                GameObject firebead4 = Instantiate(skillProjectiles[3], transform.position + Vector3.up + transform.right * -1, transform.rotation);

                firebead1.GetComponent<HitBox>().damage = stats.baseDamage * 0.2f;
                firebead1.GetComponent<HitBox>().myStats = stats;
                firebead1.transform.LookAt(transform.position + transform.right * 100 + Vector3.up);

                firebead2.GetComponent<HitBox>().damage = stats.baseDamage * 0.2f;
                firebead2.GetComponent<HitBox>().myStats = stats;
                firebead2.transform.LookAt(transform.position + transform.forward * -100 + Vector3.up);

                firebead3.GetComponent<HitBox>().damage = stats.baseDamage * 0.2f;
                firebead3.GetComponent<HitBox>().myStats = stats;
                firebead3.transform.LookAt(transform.position + transform.right * -100 + Vector3.up);

                firebead4.GetComponent<HitBox>().damage = stats.baseDamage * 0.2f;
                firebead4.GetComponent<HitBox>().myStats = stats;
                firebead4.transform.LookAt(transform.position + transform.forward * 100 + Vector3.up);
                break;

            case SkillNames.Fireball:
                target = Camera.main.transform.position + Camera.main.transform.forward * 100;

                GameObject fireball = Instantiate(skillProjectiles[11], transform.position + Vector3.up * 2, transform.rotation);
                fireball.transform.LookAt(target);

                Vector3 fireballRotation = fireball.transform.rotation.eulerAngles;
                fireballRotation.x -= 3;
                fireball.transform.rotation = Quaternion.Euler(fireballRotation);

                fireball.GetComponent<HitBox>().damage = stats.baseDamage * 8f;
                fireball.GetComponent<HitBox>().myStats = stats;
                break;

            case SkillNames.IceSpike:
                target = Camera.main.transform.position + Camera.main.transform.forward * 100;

                for (int index = 0; index <= 2; index++)
                {
                    GameObject iceSpike = Instantiate(skillProjectiles[12], transform.position + Vector3.up + transform.forward, transform.rotation);
                    iceSpike.transform.LookAt(target);

                    Vector3 iceSpikeRotation = iceSpike.transform.rotation.eulerAngles;
                    iceSpikeRotation.y += (index - 1) * 10;
                    iceSpikeRotation.x -= 3;
                    iceSpike.transform.rotation = Quaternion.Euler(iceSpikeRotation);

                    iceSpike.GetComponent<HitBox>().damage = stats.baseDamage * 1f;
                    iceSpike.GetComponent<HitBox>().myStats = stats;
                }
                break;

            case SkillNames.IcicleBarrage:
                target = Camera.main.transform.position + Camera.main.transform.forward * 100;

                for (int index = 0; index <= 2; index++)
                {
                    GameObject icicle = Instantiate(skillProjectiles[15], transform.position + Vector3.up + transform.forward, transform.rotation);
                    icicle.transform.LookAt(target);

                    Vector3 iceSpikeRotation = icicle.transform.rotation.eulerAngles;
                    iceSpikeRotation.y += Random.Range(12, -12);
                    iceSpikeRotation.x += Random.Range(12, -12);
                    icicle.transform.rotation = Quaternion.Euler(iceSpikeRotation);

                    icicle.GetComponent<HitBox>().damage = stats.baseDamage * 0.25f;
                    icicle.GetComponent<HitBox>().myStats = stats;
                }
                break;

            case SkillNames.IceJavelin:
                target = Camera.main.transform.position + Camera.main.transform.forward * 100;

                GameObject iceJavelin = Instantiate(skillProjectiles[17], transform.position + Vector3.up * 1.25f + transform.right * 0.5f, transform.rotation);
                iceJavelin.transform.LookAt(target);

                Vector3 iceJavelinRotation = iceJavelin.transform.rotation.eulerAngles;
                iceJavelinRotation.x -= 3;
                iceJavelin.transform.rotation = Quaternion.Euler(iceJavelinRotation);

                iceJavelin.GetComponent<HitBox>().damage = stats.baseDamage * 5f;
                iceJavelin.GetComponent<HitBox>().myStats = stats;
                break;

            case SkillNames.EarthernSpear:
                target = Camera.main.transform.position + Camera.main.transform.forward * 100;

                GameObject earthernSpear = Instantiate(skillProjectiles[25], transform.position + Vector3.up + transform.forward, transform.rotation);
                earthernSpear.transform.LookAt(target);

                Vector3 spearRotation = earthernSpear.transform.rotation.eulerAngles;
                spearRotation.x -= 3;
                earthernSpear.transform.rotation = Quaternion.Euler(spearRotation);

                earthernSpear.GetComponent<HitBox>().damage = stats.baseDamage * 1f;
                earthernSpear.GetComponent<HitBox>().myStats = stats;
                break;
            default:
                break;
        }

        if(spellMirrors.Count > 0)
        {
            foreach(SpellMirrorManager spellMirror in spellMirrors)
            {
                switch (skill)
                {
                    case SkillNames.Firebolt:
                        GameObject firebolt = Instantiate(skillProjectiles[2], spellMirror.transform.position + spellMirror.transform.forward, spellMirror.transform.rotation);

                        firebolt.GetComponent<HitBox>().damage = stats.baseDamage * 0.16f;
                        firebolt.GetComponent<HitBox>().myStats = stats;
                        break;

                    case SkillNames.Firebeads:
                        GameObject firebead1 = Instantiate(skillProjectiles[3], spellMirror.transform.position + transform.forward, transform.rotation);
                        GameObject firebead2 = Instantiate(skillProjectiles[3], spellMirror.transform.position + transform.right, transform.rotation);
                        GameObject firebead3 = Instantiate(skillProjectiles[3], spellMirror.transform.position + transform.forward * -1, transform.rotation);
                        GameObject firebead4 = Instantiate(skillProjectiles[3], spellMirror.transform.position + transform.right * -1, transform.rotation);

                        firebead1.GetComponent<HitBox>().damage = stats.baseDamage * 0.1f;
                        firebead1.GetComponent<HitBox>().myStats = stats;
                        firebead1.transform.LookAt(spellMirror.transform.position + transform.right * 100);

                        firebead2.GetComponent<HitBox>().damage = stats.baseDamage * 0.1f;
                        firebead2.GetComponent<HitBox>().myStats = stats;
                        firebead2.transform.LookAt(spellMirror.transform.position + transform.forward * -100);

                        firebead3.GetComponent<HitBox>().damage = stats.baseDamage * 0.1f;
                        firebead3.GetComponent<HitBox>().myStats = stats;
                        firebead3.transform.LookAt(spellMirror.transform.position + transform.right * -100);

                        firebead4.GetComponent<HitBox>().damage = stats.baseDamage * 0.1f;
                        firebead4.GetComponent<HitBox>().myStats = stats;
                        firebead4.transform.LookAt(spellMirror.transform.position + transform.forward * 100);
                        break;

                    case SkillNames.Fireball:

                        GameObject fireball = Instantiate(skillProjectiles[11], spellMirror.transform.position, spellMirror.transform.rotation);

                        fireball.GetComponent<HitBox>().damage = stats.baseDamage * 4f;
                        fireball.GetComponent<HitBox>().myStats = stats;
                        break;

                    case SkillNames.IceSpike:
                        for (int index = 0; index <= 2; index++)
                        {
                            GameObject iceSpike = Instantiate(skillProjectiles[12], spellMirror.transform.position + spellMirror.transform.forward, spellMirror.transform.rotation);

                            Vector3 iceSpikeRotation = iceSpike.transform.rotation.eulerAngles;
                            iceSpikeRotation.y += (index - 1) * 10;
                            iceSpike.transform.rotation = Quaternion.Euler(iceSpikeRotation);

                            iceSpike.GetComponent<HitBox>().damage = stats.baseDamage * 0.5f;
                            iceSpike.GetComponent<HitBox>().myStats = stats;
                        }
                        break;

                    case SkillNames.IcicleBarrage:
                        for (int index = 0; index <= 2; index++)
                        {
                            GameObject icicle = Instantiate(skillProjectiles[15], spellMirror.transform.position + spellMirror.transform.forward, spellMirror.transform.rotation);

                            Vector3 iceSpikeRotation = icicle.transform.rotation.eulerAngles;
                            iceSpikeRotation.y += Random.Range(12, -12);
                            iceSpikeRotation.x += Random.Range(12, -12);
                            icicle.transform.rotation = Quaternion.Euler(iceSpikeRotation);

                            icicle.GetComponent<HitBox>().damage = stats.baseDamage * 0.125f;
                            icicle.GetComponent<HitBox>().myStats = stats;
                        }
                        break;

                    case SkillNames.IceJavelin:
                        GameObject iceJavelin = Instantiate(skillProjectiles[17], spellMirror.transform.position + spellMirror.transform.forward, spellMirror.transform.rotation);

                        iceJavelin.GetComponent<HitBox>().damage = stats.baseDamage * 2.5f;
                        iceJavelin.GetComponent<HitBox>().myStats = stats;
                        break;

                    case SkillNames.EarthernSpear:
                        GameObject earthernSpear = Instantiate(skillProjectiles[25], spellMirror.transform.position + spellMirror.transform.forward, spellMirror.transform.rotation);

                        earthernSpear.GetComponent<HitBox>().damage = stats.baseDamage * 0.5f;
                        earthernSpear.GetComponent<HitBox>().myStats = stats;
                        break;
                    default:
                        break;
                }
            }
        }
    }

    IEnumerator SpellMirrorLifetime(SpellMirrorManager spellMirror)
    {
        spellMirrors.Add(spellMirror);

        yield return new WaitForSeconds(15);

        spellMirrors.Remove(spellMirror);
        Instantiate(skillProjectiles[24], spellMirror.transform.position, Quaternion.identity);

        Destroy(spellMirror.gameObject);
    }

    IEnumerator SpellMirrorTargetUpdater()
    {
        while(!stats.dead)
        {
            if (spellMirrors.Count > 0)
            {
                // create the target that all the mirrors must face.
                Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
                RaycastHit rayhit = new RaycastHit();
                Vector3 targetPosition = Vector3.zero;

                if (Physics.Raycast(ray, out rayhit, 100, targettingRayMaskHitEnemies))
                {
                    if (rayhit.transform.gameObject.layer == 14)
                    {
                        // we hit an enemy
                        targetPosition = rayhit.transform.position;
                    }
                    else
                        targetPosition = rayhit.point;
                }
                else
                {
                    // what if we dont hit anything? default to 10 units in front of the player.
                    targetPosition = transform.position + transform.forward * 10;
                }

                foreach (SpellMirrorManager spellMirror in spellMirrors)
                {
                    spellMirror.targetToFace = targetPosition;
                }
            }

            yield return new WaitForSeconds(0.25f);
        }
    }
    
}
