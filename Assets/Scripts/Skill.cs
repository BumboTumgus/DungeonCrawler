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
    public bool passive = false;

    public BarManager connectedBar;
    public GameObject noManaOverlay;

    public int skillIndex = 0;

    public SkillsManager myManager;
    public Animator anim;
    public PlayerMovementController pc;
    public PlayerStats stats;
    public GameObject targetIndicator;

    private Coroutine earthernSpear;
    private Coroutine boulderFist;
    private Coroutine naturePulse;
    

    // SUed to check this skills input to see if their key has been pressed and wether we should cast this skill.
    private void CheckInputs()
    {
        switch (skillIndex)
        {
            case 0:
                if (Input.GetAxisRaw(myManager.inputs.skill0Input) == 1 && myManager.inputs.skill0Released)
                    UseSkill();
                break;
            case 1:
                if (Input.GetAxisRaw(myManager.inputs.skill1Input) == 1 && myManager.inputs.skill1Released)
                    UseSkill();
                break;
            case 2:
                if (Input.GetAxisRaw(myManager.inputs.skill2Input) == 1 && myManager.inputs.skill2Released)
                    UseSkill();
                break;
            case 3:
                if (Input.GetAxisRaw(myManager.inputs.skill3Input) == 1 && myManager.inputs.skill3Released)
                    UseSkill();
                break;
            case 4:
                if (Input.GetAxisRaw(myManager.inputs.skill4Input) == 1 && myManager.inputs.skill4Released)
                    UseSkill();
                break;
            case 5:
                if (Input.GetAxisRaw(myManager.inputs.skill5Input) == 1 && myManager.inputs.skill5Released)
                    UseSkill();
                break;
            case 6:
                if (Input.GetAxisRaw(myManager.inputs.skill6Input) == 1 && myManager.inputs.skill6Released)
                    UseSkill();
                break;
            case 7:
                if (Input.GetAxisRaw(myManager.inputs.skill7Input) == 1 && myManager.inputs.skill7Released)
                    UseSkill();
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckInputs();
        if (!skillReady)
        {
            currentCooldown += Time.deltaTime;
            connectedBar.SetValue(targetCooldown - currentCooldown, false);
            
            if (currentCooldown > targetCooldown)
            {
                currentCooldown = targetCooldown;
                skillReady = true;
                connectedBar.gameObject.SetActive(false);
            }
        }
    }

    // Called When the skill is going to be used.
    public void UseSkill()
    {
        if (skillReady && pc.playerState != PlayerMovementController.PlayerState.Rolling && pc.playerState != PlayerMovementController.PlayerState.CastingAerial && pc.playerState != PlayerMovementController.PlayerState.CastingNoMovement && pc.playerState != PlayerMovementController.PlayerState.CastingRollOut
             && pc.playerState != PlayerMovementController.PlayerState.CastingWithMovement)
        {
            //Debug.Log("The skill " + skillName + " has been used");

            pc.SkillCastCoroutineClear();
            bool successfulSkillUse = true;

            switch (skillName)
            {
                case SkillsManager.SkillNames.SweepingBlow:
                    StartCoroutine(SweepingBlow());
                    break;
                case SkillsManager.SkillNames.Rapislash:
                    StartCoroutine(Rapislash());
                    break;
                case SkillsManager.SkillNames.SkywardSlash:
                    StartCoroutine(SkywardSlash());
                    break;
                case SkillsManager.SkillNames.BladeVolley:
                    StartCoroutine(BladeVolley());
                    break;
                case SkillsManager.SkillNames.BlinkStrike:
                    StartCoroutine(BlinkStrike());
                    break;
                case SkillsManager.SkillNames.TremorStab:
                    StartCoroutine(TremorStab());
                    break;
                case SkillsManager.SkillNames.LeapStrike:
                    StartCoroutine(LeapStrike());
                    break;
                case SkillsManager.SkillNames.Takedown:
                    StartCoroutine(Takedown());
                    break;
                case SkillsManager.SkillNames.Impale:
                    StartCoroutine(Impale());
                    break;
                case SkillsManager.SkillNames.Counter:
                    StartCoroutine(Counter());
                    break;
                case SkillsManager.SkillNames.SeveringStrike:
                    StartCoroutine(SeveringStrike());
                    break;
                case SkillsManager.SkillNames.Whirlwind:
                    StartCoroutine(Whirlwind());
                    break;
                case SkillsManager.SkillNames.ShatteredEarth:
                    StartCoroutine(ShatteredEarth());
                    break;
                case SkillsManager.SkillNames.FallingSword:
                    StartCoroutine(FallingSword());
                    break;
                case SkillsManager.SkillNames.SenateSlash:
                    StartCoroutine(SenateSlash());
                    break;



                case SkillsManager.SkillNames.Firebolt:
                    StartCoroutine(Firebolt());
                    break;
                case SkillsManager.SkillNames.Ignition:
                    StartCoroutine(Ignition());
                    break;
                case SkillsManager.SkillNames.EmboldeningEmbers:
                    StartCoroutine(EmboldeningEmbers());
                    break;
                case SkillsManager.SkillNames.Firebeads:
                    StartCoroutine(Firebeads());
                    break;
                case SkillsManager.SkillNames.HeatPulse:
                    StartCoroutine(HeatPulse());
                    break;
                case SkillsManager.SkillNames.FlameStrike:
                    StartCoroutine(FlameStrike());
                    break;
                case SkillsManager.SkillNames.Flamewalker:
                    StartCoroutine(FlameWalker());
                    break;
                case SkillsManager.SkillNames.WitchPyre:
                    StartCoroutine(WitchPyre());
                    break;
                case SkillsManager.SkillNames.Combustion:
                    StartCoroutine(Combustion());
                    break;
                case SkillsManager.SkillNames.RingOfFire:
                    StartCoroutine(RingOfFire());
                    break;
                case SkillsManager.SkillNames.BlessingOfFlames:
                    StartCoroutine(BlessingOfFlames());
                    break;
                case SkillsManager.SkillNames.Immolate:
                    StartCoroutine(Immolate());
                    break;
                case SkillsManager.SkillNames.Firestorm:
                    StartCoroutine(Firestorm());
                    break;
                case SkillsManager.SkillNames.Fireweave:
                    StartCoroutine(Fireweave());
                    break;
                case SkillsManager.SkillNames.Fireball:
                    StartCoroutine(Fireball());
                    break;



                case SkillsManager.SkillNames.IceSpike:
                    StartCoroutine(IceSpike());
                    break;
                case SkillsManager.SkillNames.IceShards:
                    StartCoroutine(IceShards());
                    break;
                case SkillsManager.SkillNames.HarshWinds:
                    StartCoroutine(HarshWinds());
                    break;
                case SkillsManager.SkillNames.IcicleBarrage:
                    StartCoroutine(IcicleBarrage());
                    break;
                case SkillsManager.SkillNames.FrozenBarricade:
                    StartCoroutine(FrozenBarricade());
                    break;
                case SkillsManager.SkillNames.IceJavelin:
                    StartCoroutine(IceJavelin());
                    break;
                case SkillsManager.SkillNames.Glacier:
                    StartCoroutine(Glacier());
                    break;
                case SkillsManager.SkillNames.FrostNova:
                    StartCoroutine(FrostNova());
                    break;
                case SkillsManager.SkillNames.FrostsKiss:
                    StartCoroutine(FrostsKiss());
                    break;
                case SkillsManager.SkillNames.Blizzard:
                    StartCoroutine(Blizzard());
                    break;
                case SkillsManager.SkillNames.IceArtillery:
                    StartCoroutine(IceArtillery());
                    break;
                case SkillsManager.SkillNames.RayOfIce:
                    StartCoroutine(RayOfIce());
                    break;
                case SkillsManager.SkillNames.IceArmor:
                    StartCoroutine(IceArmor());
                    break;
                case SkillsManager.SkillNames.AbsoluteZero:
                    StartCoroutine(AbsoluteZero());
                    break;
                case SkillsManager.SkillNames.SpellMirror:
                    StartCoroutine(SpellMirror());
                    break;



                case SkillsManager.SkillNames.EarthernSpear:
                    StartCoroutine(EarthernSpears());
                    break;
                case SkillsManager.SkillNames.EarthernUrchin:
                    StartCoroutine(EarthernUrchin());
                    break;
                case SkillsManager.SkillNames.IdolOfTremors:
                    StartCoroutine(IdolOfTremors());
                    break;
                case SkillsManager.SkillNames.BoulderFist:
                    StartCoroutine(BoulderFist());
                    break;
                case SkillsManager.SkillNames.StoneStrike:
                    StartCoroutine(StoneStrike());
                    break;
                case SkillsManager.SkillNames.EarthernPlateau:
                    StartCoroutine(EarthernPlateau());
                    break;
                case SkillsManager.SkillNames.GiantStrength:
                    StartCoroutine(GiantStrength());
                    break;
                case SkillsManager.SkillNames.RockShot:
                    StartCoroutine(RockShot());
                    break;
                case SkillsManager.SkillNames.StalagmiteSmash:
                    StartCoroutine(StalagmiteSmash());
                    break;
                case SkillsManager.SkillNames.UnstableEarth:
                    StartCoroutine(UnstableEarth());
                    break;
                case SkillsManager.SkillNames.Tremorfall:
                    if (pc.playerState == PlayerMovementController.PlayerState.Airborne || pc.playerState == PlayerMovementController.PlayerState.Jumping)
                        StartCoroutine(Tremorfall());
                    else
                        successfulSkillUse = false;
                    break;
                case SkillsManager.SkillNames.GaiasCyclone:
                    StartCoroutine(GaiasCyclone());
                    break;
                case SkillsManager.SkillNames.CaveIn:
                    StartCoroutine(CaveIn());
                    break;
                case SkillsManager.SkillNames.StonePrison:
                    StartCoroutine(StonePrison());
                    break;
                case SkillsManager.SkillNames.Earthquake:
                    StartCoroutine(Earthquake());
                    break;



                case SkillsManager.SkillNames.Airgust:
                    StartCoroutine(Airgust());
                    break;
                case SkillsManager.SkillNames.SecondWind:
                    StartCoroutine(SecondWind());
                    break;
                case SkillsManager.SkillNames.Airblade:
                    StartCoroutine(Airblade());
                    break;
                case SkillsManager.SkillNames.Aeroslash:
                    StartCoroutine(Aeroslash());
                    break;
                case SkillsManager.SkillNames.Aeroburst:
                    StartCoroutine(Aeroburst());
                    break;
                case SkillsManager.SkillNames.WrathOfTheWind:
                    StartCoroutine(WrathOfTheWind());
                    break;
                case SkillsManager.SkillNames.OrbOfShredding:
                    StartCoroutine(OrbOfShredding());
                    break;
                case SkillsManager.SkillNames.Multislash:
                    StartCoroutine(Multislash());
                    break;
                case SkillsManager.SkillNames.Aerolaunch:
                    StartCoroutine(Aerolaunch());
                    break;
                case SkillsManager.SkillNames.WhirlwindSlash:
                    StartCoroutine(WhirlwindSlash());
                    break;
                case SkillsManager.SkillNames.Aerobarrage:
                    StartCoroutine(Aerobarrage());
                    break;
                case SkillsManager.SkillNames.PressureDrop:
                    StartCoroutine(PressureDrop());
                    break;
                case SkillsManager.SkillNames.TwinTwisters:
                    StartCoroutine(TwinTwisters());
                    break;
                case SkillsManager.SkillNames.Vortex:
                    StartCoroutine(Vortex());
                    break;
                case SkillsManager.SkillNames.GroundZero:
                    StartCoroutine(GroundZero());
                    break;
                default:
                    break;
            }

            if (successfulSkillUse)
            {
                currentCooldown = 0;
                skillReady = false;
                connectedBar.gameObject.SetActive(true);
                noManaOverlay.SetActive(false);
            }
            else
            {
                Debug.Log("Skill not used since we do not match the criteria for using it.");
            }
        }
        // else
            // Debug.Log("Skill not ready");
    }

    // USed to cast the spell sweeping edge.
    IEnumerator SweepingBlow()
    {
        anim.SetTrigger("SweepingBlow");
        //anim.applyRootMotion = true;
        anim.SetFloat("AttackAnimSpeed", stats.attackSpeed);
        pc.SnapToFaceCamera();

        float targetTimer = 1.85f / stats.attackSpeed / 1.3f;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingWithMovement;

        myManager.hitBoxes.hitboxes[2].GetComponent<HitBox>().damage = myManager.stats.baseDamage * 2f;
        myManager.hitBoxes.hitboxes[3].GetComponent<HitBox>().damage = myManager.stats.baseDamage * 3f;

        //Vector3 directionToMove = transform.forward;
        //float distance = 3;
        //float distancePerSecond = distance / targetTimer;

        while (currentTimer < targetTimer)
        {
            //pc.SkillMovement(directionToMove, distancePerSecond);
            currentTimer += Time.deltaTime;
            yield return null;
        }

        //anim.applyRootMotion = false;
        pc.CheckForOtherLoseOfControlEffects();
    }

    // USed to cast the spell rapislash
    IEnumerator Rapislash()
    {
        anim.SetTrigger("Rapislash");
        anim.SetFloat("AttackAnimSpeed", stats.attackSpeed);
        pc.SnapToFaceCamera();

        float targetTimer = 1.33f / stats.attackSpeed;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingWithMovement;

        myManager.hitBoxes.hitboxes[4].GetComponent<HitBox>().damage = myManager.stats.baseDamage * 1.4f;

        //Vector3 directionToMove = transform.forward;
        //float distance = 2;
        // float distancePerSecond = distance / targetTimer;


        while (currentTimer < targetTimer)
        {
            //pc.SkillMovement(directionToMove, distancePerSecond);
            currentTimer += Time.deltaTime;
            yield return null;
        }

        pc.CheckForOtherLoseOfControlEffects();
    }

    // USed to cast the spell skyward slash.
    IEnumerator SkywardSlash()
    {
        anim.SetTrigger("SkywardSlash");
        anim.SetFloat("AttackAnimSpeed", stats.attackSpeed);
        pc.SnapToFaceCamera();

        float targetTimer = 1f / stats.attackSpeed;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingAerial;

        myManager.hitBoxes.hitboxes[5].GetComponent<HitBox>().damage = myManager.stats.baseDamage * 7f;
        //pc.StartCoroutine(pc.Jump(0.2f, 0.3f, false));

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;
            yield return null;
        }

        pc.CheckForOtherLoseOfControlEffects();
    }

    // USed to cast the spell blade volley.
    IEnumerator BladeVolley()
    {
        anim.SetTrigger("BladeVolley");
        anim.SetFloat("AttackAnimSpeed", stats.attackSpeed);
        pc.SnapToFaceCamera();

        bool bladesThrown = false;

        float targetTimer = 0.467f / stats.attackSpeed;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingWithMovement;

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;
            if (!bladesThrown && currentTimer / targetTimer >= 0.43f)
            {
                // shoot the blades.
                bladesThrown = true;

                Vector3 target = Camera.main.transform.position + Camera.main.transform.forward * 100;

                //Debug.LogError("knife thrown");
                for (int index = 0; index <= 4; index++)
                {
                    GameObject blade = Instantiate(myManager.skillProjectiles[0], transform.position + Vector3.up + transform.forward, transform.rotation);
                    blade.transform.LookAt(target);

                    Vector3 rotation = blade.transform.rotation.eulerAngles;
                    rotation.y += (index - 2) * 8;
                    rotation.x -= 3;
                    blade.transform.rotation = Quaternion.Euler(rotation);

                    blade.GetComponent<HitBox>().damage = myManager.stats.baseDamage * 0.33f;
                    blade.GetComponent<HitBox>().myStats = myManager.stats;
                }
            }

            yield return null;
        }

        pc.CheckForOtherLoseOfControlEffects();
    }

    // Used to cast blink strike.
    IEnumerator BlinkStrike()
    {
        anim.SetBool("BlinkStrike", true);
        float targetTimer = 0.5f / myManager.stats.attackSpeed;
        float currentTimer = 0;

        pc.playerState = PlayerMovementController.PlayerState.CastingNoMovement;
        pc.SnapToFaceCamera();

        myManager.hitBoxes.hitboxes[6].GetComponent<HitBox>().damage = stats.baseDamage * 4f;
        myManager.hitBoxes.PlayParticles(9);
        myManager.hitBoxes.PlayParticles(10);

        // Create a vector that houses our move inputs
        Vector3 desiredMoveDirection = transform.forward.normalized;
        desiredMoveDirection.y = 0;
        desiredMoveDirection = desiredMoveDirection.normalized;

        // The dash portion of the dash.
        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;
            yield return null;

            myManager.characterController.Move(desiredMoveDirection * 20 * Time.deltaTime * myManager.stats.movespeedPercentMultiplier);

            if (CheckRayHit(14, new Ray(transform.position, transform.forward), 2) ||
                CheckRayHit(14, new Ray(transform.position + transform.right * 0.5f, transform.forward), 2) ||
                CheckRayHit(14, new Ray(transform.position + transform.right * -0.5f, transform.forward), 2))
                break;
        }

        myManager.hitBoxes.StopParticles(10);

        // This is the hit portion of the dash.
        currentTimer = 0;
        targetTimer = 0.8f / myManager.stats.attackSpeed;
        anim.SetBool("BlinkStrike", false);

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;
            yield return null;
        }

        pc.CheckForOtherLoseOfControlEffects();
    }
    // USed to cast the spell Tremor Stab

    IEnumerator TremorStab()
    {
        anim.SetTrigger("TremorStab");
        //anim.applyRootMotion = true;
        anim.SetFloat("AttackAnimSpeed", stats.attackSpeed);
        pc.SnapToFaceCamera();

        float targetTimer = 2.167f / stats.attackSpeed;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingNoMovement;

        myManager.hitBoxes.hitboxes[10].GetComponent<HitBox>().damage = myManager.stats.baseDamage * 3f;

        //Vector3 directionToMove = transform.forward;
        //float distance = 3;
        //float distancePerSecond = distance / targetTimer;

        while (currentTimer < targetTimer)
        {
            //pc.SkillMovement(directionToMove, distancePerSecond);
            currentTimer += Time.deltaTime;
            yield return null;
        }

        //anim.applyRootMotion = false;
        pc.CheckForOtherLoseOfControlEffects();
    }
    // USed to cast the spell Leap strike

    IEnumerator LeapStrike()
    {
        anim.SetTrigger("LeapStrike");
        //anim.applyRootMotion = true;
        anim.SetFloat("AttackAnimSpeed", stats.attackSpeed);
        pc.SnapToFaceCamera();

        float targetTimer = 0.467f / stats.attackSpeed;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingAerial;

        myManager.hitBoxes.hitboxes[11].GetComponent<HitBox>().damage = myManager.stats.baseDamage * 5f;

        Vector3 desiredMoveDirection = transform.forward.normalized;
        desiredMoveDirection.y = 0;
        desiredMoveDirection = desiredMoveDirection.normalized;

        while (currentTimer < targetTimer)
        {
            //pc.SkillMovement(directionToMove, distancePerSecond);
            currentTimer += Time.deltaTime;
            yield return null;
        }

        //wait until we hit the ground
        while(!anim.GetBool("Grounded"))
        {
            yield return null;
            myManager.characterController.Move(desiredMoveDirection * 15 * Time.deltaTime * myManager.stats.movespeedPercentMultiplier);
        }

        targetTimer = 0.75f / stats.attackSpeed;
        currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingWithMovement;

        while (currentTimer < targetTimer)
        {
            //pc.SkillMovement(directionToMove, distancePerSecond);
            currentTimer += Time.deltaTime;
            yield return null;
        }


        //anim.applyRootMotion = false;
        pc.CheckForOtherLoseOfControlEffects();
    }

    // USed to cast the spell takedown
    IEnumerator Takedown()
    {
        anim.SetTrigger("Takedown");
        //anim.applyRootMotion = true;
        anim.SetFloat("AttackAnimSpeed", stats.attackSpeed);
        pc.SnapToFaceCamera();

        float targetTimer = 1.65f / stats.attackSpeed;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingWithMovement;

        myManager.hitBoxes.hitboxes[12].GetComponent<HitBox>().damage = myManager.stats.baseDamage * 2f;

        //Vector3 directionToMove = transform.forward;
        //float distance = 3;
        //float distancePerSecond = distance / targetTimer;

        while (currentTimer < targetTimer)
        {
            //pc.SkillMovement(directionToMove, distancePerSecond);
            currentTimer += Time.deltaTime;
            yield return null;
        }

        //anim.applyRootMotion = false;
        pc.CheckForOtherLoseOfControlEffects();
    }

    // USed to cast the spell Impale
    IEnumerator Impale()
    {
        anim.SetTrigger("Impale");
        //anim.applyRootMotion = true;
        anim.SetFloat("AttackAnimSpeed", stats.attackSpeed);
        pc.SnapToFaceCamera();

        float targetTimer = 1.3f / stats.attackSpeed;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingWithMovement;

        myManager.hitBoxes.hitboxes[13].GetComponent<HitBox>().damage = myManager.stats.baseDamage * 4f;

        //Vector3 directionToMove = transform.forward;
        //float distance = 3;
        //float distancePerSecond = distance / targetTimer;

        while (currentTimer < targetTimer)
        {
            //pc.SkillMovement(directionToMove, distancePerSecond);
            currentTimer += Time.deltaTime;
            yield return null;
        }

        //anim.applyRootMotion = false;
        pc.CheckForOtherLoseOfControlEffects();
    }

    // USed to cast the spell Counter
    IEnumerator Counter()
    {
        anim.SetTrigger("Counter");
        //anim.applyRootMotion = true;
        anim.SetFloat("AttackAnimSpeed", stats.attackSpeed);
        pc.SnapToFaceCamera();

        float targetTimer = 0.917f * 0.4f;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingNoMovement;
        stats.counter = true;
        stats.counterDamage = 0;

        //Vector3 directionToMove = transform.forward;
        //float distance = 3;
        //float distancePerSecond = distance / targetTimer;

        while (currentTimer < targetTimer)
        {
            //pc.SkillMovement(directionToMove, distancePerSecond);
            currentTimer += Time.deltaTime;
            yield return null;
        }

        if (stats.counterDamage > 0)
            StartCoroutine(CounterStrike());
        else
            StartCoroutine(CounterEndLag());
    }

    // The endlag of counter if we didnt block anything
    IEnumerator CounterEndLag()
    {
        stats.counter = false;
        float targetTimer = 0.917f * 0.6f;
        float currentTimer = 0f;

        while (currentTimer < targetTimer)
        {
            //pc.SkillMovement(directionToMove, distancePerSecond);
            currentTimer += Time.deltaTime;
            yield return null;
        }

        //anim.applyRootMotion = false;
        pc.CheckForOtherLoseOfControlEffects();
    }

    IEnumerator CounterStrike()
    {
        anim.SetTrigger("CounterStrike");
        stats.counter = false;
        stats.AddInvulnerablitySource(1);
        float targetTimer = 1.6f / 4 / stats.attackSpeed;
        float currentTimer = 0f;

        float counterDamage = myManager.stats.baseDamage * 3f * (stats.counterDamage / (stats.baseDamage * 2));
        if (counterDamage < stats.baseDamage)
            counterDamage = stats.baseDamage;
        else if (counterDamage > stats.baseDamage * 20)
            counterDamage = stats.baseDamage * 20;
        Debug.Log("the damag absorbed is: " + stats.counterDamage + ". the damage we will deal is: " + counterDamage);

        myManager.hitBoxes.hitboxes[14].GetComponent<HitBox>().damage = counterDamage;

        while (currentTimer < targetTimer)
        {
            //pc.SkillMovement(directionToMove, distancePerSecond);
            currentTimer += Time.deltaTime;
            yield return null;
        }

        //anim.applyRootMotion = false;
        stats.AddInvulnerablitySource(-1);
        pc.CheckForOtherLoseOfControlEffects();
    }

    // Used to cast the Severing Strike spell, a giant slam attack that armor breaks enemies enemies
    IEnumerator SeveringStrike()
    {
        anim.SetTrigger("SeveringStrike");
        anim.SetFloat("AttackAnimSpeed", stats.attackSpeed);
        pc.SnapToFaceCamera();

        float targetTimer = 1f / stats.attackSpeed;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingWithMovement;

        myManager.hitBoxes.hitboxes[15].GetComponent<HitBox>().damage = myManager.stats.baseDamage * 4f;

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;
            yield return null;
        }

        pc.CheckForOtherLoseOfControlEffects();
    }

    // USed to cast the spell Whirlwind
    IEnumerator Whirlwind()
    {
        anim.SetBool("Whirlwind", true);
        anim.SetFloat("AttackAnimSpeed", stats.attackSpeed);
        pc.SnapToFaceCamera();

        float targetTimer = 5f;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingWithMovement;
        stats.movespeedPercentMultiplier -= 0.5f;
        myManager.stats.channeling = true;

        myManager.hitBoxes.hitboxes[16].GetComponent<HitBox>().damage = myManager.stats.baseDamage * 0.5f;

        while (currentTimer < targetTimer)
        {
            //pc.SkillMovement(directionToMove, distancePerSecond);
            currentTimer += Time.deltaTime;
            yield return null;
        }

        anim.SetBool("Whirlwind", false);
        stats.movespeedPercentMultiplier += 0.5f;
        myManager.stats.channeling = false;
        pc.CheckForOtherLoseOfControlEffects();
    }

    // USed to cast ShatteredEarth
    IEnumerator ShatteredEarth()
    {
        anim.SetBool("ShatteredEarth", true);
        pc.playerState = PlayerMovementController.PlayerState.CastingWithMovement;
        float currentTimeCharging = 0;
        float maximumChargeTime = 15f;
        float maxDamageChargeTime = 10f / myManager.stats.attackSpeed;
        bool maxDamage = false;
        stats.movespeedPercentMultiplier -= 0.5f;

        // begin the charging process.
        while (currentTimeCharging < maximumChargeTime)
        {
            currentTimeCharging += Time.deltaTime;
            if (!maxDamage && currentTimeCharging > maxDamageChargeTime)
            {
                maxDamage = true;
                myManager.hitBoxes.PlayParticles(25);
            }
            // Check for an attack input, if so release this attack.
            if (Input.GetAxisRaw(myManager.inputs.attackInput) == 1 && myManager.inputs.attackReleased)
                break;

            yield return null;
        }

        float chargePercent = currentTimeCharging / maxDamageChargeTime;
        // begin the SLAM.
        float currentTimer = 0;
        float targetTimer = 1.6f / 2 / stats.attackSpeed;
        anim.SetBool("ShatteredEarth", false);

        if (!maxDamage)
        {
            myManager.hitBoxes.hitboxes[17].GetComponent<HitBox>().damage = myManager.stats.baseDamage * 10f * chargePercent;
            myManager.hitBoxes.hitboxes[17].GetComponent<HitBox>().damageType = HitBox.DamageType.Physical;
        }
        else
        {
            myManager.hitBoxes.hitboxes[17].GetComponent<HitBox>().damage = myManager.stats.baseDamage * 15f;
            myManager.hitBoxes.hitboxes[17].GetComponent<HitBox>().damageType = HitBox.DamageType.True;
        }

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;
            yield return null;
        }

        stats.movespeedPercentMultiplier += 0.5f;

        pc.CheckForOtherLoseOfControlEffects();
    }

    //USed to cast the spell falling sword.
    IEnumerator FallingSword()
    {
        anim.SetTrigger("FallingSword");
        anim.SetFloat("AttackAnimSpeed", stats.attackSpeed);
        pc.SnapToFaceCamera();

        float targetTimer = 0.467f / stats.attackSpeed;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingAerial;

        myManager.hitBoxes.hitboxes[18].GetComponent<HitBox>().damage = myManager.stats.baseDamage * 2f;

        Vector3 desiredMoveDirection = transform.forward.normalized;
        desiredMoveDirection.y = 0;
        desiredMoveDirection = desiredMoveDirection.normalized;

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;
            yield return null;
        }

        //wait until we hit the ground
        while (!anim.GetBool("Grounded"))
        {
            yield return null;
            myManager.characterController.Move(desiredMoveDirection * 8 * Time.deltaTime * myManager.stats.movespeedPercentMultiplier);
        }

        targetTimer = 1f / stats.attackSpeed;
        currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingWithMovement;

        while (currentTimer < targetTimer)
        {
            //pc.SkillMovement(directionToMove, distancePerSecond);
            currentTimer += Time.deltaTime;
            yield return null;
        }

        pc.CheckForOtherLoseOfControlEffects();
    }

    // USed to cast the spell Senate Slash
    IEnumerator SenateSlash()
    {
        anim.SetTrigger("SenateSlash");
        //anim.applyRootMotion = true;
        anim.SetFloat("AttackAnimSpeed", stats.attackSpeed);
        pc.SnapToFaceCamera();

        float targetTimer = 1.3f / stats.attackSpeed;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingNoMovement;

        myManager.hitBoxes.hitboxes[19].GetComponent<HitBox>().damage = myManager.stats.baseDamage * 15f;

        Vector3 desiredMoveDirection = transform.forward.normalized;
        desiredMoveDirection.y = 0;
        desiredMoveDirection = desiredMoveDirection.normalized;

        while (currentTimer < targetTimer)
        {
            if(currentTimer / targetTimer >= 0.2f)
            myManager.characterController.Move(desiredMoveDirection * 8 * Time.deltaTime * myManager.stats.attackSpeed);
            //pc.SkillMovement(directionToMove, distancePerSecond);
            currentTimer += Time.deltaTime;
            yield return null;
        }

        //anim.applyRootMotion = false;
        pc.CheckForOtherLoseOfControlEffects();
    }




    // USed to cast the spell firebolt
    IEnumerator Firebolt()
    {
        anim.SetTrigger("Firebolt");
        anim.SetFloat("AttackAnimSpeed", stats.attackSpeed);
        pc.SnapToFaceCamera();

        float targetTimer = 1.25f / stats.attackSpeed;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingWithMovement;

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;

            yield return null;
        }

        pc.CheckForOtherLoseOfControlEffects();
    }

    // USed to cast the spell Ignition
    IEnumerator Ignition()
    {
        anim.SetBool("Ignition", true);
        anim.SetFloat("AttackAnimSpeed", stats.attackSpeed);
        pc.SnapToFaceCamera();
        myManager.stats.channeling = true;

        float targetTimer = 5f;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingWithMovement;
        stats.movespeedPercentMultiplier -= 0.2f;

        myManager.hitBoxes.hitboxes[20].GetComponent<HitBox>().damage = myManager.stats.baseDamage * 0.5f;

        while (currentTimer < targetTimer)
        {
            //pc.SkillMovement(directionToMove, distancePerSecond);
            currentTimer += Time.deltaTime;
            yield return null;
        }

        anim.SetBool("Ignition", false);
        stats.movespeedPercentMultiplier += 0.2f;
        myManager.stats.channeling = false;
        pc.CheckForOtherLoseOfControlEffects();
    }

    // Used to use the Emboldening Embers spell, an AoE buff for all allies.
    IEnumerator EmboldeningEmbers()
    {
        anim.SetTrigger("EmboldeningEmbers");
        float targetTimer = 0.767f / stats.attackSpeed;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingNoMovement;
        anim.SetFloat("Speed", 0);
        myManager.hitBoxes.hitboxes[7].GetComponent<HitBox>().damage = myManager.stats.healthMax / 4f;

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;
            yield return null;
        }

        pc.CheckForOtherLoseOfControlEffects();
    }

    // USed to cast the spell firebeads
    IEnumerator Firebeads()
    {
        anim.SetTrigger("Firebeads");
        anim.SetFloat("AttackAnimSpeed", stats.attackSpeed);
        pc.SnapToFaceCamera();

        float targetTimer = .833f / stats.attackSpeed;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingWithMovement;

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;

            yield return null;
        }

        pc.CheckForOtherLoseOfControlEffects();
    }

    // USed to cast the spell heatPulse
    IEnumerator HeatPulse()
    {
        anim.SetTrigger("HeatPulse");
        anim.SetFloat("AttackAnimSpeed", stats.attackSpeed);
        pc.SnapToFaceCamera();

        myManager.hitBoxes.hitboxes[21].GetComponent<HitBox>().damage = myManager.stats.baseDamage * 3f;

        float targetTimer = 1f / stats.attackSpeed;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingWithMovement;

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;

            yield return null;
        }

        pc.CheckForOtherLoseOfControlEffects();
    }

    //USed to cast FlameStrike
    IEnumerator FlameStrike()
    {
        anim.SetTrigger("FlameStrike");
        anim.SetFloat("AttackAnimSpeed", stats.attackSpeed);
        pc.SnapToFaceCamera();

        float targetTimer = 1.333f / stats.attackSpeed;
        float currentTimer = 0;

        pc.playerState = PlayerMovementController.PlayerState.CastingWithMovement;

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;
            yield return null;
        }

        pc.CheckForOtherLoseOfControlEffects();
    }

    //USed to cast FlameWalker
    IEnumerator FlameWalker()
    {
        anim.SetTrigger("Flamewalker");
        anim.SetFloat("AttackAnimSpeed", stats.attackSpeed);
        pc.SnapToFaceCamera();

        float targetTimer = 1.267f / stats.attackSpeed;
        float currentTimer = 0;

        pc.playerState = PlayerMovementController.PlayerState.CastingWithMovement;


        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;
            yield return null;
        }

        pc.CheckForOtherLoseOfControlEffects();
    }

    // USed to cast the spell witchpyre
    IEnumerator WitchPyre()
    {
        anim.SetTrigger("WitchPyre");
        anim.SetFloat("AttackAnimSpeed", stats.attackSpeed);
        pc.SnapToFaceCamera();

        float targetTimer = 1.75f / stats.attackSpeed;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingWithMovement;

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;

            yield return null;
        }

        pc.CheckForOtherLoseOfControlEffects();
    }

    // USed to cast the spell combustion
    IEnumerator Combustion()
    {
        anim.SetTrigger("Combustion");
        anim.SetFloat("AttackAnimSpeed", stats.attackSpeed);
        pc.SnapToFaceCamera();

        float targetTimer = 1.833f / stats.attackSpeed;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingWithMovement;

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;

            yield return null;
        }

        pc.CheckForOtherLoseOfControlEffects();
    }

    // USed to cast the spell ring of fire
    IEnumerator RingOfFire()
    {
        anim.SetTrigger("RingOfFire");
        anim.SetFloat("AttackAnimSpeed", stats.attackSpeed);
        pc.SnapToFaceCamera();

        float targetTimer = 1.85f / stats.attackSpeed;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingWithMovement;

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;

            yield return null;
        }

        pc.CheckForOtherLoseOfControlEffects();
    }

    // USed to cast the spell blessing of flames
    IEnumerator BlessingOfFlames()
    {
        anim.SetTrigger("BlessingOfFlames");
        anim.SetFloat("AttackAnimSpeed", stats.attackSpeed);
        pc.SnapToFaceCamera();

        float targetTimer = 1.25f / stats.attackSpeed;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingWithMovement;

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;

            yield return null;
        }

        pc.CheckForOtherLoseOfControlEffects();
    }

    // USed to cast the spell Immolate
    IEnumerator Immolate()
    {
        anim.SetTrigger("Immolate");
        anim.SetFloat("AttackAnimSpeed", stats.attackSpeed);
        pc.SnapToFaceCamera();

        float targetTimer = 1.583f / stats.attackSpeed;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingWithMovement;

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;

            yield return null;
        }

        pc.CheckForOtherLoseOfControlEffects();
    }

    // USed to cast the spell Firestorm
    IEnumerator Firestorm()
    {
        anim.SetTrigger("Firestorm");
        anim.SetFloat("AttackAnimSpeed", stats.attackSpeed);
        pc.SnapToFaceCamera();

        float targetTimer = 2.367f / stats.attackSpeed;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingWithMovement;

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;

            yield return null;
        }

        pc.CheckForOtherLoseOfControlEffects();
    }

    // USed to cast the spell Fireweave
    IEnumerator Fireweave()
    {
        anim.SetTrigger("Fireweave");
        anim.SetFloat("AttackAnimSpeed", stats.attackSpeed);
        pc.SnapToFaceCamera();

        float targetTimer = 1.667f / stats.attackSpeed;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingWithMovement;

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;

            yield return null;
        }

        pc.CheckForOtherLoseOfControlEffects();
    }

    // USed to cast the spell fireball
    IEnumerator Fireball()
    {
        anim.SetTrigger("Fireball");
        anim.SetFloat("AttackAnimSpeed", stats.attackSpeed);
        pc.SnapToFaceCamera();

        float targetTimer = 1.583f / stats.attackSpeed;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingWithMovement;

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;

            yield return null;
        }

        pc.CheckForOtherLoseOfControlEffects();
    }




    // USed to cast the spell icespike
    IEnumerator IceSpike()
    {
        anim.SetTrigger("IceSpike");
        anim.SetFloat("AttackAnimSpeed", stats.attackSpeed);
        pc.SnapToFaceCamera();

        float targetTimer = 0.75f * 2 / stats.attackSpeed;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingWithMovement;

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;

            yield return null;
        }

        pc.CheckForOtherLoseOfControlEffects();
    }

    // USed to cast the spell iceShards
    IEnumerator IceShards()
    {
        anim.SetTrigger("IceShards");
        anim.SetFloat("AttackAnimSpeed", stats.attackSpeed);
        pc.SnapToFaceCamera();

        float targetTimer = 0.667f * 2/ stats.attackSpeed;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingWithMovement;

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;

            yield return null;
        }

        pc.CheckForOtherLoseOfControlEffects();
    }

    // USed to cast the spell harsh Winds
    IEnumerator HarshWinds()
    {
        anim.SetTrigger("HarshWinds");
        anim.SetFloat("AttackAnimSpeed", stats.attackSpeed);
        pc.SnapToFaceCamera();

        float targetTimer = 0.9f / stats.attackSpeed;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingWithMovement;

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;

            yield return null;
        }

        pc.CheckForOtherLoseOfControlEffects();
    }

    // USed to cast the spell IcicleBarrage
    IEnumerator IcicleBarrage()
    {
        anim.SetBool("IcicleBarrage", true);
        anim.SetFloat("AttackAnimSpeed", stats.attackSpeed);
        pc.SnapToFaceCamera();
        myManager.stats.channeling = true;

        float targetTimer = 5f;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingWithMovement;
        stats.movespeedPercentMultiplier -= 0.2f;

        while (currentTimer < targetTimer)
        {
            //pc.SkillMovement(directionToMove, distancePerSecond);
            currentTimer += Time.deltaTime;
            yield return null;
        }

        anim.SetBool("IcicleBarrage", false);
        stats.movespeedPercentMultiplier += 0.2f;
        myManager.stats.channeling = false;
        pc.CheckForOtherLoseOfControlEffects();
    }

    // USed to cast the spell frozen barricade
    IEnumerator FrozenBarricade()
    {
        anim.SetTrigger("FrozenBarricade");
        anim.SetFloat("AttackAnimSpeed", stats.attackSpeed);
        pc.SnapToFaceCamera();

        float targetTimer = 1.183f / stats.attackSpeed;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingWithMovement;

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;

            yield return null;
        }

        pc.CheckForOtherLoseOfControlEffects();
    }

    // USed to cast the spell ice javelin
    IEnumerator IceJavelin()
    {
        anim.SetTrigger("IceJavelin");
        anim.SetFloat("AttackAnimSpeed", stats.attackSpeed);
        pc.SnapToFaceCamera();

        float targetTimer = 1.233f / stats.attackSpeed;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingWithMovement;

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;

            yield return null;
        }

        pc.CheckForOtherLoseOfControlEffects();
    }

    // USed to cast the spell glacier
    IEnumerator Glacier()
    {
        anim.SetTrigger("Glacier");
        anim.SetFloat("AttackAnimSpeed", stats.attackSpeed);
        pc.SnapToFaceCamera();

        float targetTimer = 1.183f / stats.attackSpeed;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingWithMovement;

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;

            yield return null;
        }

        pc.CheckForOtherLoseOfControlEffects();
    }

    // USed to cast the spell frost nova
    IEnumerator FrostNova()
    {
        anim.SetTrigger("FrostNova");
        anim.SetFloat("AttackAnimSpeed", stats.attackSpeed);
        pc.SnapToFaceCamera();

        myManager.hitBoxes.hitboxes[23].GetComponent<HitBox>().damage = myManager.stats.baseDamage * 3f;

        float targetTimer = 1.55f / stats.attackSpeed;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingWithMovement;

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;

            yield return null;
        }

        pc.CheckForOtherLoseOfControlEffects();
    }

    // USed to cast the spell frosts Kiss
    IEnumerator FrostsKiss()
    {
        anim.SetTrigger("FrostsKiss");
        anim.SetFloat("AttackAnimSpeed", stats.attackSpeed);
        pc.SnapToFaceCamera();

        float targetTimer = 1.583f / stats.attackSpeed;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingWithMovement;

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;

            yield return null;
        }

        pc.CheckForOtherLoseOfControlEffects();
    }

    // USed to cast the spell blizzard
    IEnumerator Blizzard()
    {
        anim.SetTrigger("Blizzard");
        anim.SetFloat("AttackAnimSpeed", stats.attackSpeed);
        pc.SnapToFaceCamera();

        float targetTimer = 1.617f / stats.attackSpeed;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingWithMovement;

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;

            yield return null;
        }

        pc.CheckForOtherLoseOfControlEffects();
    }

    // USed to cast the spell Ice Artillery
    IEnumerator IceArtillery()
    {
        anim.SetTrigger("IceArtillery");
        anim.SetFloat("AttackAnimSpeed", stats.attackSpeed);
        pc.SnapToFaceCamera();

        float targetTimer = 1.333f / stats.attackSpeed;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingWithMovement;

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;

            yield return null;
        }

        pc.CheckForOtherLoseOfControlEffects();
    }

    // USed to cast the spell Ray of ice
    IEnumerator RayOfIce()
    {
        anim.SetBool("RayOfIce", true);
        anim.SetFloat("AttackAnimSpeed", stats.attackSpeed);
        pc.SnapToFaceCamera();
        myManager.stats.channeling = true;
        stats.movespeedPercentMultiplier -= 0.5f;

        float targetTimer = 5f;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingWithMovement;

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;

            yield return null;
        }

        stats.movespeedPercentMultiplier += 0.5f;
        myManager.stats.channeling = false;
        anim.SetBool("RayOfIce", false);
        pc.CheckForOtherLoseOfControlEffects();
    }

    // USed to cast the spell Ice Armor
    IEnumerator IceArmor()
    {
        anim.SetTrigger("IceArmor");
        anim.SetFloat("AttackAnimSpeed", stats.attackSpeed);
        pc.SnapToFaceCamera();

        float targetTimer = 1.417f / stats.attackSpeed;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingWithMovement;

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;

            yield return null;
        }

        pc.CheckForOtherLoseOfControlEffects();
    }

    // USed to cast the spell Absolute Zero
    IEnumerator AbsoluteZero()
    {
        anim.SetTrigger("AbsoluteZero");
        anim.SetFloat("AttackAnimSpeed", stats.attackSpeed);
        pc.SnapToFaceCamera();

        myManager.hitBoxes.hitboxes[24].GetComponent<HitBox>().damage = myManager.stats.baseDamage * 8f;

        float targetTimer = 2f / stats.attackSpeed;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingWithMovement;

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;

            yield return null;
        }

        pc.CheckForOtherLoseOfControlEffects();
    }

    // USed to cast the spell Spell Mirror
    IEnumerator SpellMirror()
    {
        anim.SetTrigger("SpellMirror");
        anim.SetFloat("AttackAnimSpeed", stats.attackSpeed);
        pc.SnapToFaceCamera();

        float targetTimer = 1.3f / stats.attackSpeed;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingWithMovement;

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;

            yield return null;
        }

        pc.CheckForOtherLoseOfControlEffects();
    }



    // USed to cast the spell earthern spear
    IEnumerator EarthernSpears()
    {
        anim.SetTrigger("EarthernSpears");
        anim.SetFloat("AttackAnimSpeed", stats.attackSpeed);
        pc.SnapToFaceCamera();

        float targetTimer = 1f / stats.attackSpeed;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingWithMovement;

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;

            yield return null;
        }

        pc.CheckForOtherLoseOfControlEffects();
    }

    // USed to cast the spell earthern urchin
    IEnumerator EarthernUrchin()
    {
        anim.SetTrigger("EarthernUrchin");
        anim.SetFloat("AttackAnimSpeed", stats.attackSpeed);
        pc.SnapToFaceCamera();

        float targetTimer = 1f / stats.attackSpeed;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingWithMovement;

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;

            yield return null;
        }

        pc.CheckForOtherLoseOfControlEffects();
    }

    // USed to cast the spell idol of tremors
    IEnumerator IdolOfTremors()
    {
        anim.SetTrigger("IdolOfTremors");
        anim.SetFloat("AttackAnimSpeed", stats.attackSpeed);
        pc.SnapToFaceCamera();

        float targetTimer = 0.75f / stats.attackSpeed;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingWithMovement;

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;

            yield return null;
        }

        pc.CheckForOtherLoseOfControlEffects();
    }

    // USed to cast the spell boulder fist
    IEnumerator BoulderFist()
    {
        anim.SetTrigger("BoulderFist");
        anim.SetFloat("AttackAnimSpeed", stats.attackSpeed);
        pc.SnapToFaceCamera();

        float targetTimer = 1f / stats.attackSpeed;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingWithMovement;

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;

            yield return null;
        }

        pc.CheckForOtherLoseOfControlEffects();
    }

    // USed to cast the spell stone strike
    IEnumerator StoneStrike()
    {
        anim.SetTrigger("StoneStrike");
        anim.SetFloat("AttackAnimSpeed", stats.attackSpeed);
        pc.SnapToFaceCamera();

        float targetTimer = 1.483f / stats.attackSpeed;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingWithMovement;

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;

            yield return null;
        }

        pc.CheckForOtherLoseOfControlEffects();
    }

    // USed to cast the spell Eathern Plateau
    IEnumerator EarthernPlateau()
    {
        anim.SetTrigger("EarthernPlateau");
        anim.SetFloat("AttackAnimSpeed", stats.attackSpeed);
        pc.SnapToFaceCamera();

        float targetTimer = 1.183f / stats.attackSpeed;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingWithMovement;

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;

            yield return null;
        }

        pc.CheckForOtherLoseOfControlEffects();
    }

    // USed to cast the spell Giant Strength
    IEnumerator GiantStrength()
    {
        anim.SetTrigger("GiantsStrength");
        anim.SetFloat("AttackAnimSpeed", stats.attackSpeed);
        pc.SnapToFaceCamera();

        float targetTimer = 1.05f / stats.attackSpeed;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingWithMovement;

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;

            yield return null;
        }

        pc.CheckForOtherLoseOfControlEffects();
    }

    // USed to cast the spell Rock Shot
    IEnumerator RockShot()
    {
        anim.SetTrigger("RockShot");
        anim.SetFloat("AttackAnimSpeed", stats.attackSpeed);
        pc.SnapToFaceCamera();

        float targetTimer = 1.633f / stats.attackSpeed;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingWithMovement;

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;

            yield return null;
        }

        pc.CheckForOtherLoseOfControlEffects();
    }

    // USed to cast the spell Stalagmite Smash
    IEnumerator StalagmiteSmash()
    {
        anim.SetTrigger("StalagmiteSmash");
        anim.SetFloat("AttackAnimSpeed", stats.attackSpeed);
        pc.SnapToFaceCamera();

        float targetTimer = 1.217f / stats.attackSpeed;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingWithMovement;

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;

            yield return null;
        }

        pc.CheckForOtherLoseOfControlEffects();
    }

    // USed to cast the spell Unstable Earth
    IEnumerator UnstableEarth()
    {
        anim.SetTrigger("UnstableEarth");
        anim.SetFloat("AttackAnimSpeed", stats.attackSpeed);
        pc.SnapToFaceCamera();

        float targetTimer = 1f / stats.attackSpeed;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingWithMovement;

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;

            yield return null;
        }

        pc.CheckForOtherLoseOfControlEffects();
    }

    // USed to cast the spell Tremorfall
    IEnumerator Tremorfall()
    {
        anim.SetTrigger("Tremorfall");
        anim.SetFloat("AttackAnimSpeed", stats.attackSpeed);
        pc.SnapToFaceCamera();

        float targetTimer = 1.167f / 2 / stats.attackSpeed;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingAerial;

        while (anim.GetBool("Grounded") == false)
        {
            yield return null;
        }

        pc.playerState = PlayerMovementController.PlayerState.CastingNoMovement;

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;

            yield return null;
        }

        pc.CheckForOtherLoseOfControlEffects();
    }

    // USed to cast the spell GaiasCyclone
    IEnumerator GaiasCyclone()
    {
        anim.SetTrigger("GaiasCyclone");
        anim.SetFloat("AttackAnimSpeed", stats.attackSpeed);
        pc.SnapToFaceCamera();

        float targetTimer = 0.833f / stats.attackSpeed;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingWithMovement;

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;

            yield return null;
        }

        pc.CheckForOtherLoseOfControlEffects();
    }

    // USed to cast the spell Cave In
    IEnumerator CaveIn()
    {
        anim.SetTrigger("CaveIn");
        anim.SetFloat("AttackAnimSpeed", stats.attackSpeed);
        pc.SnapToFaceCamera();

        float targetTimer = 2.367f / stats.attackSpeed;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingWithMovement;

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;

            yield return null;
        }

        pc.CheckForOtherLoseOfControlEffects();
    }

    // USed to cast the spell Stone Prison
    IEnumerator StonePrison()
    {
        anim.SetTrigger("StonePrison");
        anim.SetFloat("AttackAnimSpeed", stats.attackSpeed);
        pc.SnapToFaceCamera();

        float targetTimer = 1.3f / stats.attackSpeed;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingWithMovement;

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;

            yield return null;
        }

        pc.CheckForOtherLoseOfControlEffects();
    }

    // USed to cast the spell Earthquake
    IEnumerator Earthquake()
    {
        anim.SetTrigger("Earthquake");
        anim.SetFloat("AttackAnimSpeed", stats.attackSpeed);
        pc.SnapToFaceCamera();

        float targetTimer = 1.617f / stats.attackSpeed;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingWithMovement;

        myManager.hitBoxes.hitboxes[25].GetComponent<HitBox>().damage = myManager.stats.baseDamage * 3f;

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;

            yield return null;
        }

        pc.CheckForOtherLoseOfControlEffects();
    }




    // USed to cast the spell Air gust
    IEnumerator Airgust()
    {
        anim.SetTrigger("Airgust");
        anim.SetFloat("AttackAnimSpeed", stats.attackSpeed);
        pc.SnapToFaceCamera();

        float targetTimer = 1.25f / stats.attackSpeed;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingWithMovement;

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;

            yield return null;
        }

        pc.CheckForOtherLoseOfControlEffects();
    }

    // USed to cast the spell Second Wind
    IEnumerator SecondWind()
    {
        anim.SetTrigger("SecondWind");
        anim.SetFloat("AttackAnimSpeed", stats.attackSpeed);
        pc.SnapToFaceCamera();

        float targetTimer = 0.767f / stats.attackSpeed;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingWithMovement;

        myManager.hitBoxes.buffboxes[10].GetComponent<HitBox>().damage = myManager.stats.healthMax * 0.33f;

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;

            yield return null;
        }

        pc.CheckForOtherLoseOfControlEffects();
    }

    // USed to cast the spell Airblade
    IEnumerator Airblade()
    {
        anim.SetTrigger("Airblade");
        anim.SetFloat("AttackAnimSpeed", stats.attackSpeed);
        pc.SnapToFaceCamera();

        float targetTimer = 0.467f / stats.attackSpeed;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingWithMovement;

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;

            yield return null;
        }

        pc.CheckForOtherLoseOfControlEffects();
    }

    // USed to cast the spell Aeroslash
    IEnumerator Aeroslash()
    {
        anim.SetTrigger("Aeroslash");
        anim.SetFloat("AttackAnimSpeed", stats.attackSpeed);
        pc.SnapToFaceCamera();

        float targetTimer = 0.75f / stats.attackSpeed;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingWithMovement;

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;

            yield return null;
        }

        pc.CheckForOtherLoseOfControlEffects();
    }

    // USed to cast the spell Aeroburst
    IEnumerator Aeroburst()
    {
        anim.SetTrigger("Aeroburst");
        anim.SetFloat("AttackAnimSpeed", stats.attackSpeed);
        pc.SnapToFaceCamera();

        float targetTimer = 1.55f / stats.attackSpeed;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingWithMovement;

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;

            yield return null;
        }

        pc.CheckForOtherLoseOfControlEffects();
    }

    // USed to cast the spell WrathOfTheWind
    IEnumerator WrathOfTheWind()
    {
        anim.SetTrigger("WrathOfTheWind");
        anim.SetFloat("AttackAnimSpeed", stats.attackSpeed);
        pc.SnapToFaceCamera();

        float targetTimer = 1.583f / stats.attackSpeed;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingWithMovement;

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;

            yield return null;
        }

        pc.CheckForOtherLoseOfControlEffects();
    }

    // USed to cast the spell OrbOfShredding
    IEnumerator OrbOfShredding()
    {
        anim.SetTrigger("OrbOfShredding");
        anim.SetFloat("AttackAnimSpeed", stats.attackSpeed);
        pc.SnapToFaceCamera();

        float targetTimer = 1.333f / stats.attackSpeed;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingWithMovement;

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;

            yield return null;
        }

        pc.CheckForOtherLoseOfControlEffects();
    }

    // USed to cast the spell Multislash
    IEnumerator Multislash()
    {
        anim.SetTrigger("Multislash");
        anim.SetFloat("AttackAnimSpeed", stats.attackSpeed);
        pc.SnapToFaceCamera();

        float targetTimer = 1.483f / stats.attackSpeed;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingWithMovement;

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;

            yield return null;
        }

        pc.CheckForOtherLoseOfControlEffects();
    }

    // USed to cast the spell aerolaunch
    IEnumerator Aerolaunch()
    {
        anim.SetTrigger("Aerolaunch");
        anim.SetFloat("AttackAnimSpeed", stats.attackSpeed);
        pc.SnapToFaceCamera();

        float targetTimer = 1f / stats.attackSpeed;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingAerial;
        //pc.StartCoroutine(pc.Jump(0.2f, 0.3f, false));

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;
            yield return null;
        }

        pc.CheckForOtherLoseOfControlEffects();
    }

    // USed to cast the spell WhirlwindSlash
    IEnumerator WhirlwindSlash()
    {
        anim.SetBool("WhirlwindSlash", true);
        anim.SetFloat("AttackAnimSpeed", stats.attackSpeed);
        pc.SnapToFaceCamera();

        float targetTimer = 5f;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingWithMovement;
        stats.movespeedPercentMultiplier -= 0.5f;
        myManager.stats.channeling = true;

        while (currentTimer < targetTimer)
        {
            //pc.SkillMovement(directionToMove, distancePerSecond);
            currentTimer += Time.deltaTime;
            yield return null;
        }

        anim.SetBool("WhirlwindSlash", false);
        stats.movespeedPercentMultiplier += 0.5f;
        myManager.stats.channeling = false;
        pc.CheckForOtherLoseOfControlEffects();
    }

    // USed to cast the spell Aerobarrage
    IEnumerator Aerobarrage()
    {
        anim.SetBool("Aerobarrage", true);
        anim.SetFloat("AttackAnimSpeed", stats.attackSpeed);
        pc.SnapToFaceCamera();
        myManager.stats.channeling = true;

        float targetTimer = 5f;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingWithMovement;
        stats.movespeedPercentMultiplier -= 0.2f;

        while (currentTimer < targetTimer)
        {
            //pc.SkillMovement(directionToMove, distancePerSecond);
            currentTimer += Time.deltaTime;
            yield return null;
        }

        anim.SetBool("Aerobarrage", false);
        stats.movespeedPercentMultiplier += 0.2f;
        myManager.stats.channeling = false;
        pc.CheckForOtherLoseOfControlEffects();
    }

    // USed to cast the spell PressureDrop
    IEnumerator PressureDrop()
    {
        anim.SetTrigger("PressureDrop");
        anim.SetFloat("AttackAnimSpeed", stats.attackSpeed);
        pc.SnapToFaceCamera();

        float targetTimer = 1.667f / stats.attackSpeed;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingWithMovement;

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;

            yield return null;
        }

        pc.CheckForOtherLoseOfControlEffects();
    }

    // USed to cast the spell TwinTwisters
    IEnumerator TwinTwisters()
    {
        anim.SetTrigger("TwinTwisters");
        anim.SetFloat("AttackAnimSpeed", stats.attackSpeed);
        pc.SnapToFaceCamera();

        float targetTimer = 0.833f / stats.attackSpeed;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingWithMovement;

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;

            yield return null;
        }

        pc.CheckForOtherLoseOfControlEffects();
    }

    // USed to cast the spell Vortex
    IEnumerator Vortex()
    {
        anim.SetTrigger("Vortex");
        anim.SetFloat("AttackAnimSpeed", stats.attackSpeed);
        pc.SnapToFaceCamera();

        float targetTimer = 1.617f / stats.attackSpeed;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingWithMovement;

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;

            yield return null;
        }

        pc.CheckForOtherLoseOfControlEffects();
    }

    // USed to cast the spell GroundZero
    IEnumerator GroundZero()
    {
        anim.SetTrigger("GroundZero");
        anim.SetFloat("AttackAnimSpeed", stats.attackSpeed);
        pc.SnapToFaceCamera();

        float targetTimer = 2.167f / stats.attackSpeed;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingWithMovement;

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;

            yield return null;
        }

        pc.CheckForOtherLoseOfControlEffects();
    }

    //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    // USed to cast the spell nature pulse at the enemies
    IEnumerator NaturePulse()
    {
        anim.SetTrigger("BoulderFist");
        float targetTimer = 0.15f;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingWithMovement;

        //myManager.ps[27].Play();

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;
            yield return null;
        }

        bool targetSelected = false;
        targetIndicator = Instantiate(myManager.targetIndicatorCircle);
        while (!targetSelected)
        {
            // shoot a ray, and set the indicator toi the rays location.
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward * 100);
            RaycastHit hit;
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 100, Color.red);
            if (Physics.Raycast(ray, out hit, 100f, myManager.targettingRayMaskHitEnemies))
            {
                targetIndicator.transform.position = hit.point;
                targetIndicator.transform.rotation = Quaternion.Euler(new Vector3(hit.normal.z * 90, 0, hit.normal.x * -90));
            }

            string skillInput = GetInput();

            // If i attack launch the attack at the targtted position and continue the animation.
            if (Input.GetAxisRaw(myManager.inputs.attackInput) == 1 && myManager.inputs.attackReleased)
            {
                //myManager.ps[16].Play();
                GameObject naturePulse = Instantiate(myManager.skillProjectiles[3], targetIndicator.transform.position, targetIndicator.transform.rotation);
                naturePulse.GetComponent<HitBox>().damage = myManager.stats.baseDamage * 1.5f; 
                // 200% + 10% per int
                naturePulse.GetComponent<HitBox>().myStats = myManager.stats;
                anim.SetTrigger("ProjectileFired");
                Destroy(targetIndicator);
                //myManager.ps[28].Play();
                targetSelected = true;
            }

            yield return null;
        }

        //myManager.ps[27].Stop();
        anim.ResetTrigger("BoulderFist");
        pc.playerState = PlayerMovementController.PlayerState.Idle;
    }

    // USed to cast the skill killer instinct
    IEnumerator KillerInstinct()
    {
        anim.SetTrigger("EmboldeningEmbers");
        float targetTimer = 0.5f;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingRollOut;

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;
            yield return null;
        }

        //myManager.ps[38].Play();
        //myManager.ps[39].Play();
        //myManager.hitBoxes.LaunchBuffBox(6);
        //myManager.hitBoxes.buffboxes[6].GetComponent<HitBoxBuff>().BuffSelf();
        pc.playerState = PlayerMovementController.PlayerState.Idle;
    }

    // Used to cast the spell Toxic Ripple, an AoE poison and corrosive effect that affects everyone, but buffs your resistance to both for the duration
    IEnumerator ToxicRipple()
    {
        anim.SetTrigger("EmboldeningEmbers");
        float targetTimer = 0.5f;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingRollOut;
        //myManager.ps[35].Play();
        
        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;
            yield return null;
        }

        pc.playerState = PlayerMovementController.PlayerState.Idle;
        //myManager.ps[35].Stop();
        //myManager.ps[36].Play();
        //myManager.ps[37].Play();
        myManager.hitBoxes.LaunchBuffBox(5);
        myManager.hitBoxes.buffboxes[5].GetComponent<HitBoxBuff>().BuffSelf();
        currentTimer = 0;
        targetTimer = 10f;

        float damageToTake = myManager.stats.baseDamage * 0.2f;
        myManager.hitBoxes.hitboxes[6].GetComponent<HitBox>().damage = damageToTake;
        float currentTickTimer = 0;
        while(currentTimer < targetTimer)
        {
            currentTickTimer += Time.deltaTime;
            currentTimer += Time.deltaTime;
            if(currentTickTimer > targetTimer / 20)
            {
                currentTickTimer -= targetTimer / 20;
                myManager.hitBoxes.LaunchHitBox(6);
                //myManager.hitBoxes.buffboxes[7].GetComponent<HitBoxBuff>().AfflictSelf();
                myManager.stats.TakeDamage(damageToTake / 2, false, HitBox.DamageType.Physical, 0);
            }
            yield return null;
        }
        //myManager.ps[36].Stop();
        //myManager.ps[37].Stop();

    }

    // Used to cast Casutic Edge, a three hit strike that applies poision and deals weapon damage.
    IEnumerator CausticEdge()
    {
        anim.SetTrigger("CausticEdge");
        float targetTimer = 0.3f / myManager.stats.attackSpeed;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingWithMovement;
        
        while(currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;
            yield return null;
        }
        myManager.hitBoxes.hitboxes[4].GetComponent<HitBox>().damage = myManager.stats.baseDamage * 1f;
        myManager.hitBoxes.LaunchHitBox(4);
        //myManager.ps[29].Play();
        //myManager.ps[30].Play();
        currentTimer = 0;
        targetTimer = 0.22f;
        
        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;
            yield return null;
        }

        myManager.hitBoxes.LaunchHitBox(4);
        //myManager.ps[31].Play();
        //myManager.ps[32].Play();
        currentTimer = 0;
        targetTimer = 0.5f / myManager.stats.attackSpeed;
        
        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;
            yield return null;
        }
        myManager.hitBoxes.hitboxes[5].GetComponent<HitBox>().damage = myManager.stats.baseDamage * 3f;
        myManager.hitBoxes.LaunchHitBox(5);
        //myManager.ps[33].Play();
        //myManager.ps[34].Play();

        pc.playerState = PlayerMovementController.PlayerState.Idle;
    }

    // Used to cast aspect of rage, a buff that only applies to this chgaracter that raises attack power signifcantly.
    IEnumerator AspectOfRage()
    {
        anim.SetTrigger("AspectOfRage");
        float targetTimer = 1f;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingWithMovement;
        //myManager.ps[17].Play();
        bool playParticles = false;

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;
            if (!playParticles && currentTimer > targetTimer * 0.5f)
            {
                //myManager.ps[16].Play();
                myManager.hitBoxes.LaunchBuffBox(2);
                myManager.hitBoxes.buffboxes[2].GetComponent<HitBoxBuff>().BuffSelf();
                playParticles = true;
            }
            yield return null;
        }

        //myManager.ps[17].Stop();
        pc.playerState = PlayerMovementController.PlayerState.Idle;
    }


    // USed by abiltiies to cast a ray and returns true if it hit an object in the layer in question.
    private bool CheckRayHit(int layerToCheck, Ray ray, float length)
    {
        bool rayHitObject = false;
        RaycastHit rayHit;

        Debug.DrawRay(ray.origin, ray.direction * length, Color.red);
        Debug.Log("Shooting The Ray on layer: " + (1 << layerToCheck));
        if(Physics.Raycast(ray, out rayHit, length, 1 << layerToCheck))
        {
            if (rayHit.collider.gameObject.CompareTag("Enemy"))
                Debug.Log("WE hit an enemy");
            rayHitObject = true;
        }

        return rayHitObject;
    }

    // Used to check what the input for this skill would be.
    private string GetInput()
    {
        string inputToReturn = null;
        
        switch (skillIndex)
        {
            case 0:
                inputToReturn = myManager.inputs.skill0Input;
                break;
            case 1:
                inputToReturn = myManager.inputs.skill1Input;
                break;
            case 2:
                inputToReturn = myManager.inputs.skill2Input;
                break;
            case 3:
                inputToReturn = myManager.inputs.skill3Input;
                break;
            case 4:
                inputToReturn = myManager.inputs.skill4Input;
                break;
            case 5:
                inputToReturn = myManager.inputs.skill5Input;
                break;
            case 6:
                inputToReturn = myManager.inputs.skill6Input;
                break;
            case 7:
                inputToReturn = myManager.inputs.skill7Input;
                break;
        }

        return inputToReturn;
    }
}
