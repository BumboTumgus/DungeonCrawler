using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName;
    public string itemNameAffinitySuffix;
    public string itemNameModifiersPrefix;
    public string description;
    public int itemGearID;

    public int itemtorsoID;
    public int itemUpperRightArmID;
    public int itemUpperLeftArmID;
    public int itemLowerRightArmID;
    public int itemLowerLeftArmID;
    public int itemRightHandID;
    public int itemLeftHandID;
    public int itemRightElbowID;
    public int itemLeftElbowID;
    public int itemRightShoulderID;
    public int itemLeftShoulderID;

    public bool itemPickUpAllowed = true;

    public HitBox.DamageType damageType;
    public int stacksToAddOnHit = 0;

    public enum HelmetType { NoFeatures, NoHair, AllFeatures };
    public HelmetType helmetType;

    [HideInInspector] public bool equippedToRightHand = false;
    public bool stackable;
    public bool instantPickup;
    [HideInInspector] public GameObject previousOwner = null;
    public int currentStack;
    public int maxStack;

    public Sprite artwork;

    public int inventoryIndex = 0;

    public int goldValue;

    public enum ItemType { TrinketRing, Weapon, TwoHandWeapon, Helmet, Legs, Armor, Shield, MagicBooster, Skill, TrinketCape, TrinketBracelet, TrinketWaistItem };
    public ItemType itemType;
    public SkillsManager.SkillNames skillName;

    public enum ItemRarity { Common, Uncommon, Rare, Legendary, Masterwork }
    public ItemRarity itemRarity = ItemRarity.Common;

    public enum StartingStatValue { None, Low, Medium, High, VeryHigh, Ultra, LowNeg, MedNeg, HighNeg, VeryHighNeg, UltraNeg };
    [SerializeField] private StartingStatValue baseItemArmor = StartingStatValue.None;
    [SerializeField] private StartingStatValue baseItemHealth = StartingStatValue.None;
    [SerializeField] private StartingStatValue baseItemFlatDamageReduction = StartingStatValue.None;
    [SerializeField] private StartingStatValue baseItemMovespeed = StartingStatValue.None;

    public float attacksPerSecond;
    public float baseDamageScaling;

    public int itemMoveset = 0;

    public List<ItemTrait> itemTraits = new List<ItemTrait>();
    public List<ItemTrait> baseTraits = new List<ItemTrait>();
    public ItemTrait.TraitType[] garenteedTraits;
    public float[] garenteedTraitValues;
    public int[] garenteedTraitMultipliers;

    public enum AffinityType { None, Fire, Ice, Earth, Wind, Physical, Bleed, Poison, Stun, Knockback }
    public AffinityType affinityPrimary;
    public AffinityType affinitySecondary;
    public AffinityType affinityTertiary;

    public bool affinitySecondaryMultiElement = false;
    public bool affinityTertiaryMultiElement = false;

    public enum ModifierType { None, Devastating, Dull, Hardened, Cracked, Nimble, Sluggish, Vamperic, Cursed, Magic, Mundane, Lucky, Unfavoured, Strong, Brittle, Illustrious, Rusty, Brawny, Meager, Resistant, Vulnerable }
    public List<ModifierType> itemModifiers = new List<ModifierType>();

    private readonly int[] VALUE_RANGES_ARMOR = { 5,20  ,20,40  ,40,80  ,80,160  ,160,320};
    private readonly int[] VALUE_RANGE_HEALTH = { 10,50  ,50,100  ,100,200  ,200,400  ,400,800};
    private readonly int[] VALUE_RANGE_FLAT_DAMAGE_REDUCTION = { 1,3,  2,5,  5,10, 10,20, 20,50};
    private readonly float[] VALUE_RANGE_MOVESPEED = { 0.01f,0.04f  ,0.04f,0.08f  ,0.08f,0.12f  ,0.12f,0.16f  ,0.16f,0.20f};


    private void Start()
    {
        AddBaseItemTraits();
    }

    public void AddBaseItemTraits()
    {
        Debug.Log("Im rolling the base stats of the item");
        for (int index = 0; index < garenteedTraits.Length; index++)
        {
            AddTrait(garenteedTraits[index], garenteedTraitValues[index], garenteedTraitMultipliers[index]);
        }
        switch (baseItemArmor)
        {
            case StartingStatValue.Low:
                AddTrait(ItemTrait.TraitType.Armor, Random.Range(VALUE_RANGES_ARMOR[0], VALUE_RANGES_ARMOR[1] + 1), 1);
                break;
            case StartingStatValue.Medium:
                AddTrait(ItemTrait.TraitType.Armor, Random.Range(VALUE_RANGES_ARMOR[2], VALUE_RANGES_ARMOR[3] + 1), 1);
                break;
            case StartingStatValue.High:
                AddTrait(ItemTrait.TraitType.Armor, Random.Range(VALUE_RANGES_ARMOR[4], VALUE_RANGES_ARMOR[5] + 1), 1);
                break;
            case StartingStatValue.VeryHigh:
                AddTrait(ItemTrait.TraitType.Armor, Random.Range(VALUE_RANGES_ARMOR[6], VALUE_RANGES_ARMOR[7] + 1), 1);
                break;
            case StartingStatValue.Ultra:
                AddTrait(ItemTrait.TraitType.Armor, Random.Range(VALUE_RANGES_ARMOR[8], VALUE_RANGES_ARMOR[9] + 1), 1);
                break;
            default:
                break;
        }
        switch (baseItemHealth)
        {
            case StartingStatValue.Low:
                AddTrait(ItemTrait.TraitType.HealthFlat, Random.Range(VALUE_RANGE_HEALTH[0], VALUE_RANGE_HEALTH[1] + 1), 1);
                break;
            case StartingStatValue.Medium:
                AddTrait(ItemTrait.TraitType.HealthFlat, Random.Range(VALUE_RANGE_HEALTH[2], VALUE_RANGE_HEALTH[3] + 1), 1);
                break;
            case StartingStatValue.High:
                AddTrait(ItemTrait.TraitType.HealthFlat, Random.Range(VALUE_RANGE_HEALTH[4], VALUE_RANGE_HEALTH[5] + 1), 1);
                break;
            case StartingStatValue.VeryHigh:
                AddTrait(ItemTrait.TraitType.HealthFlat, Random.Range(VALUE_RANGE_HEALTH[6], VALUE_RANGE_HEALTH[7] + 1), 1);
                break;
            case StartingStatValue.Ultra:
                AddTrait(ItemTrait.TraitType.HealthFlat, Random.Range(VALUE_RANGE_HEALTH[8], VALUE_RANGE_HEALTH[9] + 1), 1);
                break;
            default:
                break;
        }
        switch (baseItemFlatDamageReduction)
        {
            case StartingStatValue.Low:
                AddTrait(ItemTrait.TraitType.FlatDamageReduction, Random.Range(VALUE_RANGE_FLAT_DAMAGE_REDUCTION[0], VALUE_RANGE_FLAT_DAMAGE_REDUCTION[1] + 1), 1);
                break;
            case StartingStatValue.Medium:
                AddTrait(ItemTrait.TraitType.FlatDamageReduction, Random.Range(VALUE_RANGE_FLAT_DAMAGE_REDUCTION[2], VALUE_RANGE_FLAT_DAMAGE_REDUCTION[3] + 1), 1);
                break;
            case StartingStatValue.High:
                AddTrait(ItemTrait.TraitType.FlatDamageReduction, Random.Range(VALUE_RANGE_FLAT_DAMAGE_REDUCTION[4], VALUE_RANGE_FLAT_DAMAGE_REDUCTION[5] + 1), 1);
                break;
            case StartingStatValue.VeryHigh:
                AddTrait(ItemTrait.TraitType.FlatDamageReduction, Random.Range(VALUE_RANGE_FLAT_DAMAGE_REDUCTION[6], VALUE_RANGE_FLAT_DAMAGE_REDUCTION[7] + 1), 1);
                break;
            case StartingStatValue.Ultra:
                AddTrait(ItemTrait.TraitType.FlatDamageReduction, Random.Range(VALUE_RANGE_FLAT_DAMAGE_REDUCTION[8], VALUE_RANGE_FLAT_DAMAGE_REDUCTION[9] + 1), 1);
                break;
            default:
                break;
        }
        switch (baseItemMovespeed)
        {
            case StartingStatValue.Low:
                AddTrait(ItemTrait.TraitType.MoveSpeed, Random.Range(VALUE_RANGE_MOVESPEED[0], VALUE_RANGE_MOVESPEED[1]), 1);
                break;
            case StartingStatValue.Medium:
                AddTrait(ItemTrait.TraitType.MoveSpeed, Random.Range(VALUE_RANGE_MOVESPEED[2], VALUE_RANGE_MOVESPEED[3]), 1);
                break;
            case StartingStatValue.High:
                AddTrait(ItemTrait.TraitType.MoveSpeed, Random.Range(VALUE_RANGE_MOVESPEED[4], VALUE_RANGE_MOVESPEED[5]), 1);
                break;
            case StartingStatValue.VeryHigh:
                AddTrait(ItemTrait.TraitType.MoveSpeed, Random.Range(VALUE_RANGE_MOVESPEED[6], VALUE_RANGE_MOVESPEED[7]), 1);
                break;
            case StartingStatValue.Ultra:
                AddTrait(ItemTrait.TraitType.MoveSpeed, Random.Range(VALUE_RANGE_MOVESPEED[8], VALUE_RANGE_MOVESPEED[9]), 1);
                break;
            case StartingStatValue.LowNeg:
                AddTrait(ItemTrait.TraitType.MoveSpeed, Random.Range(VALUE_RANGE_MOVESPEED[0], VALUE_RANGE_MOVESPEED[1]) * -1, 1);
                break;
            case StartingStatValue.MedNeg:
                AddTrait(ItemTrait.TraitType.MoveSpeed, Random.Range(VALUE_RANGE_MOVESPEED[2], VALUE_RANGE_MOVESPEED[3]) * -1, 1);
                break;
            case StartingStatValue.HighNeg:
                AddTrait(ItemTrait.TraitType.MoveSpeed, Random.Range(VALUE_RANGE_MOVESPEED[4], VALUE_RANGE_MOVESPEED[5]) * -1, 1);
                break;
            case StartingStatValue.VeryHighNeg:
                AddTrait(ItemTrait.TraitType.MoveSpeed, Random.Range(VALUE_RANGE_MOVESPEED[6], VALUE_RANGE_MOVESPEED[7]) * -1, 1);
                break;
            case StartingStatValue.UltraNeg:
                AddTrait(ItemTrait.TraitType.MoveSpeed, Random.Range(VALUE_RANGE_MOVESPEED[8], VALUE_RANGE_MOVESPEED[9]) * -1, 1);
                break;
            default:
                break;
        }
    }

    // This is called when the item can be picked up. The item will set it's parent as the player's ivnentory, hide itself and disable collisions.
    public void ComfirmPickup(Transform targetParent, int index)
    {
        // GetComponent<MeshRenderer>().enabled = false;
        //Debug.Log("We are picking it up now");
        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer renderer in meshRenderers)
            renderer.enabled = false;

        ParticleSystem[] particles = GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem system in particles)
        {
            system.Stop();
            system.Clear();
        }

        if (GetComponentInChildren<Light>())
            GetComponentInChildren<Light>().enabled = false;

        GetComponent<SphereCollider>().enabled = false;
        transform.SetParent(targetParent);
        transform.localPosition = Vector3.zero;
        inventoryIndex = index;
    }

    // USed to drop this item back on the ground.
    public void ComfirmDrop()
    {
        transform.SetParent(null);
        // GetComponent<MeshRenderer>().enabled = true;

        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer renderer in meshRenderers)
            renderer.enabled = true;

        ParticleSystem[] particles = GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem system in particles)
            system.Play();

        GetComponentInChildren<Light>().enabled = true;

        //GetComponent<SphereCollider>().enabled = true;
        //Debug.Log("we should be hucking this boi in");
        transform.rotation = Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0));
        ItemPopIn(transform.position + transform.forward * Random.Range(0.5f, 2f));
    }

    // Used to make the item pop into the air and land at a specific spot.
    public void ItemPopIn(Vector3 targetPosition)
    {
        GetComponent<SphereCollider>().enabled = false;
        itemPickUpAllowed = false;
        StartCoroutine(PopIn(targetPosition));
    }

    IEnumerator PopIn(Vector3 targetPosition)
    {
        //Debug.Log("item poppin in");
        float currentTimer = 0;
        float targetTimer = 0.5f;
        float yOriginal = transform.position.y;
        float yMultiplier = 1f;
        Vector3 originalPosition = transform.position;

        while (currentTimer < targetTimer / 2)
        {
            currentTimer += Time.deltaTime;
            float p = currentTimer / targetTimer;
            float newY = transform.position.y + (10f * Time.deltaTime * yMultiplier);

            yMultiplier *= 0.9f;

            Vector3 newPos = new Vector3(Mathf.Lerp(originalPosition.x, targetPosition.x, p), 0, Mathf.Lerp(originalPosition.z, targetPosition.z, p));
            transform.position = new Vector3(newPos.x, newY, newPos.z);
            yield return null;
        }
        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;
            float p = currentTimer / targetTimer;
            float newY = transform.position.y - (10f * Time.deltaTime * yMultiplier);

            yMultiplier *= 1.1f;

            Vector3 newPos = new Vector3(Mathf.Lerp(originalPosition.x, targetPosition.x, p), 0, Mathf.Lerp(originalPosition.z, targetPosition.z, p));
            transform.position = new Vector3(newPos.x, newY, newPos.z);
            yield return null;
        }

        transform.position = targetPosition;

        GetComponent<SphereCollider>().enabled = true;
        itemPickUpAllowed = true;
    }

    // Used to add an item trait to this item.
    private void AddTrait(ItemTrait.TraitType selectedTrait, float value, int multiplierValue)
    {
        bool traitExists = false;
        foreach (ItemTrait itemTrait in itemTraits)
            if (itemTrait.traitType == selectedTrait)
            {
                itemTrait.traitBonusMultiplier += multiplierValue;
                traitExists = true;
            }

        if (!traitExists)
            itemTraits.Add(new ItemTrait(selectedTrait, value, multiplierValue));
    }

    // Overload that uses a full item trait instead of components
    private void AddTrait(ItemTrait previousItemTrait)
    {
        bool traitExists = false;
        foreach (ItemTrait itemTrait in itemTraits)
            if (itemTrait.traitType == previousItemTrait.traitType)
            {
                itemTrait.traitBonusMultiplier++;
                traitExists = true;
            }

        if (!traitExists)
            itemTraits.Add(previousItemTrait);
    }

    private void WipeTraits()
    {
        itemTraits = new List<ItemTrait>();
        AddBaseItemTraits();

        switch (affinityPrimary)
        {
            case AffinityType.Physical:
                baseDamageScaling /= 2;
                break;
            case AffinityType.Stun:
                baseDamageScaling /= 2;
                break;
            case AffinityType.Knockback:
                baseDamageScaling /= 2;
                break;
            default:
                break;
        }
    }

    // This method assigns the item a primary secondary and tertiary trait for the item and assigns traits for each. We also assign the modifiers and assign traits for those too.
    public void RollItemTraitsAffinityAndModifiers()
    {
        if (itemType == ItemType.Skill)
        {
            affinityPrimary = AffinityType.None;
            affinitySecondary = AffinityType.None;
            affinityTertiary = AffinityType.None;
            return;
        }

        //Debug.Log("Rolling TRaits And shit");
        int modifierCount = 0;
        int affinityCount = 0;
        int traitCountAffinity = 0;
        int traitCountModifiers = 0;

        // set up the trait counts based on our item rarity
        switch (itemRarity)
        {
            case ItemRarity.Common:
                modifierCount = Random.Range(0, 2);
                affinityCount = 1;
                traitCountAffinity = 2;
                traitCountModifiers = 1;
                break;
            case ItemRarity.Uncommon:
                modifierCount = Random.Range(1, 3);
                affinityCount = Random.Range(1, 3);
                traitCountAffinity = 4;
                traitCountModifiers = 2;
                break;
            case ItemRarity.Rare:
                modifierCount = Random.Range(1, 3);
                affinityCount = 2;
                traitCountAffinity = 8;
                traitCountModifiers = 4;
                break;
            case ItemRarity.Legendary:
                modifierCount = Random.Range(2, 4);
                affinityCount = Random.Range(2, 4);
                traitCountAffinity = 16;
                traitCountModifiers = 8;
                break;
            case ItemRarity.Masterwork:
                modifierCount = Random.Range(2, 4);
                affinityCount = Random.Range(2, 4);
                traitCountAffinity = 32;
                traitCountModifiers = 16;
                break;
            default:
                break;
        }

        if (itemType == ItemType.TwoHandWeapon)
        {
            traitCountAffinity *= 2;
            traitCountModifiers *= 2;
        }

        //Debug.Log("assigning affinities");
        // Add Affinities based on our affinity count, wipe them all first
        affinityPrimary = AffinityType.None;
        affinitySecondary = AffinityType.None;
        affinityTertiary = AffinityType.None;
        affinitySecondaryMultiElement = false;
        affinityTertiaryMultiElement = false;
        for (int index = 0; index < affinityCount; index++)
        {
            bool chosenAffinityIsValid = false;
            AffinityType chosenAffinity = AffinityType.None;

            // choose an affinity and check to make sure it doesnt have issues with previously chosen affinities
            while (!chosenAffinityIsValid)
            {
                if (index != 2)
                {
                    switch (Random.Range(0, 9))
                    {
                        case 0:
                            chosenAffinity = AffinityType.Fire;
                            if (index == 1 && affinityPrimary != AffinityType.Fire && affinityPrimary != AffinityType.Ice)
                                chosenAffinityIsValid = true;
                            break;
                        case 1:
                            chosenAffinity = AffinityType.Ice;
                            if (index == 1 && affinityPrimary != AffinityType.Ice && affinityPrimary != AffinityType.Fire)
                                chosenAffinityIsValid = true;
                            break;
                        case 2:
                            chosenAffinity = AffinityType.Earth;
                            if (index == 1 && affinityPrimary != AffinityType.Earth && affinityPrimary != AffinityType.Ice)
                                chosenAffinityIsValid = true;
                            break;
                        case 3:
                            chosenAffinity = AffinityType.Wind;
                            if (index == 1 && affinityPrimary != AffinityType.Wind && affinityPrimary != AffinityType.Ice)
                                chosenAffinityIsValid = true;
                            break;
                        case 4:
                            chosenAffinity = AffinityType.Physical;
                            if (index == 1 && affinityPrimary != AffinityType.Physical)
                                chosenAffinityIsValid = true;
                            break;
                        case 5:
                            chosenAffinity = AffinityType.Bleed;
                            if (index == 1 && affinityPrimary != AffinityType.Bleed)
                                chosenAffinityIsValid = true;
                            break;
                        case 6:
                            chosenAffinity = AffinityType.Poison;
                            if (index == 1 && affinityPrimary != AffinityType.Poison)
                                chosenAffinityIsValid = true;
                            break;
                        case 7:
                            chosenAffinity = AffinityType.Stun;
                            if (index == 1 && affinityPrimary != AffinityType.Stun)
                                chosenAffinityIsValid = true;
                            break;
                        case 8:
                            chosenAffinity = AffinityType.Knockback;
                            if (index == 1 && affinityPrimary != AffinityType.Knockback)
                                chosenAffinityIsValid = true;
                            break;
                        default:
                            break;
                    }

                    if (index == 0)
                        chosenAffinityIsValid = true;
                }
                else
                {
                    chosenAffinity = affinitySecondary;
                    chosenAffinityIsValid = true;
                }
            }

            if (index == 1)
            {
                if (Random.Range(0, 100f) >= 50)
                    affinitySecondaryMultiElement = true;
            }

            else if (index == 2)
            {
                if (!affinitySecondaryMultiElement)
                    affinityTertiaryMultiElement = true;
            }

            switch (index)
            {
                case 0:
                    affinityPrimary = chosenAffinity;
                    break;
                case 1:
                    affinitySecondary = chosenAffinity;
                    break;
                case 2:
                    affinityTertiary = chosenAffinity;
                    break;
                default:
                    break;
            }
        }



        // Add modifiers to the list. WIpe the list first
        itemModifiers = new List<ModifierType>();
        for (int index = 0; index < modifierCount; index++)
        {
            bool chosenModifierIsValid = false;
            ModifierType chosenModifier = ModifierType.None;

            while (!chosenModifierIsValid)
            {
                if (Random.Range(0, 100f) > 20)
                {
                    switch (Random.Range(0, 10))
                    {
                        case 0:
                            chosenModifier = ModifierType.Brawny;
                            break;
                        case 1:
                            chosenModifier = ModifierType.Devastating;
                            break;
                        case 2:
                            chosenModifier = ModifierType.Hardened;
                            break;
                        case 3:
                            chosenModifier = ModifierType.Illustrious;
                            break;
                        case 4:
                            chosenModifier = ModifierType.Lucky;
                            break;
                        case 5:
                            chosenModifier = ModifierType.Magic;
                            break;
                        case 6:
                            chosenModifier = ModifierType.Nimble;
                            break;
                        case 7:
                            chosenModifier = ModifierType.Strong;
                            break;
                        case 8:
                            chosenModifier = ModifierType.Vamperic;
                            break;
                        case 9:
                            chosenModifier = ModifierType.Resistant;
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    switch (Random.Range(0, 10))
                    {
                        case 0:
                            chosenModifier = ModifierType.Brittle;
                            break;
                        case 1:
                            chosenModifier = ModifierType.Cracked;
                            break;
                        case 2:
                            chosenModifier = ModifierType.Cursed;
                            break;
                        case 3:
                            chosenModifier = ModifierType.Dull;
                            break;
                        case 4:
                            chosenModifier = ModifierType.Meager;
                            break;
                        case 5:
                            chosenModifier = ModifierType.Mundane;
                            break;
                        case 6:
                            chosenModifier = ModifierType.Rusty;
                            break;
                        case 7:
                            chosenModifier = ModifierType.Sluggish;
                            break;
                        case 8:
                            chosenModifier = ModifierType.Unfavoured;
                            break;
                        case 9:
                            chosenModifier = ModifierType.Vulnerable;
                            break;
                        default:
                            break;
                    }
                }

                chosenModifierIsValid = IsSelectedModifierValid(chosenModifier);
            }

            itemModifiers.Add(chosenModifier);
        }

        // Here we set up the prefixes and the suffixes based on our modifiers and affinities
        itemNameAffinitySuffix = "of";
        itemNameModifiersPrefix = "";

        if (affinityTertiary != AffinityType.None)
        {
            AffinityType affinitySuffixToAdd = affinityTertiary;

            if (affinityTertiaryMultiElement)
                affinitySuffixToAdd = affinityPrimary;

            switch (affinitySuffixToAdd)
            {
                case AffinityType.Fire:
                    itemNameAffinitySuffix += " <color=#FF932E>Fiery</color>";
                    break;
                case AffinityType.Ice:
                    itemNameAffinitySuffix += " <color=#5AD9F5>Icy</color>";
                    break;
                case AffinityType.Earth:
                    itemNameAffinitySuffix += " <color=#B0946C>Sundered</color>";
                    break;
                case AffinityType.Wind:
                    itemNameAffinitySuffix += " <color=#ABD1E0>Howling</color>";
                    break;
                case AffinityType.Physical:
                    itemNameAffinitySuffix += " <color=#E45B5B>Hardened</color>";
                    break;
                case AffinityType.Bleed:
                    itemNameAffinitySuffix += " <color=#AB181D>Bloody</color>";
                    break;
                case AffinityType.Poison:
                    itemNameAffinitySuffix += " <color=#93D916>Toxic</color>";
                    break;
                case AffinityType.Stun:
                    itemNameAffinitySuffix += " <color=#FFF04F>Stunning</color>";
                    break;
                case AffinityType.Knockback:
                    itemNameAffinitySuffix += " <color=#1F86CA>Forceful</color>";
                    break;
                default:
                    break;
            }
        }

        if (affinitySecondary != AffinityType.None)
        {
            AffinityType affinitySuffixToAdd = affinitySecondary;

            if (affinitySecondaryMultiElement)
                affinitySuffixToAdd = affinityPrimary;

            switch (affinitySuffixToAdd)
            {
                case AffinityType.Fire:
                    itemNameAffinitySuffix += " <color=#FF932E>Fiery</color>";
                    break;
                case AffinityType.Ice:
                    itemNameAffinitySuffix += " <color=#5AD9F5>Icy</color>";
                    break;
                case AffinityType.Earth:
                    itemNameAffinitySuffix += " <color=#B0946C>Sundered</color>";
                    break;
                case AffinityType.Wind:
                    itemNameAffinitySuffix += " <color=#ABD1E0>Howling</color>";
                    break;
                case AffinityType.Physical:
                    itemNameAffinitySuffix += " <color=#E45B5B>Hardened</color>";
                    break;
                case AffinityType.Bleed:
                    itemNameAffinitySuffix += " <color=#AB181D>Bloody</color>";
                    break;
                case AffinityType.Poison:
                    itemNameAffinitySuffix += " <color=#93D916>Toxic</color>";
                    break;
                case AffinityType.Stun:
                    itemNameAffinitySuffix += " <color=#FFF04F>Stunning</color>";
                    break;
                case AffinityType.Knockback:
                    itemNameAffinitySuffix += " <color=#1F86CA>Forceful</color>";
                    break;
                default:
                    break;
            }
        }

        if (affinityPrimary != AffinityType.None)
        {
            AffinityType affinitySuffixToAdd = affinityPrimary;

            switch (affinitySuffixToAdd)
            {
                case AffinityType.Fire:
                    itemNameAffinitySuffix += " <color=#FF932E>Flame</color>";
                    break;
                case AffinityType.Ice:
                    itemNameAffinitySuffix += " <color=#5AD9F5>Frost</color>";
                    break;
                case AffinityType.Earth:
                    itemNameAffinitySuffix += " <color=#B0946C>Earth</color>";
                    break;
                case AffinityType.Wind:
                    itemNameAffinitySuffix += " <color=#ABD1E0>Gales</color>";
                    break;
                case AffinityType.Physical:
                    itemNameAffinitySuffix += " <color=#E45B5B>Steel</color>";
                    break;
                case AffinityType.Bleed:
                    itemNameAffinitySuffix += " <color=#AB181D>Exsanguination</color>";
                    break;
                case AffinityType.Poison:
                    itemNameAffinitySuffix += " <color=#93D916>Plagues</color>";
                    break;
                case AffinityType.Stun:
                    itemNameAffinitySuffix += " <color=#FFF04F>Daze</color>";
                    break;
                case AffinityType.Knockback:
                    itemNameAffinitySuffix += " <color=#1F86CA>Impact</color>";
                    break;
                default:
                    break;
            }
        }


        foreach (ModifierType modifer in itemModifiers)
        {
            switch (modifer)
            {
                case ModifierType.Devastating:
                    itemNameModifiersPrefix += "<color=#E94453>Devastating</color> ";
                    break;
                case ModifierType.Dull:
                    itemNameModifiersPrefix += "<color=#E94453>Dull</color> ";
                    break;
                case ModifierType.Hardened:
                    itemNameModifiersPrefix += "<color=#FAFF00>Hardened</color> ";
                    break;
                case ModifierType.Cracked:
                    itemNameModifiersPrefix += "<color=#FAFF00>Cracked</color> ";
                    break;
                case ModifierType.Nimble:
                    itemNameModifiersPrefix += "<color=#5DB1E5>Nimble</color> ";
                    break;
                case ModifierType.Sluggish:
                    itemNameModifiersPrefix += "<color=#5DB1E5>Sluggish</color> ";
                    break;
                case ModifierType.Vamperic:
                    itemNameModifiersPrefix += "<color=#60D46D>Vamperic</color> ";
                    break;
                case ModifierType.Cursed:
                    itemNameModifiersPrefix += "<color=#60D46D>Cursed</color> ";
                    break;
                case ModifierType.Magic:
                    itemNameModifiersPrefix += "<color=#5DB1E5>Magic</color> ";
                    break;
                case ModifierType.Mundane:
                    itemNameModifiersPrefix += "<color=#5DB1E5>Mundane</color> ";
                    break;
                case ModifierType.Lucky:
                    itemNameModifiersPrefix += "<color=#299F50>Lucky</color> ";
                    break;
                case ModifierType.Unfavoured:
                    itemNameModifiersPrefix += "<color=#299F50>Unfavoured</color> ";
                    break;
                case ModifierType.Strong:
                    itemNameModifiersPrefix += "<color=#D8D99E>Strong</color> ";
                    break;
                case ModifierType.Brittle:
                    itemNameModifiersPrefix += "<color=#D8D99E>Brittle</color> ";
                    break;
                case ModifierType.Illustrious:
                    itemNameModifiersPrefix += "<color=#AE88C6>Illustrious</color> ";
                    break;
                case ModifierType.Rusty:
                    itemNameModifiersPrefix += "<color=#AE88C6>Rusty</color> ";
                    break;
                case ModifierType.Brawny:
                    itemNameModifiersPrefix += "<color=#AD2A2A>Brawny</color> ";
                    break;
                case ModifierType.Meager:
                    itemNameModifiersPrefix += "<color=#AD2A2A>Meager</color> ";
                    break;
                case ModifierType.Resistant:
                    itemNameModifiersPrefix += "<color=#C353AC>Resistant</color> ";
                    break;
                case ModifierType.Vulnerable:
                    itemNameModifiersPrefix += "<color=#C353AC>Vulnerable</color> ";
                    break;
                default:
                    break;
            }
        }

        // Start adding traits based on our trait counts for affinities and modifiers
        for (int modifierIndex = 0; modifierIndex < itemModifiers.Count; modifierIndex++)
        {
            //Debug.Log("adding traits for modifier: " + itemModifiers[modifierIndex]);

            int traitsToAdd = traitCountModifiers;
            ModifierType modType = itemModifiers[modifierIndex];

            if (modifierIndex != itemModifiers.Count - 1)
            {
                traitsToAdd = Mathf.RoundToInt(traitCountModifiers * Random.Range(0.3f, 0.6f));
                traitCountModifiers -= traitsToAdd;
            }

            for (int index = 0; index < traitsToAdd; index++)
            {
                ItemTrait tempTrait = new ItemTrait();
                tempTrait.GetTraitForModifier(modType);

                AddTrait(tempTrait);
            }
        }

        int affinityTypeCount = 1;
        if (affinitySecondary != AffinityType.None)
            affinityTypeCount++;
        if (affinityTertiary != AffinityType.None)
            affinityTypeCount++;

        // Start adding traits based on our trait counts for affinities and modifiers
        for (int affinityIndex = 0; affinityIndex < affinityTypeCount; affinityIndex++)
        {
            AffinityType affinityChosen = affinityPrimary;
            AffinityType affinityChosenSplit = AffinityType.None;
            switch (affinityIndex)
            {
                case 0:
                    affinityChosen = affinityPrimary;
                    break;
                case 1:
                    affinityChosen = affinitySecondary;
                    if (affinitySecondaryMultiElement)
                        affinityChosenSplit = affinityPrimary;
                    break;
                case 2:
                    affinityChosen = affinityTertiary;
                    if (affinityTertiaryMultiElement)
                        affinityChosenSplit = affinityPrimary;
                    break;
                default:
                    break;
            }

            int traitsToAdd = traitCountAffinity;

            if (affinityIndex != affinityTypeCount - 1)
            {
                traitsToAdd = Mathf.RoundToInt(traitCountAffinity * Random.Range(0.4f, 0.6f));
                traitCountAffinity -= traitsToAdd;
            }

            for (int index = 0; index < traitsToAdd; index++)
            {
                ItemTrait tempTrait = new ItemTrait();
                tempTrait.GetTraitForAffinity(affinityChosen, affinityChosenSplit);

                AddTrait(tempTrait);
            }
        }

        // Change our damage type if we are a weapon to match the primary trait.
        if (itemType == ItemType.Weapon || itemType == ItemType.TwoHandWeapon)
        {
            switch (affinityPrimary)
            {
                case AffinityType.None:
                    damageType = HitBox.DamageType.Physical;
                    stacksToAddOnHit = 0;
                    break;
                case AffinityType.Fire:
                    damageType = HitBox.DamageType.Fire;
                    break;
                case AffinityType.Ice:
                    damageType = HitBox.DamageType.Ice;
                    break;
                case AffinityType.Earth:
                    damageType = HitBox.DamageType.Earth;
                    break;
                case AffinityType.Wind:
                    damageType = HitBox.DamageType.Wind;
                    break;
                case AffinityType.Physical:
                    damageType = HitBox.DamageType.Physical;
                    baseDamageScaling *= 2;
                    stacksToAddOnHit = 0;
                    break;
                case AffinityType.Bleed:
                    damageType = HitBox.DamageType.Bleed;
                    break;
                case AffinityType.Poison:
                    damageType = HitBox.DamageType.Poison;
                    break;
                case AffinityType.Stun:
                    damageType = HitBox.DamageType.Physical;
                    baseDamageScaling *= 2;
                    stacksToAddOnHit = 0;
                    break;
                case AffinityType.Knockback:
                    damageType = HitBox.DamageType.Physical;
                    baseDamageScaling *= 2;
                    stacksToAddOnHit = 0;
                    break;
                default:
                    break;
            }
        }
    }

    // This method assigns the item a primary secondary and tertiary trait for the item and assigns traits for each. We also assign the modifiers and assign traits for those too.
    public void RollItemTraitsAffinityAndModifiers(AffinityType primaryAffinityOverride, AffinityType secondaryAffinityOverride)
    {
        WipeTraits();

        if (itemType == ItemType.Skill)
        {
            affinityPrimary = AffinityType.None;
            affinitySecondary = AffinityType.None;
            affinityTertiary = AffinityType.None;
            return;
        }

        //Debug.Log("Rolling TRaits And shit");
        int modifierCount = 0;
        int affinityCount = 0;
        int traitCountAffinity = 0;
        int traitCountModifiers = 0;


        // Set our affinity count based on if the affinity overrides match.
        if (primaryAffinityOverride == secondaryAffinityOverride)
            affinityCount = 1;
        else
            affinityCount = Random.Range(2, 4);


        // set up the trait counts based on our item rarity
        switch (itemRarity)
        {
            case ItemRarity.Common:
                modifierCount = Random.Range(0, 2);
                traitCountAffinity = 2;
                traitCountModifiers = 1;
                break;
            case ItemRarity.Uncommon:
                modifierCount = Random.Range(1, 3);
                traitCountAffinity = 4;
                traitCountModifiers = 2;
                break;
            case ItemRarity.Rare:
                modifierCount = Random.Range(1, 3);
                traitCountAffinity = 8;
                traitCountModifiers = 4;
                break;
            case ItemRarity.Legendary:
                modifierCount = Random.Range(2, 4);
                traitCountAffinity = 16;
                traitCountModifiers = 8;
                break;
            case ItemRarity.Masterwork:
                modifierCount = Random.Range(2, 4);
                traitCountAffinity = 32;
                traitCountModifiers = 16;
                break;
            default:
                break;
        }

        if (itemType == ItemType.TwoHandWeapon)
        {
            traitCountAffinity *= 2;
            traitCountModifiers *= 2;
        }

        //Debug.Log("assigning affinities");
        // Add Affinities based on our affinity count, wipe them all first
        affinityPrimary = AffinityType.None;
        affinitySecondary = AffinityType.None;
        affinityTertiary = AffinityType.None;
        affinitySecondaryMultiElement = false;
        affinityTertiaryMultiElement = false;
        for (int index = 0; index < affinityCount; index++)
        {
            AffinityType chosenAffinity = AffinityType.None;
            // Set the affinity based on the index
            switch (index)
            {
                case 0:
                    chosenAffinity = primaryAffinityOverride;
                    break;
                case 1:
                    chosenAffinity = secondaryAffinityOverride;
                    break;
                case 2:
                    chosenAffinity = secondaryAffinityOverride;
                    break;
                default:
                    break;
            }

            if (index == 1)
            {
                if (Random.Range(0, 100f) >= 50)
                    affinitySecondaryMultiElement = true;
            }

            else if (index == 2)
            {
                if (!affinitySecondaryMultiElement)
                    affinityTertiaryMultiElement = true;
            }

            switch (index)
            {
                case 0:
                    affinityPrimary = chosenAffinity;
                    break;
                case 1:
                    affinitySecondary = chosenAffinity;
                    break;
                case 2:
                    affinityTertiary = chosenAffinity;
                    break;
                default:
                    break;
            }
        }



        // Add modifiers to the list. WIpe the list first
        itemModifiers = new List<ModifierType>();
        for (int index = 0; index < modifierCount; index++)
        {
            bool chosenModifierIsValid = false;
            ModifierType chosenModifier = ModifierType.None;

            while (!chosenModifierIsValid)
            {
                if (Random.Range(0, 100f) > 20)
                {
                    switch (Random.Range(0, 10))
                    {
                        case 0:
                            chosenModifier = ModifierType.Brawny;
                            break;
                        case 1:
                            chosenModifier = ModifierType.Devastating;
                            break;
                        case 2:
                            chosenModifier = ModifierType.Hardened;
                            break;
                        case 3:
                            chosenModifier = ModifierType.Illustrious;
                            break;
                        case 4:
                            chosenModifier = ModifierType.Lucky;
                            break;
                        case 5:
                            chosenModifier = ModifierType.Magic;
                            break;
                        case 6:
                            chosenModifier = ModifierType.Nimble;
                            break;
                        case 7:
                            chosenModifier = ModifierType.Strong;
                            break;
                        case 8:
                            chosenModifier = ModifierType.Vamperic;
                            break;
                        case 9:
                            chosenModifier = ModifierType.Resistant;
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    switch (Random.Range(0, 10))
                    {
                        case 0:
                            chosenModifier = ModifierType.Brittle;
                            break;
                        case 1:
                            chosenModifier = ModifierType.Cracked;
                            break;
                        case 2:
                            chosenModifier = ModifierType.Cursed;
                            break;
                        case 3:
                            chosenModifier = ModifierType.Dull;
                            break;
                        case 4:
                            chosenModifier = ModifierType.Meager;
                            break;
                        case 5:
                            chosenModifier = ModifierType.Mundane;
                            break;
                        case 6:
                            chosenModifier = ModifierType.Rusty;
                            break;
                        case 7:
                            chosenModifier = ModifierType.Sluggish;
                            break;
                        case 8:
                            chosenModifier = ModifierType.Unfavoured;
                            break;
                        case 9:
                            chosenModifier = ModifierType.Vulnerable;
                            break;
                        default:
                            break;
                    }
                }

                chosenModifierIsValid = IsSelectedModifierValid(chosenModifier);
            }

            itemModifiers.Add(chosenModifier);
        }

        // Here we set up the prefixes and the suffixes based on our modifiers and affinities
        itemNameAffinitySuffix = "of";
        itemNameModifiersPrefix = "";

        if (affinityTertiary != AffinityType.None)
        {
            AffinityType affinitySuffixToAdd = affinityTertiary;

            if (affinityTertiaryMultiElement)
                affinitySuffixToAdd = affinityPrimary;

            switch (affinitySuffixToAdd)
            {
                case AffinityType.Fire:
                    itemNameAffinitySuffix += " <color=#FF932E>Fiery</color>";
                    break;
                case AffinityType.Ice:
                    itemNameAffinitySuffix += " <color=#5AD9F5>Icy</color>";
                    break;
                case AffinityType.Earth:
                    itemNameAffinitySuffix += " <color=#B0946C>Sundered</color>";
                    break;
                case AffinityType.Wind:
                    itemNameAffinitySuffix += " <color=#ABD1E0>Howling</color>";
                    break;
                case AffinityType.Physical:
                    itemNameAffinitySuffix += " <color=#E45B5B>Hardened</color>";
                    break;
                case AffinityType.Bleed:
                    itemNameAffinitySuffix += " <color=#AB181D>Bloody</color>";
                    break;
                case AffinityType.Poison:
                    itemNameAffinitySuffix += " <color=#93D916>Toxic</color>";
                    break;
                case AffinityType.Stun:
                    itemNameAffinitySuffix += " <color=#FFF04F>Stunning</color>";
                    break;
                case AffinityType.Knockback:
                    itemNameAffinitySuffix += " <color=#1F86CA>Forceful</color>";
                    break;
                default:
                    break;
            }
        }

        if (affinitySecondary != AffinityType.None)
        {
            AffinityType affinitySuffixToAdd = affinitySecondary;

            if (affinitySecondaryMultiElement)
                affinitySuffixToAdd = affinityPrimary;

            switch (affinitySuffixToAdd)
            {
                case AffinityType.Fire:
                    itemNameAffinitySuffix += " <color=#FF932E>Fiery</color>";
                    break;
                case AffinityType.Ice:
                    itemNameAffinitySuffix += " <color=#5AD9F5>Icy</color>";
                    break;
                case AffinityType.Earth:
                    itemNameAffinitySuffix += " <color=#B0946C>Sundered</color>";
                    break;
                case AffinityType.Wind:
                    itemNameAffinitySuffix += " <color=#ABD1E0>Howling</color>";
                    break;
                case AffinityType.Physical:
                    itemNameAffinitySuffix += " <color=#E45B5B>Hardened</color>";
                    break;
                case AffinityType.Bleed:
                    itemNameAffinitySuffix += " <color=#AB181D>Bloody</color>";
                    break;
                case AffinityType.Poison:
                    itemNameAffinitySuffix += " <color=#93D916>Toxic</color>";
                    break;
                case AffinityType.Stun:
                    itemNameAffinitySuffix += " <color=#FFF04F>Stunning</color>";
                    break;
                case AffinityType.Knockback:
                    itemNameAffinitySuffix += " <color=#1F86CA>Forceful</color>";
                    break;
                default:
                    break;
            }
        }

        if (affinityPrimary != AffinityType.None)
        {
            AffinityType affinitySuffixToAdd = affinityPrimary;

            switch (affinitySuffixToAdd)
            {
                case AffinityType.Fire:
                    itemNameAffinitySuffix += " <color=#FF932E>Flame</color>";
                    break;
                case AffinityType.Ice:
                    itemNameAffinitySuffix += " <color=#5AD9F5>Frost</color>";
                    break;
                case AffinityType.Earth:
                    itemNameAffinitySuffix += " <color=#B0946C>Earth</color>";
                    break;
                case AffinityType.Wind:
                    itemNameAffinitySuffix += " <color=#ABD1E0>Gales</color>";
                    break;
                case AffinityType.Physical:
                    itemNameAffinitySuffix += " <color=#E45B5B>Steel</color>";
                    break;
                case AffinityType.Bleed:
                    itemNameAffinitySuffix += " <color=#AB181D>Exsanguination</color>";
                    break;
                case AffinityType.Poison:
                    itemNameAffinitySuffix += " <color=#93D916>Plagues</color>";
                    break;
                case AffinityType.Stun:
                    itemNameAffinitySuffix += " <color=#FFF04F>Daze</color>";
                    break;
                case AffinityType.Knockback:
                    itemNameAffinitySuffix += " <color=#1F86CA>Impact</color>";
                    break;
                default:
                    break;
            }
        }


        foreach (ModifierType modifer in itemModifiers)
        {
            switch (modifer)
            {
                case ModifierType.Devastating:
                    itemNameModifiersPrefix += "<color=#E94453>Devastating</color> ";
                    break;
                case ModifierType.Dull:
                    itemNameModifiersPrefix += "<color=#E94453>Dull</color> ";
                    break;
                case ModifierType.Hardened:
                    itemNameModifiersPrefix += "<color=#FAFF00>Hardened</color> ";
                    break;
                case ModifierType.Cracked:
                    itemNameModifiersPrefix += "<color=#FAFF00>Cracked</color> ";
                    break;
                case ModifierType.Nimble:
                    itemNameModifiersPrefix += "<color=#5DB1E5>Nimble</color> ";
                    break;
                case ModifierType.Sluggish:
                    itemNameModifiersPrefix += "<color=#5DB1E5>Sluggish</color> ";
                    break;
                case ModifierType.Vamperic:
                    itemNameModifiersPrefix += "<color=#60D46D>Vamperic</color> ";
                    break;
                case ModifierType.Cursed:
                    itemNameModifiersPrefix += "<color=#60D46D>Cursed</color> ";
                    break;
                case ModifierType.Magic:
                    itemNameModifiersPrefix += "<color=#5DB1E5>Magic</color> ";
                    break;
                case ModifierType.Mundane:
                    itemNameModifiersPrefix += "<color=#5DB1E5>Mundane</color> ";
                    break;
                case ModifierType.Lucky:
                    itemNameModifiersPrefix += "<color=#299F50>Lucky</color> ";
                    break;
                case ModifierType.Unfavoured:
                    itemNameModifiersPrefix += "<color=#299F50>Unfavoured</color> ";
                    break;
                case ModifierType.Strong:
                    itemNameModifiersPrefix += "<color=#D8D99E>Strong</color> ";
                    break;
                case ModifierType.Brittle:
                    itemNameModifiersPrefix += "<color=#D8D99E>Brittle</color> ";
                    break;
                case ModifierType.Illustrious:
                    itemNameModifiersPrefix += "<color=#AE88C6>Illustrious</color> ";
                    break;
                case ModifierType.Rusty:
                    itemNameModifiersPrefix += "<color=#AE88C6>Rusty</color> ";
                    break;
                case ModifierType.Brawny:
                    itemNameModifiersPrefix += "<color=#AD2A2A>Brawny</color> ";
                    break;
                case ModifierType.Meager:
                    itemNameModifiersPrefix += "<color=#AD2A2A>Meager</color> ";
                    break;
                case ModifierType.Resistant:
                    itemNameModifiersPrefix += "<color=#C353AC>Resistant</color> ";
                    break;
                case ModifierType.Vulnerable:
                    itemNameModifiersPrefix += "<color=#C353AC>Vulnerable</color> ";
                    break;
                default:
                    break;
            }
        }

        // Start adding traits based on our trait counts for affinities and modifiers
        for (int modifierIndex = 0; modifierIndex < itemModifiers.Count; modifierIndex++)
        {
            //Debug.Log("adding traits for modifier: " + itemModifiers[modifierIndex]);

            int traitsToAdd = traitCountModifiers;
            ModifierType modType = itemModifiers[modifierIndex];

            if (modifierIndex != itemModifiers.Count - 1)
            {
                traitsToAdd = Mathf.RoundToInt(traitCountModifiers * Random.Range(0.3f, 0.6f));
                traitCountModifiers -= traitsToAdd;
            }

            for (int index = 0; index < traitsToAdd; index++)
            {
                ItemTrait tempTrait = new ItemTrait();
                tempTrait.GetTraitForModifier(modType);

                AddTrait(tempTrait);
            }
        }

        int affinityTypeCount = 1;
        if (affinitySecondary != AffinityType.None)
            affinityTypeCount++;
        if (affinityTertiary != AffinityType.None)
            affinityTypeCount++;

        // Start adding traits based on our trait counts for affinities and modifiers
        for (int affinityIndex = 0; affinityIndex < affinityTypeCount; affinityIndex++)
        {
            AffinityType affinityChosen = affinityPrimary;
            AffinityType affinityChosenSplit = AffinityType.None;
            switch (affinityIndex)
            {
                case 0:
                    affinityChosen = affinityPrimary;
                    break;
                case 1:
                    affinityChosen = affinitySecondary;
                    if (affinitySecondaryMultiElement)
                        affinityChosenSplit = affinityPrimary;
                    break;
                case 2:
                    affinityChosen = affinityTertiary;
                    if (affinityTertiaryMultiElement)
                        affinityChosenSplit = affinityPrimary;
                    break;
                default:
                    break;
            }

            int traitsToAdd = traitCountAffinity;

            if (affinityIndex != affinityTypeCount - 1)
            {
                traitsToAdd = Mathf.RoundToInt(traitCountAffinity * Random.Range(0.4f, 0.6f));
                traitCountAffinity -= traitsToAdd;
            }

            for (int index = 0; index < traitsToAdd; index++)
            {
                ItemTrait tempTrait = new ItemTrait();
                tempTrait.GetTraitForAffinity(affinityChosen, affinityChosenSplit);

                AddTrait(tempTrait);
            }
        }

        // Change our damage type if we are a weapon to match the primary trait.
        if (itemType == ItemType.Weapon || itemType == ItemType.TwoHandWeapon)
        {
            switch (affinityPrimary)
            {
                case AffinityType.None:
                    damageType = HitBox.DamageType.Physical;
                    stacksToAddOnHit = 0;
                    break;
                case AffinityType.Fire:
                    damageType = HitBox.DamageType.Fire;
                    break;
                case AffinityType.Ice:
                    damageType = HitBox.DamageType.Ice;
                    break;
                case AffinityType.Earth:
                    damageType = HitBox.DamageType.Earth;
                    break;
                case AffinityType.Wind:
                    damageType = HitBox.DamageType.Wind;
                    break;
                case AffinityType.Physical:
                    damageType = HitBox.DamageType.Physical;
                    baseDamageScaling *= 2;
                    stacksToAddOnHit = 0;
                    break;
                case AffinityType.Bleed:
                    damageType = HitBox.DamageType.Bleed;
                    break;
                case AffinityType.Poison:
                    damageType = HitBox.DamageType.Poison;
                    break;
                case AffinityType.Stun:
                    damageType = HitBox.DamageType.Physical;
                    baseDamageScaling *= 2;
                    stacksToAddOnHit = 0;
                    break;
                case AffinityType.Knockback:
                    damageType = HitBox.DamageType.Physical;
                    baseDamageScaling *= 2;
                    stacksToAddOnHit = 0;
                    break;
                default:
                    break;
            }
        }
    }

    //USed to cehck to see if a modifier already exists in the list, if it does return false, if not return true;
    private bool IsSelectedModifierValid(ModifierType modifier)
    {
        bool modValid = true;

        foreach(ModifierType mod in itemModifiers)
        {
            // checks to see if we have have the same or the opposite modifier already
            switch (modifier)
            {
                case ModifierType.Devastating:
                    if (mod == modifier || mod == ModifierType.Dull)
                        modValid = false;
                    break;
                case ModifierType.Dull:
                    if (mod == modifier || mod == ModifierType.Devastating)
                        modValid = false;
                    break;
                case ModifierType.Hardened:
                    if (mod == modifier || mod == ModifierType.Cracked)
                        modValid = false;
                    break;
                case ModifierType.Cracked:
                    if (mod == modifier || mod == ModifierType.Hardened)
                        modValid = false;
                    break;
                case ModifierType.Nimble:
                    if (mod == modifier || mod == ModifierType.Sluggish)
                        modValid = false;
                    break;
                case ModifierType.Sluggish:
                    if (mod == modifier || mod == ModifierType.Nimble)
                        modValid = false;
                    break;
                case ModifierType.Vamperic:
                    if (mod == modifier || mod == ModifierType.Cursed)
                        modValid = false;
                    break;
                case ModifierType.Cursed:
                    if (mod == modifier || mod == ModifierType.Vamperic)
                        modValid = false;
                    break;
                case ModifierType.Magic:
                    if (mod == modifier || mod == ModifierType.Mundane)
                        modValid = false;
                    break;
                case ModifierType.Mundane:
                    if (mod == modifier || mod == ModifierType.Magic)
                        modValid = false;
                    break;
                case ModifierType.Lucky:
                    if (mod == modifier || mod == ModifierType.Unfavoured)
                        modValid = false;
                    break;
                case ModifierType.Unfavoured:
                    if (mod == modifier || mod == ModifierType.Lucky)
                        modValid = false;
                    break;
                case ModifierType.Strong:
                    if (mod == modifier || mod == ModifierType.Brittle)
                        modValid = false;
                    break;
                case ModifierType.Brittle:
                    if (mod == modifier || mod == ModifierType.Strong)
                        modValid = false;
                    break;
                case ModifierType.Illustrious:
                    if (mod == modifier || mod == ModifierType.Rusty)
                        modValid = false;
                    break;
                case ModifierType.Rusty:
                    if (mod == modifier || mod == ModifierType.Illustrious)
                        modValid = false;
                    break;
                case ModifierType.Brawny:
                    if (mod == modifier || mod == ModifierType.Meager)
                        modValid = false;
                    break;
                case ModifierType.Meager:
                    if (mod == modifier || mod == ModifierType.Brawny)
                        modValid = false;
                    break;
                case ModifierType.Resistant:
                    if (mod == modifier || mod == ModifierType.Vulnerable)
                        modValid = false;
                    break;
                case ModifierType.Vulnerable:
                    if (mod == modifier || mod == ModifierType.Resistant)
                        modValid = false;
                    break;
                default:
                    break;
            }

        }

        return modValid;
    }
}
