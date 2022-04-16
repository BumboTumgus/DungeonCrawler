using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputs : MonoBehaviour
{
    public string horizontalInput = "Horizontal";
    public string verticalInput = "Vertical";
    public string jumpInput = "Jump";
    public string rollInput = "Roll";
    public string attackInput = "Attack";
    public string menuInput = "Menu";
    public string inventoryInput = "Inventory";
    public string interactInput = "Interact";
    public string skill0Input = "Skill0";
    public string skill1Input = "Skill1";
    public string skill2Input = "Skill2";
    public string sprintInput = "Sprint";


    public bool jumpReleased = true;
    public bool rollReleased = true;
    public bool attackReleased = true;
    public bool menuReleased = true;
    public bool inventoryReleased = true;
    public bool interactReleased = true;
    public bool skill0Released = true;
    public bool skill1Released = true;
    public bool skill2Released = true;
    public bool sprintReleased = true;

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
        if (!skill0Released && Input.GetAxisRaw(skill0Input) == 0)
            skill0Released = true;
        if (!skill1Released && Input.GetAxisRaw(skill1Input) == 0)
            skill1Released = true;
        if (!skill2Released && Input.GetAxisRaw(skill2Input) == 0)
            skill2Released = true;
        if (!sprintReleased && Input.GetAxisRaw(sprintInput) == 0)
            sprintReleased = true;
    }
}
