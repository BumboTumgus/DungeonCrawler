using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDropdownGraphicInitializer : MonoBehaviour
{
    public ElementalTranspotitionUiManager manager;

    [SerializeField] private bool grabDataOnStart = true;
    [SerializeField] private Image background, itemIconBackground, itemIcon, itemIconOutline, affinityPrimary, affinityPrimaryBackground, affinitySecondary, affinitySecondaryBackground, affinitySecondarySplit, affinitySecondarySplitBackground, affinityTertiary, affinityTertiaryBackground, affintiyTertiarySplit, affintiyTertiarySplitBackground;
    [SerializeField] private Text itemName;

    [SerializeField] private Color[] itemOutlineColors;
    [SerializeField] private Color[] itemBackgroundColors;
    [SerializeField] private Color[] affinityColors;
    [SerializeField] private Color[] affinityBackgroundColors;
    [SerializeField] private Sprite[] affinityIcons;

    // Start is called before the first frame update
    void Start()
    {
        if (transform.name != "Item" && grabDataOnStart)
        {
            //Debug.Log(transform.name + " is having it's graphic initialized. It's index being used is " + transform.GetSiblingIndex() + " - 1");
            InitializeGraphic(manager.allItems[transform.GetSiblingIndex() - 1]);
        }
    }

    public void InitializeGraphic(Item myItem)
    {

        // Set up our base graphics
        if(itemName != null)
            itemName.text = myItem.itemNameModifiersPrefix + myItem.itemName + " " + myItem.itemNameAffinitySuffix;
        itemIcon.sprite = myItem.artwork;

        // Rarity based graphics
        switch (myItem.itemRarity)
        {
            case Item.ItemRarity.Common:
                if(background != null)
                    background.color = itemBackgroundColors[0];
                itemIconBackground.color = itemBackgroundColors[0];
                itemIconOutline.color = itemOutlineColors[0];
                if (background != null)
                    itemName.color = itemOutlineColors[0];
                break;
            case Item.ItemRarity.Uncommon:
                if (background != null)
                    background.color = itemBackgroundColors[1];
                itemIconBackground.color = itemBackgroundColors[1];
                itemIconOutline.color = itemOutlineColors[1];
                if (background != null)
                    itemName.color = itemOutlineColors[1];
                break;
            case Item.ItemRarity.Rare:
                if (background != null)
                    background.color = itemBackgroundColors[2];
                itemIconBackground.color = itemBackgroundColors[2];
                itemIconOutline.color = itemOutlineColors[2];
                if (background != null)
                    itemName.color = itemOutlineColors[2];
                break;
            case Item.ItemRarity.Legendary:
                if (background != null)
                    background.color = itemBackgroundColors[3];
                itemIconBackground.color = itemBackgroundColors[3];
                itemIconOutline.color = itemOutlineColors[3];
                if (background != null)
                    itemName.color = itemOutlineColors[3];
                break;
            case Item.ItemRarity.Masterwork:
                if (background != null)
                    background.color = itemBackgroundColors[4];
                itemIconBackground.color = itemBackgroundColors[4];
                itemIconOutline.color = itemOutlineColors[4];
                if (background != null)
                    itemName.color = itemOutlineColors[4];
                break;
            default:
                break;
        }

        affinityTertiaryBackground.gameObject.SetActive(true);
        affinitySecondaryBackground.gameObject.SetActive(true);

        // affinity based graphics
        switch (myItem.affinityPrimary)
        {
            case Item.AffinityType.None:
                affinityPrimaryBackground.gameObject.SetActive(false);
                break;
            case Item.AffinityType.Fire:
                affinityPrimary.sprite = affinityIcons[0];
                affinityPrimary.color = affinityColors[0];
                affinityPrimaryBackground.color = affinityBackgroundColors[0];
                break;
            case Item.AffinityType.Ice:
                affinityPrimary.sprite = affinityIcons[1];
                affinityPrimary.color = affinityColors[1];
                affinityPrimaryBackground.color = affinityBackgroundColors[1];
                break;
            case Item.AffinityType.Earth:
                affinityPrimary.sprite = affinityIcons[2];
                affinityPrimary.color = affinityColors[2];
                affinityPrimaryBackground.color = affinityBackgroundColors[2];
                break;
            case Item.AffinityType.Wind:
                affinityPrimary.sprite = affinityIcons[3];
                affinityPrimary.color = affinityColors[3];
                affinityPrimaryBackground.color = affinityBackgroundColors[3];
                break;
            case Item.AffinityType.Physical:
                affinityPrimary.sprite = affinityIcons[4];
                affinityPrimary.color = affinityColors[4];
                affinityPrimaryBackground.color = affinityBackgroundColors[4];
                break;
            case Item.AffinityType.Bleed:
                affinityPrimary.sprite = affinityIcons[5];
                affinityPrimary.color = affinityColors[5];
                affinityPrimaryBackground.color = affinityBackgroundColors[5];
                break;
            case Item.AffinityType.Poison:
                affinityPrimary.sprite = affinityIcons[6];
                affinityPrimary.color = affinityColors[6];
                affinityPrimaryBackground.color = affinityBackgroundColors[6];
                break;
            case Item.AffinityType.Stun:
                affinityPrimary.sprite = affinityIcons[7];
                affinityPrimary.color = affinityColors[7];
                affinityPrimaryBackground.color = affinityBackgroundColors[7];
                break;
            case Item.AffinityType.Knockback:
                affinityPrimary.sprite = affinityIcons[8];
                affinityPrimary.color = affinityColors[8];
                affinityPrimaryBackground.color = affinityBackgroundColors[8];
                break;
            default:
                break;
        }

        switch (myItem.affinitySecondary)
        {
            case Item.AffinityType.None:
                affinitySecondaryBackground.gameObject.SetActive(false);
                break;
            case Item.AffinityType.Fire:
                affinitySecondary.sprite = affinityIcons[0];
                affinitySecondary.color = affinityColors[0];
                affinitySecondaryBackground.color = affinityBackgroundColors[0];
                break;
            case Item.AffinityType.Ice:
                affinitySecondary.sprite = affinityIcons[1];
                affinitySecondary.color = affinityColors[1];
                affinitySecondaryBackground.color = affinityBackgroundColors[1];
                break;
            case Item.AffinityType.Earth:
                affinitySecondary.sprite = affinityIcons[2];
                affinitySecondary.color = affinityColors[2];
                affinitySecondaryBackground.color = affinityBackgroundColors[2];
                break;
            case Item.AffinityType.Wind:
                affinitySecondary.sprite = affinityIcons[3];
                affinitySecondary.color = affinityColors[3];
                affinitySecondaryBackground.color = affinityBackgroundColors[3];
                break;
            case Item.AffinityType.Physical:
                affinitySecondary.sprite = affinityIcons[4];
                affinitySecondary.color = affinityColors[4];
                affinitySecondaryBackground.color = affinityBackgroundColors[4];
                break;
            case Item.AffinityType.Bleed:
                affinitySecondary.sprite = affinityIcons[5];
                affinitySecondary.color = affinityColors[5];
                affinitySecondaryBackground.color = affinityBackgroundColors[5];
                break;
            case Item.AffinityType.Poison:
                affinitySecondary.sprite = affinityIcons[6];
                affinitySecondary.color = affinityColors[6];
                affinitySecondaryBackground.color = affinityBackgroundColors[6];
                break;
            case Item.AffinityType.Stun:
                affinitySecondary.sprite = affinityIcons[7];
                affinitySecondary.color = affinityColors[7];
                affinitySecondaryBackground.color = affinityBackgroundColors[7];
                break;
            case Item.AffinityType.Knockback:
                affinitySecondary.sprite = affinityIcons[8];
                affinitySecondary.color = affinityColors[8];
                affinitySecondaryBackground.color = affinityBackgroundColors[8];
                break;
            default:
                break;
        }

        switch (myItem.affinityTertiary)
        {
            case Item.AffinityType.None:
                affinityTertiaryBackground.gameObject.SetActive(false);
                break;
            case Item.AffinityType.Fire:
                affinityTertiary.sprite = affinityIcons[0];
                affinityTertiary.color = affinityColors[0];
                affinityTertiaryBackground.color = affinityBackgroundColors[0];
                break;
            case Item.AffinityType.Ice:
                affinityTertiary.sprite = affinityIcons[1];
                affinityTertiary.color = affinityColors[1];
                affinityTertiaryBackground.color = affinityBackgroundColors[1];
                break;
            case Item.AffinityType.Earth:
                affinityTertiary.sprite = affinityIcons[2];
                affinityTertiary.color = affinityColors[2];
                affinityTertiaryBackground.color = affinityBackgroundColors[2];
                break;
            case Item.AffinityType.Wind:
                affinityTertiary.sprite = affinityIcons[3];
                affinityTertiary.color = affinityColors[3];
                affinityTertiaryBackground.color = affinityBackgroundColors[3];
                break;
            case Item.AffinityType.Physical:
                affinityTertiary.sprite = affinityIcons[4];
                affinityTertiary.color = affinityColors[4];
                affinityTertiaryBackground.color = affinityBackgroundColors[4];
                break;
            case Item.AffinityType.Bleed:
                affinityTertiary.sprite = affinityIcons[5];
                affinityTertiary.color = affinityColors[5];
                affinityTertiaryBackground.color = affinityBackgroundColors[5];
                break;
            case Item.AffinityType.Poison:
                affinityTertiary.sprite = affinityIcons[6];
                affinityTertiary.color = affinityColors[6];
                affinityTertiaryBackground.color = affinityBackgroundColors[6];
                break;
            case Item.AffinityType.Stun:
                affinityTertiary.sprite = affinityIcons[7];
                affinityTertiary.color = affinityColors[7];
                affinityTertiaryBackground.color = affinityBackgroundColors[7];
                break;
            case Item.AffinityType.Knockback:
                affinityTertiary.sprite = affinityIcons[8];
                affinityTertiary.color = affinityColors[8];
                affinityTertiaryBackground.color = affinityBackgroundColors[8];
                break;
            default:
                break;
        }

        if (myItem.affinitySecondaryMultiElement)
        {
            affinitySecondarySplitBackground.gameObject.SetActive(true);
            switch (myItem.affinityPrimary)
            {
                case Item.AffinityType.Fire:
                    affinitySecondarySplit.sprite = affinityIcons[0];
                    affinitySecondarySplit.color = affinityColors[0];
                    affinitySecondarySplitBackground.color = affinityBackgroundColors[0];
                    break;
                case Item.AffinityType.Ice:
                    affinitySecondarySplit.sprite = affinityIcons[1];
                    affinitySecondarySplit.color = affinityColors[1];
                    affinitySecondarySplitBackground.color = affinityBackgroundColors[1];
                    break;
                case Item.AffinityType.Earth:
                    affinitySecondarySplit.sprite = affinityIcons[2];
                    affinitySecondarySplit.color = affinityColors[2];
                    affinitySecondarySplitBackground.color = affinityBackgroundColors[2];
                    break;
                case Item.AffinityType.Wind:
                    affinitySecondarySplit.sprite = affinityIcons[3];
                    affinitySecondarySplit.color = affinityColors[3];
                    affinitySecondarySplitBackground.color = affinityBackgroundColors[3];
                    break;
                case Item.AffinityType.Physical:
                    affinitySecondarySplit.sprite = affinityIcons[4];
                    affinitySecondarySplit.color = affinityColors[4];
                    affinitySecondarySplitBackground.color = affinityBackgroundColors[4];
                    break;
                case Item.AffinityType.Bleed:
                    affinitySecondarySplit.sprite = affinityIcons[5];
                    affinitySecondarySplit.color = affinityColors[5];
                    affinitySecondarySplitBackground.color = affinityBackgroundColors[5];
                    break;
                case Item.AffinityType.Poison:
                    affinitySecondarySplit.sprite = affinityIcons[6];
                    affinitySecondarySplit.color = affinityColors[6];
                    affinitySecondarySplitBackground.color = affinityBackgroundColors[6];
                    break;
                case Item.AffinityType.Stun:
                    affinitySecondarySplit.sprite = affinityIcons[7];
                    affinitySecondarySplit.color = affinityColors[7];
                    affinitySecondarySplitBackground.color = affinityBackgroundColors[7];
                    break;
                case Item.AffinityType.Knockback:
                    affinitySecondarySplit.sprite = affinityIcons[8];
                    affinitySecondarySplit.color = affinityColors[8];
                    affinitySecondarySplitBackground.color = affinityBackgroundColors[8];
                    break;
                default:
                    break;
            }
        }
        else
            affinitySecondarySplitBackground.gameObject.SetActive(false);


        if (myItem.affinityTertiaryMultiElement)
        {
            affintiyTertiarySplitBackground.gameObject.SetActive(true);
            switch (myItem.affinityPrimary)
            {
                case Item.AffinityType.Fire:
                    affintiyTertiarySplit.sprite = affinityIcons[0];
                    affintiyTertiarySplit.color = affinityColors[0];
                    affintiyTertiarySplitBackground.color = affinityBackgroundColors[0];
                    break;
                case Item.AffinityType.Ice:
                    affintiyTertiarySplit.sprite = affinityIcons[1];
                    affintiyTertiarySplit.color = affinityColors[1];
                    affintiyTertiarySplitBackground.color = affinityBackgroundColors[1];
                    break;
                case Item.AffinityType.Earth:
                    affintiyTertiarySplit.sprite = affinityIcons[2];
                    affintiyTertiarySplit.color = affinityColors[2];
                    affintiyTertiarySplitBackground.color = affinityBackgroundColors[2];
                    break;
                case Item.AffinityType.Wind:
                    affintiyTertiarySplit.sprite = affinityIcons[3];
                    affintiyTertiarySplit.color = affinityColors[3];
                    affintiyTertiarySplitBackground.color = affinityBackgroundColors[3];
                    break;
                case Item.AffinityType.Physical:
                    affintiyTertiarySplit.sprite = affinityIcons[4];
                    affintiyTertiarySplit.color = affinityColors[4];
                    affintiyTertiarySplitBackground.color = affinityBackgroundColors[4];
                    break;
                case Item.AffinityType.Bleed:
                    affintiyTertiarySplit.sprite = affinityIcons[5];
                    affintiyTertiarySplit.color = affinityColors[5];
                    affintiyTertiarySplitBackground.color = affinityBackgroundColors[5];
                    break;
                case Item.AffinityType.Poison:
                    affintiyTertiarySplit.sprite = affinityIcons[6];
                    affintiyTertiarySplit.color = affinityColors[6];
                    affintiyTertiarySplitBackground.color = affinityBackgroundColors[6];
                    break;
                case Item.AffinityType.Stun:
                    affintiyTertiarySplit.sprite = affinityIcons[7];
                    affintiyTertiarySplit.color = affinityColors[7];
                    affintiyTertiarySplitBackground.color = affinityBackgroundColors[7];
                    break;
                case Item.AffinityType.Knockback:
                    affintiyTertiarySplit.sprite = affinityIcons[8];
                    affintiyTertiarySplit.color = affinityColors[8];
                    affintiyTertiarySplitBackground.color = affinityBackgroundColors[8];
                    break;
                default:
                    break;
            }
        }
        else
            affintiyTertiarySplitBackground.gameObject.SetActive(false);
    }
}
