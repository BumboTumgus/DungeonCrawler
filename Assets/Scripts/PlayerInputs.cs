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
    public string skill0Input = "Skill0";
    public string skill1Input = "Skill1";
    public string skill2Input = "Skill2";
    public string skill3Input = "Skill3";
    public string skill4Input = "Skill4";
    public string skill5Input = "Skill5";
    public string skill6Input = "Skill6";
    public string skill7Input = "Skill7";


    public bool jumpReleased = true;
    public bool rollReleased = true;
    public bool attackReleased = true;
    public bool menuReleased = true;
    public bool inventoryReleased = true;
    public bool interactReleased = true;
    public bool skill0Released = true;
    public bool skill1Released = true;
    public bool skill2Released = true;
    public bool skill3Released = true;
    public bool skill4Released = true;
    public bool skill5Released = true;
    public bool skill6Released = true;
    public bool skill7Released = true;

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
        if (!skill3Released && Input.GetAxisRaw(skill3Input) == 0)
            skill3Released = true;
        if (!skill4Released && Input.GetAxisRaw(skill4Input) == 0)
            skill4Released = true;
        if (!skill5Released && Input.GetAxisRaw(skill5Input) == 0)
            skill5Released = true;
        if (!skill6Released && Input.GetAxisRaw(skill6Input) == 0)
            skill6Released = true;
        if (!skill7Released && Input.GetAxisRaw(skill7Input) == 0)
            skill7Released = true;
    }
}
