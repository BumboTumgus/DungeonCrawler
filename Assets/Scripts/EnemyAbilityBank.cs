using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAbilityBank : MonoBehaviour
{
    public enum EnemyAbility { None, ThrowingAxe, GroundSlam, SummonGoblins, GoblinSeeker, AcidShot, SnakeCurseShot, Ephemeral, RockGolemSlamAOE, RockGolemSlamShockwave};

    public GameObject[] spellProjectiles;
    public GameObject[] spellSummons;
    public ParticleSystem[] spellParticles;
    public GameObject[] ephemeralObjectsToHide;

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
        Debug.Log("we are casting a spell");
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
        anim.SetTrigger("ThrowingAxe");
        movementManager.StopMovement();
        float currentTimer = 0;
        float targetAxeTimer = 0.6f;
        float targetTimer = 2;
        bool projectileThrown = false;

        while(currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;
            if(!projectileThrown && currentTimer > targetAxeTimer)
            {
                projectileThrown = true;
                // Instantiate the obhect, set it's damage and aim it at the player.
                Vector3 forward = combatController.myTarget.transform.position - transform.position;
                GameObject axe = Instantiate(spellProjectiles[0], transform.position + Vector3.up, Quaternion.LookRotation(forward, Vector3.up));
                axe.GetComponent<HitBox>().damage = myStats.baseDamage * 2;
                axe.GetComponent<HitBox>().myStats = myStats;
            }
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
        float targetGroundSlamTimer = 1.22f;
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
                groundSlam.GetComponent<HitBox>().damage = myStats.baseDamage * 3;
                groundSlam.GetComponent<HitBox>().myStats = myStats;
                spellParticles[0].Stop();
                spellParticles[1].Stop();
            }
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
        float targetSummonGoblinsTimer = 0.8f;
        float targetTimer = 2;
        bool goblinsSpawned = false;

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;
            if (!goblinsSpawned && currentTimer > targetSummonGoblinsTimer)
            {
                spellParticles[2].Stop();
                spellParticles[3].Play();
                goblinsSpawned = true;
                Instantiate(spellSummons[0], transform.position + transform.forward * 1.5f + transform.right * 1.5f, transform.rotation);
                Instantiate(spellSummons[0], transform.position + transform.forward * -1.5f + transform.right * 1.5f, transform.rotation);
                Instantiate(spellSummons[0], transform.position + transform.forward * 1.5f + transform.right * -1.5f, transform.rotation);
                Instantiate(spellSummons[0], transform.position + transform.forward * -1.5f + transform.right * -1.5f, transform.rotation);
                // Instantiate the goblins.
            }
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
        float targetAcidLaunchTimer = 0.8f;
        float targetTimer = 2;
        bool acidLaunch = false;

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;
            if (!acidLaunch && currentTimer > targetAcidLaunchTimer)
            {
                acidLaunch = true;
                
                // Instantiate the obhect, set it's damage and aim it at the player.
                Vector3 forward = combatController.myTarget.transform.position - transform.position;
                GameObject acidShot = Instantiate(spellProjectiles[0], transform.position + Vector3.up, Quaternion.LookRotation(forward, Vector3.up));
                acidShot.GetComponent<HitBox>().damage = myStats.baseDamage * 1;
                acidShot.GetComponent<HitBox>().myStats = myStats;
            }
            movementManager.RotateToTarget(combatController.myTarget.transform.position);
            yield return new WaitForEndOfFrame();
        }

        combatController.CheckActionHierarchy();

    }

    IEnumerator SnakeCurseShot()
    {
        anim.SetTrigger("AcidShot");
        movementManager.StopMovement();
        float currentTimer = 0;
        float targetAcidLaunchTimer = 0.5f;
        float targetTimer = 2;
        bool snakeShotLaunched = false;

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;
            if (!snakeShotLaunched && currentTimer > targetAcidLaunchTimer)
            {
                snakeShotLaunched = true;

                // Instantiate the obhect, set it's damage and aim it at the player.
                for (int i = 0; i < 5; i++)
                {
                    Vector3 forward = (combatController.myTarget.transform.position + combatController.myTarget.transform.right * (-2 + i)) - transform.position;
                    GameObject snakeShot = Instantiate(spellProjectiles[0], transform.position + Vector3.up, Quaternion.LookRotation(forward, Vector3.up));
                    snakeShot.GetComponent<HitBox>().damage = myStats.baseDamage * 1;
                    snakeShot.GetComponent<HitBox>().myStats = myStats;
                }
            }
            movementManager.RotateToTarget(combatController.myTarget.transform.position);
            yield return new WaitForEndOfFrame();
        }

        combatController.CheckActionHierarchy();
    }
}
