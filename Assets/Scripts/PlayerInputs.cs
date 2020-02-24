using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputs : MonoBehaviour
{
    public string horizontalInput = "Horizontal";
    public string verticalInput = "Vertical";
    public string jumpInput = "Jump";
    public string rollInput = "Roll";
    public string sprintInput = "Sprint";
    public string attackInput = "Attack";
    public string menuInput = "Menu";
    public string inventoryInput = "Inventory";
    public string interactInput = "Interact";

    public bool jumpReleased = true;
    public bool rollReleased = true;
    public bool attackReleased = true;
    public bool menuReleased = true;
    public bool inventoryReleased = true;
    public bool interactReleased = true;

    private void Update()
    {
        if (!jumpReleased && Input.GetAxisRaw(jumpInput) == 0)
            jumpReleased = true;
        if (!rollReleased && Input.GetAxisRaw(rollInput) == 0)
            rollReleased = true;
        if (!attackReleased && Input.GetAxisRaw(attackInput) == 0)
            attackReleased = true;
        if (!menuReleased && Input.GetAxisRaw(menuInput) == 0)
            menuReleased = true;
        if (!inventoryReleased && Input.GetAxisRaw(inventoryInput) == 0)
            inventoryReleased = true;
        if (!interactReleased && Input.GetAxisRaw(interactInput) == 0)
            interactReleased = true;
    }
}
