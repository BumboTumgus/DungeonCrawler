using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public string playerName = "Jose";
    public string playerTitle = "Mighty";

    public float weaponBaseAttackDelay = 1;
    public int weaponHitMin = 1;
    public int weaponHitMax = 4;
    public int weaponHitbase = 4;
    public float weaponStaggerBase = 5;
    public float weaponCritChance = 5;
    public float weaponCritMod = 2;
    public float weaponVitScaling = 0;
    public float weaponStrScaling = 1;
    public float weaponDexScaling = 0;
    public float weaponSpdScaling = 0;
    public float weaponIntScaling = 0;
    public float weaponWisScaling = 0;
    public float weaponChaScaling = 0;
    public List<float> weaponHitspeeds = new List<float>();

    public float currentAttackDelay = 13; // 10 is fast, 100 is really slow.

    public float attackDelay = 1;
    public float attackDamage = 5;
    public float attackRange = 2;
    public float attackSpeed = 1;
    public float health = 100;
    public float healthMax = 100;
    public float mana = 100;
    public float manaMax = 100;
    public float armor = 5;
    public float magicResist = 5;
    public float speed = 4;
    public float strafeSpeed = 2;
    public float acceleration = 2;
    public float poise = 5;
    public float poiseMax = 5;
    public float poiseLoseMultiplier = 1;
    public float healthRegen = 1;
    public float manaRegen = 1;
    public float bonusHealth = 0;
    public float bonusMana = 0;
    public float bonusHealthRegen = 0;
    public float bonusManaRegen = 0;

    public int level = 1;
    public float exp = 0;
    public float expTarget = 100;
    public float expMultiplier;

    public int Str = 5;
    public int Vit = 5;
    public int Dex = 5;
    public int Spd = 5;
    public int Int = 5;
    public int Wis = 5;
    public int Cha = 5;

    public int StrLvl = 1;
    public int VitLvl = 1;
    public int DexLvl = 1;
    public int SpdLvl = 1;
    public int IntLvl = 1;
    public int WisLvl = 1;
    public int ChaLvl = 1;

    public BarManager healthBar;
    public BarManager healthBarSecondary;
    public BarManager manaBar;
    public GameObject actionBarParent;
    public StatUpdater myStats;

    public bool dead = false;
 
    public float agroRange = 3;
    public int enemyCost = 1;

    private DamageNumberManager damageNumberManager;
    private bool recentlyDamaged = false;
    private float recentlyDamagedTimer = 0;

    private const float RECENTLY_DAMAGED_TIMER_START = 1f;

    [SerializeField] private GameObject enemyHealthBar;

    private void Start()
    {
        // If we do not have a heathbar, set it up now.
        HealthBarSetup();

        StatSetup(true, true);
        damageNumberManager = GetComponent<DamageNumberManager>();
    }

    private void Update()
    {
        //USed for debugging to add exp.
        if (Input.GetKeyDown(KeyCode.L) && CompareTag("Player"))
            AddExp(100);
        // HEalth and mana regen logic.
        if (!dead)
        {
            health += healthRegen * Time.deltaTime;
            mana += manaRegen * Time.deltaTime;
        }
        if (health > healthMax)
            health = healthMax;
        else if(myStats != null)
            myStats.SetHealthManaValues(this);
        if (mana > manaMax)
            mana = manaMax;
        else if (myStats != null)
            myStats.SetHealthManaValues(this);
        // Update the health bar.
        healthBar.targetValue = health;
        if (healthBarSecondary != null)
            healthBarSecondary.targetValue = health;
        if (manaBar != null)
            manaBar.targetValue = mana;

        // Regen poise if we havent been hit in a while, if not increment the timer.
        if (!recentlyDamaged)
        {
            if (poise < poiseMax)
                poise += poiseMax / 3 * Time.deltaTime;
        }
        else
        {
            recentlyDamagedTimer -= Time.deltaTime;
            if (recentlyDamagedTimer <= 0)
                recentlyDamaged = false;
        }
    }

    // Used to set up the stats at the start of the game and every time we level.
    private void StatSetup(bool LeveledUp, bool changeHealthBars)
    {
        healthMax = 20 + 3 * level + 5 * Vit + 2 * Str + Dex + Spd + Int + Wis + Cha + bonusHealth;
        if (health > healthMax)
            health = healthMax;
        // If we level up set the health to the max.
        if (LeveledUp)
            health = healthMax;

        manaMax = 20 + 3 * level + 5 * Wis + Int + bonusMana;
        if (mana > manaMax)
            mana = manaMax;
        // If we level up set the mana to the max.
        if (LeveledUp)
            mana = manaMax;

        // Ste up our health and manaRegen;
        healthRegen = Vit * 0.2f + bonusHealthRegen;
        manaRegen = Wis * 0.4f + Int * 0.1f + bonusManaRegen;

        // Set up the characters speed. enemies are half as fast as normal.
        speed = 2.5f + (float) Spd / 10;
        if (gameObject.tag == "Enemy")
            speed *= 1.3f;
        attackDelay = weaponBaseAttackDelay / (1 + 2 * Spd + Dex);
        attackSpeed = weaponBaseAttackDelay * (1 + 0.025f * Spd + 0.0125f * Dex);
        strafeSpeed = speed / 2;
        acceleration = speed;
        if (transform.CompareTag("Enemy"))
            GetComponent<UnityEngine.AI.NavMeshAgent>().speed = speed;

        // Sets up the characters poise, which is their resistance to being staggered.
        poiseMax = Str + Vit;
        if (gameObject.CompareTag("Player"))
            poiseMax += 20;

        if (changeHealthBars)
        {
            // Sets up the health and mana Bars.
            healthBar.Initialize(healthMax, true);
            if (healthBarSecondary != null)
                healthBarSecondary.Initialize(healthMax, true);
            if (manaBar != null)
                manaBar.Initialize(manaMax, true);
        }

        if(myStats != null)
            myStats.SetStatValues(this);
    }

    // Used when the player gains exp.
    public void AddExp(float value)
    {
        exp += value;
        if (myStats != null)
            myStats.SetStatValues(this);
        if (exp >= expTarget)
        {
            exp -= expTarget;
            level++;
            LevelUp();
            expTarget = level * 100;
            StatSetup(true, true);
            Debug.Log("Level Up");
        }
    }

    // USed to add base stats to the player when they gain a level
    public void LevelUp()
    {
        if (level % StrLvl == 0)
            Str++;
        if (level % VitLvl == 0)
            Vit++;
        if (level % DexLvl == 0)
            Dex++;
        if (level % SpdLvl == 0)
            Spd++;
        if (level % IntLvl == 0)
            Int++;
        if (level % WisLvl == 0)
            Wis++;
        if (level % ChaLvl == 0)
            Cha++;
        StatSetup(true, true);
    }

    // Used to calculate how much exp this is worth
    public float ExpWorth(float killerLevel)
    {
        float expValue = Vit + Str + Cha + Spd + Dex + Int + Wis;
        float expValueDifficultyMod = (killerLevel / level) * (killerLevel / level);
        expValue *= expValueDifficultyMod;
        Debug.Log("Checking Exp Worth");
        
        return expValue;
    }

    // Used to take damage
    public void TakeDamage(float amount, bool crit, float staggerAmount)
    {
        if(amount > 0)
            health -= amount;
        if (health < 0)
            health = 0;
        recentlyDamaged = true;
        recentlyDamagedTimer = RECENTLY_DAMAGED_TIMER_START;

        // Update the health bar.
        healthBar.targetValue = health;
        if (healthBarSecondary != null)
            healthBarSecondary.targetValue = health;

        // Spawn the damage number.
        SpawnFlavorText(amount, crit);

        // If we are dead, call the death logic method.
        if (health <= 0 && !dead)
            EntityDeath();

        // If we are not dead, check to see if we are staggered from this hit.
        poise -= staggerAmount * poiseLoseMultiplier;
        if(poise <= 0)
        {
            // REset our poise then stagegr us.
            poise = poiseMax;
            if (!dead && gameObject.CompareTag("Player"))
                GetComponent<PlayerController>().StaggerLaunch();
            else if (!dead && gameObject.CompareTag("Enemy"))
                GetComponent<EnemyCombatManager>().StaggerLaunch();
        }

    }

    // Used when this object dies. What will happen afterwards?
    public void EntityDeath()
    {
        dead = true;
        // Three cases, player death, player summon death, or an enemy death.
        if(gameObject.CompareTag("Enemy"))
        {
            // Debug.Log("EnemyDeath");

            // Find the player, and give them exp. If they were in combat with us, end the combat. Start the death coroutine (for a death animation).
            // Create an array of all players.
            GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
            GameObject[] players = new GameObject[gm.currentPlayers.Length];
            for(int index = 0; index < players.Length; index++)
            {
                players[index] = gm.currentPlayers[index];
            }

            // If any player was agrod onto us, end their combat. and add exp to all players.
            foreach(GameObject player in players)
                player.GetComponent<PlayerStats>().AddExp(ExpWorth(player.GetComponent<PlayerStats>().level));

            // Destroy the health bar, queue the destruction of all children and set their parents to null, then destroy ourself.
            healthBar.transform.parent.GetComponent<UiFollowTarget>().RemoveFromCullList();
            Destroy(healthBar.transform.parent.gameObject);
            if (healthBarSecondary != null)
            {
                healthBarSecondary.transform.parent.GetComponent<UiFollowTarget>().RemoveFromCullList();
                Destroy(healthBarSecondary.transform.parent.gameObject);
            }

            GetComponent<Animator>().SetTrigger("Downed");
            // Destroy all the now usless components while the enemy dies.
            Destroy(GetComponent<CapsuleCollider>());
            Destroy(GetComponent<UnityEngine.AI.NavMeshAgent>());
            Destroy(GetComponent<DamageNumberManager>());
            Destroy(GetComponent<EnemyMovement>());
            Destroy(GetComponent<EnemyCombatManager>());

            Destroy(gameObject, 5);
        }
        else if (gameObject.CompareTag("Player"))
        {
            Debug.Log("PlayerDeath");
            GetComponent<PlayerController>().PlayerDowned();
        }
        else
        {
            Debug.Log("PlayerSummonDeath");
        }
    }

    // Used to spawn the damage numbers or flavor text from our character.
    public void SpawnFlavorText(float amount, bool crit)
    {
        damageNumberManager.SpawnNumber(amount, crit);
    }

    // Used to set up our players and enemies health and mana bars if they were just spawned.
    private void HealthBarSetup()
    {
        if(gameObject.CompareTag("Enemy"))
        {
            // If we dont already ahve a healthbar.
            if (!healthBar)
            {
                // Spawn one and set its follow target and then grab it's healthbar script for updates.
                GameObject healthBarParent = Instantiate(enemyHealthBar, new Vector3(1000, 1000, 1000), new Quaternion(0, 0, 0, 0), GameObject.Find("PrimaryCanvas").transform);
                healthBarParent.GetComponent<UiFollowTarget>().target = transform.Find("UiFollowTarget_Name");
                healthBarParent.transform.SetAsFirstSibling();
                healthBar = healthBarParent.GetComponentInChildren<BarManager>();
            }
        }
    }

    // Adds the item stats to our current Stats
    public void AddItemStats(Item item, bool compelteStatSetup)
    {
        // Debug.Log("adding stats");
        Vit += item.vitMod;
        Str += item.strMod;
        Dex += item.dexMod;
        Spd += item.spdMod;
        Int += item.intMod;
        Wis += item.wisMod;
        Cha += item.chaMod;

        weaponVitScaling += item.vitScaling;
        weaponStrScaling += item.strScaling;
        weaponDexScaling += item.dexScaling;
        weaponSpdScaling += item.spdScaling;
        weaponIntScaling += item.intScaling;
        weaponWisScaling += item.wisScaling;
        weaponChaScaling += item.chaScaling;

        if (item.itemType == Item.ItemType.Weapon || item.itemType == Item.ItemType.TwoHandWeapon)
        {
            weaponHitspeeds.Add(item.baseAttackDelay);

            float attackSpeed = 0;
            if (weaponHitspeeds.Count > 0)
            {
                foreach (float attackDelay in weaponHitspeeds)
                    attackSpeed += attackDelay;
                weaponBaseAttackDelay = attackSpeed / weaponHitspeeds.Count;
            }
            else
                weaponBaseAttackDelay = 1;
        }

        weaponHitbase += item.hitBase;
        weaponHitMin += item.hitMin;
        weaponHitMax += item.hitMax;
        weaponStaggerBase += item.staggerBase;
        weaponCritChance += item.critChance;
        weaponCritMod += item.critMod;

        armor += item.armor;
        magicResist += item.resistance;
        poise += item.poise;
        bonusHealth += item.health;
        bonusHealthRegen += item.healthRegen;
        bonusMana += item.mana;
        bonusManaRegen += item.manaRegen;

        if(compelteStatSetup)
            StatSetup(false, true);
    }

    //Remvoes the item stats from our current Stats
    public void RemoveItemStats(Item item, bool completeStatSetup)
    {
        // Debug.Log("removing stats");
        Vit -= item.vitMod;
        Str -= item.strMod;
        Dex -= item.dexMod;
        Spd -= item.spdMod;
        Int -= item.intMod;
        Wis -= item.wisMod;
        Cha -= item.chaMod;

        weaponVitScaling -= item.vitScaling;
        weaponStrScaling -= item.strScaling;
        weaponDexScaling -= item.dexScaling;
        weaponSpdScaling -= item.spdScaling;
        weaponIntScaling -= item.intScaling;
        weaponWisScaling -= item.wisScaling;
        weaponChaScaling -= item.chaScaling;

        if (item.itemType == Item.ItemType.Weapon || item.itemType == Item.ItemType.TwoHandWeapon)
        {
            weaponHitspeeds.Remove(item.baseAttackDelay);

            float attackSpeed = 0;
            if (weaponHitspeeds.Count > 0)
            {
                foreach (float attackDelay in weaponHitspeeds)
                    attackSpeed += attackDelay;
                weaponBaseAttackDelay = attackSpeed / weaponHitspeeds.Count;
            }
            else
                weaponBaseAttackDelay = 1;
        }

        weaponHitbase -= item.hitBase;
        weaponHitMin -= item.hitMin;
        weaponHitMax -= item.hitMax;
        weaponStaggerBase -= item.staggerBase;
        weaponCritChance -= item.critChance;
        weaponCritMod -= item.critMod;

        armor -= item.armor;
        magicResist -= item.resistance;
        poise -= item.poise;
        bonusHealth -= item.health;
        bonusHealthRegen -= item.healthRegen;
        bonusMana -= item.mana;
        bonusManaRegen -= item.manaRegen;

        if(completeStatSetup)
            StatSetup(false, true);
    }

    // USed to check what the stats would be if we removed the stats of the other item in that slot and replaced it with this item's
    public void CheckStatChange(Item itemToAdd, Item[] itemsToRemove)
    {
        // Remove the stats, add in the new ones, then we ship off these new stats to be used as the comparison.
        if (itemsToRemove.Length > 0)
            foreach (Item item in itemsToRemove)
                if(item != null)
                    RemoveItemStats(item, false);
        if(itemToAdd != false)
            AddItemStats(itemToAdd, false);

        myStats.AssignPotentialStats(this);

        // Remove the new stats, add in the old ones.
        if (itemsToRemove.Length > 0)
            foreach (Item item in itemsToRemove)
                if (item != null)
                    AddItemStats(item, false);
        if (itemToAdd != false)
            RemoveItemStats(itemToAdd, false);
        
        myStats.CompareStatValues(this);
    }

    // Used wehn we want to force a stat value reset for when we are no logner hovering with an item.
    public void ForceStatRecheck()
    {
        StatSetup(false, false);
    }
}
