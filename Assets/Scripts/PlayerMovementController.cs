using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    public enum PlayerState { Idle, Moving, Airborne, Rolling, Sprinting, Attacking, Downed, Dead, Stunned, Asleep, CastingNoMovement, CastingRollOut, CastingWithMovement, Jumping}
    public PlayerState playerState = PlayerState.Idle;

    [HideInInspector] public bool menuOpen = false;
    [SerializeField] private Color bleedDamageColor;

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

    private bool attackReady = true;

    private bool rollReady = true;

    private bool grounded = true;
    [HideInInspector] public bool stunned = false;
    [HideInInspector] public bool asleep = false;
    [HideInInspector] public bool bleeding = false;
    private float gravityVectorStrength = 0f;
    private Ray groundRay;
    private RaycastHit groundRayHit;
    [SerializeField] private LayerMask groundingRayMask = 1 << 10;

    private const float GRAVITY = 0.4f;
    private const float GROUNDING_RAY_LENGTH = 0.7f;
    private const float JUMP_POWER = 0.18f;
    private const float ROLL_SPEED_MULTIPLIER = 2f;
    private const float ROLL_ANIMSPEED_MULITPLIER = 0.6f;
    private const float ATTACK_ANIMSPEED_MULTIPLIER = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        inputs = GetComponent<PlayerInputs>();
        buffsManager = GetComponent<BuffsManager>();
        playerStats = GetComponent<PlayerStats>();
        hitBoxManager = GetComponent<HitBoxManager>();

        mainCameraTransform = Camera.main.transform;

        // This is used to grab the length of animation clips
        //float AnimTestTimer = 99;
        //RuntimeAnimatorController ac = anim.runtimeAnimatorController;
        //for(int i = 0; i < ac.animationClips.Length; i ++)
        //{
        //    if (ac.animationClips[i].name == "Sword-Attack-R3")
        //        AnimTestTimer = ac.animationClips[i].length;
        //}
        //Debug.Log(AnimTestTimer);
    }

    // Update is called once per frame
    void Update()
    {
        switch (playerState)
        {
            case PlayerState.Idle:
                Move();
                ApplyGravity();
                CheckJump();
                CheckRoll();
                CheckAttack();
                break;
            case PlayerState.Moving:
                Move();
                ApplyGravity();
                CheckJump();
                CheckRoll();
                CheckAttack();
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
                break;
            case PlayerState.CastingRollOut:
                break;
            case PlayerState.CastingWithMovement:
                break;
            default:
                break;
        }
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
            controller.Move(positionalDifference);

            anim.SetBool("Grounded", true);
            if (playerState == PlayerState.Airborne)
                playerState = PlayerState.Idle;
        }
        else
        {
            grounded = false;
            anim.SetBool("Grounded", false);
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
        yield return new WaitForSeconds(0.3f);
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
            Debug.Log("There is a movement input");
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
        if (Input.GetAxisRaw(inputs.attackInput) != 0 && attackReady && !stunned && !asleep && !menuOpen)
            StartCoroutine(Attack());
    }

    // Used to start the attack logic.
    IEnumerator Attack()
    {
        attackReady = false;
        anim.SetTrigger("Attack");
        playerState = PlayerState.Attacking;

        // set up our timers here.
        float currentTimer = 0;
        // This represents the time we must wait: the base length of the animation clip times the speed modifier inherent in the animation clip editor plus some leeway for when the animation ends and blends away.
        float targetTimer = 0.8f / ATTACK_ANIMSPEED_MULTIPLIER * 0.6f;
        Debug.Log("the target timer is: " + targetTimer);
        bool attackLaunched = false;
        bool breakLoop = false;

        // Start the timer.
        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;
            // Laucnh the attack if we have awaited for half of the animation.
            if (!attackLaunched && currentTimer > targetTimer * 0.6)
            {
                Debug.Log("The attack has hit");
                attackLaunched = true;
                hitBoxManager.LaunchHitBox(0);
                // If the player has the bleeding debuff, we take damage.
                if (bleeding)
                    playerStats.TakeDamage(playerStats.healthMax * 0.1f, false, bleedDamageColor);
            }
            // Break away from this coroutine if we start a roll.
            if (!rollReady || stunned || asleep)
            {
                breakLoop = true;
                break;
            }
            yield return new WaitForEndOfFrame();
        }

        Debug.Log("the attack has ended");
        if (!breakLoop)
            playerState = PlayerState.Idle;
        attackReady = true;
    }

}
