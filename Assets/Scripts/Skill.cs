using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public SkillsManager.SkillNames skillName;
    public Sprite skillIcon;

    public float currentCooldown = 0;
    public float targetCooldown = 10;
    public bool skillReady = false;
    public float skillCost = 0;

    public BarManager connectedBar;
    public GameObject noManaOverlay;

    public int skillIndex = 0;

    public SkillsManager myManager;
    public Animator anim;
    public PlayerController pc;

    // Update is called once per frame
    void Update()
    {
        if (!skillReady)
        {
            currentCooldown += Time.deltaTime;
            connectedBar.SetValue(targetCooldown - currentCooldown);
            
            if (currentCooldown > targetCooldown)
            {
                currentCooldown = targetCooldown;
                skillReady = true;
                connectedBar.gameObject.SetActive(false);
            }
        }
        else
        {
            if (skillCost > myManager.stats.mana)
                noManaOverlay.SetActive(true);
            else
                noManaOverlay.SetActive(false);
        }
    }

    // Called When the skill is going to be used.
    public void UseSkill()
    {
        if (skillReady && myManager.stats.UseMana(skillCost))
        {
            Debug.Log("The skill " + skillName + " has been used");
            switch (skillName)
            {
                case SkillsManager.SkillNames.BlinkStrike:
                    break;
                case SkillsManager.SkillNames.EmboldeningEmbers:
                    StartCoroutine(EmboldeningEmbers());
                    break;
                case SkillsManager.SkillNames.FlameStrike:
                    break;
                case SkillsManager.SkillNames.SeveringStrike:
                    StartCoroutine(SeveringStrike());
                    break;
                default:
                    break;
            }
            currentCooldown = 0;
            skillReady = false;
            connectedBar.gameObject.SetActive(true);
            noManaOverlay.SetActive(false);
        }
        // else
            // Debug.Log("Skill not ready");
    }

    // Used to use the Emboldening Embers spell, an AoE buff for all allies.
    IEnumerator EmboldeningEmbers()
    {
        anim.SetTrigger("EmboldeningEmbers");
        float targetTimer = 1f;
        float currentTimer = 0;
        pc.playerState = PlayerController.PlayerState.CastingNoMovement;
        pc.KillMovement();
        myManager.ps[0].Play();
        myManager.ps[1].Play();
        myManager.ps[2].Play();
        bool playParticles = false;

        while(currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;
            if(!playParticles && currentTimer > targetTimer * 0.5f)
            {
                myManager.ps[3].Play();
                myManager.ps[4].Play();
                myManager.ps[5].Play();
                myManager.hitBoxes.LaunchBuffBox(0);
                myManager.hitBoxes.buffboxes[0].GetComponent<HitBoxBuff>().BuffSelf();
                playParticles = true;
            }
            yield return null;
        }

        myManager.ps[0].Stop();
        myManager.ps[1].Stop();
        myManager.ps[2].Stop();
        pc.playerState = PlayerController.PlayerState.Idle;
    }

    // Used to cast the Severing Strike spell, a giant slam attack that stuns enemies
    IEnumerator SeveringStrike()
    {
        anim.SetTrigger("SeveringStrike");
        float targetTimer = 1f;
        float currentTimer = 0;
        pc.playerState = PlayerController.PlayerState.CastingRollOut;
        pc.KillMovement();
        myManager.ps[6].Play();
        bool playParticles = false;
        myManager.hitBoxes.hitboxes[1].GetComponent<HitBox>().damage = 25 + myManager.stats.Str * 2 + myManager.stats.weaponHitbase + myManager.stats.weaponHitMax;

        while(currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;
            if (!playParticles && currentTimer > targetTimer * 0.5f)
            {
                myManager.ps[7].Play();
                myManager.ps[8].Play();
                myManager.ps[9].Play();
                myManager.hitBoxes.LaunchHitBox(1);
                playParticles = true;
            }
            yield return null;
        }
        
        myManager.ps[6].Stop();
        pc.playerState = PlayerController.PlayerState.Idle;
    }
}
