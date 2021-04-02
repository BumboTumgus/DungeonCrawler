using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxBuff : MonoBehaviour
{
    public BuffsManager.BuffType buff;
    public bool applyBuff = false;

    [SerializeField] private bool hitEnemies = false;
    [SerializeField] private bool hitPlayers = false;
    public bool hitSelf = false;

    public int aflameValue = 0;
    public int frostbiteValue = 0;
    public int overchargeValue = 0;
    public int overgrownValue = 0;
    public int sunderValue = 0;
    public int windshearValue = 0;
    public int bleedValue = 0;
    public int poisonValue = 0;

    public bool asleep = false;
    public bool stun = false;
    public bool freeze = false;
    public bool knockback = false;
    public float knockbackStrength = 0;
    public bool knockbackFromCenter = false;
    public bool knockForwardFromLocalTransform = false;
    public Vector3 knockbackDirection = Vector3.zero;

    public float baseDamage = 0;

    public void BuffSelf()
    {
        //Debug.Log("buffing self");
        if (hitSelf)
            transform.root.GetComponent<BuffsManager>().NewBuff(buff, baseDamage);
    }

    // USed to add afllictions to the caster of this spell.
    public void AfflictSelf()
    {
        if (hitSelf)
        {
            if (aflameValue > 0)
                transform.root.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Aflame, aflameValue, baseDamage);
            if (frostbiteValue > 0)
                transform.root.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Frostbite, frostbiteValue, baseDamage);
            if (overchargeValue > 0)
                transform.root.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Overcharge, overchargeValue, baseDamage);
            if (overgrownValue > 0)
                transform.root.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Overgrown, overgrownValue, baseDamage);
            if (sunderValue > 0)
                transform.root.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Sunder, sunderValue, baseDamage);
            if (windshearValue > 0)
                transform.root.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Windshear, windshearValue, baseDamage);
            if (knockback)
            {
                // Default case
                if (knockbackDirection == Vector3.zero)
                    knockbackDirection = Vector3.up + transform.forward * 10;

                if(knockbackFromCenter)
                {
                    knockbackDirection = (transform.root.position - transform.position).normalized;
                }

                if(knockForwardFromLocalTransform)
                    knockbackDirection = transform.forward;

                if (CompareTag("Player"))
                    transform.root.GetComponent<PlayerMovementController>().KnockbackLaunch(knockbackDirection * knockbackStrength);
                else
                    transform.root.GetComponent<EnemyCrowdControlManager>().KnockbackLaunch(knockbackDirection * knockbackStrength);
            }
            if (asleep)
                transform.root.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Asleep, 1, baseDamage);
            if (stun)
                transform.root.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Stunned, 1, baseDamage);
            if (freeze)
                transform.root.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Frozen, 1, baseDamage);
            if (bleedValue > 0)
                transform.root.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Bleeding, bleedValue, baseDamage);
            if (poisonValue > 0)
                transform.root.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Poisoned, poisonValue, baseDamage);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("col detected");
        if (other.CompareTag("Enemy") && hitEnemies)
        {
            if (applyBuff)
                other.GetComponent<BuffsManager>().NewBuff(buff, baseDamage);

            if (aflameValue > 0)
                other.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Aflame, aflameValue, baseDamage);
            if (frostbiteValue > 0)
                other.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Frostbite, frostbiteValue, baseDamage);
            if (overchargeValue > 0)
                other.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Overcharge, overchargeValue, baseDamage);
            if (overgrownValue > 0)
                other.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Overgrown, overgrownValue, baseDamage);
            if (sunderValue > 0)
                other.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Sunder, sunderValue, baseDamage);
            if (windshearValue > 0)
                other.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Windshear, windshearValue, baseDamage);
            if (knockback)
            {
                // Default case
                if (knockbackDirection == Vector3.zero)
                    knockbackDirection = Vector3.up + transform.forward;

                if (knockbackFromCenter)
                {
                    knockbackDirection = (other.transform.position - transform.position).normalized;
                }


                if (knockForwardFromLocalTransform)
                    knockbackDirection = transform.forward;

                //Debug.Log("The knckback direction is: " + knockbackDirection);
                other.GetComponent<EnemyCrowdControlManager>().KnockbackLaunch(knockbackDirection * knockbackStrength);
            }
            if (asleep)
                other.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Asleep, 1, baseDamage);
            if (stun)
                other.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Stunned, 1, baseDamage);
            if (freeze)
                other.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Frozen, 1, baseDamage);
            if (bleedValue > 0)
                other.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Bleeding, bleedValue, baseDamage);
            if (poisonValue > 0)
                other.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Poisoned, poisonValue, baseDamage);
        }
        else if (other.CompareTag("Player") && hitPlayers)
        {
            if (applyBuff)
                other.GetComponent<BuffsManager>().NewBuff(buff, baseDamage);

            if (aflameValue > 0)
                other.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Aflame, aflameValue, baseDamage);
            if (frostbiteValue > 0)
                other.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Frostbite, frostbiteValue, baseDamage);
            if (overchargeValue > 0)
                other.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Overcharge, overchargeValue, baseDamage);
            if (overgrownValue > 0)
                other.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Overgrown, overgrownValue, baseDamage);
            if (sunderValue > 0)
                other.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Sunder, sunderValue, baseDamage);
            if (windshearValue > 0)
                other.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Windshear, windshearValue, baseDamage);
            if (knockback)
            {
                // Default case
                if (knockbackDirection == Vector3.zero)
                    knockbackDirection = Vector3.up + transform.forward * 10;

                if (knockbackFromCenter)
                {
                    knockbackDirection = (other.transform.position - transform.position).normalized;
                    Debug.Log(knockbackDirection);
                }


                if (knockForwardFromLocalTransform)
                    knockbackDirection = transform.forward;

                other.GetComponent<PlayerMovementController>().KnockbackLaunch(knockbackDirection * knockbackStrength);
            }
            if (asleep)
                other.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Asleep, 1, baseDamage);
            if (stun)
                other.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Stunned, 1, baseDamage);
            if (freeze)
                other.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Frozen, 1, baseDamage);
            if (bleedValue > 0)
                other.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Bleeding, bleedValue, baseDamage);
            if (poisonValue > 0)
                other.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Poisoned, poisonValue, baseDamage);
        }
    }
}

