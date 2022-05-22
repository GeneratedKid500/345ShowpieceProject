using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerActionDistributor : MonoBehaviour
{
    public bool attacking;
    public bool canInput;

    private PlayerMainStateManager pmsm;
    private Transform cam;
    private Vector3 playerOrientation;

    [Header("Inputs")]
    [SerializeField] private string lightAttackButton1;
    [SerializeField] private string lightAttackButton2;
    [Space]
    [SerializeField] private string heavyAttackButton1;
    [SerializeField] private string heavyAttackButton2;
    [Space]
    [SerializeField] private string itemButton1;
    [SerializeField] private string itemButton2;
    [Space]
    [SerializeField] private string dodgeButton1;
    [SerializeField] private string dodgeButton2;

    [Header("Input List Settings")]
    [SerializeField] private float maximumTimeBetweenInputs = 0.2f;
    [SerializeField] private int maximumInputHistory = 15;
    private float timeBetweenInputs;
    private float timeUntilListErasure;
    private List<int> inputList;

    private Animator anim;
    private int comboStep = 0;

    private Weapon[] weapons;
    private int currentWeapon;

    private void Start()
    {
        pmsm = GetComponent<PlayerMainStateManager>();
        cam = Camera.main.transform;
        anim = GetComponentInChildren<Animator>();

        inputList = new List<int> { };

        weapons = Resources.LoadAll<Weapon>("");
        foreach (Weapon weapon in weapons)
        {
            weapon.InitialiseCombos();
        }
        currentWeapon = 0;
    }

    private void Update()
    {
        #region Calculate Player Orientation
        float horz = Input.GetAxisRaw("Horizontal");
        float vert = Input.GetAxisRaw("Vertical");
        playerOrientation = cam.forward * vert;
        playerOrientation = playerOrientation + cam.right * horz;
        playerOrientation.Normalize();
        playerOrientation.y = 0;
        playerOrientation = transform.InverseTransformDirection(playerOrientation);
        playerOrientation.x = (float)Math.Round(playerOrientation.x, 2);
        playerOrientation.z = (float)Math.Round(playerOrientation.z, 2);
        #endregion

        if (canInput)
        {
            #region Calculate Directional and Button Inputs
            if (playerOrientation.z > 0.8f)
            {
                AddMovementInput(1);
            }
            else if (playerOrientation.z < -0.8f)
            {
                AddMovementInput(5);
            }
            else if (playerOrientation.x < -0.8f)
            {
                AddMovementInput(7);
            }
            else if (playerOrientation.x > 0.8f)
            {
                AddMovementInput(3);
            }
            else if (playerOrientation.x > 0.5f && playerOrientation.x < 0.8f && playerOrientation.z > 0.5f && playerOrientation.z < 0.8f)
            {
                AddMovementInput(2);
            }
            else if (playerOrientation.x < -0.5f && playerOrientation.x > -0.8f && playerOrientation.z > 0.5f && playerOrientation.z < 0.8f)
            {
                AddMovementInput(8);
            }
            else if (playerOrientation.x < -0.5f && playerOrientation.x > -0.8f && playerOrientation.z < -0.5f && playerOrientation.z > -0.8f)
            {
                AddMovementInput(6);
            }
            else if (playerOrientation.x > 0.5f && playerOrientation.x < 0.8f && playerOrientation.z < -0.5f && playerOrientation.z > -0.8f)
            {
                AddMovementInput(4);
            }

            if (Input.GetButtonDown(lightAttackButton1) || Input.GetButtonDown(lightAttackButton2))
            {
                AddButtonInput(101);
            }

            if (Input.GetButtonDown(heavyAttackButton1) || Input.GetButtonDown(heavyAttackButton2))
            {
                AddButtonInput(102);
            }

            if (Input.GetButtonDown(itemButton1) || Input.GetButtonDown(itemButton2))
            {
                AddButtonInput(103);
            }

            if (Input.GetButtonDown(dodgeButton1) || Input.GetButtonDown(dodgeButton2))
            {
                AddButtonInput(104);
            }
            #endregion
        }
        else
        {
            comboStep = 0;
            attacking = false;
        }

        if (inputList.Count > 0)
        {
            timeUntilListErasure += Time.deltaTime;
            if (timeUntilListErasure >= maximumTimeBetweenInputs)
            {
                inputList.Clear();
            }
        }
    }

    void AddMovementInput(int input)
    {
        if (inputList.Count > 0 && inputList[inputList.Count - 1] == input)
        {
            timeUntilListErasure = 0;
            return;
        }

        AddInput(input);
    }

    void AddButtonInput(int input)
    {
        AddInput(input);

        string stringput = input.ToString();

        switch (input)
        {
            case 101:
                // light
            case 102:
                // heavy
                CalculateAttack(input);
                break;

            case 103:
                // item

                break;

            case 104:
                // dodge

                break;

            default: break;
        }

        if (attacking)
        {
            pmsm.EnableGenericAttackingLayer();
        }
    }

    void CalculateAttack(int input)
    {
        attacking = true;
        if (!pmsm.grounded)
        {
            if (comboStep > 1) comboStep = 0;

            anim.CrossFade(weapons[currentWeapon].airAttacks[comboStep], 0.1f);
            comboStep++;
        }
        else if (pmsm.sprinting)
        {
            switch (input)
            {
                case 101:
                    anim.CrossFade(weapons[currentWeapon].sprintLight, 0.1f);
                    break;

                case 102:
                    anim.CrossFade(weapons[currentWeapon].sprintHeavy, 0.1f);
                    break;
            }
        }
        else
        {
            switch (input)
            {
                case 101:
                    anim.CrossFade(weapons[currentWeapon].combo[comboStep], 0.1f);
                    if (comboStep >= weapons[currentWeapon].combo.Length - 1)
                    {
                        comboStep = 0;
                    }
                    else
                    {
                        comboStep++;
                    }
                    break;

                case 102:
                    if (comboStep == 0)
                    {
                        if (anim.GetCurrentAnimatorStateInfo(3).IsName(weapons[currentWeapon].weaponName + " Light Attack " + (weapons[currentWeapon].lightAttacks.Length).ToString()))
                        {
                            anim.CrossFade(weapons[currentWeapon].heavyAttacks[weapons[currentWeapon].lightAttacks.Length], 0.1f);
                        }
                        else
                        {
                            anim.CrossFade(weapons[currentWeapon].heavyAttacks[0], 0.1f);
                        }
                    }
                    else
                    {
                        string atk = weapons[currentWeapon].FindHeavyAttack(comboStep);
                        anim.CrossFade(atk, 0.1f);
                        comboStep = 0;
                    }
                    break;

                default:
                    comboStep = 0;
                    break;
            }
        }
    }

    void AddInput(int input)
    {
        timeBetweenInputs = timeUntilListErasure;
        timeUntilListErasure = 0;

        if (inputList.Count > maximumInputHistory)
        {
            inputList.RemoveAt(0);
        }
        inputList.Add(input);
    }
    
    public void DisableAttack()
    {
        attacking = false;
        comboStep = 0;
    }
}
