using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAbilityBank : MonoBehaviour
{
    public enum EnemyAbility { ThrowingAxe, None};

    public GameObject[] spellProjectiles;

    private PlayerStats myStats;
    private EnemyCombatController combatController;
    private EnemyMovementManager movementManager;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
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
}
