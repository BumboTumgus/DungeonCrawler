using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviour
{
    public BuffsManager.BuffType myType;
    
    public List<ParticleSystem> effectParticleSystem = new List<ParticleSystem>();
    public PlayerStats connectedPlayer;
    public GameObject connectedIcon;
    public Color damageColor;
    
    public bool infiniteDuration = false;
    public float duration = 10f;
    public float currentTimer = 0f;
    public float DPS = 0f;

    public int vitSC = 0;     //SC stands for stat change.
    public int strSC = 0;
    public int dexSC = 0;
    public int spdSC = 0;
    public int intSC = 0;
    public int wisSC = 0;
    public int chaSC = 0;

    public float healthSC = 0;
    public float manaSC = 0;
    public float healthRegenSC = 0;
    public float manaRegenSC = 0;
    public float armorSC = 0;
    public float resistanceSC = 0;

    public float minDamageSC = 0;
    public float maxDamageSC = 0;
    public float atkSpdSC = 0;
    public float critChanceSC = 0;
    public float critDamageSC = 0;

    public float aflameResistSC = 0;
    public float asleepResistSC = 0;
    public float stunResistSC = 0;
    public float knockBackResistSC = 0;
    public float curseResistSC = 0;
    public float bleedResistSC = 0;
    public float poisonResistSC = 0;
    public float corrosionResistSC = 0;
    public float frostbiteResistSC = 0;

    public float targetDamageTickTimer = 0.5f;
    public float currentDamageTick = 0;

    private void Update()
    {
        // If this is not an infinite duration buff, count it down.
        if (!infiniteDuration)
        {
            currentTimer += Time.deltaTime;
            if (currentTimer > duration)
                EndBuff();
        }

        if(DPS != 0)
        {
            currentDamageTick += Time.deltaTime;
            if (currentDamageTick > targetDamageTickTimer)
            {
                currentDamageTick -= targetDamageTickTimer;
                connectedPlayer.TakeDamage(DPS * targetDamageTickTimer, false, damageColor);
            }
        }
    }

    // USed to add core stats to the player
    public void ChangeCoreStats(int vitGain, int strGain, int dexGain, int spdGain, int intGain, int wisGain, int chaGain)
    {
        connectedPlayer.Vit += vitGain;
        connectedPlayer.Str += strGain;
        connectedPlayer.Dex += dexGain;
        connectedPlayer.Spd += spdGain;
        connectedPlayer.Int += intGain;
        connectedPlayer.Wis += wisGain;
        connectedPlayer.Cha += chaGain;

        vitSC = vitGain;
        strSC = strGain;
        dexSC = dexGain;
        spdSC = spdGain;
        intSC = intGain;
        wisSC = wisGain;
        chaSC = chaGain;

        connectedPlayer.StatSetup(false, true);
    }

    // Used to change the characters health, manan, regens, armor, and mr
    public void ChangeDefensiveStats(float healthGain, float manaGain, float healthRegenGain, float manaRegenGain, float armorGain, float resistanceGain)
    {
        connectedPlayer.bonusHealth += healthGain;
        connectedPlayer.bonusMana += manaGain;
        connectedPlayer.bonusHealthRegen += healthRegenGain;
        connectedPlayer.bonusManaRegen += manaRegenGain;
        connectedPlayer.armor += armorGain;
        connectedPlayer.magicResist += resistanceGain;

        healthSC = healthGain;
        manaSC = manaGain;
        healthRegenSC = healthRegenGain;
        manaRegenSC = manaRegenGain;
        armorSC = armorGain;
        resistanceSC = resistanceGain;
        
        connectedPlayer.StatSetup(false, true);
    }


    // Used when the buff is over
    public void EndBuff()
    {
        foreach (ParticleSystem ps in effectParticleSystem)
            ps.Stop();

        if (myType == BuffsManager.BuffType.Stunned)
            connectedPlayer.GetComponent<PlayerController>().stunned = false;
        else if (myType == BuffsManager.BuffType.Asleep)
            connectedPlayer.GetComponent<PlayerController>().asleep = false;
        else if (myType == BuffsManager.BuffType.Bleeding)
            connectedPlayer.GetComponent<PlayerController>().bleeding = false;


        // If we changed the core stats, change them back.
        if (vitSC != 0 || strSC != 0 || dexSC != 0 || spdSC != 0 || intSC != 0 || wisSC != 0 || chaSC != 0)
            ChangeCoreStats(vitSC * -1, strSC * -1, dexSC * -1, spdSC * -1, intSC * -1, wisSC * -1, chaSC * -1);

        // If we changed the defensive stats, change them back.
        if (healthSC != 0 || manaSC != 0 || healthRegenSC != 0 || manaRegenSC != 0 || armorSC != 0 || resistanceSC != 0)
            ChangeDefensiveStats(healthSC * -1, manaSC * -1, healthRegenSC * -1, manaRegenSC * -1, armorSC * -1, resistanceSC * -1);

        // This contacts the buff manager, removi8ng us from their list of active buffs , deleting our icon, then killing this instance of the class. ALL fixes to stats should be done before this.
        connectedPlayer.GetComponent<BuffsManager>().RemoveBuff(this);
    }
}
