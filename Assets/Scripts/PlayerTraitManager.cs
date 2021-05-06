using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTraitManager : MonoBehaviour
{
    public class TraitSource
    {
        public ItemTrait.TraitType traitType;
        public int sourceCount = 0;
        public float traitValue = 0;
    }

    public List<TraitSource> OnKillEffects = new List<TraitSource>();
    public List<TraitSource> OnHitEffects = new List<TraitSource>();
    public List<TraitSource> OnStruckEffects = new List<TraitSource>();
    public List<TraitSource> IdleEffects = new List<TraitSource>();
    public List<TraitSource> OnAttackEffects = new List<TraitSource>();
    public List<TraitSource> OnStunEffects = new List<TraitSource>();
    public List<TraitSource> OnKnockbackEffects = new List<TraitSource>();

    // Adds an on kill effect to the list we parse through.
    public void AddOnKillEffect(ItemTrait.TraitType itemTraitType, float itemTraitValue)
    {
        bool traitAlreadyExists = false;

        foreach (TraitSource traitSource in OnKillEffects)
        {
            if(traitSource.traitType == itemTraitType)
            {
                traitSource.traitValue += itemTraitValue;
                traitSource.sourceCount++;
                traitAlreadyExists = true;
                break;
            }
        }

        if(!traitAlreadyExists)
        {
            TraitSource traitToAdd = new TraitSource();
            traitToAdd.traitValue = itemTraitValue;
            traitToAdd.sourceCount = 1;
            traitToAdd.traitType = itemTraitType;

            OnKillEffects.Add(traitToAdd);
        }
    }

    // Remove a kill effect from the list we parse through.
    public void RemoveOnKillEffect(ItemTrait.TraitType itemTraitType, float itemTraitValue)
    {
        TraitSource sourceToRemove = null;

        foreach (TraitSource traitSource in OnKillEffects)
        {
            if (traitSource.traitType == itemTraitType)
            {
                traitSource.traitValue -= itemTraitValue;
                traitSource.sourceCount--;
                if (traitSource.sourceCount == 0)
                    sourceToRemove = traitSource;
                break;
            }
        }

        if (sourceToRemove != null)
            OnKillEffects.Remove(sourceToRemove);
    }

    // Adds an on hit effect to the list we parse through.
    public void AddOnHitEffect(ItemTrait.TraitType itemTraitType, float itemTraitValue)
    {
        bool traitAlreadyExists = false;

        foreach (TraitSource traitSource in OnHitEffects)
        {
            if (traitSource.traitType == itemTraitType)
            {
                traitSource.traitValue += itemTraitValue;
                traitSource.sourceCount++;
                traitAlreadyExists = true;
                break;
            }
        }

        if (!traitAlreadyExists)
        {
            TraitSource traitToAdd = new TraitSource();
            traitToAdd.traitValue = itemTraitValue;
            traitToAdd.sourceCount = 1;
            traitToAdd.traitType = itemTraitType;

            OnHitEffects.Add(traitToAdd);
        }
    }

    // Remove an hit effect from the list we parse through.
    public void RemoveOnHitEffect(ItemTrait.TraitType itemTraitType, float itemTraitValue)
    {
        TraitSource sourceToRemove = null;

        foreach (TraitSource traitSource in OnHitEffects)
        {
            if (traitSource.traitType == itemTraitType)
            {
                traitSource.traitValue -= itemTraitValue;
                traitSource.sourceCount--;
                if (traitSource.sourceCount == 0)
                    sourceToRemove = traitSource;
                break;
            }
        }

        if (sourceToRemove != null)
            OnHitEffects.Remove(sourceToRemove);
    }

    // Adds an idle effect from the list we parse through
    public void AddIdleEffect(ItemTrait.TraitType itemTraitType, float itemTraitValue)
    {
        bool traitAlreadyExists = false;

        foreach (TraitSource traitSource in IdleEffects)
        {
            if (traitSource.traitType == itemTraitType)
            {
                traitSource.traitValue += itemTraitValue;
                traitSource.sourceCount++;
                traitAlreadyExists = true;
                break;
            }
        }

        if (!traitAlreadyExists)
        {
            TraitSource traitToAdd = new TraitSource();
            traitToAdd.traitValue = itemTraitValue;
            traitToAdd.sourceCount = 1;
            traitToAdd.traitType = itemTraitType;

            IdleEffects.Add(traitToAdd);
        }
    }

    // Remove an idle effect from the list we parse through.
    public void RemoveIdleEffect(ItemTrait.TraitType itemTraitType, float itemTraitValue)
    {
        TraitSource sourceToRemove = null;

        foreach (TraitSource traitSource in IdleEffects)
        {
            if (traitSource.traitType == itemTraitType)
            {
                traitSource.traitValue -= itemTraitValue;
                traitSource.sourceCount--;
                if (traitSource.sourceCount == 0)
                    sourceToRemove = traitSource;
                break;
            }
        }

        if (sourceToRemove != null)
            IdleEffects.Remove(sourceToRemove);
    }

    // Check to see if we have an idle effect and the value associated with it.
    public float CheckForIdleEffectValue(ItemTrait.TraitType itemTraitTypeToCheckFor)
    {
        float traitValue = 0;

        foreach (TraitSource traitSource in IdleEffects)
        {
            if (traitSource.traitType == itemTraitTypeToCheckFor)
            {
                traitValue = traitSource.traitValue;
                break;
            }
        }

        return traitValue;
    }

    // Check to see if we have an onhit effect and the value associated with it.
    public float CheckForOnHitValue(ItemTrait.TraitType itemTraitTypeToCheckFor)
    {
        float traitValue = 0;

        foreach (TraitSource traitSource in OnHitEffects)
        {
            if (traitSource.traitType == itemTraitTypeToCheckFor)
            {
                traitValue = traitSource.traitValue;
                break;
            }
        }

        return traitValue;
    }

    // Adds an on struck effect from the list we parse through
    public void AddOnStruckEffect(ItemTrait.TraitType itemTraitType, float itemTraitValue)
    {
        bool traitAlreadyExists = false;

        foreach (TraitSource traitSource in OnStruckEffects)
        {
            if (traitSource.traitType == itemTraitType)
            {
                traitSource.traitValue += itemTraitValue;
                traitSource.sourceCount++;
                traitAlreadyExists = true;
                break;
            }
        }

        if (!traitAlreadyExists)
        {
            TraitSource traitToAdd = new TraitSource();
            traitToAdd.traitValue = itemTraitValue;
            traitToAdd.sourceCount = 1;
            traitToAdd.traitType = itemTraitType;

            OnStruckEffects.Add(traitToAdd);
        }
    }

    // Remove an idle effect from the list we parse through.
    public void RemoveOnStruckEffect(ItemTrait.TraitType itemTraitType, float itemTraitValue)
    {
        TraitSource sourceToRemove = null;

        foreach (TraitSource traitSource in OnStruckEffects)
        {
            if (traitSource.traitType == itemTraitType)
            {
                traitSource.traitValue -= itemTraitValue;
                traitSource.sourceCount--;
                if (traitSource.sourceCount == 0)
                    sourceToRemove = traitSource;
                break;
            }
        }

        if (sourceToRemove != null)
            OnStruckEffects.Remove(sourceToRemove);
    }

    // Adds an on attack effect from the list we parse through
    public void AddOnAttackEffect(ItemTrait.TraitType itemTraitType, float itemTraitValue)
    {
        bool traitAlreadyExists = false;

        foreach (TraitSource traitSource in OnAttackEffects)
        {
            if (traitSource.traitType == itemTraitType)
            {
                traitSource.traitValue += itemTraitValue;
                traitSource.sourceCount++;
                traitAlreadyExists = true;
                break;
            }
        }

        if (!traitAlreadyExists)
        {
            TraitSource traitToAdd = new TraitSource();
            traitToAdd.traitValue = itemTraitValue;
            traitToAdd.sourceCount = 1;
            traitToAdd.traitType = itemTraitType;

            OnAttackEffects.Add(traitToAdd);
        }
    }

    // Remove an idle effect from the list we parse through.
    public void RemoveOnAttackEffect(ItemTrait.TraitType itemTraitType, float itemTraitValue)
    {
        TraitSource sourceToRemove = null;

        foreach (TraitSource traitSource in OnAttackEffects)
        {
            if (traitSource.traitType == itemTraitType)
            {
                traitSource.traitValue -= itemTraitValue;
                traitSource.sourceCount--;
                if (traitSource.sourceCount == 0)
                    sourceToRemove = traitSource;
                break;
            }
        }

        if (sourceToRemove != null)
            OnAttackEffects.Remove(sourceToRemove);
    }
}
