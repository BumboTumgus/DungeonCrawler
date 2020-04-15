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
    public int maxSkillNumber = 5;
    public GameObject targetIndicatorCircle;
    public LayerMask targettingRayMask;
    public LayerMask targettingRayMaskHitEnemies;

    public ParticleSystem[] ps;

    public enum SkillNames { BlinkStrike, EmboldeningEmbers, FlameStrike, SeveringStrike, ShatteredEarth, AspectOfRage, Rampage, BlessingOfFlames,
                            GiantStrength, EarthernPlateau, BoulderFist, EarthernSpear, CausticEdge, ToxicRipple, KillerInstinct, NaturePulse,
                            Revitalize, PoisonedMud, StrangleThorn, SoothingStone, Deadeye, ShearingCyclone, WindHarpoon, SplitswordStrikes,
                            ThunderLance, LightningStorm, WrathOfTheRagingWinds, BladeBarrage, ViolentZephyr, Permafrost, IceBarrage, FrozenBarrier,
                            SoothingStream, SwirlingVortex};

    public Transform skillsContainer;

    private SkillBank skillBank;
    public PlayerInputs inputs;
    public HitBoxManager hitBoxes;
    public PlayerStats stats;
    public Rigidbody rb;
    public PlayerController controller;


    private void Start()
    {
        skillBank = FindObjectOfType<SkillBank>();
        inputs = GetComponent<PlayerInputs>();
        stats = GetComponent<PlayerStats>();
        controller = GetComponent<PlayerController>();
        hitBoxes = GetComponent<HitBoxManager>();
        rb = GetComponent<Rigidbody>();
        foreach (ParticleSystem particle in ps)
            particle.Stop();
        AddSkill(0, SkillNames.ToxicRipple);
        AddSkill(1, SkillNames.KillerInstinct);
        AddSkill(2, SkillNames.NaturePulse);
        AddSkill(3, SkillNames.EarthernSpear);
    }


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            foreach (Skill skill in mySkills)
                skill.UseSkill();
        }
        if(Input.GetAxisRaw(inputs.skill0Input) == 1 && inputs.skill0Released)
        {
            inputs.skill0Released = false;
            if (mySkills.Count > 0 && mySkills[0] != null)
                mySkills[0].UseSkill();
        }
        if (Input.GetAxisRaw(inputs.skill1Input) == 1 && inputs.skill1Released)
        {
            inputs.skill1Released = false;
            if (mySkills.Count > 1 && mySkills[1] != null)
                mySkills[1].UseSkill();
        }
        if (Input.GetAxisRaw(inputs.skill2Input) == 1 && inputs.skill2Released)
        {
            inputs.skill2Released = false;
            if (mySkills.Count > 2 && mySkills[2] != null)
                mySkills[2].UseSkill();
        }
        if (Input.GetAxisRaw(inputs.skill3Input) == 1 && inputs.skill3Released)
        {
            inputs.skill3Released = false;
            if (mySkills.Count > 3 && mySkills[3] != null)
                mySkills[3].UseSkill();
        }
        if (Input.GetAxisRaw(inputs.skill4Input) == 1 && inputs.skill4Released)
        {
            inputs.skill4Released = false;
            if (mySkills.Count > 4 && mySkills[4] != null)
                mySkills[4].UseSkill();
        }
        if (Input.GetAxisRaw(inputs.skill5Input) == 1 && inputs.skill5Released)
        {
            inputs.skill5Released = false;
            if (mySkills.Count > 5 && mySkills[5] != null)
                mySkills[5].UseSkill();
        }
        if (Input.GetAxisRaw(inputs.skill6Input) == 1 && inputs.skill6Released)
        {
            inputs.skill6Released = false;
            if (mySkills.Count > 6 && mySkills[6] != null)
                mySkills[6].UseSkill();
        }
        if (Input.GetAxisRaw(inputs.skill7Input) == 1 && inputs.skill7Released)
        {
            inputs.skill7Released = false;
            if (mySkills.Count > 7 && mySkills[7] != null)
                mySkills[7].UseSkill();
        }
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
            addedSkill.connectedBar.Initialize(addedSkill.targetCooldown, true);
            addedSkill.skillIndex = index;
            addedSkill.myManager = this;
            addedSkill.pc = GetComponent<PlayerController>();
            addedSkill.anim = GetComponent<Animator>();
            addedSkill.currentCooldown = addedSkill.targetCooldown;
            addedIcon.transform.GetChild(1).GetComponent<Image>().sprite = addedSkill.skillIcon;

            mySkills.Add(addedSkill);
            mySkillBars.Add(addedIcon);
            PositionSkillIcons();
        }
        else
            Debug.Log("You have too many skills already!");
    }

    // SUed to remove a skill at an index.
    public void RemoveSkill(int index)
    {
        if (index < maxSkillNumber)
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
            if(skillToRemove != null)
            {
                Debug.Log("The skill " + skillToRemove.skillName + " has been removed");
                mySkills.Remove(skillToRemove);
                mySkillBars.Remove(skillToRemove.connectedBar.transform.parent.gameObject);

                Destroy(skillToRemove.connectedBar.transform.parent.gameObject);
                Destroy(skillToRemove);

                PositionSkillIcons();
            }
        }
        else
            Debug.Log("this index is out of our maximum range");
    }

    // Used to position the skills icons properly in the UI.
    private void PositionSkillIcons()
    {
        // Only do this if we have 
        if(mySkillBars.Count != 0)
        {
            // If its an odd count well have to position them differently then an even count.
            if(mySkillBars.Count % 2 == 1)
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
}
