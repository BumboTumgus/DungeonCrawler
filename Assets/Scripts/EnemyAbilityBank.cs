using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAbilityBank : MonoBehaviour
{
    public enum EnemyAbility { None, ThrowingAxe, GroundSlam, SummonGoblins};

    public GameObject[] spellProjectiles;
    public GameObject[] spellSummons;
    public ParticleSystem[] spellParticles;

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
            default:
                break;
        }
    }

    IEnumerator ThrowingAxe()
    {
        Debug.Log(" i am performing the axe throw");
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
                axe.GetComponent<HitBox>().damage = myStats.weaponHitbase + myStats.weaponHitMax * 3;
                axe.GetComponent<HitBox>().myStats = myStats;
            }
            movementManager.RotateToTarget(combatController.myTarget.transform.position);
            yield return new WaitForEndOfFrame();
        }

        combatController.SwitchAction(EnemyCombatController.ActionType.Attack);
    }

    IEnumerator GroundSlam()
    {
        Debug.Log(" i am performing the ground slam");
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
                groundSlam.GetComponent<HitBox>().damage = myStats.weaponHitbase + myStats.weaponHitMax * 3;
                groundSlam.GetComponent<HitBox>().myStats = myStats;
                spellParticles[0].Stop();
                spellParticles[1].Stop();
            }
            movementManager.RotateToTarget(combatController.myTarget.transform.position);
            yield return new WaitForEndOfFrame();
        }

        combatController.SwitchAction(EnemyCombatController.ActionType.Attack);
    }

    IEnumerator SummonGoblins()
    {
        Debug.Log("I am summon the globins");
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

        combatController.SwitchAction(EnemyCombatController.ActionType.Attack);
    }
}
