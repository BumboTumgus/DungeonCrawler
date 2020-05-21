using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    public enum PlayerState { Idle, Moving, Airborne, Rolling, Sprinting, Attacking, Downed, Dead, Stunned, Asleep, CastingNoMovement, CastingRollOut, CastingWithMovement, Jumping}
    public PlayerState playerState = PlayerState.Idle;

    [HideInInspector] public bool menuOpen = false;
    public GameObject inventoryWindow;

    [SerializeField] private float movementSpeed = 2f;
    private float currentSpeed = 0f;
    private float speedSmoothVelocity = 0f;
    private float speedSmoothTime = 0.1f;
    private float rotationSpeed = 0.1f;

    private Transform mainCameraTransform = null;

    private CharacterController controller = null;
    private PlayerInputs inputs = null;
    private Animator anim = null;
    private BuffsManager buffsManager;
    private PlayerStats playerStats;
    private HitBoxManager hitBoxManager;
    private Inventory inventory;
    private CameraControls cameraControls;

    private bool attackReady = true;

    private bool rollReady = true;

    private bool grounded = true;
    private float gravityVectorStrength = 0f;
    private Ray groundRay;
    private RaycastHit groundRayHit;
    [SerializeField] private LayerMask groundingRayMask = 1 << 10;

    private const float GRAVITY = 0.4f;
    private const float GROUNDING_RAY_LENGTH = 0.7f;
    private const float JUMP_POWER = 0.18f;
    private const float ROLL_SPEED_MULTIPLIER = 2f;
    private const float ROLL_ANIMSPEED_MULITPLIER = 0.6f;
    //private const float POSITIONAL_DIFFERENCE_OFFSET = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        mainCameraTransform = Camera.main.transform;

        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        inputs = GetComponent<PlayerInputs>();
        buffsManager = GetComponent<BuffsManager>();
        playerStats = GetComponent<PlayerStats>();
        hitBoxManager = GetComponent<HitBoxManager>();
        cameraControls = mainCameraTransform.GetComponentInChildren<CameraControls>();
        inventory = GetComponent<Inventory>();

        // This is used to grab the length of animation clips
        //float AnimTestTimer = 99;
        //RuntimeAnimatorController ac = anim.runtimeAnimatorController;
        //for(int i = 0; i < ac.animationClips.Length; i ++)
        //{
        //    if (ac.animationClips[i].name == "ArmedAttackDual3_Cleaned")
        //        AnimTestTimer = ac.animationClips[i].length;
        //}
        //Debug.Log(AnimTestTimer);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
            PlayerDowned();
        if (Input.GetKeyDown(KeyCode.L))
            PlayerRevived();
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
                break;
            case PlayerState.Attacking:
                Move();
                ApplyGravity();
                CheckJump();
                CheckRoll();
                break;
            case PlayerState.Downed:
                break;
            case PlayerState.Dead:
                break;
            case PlayerState.Stunned:
                break;
            case PlayerState.Asleep:
                break;
            case PlayerState.CastingNoMovement:
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
            default:
                break;
        }
        CheckMenuInputs();
    }

    private void Move()
    {
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

        // Rotate towards the target move direction.
        if (desiredMoveDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), rotationSpeed);
            anim.SetFloat("Speed", 1);
        }
        else
            anim.SetFloat("Speed", 0);

        // Set up the players speed.
        float targetSpeed = movementSpeed * movementInput.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);

        controller.Move(desiredMoveDirection * currentSpeed * Time.deltaTime);
    }

    // Applies a downward force to the player if they are not grounded
    private void ApplyGravity()
    {
        Ray groundRay = new Ray(transform.position + Vector3.up * 0.5f, Vector3.down);
        RaycastHit groundRayHit;

        // Shoot a ray, if it we hit we are grounded if not we are no longer grounded. If we just jumped ignore this and set us as not grounded.
        if (Physics.Raycast(groundRay, out groundRayHit, GROUNDING_RAY_LENGTH, groundingRayMask) && playerState != PlayerState.Jumping)
        {
            grounded = true;
            gravityVectorStrength = 0;

            Vector3 positionalDifference = groundRayHit.point - transform.position;
            //positionalDifference.y -= POSITIONAL_DIFFERENCE_OFFSET;
            controller.Move(positionalDifference);

            //Debug.Log("The positional difference is: " + positionalDifference + ". Our transform is: " + transform.position);

            anim.SetBool("Grounded", true);
            if (playerState == PlayerState.Airborne)
                playerState = PlayerState.Idle;
        }
        else
        {
            grounded = false;
            anim.SetBool("Grounded", false);
            if(playerState != PlayerState.Jumping)
                playerState = PlayerState.Airborne;

            Vector3 gravityVector = Vector3.zero;

            gravityVectorStrength -= GRAVITY * Time.deltaTime;
            gravityVector.y = gravityVectorStrength;

            controller.Move(gravityVector);
        }

    }

    // Used to check if the player's jump imput was pressed.
    private void CheckJump()
    {
        if (Input.GetAxisRaw(inputs.jumpInput) != 0 && grounded && !menuOpen)
            StartCoroutine(Jump());
    }

    // Used to complete the jump logic 
    IEnumerator Jump()
    {
        anim.SetTrigger("Jump");
        playerState = PlayerState.Jumping;
        grounded = false;
        gravityVectorStrength = JUMP_POWER;
        yield return new WaitForSeconds(0.5f);
        playerState = PlayerState.Airborne;
    }

    // Used to check and see if the player has started a roll action.
    private void CheckRoll()
    {
        if (Input.GetAxisRaw(inputs.rollInput) != 0 && grounded && (playerState != PlayerState.Airborne && playerState != PlayerState.Stunned) && rollReady && !menuOpen)
            StartCoroutine(Roll());
    }

    // Used to compelte the roll logic.
    IEnumerator Roll()
    {
        rollReady = false;
        anim.SetTrigger("Roll");
        playerState = PlayerState.Rolling;

        // here we see if we are on fire, if so lower the duration of the debuff.
        foreach (Buff buff in buffsManager.activeBuffs)
        {
            Debug.Log("we are checking for a fire debuff");
            if (buff.myType == BuffsManager.BuffType.Aflame)
            {
                Debug.Log("we found one");
                buff.currentTimer += 7;
                AfflictionManager afflictionManager = GetComponent<AfflictionManager>();

                afflictionManager.currentAflameValue -= ((7 / buff.duration) * 100);
                if (afflictionManager.currentAflameValue < 0.1f)
                    afflictionManager.RemoveBar(buff.myType);
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
        float targetTimer = 0.8f / ROLL_ANIMSPEED_MULITPLIER * 0.7f;
        //float targetTimer = anim.GetCurrentAnimatorClipInfo(5).Length;
        //Debug.Log("the target timer is: " + targetTimer);

        while(currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;
            
            // Rotate towards the target move direction.
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), rotationSpeed);

            // Set up the players speed.
            float targetSpeed = movementSpeed * ROLL_SPEED_MULTIPLIER;
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
            StartCoroutine(Attack());
    }

    // Used to start the attack logic.
    IEnumerator Attack()
    {
        attackReady = false;
        anim.SetTrigger("Attack");
        anim.SetFloat("AttackAnimSpeed", playerStats.attackSpeed);
        playerState = PlayerState.Attacking;

        // set up our timers here.
        float currentTimer = 0;
        // This represents the time we must wait: the base length of the animation clip times the speed modifier inherent in the animation clip editor plus some leeway for when the animation ends and blends away.
        float targetTimer = 0.8f / playerStats.attackSpeed;
        //Debug.Log("the target timer is: " + targetTimer);
        bool breakLoop = false;

        // Start the timer.
        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;

            // Break away from this coroutine if we start a roll.
            if (playerState == PlayerState.Rolling || playerStats.stunned || playerStats.asleep)
            {
                breakLoop = true;
                break;
            }
            yield return new WaitForEndOfFrame();
        }

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
        playerStats.stunned = true;
        playerState = PlayerState.Stunned;
        anim.SetBool("Stunned", true);

        while (playerStats.stunned)
            yield return null;

        playerState = PlayerState.Idle;
        playerStats.stunned = false;
        anim.SetBool("Stunned", false);
    }

    // Used when the player gets stunned
    public void AsleepLaunch()
    {
        StartCoroutine(Asleep());
    }

    // The stunned coroutine. Makes the player unable to take action.
    IEnumerator Asleep()
    {
        playerStats.asleep = true;
        playerState = PlayerState.Asleep;
        anim.SetBool("Sleeping", true);

        while (playerStats.asleep)
            yield return null;

        playerState = PlayerState.Idle;
        playerStats.asleep = false;
        anim.SetBool("Sleeping", false);
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
                else if (interactable.GetComponent<DoorBehaviour>() != null)
                    interactable.GetComponent<DoorBehaviour>().InteractWithDoor();

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

}
