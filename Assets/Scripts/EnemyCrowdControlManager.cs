using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCrowdControlManager : MonoBehaviour
{
    [SerializeField] private float knockbackGetUpTimer = 0.5f;

    private PlayerStats myStats;
    private EnemyCombatController combatController;
    private Animator anim;
    private BuffsManager buffsManager;
    private RagdollManager ragdollManager;
    private EnemyMovementManager movementManager;

    private IEnumerator knockbackCoroutine;

    private void Start()
    {
        myStats = GetComponent<PlayerStats>();
        combatController = GetComponent<EnemyCombatController>();
        anim = GetComponent<Animator>();
        buffsManager = GetComponent<BuffsManager>();
        ragdollManager = GetComponent<RagdollManager>();
        movementManager = GetComponent<EnemyMovementManager>();
    }

    // Used when the player gets stunned
    public void StunLaunch()
    {
        StartCoroutine(Stunned());
    }

    // The stunned coroutine. Makes the player unable to take action.
    IEnumerator Stunned()
    {
        movementManager.StopMovement();
        movementManager.enableMovement = false;

        if (!myStats.frozen)
            anim.SetFloat("FrozenMultiplier", 1f);
        myStats.stunned = true;
        combatController.SwitchAction(EnemyCombatController.ActionType.LossOfControl);
        anim.SetBool("Stunned", true);

        while (myStats.stunned)
            yield return null;

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
        movementManager.StopMovement();
        movementManager.enableMovement = false;

        myStats.asleep = true;
        combatController.SwitchAction(EnemyCombatController.ActionType.LossOfControl);
        anim.SetBool("Sleeping", true);

        while (myStats.asleep)
            yield return null;

        myStats.asleep = false;
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
        movementManager.StopMovement();
        movementManager.enableMovement = false;

        myStats.frozen = true;
        combatController.SwitchAction(EnemyCombatController.ActionType.LossOfControl);
        anim.SetBool("Stunned", true);
        anim.SetFloat("FrozenMultiplier", 0f);

        while (myStats.frozen)
            yield return null;

        myStats.frozen = false;

        anim.SetFloat("FrozenMultiplier", 1f);

        CheckForOtherLoseOfControlEffects();
    }

    // Used when the player gets frozen
    public void KnockbackLaunch(Vector3 directionOfKnockback, PlayerStats buffApplier)
    {
        // Check to see if the knockback works and goes through.
        if (Random.Range(0, 100) > myStats.knockbackResistance * 100)
        {
            movementManager.StopMovement();
            movementManager.enableMovement = false;

            ragdollManager.StopAllCoroutines();
            anim.ResetTrigger("GettingUpFacingDown");
            anim.ResetTrigger("GettingUpFacingUp");

            if (knockbackCoroutine != null)
                StopCoroutine(knockbackCoroutine);
            knockbackCoroutine = Knockback(directionOfKnockback);
            StartCoroutine(knockbackCoroutine);

            GetComponent<BuffsManager>().NewBuff(BuffsManager.BuffType.Knockback, 0, buffApplier);
        }
    }

    // The asleep coroutine. Makes the player unable to take action.
    IEnumerator Knockback(Vector3 directionalKnockback)
    {
        myStats.knockedBack = true;
        combatController.SwitchAction(EnemyCombatController.ActionType.LossOfControl);
        ragdollManager.EnableRagDollState(directionalKnockback);

        float currentTimer = 0;
        float targetTimer = 0.2f;

        while (myStats.knockedBack)
        {
            currentTimer += Time.deltaTime;
            if (currentTimer >= targetTimer)
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

        ragdollManager.LerpBonesToGetUpAnim();

        yield return new WaitForSeconds(ragdollManager.lerpTime);

        currentTimer = 0f;
        targetTimer = knockbackGetUpTimer;

        anim.enabled = true;
        /*
        for (int index = 0; index < anim.layerCount; index++)
        {
            Debug.Log(anim.GetLayerName(index));
            if(anim.GetLayerName(index) != "GettingUp")
                anim.SetLayerWeight(index, 0);
        }
        */

        if (ragdollManager.getUpFaceUp)
            anim.SetTrigger("GettingUpFacingDown");
        else
            anim.SetTrigger("GettingUpFacingUp");

        // player is getting up.
        while (myStats.knockedBack)
        {
            currentTimer += Time.deltaTime;
            if (currentTimer >= targetTimer)
                buffsManager.AttemptRemovalOfBuff(BuffsManager.BuffType.Knockback, false);

            yield return null;
        }

        /*
        for (int index = 0; index < anim.layerCount; index++)
        {
            //Debug.Log(anim.GetLayerName(index));
            anim.SetLayerWeight(index, 1);
        }
        */

        CheckForOtherLoseOfControlEffects();
    }

    private void CheckForOtherLoseOfControlEffects()
    {
        if (myStats.stunned || myStats.frozen || myStats.asleep || myStats.knockedBack)
        {

        }
        else
        {
            anim.SetBool("Stunned", false);
            movementManager.enableMovement = true;
            combatController.SwitchAction(EnemyCombatController.ActionType.ChaseTarget);
        }

    }

}
