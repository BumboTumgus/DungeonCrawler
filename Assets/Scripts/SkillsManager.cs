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
    public float[] storedRemainingCooldowns;
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
        storedRemainingCooldowns = new float[3];

        skillBank = FindObjectOfType<SkillBank>();
        inputs = GetComponent<PlayerInputs>();
        stats = GetComponent<PlayerStats>();
        controller = GetComponent<PlayerMovementController>();
        hitBoxes = GetComponent<HitBoxManager>();
        cameraControls = Camera.main.GetComponent<CameraControls>();
        characterController = GetComponent<CharacterController>();

        StartCoroutine(SpellMirrorTargetUpdater());
    }

    private void Update()
    {
        // Lowers our stored cooldowns.
        for (int index = 0; index < storedRemainingCooldowns.Length; index++)
        {
            if (storedRemainingCooldowns[index] > 0)
            {
                storedRemainingCooldowns[index] -= Time.deltaTime;
                if (storedRemainingCooldowns[index] < 0)
                    storedRemainingCooldowns[index] = 0;
            }
        }
    }

    //USed to swap two skills in two index places with one another
    public void SwapSkills(int cursorSkillIndex, SkillNames cursorSkillName, int otherSlotSkillIndex, SkillNames otherSlotSkillName)
    {
        Debug.Log(string.Format("I am swapping skill {0} at index {1} with skill {2} at index {3}.", cursorSkillName, cursorSkillIndex, otherSlotSkillName, otherSlotSkillIndex));

        Skill cursorSkill = GetSkillWithIndexValue(cursorSkillIndex);
        Skill otherSlotSkill = GetSkillWithIndexValue(otherSlotSkillIndex);

        // Store each of the skills current cooldowns into the OPPOSITE index so they can be pulled from when we add skills into both.
        float cursorSkillCDOverride = otherSlotSkill.targetCooldown - otherSlotSkill.currentCooldown;
        float otherSlotSkillCDOverride = cursorSkill.targetCooldown - cursorSkill.currentCooldown;

        RemoveSkill(otherSlotSkillIndex);
        RemoveSkill(cursorSkillIndex);

        storedRemainingCooldowns[cursorSkillIndex] = cursorSkillCDOverride;
        storedRemainingCooldowns[otherSlotSkillIndex] = otherSlotSkillCDOverride;

        AddSkill(cursorSkillIndex, otherSlotSkillName); 
        AddSkill(otherSlotSkillIndex, cursorSkillName);


    }
    // Theis is called if there is no skill in the other slot. We essentially move the skill over to a new slot.
    public void SwapSkills(int cursorSkillIndex, SkillNames cursorSkillName, int otherSlotSkillIndex)
    {
        Debug.Log(string.Format("I am swapping skill {0} at index {1} with no other skill at index {2}.", cursorSkillName, cursorSkillIndex, otherSlotSkillIndex));

        Skill cursorSkill = GetSkillWithIndexValue(cursorSkillIndex);

        storedRemainingCooldowns[otherSlotSkillIndex] = cursorSkill.targetCooldown - cursorSkill.currentCooldown;

        AddSkill(otherSlotSkillIndex, cursorSkillName);

        RemoveSkill(cursorSkillIndex);

        storedRemainingCooldowns[cursorSkillIndex] = 0;
    }

    // Used to add a new skill to our player at an index.
    public void AddSkill(int index, SkillNames skillName)
    {
        //Debug.Log("we are adding a skill and i dont know the target cd");
        if (index < maxSkillNumber)
        {
            RemoveSkill(index);

            Skill addedSkill = skillsContainer.gameObject.AddComponent<Skill>();
            GameObject addedIcon = Instantiate(skillIconPrefab, iconParent);

            //Debug.Log("below is where the target cd is set: " + addedSkill.targetCooldown);
            skillBank.SetSkill(skillName, addedSkill);
            //Debug.Log("the target cd is set: " + addedSkill.targetCooldown);
            addedSkill.targetCooldown *= (1 - stats.cooldownReduction);
            //Debug.Log("the target cd post CDR is set: " + addedSkill.targetCooldown);

            addedSkill.currentCooldown = addedSkill.targetCooldown - storedRemainingCooldowns[index];
            storedRemainingCooldowns[index] = 0;

            // Add this skills bar to the container as well.
            addedSkill.connectedBar = addedIcon.GetComponentInChildren<BarManager>();
            addedSkill.connectedBar.Initialize(addedSkill.targetCooldown - addedSkill.currentCooldown, true, false, 0);
            addedSkill.skillIndex = index;
            addedSkill.myManager = this;

            addedSkill.pc = GetComponent<PlayerMovementController>();
            addedSkill.stats = GetComponent<PlayerStats>();
            addedSkill.anim = GetComponent<Animator>();

            addedIcon.transform.GetChild(2).GetComponent<Image>().sprite = addedSkill.skillIcon;
            addedIcon.transform.GetChild(2).GetComponent<Image>().color = addedSkill.skillIconColor;
            addedIcon.transform.GetChild(0).GetComponent<Image>().color = addedSkill.rarityBorderColor;
            addedIcon.GetComponent<Image>().color = addedSkill.skillBackgroundColor;

            mySkills.Add(addedSkill);
            mySkillBars.Add(addedIcon);
            PositionSkillIcons();

            /*
            // If this skill is a passive, add this buff.
            if(addedSkill.passive)
                switch (addedSkill.skillName)
                {
                    case SkillNames.Revitalize:
                        GetComponent<BuffsManager>().NewBuff(BuffsManager.BuffType.Revitalize, stats.baseDamage);
                        stats.revitalizeCount++;
                        break;
                }
            */
        }
        else
            Debug.Log("You have too many skills already!");
    }

    public void UpdateCooldownSkillCooldowns()
    {
        foreach(Skill skill in mySkills)
        {
            float currentCDRatio = skill.currentCooldown / skill.targetCooldown;

            skillBank.SetSkill(skill.skillName, skill);
            skill.targetCooldown *= (1 - stats.cooldownReduction);
            skill.currentCooldown = skill.targetCooldown * currentCDRatio;
            skill.connectedBar.Initialize(skill.targetCooldown, false, true, skill.currentCooldown);
        }
    }
    public void ReduceSkillCooldowns(float value, bool percentage)
    {
        if (!percentage)
        {
            foreach (Skill skill in mySkills)
                skill.currentCooldown += value;
        }
        else
        {
            foreach (Skill skill in mySkills)
                skill.currentCooldown += (skill.targetCooldown - skill.currentCooldown) * value;
        }
    }

    // Used to remove a skill at an index.
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

            /*
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
            */
            storedRemainingCooldowns[index] = skillToRemove.targetCooldown - skillToRemove.currentCooldown;

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
                mySkillBars[centerIndex].transform.localPosition = new Vector3(0, 25, 0);

                // everything below this index is placed to the left.
                for (int index = centerIndex - 1; index > -1; index--)
                    mySkillBars[index].transform.localPosition = new Vector3(-40 * (centerIndex - index), 25, 0);
                
                // everything above this index is placed to the right.
                for (int index = centerIndex + 1; index < mySkillBars.Count; index++)
                    mySkillBars[index].transform.localPosition = new Vector3(40 * (index - centerIndex), 25, 0);
            }
            else
            {
                // Evens case tehre are two different icons that are the start of the center of the array.
                int centerIndex = (int)((float)mySkillBars.Count / 2);
                
                // everything below this index is placed to the left.
                for (int index = centerIndex - 1; index > -1; index--)
                    mySkillBars[index].transform.localPosition = new Vector3(-20 + -40 * (centerIndex - 1 - index), 25, 0);

                // everything above this index is placed to the right.
                for (int index = centerIndex; index < mySkillBars.Count; index++)
                    mySkillBars[index].transform.localPosition = new Vector3(20 + 40 * (index - centerIndex), 25, 0);
            }
        }
    }

    private Skill GetSkillWithIndexValue(int index)
    {
        Skill skillToReturn = null;

        foreach(Skill skill in mySkills)
        {
            if(skill.skillIndex == index)
            {
                skillToReturn = skill;
                break;
            }
        }

        return skillToReturn;
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
                ringOfFire.GetComponent<HitBoxBuff>().buffOrigin = stats;
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

            case SkillNames.UnstableEarth:
                GameObject unstableEarth = Instantiate(skillProjectiles[32], transform.position, Quaternion.identity);
                unstableEarth.GetComponent<HitBox>().damage = stats.baseDamage * 5f;
                unstableEarth.GetComponent<HitBox>().myStats = stats;
                unstableEarth.GetComponent<HitBoxBuff>().buffOrigin = stats;
                break;

            case SkillNames.Tremorfall:
                GameObject tremorFall = Instantiate(skillProjectiles[33], transform.position, Quaternion.identity);
                tremorFall.GetComponent<HitBox>().damage = stats.baseDamage * 5f;
                tremorFall.GetComponent<HitBox>().myStats = stats;
                tremorFall.GetComponent<HitBoxBuff>().buffOrigin = stats;
                break;

            case SkillNames.Airgust:
                GameObject airgust = Instantiate(skillProjectiles[37], transform.position + transform.forward + Vector3.up, transform.rotation);
                airgust.GetComponent<HitBox>().damage = stats.baseDamage * 1f;
                airgust.GetComponent<HitBox>().myStats = stats;
                airgust.GetComponent<HitBoxBuff>().knockbackDirection = transform.forward + (Vector3.up * 0.25f);
                airgust.GetComponent<HitBoxBuff>().buffOrigin = stats;
                break;

            case SkillNames.Aeroburst:
                GameObject aeroburst = Instantiate(skillProjectiles[40], transform.position + Vector3.up, transform.rotation);
                aeroburst.GetComponent<HitBox>().damage = stats.baseDamage * 3f;
                aeroburst.GetComponent<HitBox>().myStats = stats;
                break;

            case SkillNames.Aerolaunch:
                GameObject aerolaunch = Instantiate(skillProjectiles[43], transform.position, transform.rotation);
                aerolaunch.GetComponent<HitBox>().damage = stats.baseDamage * 3f;
                aerolaunch.GetComponent<HitBox>().myStats = stats;
                aerolaunch.GetComponent<HitBoxBuff>().buffOrigin = stats;
                break;

            case SkillNames.Vortex:
                GameObject vortex = Instantiate(skillProjectiles[47], transform.position + Vector3.up, transform.rotation);
                vortex.GetComponent<HitBox>().damage = stats.baseDamage * 0.66f;
                vortex.GetComponent<HitBox>().myStats = stats;

                HitBoxBuff[] hitboxBuffs = vortex.GetComponentsInChildren<HitBoxBuff>();
                foreach (HitBoxBuff buffBox in hitboxBuffs)
                    buffBox.buffOrigin = stats;

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
                        ringOfFire.GetComponent<HitBoxBuff>().buffOrigin = stats;
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

                    case SkillNames.UnstableEarth:
                        GameObject unstableEarth = Instantiate(skillProjectiles[32], downwardsTargetPosition, Quaternion.identity);
                        unstableEarth.GetComponent<HitBox>().damage = stats.baseDamage * 2.5f;
                        unstableEarth.GetComponent<HitBox>().myStats = stats;
                        unstableEarth.GetComponent<HitBoxBuff>().buffOrigin = stats;
                        break;

                    case SkillNames.Tremorfall:
                        GameObject tremorFall = Instantiate(skillProjectiles[33], downwardsTargetPosition, Quaternion.identity);
                        tremorFall.GetComponent<HitBox>().damage = stats.baseDamage * 2.5f;
                        tremorFall.GetComponent<HitBox>().myStats = stats;
                        tremorFall.GetComponent<HitBoxBuff>().buffOrigin = stats;
                        break;

                    case SkillNames.Airgust:
                        GameObject airgust = Instantiate(skillProjectiles[37], spellMirror.transform.position + spellMirror.transform.forward, spellMirror.transform.rotation );
                        airgust.GetComponent<HitBox>().damage = stats.baseDamage * 0.5f;
                        airgust.GetComponent<HitBox>().myStats = stats;
                        airgust.GetComponent<HitBoxBuff>().knockbackDirection = transform.forward + (Vector3.up * 0.25f);
                        airgust.GetComponent<HitBoxBuff>().buffOrigin = stats;
                        break;

                    case SkillNames.Aeroburst:
                        GameObject aeroburst = Instantiate(skillProjectiles[40], downwardsTargetPosition + Vector3.up, spellMirror.transform.rotation);
                        aeroburst.GetComponent<HitBox>().damage = stats.baseDamage * 1.5f;
                        aeroburst.GetComponent<HitBox>().myStats = stats;
                        break;

                    case SkillNames.Aerolaunch:
                        GameObject aerolaunch = Instantiate(skillProjectiles[43], downwardsTargetPosition, Quaternion.identity);
                        aerolaunch.GetComponent<HitBox>().damage = stats.baseDamage * 1.5f;
                        aerolaunch.GetComponent<HitBox>().myStats = stats;
                        aerolaunch.GetComponent<HitBoxBuff>().buffOrigin = stats;
                        break;

                    case SkillNames.Vortex:
                        GameObject vortex = Instantiate(skillProjectiles[47], downwardsTargetPosition + Vector3.up, transform.rotation);
                        vortex.GetComponent<HitBox>().damage = stats.baseDamage * 0.33f;
                        vortex.GetComponent<HitBox>().myStats = stats;

                        HitBoxBuff[] hitboxBuffs = vortex.GetComponentsInChildren<HitBoxBuff>();
                        foreach (HitBoxBuff buffBox in hitboxBuffs)
                            buffBox.buffOrigin = stats;
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

                boulderFist.GetComponent<HitBox>().damage = stats.baseDamage * 4f;
                boulderFist.GetComponent<HitBox>().myStats = stats;
                boulderFist.GetComponent<HitBoxBuff>().buffOrigin = stats;
                boulderFist.GetComponent<HitBoxBuff>().knockbackDirection = transform.forward + Vector3.up * 0.5f;
                break;

            case SkillNames.EarthernPlateau:
                GameObject earthernPlateau = Instantiate(skillProjectiles[29], targetPosition, transform.rotation);

                earthernPlateau.GetComponent<HitBox>().damage = stats.baseDamage * 2f;
                earthernPlateau.GetComponent<HitBox>().myStats = stats;
                break;

            case SkillNames.StalagmiteSmash:
                GameObject stalagmiteSmash = Instantiate(skillProjectiles[31], targetPosition, transform.rotation);

                stalagmiteSmash.GetComponent<HitBox>().damage = stats.baseDamage * 3f;
                stalagmiteSmash.GetComponent<HitBox>().myStats = stats;
                stalagmiteSmash.GetComponent<HitBoxBuff>().buffOrigin = stats;
                break;

            case SkillNames.GaiasCyclone:
                GameObject gaiasCyclone = Instantiate(skillProjectiles[34], targetPosition, transform.rotation);

                gaiasCyclone.GetComponent<HitBox>().damage = stats.baseDamage * 0.5f;
                gaiasCyclone.GetComponent<HitBox>().myStats = stats;
                break;

            case SkillNames.CaveIn:
                GameObject caveIn = Instantiate(skillProjectiles[35], targetPosition, transform.rotation);

                caveIn.GetComponent<HitBox>().damage = stats.baseDamage * 1.5f;
                caveIn.GetComponent<HitBox>().myStats = stats;
                caveIn.GetComponent<HitBoxBuff>().buffOrigin = stats;
                break;

            case SkillNames.StonePrison:
                GameObject stonePrison = Instantiate(skillProjectiles[36], targetPosition, transform.rotation);
                stonePrison.GetComponent<HitBoxBuff>().buffOrigin = stats;
                break;

            case SkillNames.TwinTwisters:
                GameObject twinTwisters = Instantiate(skillProjectiles[46], targetPosition, transform.rotation);

                twinTwisters.GetComponent<HitBox>().damage = stats.baseDamage * 0.4f;
                twinTwisters.GetComponent<HitBox>().myStats = stats;

                GameObject twinTwistersTwo = Instantiate(skillProjectiles[46], targetPosition, transform.rotation);

                Vector3 twinTwisterRotation = twinTwistersTwo.transform.rotation.eulerAngles;
                twinTwisterRotation.y -= Random.Range(0, 360);
                twinTwistersTwo.transform.rotation = Quaternion.Euler(twinTwisterRotation);

                twinTwistersTwo.GetComponent<HitBox>().damage = stats.baseDamage * 0.4f;
                twinTwistersTwo.GetComponent<HitBox>().myStats = stats;
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

                        boulderFist.GetComponent<HitBox>().damage = stats.baseDamage * 2f;
                        boulderFist.GetComponent<HitBox>().myStats = stats;
                        boulderFist.GetComponent<HitBoxBuff>().buffOrigin = stats;
                        boulderFist.GetComponent<HitBoxBuff>().knockbackDirection = transform.forward + Vector3.up * 0.5f;
                        break;

                    case SkillNames.EarthernPlateau:
                        GameObject earthernPlateau = Instantiate(skillProjectiles[29], spellMirrorTargetPosition, transform.rotation);

                        earthernPlateau.GetComponent<HitBox>().damage = stats.baseDamage * 1f;
                        earthernPlateau.GetComponent<HitBox>().myStats = stats;
                        break;

                    case SkillNames.StalagmiteSmash:
                        GameObject stalagmiteSmash = Instantiate(skillProjectiles[31], spellMirrorTargetPosition, transform.rotation);

                        stalagmiteSmash.GetComponent<HitBox>().damage = stats.baseDamage * 1.5f;
                        stalagmiteSmash.GetComponent<HitBox>().myStats = stats;
                        stalagmiteSmash.GetComponent<HitBoxBuff>().buffOrigin = stats;
                        break;

                    case SkillNames.GaiasCyclone:
                        GameObject gaiasCyclone = Instantiate(skillProjectiles[34], spellMirrorTargetPosition, transform.rotation);

                        gaiasCyclone.GetComponent<HitBox>().damage = stats.baseDamage * 0.25f;
                        gaiasCyclone.GetComponent<HitBox>().myStats = stats;
                        break;

                    case SkillNames.CaveIn:
                        GameObject caveIn = Instantiate(skillProjectiles[35], spellMirrorTargetPosition, transform.rotation);

                        caveIn.GetComponent<HitBox>().damage = stats.baseDamage * 0.75f;
                        caveIn.GetComponent<HitBox>().myStats = stats;
                        caveIn.GetComponent<HitBoxBuff>().buffOrigin = stats;
                        break;

                    case SkillNames.StonePrison:
                        GameObject stonePrison = Instantiate(skillProjectiles[36], spellMirrorTargetPosition, transform.rotation);
                        stonePrison.GetComponent<HitBoxBuff>().buffOrigin = stats;
                        break;

                    case SkillNames.TwinTwisters:
                        GameObject twinTwisters = Instantiate(skillProjectiles[46], spellMirrorTargetPosition, transform.rotation);

                        twinTwisters.GetComponent<HitBox>().damage = stats.baseDamage * 0.2f;
                        twinTwisters.GetComponent<HitBox>().myStats = stats;

                        GameObject twinTwistersTwo = Instantiate(skillProjectiles[46], spellMirrorTargetPosition, transform.rotation);

                        Vector3 twinTwisterRotation = twinTwistersTwo.transform.rotation.eulerAngles;
                        twinTwisterRotation.y -= Random.Range(0, 360);
                        twinTwistersTwo.transform.rotation = Quaternion.Euler(twinTwisterRotation);

                        twinTwistersTwo.GetComponent<HitBox>().damage = stats.baseDamage * 0.2f;
                        twinTwistersTwo.GetComponent<HitBox>().myStats = stats;
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

            case SkillNames.RockShot:
                target = Camera.main.transform.position + Camera.main.transform.forward * 100;

                GameObject rockShot = Instantiate(skillProjectiles[30], transform.position + Vector3.up + transform.forward, transform.rotation);
                rockShot.transform.LookAt(target);

                Vector3 rockShotRotation = rockShot.transform.rotation.eulerAngles;
                rockShotRotation.x -= 3;
                rockShot.transform.rotation = Quaternion.Euler(rockShotRotation);

                rockShot.GetComponent<HitBox>().damage = stats.baseDamage * 4f;
                rockShot.GetComponent<HitBox>().myStats = stats;
                break;

            case SkillNames.Airblade:
                target = Camera.main.transform.position + Camera.main.transform.forward * 100;

                GameObject airblade = Instantiate(skillProjectiles[38], transform.position + Vector3.up + transform.forward, transform.rotation);
                airblade.transform.LookAt(target);

                Vector3 airbladeRotation = airblade.transform.rotation.eulerAngles;
                airbladeRotation.x -= 3;
                airblade.transform.rotation = Quaternion.Euler(airbladeRotation);

                airblade.GetComponent<HitBox>().damage = stats.baseDamage * 4f;
                airblade.GetComponent<HitBox>().myStats = stats;
                break;

            case SkillNames.Aeroslash:
                target = Camera.main.transform.position + Camera.main.transform.forward * 100;

                GameObject aeroslash = Instantiate(skillProjectiles[39], transform.position + Vector3.up + transform.forward, transform.rotation);
                aeroslash.transform.LookAt(target);

                Vector3 aeroslashRotation = aeroslash.transform.rotation.eulerAngles;
                aeroslashRotation.x -= 3;
                aeroslash.transform.rotation = Quaternion.Euler(aeroslashRotation);

                aeroslash.GetComponent<HitBox>().damage = stats.baseDamage * 0.8f;
                aeroslash.GetComponent<HitBox>().myStats = stats;
                aeroslash.GetComponent<HitBoxBuff>().buffOrigin = stats;
                break;

            case SkillNames.OrbOfShredding:
                target = Camera.main.transform.position + Camera.main.transform.forward * 100;

                GameObject orbOfShredding = Instantiate(skillProjectiles[41], transform.position + Vector3.up + transform.forward, transform.rotation);
                orbOfShredding.transform.LookAt(target);

                Vector3 orbOfShreddingRotation = orbOfShredding.transform.rotation.eulerAngles;
                orbOfShreddingRotation.x -= 3;
                orbOfShredding.transform.rotation = Quaternion.Euler(orbOfShreddingRotation);

                orbOfShredding.GetComponent<HitBox>().damage = stats.baseDamage * 0.4f;
                orbOfShredding.GetComponent<HitBox>().myStats = stats;
                orbOfShredding.GetComponent<HitBoxBuff>().buffOrigin = stats;
                break;

            case SkillNames.WhirlwindSlash:

                for (int index = 0; index < 5; index++)
                {
                    GameObject whirlwindSlash = Instantiate(skillProjectiles[44], transform.position + Vector3.up + transform.forward, transform.rotation);

                    Vector3 whrilwindSlashRotation = new Vector3(Random.Range(-20,20), Random.Range(0,360), Random.Range(-10,10));
                    whirlwindSlash.transform.rotation = Quaternion.Euler(whrilwindSlashRotation);

                    whirlwindSlash.GetComponent<HitBox>().damage = stats.baseDamage * 1.2f;
                    whirlwindSlash.GetComponent<HitBox>().myStats = stats;
                }
                break;

            case SkillNames.Aerobarrage:
                target = Camera.main.transform.position + Camera.main.transform.forward * 100;

                for (int index = 0; index < 3; index++)
                {
                    GameObject aerobarrage = Instantiate(skillProjectiles[45], transform.position + Vector3.up + transform.forward, transform.rotation);
                    aerobarrage.transform.LookAt(target);

                    Vector3 aerobarrageRotation = aerobarrage.transform.rotation.eulerAngles;
                    aerobarrageRotation.x -= 3;
                    aerobarrageRotation.y += Random.Range(20,33) * (index - 1) + Random.Range(-5, 5);
                    aerobarrage.transform.rotation = Quaternion.Euler(aerobarrageRotation);

                    aerobarrage.GetComponent<HitBox>().damage = stats.baseDamage * 1f;
                    aerobarrage.GetComponent<HitBox>().myStats = stats;
                    aerobarrage.GetComponent<HitBoxBuff>().buffOrigin = stats;
                }
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

                    case SkillNames.RockShot:
                        GameObject rockShot = Instantiate(skillProjectiles[30], spellMirror.transform.position + spellMirror.transform.forward, spellMirror.transform.rotation);

                        rockShot.GetComponent<HitBox>().damage = stats.baseDamage * 2f;
                        rockShot.GetComponent<HitBox>().myStats = stats;
                        break;

                    case SkillNames.Airblade:
                        GameObject airblade = Instantiate(skillProjectiles[38], spellMirror.transform.position + spellMirror.transform.forward, spellMirror.transform.rotation);

                        airblade.GetComponent<HitBox>().damage = stats.baseDamage * 2f;
                        airblade.GetComponent<HitBox>().myStats = stats;
                        break;

                    case SkillNames.Aeroslash:
                        GameObject aeroslash = Instantiate(skillProjectiles[39], spellMirror.transform.position + spellMirror.transform.forward, spellMirror.transform.rotation);

                        aeroslash.GetComponent<HitBox>().damage = stats.baseDamage * 0.4f;
                        aeroslash.GetComponent<HitBox>().myStats = stats;
                        aeroslash.GetComponent<HitBoxBuff>().buffOrigin = stats;
                        break;

                    case SkillNames.OrbOfShredding:
                        GameObject orbOfShredding = Instantiate(skillProjectiles[41], spellMirror.transform.position + spellMirror.transform.forward, spellMirror.transform.rotation);

                        orbOfShredding.GetComponent<HitBox>().damage = stats.baseDamage * 0.4f;
                        orbOfShredding.GetComponent<HitBox>().myStats = stats;
                        orbOfShredding.GetComponent<HitBoxBuff>().buffOrigin = stats;
                        break;

                    case SkillNames.WhirlwindSlash:
                            GameObject whirlwindSlash = Instantiate(skillProjectiles[44], spellMirror.transform.position, spellMirror.transform.rotation);

                            whirlwindSlash.GetComponent<HitBox>().damage = stats.baseDamage * 0.6f;
                            whirlwindSlash.GetComponent<HitBox>().myStats = stats;
                        break;


                    case SkillNames.Aerobarrage:

                        for (int index = 0; index < 3; index++)
                        {
                            GameObject aerobarrage = Instantiate(skillProjectiles[45], spellMirror.transform.position, spellMirror.transform.rotation);

                            Vector3 aerobarrageRotation = aerobarrage.transform.rotation.eulerAngles;
                            aerobarrageRotation.y += Random.Range(20, 33) * (index - 1) + Random.Range(-5, 5);
                            aerobarrage.transform.rotation = Quaternion.Euler(aerobarrageRotation);

                            aerobarrage.GetComponent<HitBox>().damage = stats.baseDamage * 0.5f;
                            aerobarrage.GetComponent<HitBox>().myStats = stats;
                            aerobarrage.GetComponent<HitBoxBuff>().buffOrigin = stats;
                        }
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
