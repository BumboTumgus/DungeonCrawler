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

    public ParticleSystem[] ps;

    public enum SkillNames { BlinkStrike, EmboldeningEmbers, FlameStrike, SeveringStrike, ShatteredEarth, AspectOfRage, Rampage, BlessingOfFlames,
                            GiantStrength, EarthernPlateau, BoulderFist, EarthernSpear, CausticEdge, ToxicRipple, KillerInstinct, NaturePulse,
                            Revitalize, PoisonedMud, StrangleThorn, SoothingStone, Deadeye, ShearingCyclone, WindHarpoon, SplitswordStrikes,
                            ThunderLance, LightningStorm, WrathOfTheRagingWinds, BladeBarrage, ViolentZephyr, Permafrost, IceBarrage, FrozenBarrier,
                            SoothingStream, SwirlingVortex};

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
        foreach (ParticleSystem particle in ps)
            particle.Stop();
        //AddSkill(0, SkillNames.ToxicRipple);
        //AddSkill(1, SkillNames.CausticEdge);
        //AddSkill(2, SkillNames.NaturePulse);
        //AddSkill(3, SkillNames.Revitalize);
    }
    /**
    // USed to check the inputs to see if a skill is at that index that should be 
    private void CheckSkillInput(int index)
    {
        foreach (Skill skill in mySkills)
        {
            if (skill.skillIndex == index)
                skill.UseSkill();
        }
    }

    private void Update()
    {
        if (Input.GetAxisRaw(inputs.skill0Input) == 1 && inputs.skill0Released)
            CheckSkillInput(0);
        if (Input.GetAxisRaw(inputs.skill1Input) == 1 && inputs.skill1Released)
            CheckSkillInput(1);
        if (Input.GetAxisRaw(inputs.skill2Input) == 1 && inputs.skill2Released)
            CheckSkillInput(2);
        if (Input.GetAxisRaw(inputs.skill3Input) == 1 && inputs.skill3Released)
            CheckSkillInput(3);
        if (Input.GetAxisRaw(inputs.skill4Input) == 1 && inputs.skill4Released)
            CheckSkillInput(4);
        if (Input.GetAxisRaw(inputs.skill5Input) == 1 && inputs.skill5Released)
            CheckSkillInput(5);
        if (Input.GetAxisRaw(inputs.skill6Input) == 1 && inputs.skill6Released)
            CheckSkillInput(6);
        if (Input.GetAxisRaw(inputs.skill7Input) == 1 && inputs.skill7Released)
            CheckSkillInput(7);
    }
    */
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
    
}
