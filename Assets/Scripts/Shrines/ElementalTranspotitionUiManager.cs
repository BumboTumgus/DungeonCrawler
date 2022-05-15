using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElementalTranspotitionUiManager : MonoBehaviour
{
    public Inventory inventory;
    public PlayerStats connectedPlayerStats;
    public List<Item> allItems = new List<Item>();
    public ShrineBehaviour_ElementalFountain connectedShrine;
    private Item chosenItem;

    [SerializeField] Transform outerWheel;
    [SerializeField] Transform innerWheel;
    [SerializeField] Dropdown gearDropdown;
    [SerializeField] ItemDropdownGraphicInitializer itemIcon;

    Item.AffinityType primaryAffinity = Item.AffinityType.Fire;
    Item.AffinityType secondaryAffinity = Item.AffinityType.Fire;

    IEnumerator innerRingSpin;
    IEnumerator outerRingSpin;

    private const float WHEEL_SPIN_SPEED = 0.05f;
    private const float WHEEL_SNAP_ANGLE = 0.01f;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    public void OnOpen(ShrineBehaviour_ElementalFountain shrine)
    {
        connectedShrine = shrine;

        // Populate the dropdown here.
        gearDropdown.ClearOptions();

        allItems = inventory.GetListOfAllItems();

        Debug.Log("all the items were aded to a list. Currently there are: " + allItems.Count);
        List<string> options = new List<string>();
        for (int index = 0; index < allItems.Count; index++)
            options.Add(allItems[index].itemName);
        gearDropdown.AddOptions(options);
        gearDropdown.value = 0;
        SetChosenItem(0);
    }

    public void OnClose()
    {
        connectedShrine = null;
    }

    public void OnButtonPressOuterRing(int affinityIndex)
    {
        Item.AffinityType affinity = GetAffinityFromIndex(affinityIndex);
        bool affinityGoodWithOtherAffinity = true;
        switch (affinity)
        {
            case Item.AffinityType.Fire:
                if (secondaryAffinity == Item.AffinityType.Ice)
                    affinityGoodWithOtherAffinity = false;
                break;
            case Item.AffinityType.Ice:
                if (secondaryAffinity == Item.AffinityType.Fire)
                    affinityGoodWithOtherAffinity = false;
                break;
            case Item.AffinityType.Earth:
                if (secondaryAffinity == Item.AffinityType.Wind)
                    affinityGoodWithOtherAffinity = false;
                break;
            case Item.AffinityType.Wind:
                if (secondaryAffinity == Item.AffinityType.Earth)
                    affinityGoodWithOtherAffinity = false;
                break;
            default:
                break;
        }

        if (affinity == primaryAffinity || !affinityGoodWithOtherAffinity)
            return;

        primaryAffinity = affinity;

        SpinOuterRing(affinity);
    }

    public void OnButtonPressInnerRing(int affinityIndex)
    {
        Item.AffinityType affinity = GetAffinityFromIndex(affinityIndex);
        bool affinityGoodWithOtherAffinity = true;
        switch (affinity)
        {
            case Item.AffinityType.Fire:
                if (primaryAffinity == Item.AffinityType.Ice)
                    affinityGoodWithOtherAffinity = false;
                break;
            case Item.AffinityType.Ice:
                if (primaryAffinity == Item.AffinityType.Fire)
                    affinityGoodWithOtherAffinity = false;
                break;
            case Item.AffinityType.Earth:
                if (primaryAffinity == Item.AffinityType.Wind)
                    affinityGoodWithOtherAffinity = false;
                break;
            case Item.AffinityType.Wind:
                if (primaryAffinity == Item.AffinityType.Earth)
                    affinityGoodWithOtherAffinity = false;
                break;
            default:
                break;
        }

        if (affinity == secondaryAffinity || !affinityGoodWithOtherAffinity)
            return;

        secondaryAffinity = affinity;

        SpinInnerRing(secondaryAffinity);
    }

    public void OnButtonPressTranspose()
    {
        bool itemEquipped = false;

        foreach (Item weapon in inventory.weapons)
            if (chosenItem == weapon)
                itemEquipped = true;
        foreach (Item trinket in inventory.trinkets)
            if (chosenItem == trinket)
                itemEquipped = true;
        if (chosenItem == inventory.armor)
            itemEquipped = true;
        if (chosenItem == inventory.leggings)
            itemEquipped = true;
        if (chosenItem == inventory.helmet)
            itemEquipped = true;

        // If the item is equipped, uneqiup it.
        if (itemEquipped)
            connectedPlayerStats.RemoveItemStats(chosenItem, true, false);

        Debug.Log("The item we are transposing is " + chosenItem.itemName);
        // reroll the item to the selected affinities.
        chosenItem.RollItemTraitsAffinityAndModifiers(primaryAffinity, secondaryAffinity);

        if (itemEquipped)
        {
            connectedPlayerStats.AddItemStats(chosenItem, true, false);
            // Update the slot of equipment this item is in.
            inventory.inventoryUI.UpdateEquipmentSlot(chosenItem);
        }
        else
            inventory.inventoryUI.UpdateInventorySlot(chosenItem);

        // Interact logic for the shrine.
        connectedShrine.OnInteract(primaryAffinity, secondaryAffinity);
        connectedShrine = null;

        // Hide the menu.
        transform.parent.GetComponent<PauseMenuController>().HideElementalTranspositionPanel();
    }

    private void SpinOuterRing(Item.AffinityType affinity)
    {
        if (outerRingSpin != null)
            StopCoroutine(outerRingSpin);
        outerRingSpin = OuterRingSpinToTarget(GetTargetValueBasedAffinity(affinity));
        StartCoroutine(outerRingSpin);
    }

    private void SpinInnerRing(Item.AffinityType affinity)
    {
        if (innerRingSpin != null)
            StopCoroutine(innerRingSpin);
        innerRingSpin = InnerRingSpinToTarget(GetTargetValueBasedAffinity(affinity));
        StartCoroutine(innerRingSpin);
    }

    private float GetTargetValueBasedAffinity(Item.AffinityType chosenAffinity)
    {
        float value = 0;

        switch (chosenAffinity)
        {
            case Item.AffinityType.Fire:
                value = 0;
                break;
            case Item.AffinityType.Ice:
                value = 240;
                break;
            case Item.AffinityType.Earth:
                value = 280;
                break;
            case Item.AffinityType.Wind:
                value = 40;
                break;
            case Item.AffinityType.Physical:
                value = 200;
                break;
            case Item.AffinityType.Bleed:
                value = 80;
                break;
            case Item.AffinityType.Poison:
                value = 320;
                break;
            case Item.AffinityType.Stun:
                value = 160;
                break;
            case Item.AffinityType.Knockback:
                value = 120;
                break;
            default:
                break;
        }

        return value;
    }

    private Item.AffinityType GetAffinityFromIndex(int index)
    {
        Item.AffinityType affinity = Item.AffinityType.None;

        switch (index)
        {
            case 0:
                affinity = Item.AffinityType.Fire;
                break;
            case 1:
                affinity = Item.AffinityType.Wind;
                break;
            case 2:
                affinity = Item.AffinityType.Bleed;
                break;
            case 3:
                affinity = Item.AffinityType.Knockback;
                break;
            case 4:
                affinity = Item.AffinityType.Stun;
                break;
            case 5:
                affinity = Item.AffinityType.Physical;
                break;
            case 6:
                affinity = Item.AffinityType.Ice;
                break;
            case 7:
                affinity = Item.AffinityType.Earth;
                break;
            case 8:
                affinity = Item.AffinityType.Poison;
                break;
            default:
                break;
        }

        return affinity;
    }

    IEnumerator OuterRingSpinToTarget(float targetSpin)
    {
        float currentSpin = outerWheel.rotation.eulerAngles.z;

        while(currentSpin != targetSpin)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                break;
            float oldvalue = currentSpin;
            currentSpin = Mathf.LerpAngle(currentSpin, targetSpin, WHEEL_SPIN_SPEED);

            outerWheel.transform.rotation = Quaternion.Euler(new Vector3(0, 0, currentSpin));

            if (Mathf.Abs(currentSpin - oldvalue) <= WHEEL_SNAP_ANGLE)
                currentSpin = targetSpin;

            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator InnerRingSpinToTarget(float targetSpin)
    {
        float currentSpin = innerWheel.rotation.eulerAngles.z;

        while (currentSpin != targetSpin)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                break;
            float oldvalue = currentSpin;
            currentSpin = Mathf.LerpAngle(currentSpin, targetSpin, WHEEL_SPIN_SPEED);

            innerWheel.transform.rotation = Quaternion.Euler(new Vector3(0, 0, currentSpin));

            if (Mathf.Abs(currentSpin - oldvalue) <= WHEEL_SNAP_ANGLE)
                currentSpin = targetSpin;

            yield return new WaitForEndOfFrame();
        }
    }

    public void SetChosenItem(int index)
    {
        chosenItem = allItems[index];
        Debug.Log("The chosen item is " + chosenItem.itemName);

        itemIcon.InitializeGraphic(chosenItem);
        SpinOuterRing(chosenItem.affinityPrimary);

        if (chosenItem.affinitySecondary != Item.AffinityType.None)
            SpinInnerRing(chosenItem.affinitySecondary);
        else
            SpinInnerRing(chosenItem.affinityPrimary);
    }
}
