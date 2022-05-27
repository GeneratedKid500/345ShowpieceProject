using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerActionDistributor : MonoBehaviour
{
    public bool attacking;
    public bool canInput;

    private PlayerHealthBarManager healthBarManager;
    private PlayerMainStateManager pmsm;
    private WeaponLoader weaponLoader;
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
    [Space]
    [SerializeField] private string swapWeapon1;
    [SerializeField] private string swapWeapon2;


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

    private bool weapon2 = false;
    private bool weapon3 = false;

    bool waitingForTransition = false;
    string animationToTransitionInto = null;

    private void Start()
    {
        healthBarManager = GameObject.Find("PlayerHealthCanvas").GetComponent<PlayerHealthBarManager>();
        pmsm = GetComponent<PlayerMainStateManager>();
        cam = Camera.main.transform;
        anim = GetComponentInChildren<Animator>();
        weaponLoader = GetComponent<WeaponLoader>();

        inputList = new List<int> { };

        weapons = Resources.LoadAll<Weapon>("");
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

            if (Input.GetButtonDown(swapWeapon1) || Input.GetButtonDown(swapWeapon2))
            {
                AddButtonInput(105);
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

        if (pmsm.grounded)
        {
            if (waitingForTransition && anim.GetCurrentAnimatorStateInfo(3).normalizedTime > 0.9f)
            {
                anim.CrossFade(animationToTransitionInto, 0.15f);
                waitingForTransition = false;
            }
        }
        else
        {
            if (waitingForTransition && anim.GetCurrentAnimatorStateInfo(4).normalizedTime > 0.8f)
            {
                anim.CrossFade(animationToTransitionInto, 0.1f);
                waitingForTransition = false;
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

            case 105:
                // weapon switch
                SwitchWeapon();
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
        if (waitingForTransition) return;

        attacking = true;
        waitingForTransition = true;
        if (!pmsm.grounded)
        {
            if (comboStep > 1) comboStep = 0;

            animationToTransitionInto = weapons[currentWeapon].airAttacks[comboStep];

            comboStep++;
        }
        else if (pmsm.sprinting)
        {
            switch (input)
            {
                case 101:
                    animationToTransitionInto = weapons[currentWeapon].sprintLight;
                    break;

                case 102:
                    animationToTransitionInto = weapons[currentWeapon].sprintHeavy;
                    break;
            }
        }
        else
        {
            switch (input)
            {
                case 101:
                    Debug.Log(weapons[currentWeapon].combo[comboStep]);
                    animationToTransitionInto = weapons[currentWeapon].combo[comboStep];
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
                        if (anim.GetCurrentAnimatorStateInfo(3).IsName(weapons[currentWeapon].weaponType + " Light Attack " + (weapons[currentWeapon].lightAttacks.Length).ToString()))
                        {
                            animationToTransitionInto = weapons[currentWeapon].heavyAttacks[weapons[currentWeapon].lightAttacks.Length];
                        }
                        else
                        {
                            animationToTransitionInto = weapons[currentWeapon].heavyAttacks[0];
                        }
                    }
                    else
                    {
                        animationToTransitionInto = weapons[currentWeapon].FindHeavyAttack(comboStep);
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
    
    void SwitchWeapon()
    {
        if (attacking) return;

        switch (currentWeapon) 
        {
            case 0:
                if (weapon2)
                {
                    currentWeapon = 1;
                    healthBarManager.AlterTextShowButton("Axe");
                }
                else if (weapon3)
                {
                    currentWeapon = 2;
                }
                break;

            case 1:
                if (weapon3)
                {
                    currentWeapon = 2;
                }
                else
                {
                    currentWeapon = 0;
                    healthBarManager.AlterTextShowButton("Greatsword");
                }
                break;

            case 2:
                currentWeapon = 0;
                break;
        }
        weaponLoader.SwapWeapons(currentWeapon);

    }

    public void ActivateWeapon(int i)
    {
        if (i == 2)
        {
            weapon2 = true;
        }
        else weapon3 = true;
    }

    public void DisableAttack()
    {
        attacking = false;
        comboStep = 0;
    }
}
