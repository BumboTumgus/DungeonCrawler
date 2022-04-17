using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    // a state machine that dictates the actions the player can take.
    public enum PlayerState { Idle, Moving, Airborne, Rolling, Attacking, Downed, Dead, LossOfControl, LossOfControlNoGravity, CastingNoMovement, CastingRollOut, CastingWithMovement, CastingAerial, CastingAerialWithMovement, CastingIgnoreGravity, Jumping, Teleporting}
    public PlayerState playerState = PlayerState.Idle;

    [HideInInspector] public bool inventoryMenuOpen = false;          // USed to lock movement if the menu is open.
    [HideInInspector] public bool pauseMenuOpen = false;              // USed to lock movement if the menu is open.
    public GameObject inventoryWindow;                                // a public reference to the gameobject that is the inventory window.
    [HideInInspector] public bool freezePlayerMovementForMenu = false;
    public Vector3 transformNavMeshPosition;
    private Ray navMeshPositionRay;
    private RaycastHit navMeshPositionRayHit;

    [SerializeField] private float movementSpeed = 2f;                // our base movespeed, gets overridden based on the characters stats in the stats script.
    private float currentSpeed = 0f;                                  // the current speed we are moving at.
    private float speedSmoothVelocity = 0f;
    private float speedSmoothTime = 0.1f;
    private float rotationSpeed = 0.1f;                               // how fast the player rotates towards a target

    public Transform mainCameraTransform = null;                      // The position of the camera follwing us
    public ParticleSystem sprintingSpeedLines;                        // The speed line particles that are a child to the camera.

    private CharacterController controller = null;                    // Other connected components we grab at launch
    private PlayerInputs inputs = null;
    private Animator anim = null;
    private BuffsManager buffsManager;
    private PlayerStats playerStats;
    private Inventory inventory;
    private CameraControls cameraControls;
    private RagdollManager ragdollManager;
    private AudioManager audioManager;

    private bool attackReady = true;                                  // a check to see if we can launch an attack, gets flicked off whern we attack and on when we wait long enough
    private bool recentlyAttacked = false;
    private float currentTimeSinceLastAttack = 0f;
    private float targetTimeSinceLastAttack = 3f;

    private bool rollReady = true;                                    // a check to see if we can roll, flicks off when we roll and on when we wait long enoguh
    private IEnumerator rollCoroutine;
    private IEnumerator attackCoroutine;
    private IEnumerator knockbackCoroutine;
    private IEnumerator jumpCoroutine;
    private IEnumerator sprintCooldownCoroutine;

    private bool sprinting = false;
    private bool canSprint = true;

    public bool grounded = true;                                     // is the character on walkable ground. Used for jumping, rlling, and other movement
    private float gravityVectorStrength = 0f;                         // the current downward force of gravity, so the player accelerates towards the ground.
    private Ray groundRay;                                            // an uncreated ray used to check to see if are near / on the ground
    private RaycastHit groundRayHit;
    [SerializeField] private LayerMask groundingRayMask = 1 << 10;    // ensures the ray will only check for the COLLIDABLE ENVIRONMENT layer.

    private float gravityModifier = 1f;

    private float flameWalkerDistance = 0;
    private float flameWalkerDistanceTarget = 10f;

    private float currentHighestYValue = 0;

    public int currentJumps = 1;

    private const float GRAVITY = 0.4f;
    private const float GROUNDING_RAY_LENGTH = 0.7f;
    private const float GROUNDING_RAY_BONUS_TO_AVOID_MICROHOPS = 0.4f;
    private const float JUMP_POWER = 0.14f;
    private const float ROLL_SPEED_MULTIPLIER = 2f;
    private const float ROLL_ANIMSPEED_MULITPLIER = 0.6f;
    //private const float GRAVITY_VECTOR_DAMAGE_THRESHOLD = 0.27f;
    private const float GRAVITY_DAMAGE_THRESHOLD = 15f;
    private const float GRAVITY_MAX_DISTANCE_TO_FALL = 55f;
    private const float SPRINT_SPEED_INCREASE = 1.75f;
    private const float SPRINT_DELAY_FROM_DAMAGE = 3f;


    //private const float POSITIONAL_DIFFERENCE_OFFSET = 0.1f;

    // Start is called before the first frame update. Herte we grab a;; the connected scripts on the gameobject
    void Start()
    {
        //mainCameraTransform = Camera.main.transform.parent;

        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        inputs = GetComponent<PlayerInputs>();
        buffsManager = GetComponent<BuffsManager>();
        playerStats = GetComponent<PlayerStats>();
        cameraControls = mainCameraTransform.GetComponentInChildren<CameraControls>();
        sprintingSpeedLines = mainCameraTransform.GetComponentInChildren<ParticleSystem>();
        inventory = GetComponent<Inventory>();
        ragdollManager = GetComponent<RagdollManager>();
        audioManager = GetComponent<AudioManager>();

    }

    // Update is called once per frame
    void Update()
    {
        if (!freezePlayerMovementForMenu)
        {
            //Debug.Log($"Current position = {transform.position}");
            // This is logic to ensure our character rotates towards the proper target.
            if (recentlyAttacked && !playerStats.channeling)
            {
                currentTimeSinceLastAttack += Time.deltaTime;
                if (currentTimeSinceLastAttack >= targetTimeSinceLastAttack)
                {
                    recentlyAttacked = false;
                    anim.SetBool("FaceAttackDirection", false);
                    currentTimeSinceLastAttack = 0;
                }
            }

            switch (playerState)
            {
                case PlayerState.Idle:
                    Move();
                    ApplyGravity();
                    CheckJump();
                    CheckRoll();
                    CheckAttack();
                    CheckInteract();
                    CheckSprint();
                    break;
                case PlayerState.Moving:
                    Move();
                    ApplyGravity();
                    CheckJump();
                    CheckRoll();
                    CheckAttack();
                    CheckInteract();
                    CheckSprint();
                    break;
                case PlayerState.Airborne:
                    Move();
                    ApplyGravity();
                    CheckJump();
                    CheckSprint();
                    break;
                case PlayerState.Rolling:
                    ApplyGravity();
                    break;
                case PlayerState.Jumping:
                    Move();
                    ApplyGravity();
                    CheckJump();
                    CheckSprint();
                    break;
                case PlayerState.Attacking:
                    Move();
                    ApplyGravity();
                    CheckJump();
                    CheckRoll();
                    break;
                case PlayerState.Downed:
                    ApplyGravity();
                    break;
                case PlayerState.Dead:
                    ApplyGravity();
                    break;
                case PlayerState.LossOfControl:
                    ApplyGravity();
                    break;
                case PlayerState.LossOfControlNoGravity:
                    break;
                case PlayerState.CastingNoMovement:
                    anim.SetFloat("Speed", 0);
                    ApplyGravity();
                    break;
                case PlayerState.CastingRollOut:
                    ApplyGravity();
                    CheckRoll();
                    break;
                case PlayerState.CastingWithMovement:
                    Move();
                    ApplyGravity();
                    CheckJump();
                    CheckRoll();
                    break;
                case PlayerState.CastingIgnoreGravity:
                    break;
                case PlayerState.CastingAerial:
                    ApplyGravity();
                    break;
                case PlayerState.CastingAerialWithMovement:
                    Move();
                    ApplyGravity();
                    break;
                case PlayerState.Teleporting:
                    break;
                default:
                    break;
            }

            navMeshPositionRay = new Ray(transform.position, Vector3.down * 50);
            if (Physics.Raycast(navMeshPositionRay, out navMeshPositionRayHit, 100, groundingRayMask))
                transformNavMeshPosition = navMeshPositionRayHit.point;
        }
        CheckMenuInputs();
    }

    private void CheckSprint()
    {
        if (canSprint && Input.GetAxisRaw(inputs.sprintInput) == 1 && inputs.sprintReleased && !playerStats.dead)
        {
            //Debug.Log("Begin Sprinting");
            sprinting = true;
            inputs.sprintReleased = false;
            anim.SetBool("Sprinting", true);
            sprintingSpeedLines.Play();
        }
    }

    private void Move()
    {
        if (playerStats.movespeedPercentMultiplier >= 0.1f)
            anim.SetFloat("AnimSpeed", 1 * playerStats.movespeedPercentMultiplier);
        else
            anim.SetFloat("AnimSpeed", 1 * 0.1f);

        // Create a vector that houses our move inputs
        Vector2 movementInput = new Vector2(Input.GetAxisRaw(inputs.horizontalInput), Input.GetAxisRaw(inputs.verticalInput));

        Vector3 forward = mainCameraTransform.forward;
        Vector3 right = mainCameraTransform.right;

        // Set up our directions based on the camera position, zero out the y values and normalize their directions.
        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        // Create the desired movement direction vector, a combination of both times thir respective inputs normalized to give us a direction.
        Vector3 desiredMoveDirection = (forward * movementInput.y + right * movementInput.x).normalized;

        //Debug.Log("The desired movement vector's normalzied square magnitude is " + desiredMoveDirection.sqrMagnitude);

        // Rotate towards the target move direction. Set the animation speed in the animator so the character walks properly.
        if (desiredMoveDirection != Vector3.zero)
        {
            if (!recentlyAttacked)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), rotationSpeed);

            if (playerStats.movespeedPercentMultiplier > 0.1f)
                anim.SetFloat("Speed", 1 * playerStats.movespeedPercentMultiplier);
            else
                anim.SetFloat("Speed", 1 * 0.1f);
        }
        else
        {
            anim.SetFloat("Speed", 0);
            CancelSprint(false);
        }

        if(recentlyAttacked)
        {
            Vector3 cameraRotation = Camera.main.transform.forward;
            cameraRotation.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(cameraRotation, Vector3.up), rotationSpeed);

            anim.SetFloat("RelativeXMovement", movementInput.x);
            anim.SetFloat("RelativeZMovement", movementInput.y);
        }

        // Set up the players speed.
        float targetSpeed = 0;
        if (playerStats.movespeedPercentMultiplier > 0.1f)
            targetSpeed = movementSpeed * desiredMoveDirection.sqrMagnitude * playerStats.movespeedPercentMultiplier;
        else
            targetSpeed = movementSpeed * desiredMoveDirection.sqrMagnitude * 0.1f;

        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);


        if (desiredMoveDirection != Vector3.zero)
        {
            //Debug.Log("The previous position is: " + transform.position);
            if (sprinting)
                controller.Move(desiredMoveDirection * currentSpeed * Time.deltaTime * SPRINT_SPEED_INCREASE);
            else
                controller.Move(desiredMoveDirection * currentSpeed * Time.deltaTime);
            //Debug.Log("The new position is: " + transform.position);
        }

        // If we have the flamewalker buff active, increase this the more we move.
        if (playerStats.flameWalkerEnabled)
        {
            if(sprinting)
                flameWalkerDistance += currentSpeed * Time.deltaTime * SPRINT_SPEED_INCREASE;
            else
                flameWalkerDistance += currentSpeed * Time.deltaTime;

            if (flameWalkerDistance >= flameWalkerDistanceTarget)
            {
                flameWalkerDistance -= flameWalkerDistanceTarget;

                audioManager.PlayAudio(64);
                GameObject flamewalkerDamage = Instantiate(GetComponent<SkillsManager>().skillProjectiles[5], transform.position + Vector3.up * 0.1f, Quaternion.identity);
                flamewalkerDamage.GetComponent<HitBox>().damage = playerStats.baseDamage * 3f * playerStats.spellDamageMultiplier;
                flamewalkerDamage.GetComponent<HitBox>().myStats = playerStats;
            }
        }

    }

    // Applies a downward force to the player if they are not grounded
    private void ApplyGravity()
    {
        Ray groundRay = new Ray(transform.position + Vector3.up * 0.5f, Vector3.down);

        RaycastHit groundRayHit;
        bool rayHitGround = false;
        Vector3 groundRayHitPoint = Vector3.zero;
        bool primaryRayHit = false;

        groundRay = new Ray(transform.position + Vector3.up * 0.5f, Vector3.down);
        if (Physics.Raycast(groundRay, out groundRayHit, GROUNDING_RAY_LENGTH, groundingRayMask) && playerState != PlayerState.Jumping && playerState != PlayerState.CastingAerial && playerState != PlayerState.CastingAerialWithMovement)
        {
            rayHitGround = true;
            groundRayHitPoint = groundRayHit.point;
            primaryRayHit = true;
        }
        Debug.DrawRay(transform.position + Vector3.up * 0.5f, Vector3.down, Color.yellow);

        //Debug.Log("we have hit the ground with one of our 15 rays? " + rayHitGround);
        // Shoot a ray, if it we hit we are grounded if not we are no longer grounded. If we just jumped ignore this and set us as not grounded.
        //Debug.Log("did our ray hit the ground");
        if (rayHitGround)
        {
            //Debug.Log("check if were grounded");
            if (!grounded)
            {
                //Debug.Log($"WE fell a total of {currentHighestYValue - transform.position.y}");
                //Debug.Log("Apply gravity damage here, our gravity vector is: " + gravityVectorStrength);
                if(currentHighestYValue - transform.position.y > GRAVITY_DAMAGE_THRESHOLD)
                {
                    playerStats.TakeDamage(playerStats.healthMax * ((currentHighestYValue - transform.position.y - GRAVITY_DAMAGE_THRESHOLD) /  GRAVITY_MAX_DISTANCE_TO_FALL), false, HitBox.DamageType.True, 0, null, false);
                }

                // if the ray hit the ground, set us as grounded, snap us to the ground, and change the state while updating the aniamtion.
                grounded = true;
                gravityModifier = 1;
                currentHighestYValue = -999;

                audioManager.PlayAudio(13);

                if (primaryRayHit)
                {
                    Vector3 positionalDifference = groundRayHitPoint - transform.position;
                    controller.Move(positionalDifference);
                }

                //Debug.Log("The positional difference is: " + positionalDifference + ". Our transform is: " + transform.position);

                anim.SetBool("Grounded", true);
                currentJumps = playerStats.jumps;
                if (!playerStats.stunned && !playerStats.knockedBack && !playerStats.asleep && !playerStats.frozen)
                {
                    if (playerState == PlayerState.Airborne)
                        playerState = PlayerState.Idle;
                }
            }

            gravityVectorStrength = 0;
        }
        else
        {
            // if we are np longer grounded switch the state and and animation.
            if (!playerStats.stunned && !playerStats.knockedBack && !playerStats.asleep && !playerStats.frozen)
            {
                // shoot a ray down to see if were just slightly off the ground. if so we will ignore the airborne switch
                // this is done to avoid awkward movement going down hills.
                groundRay = new Ray(transform.position + Vector3.up * 0.5f, Vector3.down);
                Ray groundRay02 = new Ray(transform.position + Vector3.up * 0.5f, new Vector3(1f, -1f, 0));
                Ray groundRay03 = new Ray(transform.position + Vector3.up * 0.5f, new Vector3(-1f, -1f, 0));
                Ray groundRay04 = new Ray(transform.position + Vector3.up * 0.5f, new Vector3(0, -1f, 1f));
                Ray groundRay05 = new Ray(transform.position + Vector3.up * 0.5f, new Vector3(0, -1f, -1f));
                //!Physics.Raycast(groundRay, GROUNDING_RAY_LENGTH + GROUNDING_RAY_BONUS_TO_AVOID_MICROHOPS, groundingRayMask) &&
                if (!Physics.Raycast(groundRay02, GROUNDING_RAY_LENGTH + GROUNDING_RAY_BONUS_TO_AVOID_MICROHOPS, groundingRayMask) &&
                    !Physics.Raycast(groundRay03, GROUNDING_RAY_LENGTH + GROUNDING_RAY_BONUS_TO_AVOID_MICROHOPS, groundingRayMask) &&
                    !Physics.Raycast(groundRay04, GROUNDING_RAY_LENGTH + GROUNDING_RAY_BONUS_TO_AVOID_MICROHOPS, groundingRayMask) &&
                    !Physics.Raycast(groundRay05, GROUNDING_RAY_LENGTH + GROUNDING_RAY_BONUS_TO_AVOID_MICROHOPS, groundingRayMask) ||
                    !Physics.Raycast(groundRay, GROUNDING_RAY_LENGTH + GROUNDING_RAY_BONUS_TO_AVOID_MICROHOPS * 3, groundingRayMask))
                {

                    //Debug.Log("The ray missed, we are now airborne");
                    grounded = false;
                    anim.SetBool("Grounded", false);

                    if (playerState != PlayerState.Jumping && playerState != PlayerState.CastingAerial && playerState != PlayerState.CastingAerialWithMovement)
                    {
                        //Debug.Log("we were jumping or casting an aeril skill so set us to airborne");
                        playerState = PlayerState.Airborne;
                    }
                }
            }

            Vector3 gravityVector = Vector3.zero;


            gravityVectorStrength -= GRAVITY * gravityModifier * Time.deltaTime;
            gravityVector.y = gravityVectorStrength;

            //Debug.Log("GRAVITY MOVEMENT PRE: " + transform.position);
            controller.Move(gravityVector);
            //Debug.Log("GRAVITY MOVEMENT POST: " + transform.position);

            if (currentHighestYValue < transform.position.y)
                currentHighestYValue = transform.position.y;
        }

    }

    // Used to snap our character to the floor is possible.
    public void SnapToFloor()
    {
        Ray groundRay = new Ray(transform.position + Vector3.up * 0.5f, Vector3.down);
        RaycastHit groundRayHit;

        // Shoot a ray, if it we hit we are grounded if not we are no longer grounded. If we just jumped ignore this and set us as not grounded.
        if (Physics.Raycast(groundRay, out groundRayHit, GROUNDING_RAY_LENGTH * 1.4f, groundingRayMask))
        {
            Vector3 positionalDifference = groundRayHit.point - transform.position + Vector3.up * 0.02f;
            transform.position += positionalDifference;
            //controller.Move(positionalDifference);
        }
    }

    // Used to check if the player's jump imput was pressed.
    private void CheckJump()
    {
        if (Input.GetAxisRaw(inputs.jumpInput) != 0 && inputs.jumpReleased  && currentJumps > 0 && !inventoryMenuOpen)
        {
            //Debug.Log("we jumped here");
            inputs.jumpReleased = false;
            if(jumpCoroutine != null)
                StopCoroutine(jumpCoroutine);

            jumpCoroutine = Jump(JUMP_POWER, 0.5f, true);
            Instantiate(GetComponent<SkillsManager>().skillProjectiles[84], transform.position, Quaternion.identity);
            StartCoroutine(jumpCoroutine);
            currentJumps--;
        }
    }

    //USed to apply the jump force.
    public void ApplyJumpForce(float jumpStrength)
    {
        //Debug.Log("we added jump force");
        StartCoroutine(Jump(jumpStrength, 0.3f, false));
    }

    // Used to complete the jump logic 
    IEnumerator Jump(float jumpPower, float timeAppliedFor, bool setTrigger)
    {
        //Debug.Log("jump coroutine started");
        //Debug.Log("we have launched with a force of " + jumpPower + " and and bool of: " + setTrigger);
        if (setTrigger)
        {
            anim.SetTrigger("Jump");
            playerState = PlayerState.Jumping;
        }
        currentHighestYValue = transform.position.y;

        grounded = false;
        gravityVectorStrength = jumpPower;
        audioManager.PlayAudio(11);

        yield return new WaitForSeconds(timeAppliedFor);

        if (playerState == PlayerState.LossOfControl || playerState == PlayerState.LossOfControlNoGravity || playerState == PlayerState.CastingAerial || playerState == PlayerState.CastingAerialWithMovement)
        {
            //Debug.Log("we have some sort of cc on us");
        }
        else
            playerState = PlayerState.Airborne;
    }

    // Used to check and see if the player has started a roll action.
    private void CheckRoll()
    {
        if (Input.GetAxisRaw(inputs.rollInput) != 0 && grounded && (playerState != PlayerState.Airborne && playerState != PlayerState.LossOfControl && playerState != PlayerState.LossOfControlNoGravity) && rollReady && !inventoryMenuOpen)
        {
            rollCoroutine = Roll();
            anim.applyRootMotion = false;

            // This resets all oither attack based animations
            anim.Play("Idle", 3);
            anim.Play("Idle", 4);
            anim.Play("SwitchAnims", 1);

            StartCoroutine(rollCoroutine);
        }
    }

    // Used to compelte the roll logic.
    IEnumerator Roll()
    {
        rollReady = false;
        GetComponent<SkillsManager>().InterruptSkills();

        buffsManager.ProcOnRoll();
        audioManager.PlayAudio(12);

        anim.SetTrigger("Roll");
        if(playerStats.movespeedPercentMultiplier >= 0.25f)
            anim.SetFloat("AnimSpeed", 1 * playerStats.movespeedPercentMultiplier);
        else
            anim.SetFloat("AnimSpeed", 1 * 0.25f);

        playerState = PlayerState.Rolling;

        // Ensures we face forward after a roll.
        recentlyAttacked = false;
        CancelSprint(false);
        anim.SetBool("FaceAttackDirection", false);
        currentTimeSinceLastAttack = 0;

        // here we see if we are on fire, if so lower the duration of the debuff.
        foreach (Buff buff in buffsManager.activeBuffs)
        {
            //Debug.Log("we are checking for a fire debuff");
            if (buff.myType == BuffsManager.BuffType.Aflame)
            {
                //Debug.Log("we found one");
                buff.RemoveStacks(Mathf.RoundToInt(buff.currentStacks / 2), false);
                // remove stacks of aflame here.-------------------------------------------------------------
            }
        }
        
        // Create a vector that houses our move inputs
        Vector2 movementInput = new Vector2(Input.GetAxisRaw(inputs.horizontalInput), Input.GetAxisRaw(inputs.verticalInput));
        Vector3 desiredMoveDirection = Vector3.zero;

        // if the movement inputs are greater than 0.2, theyll always snap from 1 to 0, then do the logic for a roll in the movement direction.
        if(movementInput.sqrMagnitude >= 0.2f)
        {
            //Debug.Log("There is a movement input");
            Vector3 forward = mainCameraTransform.forward;
            Vector3 right = mainCameraTransform.right;

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

        // Create the timers for the roll duration
        float currentTimer = 0;
        // This represents the time we must wait: the base length of the animation clip times the speed modifier inherent in the animation clip editor plus some leeway for when the animation ends and blends away.
        float targetTimer = 0;
        if(playerStats.movespeedPercentMultiplier >= 0.25f)
            targetTimer = 0.8f / ROLL_ANIMSPEED_MULITPLIER * 0.7f * (1 / playerStats.movespeedPercentMultiplier);
        else
            targetTimer = 0.8f / ROLL_ANIMSPEED_MULITPLIER * 0.7f * 4;

        //float targetTimer = anim.GetCurrentAnimatorClipInfo(5).Length;
        //Debug.Log("the target timer is: " + targetTimer);

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;
            
            // Rotate towards the target move direction.
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), rotationSpeed);

            // Set up the players speed.
            float targetSpeed = 0;
            if (playerStats.movespeedPercentMultiplier >= 0.25f)
                targetSpeed = movementSpeed * ROLL_SPEED_MULTIPLIER * playerStats.movespeedPercentMultiplier;
            else
                targetSpeed = movementSpeed * ROLL_SPEED_MULTIPLIER * 0.25f;

            currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);

            controller.Move(desiredMoveDirection * currentSpeed * Time.deltaTime);


            yield return new WaitForFixedUpdate();
        }

        
        // yield return new WaitForSeconds(ROLL_DURATION * ANIM_SPEED_REDUCTION / playerStats.speed);
        playerState = PlayerState.Idle;

        // here we must wait before our roll is ready again.
        yield return new WaitForSeconds(0.25f);
        rollReady = true;
    }

    // Used to check if the basic attack input was pressed.
    private void CheckAttack()
    {
        if (Input.GetAxisRaw(inputs.attackInput) != 0 && attackReady && !playerStats.stunned && !playerStats.asleep && !inventoryMenuOpen)
        {
            attackCoroutine = Attack();
            StartCoroutine(attackCoroutine);
        }
    }

    // Used to start the attack logic.
    IEnumerator Attack()
    {
        buffsManager.ProcOnAttack();
        CancelSprint(true);
        attackReady = false;

        recentlyAttacked = true;
        currentTimeSinceLastAttack = 0;
        anim.SetBool("FaceAttackDirection", true);

        anim.SetTrigger("Attack");
        float baseWeaponAttackSpeed = 1f;
        switch (playerStats.weaponBaseAttacksPerSecond.Count)
        {
            case 0:
                baseWeaponAttackSpeed = 1f;
                break;
            case 1:
                baseWeaponAttackSpeed = playerStats.weaponBaseAttacksPerSecond[0];
                break;
            case 2:
                baseWeaponAttackSpeed = (playerStats.weaponBaseAttacksPerSecond[0] + playerStats.weaponBaseAttacksPerSecond[1]) / 2;
                break;
            default:
                baseWeaponAttackSpeed = 1f;
                break;
        }
        anim.SetFloat("AttackAnimSpeed", baseWeaponAttackSpeed * playerStats.attackSpeed);
        playerState = PlayerState.Attacking;

        // set up our timers here.
        float currentTimer = 0;
        // This represents the time we must wait: the base length of the animation clip times the speed modifier inherent in the animation clip editor plus some leeway for when the animation ends and blends away.
        float targetTimer =  (1 / baseWeaponAttackSpeed) / playerStats.attackSpeed;
        //Debug.Log("the target timer is: " + targetTimer);
        bool breakLoop = false;

        // Start the timer.
        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;

            // Break away from this coroutine if we start a roll.
            if (playerState == PlayerState.Rolling || playerState == PlayerState.LossOfControl || playerState == PlayerState.LossOfControlNoGravity)
            {
                breakLoop = true;
                break;
            }
            yield return new WaitForEndOfFrame();
        }

        currentTimeSinceLastAttack = 0;
        //Debug.Log("the attack has ended");
        if (!breakLoop)
            playerState = PlayerState.Idle;
        attackReady = true;
    }

    // Called when the player is hit below 0 hp and downed.
    public void PlayerDowned()
    {
        StopAllCoroutines();
        playerState = PlayerState.Downed;
        anim.SetTrigger("Downed");
    }

    // Used when the player gets stunned
    public void StunLaunch()
    {
        StartCoroutine(Stunned());
    }

    // The stunned coroutine. Makes the player unable to take action.
    IEnumerator Stunned()
    {
        GetComponent<SkillsManager>().InterruptSkills();
        CancelSprint(true);
        if (!playerStats.frozen)
            anim.SetFloat("FrozenMultiplier", 1f);
        playerStats.stunned = true;
        playerState = PlayerState.LossOfControl;
        anim.SetBool("Stunned", true);

        while (playerStats.stunned)
            yield return null;

        playerStats.stunned = false;

        CheckForOtherLoseOfControlEffects();
    }

    // Used when the player gets put to sleep
    public void AsleepLaunch()
    {
        StartCoroutine(Asleep());
    }

    // The asleep coroutine. Makes the player unable to take action.
    IEnumerator Asleep()
    {
        GetComponent<SkillsManager>().InterruptSkills();
        CancelSprint(true);
        playerStats.asleep = true;
        playerState = PlayerState.LossOfControl;
        anim.SetBool("Sleeping", true);

        while (playerStats.asleep)
            yield return null;

        playerStats.asleep = false;
        anim.SetBool("Sleeping", false);

        CheckForOtherLoseOfControlEffects();
    }

    // Used when the player gets frozen
    public void FrozenLaunch()
    {
        StartCoroutine(Frozen());
    }

    // The asleep coroutine. Makes the player unable to take action.
    IEnumerator Frozen()
    {
        GetComponent<SkillsManager>().InterruptSkills();
        CancelSprint(true);
        playerStats.frozen = true;
        playerState = PlayerState.LossOfControl;
        anim.SetBool("Stunned", true);
        anim.SetFloat("FrozenMultiplier", 0f);

        while (playerStats.frozen)
            yield return null;

        playerStats.frozen = false;

        anim.SetFloat("FrozenMultiplier", 1f);

        CheckForOtherLoseOfControlEffects();
    }

    // Used when the player gets frozen
    public void KnockbackLaunch(Vector3 directionOfKnockback, PlayerStats buffOrigin)
    {
        // Check to see if the knockback works and goes through.
        if (Random.Range(0, 100) > playerStats.knockbackResistance * 100)
        {
            GetComponent<SkillsManager>().InterruptSkills();
            ragdollManager.StopAllCoroutines();
            anim.ResetTrigger("GettingUpFacingDown");
            anim.ResetTrigger("GettingUpFacingUp");

            anim.Play("Idle", 6);

            if (knockbackCoroutine != null)
                StopCoroutine(knockbackCoroutine);
            knockbackCoroutine = Knockback(directionOfKnockback);
            StartCoroutine(knockbackCoroutine);

            GetComponent<BuffsManager>().NewBuff(BuffsManager.BuffType.Knockback, 0, buffOrigin);
        }
    }

    // The asleep coroutine. Makes the player unable to take action.
    IEnumerator Knockback(Vector3 directionalKnockback)
    {
        if(rollCoroutine != null)
            StopCoroutine(rollCoroutine);
        rollReady = true;

        if (attackCoroutine != null)
            StopCoroutine(attackCoroutine);
        attackReady = true;

        playerStats.knockedBack = true;
        playerState = PlayerState.LossOfControlNoGravity;
        ragdollManager.EnableRagDollState(directionalKnockback);

        float currentTimer = 0;
        float targetTimer = 0.2f;

        while (playerStats.knockedBack)
        {
            currentTimer += Time.deltaTime;
            if(currentTimer >= targetTimer)
            {
                currentTimer -= targetTimer;

                if (ragdollManager.CanWeGetUpVelocityPoll())
                {
                    ragdollManager.DisableRagDollState();
                    break;
                }
            }

            yield return null;
        }

        playerState = PlayerState.LossOfControl;

        currentTimer = 0f;
        targetTimer = 0.5f;

        for(int index = 0; index < 6; index++)
        {
            //Debug.Log(anim.GetLayerName(index));
            anim.SetLayerWeight(index, 0);
        }

        if (ragdollManager.getUpFaceUp)
            anim.SetTrigger("GettingUpFacingDown");
        else
            anim.SetTrigger("GettingUpFacingUp");
        anim.enabled = true;

        // player is getting up.
        while (playerStats.knockedBack)
        {
            currentTimer += Time.deltaTime;
            if (currentTimer >= targetTimer)
                buffsManager.AttemptRemovalOfBuff(BuffsManager.BuffType.Knockback, false);

            yield return null;
        }

        for (int index = 0; index < 6; index++)
        {
            //Debug.Log(anim.GetLayerName(index));
            anim.SetLayerWeight(index, 1);
        }

        CheckForOtherLoseOfControlEffects();
    }

    // USed when we need to cancel the sprint. Happens when we cast a spell, attack, take damage.
    public void CancelSprint(bool startSprintCountdown)
    {
        sprinting = false;
        anim.SetBool("Sprinting", false);
        sprintingSpeedLines.Stop();
        //Debug.Log("Cancel Sprinting");

        if (startSprintCountdown)
        {
            //Debug.Log("... AND START OUR DELAY");
            if (sprintCooldownCoroutine != null)
                StopCoroutine(sprintCooldownCoroutine);
            sprintCooldownCoroutine = DelayUntilCanSprintAgain();
            StartCoroutine(sprintCooldownCoroutine);
        }
    }

    private IEnumerator DelayUntilCanSprintAgain()
    {
        canSprint = false;
        yield return new WaitForSeconds(SPRINT_DELAY_FROM_DAMAGE);
        canSprint = true;
    }

    // Used to make the player do a pickup animation.
    public void CheckInteract()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Debug.Log("an interact attempt was made");
            if (inventory.interactablesInRange.Count > 0)
            {
                bool removeInteractable = true;
                // Here we check what kind of interactable it is and then interact with it accordingly.
                GameObject interactable = inventory.GrabClosestInteractable();
                anim.SetTrigger("Interact");

                if (interactable.GetComponent<ChestBehaviour>() != null)
                {
                    if (playerStats.gold >= interactable.GetComponent<ChestBehaviour>().chestCost)
                    {
                        interactable.GetComponent<ChestBehaviour>().OpenChest();
                        playerStats.AddGold(-1 * interactable.GetComponent<ChestBehaviour>().chestCost);
                        audioManager.PlayAudio(15);
                    }
                    else
                    {
                        removeInteractable = false;
                    }
                }
                else if (interactable.GetComponent<DoorOpenVolumeBehaviour>() != null)
                    interactable.GetComponent<DoorOpenVolumeBehaviour>().InteractWithDoor();
                else if (interactable.transform.root.GetComponent<TeleporterBehaviour>() != null)
                {
                    interactable.transform.root.GetComponent<TeleporterBehaviour>().teleporterActive = false;
                    GameManager.instance.LaunchPlayerTeleport();
                }
                else if(interactable.GetComponent<ArtifactBehaviour>() != null)
                {
                    interactable.GetComponent<ArtifactBehaviour>().ActivateArtifact();
                }


                if(removeInteractable)
                    inventory.interactablesInRange.Remove(interactable);
            }
            else if (inventory.itemsInRange.Count > 0)
            {
                inventory.PickUpItem(inventory.GrabClosestItem());
                anim.SetTrigger("PickUp");
                audioManager.PlayAudio(14);
            }
        }
    }

    // Check if the menu inputs were pressed. If so display the proper menu.
    private void CheckMenuInputs()
    {
        // If we have pressed the inventory window, 
        if (Input.GetAxisRaw(inputs.inventoryInput) == 1 && inputs.inventoryReleased && !playerStats.dead)
        {
            inputs.inventoryReleased = false;
            //Debug.Log("Inventory will be opened or closed");
            if (!inventoryWindow.activeSelf)
            {
                // Set the lock for our movement and camera controls after we press and open the inventory.
                inventoryWindow.SetActive(true);
                inventoryWindow.GetComponent<InventoryUiManager>().audioManager.PlayAudio(2);

                inventoryMenuOpen = true;
                freezePlayerMovementForMenu = true;
                cameraControls.menuOpen = true;
                Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
            }
            else
            {
                HideInventoryWindow();
            }
        }
    }

    public void HideInventoryWindow()
    {
        // Remvoe the lock for our movement and camera controls after we press and open the inventory.
        inventoryWindow.SetActive(false);
        inventoryWindow.GetComponent<InventoryUiManager>().audioManager.PlayAudio(3);


        inventoryWindow.GetComponent<InventoryPopupTextManager>().lockPointer = false;
        inventoryWindow.GetComponent<InventoryPopupTextManager>().HidePopups(true);
        inventoryMenuOpen = false;
        if (!pauseMenuOpen)
        {
            freezePlayerMovementForMenu = false;
            cameraControls.menuOpen = false;
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    /*
    // Used when we enter a trigger and it has a room tag
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("RoomVolume"))
        {
            //Debug.Log("We have entered a new room");
            GameManager.instance.ShowRoom(other.transform.parent.GetComponent<RoomManager>());
        }
    }
    */

    // Called afetr any other negative effects end to see if we shoudl regain control of our player or not.
    public void CheckForOtherLoseOfControlEffects()
    {
        if (playerStats.knockedBack)
            playerState = PlayerState.LossOfControlNoGravity;
        else if (playerStats.stunned || playerStats.frozen || playerStats.asleep)
            playerState = PlayerState.LossOfControl;
        else
        {
            anim.SetBool("Stunned", false);
            playerState = PlayerState.Idle;
        }
    }

    // Used to make the player move in this desired direction for the duration of a skill.
    public void SkillMovement(Vector3 direction, float distancePerSecond)
    {
        controller.Move(direction * distancePerSecond * Time.deltaTime);
    }

    //USed when we start a skill, stop all other coroutines in the player movement that would impede it.
    public void SkillCastCoroutineClear()
    {
        if (rollCoroutine != null)
            StopCoroutine(rollCoroutine);
        rollReady = true;

        if (attackCoroutine != null)
            StopCoroutine(attackCoroutine);
        attackReady = true;
    }
    
    //USed t ensure the plyer faces the direction they are attacking
    public void SnapToFaceCamera()
    {
        Vector3 cameraRotation = Camera.main.transform.forward;
        cameraRotation.y = 0;
        transform.rotation = Quaternion.LookRotation(cameraRotation, Vector3.up);

        recentlyAttacked = true;
        currentTimeSinceLastAttack = 0;
        anim.SetBool("FaceAttackDirection", true);
    }

    public void SnapToFaceVector(Vector3 directionToSnapTo)
    {
        directionToSnapTo.y = 0;
        transform.rotation = Quaternion.LookRotation(directionToSnapTo, Vector3.up);
    }

    // USed to change the gravity modifier so we fall faster. Must be in a method so the animation clips can access it.
    public void ChangeGravityModifier(float value)
    {
        gravityModifier = value;
    }

    // Have we entered an out of bounds box? If so take damage then make the game manager snap us to a new location.
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("The layer is: " + other.gameObject.layer);
        if(other.gameObject.layer == 7)
        {
            //playerStats.TakeDamage(playerStats.healthMax * 0.2f, false, HitBox.DamageType.True, 0, null, false);
            GameManager.instance.SnapToNearestTPLocation(gameObject);
        }
    }

    public void ResetCurrentHighestPoint()
    {
        currentHighestYValue = -999;
    }
}
