using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName;
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

    public enum HelmetType {NoFeatures, NoHair, AllFeatures};
    public HelmetType helmetType;

    [HideInInspector]public bool equippedToRightHand = false;
    public bool stackable;
    public bool instantPickup;
    [HideInInspector]public GameObject previousOwner = null;
    public int currentStack;
    public int maxStack;

    public Sprite artwork;

    public int inventoryIndex = 0;

    public int goldValue;

    public enum ItemType {TrinketRing, Weapon, TwoHandWeapon, Helmet, Legs, Armor, Shield, MagicBooster, Skill, TrinketCape, TrinketBracelet, TrinketWaistItem};
    public ItemType itemType;
    public SkillsManager.SkillNames skillName;

    public enum ItemRarity { Common, Uncommon, Rare, Legendary, Masterwork}
    public ItemRarity itemRarity = ItemRarity.Common;

    public float attacksPerSecond;
    public float baseDamageScaling;

    public int itemMoveset = 0;

    public List<ItemTrait> itemTraits = new List<ItemTrait>();
    public ItemTrait.TraitType[] garenteedTraits;
    public float[] garenteedTraitValues;

    /*
    public int health;
    public int mana;
    public int armor;
    public int resistance;
    public float healthRegen;
    public float manaRegen;
    */

    //These will be put in as traits
    /*
    public float aflameResist;
    public float asleepResist;
    public float stunResist;
    public float curseResist;
    public float bleedResist;
    public float poisonResist;
    public float corrosionResist;
    public float frostbiteResist;
    public float knockbackResist;
    */

    private const float STAT_MODIFIER_RANGE = 0.25f;
    private const float LERP_STRENGTH = 0.04f;

    private void Start()
    {
        for(int index = 0; index < garenteedTraits.Length; index++)
        {
            //Debug.Log("we added a garenteed trait of: " + garenteedTraits[index]);
            AddTrait(garenteedTraits[index], garenteedTraitValues[index]);
        }
    }
    /*
    // Rerolls all the base stats plus or minus a percentage based on the stat modifier range.
    public void RollStatModifiers()
    {
        vitMod += Mathf.RoundToInt(Random.Range(-vitMod * STAT_MODIFIER_RANGE, vitMod * STAT_MODIFIER_RANGE));
        strMod += Mathf.RoundToInt(Random.Range(-strMod * STAT_MODIFIER_RANGE, strMod * STAT_MODIFIER_RANGE));
        dexMod += Mathf.RoundToInt(Random.Range(-dexMod * STAT_MODIFIER_RANGE, dexMod * STAT_MODIFIER_RANGE));
        spdMod += Mathf.RoundToInt(Random.Range(-spdMod * STAT_MODIFIER_RANGE, spdMod * STAT_MODIFIER_RANGE));
        intMod += Mathf.RoundToInt(Random.Range(-intMod * STAT_MODIFIER_RANGE, intMod * STAT_MODIFIER_RANGE));
        wisMod += Mathf.RoundToInt(Random.Range(-wisMod * STAT_MODIFIER_RANGE, wisMod * STAT_MODIFIER_RANGE));
        chaMod += Mathf.RoundToInt(Random.Range(-chaMod * STAT_MODIFIER_RANGE, chaMod * STAT_MODIFIER_RANGE));
        
        baseAttackDelay += Mathf.RoundToInt(Random.Range(-baseAttackDelay * STAT_MODIFIER_RANGE, baseAttackDelay * STAT_MODIFIER_RANGE));
        hitMax += Mathf.RoundToInt(Random.Range(-hitMax * STAT_MODIFIER_RANGE, hitMax * STAT_MODIFIER_RANGE));
        hitBase += Mathf.RoundToInt(Random.Range(-hitBase * STAT_MODIFIER_RANGE, hitBase * STAT_MODIFIER_RANGE));
        critChance += Mathf.RoundToInt(Random.Range(-critChance * STAT_MODIFIER_RANGE, critChance * STAT_MODIFIER_RANGE));
        critMod += Mathf.RoundToInt(Random.Range(-critMod * STAT_MODIFIER_RANGE, critMod * STAT_MODIFIER_RANGE));
        

        baseDamageScaling += Mathf.RoundToInt(Random.Range(-baseDamageScaling * STAT_MODIFIER_RANGE, baseDamageScaling * STAT_MODIFIER_RANGE));

        armor += Mathf.RoundToInt(Random.Range(-armor * STAT_MODIFIER_RANGE, armor * STAT_MODIFIER_RANGE));
        resistance += Mathf.RoundToInt(Random.Range(-resistance * STAT_MODIFIER_RANGE, resistance * STAT_MODIFIER_RANGE));
        healthRegen += Mathf.RoundToInt(Random.Range(-healthRegen * STAT_MODIFIER_RANGE, healthRegen * STAT_MODIFIER_RANGE));
        manaRegen += Mathf.RoundToInt(Random.Range(-manaRegen * STAT_MODIFIER_RANGE, manaRegen * STAT_MODIFIER_RANGE));
    }
*/

    // This is called when the item can be picked up. The item will set it's parent as the player's ivnentory, hide itself and disable collisions.
    public void ComfirmPickup(Transform targetParent, int index)
    {
        // GetComponent<MeshRenderer>().enabled = false;

        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer renderer in meshRenderers)
            renderer.enabled = false;

        ParticleSystem[] particles = GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem system in particles)
        {
            system.Stop();
            system.Clear();
        }

        if(GetComponentInChildren<Light>())
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

        while(currentTimer < targetTimer / 2)
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
    private void AddTrait(ItemTrait.TraitType selectedTrait, float value)
    {
        bool traitExists = false;
        foreach (ItemTrait itemTrait in itemTraits)
            if (itemTrait.traitType == selectedTrait)
            {
                itemTrait.traitBonus += value;
                traitExists = true;
            }

        if (!traitExists)
            itemTraits.Add(new ItemTrait(selectedTrait, value));
    }
    // Overload that uses a full item trait instead of components
    private void AddTrait(ItemTrait previousItemTrait)
    {
        bool traitExists = false;
        foreach (ItemTrait itemTrait in itemTraits)
            if (itemTrait.traitType == previousItemTrait.traitType)
            {
                itemTrait.traitBonus += previousItemTrait.traitBonus;
                traitExists = true;
            }

        if (!traitExists)
            itemTraits.Add(previousItemTrait);
    }

    // Used to add random traits to the item. These are added on top of any garenteed traits the item would have.
    public void AddRandomTraits()
    {
        // How many traits will this item have.
        int traitCount = 0;
        switch (itemRarity)
        {
            case ItemRarity.Common:
                traitCount = Random.Range(1, 2);
                break;
            case ItemRarity.Uncommon:
                traitCount = Random.Range(2, 3);
                break;
            case ItemRarity.Rare:
                traitCount = Random.Range(3, 5);
                break;
            case ItemRarity.Legendary:
                traitCount = Random.Range(5, 7);
                break;
            case ItemRarity.Masterwork:
                traitCount = Random.Range(7, 11);
                break;
            default:
                break;
        }
        if (itemType == ItemType.TwoHandWeapon)
            traitCount *= 2;

        for (int index = 0; index < traitCount; index++)
        {
            ItemTrait tempTrait = new ItemTrait();
            tempTrait.GetRandomTrait();
            tempTrait.GetRandomTraitValue(itemRarity);
            AddTrait(tempTrait);
        }
    }

}
