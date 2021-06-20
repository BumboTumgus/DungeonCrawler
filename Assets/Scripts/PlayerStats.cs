using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public string playerName = "Jose";
    public string playerTitle = "Mighty";

    public float baseDamage = 1;
    public float baseBaseDamage = 8;
    public float baseDamageGrowth = 1;

    public float health = 100;
    public float healthMax = 100;
    public float bonusHealth = 0;
    public float bonusPercentHealth = 1;
    public float baseHealth = 125;
    public float baseHealthGrowth = 40;

    [HideInInspector] public List<float> weaponBaseAttacksPerSecond = new List<float>();
    [HideInInspector] public List<Item> weaponsToHitWith = new List<Item>();

    [HideInInspector] public List<float> cooldownReductionSources = new List<float>();
    public float cooldownReduction = 0;

    public HitBox basicAttack1;
    public HitBox basicAttack2;

    public float currentAttackDelay = 13; // 10 is fast, 100 is really slow.
    
    public float attackRange = 2; 

    public float attackSpeed = 1;
    public float weaponAttacksPerSecond = 1;
    public float bonusAttackSpeed = 0;

    public float armor = 0;
    public float armorReductionMultiplier = 1;
    public float armorShreddedBonusDamage = 0f;

    public float flatDamageReduction = 0;

    public float healingMultiplier = 1;

    public float speed = 2.5f;
    public float movespeedPercentMultiplier = 1;
    public int jumps = 1;

    public float damageReductionMultiplier = 1;
    public float damageIncreaseMultiplier = 1;

    public float healthRegen = 1;
    public float bonusHealthRegen = 0;
    public float baseHealthRegen = 1f;
    public float baseHealthRegenGrowth = 0.3333f;

    public int level = 1;
    public float exp = 0;
    public float expTarget = 100;

    public float sizeMultiplier = 1f;

    public float critChance = 0f;
    public float critDamageMultiplier = 1.5f;

    public float gold = 0;

    public MoneyUiCounterBehaviour moneyCounter;

    public float aflameResistance = 0f;
    public float frostbiteResistance = 0f;
    public float overchargeResistance = 0f;
    public float overgrowthResistance = 0f;
    public float sunderResistance = 0f;
    public float windshearResistance = 0f;
    public float stunResistance = 0f;
    public float knockbackResistance = 0f;
    public float sleepResistance = 0f;
    public float bleedResistance = 0f;
    public float poisonResistance = 0f;
    public float slowResistance = 0f;

    public BarManager healthBar;
    public StatUpdater myStats;

    [HideInInspector] public bool invulnerable = false;
    [HideInInspector] public bool untargetable = false;
    [HideInInspector] public bool invisibile = false;
    [HideInInspector] public bool stunned = false;
    [HideInInspector] public bool knockedBack = false;
    [HideInInspector] public bool asleep = false;
    [HideInInspector] public bool frozen = false;
    [HideInInspector] public bool bleeding = false;
    [HideInInspector] public bool ephemeral = false;
    [HideInInspector] public bool channeling = false;
    [HideInInspector] public bool flameWalkerEnabled = false;
    [HideInInspector] public bool immolationEnabled = false;
    [HideInInspector] public bool arcticAuraEnabled = false;
    [HideInInspector] public float counterDamage = 0;
    [HideInInspector] public bool counter = false;
    [HideInInspector] public float invulnerableCount = 0;
    [HideInInspector] public float untargetableCount = 0;
    [HideInInspector] public float invisibleCount = 0;
    [HideInInspector] public float ephemeralCount = 0;
    //[HideInInspector] public float revitalizeCount = 0;
    //[HideInInspector] public bool revitalizeBuff = false;

    [HideInInspector] public bool dead = false;

    private float immolateCurrentTimer = 0;
    private float immolateTargetTimer = 0.5f;
    private float arcticAuraCurrentTimer = 0;
    private float arcticAuraTargetTimer = 2f;

    private DamageNumberManager damageNumberManager;
    private BuffsManager buffManager;
    private HitBoxManager hitboxManager;
    private PlayerTraitManager playerTraitManager;
    public ComboManager comboManager;
    public PlayerStats lastHitBy;

    public bool traitMoreAflameStacksOnHitThresholdFatigue = false;
    public bool traitPoisonFireSpellOnHitReady = true;
    private float traitPoisonFireSpellOnHitCurrentTimer = 0;
    private float traitPoisonFireSpellOnHitTargetTimer = 3;
    public bool traitAflameStunsPeriodicallyReady = true;
    private float traitAflameStunPeriodicallyCurrentTimer = 0;
    public float traitAflameStunPeriodicallyTargetTimer = 30;
    public bool traitAflameStunStunOnThresholdReady = true;
    public bool traitFreezeOnThresholdReady = true;
    public bool traitFreezeRefreshesStunReady = true;
    public bool traitEarthMaxHpDamageReady = true;
    public bool traitEarthAfflictionDamageAmpReady = true;
    public bool traitEarthTrueDamageConversion = false;
    public bool traitEarthPoisonSummonPillarOnThresholdReady = true;
    public bool traitEarthStunStunOnThresholdReady = true;
    public bool traitEarthKnockbackRocksOnSunderReady = true;
    public bool traitWindBleedBonusDamageAtThresholdEnabled = false;
    public bool traitLaunchKnivesIfKnockedbackReady = true;
    public bool traitBleedBloodWellOnThresholdReady = true;
    public bool traitBleedStunStunBelowHalfHPReady = true;

    [SerializeField] private GameObject enemyHealthBar;

    private SkillsManager skills;

    private void Start()
    {
        if (!CompareTag("Hazard"))
        {
            // If we do not have a heathbar, set it up now.
            HealthBarSetup();

            damageNumberManager = GetComponent<DamageNumberManager>();
            buffManager = GetComponent<BuffsManager>();
            skills = GetComponent<SkillsManager>();
            hitboxManager = GetComponent<HitBoxManager>();

            StatSetup(true, true);

            if (CompareTag("Enemy"))
            {
                EnemyManager.instance.enemyStats.Add(this);
            }
            else
            {
                UpdateWeaponsToHitWith();
                comboManager = GetComponent<ComboManager>();

                playerTraitManager = GetComponent<PlayerTraitManager>();
            }
        }

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O) && CompareTag("Player"))
            TakeDamage(50, false, HitBox.DamageType.Physical, 0, null);
        if (Input.GetKeyDown(KeyCode.L) && CompareTag("Player"))
            AddGold(25);
        if (Input.GetKeyDown(KeyCode.K) && CompareTag("Player"))
            AddGold(-1 * gold);
        if (Input.GetKeyDown(KeyCode.J) && CompareTag("Player"))
            AddGold((int)Random.Range(1, 100000));
        if (Input.GetKeyDown(KeyCode.H) && CompareTag("Player"))
            ItemGenerator.instance.IncrementRcIndex();
        /*
        //USed to bebug money and the economy
          
        //USed for debugging to add exp.
        if (Input.GetKeyDown(KeyCode.L) && CompareTag("Player"))
            AddExp(1000);
        if (Input.GetKeyDown(KeyCode.P) && CompareTag("Player"))
            AddExp(25);
        if (Input.GetKeyDown(KeyCode.O) && CompareTag("Player"))
            TakeDamage(50, false, HitBox.DamageType.Physical, 0, null);
        if (Input.GetKeyDown(KeyCode.U) && CompareTag("Player"))
            comboManager.AddComboCounter(1);

        if (Input.GetKeyDown(KeyCode.Keypad0) && CompareTag("Player"))
            buffManager.CheckResistanceToBuff(BuffsManager.BuffType.Aflame, 1, baseDamage, this);
        if (Input.GetKeyDown(KeyCode.Keypad1) && CompareTag("Player"))
            buffManager.CheckResistanceToBuff(BuffsManager.BuffType.Frostbite, 1, baseDamage, this);
        if (Input.GetKeyDown(KeyCode.Keypad2) && CompareTag("Player"))
            buffManager.CheckResistanceToBuff(BuffsManager.BuffType.Overcharge, 1, baseDamage, this);
        if (Input.GetKeyDown(KeyCode.Keypad3) && CompareTag("Player"))
            buffManager.CheckResistanceToBuff(BuffsManager.BuffType.Overgrown, 1, baseDamage, this);
        if (Input.GetKeyDown(KeyCode.Keypad4) && CompareTag("Player"))
            buffManager.CheckResistanceToBuff(BuffsManager.BuffType.Windshear, 1, baseDamage, this);
        if (Input.GetKeyDown(KeyCode.Keypad5) && CompareTag("Player"))
            buffManager.CheckResistanceToBuff(BuffsManager.BuffType.Sunder, 1, baseDamage, this);
        if (Input.GetKeyDown(KeyCode.Keypad6) && CompareTag("Player"))
            buffManager.CheckResistanceToBuff(BuffsManager.BuffType.Bleeding, 1, baseDamage, this);
        if (Input.GetKeyDown(KeyCode.Keypad7) && CompareTag("Player"))
            buffManager.CheckResistanceToBuff(BuffsManager.BuffType.Poisoned, 1, baseDamage, this);


        if (Input.GetKeyDown(KeyCode.Keypad8))
            buffManager.CheckResistanceToBuff(BuffsManager.BuffType.Frozen, 1, baseDamage, this);
        if (Input.GetKeyDown(KeyCode.Keypad9))
            buffManager.CheckResistanceToBuff(BuffsManager.BuffType.Asleep, 1, baseDamage, this);
        if (Input.GetKeyDown(KeyCode.KeypadPeriod))
            buffManager.CheckResistanceToBuff(BuffsManager.BuffType.Stunned, 1, baseDamage, this);
        if (Input.GetKeyDown(KeyCode.KeypadEnter) && CompareTag("Player"))
            buffManager.NewBuff(BuffsManager.BuffType.ArmorBroken, baseDamage, this);
        if (Input.GetKeyDown(KeyCode.KeypadPlus) && CompareTag("Player"))
            GetComponent<PlayerMovementController>().KnockbackLaunch((transform.forward + Vector3.up) * 5, this);
        if (Input.GetKeyDown(KeyCode.KeypadMinus))
            buffManager.CheckResistanceToBuff(BuffsManager.BuffType.GreviousWounds, 1, baseDamage, this);
        if (Input.GetKeyDown(KeyCode.KeypadMinus))
            GetComponent<SkillsManager>().ReduceSkillCooldowns(2f, false);
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
            GetComponent<SkillsManager>().ReduceSkillCooldowns(0.5f, true);
        */
        // Health and mana regen logic.
        /*
        if (!dead)
        {
            float revitalizeBonus = 0f;
            if (revitalizeBuff)
            {
                //revitalizeBonus = (Vit * 0.2f + bonusHealthRegen) * (1 - (health / healthMax)) * 2f;
                //healthRegen = Vit * 0.2f + bonusHealthRegen + (revitalizeBonus * revitalizeCount);
                myStats.UpdateHealthManaBarValues(this);
            }

        }
        */
        if (!CompareTag("Hazard"))
        {
            if (!dead)
                health += healthRegen * Time.deltaTime;

            if (health > healthMax)
                health = healthMax;
            else if (myStats != null)
                myStats.UpdateHealthManaBarValues(this);

            // immolation logic.
            if (immolationEnabled)
            {
                immolateCurrentTimer += Time.deltaTime;
                if (immolateCurrentTimer >= immolateTargetTimer)
                {
                    immolateCurrentTimer -= immolateTargetTimer;
                    // flicker the hit box.
                    hitboxManager.hitboxes[22].GetComponent<HitBox>().damage = baseDamage * 0.5f;
                    foreach (Buff buff in buffManager.activeBuffs)
                    {
                        if (buff.myType == BuffsManager.BuffType.Aflame)
                        {
                            hitboxManager.hitboxes[22].GetComponent<HitBox>().damage = ((buff.currentStacks * 0.045f) + 0.5f) * baseDamage;
                            break;
                        }
                    }

                    hitboxManager.LaunchHitBox(22);
                }
            }
            else
                immolateCurrentTimer = 0;

            // artic aura logic.
            if (arcticAuraEnabled)
            {
                arcticAuraCurrentTimer += Time.deltaTime;
                if (arcticAuraCurrentTimer >= arcticAuraTargetTimer)
                {
                    arcticAuraCurrentTimer -= arcticAuraTargetTimer;
                    // flicker the hit box.

                    hitboxManager.LaunchHitBox(28);
                    hitboxManager.PlayParticles(67);
                }
            }
            else
                arcticAuraCurrentTimer = 0;

            // Update the health bar.
            healthBar.targetValue = health;

            if (!traitPoisonFireSpellOnHitReady)
            {
                traitPoisonFireSpellOnHitCurrentTimer += Time.deltaTime;
                if (traitPoisonFireSpellOnHitCurrentTimer >= traitPoisonFireSpellOnHitTargetTimer)
                {
                    traitPoisonFireSpellOnHitReady = true;
                    traitPoisonFireSpellOnHitCurrentTimer = 0;
                }
            }
            if (!traitAflameStunsPeriodicallyReady)
            {
                traitAflameStunPeriodicallyCurrentTimer += Time.deltaTime;
                if (traitAflameStunPeriodicallyCurrentTimer >= traitAflameStunPeriodicallyTargetTimer)
                {
                    traitAflameStunsPeriodicallyReady = true;
                    traitAflameStunPeriodicallyCurrentTimer = 0;
                }
            }
        }
    }

    // Used to set up the stats at the start of the game and every time we level.
    public void StatSetup(bool LeveledUp, bool changeHealthBars)
    {
        baseDamage = baseBaseDamage + baseDamageGrowth * level;

        healthMax = (baseHealth + baseHealthGrowth * level + bonusHealth) * bonusPercentHealth;
        if (health > healthMax)
            health = healthMax;

        attackSpeed = 1 + bonusAttackSpeed;

        traitPoisonFireSpellOnHitTargetTimer = 3 / attackSpeed;

        // If we level up set the health to the max.
        if (LeveledUp)
            health = healthMax;

        // Ste up our health and manaRegen;
        healthRegen = (baseHealthRegen + baseHealthRegenGrowth * level + bonusHealthRegen) * healingMultiplier;

        if (transform.CompareTag("Enemy"))
            GetComponent<UnityEngine.AI.NavMeshAgent>().speed = speed * movespeedPercentMultiplier;

        cooldownReduction = 0;

        foreach(float cdr in cooldownReductionSources)
        {
            // grab the amount of perentage remaining.
            float totalAmountToReduce = 1 - cooldownReduction;
            // add cdr to that percentage.
            totalAmountToReduce *= cdr;
            // add it back to total
            cooldownReduction += totalAmountToReduce;
        }

        if(CompareTag("Player"))
            skills.UpdateCooldownSkillCooldowns();

        if (changeHealthBars)
        {
            // Sets up the health and mana Bars.
            healthBar.Initialize(healthMax, false, true, health);
            if(CompareTag("Player"))
                myStats.mouseWithItemHovered = false;
        }

        if(myStats != null)
            myStats.SetStatValues(this);
    }

    // USed to add gold to the players
    public void AddGold(float amount)
    {
        gold += amount;
        moneyCounter.ChangeGoldValue(gold);
        myStats.UpdateGoldCounter(this);

        if (amount < 0)
            Instantiate(GetComponent<SkillsManager>().skillProjectiles[85], transform.position + Vector3.up, transform.rotation);
    }

    // Used when the player gains exp.
    public void AddExp(float value)
    {
        exp += value;
        if (myStats != null)
            myStats.SetStatValues(this);
        //Debug.Log("+" + value + " || " + exp + " / " + expTarget);
        while (exp >= expTarget)
        {
            exp -= expTarget;
            level++;
            expTarget = level * 100;
            GetComponent<DamageNumberManager>().SpawnFlavorText("Level Up!", UiPopUpTextColorBank.instance.damageColors[0]);
            StatSetup(true, true);
            GetComponent<HitBoxManager>().PlayParticles(8);
            Debug.Log("Level Up");
        }
    }

    //USed to ugrade the enemies base stats of the enemies
    public void LevelUpEnemy(int currentLevel)
    {
        int levelDifference = currentLevel - level;
        for(int index = 0; index < levelDifference; index++)
        {
            baseDamage *= 1.1f;
            bonusHealth *= 1.1f;
        }
        level = currentLevel;
        StatSetup(false, true);
    }

    // Used to heal Health
    public void HealHealth(float amount, HitBox.DamageType damage)
    {
        if (healingMultiplier > 0)
            amount *= healingMultiplier;
        else
            amount = 0;

        if (amount > 0)
        {
            health += amount;
            if (health > healthMax)
                health = healthMax;

            Color chosenColor = Color.white;
            switch (damage)
            {
                case HitBox.DamageType.Healing:
                    chosenColor = UiPopUpTextColorBank.instance.damageColors[14];
                    break;
                default:
                    break;
            }

            // Spawn the damage number.
            SpawnFlavorText(amount, false, chosenColor);
        }
    }

    // Used to take damage
    public void TakeDamage(float amount, bool crit, HitBox.DamageType damage, float comboCount, PlayerStats playerThatLastHitUs)
    {
        if (health > 0)
        {
            lastHitBy = playerThatLastHitUs;

            if (counter)
            {
                counterDamage += amount;
                amount = 0;
            }

            if (immolationEnabled && damage == HitBox.DamageType.Fire)
            {
                amount *= 0.1f;
            }

            if (asleep)
            {
                asleep = false;
                // disable the sleeping debuff.
                foreach (Buff buff in GetComponent<BuffsManager>().activeBuffs)
                {
                    if (buff.myType == BuffsManager.BuffType.Asleep)
                        buff.EndBuff();
                }
                amount *= 2f;
            }

            if (!CompareTag("Player"))
            {
                if (GetComponent<BuffsManager>().PollForBuffStacks(BuffsManager.BuffType.Windshear) >= 20 && lastHitBy.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.WindAmpsComboArmorShred) > 0)
                    comboCount *= 1 + lastHitBy.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.WindAmpsComboArmorShred);
                if (damage == HitBox.DamageType.Physical && playerThatLastHitUs.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.PhysicalPhysicalSkillsComboAmp) > 0)
                    comboCount *= 1 + playerThatLastHitUs.GetComponent<PlayerTraitManager>().CheckForIdleEffectValue(ItemTrait.TraitType.PhysicalPhysicalSkillsComboAmp);
                //Debug.Log("current combo count is: " + comboCount);
            }


            //Debug.Log("the combo count is: " + comboCount);
            if (damage != HitBox.DamageType.True)
            {
                // if th percent reduced damag and the percet increasd damage from armor are greater then 0, we multipluy the damage by the value.
                if (damageReductionMultiplier + armorShreddedBonusDamage > 0)
                {
                    amount *= damageReductionMultiplier + armorShreddedBonusDamage;
                }
                else
                    amount = 0;

                if (armor * armorReductionMultiplier - comboCount > 0)
                    amount *= 100 / (100 + (armor * armorReductionMultiplier - comboCount));

                amount -= flatDamageReduction;
            }

            if (amount > 0)
                health -= amount;
            if (health < 0)
                health = 0;

            if (CompareTag("Enemy") && GetComponent<EnemyCombatController>() != null && GetComponent<EnemyCombatController>().onHitActionHierarchy.Length > 0)
                GetComponent<EnemyCombatController>().CheckOnHitActionHierarchy();

            // Update the health bar.
            healthBar.targetValue = health;

            Color chosenColor = Color.white;
            switch (damage)
            {
                case HitBox.DamageType.Physical:
                    chosenColor = UiPopUpTextColorBank.instance.damageColors[0];
                    break;
                case HitBox.DamageType.Fire:
                    chosenColor = UiPopUpTextColorBank.instance.damageColors[2];
                    break;
                case HitBox.DamageType.Ice:
                    chosenColor = UiPopUpTextColorBank.instance.damageColors[3];
                    break;
                case HitBox.DamageType.Lightning:
                    chosenColor = UiPopUpTextColorBank.instance.damageColors[4];
                    break;
                case HitBox.DamageType.Nature:
                    chosenColor = UiPopUpTextColorBank.instance.damageColors[5];
                    break;
                case HitBox.DamageType.Earth:
                    chosenColor = UiPopUpTextColorBank.instance.damageColors[6];
                    break;
                case HitBox.DamageType.Wind:
                    chosenColor = UiPopUpTextColorBank.instance.damageColors[7];
                    break;
                case HitBox.DamageType.Poison:
                    chosenColor = UiPopUpTextColorBank.instance.damageColors[9];
                    break;
                case HitBox.DamageType.Bleed:
                    chosenColor = UiPopUpTextColorBank.instance.damageColors[8];
                    break;
                case HitBox.DamageType.True:
                    chosenColor = UiPopUpTextColorBank.instance.damageColors[1];
                    break;
                default:
                    break;
            }
            // Spawn the damage number.
            SpawnFlavorText(amount, crit, chosenColor);

            // If we are dead, call the death logic method.
            if (health <= 0 && !dead)
                EntityDeath(damage);
        }
    }

    // Used when this object dies. What will happen afterwards?
    public void EntityDeath(HitBox.DamageType damageType)
    {
        //Debug.Log("SOmething died");
        dead = true;
        // Three cases, player death, player summon death, or an enemy death.
        if(gameObject.CompareTag("Enemy"))
        {
            // Debug.Log("EnemyDeath");

            // Find the player, and give them exp. If they were in combat with us, end the combat. Start the death coroutine (for a death animation).
            // Create an array of all players.
            lastHitBy.GetComponent<BuffsManager>().ProcOnKill(gameObject, damageType);

            GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
            GameObject[] players = new GameObject[gm.currentPlayers.Length];
            for(int index = 0; index < players.Length; index++)
            {
                players[index] = gm.currentPlayers[index];
            }

            // If any player was agrod onto us, end their combat. and add exp to all players.
            foreach (GameObject player in players)
            {
                player.GetComponent<PlayerStats>().AddExp(exp);
                player.GetComponent<PlayerStats>().AddGold(gold);
            }

            // Create the exp value text the player sees when an enmy dies.
            GetComponent<DamageNumberManager>().SpawnEXPValue(exp);

            // Destroy the health bar, queue the destruction of all children and set their parents to null, then destroy ourself.
            healthBar.transform.parent.GetComponent<UiFollowTarget>().RemoveFromCullList();
            Destroy(healthBar.transform.parent.gameObject);

            GetComponent<Animator>().SetTrigger("Downed"); 
            EnemyManager.instance.enemyStats.Remove(this);

            Animator anim = GetComponent<Animator>();
            for(int animIndex = 0; animIndex < 6; animIndex++)
                anim.Play("Idle", animIndex);

            // Destroy all the now usless components while the enemy dies.
            Destroy(GetComponent<Rigidbody>());
            Destroy(GetComponent<CapsuleCollider>());
            Destroy(GetComponent<DamageNumberManager>());
            Destroy(GetComponent<HitBoxManager>());
            Destroy(GetComponent<UnityEngine.AI.NavMeshAgent>());
            Destroy(GetComponent<BuffsManager>());
            Destroy(GetComponent<EnemyMovementManager>());
            Destroy(GetComponent<EnemyCombatController>());
            Destroy(GetComponent<EnemyCrowdControlManager>());
            Destroy(GetComponent<EnemyAbilityBank>());
            Destroy(GetComponent<RagdollManager>()); 

            Destroy(gameObject, 5);
            Destroy(this);
        }
        else if (gameObject.CompareTag("Player"))
        {
            Debug.Log("PlayerDeath");
            GetComponent<PlayerMovementController>().PlayerDowned();
            GameManager.instance.PlayerDeath();
        }
        else
        {
            Debug.Log("PlayerSummonDeath");
        }
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
    public void AddItemStats(Item item, bool compelteStatSetup, bool statCompare)
    {
        //Debug.Log("adding stats");
        if(item.itemType == Item.ItemType.Weapon || item.itemType == Item.ItemType.TwoHandWeapon)
        {

            weaponBaseAttacksPerSecond.Add(item.attacksPerSecond);

            float totalWeaponAttackSpeeds = 0f;
            if (weaponBaseAttacksPerSecond.Count > 0)
            {
                foreach (float value in weaponBaseAttacksPerSecond)
                    totalWeaponAttackSpeeds += value;
                if (weaponBaseAttacksPerSecond.Count >= 2)
                    totalWeaponAttackSpeeds /= 2;
                weaponAttacksPerSecond = totalWeaponAttackSpeeds;
            }
            else
                weaponAttacksPerSecond = 1;

            //Debug.Log("adding the weapon to the list");
            weaponsToHitWith.Add(item);
            //Debug.Log("the count should increase here : " + weaponsToHitWith.Count);
            UpdateWeaponsToHitWith();
        }

        foreach(ItemTrait trait in item.itemTraits)
        {
            switch (trait.traitType)
            {
                case ItemTrait.TraitType.HealthFlat:
                    bonusHealth += trait.traitBonus * trait.traitBonusMultiplier;
                    break;
                case ItemTrait.TraitType.HealthPercent:
                    bonusPercentHealth += trait.traitBonus * trait.traitBonusMultiplier;
                    break;
                case ItemTrait.TraitType.Armor:
                    armor += trait.traitBonus * trait.traitBonusMultiplier;
                    break;
                case ItemTrait.TraitType.HealthRegen:
                    bonusHealthRegen += trait.traitBonus * trait.traitBonusMultiplier;
                    break;
                case ItemTrait.TraitType.HealingOnHit:
                    playerTraitManager.AddOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.HealingOnKill:
                    playerTraitManager.AddOnKillEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.CooldownReduction:
                    cooldownReductionSources.Add(trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.AttackSpeed:
                    bonusAttackSpeed += trait.traitBonus * trait.traitBonusMultiplier;
                    break;
                case ItemTrait.TraitType.MoveSpeed:
                    movespeedPercentMultiplier += trait.traitBonus * trait.traitBonusMultiplier;
                    break;
                case ItemTrait.TraitType.CritChance:
                    critChance += trait.traitBonus * trait.traitBonusMultiplier;
                    break;
                case ItemTrait.TraitType.CritDamage:
                    critDamageMultiplier += trait.traitBonus * trait.traitBonusMultiplier;
                    break;
                case ItemTrait.TraitType.Jumps:
                    jumps += (int)trait.traitBonus * trait.traitBonusMultiplier;
                    break;
                case ItemTrait.TraitType.AflameResistance:
                    aflameResistance += trait.traitBonus * trait.traitBonusMultiplier;
                    break;
                case ItemTrait.TraitType.FrostbiteResistance:
                    frostbiteResistance += trait.traitBonus * trait.traitBonusMultiplier;
                    break;
                case ItemTrait.TraitType.SunderResistance:
                    sunderResistance += trait.traitBonus * trait.traitBonusMultiplier;
                    break;
                case ItemTrait.TraitType.WindshearResistance:
                    windshearResistance += trait.traitBonus * trait.traitBonusMultiplier;
                    break;
                case ItemTrait.TraitType.OverchargeResistance:
                    overchargeResistance += trait.traitBonus * trait.traitBonusMultiplier;
                    break;
                case ItemTrait.TraitType.OvergrowthResistance:
                    overgrowthResistance += trait.traitBonus * trait.traitBonusMultiplier;
                    break;
                case ItemTrait.TraitType.BleedResistance:
                    bleedResistance += trait.traitBonus * trait.traitBonusMultiplier;
                    break;
                case ItemTrait.TraitType.PoisonResistance:
                    poisonResistance += trait.traitBonus * trait.traitBonusMultiplier;
                    break;
                case ItemTrait.TraitType.StunResistance:
                    stunResistance += trait.traitBonus * trait.traitBonusMultiplier;
                    break;
                case ItemTrait.TraitType.AsleepResistance:
                    sleepResistance += trait.traitBonus * trait.traitBonusMultiplier;
                    break;
                case ItemTrait.TraitType.KnockbackResistance:
                    knockbackResistance += trait.traitBonus * trait.traitBonusMultiplier;
                    break;
                case ItemTrait.TraitType.FlatDamageReduction:
                    flatDamageReduction += trait.traitBonus * trait.traitBonusMultiplier;
                    break;
                case ItemTrait.TraitType.FireExplosionOnKill:
                    playerTraitManager.AddOnKillEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.MoreAflameStacksOnHitThreshold:
                    playerTraitManager.AddOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.BurnDoesMaxHpDamageAtThreshold:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.BasicAttacksShredArmorOnAflame:
                    playerTraitManager.AddOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.FlameVamperism:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.RingOfFireOnHit:
                    playerTraitManager.AddOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.AflameToSunderStackOnEarthSpell:
                    playerTraitManager.AddOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.SunderFurtherDecreasesFireResist:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.AflameSunderCritsSummonFireballs:
                    playerTraitManager.AddOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.AflameWindshearWindAttacksGainCritOnBurningTarget:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.AflameWindshearSummonFirePillarsOnHit:
                    playerTraitManager.AddOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.AflameWindshearWindSpellsAddFireStacks:
                    playerTraitManager.AddOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.AflamePhysicalAddFireStacksOnHit:
                    playerTraitManager.AddOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.AflamePhysicalDamageAmpOnBurningTarget:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.AflamePhysicalBladeExplosionOnKill:
                    playerTraitManager.AddOnKillEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.AflamePhysicalBigHitsAddAflame:
                    playerTraitManager.AddOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.AflameBleedIncreasesFlameCritChance:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.AflameBleedFireDamageAmpOnBleedThreshold:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.AflameBleedAflameAddsBleedAtThreshhold:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.AflameBleedAflameRemovesBleedResist:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.AflameBleedDamageAmpOnDoubleThreshhold:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.AflamePoisonBurningEnemySpreadPoisonStacksOnDeath:
                    playerTraitManager.AddOnKillEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.AflamePoisonGreviousWoundsOnStackThreshold:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.AflamePoisonPoisonReducesFireResist:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.AflamePoisonFireSpellsSummonsPoisonBurst:
                    playerTraitManager.AddOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.AflamePoisonFireAmpsPoison:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.AflamePoisonPoisonCloudOnFireKill:
                    playerTraitManager.AddOnKillEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.AflameStunPeriodBurnStun:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.AflameStunStunOnThreshold:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.AflameStunStunReducesFireResistance:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.AflameStunStunAmpsBurnDamage:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.AflameKnockbackAflameReducesKnockbackResist:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.AflameKnockbackAflameSpellsOnKnockbackedTargetExplode:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.AflameKnockbackKnockbackAmpsFireDamage:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.IceFreezeAtStackThreshold:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.IceAmpAllDamageAtThreshold:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.IceBasicAttacksConsumeStacksAtThreshold:
                    playerTraitManager.AddOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.IceEnemyAttacksWeakendAtThreshold:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.IceEnemiesGainFrostbiteOnStrikingYou:
                    playerTraitManager.AddOnStruckEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.IceAddStacksToNearbyEnemies:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    arcticAuraEnabled = true;
                    hitboxManager.hitboxes[28].GetComponent<HitBoxBuff>().frostbiteValue = Mathf.RoundToInt(playerTraitManager.CheckForIdleEffectValue(trait.traitType));
                    break;
                case ItemTrait.TraitType.IceAmpFrostbiteDamage:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.IceEarthFrostToEarthBonusDamage:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.IceEarthSunderAmpsIceDamage:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.IceEarthIceDOTAtThreshold:
                    playerTraitManager.AddOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.IceEarthEarthSpellBonusCritDamage:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.IceWindWindAmpsFrostbiteDamage:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.IceWindWindSpellsDamageAmp:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.IceWindIncreaseArmorShredPerFrostbite:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.IceWindSummonTornadoOnHit:
                    playerTraitManager.AddOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.IcePhysicalFrostbiteAmpsPhysicalCritDamage:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.IcePhysicalPhysicalVampOnFrostbite:
                    playerTraitManager.AddOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.IcePhysicalBladeVortexOnHit:
                    playerTraitManager.AddOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.IceBleedFrostbiteAmpsBleed:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.IceBleedBleedDoesDamageInstantlyOnThreshold:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.IcePoisonFreezingPoison:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.IcePoisonFrostbiteResetsPoisonAndAmps:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.IcePoisonSummonPoisonPillarOnThreshold:
                    playerTraitManager.AddOnKillEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.IceStunRudeAwakening:
                    playerTraitManager.AddOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.IceStunIceRefreshesStun:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.IceKnockbackFrostbiteIncreasesKnockbackForce:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.IceKnockbackSnowEruptionOnKnockback:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.IceKnockbackBonusStacksOnDownedTargets:
                    playerTraitManager.AddOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.EarthMaxHpDamageAtThreshold:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.EarthAmpAllAfflictionsOnThreshhold:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.EarthSunderedEnemiesDealLessDamage:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.EarthRockRingExplosionOnKill:
                    playerTraitManager.AddOnKillEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.EarthTrueDamageAtThreshold:
                    playerTraitManager.AddOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.EarthSunderFurtherReducesResistances:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.EarthIncreasedDamageToLowerArmorTargets:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.EarthAmpDamageOnHealthyTargets:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.EarthHealOnCritAtSunderThreshold:
                    playerTraitManager.AddOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.EarthPhysicalBonusSunderStacksOnThreshold:
                    playerTraitManager.AddOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.EarthPhysicalSunderAmpsDamage:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.EarthPhysicalSunderAmpsCrits:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.EarthBleedBonusCritChanceOnBleedingTarget:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.EarthBleedSunderAddsPercentageOfBleed:
                    playerTraitManager.AddOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.EarthBleedBonusEarthDamageToBleeding:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.EarthBleedBloodExplosionOnBleed:
                    playerTraitManager.AddOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.EarthPoisonAddSunderedOnPoisonTick:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.EarthPoisonSunderToPoisonConversion:
                    playerTraitManager.AddOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.EarthPoisonSunderToPoisonOnCrit:
                    playerTraitManager.AddOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.EarthStunStunOnThreshold:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.EarthStunBonusDamageOnStun:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.EarthStunKillingStunnedWithEarthRefundsCooldowns:
                    playerTraitManager.AddOnKillEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.EarthStunStunningAddsSunder:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.EarthStunSunderAmpsStunDamageLength:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.EarthKnockbackTremorsOnKnockback:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.EarthKnockbackSunderReducesKnockbackResistance:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.EarthKnockbackSummonRocksOnRecentKnockbackTarget:
                    playerTraitManager.AddOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.WindAmpsDamageTaken:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.WindAmpsComboArmorShred:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.WindTargetGainsBleedOnAttack:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.WindSummonAerobladesOnThreshold:
                    playerTraitManager.AddOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.WindWindshearAmpsTrueDamage:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.WindAddMoreStacksOnInitialStack:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.WindMoreDamageOnMaximumStacks:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.WindPhysicalSummonWhirlwindOnSkillHit:
                    playerTraitManager.AddOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.WindPhysicalWindshearAmpsBasicAttacks:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.WindPhysicalCritsDealArmorAsDamage:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.WindBleedAmpBleedAtThreshold:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.WindBleedMoreBleedStacksAThreshold:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.WindBleedBleedGrantsWindCritChance:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.WindBleedAddBleedOnWindCrit:
                    playerTraitManager.AddOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.WindPoisonTransferPoisonStacksOnKill:
                    playerTraitManager.AddOnKillEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.WindPoisonWindAddsPercentageOfPoisonOnHit:
                    playerTraitManager.AddOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.WindPoisonPoisonBurstAtWindThreshold:
                    playerTraitManager.AddOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.WindStunStunDealsTrueDamageAtThreshold:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.WindStunWindblastOnStun:
                    playerTraitManager.AddOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.WindStunStunAmpsWindshearGain:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.WindKnockbackKnockbackSummonsMiniCyclone:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.WindKnockbackLoseKnockbackResistanceOnThreshold:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.WindKnockbackWindshearDoesDamageIfKnockedBack:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.PhysicalPhysicalAmpsCritChance:
                    playerTraitManager.AddOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.PhysicalPhysicalSkillsComboAmp:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.PhysicalLifestealAmp:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.PhysicalSkillAmpArmorOnKill:
                    playerTraitManager.AddOnKillEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.PhysicalAmpDamageBelowHalfHp:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.PhysicalBleedBleedAmpsPhysicalDamage:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.PhysicalBleedPhysicalSkillsAddBleed:
                    playerTraitManager.AddOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.PhysicalBleedSkillsDoTrueDamageAtThreshold:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.PhysicalPoisonPhysicalAmpsPoisonDamage:
                    playerTraitManager.AddOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.PhysicalPoisonPlayerMaxHpDamageOnThreshold:
                    playerTraitManager.AddOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.PhysicalPoisonPoisonAmpsPhysicalDamage:
                    playerTraitManager.AddOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.PhysicalStunAmpDamageOnStunned:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.PhysicalStunBladeRiftOnStun:
                    playerTraitManager.AddOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.PhysicalKnockbackKnockbackKillAmpsPhysicalDamage:
                    playerTraitManager.AddOnKillEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.PhysicalKnockbackPhysicalAttacksGainInnateKnockback:
                    playerTraitManager.AddOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.PhysicalKnockbackSummonKnivesOnKnockbackHit:
                    playerTraitManager.AddOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.BleedReducesResistances:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.BleedAmpsCritHitsAddsBleedToNearby:
                    playerTraitManager.AddOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.BleedSlowsTargets:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.BleedAmpDamageTakenOnAttack:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.BleedHealOnBleedingEnemyKill:
                    playerTraitManager.AddOnKillEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.BleedCritsConsumeBleed:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.BleedAmpDamageAtThreshold:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.BleedBloodWellAtThreshold:
                    playerTraitManager.AddOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.BleedExpungeBleedAtThreshold:
                    playerTraitManager.AddOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.BleedPoisonReduceOtherResistances:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.BleedPoisonChanceOfPoisonCloudOnBleed:
                    playerTraitManager.AddOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.BleedStunStunReducesBleedResistance:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.BleedStunStunAtThresholdBelowHalfHP:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.BleedStunStunAddsBleed:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.BleedKnockbackBonusBleed:
                    playerTraitManager.AddOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.BleedKnockbackKnockbackExposionOfBlood:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.BleedKnockbackKnockbackAmpsDamage:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.PoisonSpreadStacksOnDeath:
                    playerTraitManager.AddOnKillEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.PoisonAmpsLifesteal:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.PoisonAmpNextAttackAfterPoisonKill:
                    playerTraitManager.AddOnKillEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.PoisonAmpDamageOnFirstStack:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.PoisonPrimaryTraitsAmpPoison:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.PoisonShredArmorOnThreshold:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.PoisonEnemiesAmpPoisonOnKill:
                    playerTraitManager.AddOnKillEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.PoisonTrueDamageAtThreshold:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.PoisonVamperism:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.PoisonStunPoisonReducesStunResist:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.PoisonStunPoisonSpreadOnThreshold:
                    playerTraitManager.AddOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.PoisonKnockbackPoisonReducesKnockbackResistance:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.PoisonKnockbackConsumePoisonStacksForTrueDamage:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.StunReducesArmor:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.StunAmpsDamageTaken:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.StunAmpsAfflictionGain:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.StunAmpDuration:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.StunShockwaveOnStunningStunned:
                    playerTraitManager.AddOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.StunOnStunDealsAdditionalBaseDamage:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.StunAmpDurationOnThreshold:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.StunAmpCritDamage:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.StunAmpsMovespeed:
                    playerTraitManager.AddOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.StunKnockbackStunReduceResistance:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.KnockbackReducesSpellCooldowns:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.KnockBackAmpsBasicAttacks:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.KnockbackAmpKnockbackForce:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.KnockbackAmpsDamageTaken:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.KnockbackAmpsArmor:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.KnockbackDoesBonusDamageOnKnockbacked:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.KnockbackReducesResistances:
                    playerTraitManager.AddIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                default:
                    break;
            }
        }
            if (compelteStatSetup)
                StatSetup(false, true);
            if (statCompare)
                StatSetup(false, false);
    }

    //Remvoes the item stats from our current Stats
    public void RemoveItemStats(Item item, bool completeStatSetup, bool statCompare)
    {
        //Debug.Log("removing stats");
        if (item.itemType == Item.ItemType.Weapon || item.itemType == Item.ItemType.TwoHandWeapon)
        {

            weaponBaseAttacksPerSecond.Remove(item.attacksPerSecond);

            float totalWeaponAttackSpeeds = 0f;
            if (weaponBaseAttacksPerSecond.Count > 0)
            {
                foreach (float value in weaponBaseAttacksPerSecond)
                    totalWeaponAttackSpeeds += value;
                if (weaponBaseAttacksPerSecond.Count > 2)
                    totalWeaponAttackSpeeds -= 1;
                weaponAttacksPerSecond = totalWeaponAttackSpeeds;
            }
            else
            {
                weaponAttacksPerSecond = 1;
            }

            weaponsToHitWith.Remove(item);
            UpdateWeaponsToHitWith();
        }

        foreach (ItemTrait trait in item.itemTraits)
        {
            switch (trait.traitType)
            {
                case ItemTrait.TraitType.HealthFlat:
                    bonusHealth -= trait.traitBonus * trait.traitBonusMultiplier;
                    break;
                case ItemTrait.TraitType.HealthPercent:
                    bonusPercentHealth -= trait.traitBonus * trait.traitBonusMultiplier;
                    break;
                case ItemTrait.TraitType.Armor:
                    armor -= trait.traitBonus * trait.traitBonusMultiplier;
                    break;
                case ItemTrait.TraitType.HealthRegen:
                    bonusHealthRegen -= trait.traitBonus * trait.traitBonusMultiplier;
                    break;
                case ItemTrait.TraitType.HealingOnHit:
                    playerTraitManager.RemoveOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.HealingOnKill:
                    playerTraitManager.RemoveOnKillEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.CooldownReduction:
                    cooldownReductionSources.Remove(trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.AttackSpeed:
                    bonusAttackSpeed -= trait.traitBonus * trait.traitBonusMultiplier;
                    break;
                case ItemTrait.TraitType.MoveSpeed:
                    movespeedPercentMultiplier -= trait.traitBonus * trait.traitBonusMultiplier;
                    break;
                case ItemTrait.TraitType.CritChance:
                    critChance -= trait.traitBonus * trait.traitBonusMultiplier;
                    break;
                case ItemTrait.TraitType.CritDamage:
                    critDamageMultiplier -= trait.traitBonus * trait.traitBonusMultiplier;
                    break;
                case ItemTrait.TraitType.Jumps:
                    jumps -= (int) trait.traitBonus * trait.traitBonusMultiplier;
                    break;
                case ItemTrait.TraitType.AflameResistance:
                    aflameResistance -= trait.traitBonus * trait.traitBonusMultiplier;
                    break;
                case ItemTrait.TraitType.FrostbiteResistance:
                    frostbiteResistance -= trait.traitBonus * trait.traitBonusMultiplier;
                    break;
                case ItemTrait.TraitType.SunderResistance:
                    sunderResistance -= trait.traitBonus * trait.traitBonusMultiplier;
                    break;
                case ItemTrait.TraitType.WindshearResistance:
                    windshearResistance -= trait.traitBonus * trait.traitBonusMultiplier;
                    break;
                case ItemTrait.TraitType.OverchargeResistance:
                    overchargeResistance -= trait.traitBonus * trait.traitBonusMultiplier;
                    break;
                case ItemTrait.TraitType.OvergrowthResistance:
                    overgrowthResistance -= trait.traitBonus * trait.traitBonusMultiplier;
                    break;
                case ItemTrait.TraitType.BleedResistance:
                    bleedResistance -= trait.traitBonus * trait.traitBonusMultiplier;
                    break;
                case ItemTrait.TraitType.PoisonResistance:
                    poisonResistance -= trait.traitBonus * trait.traitBonusMultiplier;
                    break;
                case ItemTrait.TraitType.StunResistance:
                    stunResistance -= trait.traitBonus * trait.traitBonusMultiplier;
                    break;
                case ItemTrait.TraitType.AsleepResistance:
                    sleepResistance -= trait.traitBonus * trait.traitBonusMultiplier;
                    break;
                case ItemTrait.TraitType.KnockbackResistance:
                    knockbackResistance -= trait.traitBonus * trait.traitBonusMultiplier;
                    break;
                case ItemTrait.TraitType.FlatDamageReduction:
                    flatDamageReduction -= trait.traitBonus * trait.traitBonusMultiplier;
                    break;
                case ItemTrait.TraitType.FireExplosionOnKill:
                    playerTraitManager.RemoveOnKillEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.MoreAflameStacksOnHitThreshold:
                    playerTraitManager.RemoveOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.BurnDoesMaxHpDamageAtThreshold:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.BasicAttacksShredArmorOnAflame:
                    playerTraitManager.RemoveOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.FlameVamperism:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.RingOfFireOnHit:
                    playerTraitManager.RemoveOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.AflameToSunderStackOnEarthSpell:
                    playerTraitManager.RemoveOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.SunderFurtherDecreasesFireResist:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.AflameSunderCritsSummonFireballs:
                    playerTraitManager.RemoveOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.AflameWindshearWindAttacksGainCritOnBurningTarget:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.AflameWindshearSummonFirePillarsOnHit:
                    playerTraitManager.RemoveOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.AflameWindshearWindSpellsAddFireStacks:
                    playerTraitManager.RemoveOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.AflamePhysicalAddFireStacksOnHit:
                    playerTraitManager.RemoveOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.AflamePhysicalDamageAmpOnBurningTarget:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.AflamePhysicalBladeExplosionOnKill:
                    playerTraitManager.RemoveOnKillEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.AflamePhysicalBigHitsAddAflame:
                    playerTraitManager.RemoveOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.AflameBleedIncreasesFlameCritChance:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.AflameBleedFireDamageAmpOnBleedThreshold:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.AflameBleedAflameAddsBleedAtThreshhold:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.AflameBleedAflameRemovesBleedResist:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.AflameBleedDamageAmpOnDoubleThreshhold:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.AflamePoisonBurningEnemySpreadPoisonStacksOnDeath:
                    playerTraitManager.RemoveOnKillEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.AflamePoisonGreviousWoundsOnStackThreshold:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.AflamePoisonPoisonReducesFireResist:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.AflamePoisonFireSpellsSummonsPoisonBurst:
                    playerTraitManager.RemoveOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.AflamePoisonFireAmpsPoison:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.AflamePoisonPoisonCloudOnFireKill:
                    playerTraitManager.RemoveOnKillEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.AflameStunPeriodBurnStun:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.AflameStunStunOnThreshold:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.AflameStunStunReducesFireResistance:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.AflameStunStunAmpsBurnDamage:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.AflameKnockbackAflameReducesKnockbackResist:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.AflameKnockbackAflameSpellsOnKnockbackedTargetExplode:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.AflameKnockbackKnockbackAmpsFireDamage:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.IceFreezeAtStackThreshold:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.IceAmpAllDamageAtThreshold:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.IceBasicAttacksConsumeStacksAtThreshold:
                    playerTraitManager.RemoveOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.IceEnemyAttacksWeakendAtThreshold:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.IceEnemiesGainFrostbiteOnStrikingYou:
                    playerTraitManager.RemoveOnStruckEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.IceAddStacksToNearbyEnemies:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    if(playerTraitManager.CheckForIdleEffectValue(trait.traitType) <= 0)
                        arcticAuraEnabled = false;
                    break;
                case ItemTrait.TraitType.IceAmpFrostbiteDamage:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.IceEarthFrostToEarthBonusDamage:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.IceEarthSunderAmpsIceDamage:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.IceEarthIceDOTAtThreshold:
                    playerTraitManager.RemoveOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.IceEarthEarthSpellBonusCritDamage:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.IceWindWindAmpsFrostbiteDamage:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.IceWindWindSpellsDamageAmp:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.IceWindIncreaseArmorShredPerFrostbite:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.IceWindSummonTornadoOnHit:
                    playerTraitManager.RemoveOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.IcePhysicalFrostbiteAmpsPhysicalCritDamage:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.IcePhysicalPhysicalVampOnFrostbite:
                    playerTraitManager.RemoveOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.IcePhysicalBladeVortexOnHit:
                    playerTraitManager.RemoveOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.IceBleedFrostbiteAmpsBleed:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.IceBleedBleedDoesDamageInstantlyOnThreshold:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.IcePoisonFreezingPoison:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.IcePoisonFrostbiteResetsPoisonAndAmps:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.IcePoisonSummonPoisonPillarOnThreshold:
                    playerTraitManager.RemoveOnKillEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.IceStunRudeAwakening:
                    playerTraitManager.RemoveOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.IceStunIceRefreshesStun:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.IceKnockbackFrostbiteIncreasesKnockbackForce:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.IceKnockbackSnowEruptionOnKnockback:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.IceKnockbackBonusStacksOnDownedTargets:
                    playerTraitManager.RemoveOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.EarthMaxHpDamageAtThreshold:
                    playerTraitManager.RemoveOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.EarthAmpAllAfflictionsOnThreshhold:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.EarthSunderedEnemiesDealLessDamage:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.EarthRockRingExplosionOnKill:
                    playerTraitManager.RemoveOnKillEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.EarthTrueDamageAtThreshold:
                    playerTraitManager.RemoveOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.EarthSunderFurtherReducesResistances:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.EarthIncreasedDamageToLowerArmorTargets:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.EarthAmpDamageOnHealthyTargets:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.EarthHealOnCritAtSunderThreshold:
                    playerTraitManager.RemoveOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.EarthPhysicalBonusSunderStacksOnThreshold:
                    playerTraitManager.RemoveOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.EarthPhysicalSunderAmpsDamage:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.EarthPhysicalSunderAmpsCrits:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.EarthBleedBonusCritChanceOnBleedingTarget:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.EarthBleedSunderAddsPercentageOfBleed:
                    playerTraitManager.RemoveOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.EarthBleedBonusEarthDamageToBleeding:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.EarthBleedBloodExplosionOnBleed:
                    playerTraitManager.RemoveOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.EarthPoisonAddSunderedOnPoisonTick:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.EarthPoisonSummonPillarOnThreshold:
                    playerTraitManager.RemoveOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.EarthPoisonSunderToPoisonConversion:
                    playerTraitManager.RemoveOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.EarthPoisonSunderToPoisonOnCrit:
                    playerTraitManager.RemoveOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.EarthStunStunOnThreshold:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.EarthStunBonusDamageOnStun:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.EarthStunKillingStunnedWithEarthRefundsCooldowns:
                    playerTraitManager.RemoveOnKillEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.EarthStunStunningAddsSunder:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.EarthStunSunderAmpsStunDamageLength:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.EarthKnockbackTremorsOnKnockback:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.EarthKnockbackSunderReducesKnockbackResistance:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.EarthKnockbackSummonRocksOnRecentKnockbackTarget:
                    playerTraitManager.RemoveOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.WindAmpsDamageTaken:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.WindAmpsComboArmorShred:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.WindTargetGainsBleedOnAttack:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.WindSummonAerobladesOnThreshold:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.WindWindshearAmpsTrueDamage:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.WindAddMoreStacksOnInitialStack:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.WindMoreDamageOnMaximumStacks:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.WindPhysicalSummonWhirlwindOnSkillHit:
                    playerTraitManager.RemoveOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.WindPhysicalWindshearAmpsBasicAttacks:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.WindPhysicalCritsDealArmorAsDamage:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.WindBleedAmpBleedAtThreshold:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.WindBleedMoreBleedStacksAThreshold:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.WindBleedBleedGrantsWindCritChance:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.WindBleedAddBleedOnWindCrit:
                    playerTraitManager.RemoveOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.WindPoisonTransferPoisonStacksOnKill:
                    playerTraitManager.RemoveOnKillEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.WindPoisonWindAddsPercentageOfPoisonOnHit:
                    playerTraitManager.RemoveOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.WindPoisonPoisonBurstAtWindThreshold:
                    playerTraitManager.RemoveOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.WindStunStunDealsTrueDamageAtThreshold:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.WindStunWindblastOnStun:
                    playerTraitManager.RemoveOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.WindStunStunAmpsWindshearGain:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.WindKnockbackKnockbackSummonsMiniCyclone:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.WindKnockbackLoseKnockbackResistanceOnThreshold:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.WindKnockbackWindshearDoesDamageIfKnockedBack:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.PhysicalPhysicalAmpsCritChance:
                    playerTraitManager.RemoveOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.PhysicalPhysicalSkillsComboAmp:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.PhysicalLifestealAmp:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.PhysicalSkillAmpArmorOnKill:
                    playerTraitManager.RemoveOnKillEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.PhysicalAmpDamageBelowHalfHp:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.PhysicalBleedBleedAmpsPhysicalDamage:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.PhysicalBleedPhysicalSkillsAddBleed:
                    playerTraitManager.RemoveOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.PhysicalBleedSkillsDoTrueDamageAtThreshold:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.PhysicalPoisonPhysicalAmpsPoisonDamage:
                    playerTraitManager.RemoveOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.PhysicalPoisonPlayerMaxHpDamageOnThreshold:
                    playerTraitManager.RemoveOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.PhysicalPoisonPoisonAmpsPhysicalDamage:
                    playerTraitManager.RemoveOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.PhysicalStunAmpDamageOnStunned:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.PhysicalStunBladeRiftOnStun:
                    playerTraitManager.RemoveOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.PhysicalKnockbackKnockbackKillAmpsPhysicalDamage:
                    playerTraitManager.RemoveOnKillEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.PhysicalKnockbackPhysicalAttacksGainInnateKnockback:
                    playerTraitManager.RemoveOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.PhysicalKnockbackSummonKnivesOnKnockbackHit:
                    playerTraitManager.RemoveOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.BleedReducesResistances:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.BleedAmpsCritHitsAddsBleedToNearby:
                    playerTraitManager.RemoveOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.BleedSlowsTargets:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.BleedAmpDamageTakenOnAttack:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.BleedHealOnBleedingEnemyKill:
                    playerTraitManager.RemoveOnKillEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.BleedCritsConsumeBleed:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.BleedAmpDamageAtThreshold:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.BleedBloodWellAtThreshold:
                    playerTraitManager.RemoveOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.BleedExpungeBleedAtThreshold:
                    playerTraitManager.RemoveOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.BleedPoisonReduceOtherResistances:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.BleedPoisonChanceOfPoisonCloudOnBleed:
                    playerTraitManager.RemoveOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.BleedStunStunReducesBleedResistance:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.BleedStunStunAtThresholdBelowHalfHP:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.BleedStunStunAddsBleed:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.BleedKnockbackBonusBleed:
                    playerTraitManager.RemoveOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.BleedKnockbackKnockbackExposionOfBlood:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.BleedKnockbackKnockbackAmpsDamage:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.PoisonSpreadStacksOnDeath:
                    playerTraitManager.RemoveOnKillEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.PoisonAmpsLifesteal:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.PoisonAmpNextAttackAfterPoisonKill:
                    playerTraitManager.RemoveOnKillEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.PoisonAmpDamageOnFirstStack:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.PoisonPrimaryTraitsAmpPoison:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.PoisonShredArmorOnThreshold:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.PoisonEnemiesAmpPoisonOnKill:
                    playerTraitManager.RemoveOnKillEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.PoisonTrueDamageAtThreshold:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.PoisonVamperism:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.PoisonStunPoisonReducesStunResist:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.PoisonStunPoisonSpreadOnThreshold:
                    playerTraitManager.RemoveOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.PoisonKnockbackPoisonReducesKnockbackResistance:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.PoisonKnockbackConsumePoisonStacksForTrueDamage:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.StunReducesArmor:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.StunAmpsDamageTaken:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.StunAmpsAfflictionGain:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.StunAmpDuration:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.StunShockwaveOnStunningStunned:
                    playerTraitManager.RemoveOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.StunOnStunDealsAdditionalBaseDamage:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.StunAmpDurationOnThreshold:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.StunAmpCritDamage:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.StunAmpsMovespeed:
                    playerTraitManager.RemoveOnHitEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.StunKnockbackStunReduceResistance:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.KnockbackReducesSpellCooldowns:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.KnockBackAmpsBasicAttacks:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.KnockbackAmpKnockbackForce:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.KnockbackAmpsDamageTaken:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.KnockbackAmpsArmor:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.KnockbackDoesBonusDamageOnKnockbacked:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                case ItemTrait.TraitType.KnockbackReducesResistances:
                    playerTraitManager.RemoveIdleEffect(trait.traitType, trait.traitBonus * trait.traitBonusMultiplier);
                    break;
                default:
                    break;
            }
        }
        if (completeStatSetup)
            StatSetup(false, true);
        if (statCompare)
            StatSetup(false, false);
    }

    // USed to check what the stats would be if we removed the stats of the other item in that slot and replaced it with this item's
    public void CheckStatChange(Item itemToAdd, Item[] itemsToRemove)
    {
        //Debug.Log("Check stat change");
        // Remove the stats, add in the new ones, then we ship off these new stats to be used as the comparison.
        if (itemsToRemove.Length > 0)
            foreach (Item item in itemsToRemove)
                if(item != null)
                    RemoveItemStats(item, false, true);
        if(itemToAdd != false)
            AddItemStats(itemToAdd, false, true);

        //Debug.Log("created new stats here");
        //Debug.Log("weapons to hit with coutn is: " + weaponsToHitWith.Count);
        myStats.AssignPotentialStats(this);

        // Remove the new stats, add in the old ones.
        if (itemsToRemove.Length > 0)
            foreach (Item item in itemsToRemove)
                if (item != null)
                    AddItemStats(item, false, true);
        if (itemToAdd != false)
            RemoveItemStats(itemToAdd, false, true);

        myStats.mouseWithItemHovered = true;
        //Debug.Log("Launching comapre method in stat updater");
        myStats.CompareStatValues(this);
    }

    // Used wehn we want to force a stat value reset for when we are no logner hovering with an item.
    public void ForceStatRecheck()
    {
        Debug.Log(" we have forced a stat recheck");
        myStats.mouseWithItemHovered = false;
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
            GetComponent<PlayerGearManager>().AddMaterialOverride(PlayerGearManager.MaterialOverrideCode.Invisible);
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
                GetComponent<PlayerGearManager>().ResetToOriginalMaterial();
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

    // USed to add a source of ephemeral
    public void AddEphemeralSource(float amount)
    {
        if( amount > 0)
        {
            ephemeral = true;
            ephemeralCount += amount;
        }
        else if (amount < 0)
        {
            ephemeralCount += amount;
            if (ephemeralCount <= 0)
                ephemeral = false;
        }
    }

    // USed to updates the hitboxes fopr our basic attack weapons.
    public void UpdateWeaponsToHitWith()
    {
        switch (weaponsToHitWith.Count)
        {
            case 0:
                basicAttack2.gameObject.SetActive(false);
                basicAttack1.damage = baseDamage;
                basicAttack1.damageType = HitBox.DamageType.Physical;
                break;
            case 1:
                basicAttack2.gameObject.SetActive(false);
                basicAttack1.damage = weaponsToHitWith[0].baseDamageScaling * baseDamage;
                basicAttack1.stacksToAdd = weaponsToHitWith[0].stacksToAddOnHit;
                basicAttack1.damageType = weaponsToHitWith[0].damageType;
                break;
            case 2:
                basicAttack2.gameObject.SetActive(true);
                basicAttack1.damage = weaponsToHitWith[0].baseDamageScaling * baseDamage;
                basicAttack1.stacksToAdd = weaponsToHitWith[0].stacksToAddOnHit;
                basicAttack1.damageType = weaponsToHitWith[0].damageType;
                basicAttack2.damage = weaponsToHitWith[1].baseDamageScaling * baseDamage;
                basicAttack2.stacksToAdd = weaponsToHitWith[1].stacksToAddOnHit;
                basicAttack2.damageType = weaponsToHitWith[1].damageType;
                break;
            default:
                break;
        }
    }

    // Used when we lose armor. If we lose more armor then oyur max we start taking increased damage.
    public void ChangeArmor(float percentageToChangeBy)
    {
        if (percentageToChangeBy < 0)
        {
            // negative case, we are removing armor.
            if (armorReductionMultiplier > 0)
            {
                // case we can put all the percentahe in the armor shred.
                if (armorReductionMultiplier + percentageToChangeBy > 0)
                    armorReductionMultiplier += percentageToChangeBy;
                else
                {
                    // case we put as much armor reduction as we cannand have reached zero so now we are adding to the increased damage percentage.
                    percentageToChangeBy += armorReductionMultiplier;
                    armorReductionMultiplier = 0;
                    armorShreddedBonusDamage -= percentageToChangeBy;
                }
            }
            // case armor is already shredded, put the percventage in increased damage.
            else
                armorShreddedBonusDamage -= percentageToChangeBy;
        }
        else
        {
            // case the value is positive.
            if(armorShreddedBonusDamage - percentageToChangeBy > 0)
            {
                // case we are fully shredded and are taking bonus damage, and the value doesnt fully remove it.
                armorShreddedBonusDamage -= percentageToChangeBy;
            }
            else
            {
                if(armorShreddedBonusDamage == 0)
                {
                    // case ther is no bonus shredde damage so we can add our value directly to the armor.
                    armorReductionMultiplier += percentageToChangeBy;
                }
                else
                {
                    percentageToChangeBy -= armorShreddedBonusDamage;
                    armorShreddedBonusDamage = 0;
                    armorReductionMultiplier += percentageToChangeBy;
                }
            }
        }
    }
}
