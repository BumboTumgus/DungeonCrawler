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
    public float skillCost = 0;

    public BarManager connectedBar;
    public GameObject noManaOverlay;

    public int skillIndex = 0;

    public SkillsManager myManager;
    public Animator anim;
    public PlayerMovementController pc;
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
        if (skillReady)
        {
            Debug.Log("The skill " + skillName + " has been used");
            switch (skillName)
            {
                case SkillsManager.SkillNames.BlinkStrike:
                    StartCoroutine(BlinkStrike());
                    break;
                case SkillsManager.SkillNames.EmboldeningEmbers:
                    StartCoroutine(EmboldeningEmbers());
                    break;
                case SkillsManager.SkillNames.FlameStrike:
                    StartCoroutine(FlameStrike());
                    break;
                case SkillsManager.SkillNames.SeveringStrike:
                    StartCoroutine(SeveringStrike());
                    break;
                case SkillsManager.SkillNames.AspectOfRage:
                    StartCoroutine(AspectOfRage());
                    break;
                case SkillsManager.SkillNames.BlessingOfFlames:
                    StartCoroutine(BlessingOfFlames());
                    break;
                case SkillsManager.SkillNames.ShatteredEarth:
                    StartCoroutine(ShatteredEarth());
                    break;
                case SkillsManager.SkillNames.GiantStrength:
                    StartCoroutine(GiantStrength());
                    break;
                case SkillsManager.SkillNames.EarthernPlateau:
                    StartCoroutine(EarthernPlateau());
                    break;
                case SkillsManager.SkillNames.BoulderFist:
                    if (boulderFist != null)
                    {
                        if (targetIndicator != null)
                            Destroy(targetIndicator);
                        anim.ResetTrigger("BoulderFist");
                        anim.ResetTrigger("ProjectileFired");
                        StopCoroutine(boulderFist);
                    }
                    boulderFist = StartCoroutine(BoulderFist());
                    break;
                case SkillsManager.SkillNames.EarthernSpear:
                    if (earthernSpear != null)
                    {
                        if (targetIndicator != null)
                            Destroy(targetIndicator);
                        anim.ResetTrigger("EarthernSpear");
                        anim.ResetTrigger("ProjectileFired");
                        anim.SetBool("Multifire", false);
                        StopCoroutine(earthernSpear);
                    }
                    earthernSpear = StartCoroutine(EarthernSpear());
                    break;
                case SkillsManager.SkillNames.CausticEdge:
                    StartCoroutine(CausticEdge());
                    break;
                case SkillsManager.SkillNames.ToxicRipple:
                    StartCoroutine(ToxicRipple());
                    break;
                case SkillsManager.SkillNames.KillerInstinct:
                    StartCoroutine(KillerInstinct());
                    break;
                case SkillsManager.SkillNames.NaturePulse:
                    if (naturePulse != null)
                    {
                        if (targetIndicator != null)
                            Destroy(targetIndicator);
                        anim.ResetTrigger("BoulderFist");
                        anim.ResetTrigger("ProjectileFired");
                        StopCoroutine(naturePulse);
                    }
                    naturePulse = StartCoroutine(NaturePulse());
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

    // USed to cast the spell nature pulse at the enemies
    IEnumerator NaturePulse()
    {
        anim.SetTrigger("BoulderFist");
        float targetTimer = 0.15f;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingWithMovement;

        myManager.ps[27].Play();

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

        myManager.ps[27].Stop();
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

        myManager.ps[38].Play();
        myManager.ps[39].Play();
        myManager.hitBoxes.LaunchBuffBox(6);
        myManager.hitBoxes.buffboxes[6].GetComponent<HitBoxBuff>().BuffSelf();
        pc.playerState = PlayerMovementController.PlayerState.Idle;
    }

    // Used to cast the spell Toxic Ripple, an AoE poison and corrosive effect that affects everyone, but buffs your resistance to both for the duration
    IEnumerator ToxicRipple()
    {
        anim.SetTrigger("EmboldeningEmbers");
        float targetTimer = 0.5f;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingRollOut;
        myManager.ps[35].Play();
        
        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;
            yield return null;
        }

        pc.playerState = PlayerMovementController.PlayerState.Idle;
        myManager.ps[35].Stop();
        myManager.ps[36].Play();
        myManager.ps[37].Play();
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
                myManager.stats.TakeDamage(damageToTake / 2, false, HitBox.DamageType.Physical);
            }
            yield return null;
        }
        myManager.ps[36].Stop();
        myManager.ps[37].Stop();

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
        myManager.ps[29].Play();
        myManager.ps[30].Play();
        currentTimer = 0;
        targetTimer = 0.22f;
        
        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;
            yield return null;
        }

        myManager.hitBoxes.LaunchHitBox(4);
        myManager.ps[31].Play();
        myManager.ps[32].Play();
        currentTimer = 0;
        targetTimer = 0.5f / myManager.stats.attackSpeed;
        
        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;
            yield return null;
        }
        myManager.hitBoxes.hitboxes[5].GetComponent<HitBox>().damage = myManager.stats.baseDamage * 3f;
        myManager.hitBoxes.LaunchHitBox(5);
        myManager.ps[33].Play();
        myManager.ps[34].Play();

        pc.playerState = PlayerMovementController.PlayerState.Idle;
    }

    // Used to cast the spell earthern spear at the enemies.
    IEnumerator EarthernSpear()
    {
        Debug.Log("starting skill");
        anim.SetTrigger("EarthernSpear");
        float targetTimer = 0.25f;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingWithMovement;

        myManager.ps[27].Play();

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;
            yield return null;
        }

        anim.SetBool("Multifire", true);
        targetIndicator = Instantiate(myManager.targetIndicatorCircle);
        for (int index = 0; index < 3; index++)
        {
            bool targetSelected = false;
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

                //string skillInput = GetInput();

                // If i attack launch the attack at the targtted position and continue the animation.
                if (Input.GetAxisRaw(myManager.inputs.attackInput) == 1 && myManager.inputs.attackReleased)
                {
                    // Debug.Log("The input was pressed");
                    //myManager.ps[16].Play();
                    Debug.Log("we have shot a spear");
                    myManager.inputs.attackReleased = false;
                    GameObject earthernSpear = Instantiate(myManager.skillProjectiles[2], transform.position + new Vector3(0, 1, 0), transform.rotation);
                    earthernSpear.transform.LookAt(targetIndicator.transform.position);
                    earthernSpear.GetComponent<HitBox>().damage = myManager.stats.baseDamage * 1f;
                    earthernSpear.GetComponent<HitBox>().myStats = myManager.stats;
                    anim.SetTrigger("ProjectileFired");
                    myManager.ps[28].Play();
                    targetSelected = true;
                }

                yield return null;
            }
        }
        anim.SetBool("Multifire", false);
        anim.ResetTrigger("EarthernSpear");
        Destroy(targetIndicator);

        myManager.ps[27].Stop();
        pc.playerState = PlayerMovementController.PlayerState.Idle;

    }

    // USed to cast the spell boulder fist, a spell that summons a boulder fist at the target location, dealing damage.
    IEnumerator BoulderFist()
    {
        anim.SetTrigger("BoulderFist");
        float targetTimer = 0.25f;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingWithMovement;
        
        myManager.ps[27].Play();

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
            if(Physics.Raycast(ray, out hit, 100f, myManager.targettingRayMask))
            {
                targetIndicator.transform.position = hit.point;
                targetIndicator.transform.rotation = Quaternion.Euler(new Vector3(hit.normal.z * 90, 0, hit.normal.x * -90));
            }

            string skillInput = GetInput();

            // If i attack launch the attack at the targtted position and continue the animation.
            if (Input.GetAxisRaw(myManager.inputs.attackInput) == 1 && myManager.inputs.attackReleased)
            {
                Debug.Log("The input was pressed");
                //myManager.ps[16].Play();
                GameObject boulderFist = Instantiate(myManager.skillProjectiles[1], targetIndicator.transform.position, targetIndicator.transform.root.rotation);
                boulderFist.GetComponent<HitBoxTerrain>().damage = myManager.stats.baseDamage * 3f;
                anim.SetTrigger("ProjectileFired");
                Destroy(targetIndicator);
                myManager.ps[28].Play();
                targetSelected = true;
            }

            yield return null;
        }

        myManager.ps[27].Stop();
        anim.ResetTrigger("BoulderFist");
        pc.playerState = PlayerMovementController.PlayerState.Idle;

    }

    // Used to cast earthern plateau, a terrain shaping spell that does damage in anb AOE based on your armpr.
    IEnumerator EarthernPlateau()
    {
        anim.SetTrigger("EarthernPlateau");
        float targetTimer = 1f;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingRollOut;
        myManager.ps[27].Play();
        //myManager.controller.speedMultiplier = 0.4f;
        bool playParticles = false;

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;
            if (!playParticles && currentTimer > targetTimer * 0.5f)
            {
                GameObject terrain = Instantiate(myManager.skillProjectiles[0], transform.root.position, transform.root.rotation);
                terrain.GetComponent<HitBoxTerrain>().damage = myManager.stats.baseDamage * (5f);
                myManager.ps[28].Play();
                playParticles = true;
            }
            yield return null;
        }

        myManager.ps[27].Stop();
        //myManager.controller.speedMultiplier = 1f;
        pc.playerState = PlayerMovementController.PlayerState.Idle;
    }

    // Used to cast giants strength, a buff that increases strength defense and hp but severly decreases mobility.
    IEnumerator GiantStrength()
    {
        anim.SetTrigger("GiantsStrength");
        float targetTimer = 1f;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingRollOut;
        bool playParticles = false;

        while (currentTimer < targetTimer) 
        {
            currentTimer += Time.deltaTime;
            if (!playParticles && currentTimer > targetTimer * 0.5f)
            {
                myManager.ps[26].Play();
                myManager.hitBoxes.LaunchBuffBox(4);
                myManager.hitBoxes.buffboxes[4].GetComponent<HitBoxBuff>().BuffSelf();
                playParticles = true;
            }
            yield return null;
        }
        
        pc.playerState = PlayerMovementController.PlayerState.Idle;
    }

    // Used to cast aspect of rage, a buff that only applies to this chgaracter that raises attack power signifcantly.
    IEnumerator AspectOfRage()
    {
        anim.SetTrigger("AspectOfRage");
        float targetTimer = 1f;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingWithMovement;
        myManager.ps[17].Play();
        bool playParticles = false;

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;
            if (!playParticles && currentTimer > targetTimer * 0.5f)
            {
                myManager.ps[16].Play();
                myManager.hitBoxes.LaunchBuffBox(2);
                myManager.hitBoxes.buffboxes[2].GetComponent<HitBoxBuff>().BuffSelf();
                playParticles = true;
            }
            yield return null;
        }

        myManager.ps[17].Stop();
        pc.playerState = PlayerMovementController.PlayerState.Idle;
    }

    // Used by the player to cast blessings of flames, a buff that grants a player increased defensive stats and health regen for a short time.
    IEnumerator BlessingOfFlames()
    {
        anim.SetTrigger("EmboldeningEmbers");
        float targetTimer = 1f;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingNoMovement;
        myManager.ps[0].Play();
        myManager.ps[1].Play();
        myManager.ps[2].Play();
        bool playParticles = false;

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;
            if (!playParticles && currentTimer > targetTimer * 0.5f)
            {
                myManager.ps[18].Play();
                myManager.hitBoxes.LaunchBuffBox(3);
                myManager.hitBoxes.buffboxes[3].GetComponent<HitBoxBuff>().BuffSelf();
                playParticles = true;
            }
            yield return null;
        }

        myManager.ps[0].Stop();
        myManager.ps[1].Stop();
        myManager.ps[2].Stop();
        pc.playerState = PlayerMovementController.PlayerState.Idle;
    }

    // Used to use the Emboldening Embers spell, an AoE buff for all allies.
    IEnumerator EmboldeningEmbers()
    {
        anim.SetTrigger("EmboldeningEmbers");
        float targetTimer = 1f;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingNoMovement;
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
                myManager.hitBoxes.hitboxes[7].GetComponent<HitBox>().damage = myManager.stats.healthMax / 4f;
                myManager.hitBoxes.LaunchHitBox(7);
                myManager.hitBoxes.buffboxes[0].GetComponent<HitBoxBuff>().BuffSelf();
                playParticles = true;
            }
            yield return null;
        }

        myManager.ps[0].Stop();
        myManager.ps[1].Stop();
        myManager.ps[2].Stop();
        pc.playerState = PlayerMovementController.PlayerState.Idle;
    }

    // Used to cast the Severing Strike spell, a giant slam attack that stuns enemies
    IEnumerator SeveringStrike()
    {
        anim.SetTrigger("SeveringStrike");
        float targetTimer = 1f;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingRollOut;
        myManager.ps[6].Play();
        bool playParticles = false;
        myManager.hitBoxes.hitboxes[1].GetComponent<HitBox>().damage = myManager.stats.baseDamage * 4f;

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
        pc.playerState = PlayerMovementController.PlayerState.Idle;
    }

    // Used to cast blink strike.
    IEnumerator BlinkStrike()
    {
        anim.SetBool("BlinkStrike", true);
        float targetTimer = 0.5f / myManager.stats.attackSpeed;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingNoMovement;
        myManager.ps[10].Play();
        myManager.ps[11].Play();
        myManager.ps[12].Play();
        bool playParticles = false;
        //myManager.hitBoxes.hitboxes[2].GetComponent<HitBox>().damage = 25 + myManager.stats.Str * 2 + myManager.stats.weaponHitbase + myManager.stats.weaponHitMax;

        // Create a vector that houses our move inputs
        Vector2 movementInput = new Vector2(Input.GetAxisRaw(myManager.inputs.horizontalInput), Input.GetAxisRaw(myManager.inputs.verticalInput));
        Vector3 desiredMoveDirection = Vector3.zero;

        // if the movement inputs are greater than 0.2, theyll always snap from 1 to 0, then do the logic for a roll in the movement direction.
        if (movementInput.sqrMagnitude >= 0.2f)
        {
            //Debug.Log("There is a movement input");
            Vector3 forward = Camera.main.transform.forward;
            Vector3 right = Camera.main.transform.right;

            // Set up our directions based on the camera position, zero out the y values and normalize their directions.
            forward.y = 0;
            right.y = 0;

            forward.Normalize();
            right.Normalize();

            // Create the desired movement direction vector, a combination of both times thir respective inputs normalized to give us a direction.
            desiredMoveDirection = (forward * movementInput.y + right * movementInput.x).normalized;
        }
        else
            desiredMoveDirection = transform.forward.normalized;

        // The dash portion of the dash.
        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;
            yield return null;

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), 0.1f);
            myManager.characterController.Move(desiredMoveDirection * 20 * Time.deltaTime * myManager.stats.attackSpeed);

            //myManager.rb.velocity = transform.forward * 2000 * Time.deltaTime;
            //myManager.rb.angularVelocity = Vector3.zero;

            if (CheckRayHit(14, new Ray(transform.position, transform.forward), 2) ||
                CheckRayHit(14, new Ray(transform.position + transform.right * 0.5f, transform.forward), 2) ||
                CheckRayHit(14, new Ray(transform.position + transform.right * -0.5f, transform.forward), 2))
                break;
        }

        myManager.ps[10].Stop();
        myManager.ps[12].Stop();
        myManager.ps[13].Play();
        myManager.ps[6].Play();
        // This is the hit portion of the dash.
        currentTimer = 0;
        targetTimer = 0.5f / myManager.stats.attackSpeed;
        anim.SetBool("BlinkStrike", false);
        float rbSpeedMultiplier = 1f;

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;
            //myManager.rb.velocity = transform.forward * 2000 * Time.deltaTime * rbSpeedMultiplier;
            //myManager.rb.angularVelocity = Vector3.zero;
            rbSpeedMultiplier *= 0.8f;
            if (!playParticles && currentTimer > targetTimer * 0.5f)
            {
                myManager.ps[14].Play();
                myManager.ps[15].Play();
                myManager.hitBoxes.hitboxes[2].GetComponent<HitBox>().damage = myManager.stats.baseDamage * 2f;
                myManager.hitBoxes.LaunchHitBox(2);
                playParticles = true;
            }
            yield return null;
        }

        myManager.ps[6].Stop();
        pc.playerState = PlayerMovementController.PlayerState.Idle;
    }

    //USed to cast FlameStrike
    IEnumerator FlameStrike()
    {
        anim.SetTrigger("FlameStrike");
        float targetTimer = 0.4f;
        float currentTimer = 0;
        pc.playerState = PlayerMovementController.PlayerState.CastingWithMovement;
        bool playParticles = false;

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;
            if (!playParticles && currentTimer > targetTimer * 0.5f)
            {
                myManager.hitBoxes.LaunchBuffBox(1);
                myManager.hitBoxes.buffboxes[1].GetComponent<HitBoxBuff>().BuffSelf();
                playParticles = true;
            }
            yield return null;
        }
        
        pc.playerState = PlayerMovementController.PlayerState.Idle;
    }

    // USed to cast ShatteredEarth
    IEnumerator ShatteredEarth()
    {
        anim.SetBool("ShatteredEarth", true);
        pc.playerState = PlayerMovementController.PlayerState.CastingWithMovement;
        float currentTimeCharging = 0;
        float maximumChargeTime = 10f;
        float maxDamageChargeTime = 5f / myManager.stats.attackSpeed;
        bool maxDamage = false;
        myManager.ps[19].Play();
        //myManager.controller.speedMultiplier = 0.4f;
        // set our movespeed to like 50 percent of standard speed.

        // begin the charging process.
        while(currentTimeCharging < maximumChargeTime)
        {
            currentTimeCharging += Time.deltaTime;
            if(!maxDamage && currentTimeCharging > maxDamageChargeTime)
            {
                maxDamage = true;
                myManager.ps[20].Play();
                myManager.ps[21].Play();
                myManager.ps[22].Play();
            }
            // Check for an attack input, if so release this attack.
            if (Input.GetAxisRaw(myManager.inputs.attackInput) == 1 && myManager.inputs.attackReleased)
                break;

            yield return null;
        }

        myManager.ps[19].Stop();
        myManager.ps[20].Stop();
        myManager.ps[21].Stop();
        myManager.ps[22].Stop();

        float chargePercent = currentTimeCharging / maxDamageChargeTime;
        // begin the SLAM.
        float currentTimer = 0;
        float targetTimer = 1f;
        bool particlesPlayed = false;
        anim.SetBool("ShatteredEarth", false);
        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;
            if(!particlesPlayed && currentTimer > targetTimer * 0.5f)
            {
                particlesPlayed = true;
                // play particles for hit

                myManager.ps[23].Play();
                myManager.ps[24].Play();
                myManager.ps[25].Play();
                // flicker hitbox
                if (!maxDamage)
                {
                    myManager.hitBoxes.hitboxes[3].GetComponent<HitBox>().damage = myManager.stats.baseDamage * 6f * chargePercent;
                    myManager.hitBoxes.hitboxes[3].GetComponent<HitBox>().damageType = HitBox.DamageType.Physical;
                }
                else
                {
                    myManager.hitBoxes.hitboxes[3].GetComponent<HitBox>().damage = myManager.stats.baseDamage * 9f;
                    myManager.hitBoxes.hitboxes[3].GetComponent<HitBox>().damageType = HitBox.DamageType.True;
                }

                myManager.hitBoxes.LaunchHitBox(3);
            }
            yield return null;
        }

        //myManager.controller.speedMultiplier = 1f;
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
