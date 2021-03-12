using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillsManager : MonoBehaviour
{
    public List<Skill> mySkills = new List<Skill>();
    public List<GameObject> mySkillBars = new List<GameObject>();
    public List<GameObject> skillProjectiles = new List<GameObject>();
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
        AspectOfRage, Rampage, GiantStrength, EarthernPlateau, BoulderFist, EarthernSpear, CausticEdge, ToxicRipple, KillerInstinct, NaturePulse, Revitalize };

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
            default:
                break;
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
            default:
                break;
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

            default:
                break;
        }
    }


    
}
