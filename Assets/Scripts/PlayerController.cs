using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum PlayerState { Idle, Moving, Airborne, Rolling, Sprinting, Attacking, Downed, Dead, Staggered}
    public PlayerState playerState = PlayerState.Idle;
    public GameObject inventoryWindow;

    private PlayerStats playerStats;
    private PlayerInputs playerInputs;
    private HitBoxManager hitBoxManager;
    private Rigidbody rb;
    private Animator anim;
    [SerializeField] private Transform cameraParent;
    private Vector3 targetRotation;
    private CameraControls cameraControls;
    private Inventory inventory;

    private bool grounded = true;
    private bool justJumped = false;
    private bool rollReady = true;
    private bool attackReady = true;
    private bool staggered = false;
    private Ray groundRay;
    private RaycastHit groundRayHit;
    private LayerMask groundingRayMask = 1 << 10;
    private AnimatorClipInfo[] attackClip;
    private bool menuOpen = false;

    private Vector3 movement;

    private const float SPEED_MULTIPLIER = 100;
    private const float PLAYER_ROTATION_SPEED = 25;
    private const float GROUNDING_RAY_LENGTH = 0.6f;
    private const float JUMP_POWER = 10;
    private const float GRAVITY = 20;
    private const float ROLL_DURATION = 0.5f;
    private const float ROLL_SPEED_MULTIPLIER = 2f;
    private const float SPRINT_MULTIPLIER = 1.4f;
    private const float ANIM_SPEED_REDUCTION = 5f;
    private const float ATTACK_DURATION = 0.5f;
    private const float ATTACK_SPEED_MULTIPLIER = 0.4f;
    private const float STAGGER_DURATION = 0.8f;

    // Start is called before the first frame update
    void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        playerInputs = GetComponent<PlayerInputs>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        hitBoxManager = GetComponent<HitBoxManager>();
        cameraControls = cameraParent.GetComponentInChildren<CameraControls>();
        inventory = GetComponent<Inventory>();
    }

    // Update is called once per frame, we call different functions based on our current state.
    void Update()
    {
        switch (playerState)
        {
            case PlayerState.Idle:
                if (Input.GetAxisRaw(playerInputs.horizontalInput) != 0 || Input.GetAxisRaw(playerInputs.verticalInput) != 0)
                    playerState = PlayerState.Moving;
                // Check the movement and jump inputs.
                PlayerMovement();
                PlayerRotation();
                CheckRoll();
                CheckJump();
                CheckGrounded();
                CheckAttack();
                CheckInteract();
                break;
            case PlayerState.Moving:
                if (Input.GetAxisRaw(playerInputs.horizontalInput) == 0 && Input.GetAxisRaw(playerInputs.verticalInput) == 0)
                    playerState = PlayerState.Idle;
                if (Input.GetAxis(playerInputs.sprintInput) != 0)
                    playerState = PlayerState.Sprinting;
                // Check the movement and jump inputs.
                PlayerMovement();
                PlayerRotation();
                CheckRoll();
                CheckJump();
                CheckGrounded();
                CheckAttack();
                CheckInteract();
                break;
            case PlayerState.Airborne:
                PlayerMovement();
                PlayerRotation();
                CheckGrounded();
                break;
            case PlayerState.Rolling:
                break;
            case PlayerState.Sprinting:
                if (Input.GetAxisRaw(playerInputs.horizontalInput) == 0 && Input.GetAxisRaw(playerInputs.verticalInput) == 0)
                    playerState = PlayerState.Idle;
                if (Input.GetAxis(playerInputs.sprintInput) == 0)
                    playerState = PlayerState.Moving;
                PlayerMovement();
                PlayerRotation();
                CheckRoll();
                CheckJump();
                CheckGrounded();
                CheckAttack();
                CheckInteract();
                break;
            case PlayerState.Attacking:
                PlayerMovement();
                PlayerRotation();
                CheckRoll();
                CheckJump();
                CheckGrounded();
                break;
            case PlayerState.Downed:
                // Here i would put the revive logic.
                break;
            default:
                break;
        }
        CheckMenuInputs();
    }

    // Checks the movement inputs and moves the player in relation to the curent rotation of the camera.
    private void PlayerMovement()
    {
        Vector3 horizontalMovement = Vector3.zero;

        horizontalMovement.x = Input.GetAxis(playerInputs.horizontalInput);
        horizontalMovement.z = Input.GetAxis(playerInputs.verticalInput);
        if (playerState == PlayerState.Sprinting)
        {
            horizontalMovement = horizontalMovement.normalized * playerStats.speed * Time.deltaTime * SPEED_MULTIPLIER * SPRINT_MULTIPLIER;
            anim.SetFloat("AnimSpeed", playerStats.speed * SPRINT_MULTIPLIER / ANIM_SPEED_REDUCTION);
        }
        else if(playerState == PlayerState.Attacking)
        {
            horizontalMovement = horizontalMovement.normalized * playerStats.speed * Time.deltaTime * SPEED_MULTIPLIER * ATTACK_SPEED_MULTIPLIER;
            // anim.SetFloat("AnimSpeed", playerStats.speed / ANIM_SPEED_REDUCTION);
        }
        else
        {
            horizontalMovement = horizontalMovement.normalized * playerStats.speed * Time.deltaTime * SPEED_MULTIPLIER;
            anim.SetFloat("AnimSpeed", playerStats.speed / ANIM_SPEED_REDUCTION);
        }

        movement.x = horizontalMovement.x;
        movement.z = horizontalMovement.z;
        movement = Quaternion.AngleAxis(cameraParent.rotation.eulerAngles.y, Vector3.up) * movement;
        
        rb.velocity = movement;
        rb.angularVelocity = Vector3.zero;

        // Set the parameters of our aniamtor so it shows the proper animation.
        anim.SetFloat("Speed", movement.normalized.sqrMagnitude);
    }

    // Used to make the character face the direction they are going towards.
    private void PlayerRotation()
    {
        Vector3 horizontalMovement = movement;
        horizontalMovement.y = 0;
        if (horizontalMovement.sqrMagnitude >= 0.2)
        {
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(movement.x, 0, movement.z).normalized, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, PLAYER_ROTATION_SPEED / 100);
        }
    }
    
    // Used to check if the player is still grounded
    private void CheckGrounded()
    {
        Ray groundRay = new Ray(transform.position + Vector3.up * 0.5f, Vector3.down);
        RaycastHit groundRayHit;
        // Shoot a ray, if it we hit we are grounded if not we are no longer grounded. If we just jumped ignore this and set us as not grounded.
        if(Physics.Raycast(groundRay, out groundRayHit, GROUNDING_RAY_LENGTH, groundingRayMask) && !justJumped)
        {
            grounded = true;
            movement.y = 0;
            anim.SetBool("Grounded", true);
            if (playerState == PlayerState.Airborne)
                playerState = PlayerState.Idle;
        }
        else
        {
            grounded = false;
            movement.y -= GRAVITY * Time.deltaTime;
            anim.SetBool("Grounded", false);
            playerState = PlayerState.Airborne;
        }
    }

    // Used to check if the player's jump imput was pressed.
    private void CheckJump()
    {
        if (Input.GetAxisRaw(playerInputs.jumpInput) != 0 && grounded && !justJumped && (playerState != PlayerState.Rolling || playerState != PlayerState.Staggered) && !menuOpen)
            StartCoroutine(Jump());
    }

    // Used to complete the jump logic 
    IEnumerator Jump()
    {
        anim.SetTrigger("Jump");
        justJumped = true;
        grounded = false;
        movement.y = JUMP_POWER;
        yield return new WaitForSeconds(0.5f);
        justJumped = false;
    }

    // Used to check and see if the player has started a roll action.
    private void CheckRoll()
    {
        if (Input.GetAxisRaw(playerInputs.rollInput) != 0 && grounded && (playerState != PlayerState.Airborne && playerState != PlayerState.Staggered)&& rollReady && !menuOpen)
            StartCoroutine(Roll());
    }

    // Used to compelte the roll logic.
    IEnumerator Roll()
    {
        rollReady = false;
        anim.SetTrigger("Roll");
        playerState = PlayerState.Rolling;
        anim.SetFloat("AnimSpeed", playerStats.speed * ROLL_SPEED_MULTIPLIER / ANIM_SPEED_REDUCTION);

        // Grab the horizontal vector, if it is too small we'll use the direction we are facing instead.
        Vector3 horizontalMovement = movement;
        horizontalMovement.y = 0;
        if (horizontalMovement.sqrMagnitude >= 0.2)
        {
            movement = horizontalMovement.normalized * Time.deltaTime * playerStats.speed * SPEED_MULTIPLIER * ROLL_SPEED_MULTIPLIER;
        }
        else
        {
            movement = transform.forward * Time.deltaTime * playerStats.speed * SPEED_MULTIPLIER * ROLL_SPEED_MULTIPLIER;
        }
        rb.velocity = movement;

        yield return new WaitForSeconds(ROLL_DURATION * ANIM_SPEED_REDUCTION / playerStats.speed);
        playerState = PlayerState.Idle;
        rollReady = true;
    }

    // Used to check if the basic attack input was pressed.
    private void CheckAttack()
    {
        if (Input.GetAxisRaw(playerInputs.attackInput) != 0 && grounded && attackReady && (playerState != PlayerState.Airborne || playerState != PlayerState.Rolling) && !staggered && !menuOpen)
            StartCoroutine(Attack());
    }

    // Used to start the attack logic.
    IEnumerator Attack()
    {
        attackReady = false;
        anim.SetTrigger("Attack");
        playerState = PlayerState.Attacking;
        anim.SetFloat("AttackAnimSpeed", playerStats.attackSpeed);  

        float currentTimer = 0;
        float targetTimer = 1 / playerStats.attackSpeed;
        bool attackLaunched = false;
        bool breakLoop = false;
        // Start the timer.
        while(currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;
            // Laucnh the attack if we have awaited for half of the animation.
            if(!attackLaunched && currentTimer > targetTimer /2)
            {
                attackLaunched = true;
                hitBoxManager.LaunchHitBox(0);
            }
            // Break if we start a roll.
            if(!rollReady || staggered)
            {
                breakLoop = true;
                break;
            }
            yield return new WaitForEndOfFrame();
        }

        if(!breakLoop)
            playerState = PlayerState.Idle;
        attackReady = true;

        // Debug.Log("attack done");
    }

    // Called when the player is hit below 0 hp and downed.
    public void PlayerDowned()
    {
        StopAllCoroutines();
        playerState = PlayerState.Downed;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        anim.SetTrigger("Downed");
    }

    // USed to stagger the player when their poise gets broken.
    public void StaggerLaunch()
    {
        StartCoroutine(Stagger());
    }

    // The stagger coroutine. Laucnehs a player back a setp or based based on a direction, and makes them unable to act for this time.
    IEnumerator Stagger()
    {
        staggered = true;
        playerState = PlayerState.Staggered;
        rb.velocity = new Vector3(0, rb.velocity.y, 0);
        rb.angularVelocity = Vector3.zero;
        anim.SetTrigger("Staggered");
        yield return new WaitForSeconds(STAGGER_DURATION);
        playerState = PlayerState.Idle;
        staggered = false;
    }

    // Used to make the player do a pickup animation.
    public void CheckInteract()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (inventory.interactablesInRange.Count > 0)
            {
                // Here we check what kind of interactable it is and then interact with it accordingly.
                GameObject interactable = inventory.GrabClosestInteractable();
                anim.SetTrigger("Interact");

                if (interactable.GetComponent<ChestBehaviour>() != null)
                    interactable.GetComponent<ChestBehaviour>().OpenChest();

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
        if(Input.GetAxisRaw(playerInputs.inventoryInput) == 1 && playerInputs.inventoryReleased)
        {
            playerInputs.inventoryReleased = false;
            Debug.Log("Inventory will be opened or closed");
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
