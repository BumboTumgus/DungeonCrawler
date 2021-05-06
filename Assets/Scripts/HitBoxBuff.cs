using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxBuff : MonoBehaviour
{
    public BuffsManager.BuffType buff;
    public bool applyBuff = false;
    public bool disjointedHitbox = false;
    public PlayerStats buffOrigin;

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
    private float knockbackMultiplier = 1;
    public bool knockbackFromCenter = false;
    public bool knockForwardFromLocalTransform = false;
    public Vector3 knockbackDirection = Vector3.zero;

    private void Start()
    {
        if (!disjointedHitbox)
            buffOrigin = transform.root.GetComponent<PlayerStats>();
    }

    public void BuffSelf()
    {
        //Debug.Log("buffing self");
        if (hitSelf)
            transform.root.GetComponent<BuffsManager>().NewBuff(buff, buffOrigin.baseDamage, GetComponent<PlayerStats>());
    }

    // USed to add afllictions to the caster of this spell.
    public void AfflictSelf()
    {
        if (hitSelf)
        {
            if (aflameValue > 0)
                transform.root.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Aflame, aflameValue, buffOrigin.baseDamage, buffOrigin);
            if (frostbiteValue > 0)
                transform.root.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Frostbite, frostbiteValue, buffOrigin.baseDamage, buffOrigin);
            if (overchargeValue > 0)
                transform.root.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Overcharge, overchargeValue, buffOrigin.baseDamage, buffOrigin);
            if (overgrownValue > 0)
                transform.root.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Overgrown, overgrownValue, buffOrigin.baseDamage, buffOrigin);
            if (sunderValue > 0)
                transform.root.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Sunder, sunderValue, buffOrigin.baseDamage, buffOrigin);
            if (windshearValue > 0)
                transform.root.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Windshear, windshearValue, buffOrigin.baseDamage, buffOrigin);
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
                    transform.root.GetComponent<PlayerMovementController>().KnockbackLaunch(knockbackDirection * knockbackStrength, buffOrigin);
                else
                    transform.root.GetComponent<EnemyCrowdControlManager>().KnockbackLaunch(knockbackDirection * knockbackStrength, buffOrigin);
            }
            if (asleep)
                transform.root.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Asleep, 1, buffOrigin.baseDamage, buffOrigin);
            if (stun)
                transform.root.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Stunned, 1, buffOrigin.baseDamage, buffOrigin);
            if (freeze)
                transform.root.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Frozen, 1, buffOrigin.baseDamage, buffOrigin);
            if (bleedValue > 0)
                transform.root.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Bleeding, bleedValue, buffOrigin.baseDamage, buffOrigin);
            if (poisonValue > 0)
                transform.root.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Poisoned, poisonValue, buffOrigin.baseDamage, buffOrigin);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        knockbackMultiplier = 1;
        //Debug.Log("col detected");
        if (other.CompareTag("Enemy") && hitEnemies)
        {
            if (applyBuff)
                other.GetComponent<BuffsManager>().NewBuff(buff, buffOrigin.baseDamage, buffOrigin);

            if (aflameValue > 0)
                other.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Aflame, aflameValue, buffOrigin.baseDamage, buffOrigin);
            if (frostbiteValue > 0)
                other.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Frostbite, frostbiteValue, buffOrigin.baseDamage, buffOrigin);
            if (overchargeValue > 0)
                other.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Overcharge, overchargeValue, buffOrigin.baseDamage, buffOrigin);
            if (overgrownValue > 0)
                other.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Overgrown, overgrownValue, buffOrigin.baseDamage, buffOrigin);
            if (sunderValue > 0)
                other.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Sunder, sunderValue, buffOrigin.baseDamage, buffOrigin);
            if (windshearValue > 0)
                other.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Windshear, windshearValue, buffOrigin.baseDamage, buffOrigin);
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
                if (buffOrigin.CompareTag("Player") && buffOrigin.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.AflameKnockbackAflameSpellsOnKnockbackedTargetExplode) > 0 && other.GetComponent<BuffsManager>().PollForBuffStacks(BuffsManager.BuffType.Aflame) >= 20)
                {
                    GameObject knockbackFireWave = Instantiate(buffOrigin.GetComponent<SkillsManager>().skillProjectiles[57], other.transform.position + Vector3.up, Quaternion.identity);
                    knockbackFireWave.GetComponent<HitBox>().myStats = buffOrigin;
                    knockbackFireWave.GetComponent<HitBox>().damage = (1 + buffOrigin.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.AflameKnockbackAflameSpellsOnKnockbackedTargetExplode)) * buffOrigin.baseDamage;
                }
                if (buffOrigin.CompareTag("Player") && buffOrigin.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.IceKnockbackFrostbiteIncreasesKnockbackForce) > 0 && other.GetComponent<BuffsManager>().PollForBuffStacks(BuffsManager.BuffType.Frostbite) >= 1)
                {
                    knockbackMultiplier += buffOrigin.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.IceKnockbackFrostbiteIncreasesKnockbackForce) * other.GetComponent<BuffsManager>().PollForBuffStacks(BuffsManager.BuffType.Frostbite);
                }
                if (buffOrigin.CompareTag("Player") && buffOrigin.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.IceKnockbackSnowEruptionOnKnockback) > 0 && other.GetComponent<BuffsManager>().PollForBuffStacks(BuffsManager.BuffType.Frostbite) >= 25)
                {
                    GameObject knockbackSnowPulse = Instantiate(buffOrigin.GetComponent<SkillsManager>().skillProjectiles[62], other.transform.position + Vector3.up, Quaternion.identity);
                    knockbackSnowPulse.GetComponent<HitBox>().myStats = buffOrigin;
                    knockbackSnowPulse.GetComponent<HitBox>().damage = (1 + buffOrigin.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.IceKnockbackSnowEruptionOnKnockback)) * buffOrigin.baseDamage;
                }
                if (buffOrigin.CompareTag("Player") && buffOrigin.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.EarthKnockbackTremorsOnKnockback) > 0 && other.GetComponent<BuffsManager>().PollForBuffStacks(BuffsManager.BuffType.Sunder) >= 10)
                {
                    GameObject knockbackEarthReverb = Instantiate(buffOrigin.GetComponent<SkillsManager>().skillProjectiles[67], other.transform.position + Vector3.up * 0.1f, Quaternion.identity);
                    knockbackEarthReverb.GetComponent<HitBox>().myStats = buffOrigin;
                    knockbackEarthReverb.GetComponent<HitBox>().damage = (0.5f + buffOrigin.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.EarthKnockbackTremorsOnKnockback)) * buffOrigin.baseDamage;
                }

                //Debug.Log("knockback multiplier is: " + knockbackMultiplier);
                other.GetComponent<EnemyCrowdControlManager>().KnockbackLaunch(knockbackDirection * knockbackStrength * knockbackMultiplier, buffOrigin);
            }
            if (asleep)
                other.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Asleep, 1, buffOrigin.baseDamage, buffOrigin);
            if (stun)
                other.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Stunned, 1, buffOrigin.baseDamage, buffOrigin);
            if (freeze)
                other.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Frozen, 1, buffOrigin.baseDamage, buffOrigin);
            if (bleedValue > 0)
                other.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Bleeding, bleedValue, buffOrigin.baseDamage, buffOrigin);
            if (poisonValue > 0)
                other.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Poisoned, poisonValue, buffOrigin.baseDamage, buffOrigin);
        }
        else if (other.CompareTag("Player") && hitPlayers)
        {
            if (applyBuff)
                other.GetComponent<BuffsManager>().NewBuff(buff, buffOrigin.baseDamage, buffOrigin);

            if (aflameValue > 0)
                other.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Aflame, aflameValue, buffOrigin.baseDamage, buffOrigin);
            if (frostbiteValue > 0)
                other.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Frostbite, frostbiteValue, buffOrigin.baseDamage, buffOrigin);
            if (overchargeValue > 0)
                other.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Overcharge, overchargeValue, buffOrigin.baseDamage, buffOrigin);
            if (overgrownValue > 0)
                other.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Overgrown, overgrownValue, buffOrigin.baseDamage, buffOrigin);
            if (sunderValue > 0)
                other.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Sunder, sunderValue, buffOrigin.baseDamage, buffOrigin);
            if (windshearValue > 0)
                other.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Windshear, windshearValue, buffOrigin.baseDamage, buffOrigin);
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

                other.GetComponent<PlayerMovementController>().KnockbackLaunch(knockbackDirection * knockbackStrength, buffOrigin);
            }
            if (asleep)
                other.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Asleep, 1, buffOrigin.baseDamage, buffOrigin);
            if (stun)
                other.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Stunned, 1, buffOrigin.baseDamage, buffOrigin);
            if (freeze)
                other.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Frozen, 1, buffOrigin.baseDamage, buffOrigin);
            if (bleedValue > 0)
                other.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Bleeding, bleedValue, buffOrigin.baseDamage, buffOrigin);
            if (poisonValue > 0)
                other.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Poisoned, poisonValue, buffOrigin.baseDamage, buffOrigin);
        }
    }
}

