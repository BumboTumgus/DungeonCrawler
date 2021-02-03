using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxBuff : MonoBehaviour
{
    public BuffsManager.BuffType buff;
    public bool applyBuff = false;

    [SerializeField] private bool hitEnemies = false;
    [SerializeField] private bool hitPlayers = false;
    [SerializeField] private bool hitSelf = false;

    public int aflameValue = 0;
    public int frostbiteValue = 0;
    public int overchargeValue = 0;
    public int overgrownValue = 0;
    public int sunderValue = 0;
    public int windshearValue = 0;
    public int knockbackValue = 0;
    public int asleepValue = 0;
    public int stunValue = 0;
    public int bleedValue = 0;
    public int poisonValue = 0;

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
            if (knockbackValue > 0)
                transform.root.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Knockback, knockbackValue, baseDamage);
            if (asleepValue > 0)
                transform.root.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Asleep, asleepValue, baseDamage);
            if (stunValue > 0)
                transform.root.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Stunned, stunValue, baseDamage);
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
            if(applyBuff)
                other.GetComponent<BuffsManager>().NewBuff(buff, baseDamage);
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
            if (knockbackValue > 0)
                transform.root.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Knockback, knockbackValue, baseDamage);
            if (asleepValue > 0)
                transform.root.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Asleep, asleepValue, baseDamage);
            if (stunValue > 0)
                transform.root.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Stunned, stunValue, baseDamage);
            if (bleedValue > 0)
                transform.root.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Bleeding, bleedValue, baseDamage);
            if (poisonValue > 0)
                transform.root.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Poisoned, poisonValue, baseDamage);
            //Debug.Log("adding buff to enemy");
        }
        else if (other.CompareTag("Player") && hitPlayers)
        {
            if (applyBuff)
                other.GetComponent<BuffsManager>().NewBuff(buff, baseDamage);
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
            if (knockbackValue > 0)
                transform.root.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Knockback, knockbackValue, baseDamage);
            if (asleepValue > 0)
                transform.root.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Asleep, asleepValue, baseDamage);
            if (stunValue > 0)
                transform.root.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Stunned, stunValue, baseDamage);
            if (bleedValue > 0)
                transform.root.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Bleeding, bleedValue, baseDamage);
            if (poisonValue > 0)
                transform.root.GetComponent<BuffsManager>().CheckResistanceToBuff(BuffsManager.BuffType.Poisoned, poisonValue, baseDamage);
            //Debug.Log("adding buff to player");
        }
    }
}

