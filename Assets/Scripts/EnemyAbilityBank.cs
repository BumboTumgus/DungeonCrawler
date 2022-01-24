using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAbilityBank : MonoBehaviour
{
    public enum EnemyAbility { None, ThrowingAxe, GroundSlam, SummonGoblins, GoblinSeeker, AcidShot, SnakeCurseShot, Ephemeral, RockGolemSlamAOE, RockGolemSlamShockwave, SummonIceWolf, GolemTriplePunch, GolemBellyflop, GolemRockThrow, ForgeGiantStomp, ForgeGiantTracerOrb, ForgeGiantChestLaser};

    public GameObject[] spellProjectiles;
    public GameObject[] spellSummons;
    public ParticleSystem[] spellParticles;
    public GameObject[] ephemeralObjectsToHide;

    public GameObject targetDesignator;
    public Transform designatorOrigin;

    [SerializeField] private LayerMask rayMask;

    [SerializeField] private Transform[] projectileSpawnReferences;

    private PlayerStats myStats;
    private EnemyCombatController combatController;
    private EnemyMovementManager movementManager;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        foreach (ParticleSystem ps in spellParticles)
            ps.Stop();
        myStats = GetComponent<PlayerStats>();
        combatController = GetComponent<EnemyCombatController>();
        movementManager = GetComponent<EnemyMovementManager>();
        anim = GetComponent<Animator>();
    }
    
    // Depending on the spell, start the proper coroutine.
    public void CastSpell(EnemyAbility spellType)
    {
        //Debug.Log("we are casting a spell");
        switch (spellType)
        {
            case EnemyAbility.None:
                combatController.SwitchAction(EnemyCombatController.ActionType.Attack);
                break;
            case EnemyAbility.ThrowingAxe:
                StartCoroutine(ThrowingAxe());
                break;
            case EnemyAbility.GroundSlam:
                StartCoroutine(GroundSlam());
                break;
            case EnemyAbility.SummonGoblins:
                StartCoroutine(SummonGoblins());
                break;
            case EnemyAbility.GoblinSeeker:
                StartCoroutine(GoblinSeeker());
                break;
            case EnemyAbility.AcidShot:
                StartCoroutine(AcidShot());
                break;
            case EnemyAbility.SnakeCurseShot:
                StartCoroutine(SnakeCurseShot());
                break;
            case EnemyAbility.Ephemeral:
                StartCoroutine(Ephemeral());
                break;
            case EnemyAbility.RockGolemSlamAOE:
                StartCoroutine(RockGolemSlamAOE());
                break;
            case EnemyAbility.RockGolemSlamShockwave:
                StartCoroutine(RockGolemSlamShockwave());
                break;
            case EnemyAbility.SummonIceWolf:
                StartCoroutine(SummonIceWolf());
                break;
            case EnemyAbility.GolemTriplePunch:
                StartCoroutine(GolemTriplePunch());
                break;
            case EnemyAbility.GolemBellyflop:
                StartCoroutine(GolemBellyflop());
                break;
            case EnemyAbility.GolemRockThrow:
                StartCoroutine(GolemRockThrow());
                break;
            case EnemyAbility.ForgeGiantStomp:
                StartCoroutine(ForgeGiantStomp());
                break;
            case EnemyAbility.ForgeGiantTracerOrb:
                StartCoroutine(ForgeGiantTracerOrb());
                break;
            case EnemyAbility.ForgeGiantChestLaser:
                StartCoroutine(ForgeGiantChestLaser());
                break;
            default:
                break;
        }
    }

    public void ShootProjectileForward(int index)
    {
        Vector3 forward = combatController.myTarget.transform.position - transform.position;
        switch (index)
        {
            case 0:
                // Instantiate the obhect, set it's damage and aim it at the player.
                GameObject axe = Instantiate(spellProjectiles[0], transform.position + Vector3.up, Quaternion.LookRotation(forward, Vector3.up));
                axe.GetComponent<HitBox>().damage = myStats.baseDamage * 1.5f;
                axe.GetComponent<HitBox>().myStats = myStats;
                break;
            case 1:
                // Instantiate the obhect, set it's damage and aim it at the player.
                GameObject acidShot = Instantiate(spellProjectiles[0], transform.position + Vector3.up, Quaternion.LookRotation(forward, Vector3.up));
                acidShot.GetComponent<HitBox>().damage = myStats.baseDamage * 1;
                acidShot.GetComponent<HitBox>().myStats = myStats;
                acidShot.GetComponent<HitBoxBuff>().buffOrigin = myStats;
                break;
            case 2:
                // Instantiate the obhect, set it's damage and aim it at the player.
                for (int i = 0; i < 5; i++)
                {
                    GameObject snakeShot = Instantiate(spellProjectiles[0], transform.position + Vector3.up, Quaternion.LookRotation(forward, Vector3.up));

                    Vector3 rotation = snakeShot.transform.rotation.eulerAngles;
                    rotation.y += (i - 2) * 6;
                    snakeShot.transform.rotation = Quaternion.Euler(rotation);

                    snakeShot.GetComponent<HitBox>().damage = myStats.baseDamage * 2;
                    snakeShot.GetComponent<HitBox>().myStats = myStats;
                }
                break;
            case 3:
                GameObject groundSlam = Instantiate(spellProjectiles[0], transform.position + transform.forward * 2, transform.rotation);
                groundSlam.GetComponent<HitBox>().damage = myStats.baseDamage * 3;
                groundSlam.GetComponent<HitBox>().myStats = myStats;
                groundSlam.GetComponent<HitBoxBuff>().buffOrigin = myStats;
                spellParticles[0].Stop();
                spellParticles[1].Stop();
                break;
            case 4:
                spellParticles[2].Stop();
                spellParticles[3].Play();
                Instantiate(spellSummons[0], transform.position + transform.forward * 1.5f + transform.right * 1.5f, transform.rotation);
                Instantiate(spellSummons[0], transform.position + transform.forward * -1.5f + transform.right * 1.5f, transform.rotation);
                Instantiate(spellSummons[0], transform.position + transform.forward * 1.5f + transform.right * -1.5f, transform.rotation);
                Instantiate(spellSummons[0], transform.position + transform.forward * -1.5f + transform.right * -1.5f, transform.rotation);
                // Instantiate the goblins.
                break;
            case 5:
                Instantiate(spellSummons[0], transform.position + transform.right * 1.5f, transform.rotation);
                break;
            case 6:
                // Instantiate the obhect, set it's damage and aim it at the player.
                for (int i = 0; i < 7; i++)
                {
                    GameObject rockShot = Instantiate(spellProjectiles[0], transform.position + Vector3.up * 1.3f + transform.forward * 1.3f, Quaternion.LookRotation(forward, Vector3.up));

                    Vector3 rotation = rockShot.transform.rotation.eulerAngles;
                    rotation.y += Random.Range(-15f, 15f);
                    rotation.x += Random.Range(-20f, 5f);
                    rockShot.transform.rotation = Quaternion.Euler(rotation);

                    rockShot.GetComponent<HitBox>().damage = myStats.baseDamage;
                    rockShot.GetComponent<HitBox>().myStats = myStats;
                }
                break;
            case 7:
                // Create the Tracer orb spawner.
                GameObject tracerOrb = Instantiate(spellProjectiles[0], projectileSpawnReferences[0].position, Quaternion.LookRotation(forward, Vector3.up));
                tracerOrb.GetComponent<HitBox>().damage = myStats.baseDamage * 0.5f;
                tracerOrb.GetComponent<HitBox>().myStats = myStats;
                tracerOrb.GetComponent<HitBox>().enemyStats = combatController.myTarget.GetComponent<PlayerStats>();
                break;
            case 8:
                // Shoot a ray and check to see what it hits.
                RaycastHit rayhit;
                Vector3 beamTarget = combatController.myTarget.transform.position;
                float magnitude = (combatController.myTarget.transform.position - designatorOrigin.position).magnitude;

                if (magnitude > 100)
                    magnitude = 100;

                // Snapping to target Logic.
                if (Physics.Raycast(new Ray(designatorOrigin.position, combatController.myTarget.transform.position - designatorOrigin.position), out rayhit, magnitude, rayMask))
                    beamTarget = rayhit.point;


                GameObject beam = Instantiate(spellProjectiles[1], transform.position, Quaternion.identity);
                beam.GetComponent<LineRenderer>().SetPositions(new Vector3[] { designatorOrigin.position, beamTarget });

                GameObject beamExplosion = Instantiate(spellProjectiles[2], beamTarget, Quaternion.identity);
                beamExplosion.GetComponent<HitBox>().damage = myStats.baseDamage * 5f;
                beamExplosion.GetComponent<HitBox>().myStats = myStats;
                break;
            default:
                break;
        }
    }

    IEnumerator RockGolemSlamShockwave()
    {
        //Debug.Log(" i am performing the slam shockwave");
        anim.SetTrigger("GroundSlamShockwave");
        movementManager.StopMovement();
        float currentTimer = 0;
        float numberOfTicks = 20;
        float currentTick = 0;
        float abilityRange = 10f;
        float currentTickTimer = 0;
        float targetTimer = 1f;
        float targetTickTimer = targetTimer / numberOfTicks;

        while(currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;
            movementManager.RotateToTarget(combatController.myTarget.transform.position);
            yield return new WaitForEndOfFrame();
        }

        combatController.CheckActionHierarchy();
        Vector3 forwardDirection = transform.forward;
        Vector3 originalPosition = transform.position;
        currentTimer = 0;
        targetTimer = 2;

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;
            currentTickTimer += Time.deltaTime;

            if (currentTickTimer > targetTickTimer)
            {
                currentTickTimer -= targetTickTimer;
                currentTick++;

                Vector3 rayOrigin = originalPosition + (forwardDirection * ((currentTick / numberOfTicks) * abilityRange));
                Ray ray = new Ray(rayOrigin, Vector3.down);
                RaycastHit rayhit;

                if(Physics.Raycast(ray, out rayhit, 25))
                {
                    //Vector3 instantiationPoint =

                    // Instantiate the object, set it's damage and aim it at the player.
                    GameObject groundSlam = Instantiate(spellProjectiles[1], rayhit.point, Quaternion.identity);
                    groundSlam.GetComponent<HitBox>().damage = myStats.baseDamage * 1;
                    groundSlam.GetComponent<HitBox>().myStats = myStats;
                }

            }
            movementManager.RotateToTarget(combatController.myTarget.transform.position);
            yield return new WaitForEndOfFrame();
        }

    }

    IEnumerator RockGolemSlamAOE()
    {
        //Debug.Log(" i am performing the slam AOE");
        anim.SetTrigger("GroundSlam");
        movementManager.StopMovement();
        float currentTimer = 0;
        float targetGroundSlamTimer = 1f;
        float targetTimer = 2;
        bool hitboxLaunched = false;

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;
            if (!hitboxLaunched && currentTimer > targetGroundSlamTimer)
            {
                hitboxLaunched = true;
                // Instantiate the obhect, set it's damage and aim it at the player.
                GameObject groundSlam = Instantiate(spellProjectiles[0], transform.position + transform.forward * 2, transform.rotation);
                groundSlam.GetComponent<HitBox>().damage = myStats.baseDamage * 5;
                groundSlam.GetComponent<HitBox>().myStats = myStats;
            }
            movementManager.RotateToTarget(combatController.myTarget.transform.position);
            yield return new WaitForEndOfFrame();
        }

        combatController.CheckActionHierarchy();
    }

    IEnumerator Ephemeral()
    {
        //Debug.Log("We are now ephemeral");
        float currentTimer = 0;
        float targetTimer = 5;
        myStats.AddEphemeralSource(1);
        GetComponent<Collider>().enabled = false;

        foreach (GameObject item in ephemeralObjectsToHide)
            item.SetActive(false);
        spellParticles[0].Play();
        spellParticles[1].Play();

        while(currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        //Debug.Log("we are no longer ephemeral");

        foreach (GameObject item in ephemeralObjectsToHide)
            item.SetActive(true);
        spellParticles[0].Play();
        spellParticles[1].Play();
        myStats.AddEphemeralSource(-1);
        GetComponent<Collider>().enabled = true;

        combatController.CheckActionHierarchy();
    }

    IEnumerator GoblinSeeker()
    {
        Debug.Log("Shooting a goblin Seeker");
        anim.SetTrigger("GoblinTracer");
        anim.SetFloat("Speed", 0);
        movementManager.StopMovement();
        float currentTimer = 0;
        float targetGoblinSeekerTimer = 0.6f;
        float targetTimer = 2;
        bool projectileLaunched = false;

        while(currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;

            if (!projectileLaunched && currentTimer > targetGoblinSeekerTimer)
            {
                projectileLaunched = true;
                // Instantiate the obhect, set it's damage and aim it at the player.
                Vector3 forward = combatController.myTarget.transform.position - transform.position;
                GameObject tracer = Instantiate(spellProjectiles[0], transform.position + Vector3.up, Quaternion.LookRotation(forward, Vector3.up));
                tracer.GetComponent<HitBox>().damage = myStats.baseDamage * 4;
                tracer.GetComponent<HitBox>().myStats = myStats;
                tracer.GetComponent<ProjectileBehaviour>().target = combatController.myTarget.transform;
            }
            yield return new WaitForEndOfFrame();
        }

        combatController.CheckActionHierarchy();
    }

    IEnumerator ThrowingAxe()
    {
        //Debug.Log(" i am performing the axe throw");
        anim.SetTrigger("AxeThrow");
        movementManager.StopMovement();
        float currentTimer = 0;
        float targetTimer = 1.4f;

        while(currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;
            movementManager.RotateToTarget(combatController.myTarget.transform.position);
            yield return new WaitForEndOfFrame();
        }

        combatController.CheckActionHierarchy();
    }

    IEnumerator GroundSlam()
    {
        //Debug.Log(" i am performing the ground slam");
        anim.SetTrigger("GroundSlam");
        spellParticles[0].Play();
        spellParticles[1].Play();
        movementManager.StopMovement();
        float currentTimer = 0;
        float targetTimer = 1.6f / 0.6f;
        
        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;
            movementManager.RotateToTarget(combatController.myTarget.transform.position);
            yield return new WaitForEndOfFrame();
        }

        combatController.CheckActionHierarchy();
    }

    IEnumerator SummonGoblins()
    {
        //Debug.Log("I am summon the globins");
        anim.SetTrigger("SummonGoblin");
        spellParticles[2].Play();
        movementManager.StopMovement();
        float currentTimer = 0;
        float targetTimer = 2.166f;

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;
            movementManager.RotateToTarget(combatController.myTarget.transform.position);
            yield return new WaitForEndOfFrame();
        }

        combatController.CheckActionHierarchy();
    }

    IEnumerator AcidShot()
    {
        anim.SetTrigger("AcidShot");
        movementManager.StopMovement();
        float currentTimer = 0;
        float targetTimer = 1.167f;


        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;
            movementManager.RotateToTarget(combatController.myTarget.transform.position);
            yield return new WaitForEndOfFrame();
        }

        combatController.CheckActionHierarchy();

    }

    IEnumerator SnakeCurseShot()
    {
        movementManager.StopMovement();
        float currentTimer = 0;
        float targetTimer = 2.8f;

        // Create the target Indicator
        TargetIndicatorController targetIndicatorController = Instantiate(targetDesignator).GetComponent<TargetIndicatorController>();
        targetIndicatorController.originAnchor = transform;
        targetIndicatorController.targetAnchor = combatController.myTarget.transform;

        while(currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        anim.SetTrigger("AcidShot");
        currentTimer = 0;
        targetTimer = 1;
        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;
            movementManager.RotateToTarget(combatController.myTarget.transform.position);
            yield return new WaitForEndOfFrame();
        }

        combatController.CheckActionHierarchy();
    }

    IEnumerator SummonIceWolf()
    {
        anim.SetTrigger("SummonIceWolf");
        movementManager.StopMovement();
        float currentTimer = 0;
        float targetTimer = 1.333f;

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;
            movementManager.RotateToTarget(combatController.myTarget.transform.position);
            yield return new WaitForEndOfFrame();
        }

        combatController.CheckActionHierarchy();

    }

    IEnumerator GolemTriplePunch()
    {
        anim.SetTrigger("TriplePunch");
        movementManager.StopMovement();
        float currentTimer = 0;
        float targetTimer = 2.334f;
        GetComponent<HitBoxManager>().hitboxes[1].GetComponent<HitBox>().damage = myStats.baseDamage;

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;
            movementManager.RotateToTarget(combatController.myTarget.transform.position);
            yield return new WaitForEndOfFrame();
        }

        combatController.CheckActionHierarchy();
    }

    IEnumerator GolemBellyflop()
    {
        Debug.Log("BELLLLLLLY FLOP");
        anim.SetTrigger("BellyFlop");
        movementManager.StopMovement();
        float currentTimer = 0;
        float targetTimer = 4f;
        GetComponent<HitBoxManager>().hitboxes[2].GetComponent<HitBox>().damage = myStats.baseDamage * 2;

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        combatController.CheckActionHierarchy();
    }

    IEnumerator GolemRockThrow()
    {
        Debug.Log("ROCK THROW OF DOOOOOM");
        anim.SetTrigger("RockThrow");
        movementManager.StopMovement();
        float currentTimer = 0;
        float targetTimer = 3.834f;

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;
            movementManager.RotateToTarget(combatController.myTarget.transform.position);
            yield return new WaitForEndOfFrame();
        }

        combatController.CheckActionHierarchy();
    }

    IEnumerator ForgeGiantStomp()
    {
        Debug.Log("STOMPING");
        anim.SetTrigger("Stomping");
        movementManager.StopMovement();
        float currentTimer = 0;
        float targetTimer = 5.034f;


        GetComponent<HitBoxManager>().hitboxes[1].GetComponent<HitBox>().damage = myStats.baseDamage;
        GetComponent<HitBoxManager>().hitboxes[2].GetComponent<HitBox>().damage = myStats.baseDamage;
        GetComponent<HitBoxManager>().hitboxes[3].GetComponent<HitBox>().damage = myStats.baseDamage;
        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        combatController.CheckActionHierarchy();
    }

    IEnumerator ForgeGiantTracerOrb()
    {
        Debug.Log("SUMMON TRACER ORB");
        anim.SetTrigger("SummonTracerOrb");
        movementManager.StopMovement();
        float currentTimer = 0;
        float targetTimer = 4f;

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;
            movementManager.RotateToTarget(combatController.myTarget.transform.position);
            yield return new WaitForEndOfFrame();
        }

        combatController.CheckActionHierarchy();
    }

    IEnumerator ForgeGiantChestLaser()
    {
        Debug.Log("CHEST LASER");
        anim.SetTrigger("ChestLaser");
        movementManager.StopMovement();
        float currentTimer = 0;
        float targetTimer = 7.833f;

        // Create the target Indicator
        TargetIndicatorController targetIndicatorController = Instantiate(targetDesignator).GetComponent<TargetIndicatorController>();
        targetIndicatorController.originAnchor = designatorOrigin;
        targetIndicatorController.targetAnchor = combatController.myTarget.transform;

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;
            movementManager.RotateToTarget(combatController.myTarget.transform.position);
            yield return new WaitForEndOfFrame();
        }

        combatController.CheckActionHierarchy();
    }
}
