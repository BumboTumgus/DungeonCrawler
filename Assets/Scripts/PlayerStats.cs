using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public string playerName = "Jose";
    public string playerTitle = "Mighty";

    public float weaponBaseAttackDelay = 1;
    public float weaponHitMax = 4;
    public float weaponBonusHitMax = 0;
    public float weaponHitbase = 4;
    public float weaponBonusHitBase = 0;
    public float weaponCritChance = 5;
    public float weaponBonusCritChance = 0;
    public float weaponCritMod = 2;
    public float weaponBonusCritMod = 0;
    public float weaponVitScaling = 0;
    public float weaponStrScaling = 2;
    public float weaponBonusStrScaling = 0;
    public float weaponDexScaling = 2;
    public float weaponBonusDexScaling = 0;
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
    public float bonusAttackSpeed = 0;
    public float health = 100;
    public float healthMax = 100;
    public float mana = 100;
    public float manaMax = 100;
    public float armor = 5;
    public float magicResist = 5;
    public float speed = 4;
    public float strafeSpeed = 2;
    public float acceleration = 2;
    public float damageReduction = 0;
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
    public float sizeMultiplier = 1f;

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
    public BarManager manaBar;
    public StatUpdater myStats;

    public bool invulnerable = false;
    public bool untargetable = false;
    public bool invisibile = false;
    public float invulnerableCount = 0;
    public float untargetableCount = 0;
    public float invisibleCount = 0;
    public bool revitalizeBuff = false;

    public bool dead = false;
 
    public float agroRange = 3;
    public int enemyCost = 1;

    public Color levelUpColor;

    private DamageNumberManager damageNumberManager;
    private bool recentlyDamaged = false;
    private float recentlyDamagedTimer = 0;

    private const float RECENTLY_DAMAGED_TIMER_START = 1f;

    [SerializeField] private GameObject enemyHealthBar;

    private AfflictionManager afflictions;

    private void Start()
    {
        // If we do not have a heathbar, set it up now.
        HealthBarSetup();

        StatSetup(true, true);
        damageNumberManager = GetComponent<DamageNumberManager>();
        afflictions = GetComponent<AfflictionManager>();
    }

    private void Update()
    {
        //USed for debugging to add exp.
        if (Input.GetKeyDown(KeyCode.L) && CompareTag("Player"))
            AddExp(100);

        // USed for added afflcitions and debugging.
        if (Input.GetKeyDown(KeyCode.Keypad0) && CompareTag("Player"))
        {
            afflictions.AddAffliction(AfflictionManager.AfflictionTypes.Aflame, 30);
            afflictions.AddAffliction(AfflictionManager.AfflictionTypes.Asleep, 30);
            afflictions.AddAffliction(AfflictionManager.AfflictionTypes.Bleed, 30);
            afflictions.AddAffliction(AfflictionManager.AfflictionTypes.Corrosion, 30);
            afflictions.AddAffliction(AfflictionManager.AfflictionTypes.Curse, 30);
            afflictions.AddAffliction(AfflictionManager.AfflictionTypes.Frostbite, 30);
            afflictions.AddAffliction(AfflictionManager.AfflictionTypes.Poison, 30);
            afflictions.AddAffliction(AfflictionManager.AfflictionTypes.Stun, 30);
        }

        if (Input.GetKeyDown(KeyCode.Keypad1) && CompareTag("Player"))
            afflictions.AddAffliction(AfflictionManager.AfflictionTypes.Aflame, 30);
        if (Input.GetKeyDown(KeyCode.Keypad2) && CompareTag("Player"))
            afflictions.AddAffliction(AfflictionManager.AfflictionTypes.Asleep, 30);
        if (Input.GetKeyDown(KeyCode.Keypad3) && CompareTag("Player"))
            afflictions.AddAffliction(AfflictionManager.AfflictionTypes.Bleed, 30);
        if (Input.GetKeyDown(KeyCode.Keypad4) && CompareTag("Player"))
            afflictions.AddAffliction(AfflictionManager.AfflictionTypes.Corrosion, 30);
        if (Input.GetKeyDown(KeyCode.Keypad5) && CompareTag("Player"))
            afflictions.AddAffliction(AfflictionManager.AfflictionTypes.Curse, 30);
        if (Input.GetKeyDown(KeyCode.Keypad6) && CompareTag("Player"))
            afflictions.AddAffliction(AfflictionManager.AfflictionTypes.Frostbite, 30);
        if (Input.GetKeyDown(KeyCode.Keypad7) && CompareTag("Player"))
            afflictions.AddAffliction(AfflictionManager.AfflictionTypes.Poison, 30);
        if (Input.GetKeyDown(KeyCode.Keypad8) && CompareTag("Player"))
            afflictions.AddAffliction(AfflictionManager.AfflictionTypes.Stun, 30);
        if (Input.GetKeyDown(KeyCode.P) && CompareTag("Player"))
            AddExp(25);

        // Health and mana regen logic.
        if (!dead)
        {
            health += healthRegen * Time.deltaTime;
            if (revitalizeBuff)
            {
                healthRegen = (Vit * 0.2f + bonusHealthRegen) * (1 + (1 - (health / healthMax)) * 3f);
                myStats.SetHealthManaRegenValues(this);
            }
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
        if (manaBar != null)
            manaBar.targetValue = mana;

        // Regen poise if we havent been hit in a while, if not increment the timer.
        //if (!recentlyDamaged)
        //{
            //if (poise < poiseMax)
                //poise += poiseMax / 3 * Time.deltaTime;
        //}
        //else
        //{
            //recentlyDamagedTimer -= Time.deltaTime;
            //if (recentlyDamagedTimer <= 0)
               // recentlyDamaged = false;
        //}
    }

    // Used to set up the stats at the start of the game and every time we level.
    public void StatSetup(bool LeveledUp, bool changeHealthBars)
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
        attackSpeed = 1 + 0.025f * Spd + 0.0125f * Dex + bonusAttackSpeed;
        attackDelay = (1 / weaponBaseAttackDelay) / attackSpeed;
        strafeSpeed = speed / 2;
        acceleration = speed;
        if (transform.CompareTag("Enemy"))
            GetComponent<UnityEngine.AI.NavMeshAgent>().speed = speed;

        // Sets up the characters poise, which is their resistance to being staggered.
        // poiseMax = Str + Vit;
        // if (gameObject.CompareTag("Player"))
            // poiseMax += 20;

        if (changeHealthBars)
        {
            // Sets up the health and mana Bars.
            healthBar.Initialize(healthMax, false);
            if (manaBar != null)
                manaBar.Initialize(manaMax, false);
            if(CompareTag("Player"))
            {
                healthBar.GetComponent<BarResizer>().ResizeBar(healthMax / 3 + 90);
                manaBar.GetComponent<BarResizer>().ResizeBar(manaMax / 3 + 90); 
            }
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
        Debug.Log("+" + value + " || " + exp + " / " + expTarget);
        if (exp >= expTarget)
        {
            exp -= expTarget;
            level++;
            LevelUp();
            expTarget = level * 100;
            GetComponent<DamageNumberManager>().SpawnFlavorText("Level Up!", levelUpColor);
            StatSetup(true, true);
            GetComponent<SkillsManager>().ps[40].Play();
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
        
        return expValue;
    }

    // USed to use some mana, if we do not have enpugh, then return false;
    public bool UseMana(float amount)
    {
        bool enoughMana = true;

        // Check to see if we have enough mana
        if (mana > amount)
        {
            mana -= amount;
        }
        else
            enoughMana = false;

        return enoughMana;
    }

    // Used to take damage
    public void TakeDamage(float amount, bool crit)
    {
        if (health > 0)
        {
            if (damageReduction < 100)
                amount *= (100f - damageReduction) / 100f;
            else
                amount = 0;

            if(GetComponent<PlayerController>() != null && GetComponent<PlayerController>().asleep)
            {
                GetComponent<PlayerController>().asleep = false;
                // disable the sleeping debuff.
                foreach (Buff buff in GetComponent<BuffsManager>().activeBuffs)
                {
                    if (buff.myType == BuffsManager.BuffType.Asleep)
                        buff.EndBuff();
                }
                amount *= 2f;
            }
            if (amount > 0)
                health -= amount;
            if (health < 0)
                health = 0;
            recentlyDamaged = true;
            recentlyDamagedTimer = RECENTLY_DAMAGED_TIMER_START;

            // Update the health bar.
            healthBar.targetValue = health;

            // Spawn the damage number.
            SpawnFlavorText(amount, crit);

            // If we are dead, call the death logic method.
            if (health <= 0 && !dead)
                EntityDeath();
        }
    }
    // Used to take damage and overide the cvolor of the text
    public void TakeDamage(float amount, bool crit, Color colorOveride)
    {
        if (health > 0)
        {
            if (amount > 0 )
                health -= amount;
            if (health < 0)
                health = 0;
            recentlyDamaged = true;
            recentlyDamagedTimer = RECENTLY_DAMAGED_TIMER_START;

            // Update the health bar.
            healthBar.targetValue = health;

            // Spawn the damage number.
            SpawnFlavorText(amount, crit, colorOveride);

            // If we are dead, call the death logic method.
            if (health <= 0 && !dead)
                EntityDeath();
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

            float playerAverageLevel = 0;
            // If any player was agrod onto us, end their combat. and add exp to all players.
            foreach (GameObject player in players)
            {
                player.GetComponent<PlayerStats>().AddExp(ExpWorth(player.GetComponent<PlayerStats>().level));
                playerAverageLevel += player.GetComponent<PlayerStats>().level;
            }
            playerAverageLevel /= players.Length;

            // Create the exp value text the player sees when an enmy dies.
            GetComponent<DamageNumberManager>().SpawnEXPValue(ExpWorth(playerAverageLevel));

            // Destroy the health bar, queue the destruction of all children and set their parents to null, then destroy ourself.
            healthBar.transform.parent.GetComponent<UiFollowTarget>().RemoveFromCullList();
            Destroy(healthBar.transform.parent.gameObject);

            GetComponent<Animator>().SetTrigger("Downed");


            // Destroy all the now usless components while the enemy dies.
            Destroy(GetComponent<CapsuleCollider>());
            Destroy(GetComponent<UnityEngine.AI.NavMeshAgent>());
            Destroy(GetComponent<DamageNumberManager>());
            Destroy(GetComponent<Rigidbody>());
            Destroy(GetComponent<EnemyMovementManager>());
            Destroy(GetComponent<EnemyCombatController>());

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
    // Used to spawn the damage numbers or flavor text from our character, with a color overide
    public void SpawnFlavorText(float amount, bool crit, Color colorOveride)
    {
        damageNumberManager.SpawnNumber(amount, crit, colorOveride);
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
                GetComponent<BuffsManager>().canvasParent = healthBarParent.transform.Find("BuffIconParents");
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
        weaponBonusStrScaling += item.strScaling;
        weaponBonusDexScaling += item.dexScaling;
        weaponSpdScaling += item.spdScaling;
        weaponIntScaling += item.intScaling;
        weaponWisScaling += item.wisScaling;
        weaponChaScaling += item.chaScaling;

        if (item.itemType == Item.ItemType.Weapon || item.itemType == Item.ItemType.TwoHandWeapon)
        {
            weaponHitspeeds.Add(item.baseAttackDelay);
            
            float totalAttackDelay = 0;
            if (weaponHitspeeds.Count > 0)
            {
                foreach (float attackDelay in weaponHitspeeds)
                    totalAttackDelay += attackDelay;
                weaponBaseAttackDelay = totalAttackDelay / weaponHitspeeds.Count;
            }
            else
                weaponBaseAttackDelay = 1;
        }

        weaponBonusHitBase += item.hitBase;
        weaponBonusHitMax += item.hitMax;
        weaponBonusCritChance += item.critChance;
        weaponBonusCritMod += item.critMod;
        
        if (weaponHitspeeds.Count > 0)
        {
            weaponCritChance = 0;
            weaponCritMod = 1;
            weaponHitMax = 0;
            weaponHitbase = 0;
            weaponDexScaling = 0;
            weaponStrScaling = 0;
        }
        else
        {
            weaponCritChance = 10;
            weaponCritMod = 2;
            weaponHitMax = 4;
            weaponHitbase = 2;
            weaponDexScaling = 1;
            weaponStrScaling = 1;
        }

        armor += item.armor;
        magicResist += item.resistance;
        // poise += item.poise;
        bonusHealth += item.health;
        bonusHealthRegen += item.healthRegen;
        bonusMana += item.mana;
        bonusManaRegen += item.manaRegen;

        afflictions.aflameResist += item.aflameResist;
        afflictions.sleepResist += item.asleepResist;
        afflictions.stunResist += item.stunResist;
        afflictions.curseResist += item.curseResist;
        afflictions.bleedResist += item.bleedResist;
        afflictions.poisonResist += item.poisonResist;
        afflictions.corrosionResist += item.corrosionResist;
        afflictions.frostbiteResist += item.frostbiteResist;
        afflictions.knockBackResist += item.knockbackResist;

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
        weaponBonusStrScaling -= item.strScaling;
        weaponBonusDexScaling -= item.dexScaling;
        weaponSpdScaling -= item.spdScaling;
        weaponIntScaling -= item.intScaling;
        weaponWisScaling -= item.wisScaling;
        weaponChaScaling -= item.chaScaling;

        if (item.itemType == Item.ItemType.Weapon || item.itemType == Item.ItemType.TwoHandWeapon)
        {
            weaponHitspeeds.Remove(item.baseAttackDelay);

            float totalAttackDelay = 0;
            if (weaponHitspeeds.Count > 0)
            {
                foreach (float attackDelay in weaponHitspeeds)
                    totalAttackDelay += attackDelay;
                weaponBaseAttackDelay = totalAttackDelay / weaponHitspeeds.Count;
            }
            else
                weaponBaseAttackDelay = 1;
        }

        weaponBonusHitBase -= item.hitBase;
        weaponBonusHitMax -= item.hitMax;
        weaponBonusCritChance -= item.critChance;
        weaponBonusCritMod -= item.critMod;

        if(weaponHitspeeds.Count > 0)
        {
            weaponCritChance = 0;
            weaponCritMod = 1;
            weaponHitMax = 0;
            weaponHitbase = 0;
            weaponStrScaling = 0;
            weaponDexScaling = 0;
        }
        else
        {
            weaponCritChance = 10;
            weaponCritMod = 2;
            weaponHitMax = 4;
            weaponHitbase = 2;
            weaponStrScaling = 1;
            weaponDexScaling = 1;
        }

        armor -= item.armor;
        magicResist -= item.resistance;
        // poise -= item.poise;
        bonusHealth -= item.health;
        bonusHealthRegen -= item.healthRegen;
        bonusMana -= item.mana;
        bonusManaRegen -= item.manaRegen;
        
        afflictions.aflameResist -= item.aflameResist;
        afflictions.sleepResist -= item.asleepResist;
        afflictions.stunResist -= item.stunResist;
        afflictions.curseResist -= item.curseResist;
        afflictions.bleedResist -= item.bleedResist;
        afflictions.poisonResist -= item.poisonResist;
        afflictions.corrosionResist -= item.corrosionResist;
        afflictions.frostbiteResist -= item.frostbiteResist;
        afflictions.knockBackResist -= item.knockbackResist;

        if (completeStatSetup)
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

    // USed to change the size of the player
    public void ChangeSize(float sizeValueToAdd)
    {
        StartCoroutine(ChangeSizeOverTime(sizeMultiplier + sizeValueToAdd, sizeMultiplier));
        sizeMultiplier += sizeValueToAdd;
    }

    // The coroutine that changes our size over time.
    public IEnumerator ChangeSizeOverTime(float targetValue, float initialValue)
    {
        float currentTimer = 0f;
        float targetTimer = 0.15f;

        while(currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;
            float currentScale = Mathf.Lerp(initialValue, targetValue, currentTimer / targetTimer);

            transform.localScale = new Vector3(currentScale, currentScale, currentScale);
            yield return null;
        }
    }

    // USed to add a source of invulnerablility
    public void AddInvulnerablitySource(float amount)
    {
        if (amount > 0)
        {
            invulnerable = true;
            invulnerableCount += amount;
        }
        else if(amount < 0)
        {
            invulnerableCount += amount;
            if (invulnerableCount <= 0)
                invulnerable = false;
        }
    }

    // USed to add a source of invisibility
    public void AddInvisibilitySource(float amount)
    {
        if (amount > 0)
        {
            invisibile = true;
            GetComponent<PlayerGearManager>().InvisibilityChange(true);
            invisibleCount += amount;
        }
        else if (amount < 0)
        {
            Debug.Log("we are adding " + amount);
            invisibleCount += amount;
            if (invisibleCount <= 0)
            {
                Debug.Log("the invisible count is lower than it should be");
                invisibile = false;
                GetComponent<PlayerGearManager>().InvisibilityChange(false);
            }
        }
    }

    // USed to add a source of invisibility
    public void AddUntargetableSource(float amount)
    {
        if (amount > 0)
        {
            untargetable = true;
            untargetableCount += amount;
        }
        else if (amount < 0)
        {
            untargetableCount += amount;
            if (untargetableCount <= 0)
                untargetable = false;
        }
    }
}
