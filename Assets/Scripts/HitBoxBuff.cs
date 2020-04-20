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

    public float aflameValue = 0;
    public float asleepValue = 0;
    public float stunValue = 0;
    public float curseValue = 0;
    public float bleedValue = 0;
    public float poisonValue = 0;
    public float corrosionValue = 0;
    public float frostbiteValue = 0;

    public void BuffSelf()
    {
        //Debug.Log("buffing self");
        if (hitSelf)
            transform.root.GetComponent<BuffsManager>().NewBuff(buff);
    }

    // USed to add afllictions to the caster of this spell.
    public void AfflictSelf()
    {
        if (hitSelf)
        {
            if (aflameValue > 0)
                transform.root.GetComponent<AfflictionManager>().AddAffliction(AfflictionManager.AfflictionTypes.Aflame, aflameValue);
            if (asleepValue > 0)
                transform.root.GetComponent<AfflictionManager>().AddAffliction(AfflictionManager.AfflictionTypes.Asleep, asleepValue);
            if (stunValue > 0)
                transform.root.GetComponent<AfflictionManager>().AddAffliction(AfflictionManager.AfflictionTypes.Stun, stunValue);
            if (curseValue > 0)
                transform.root.GetComponent<AfflictionManager>().AddAffliction(AfflictionManager.AfflictionTypes.Curse, curseValue);
            if (bleedValue > 0)
                transform.root.GetComponent<AfflictionManager>().AddAffliction(AfflictionManager.AfflictionTypes.Bleed, bleedValue);
            if (poisonValue > 0)
                transform.root.GetComponent<AfflictionManager>().AddAffliction(AfflictionManager.AfflictionTypes.Poison, poisonValue);
            if (frostbiteValue > 0)
                transform.root.GetComponent<AfflictionManager>().AddAffliction(AfflictionManager.AfflictionTypes.Frostbite, frostbiteValue);
            if (corrosionValue > 0)
                transform.root.GetComponent<AfflictionManager>().AddAffliction(AfflictionManager.AfflictionTypes.Corrosion, corrosionValue);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("col detected");
        if (other.CompareTag("Enemy") && hitEnemies)
        {
            if(applyBuff)
                other.GetComponent<BuffsManager>().NewBuff(buff);
            if (aflameValue > 0)
                other.GetComponent<AfflictionManager>().AddAffliction(AfflictionManager.AfflictionTypes.Aflame, aflameValue);
            if (asleepValue > 0)
                other.GetComponent<AfflictionManager>().AddAffliction(AfflictionManager.AfflictionTypes.Asleep, asleepValue);
            if (stunValue > 0)
                other.GetComponent<AfflictionManager>().AddAffliction(AfflictionManager.AfflictionTypes.Stun, stunValue);
            if (curseValue > 0)
                other.GetComponent<AfflictionManager>().AddAffliction(AfflictionManager.AfflictionTypes.Curse, curseValue);
            if (bleedValue > 0)
                other.GetComponent<AfflictionManager>().AddAffliction(AfflictionManager.AfflictionTypes.Bleed, bleedValue);
            if (poisonValue > 0)
                other.GetComponent<AfflictionManager>().AddAffliction(AfflictionManager.AfflictionTypes.Poison, poisonValue);
            if (frostbiteValue > 0)
                other.GetComponent<AfflictionManager>().AddAffliction(AfflictionManager.AfflictionTypes.Frostbite, frostbiteValue);
            if (corrosionValue > 0)
                transform.root.GetComponent<AfflictionManager>().AddAffliction(AfflictionManager.AfflictionTypes.Corrosion, corrosionValue);
            //Debug.Log("adding buff to enemy");
        }
        else if (other.CompareTag("Player") && hitPlayers)
        {
            if (applyBuff)
                other.GetComponent<BuffsManager>().NewBuff(buff);
            if (aflameValue > 0)
                other.GetComponent<AfflictionManager>().AddAffliction(AfflictionManager.AfflictionTypes.Aflame, aflameValue);
            if (asleepValue > 0)
                other.GetComponent<AfflictionManager>().AddAffliction(AfflictionManager.AfflictionTypes.Asleep, asleepValue);
            if (stunValue > 0)
                other.GetComponent<AfflictionManager>().AddAffliction(AfflictionManager.AfflictionTypes.Stun, stunValue);
            if (curseValue > 0)
                other.GetComponent<AfflictionManager>().AddAffliction(AfflictionManager.AfflictionTypes.Curse, curseValue);
            if (bleedValue > 0)
                other.GetComponent<AfflictionManager>().AddAffliction(AfflictionManager.AfflictionTypes.Bleed, bleedValue);
            if (poisonValue > 0)
                other.GetComponent<AfflictionManager>().AddAffliction(AfflictionManager.AfflictionTypes.Poison, poisonValue);
            if (frostbiteValue > 0)
                other.GetComponent<AfflictionManager>().AddAffliction(AfflictionManager.AfflictionTypes.Frostbite, frostbiteValue);
            if (corrosionValue > 0)
                transform.root.GetComponent<AfflictionManager>().AddAffliction(AfflictionManager.AfflictionTypes.Corrosion, corrosionValue);
            //Debug.Log("adding buff to player");
        }
    }
}

