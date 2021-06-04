using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    // a state machine that dictates the actions the player can take.
    public enum PlayerState { Idle, Moving, Airborne, Rolling, Sprinting, Attacking, Downed, Dead, LossOfControl, LossOfControlNoGravity, CastingNoMovement, CastingRollOut, CastingWithMovement, CastingAerial, CastingIgnoreGravity, Jumping}
    public PlayerState playerState = PlayerState.Idle;

    [HideInInspector] public bool menuOpen = false;                   // USed to lock movement if the menu is open.
    public GameObject inventoryWindow;                                // a public reference to the gameobject that is the inventory window.

    [SerializeField] private float movementSpeed = 2f;                // our base movespeed, gets overridden based on the characters stats in the stats script.
    private float currentSpeed = 0f;                                  // the current speed we are moving at.
    private float speedSmoothVelocity = 0f;
    private float speedSmoothTime = 0.1f;
    private float rotationSpeed = 0.1f;                               // how fast the player rotates towards a target

    private Transform mainCameraTransform = null;                     // The position of the camera follwing us

    private CharacterController controller = null;                    // Other connected components we grab at launch
    private PlayerInputs inputs = null;
    private Animator anim = null;
    private BuffsManager buffsManager;
    private PlayerStats playerStats;
    private HitBoxManager hitBoxManager;
    private Inventory inventory;
    private CameraControls cameraControls;
    private RagdollManager ragdollManager;

    private bool attackReady = true;                                  // a check to see if we can launch an attack, gets flicked off whern we attack and on when we wait long enough
    private bool recentlyAttacked = false;
    private float currentTimeSinceLastAttack = 0f;
    private float targetTimeSinceLastAttack = 3f;

    private bool rollReady = true;                                    // a check to see if we can roll, flicks off when we roll and on when we wait long enoguh
    private IEnumerator rollCoroutine;
    private IEnumerator attackCoroutine;
    private IEnumerator knockbackCoroutine;
    private IEnumerator jumpCoroutine;

    private bool grounded = true;                                     // is the character on walkable ground. Used for jumping, rlling, and other movement
    private float gravityVectorStrength = 0f;                         // the current downward force of gravity, so the player accelerates towards the ground.
    private Ray groundRay;                                            // an uncreated ray used to check to see if are near / on the ground
    private RaycastHit groundRayHit;
    [SerializeField] private LayerMask groundingRayMask = 1 << 10;    // ensures the ray will only check for the COLLIDABLE ENVIRONMENT layer.

    private float gravityModifier = 1f;

    private float flameWalkerDistance = 0;
    private float flameWalkerDistanceTarget = 10f;

    public int currentJumps = 1;

    private const float GRAVITY = 0.4f;
    private const float GROUNDING_RAY_LENGTH = 0.7f;
    private const float JUMP_POWER = 0.18f;
    private const float ROLL_SPEED_MULTIPLIER = 2f;
    private const float ROLL_ANIMSPEED_MULITPLIER = 0.6f;


    //private const float POSITIONAL_DIFFERENCE_OFFSET = 0.1f;

    // Start is called before the first frame update. Herte we grab a;; the connected scripts on the gameobject
    void Start()
    {
        mainCameraTransform = Camera.main.transform.parent;

        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        inputs = GetComponent<PlayerInputs>();
        buffsManager = GetComponent<BuffsManager>();
        playerStats = GetComponent<PlayerStats>();
        hitBoxManager = GetComponent<HitBoxManager>();
        cameraControls = mainCameraTransform.GetComponentInChildren<CameraControls>();
        inventory = GetComponent<Inventory>();
        ragdollManager = GetComponent<RagdollManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // This is logic to ensure our character rotates towards the proper target.
        if(recentlyAttacked && !playerStats.channeling)
        {
            currentTimeSinceLastAttack += Time.deltaTime;
            if(currentTimeSinceLastAttack >= targetTimeSinceLastAttack)
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
                break;
            case PlayerState.Moving:
                Move();
                ApplyGravity();
                CheckJump();
                CheckRoll();
                CheckAttack();
                CheckInteract();
                break;
            case PlayerState.Airborne:
                Move();
                ApplyGravity();
                CheckJump();
                break;
            case PlayerState.Rolling:
                ApplyGravity();
                break;
            case PlayerState.Sprinting:
                Move();
                ApplyGravity();
                CheckJump();
                CheckRoll();
                CheckAttack();
                CheckInteract();
                break;
            case PlayerState.Jumping:
                Move();
                ApplyGravity();
                CheckJump();
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
            default:
                break;
        }
        CheckMenuInputs();
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

        // Rotate towards the target move direction. Set the animation speed in the animator so the character walks properly.
        if (desiredMoveDirection != Vector3.zero)
        {
            if (!recentlyAttacked)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), rotationSpeed);

            if(playerStats.movespeedPercentMultiplier > 0.1f)
                anim.SetFloat("Speed", 1 * playerStats.movespeedPercentMultiplier);
            else
                anim.SetFloat("Speed", 1 * 0.1f);
        }
        else
            anim.SetFloat("Speed", 0);

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
            targetSpeed = movementSpeed * desiredMoveDirection.magnitude * playerStats.movespeedPercentMultiplier;
        else
            targetSpeed = movementSpeed * desiredMoveDirection.magnitude * 0.1f;

        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);

        controller.Move(desiredMoveDirection * currentSpeed * Time.deltaTime);

        // If we have the flamewalker buff active, increase this the more we move.
        if (playerStats.flameWalkerEnabled)
        {
            flameWalkerDistance += currentSpeed * Time.deltaTime;

            if(flameWalkerDistance >= flameWalkerDistanceTarget)
            {
                flameWalkerDistance -= flameWalkerDistanceTarget;

                GameObject flamewalkerDamage = Instantiate(GetComponent<SkillsManager>().skillProjectiles[5], transform.position + Vector3.up * 0.1f, Quaternion.identity);
                flamewalkerDamage.GetComponent<HitBox>().damage = playerStats.baseDamage * 0.8f;
                flamewalkerDamage.GetComponent<HitBox>().myStats = playerStats;
            }
        }

    }

    // Applies a downward force to the player if they are not grounded
    private void ApplyGravity()
    {
        Ray groundRay = new Ray(transform.position + Vector3.up * 0.5f, Vector3.down);
        RaycastHit groundRayHit;

        // Shoot a ray, if it we hit we are grounded if not we are no longer grounded. If we just jumped ignore this and set us as not grounded.
        if (Physics.Raycast(groundRay, out groundRayHit, GROUNDING_RAY_LENGTH, groundingRayMask) && playerState != PlayerState.Jumping && playerState != PlayerState.CastingAerial)
        {
            // if the ray hit the ground, set us as grounded, snap us to the ground, and change the state while updating the aniamtion.
            grounded = true;
            gravityVectorStrength = 0;
            gravityModifier = 1;

            Vector3 positionalDifference = groundRayHit.point - transform.position;
            //positionalDifference.y -= POSITIONAL_DIFFERENCE_OFFSET;
            controller.Move(positionalDifference);

            //Debug.Log("The positional difference is: " + positionalDifference + ". Our transform is: " + transform.position);

            if (!playerStats.stunned && !playerStats.knockedBack && !playerStats.asleep && !playerStats.frozen)
            {
                anim.SetBool("Grounded", true);
                currentJumps = playerStats.jumps;
                if (playerState == PlayerState.Airborne)
                    playerState = PlayerState.Idle;
            }
        }
        else
        {
            // if we are np longer grounded switch the state and and animation.
            if (!playerStats.stunned && !playerStats.knockedBack && !playerStats.asleep && !playerStats.frozen)
            {
                grounded = false;
                anim.SetBool("Grounded", false);

                if (playerState != PlayerState.Jumping && playerState != PlayerState.CastingAerial)
                {
                    //Debug.Log("we were jumping or casting an aeril skill so set us to airborne");
                    playerState = PlayerState.Airborne;
                }
            }

            Vector3 gravityVector = Vector3.zero;

            gravityVectorStrength -= GRAVITY * gravityModifier * Time.deltaTime;
            gravityVector.y = gravityVectorStrength;

            controller.Move(gravityVector);
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
        if (Input.GetAxisRaw(inputs.jumpInput) != 0 && inputs.jumpReleased  && currentJumps > 0 && !menuOpen)
        {
            inputs.jumpReleased = false;
            if(jumpCoroutine != null)
                StopCoroutine(jumpCoroutine);

            jumpCoroutine = Jump(JUMP_POWER, 0.5f, true);
            StartCoroutine(jumpCoroutine);
            currentJumps--;
        }
    }

    //USed to apply the jump force.
    public void ApplyJumpForce(float jumpStrength)
    {
        StartCoroutine(Jump(jumpStrength, 0.3f, false));
    }

    // Used to complete the jump logic 
    IEnumerator Jump(float jumpPower, float timeAppliedFor, bool setTrigger)
    {
        //Debug.Log("we have launched with a force of " + jumpPower + " and and bool of: " + setTrigger);
        if (setTrigger)
        {
            anim.SetTrigger("Jump");
            playerState = PlayerState.Jumping;
        }

        grounded = false;
        gravityVectorStrength = jumpPower;

        yield return new WaitForSeconds(timeAppliedFor);

        if (playerState == PlayerState.LossOfControl || playerState == PlayerState.LossOfControlNoGravity )
        {
            //Debug.Log("we have some sort of cc on us");
        }
        else
            playerState = PlayerState.Airborne;
    }

    // Used to check and see if the player has started a roll action.
    private void CheckRoll()
    {
        if (Input.GetAxisRaw(inputs.rollInput) != 0 && grounded && (playerState != PlayerState.Airborne && playerState != PlayerState.LossOfControl && playerState != PlayerState.LossOfControlNoGravity) && rollReady && !menuOpen)
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

        buffsManager.ProcOnRoll();

        anim.SetTrigger("Roll");
        if(playerStats.movespeedPercentMultiplier >= 0.25f)
            anim.SetFloat("AnimSpeed", 1 * playerStats.movespeedPercentMultiplier);
        else
            anim.SetFloat("AnimSpeed", 1 * 0.25f);

        playerState = PlayerState.Rolling;

        // Ensures we face forward after a roll.
        recentlyAttacked = false;
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
        if (Input.GetAxisRaw(inputs.attackInput) != 0 && attackReady && !playerStats.stunned && !playerStats.asleep && !menuOpen)
        {
            attackCoroutine = Attack();
            StartCoroutine(attackCoroutine);
        }
    }

    // Used to start the attack logic.
    IEnumerator Attack()
    {
        buffsManager.ProcOnAttack();
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

    // Called when the player is revived and revived back from the downed state.
    public void PlayerRevived()
    {
        StopAllCoroutines();
        playerState = PlayerState.Idle;
        anim.SetTrigger("Revived");
    }

    // Used when the player gets stunned
    public void StunLaunch()
    {
        StartCoroutine(Stunned());
    }

    // The stunned coroutine. Makes the player unable to take action.
    IEnumerator Stunned()
    {
        if(!playerStats.frozen)
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
        ragdollManager.LerpBonesToGetUpAnim();

        yield return new WaitForSeconds(ragdollManager.lerpTime);

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

    // Used to make the player do a pickup animation.
    public void CheckInteract()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Debug.Log("an interact attempt was made");
            if (inventory.interactablesInRange.Count > 0)
            {
                // Here we check what kind of interactable it is and then interact with it accordingly.
                GameObject interactable = inventory.GrabClosestInteractable();
                anim.SetTrigger("Interact");

                if (interactable.GetComponent<ChestBehaviour>() != null)
                    interactable.GetComponent<ChestBehaviour>().OpenChest();
                else if (interactable.GetComponent<DoorOpenVolumeBehaviour>() != null)
                    interactable.GetComponent<DoorOpenVolumeBehaviour>().InteractWithDoor();

                inventory.interactablesInRange.Remove(interactable);
            }
            else if (inventory.itemsInRange.Count > 0)
            {
                inventory.PickUpItem(inventory.GrabClosestItem());
                anim.SetTrigger("PickUp");
            }
        }
    }

    // Check if the menu inputs were pressed. If so display the proper menu.
    private void CheckMenuInputs()
    {
        // If we have pressed the inventory window, 
        if (Input.GetAxisRaw(inputs.inventoryInput) == 1 && inputs.inventoryReleased)
        {
            inputs.inventoryReleased = false;
            //Debug.Log("Inventory will be opened or closed");
            if (!inventoryWindow.activeSelf)
            {
                // Set the lock for our movement and camera controls after we press and open the inventory.
                inventoryWindow.SetActive(true);
                menuOpen = true;
                cameraControls.menuOpen = true;
            }
            else
            {
                // Remvoe the lock for our movement and camera controls after we press and open the inventory.
                inventoryWindow.SetActive(false);
                inventoryWindow.GetComponent<InventoryPopupTextManager>().lockPointer = false;
                inventoryWindow.GetComponent<InventoryPopupTextManager>().HidePopups();
                menuOpen = false;
                cameraControls.menuOpen = false;
            }
        }
    }

    // Used when we enter a trigger and it has a room tag
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("RoomVolume"))
        {
            //Debug.Log("We have entered a new room");
            GameManager.instance.ShowRoom(other.transform.parent.GetComponent<RoomManager>());
        }
    }

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

    // USed to change the gravity modifier so we fall faster. Must be in a method so the animation clips can access it.
    public void ChangeGravityModifier(float value)
    {
        gravityModifier = value;
    }
}
