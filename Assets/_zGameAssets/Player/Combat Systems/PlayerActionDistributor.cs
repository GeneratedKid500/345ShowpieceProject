using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerActionDistributor : MonoBehaviour
{
    private Transform cam;
    private Vector3 playerOrientation;

    [Header("Inputs")]
    [SerializeField] private string attackButtonA1;
    [SerializeField] private string attackButtonA2;

    [SerializeField] private string attackButtonB1;
    [SerializeField] private string attackButtonB2;

    [SerializeField] private string attackButtonC1;
    [SerializeField] private string attackButtonC2;

    [Header("Input List Settings")]
    [SerializeField] private float minimumTimeBetweenInputs = 0.005f;
    [SerializeField] private float maximumTimeBetweenInputs = 0.2f;
    [SerializeField] private int maximumInputHistory = 15;
    private int lastButtonInput;
    private int buttonCombo;
    private float timeSinceLastInput;
    private float timeBetweenInputs;
    private List<int> inputList;

    private Animator anim;
    private AttackSO[] comboAttackRegister;
    private AttackSO[] inputAttackRegister;

    private void Start()
    {
        cam = Camera.main.transform;

        inputList = new List<int> { };

        anim = GetComponentInChildren<Animator>();

        AttackSO[] fullAttackArray = Resources.LoadAll<AttackSO>("");
        List<AttackSO> tempComboAttackList =  new List<AttackSO>();
        List<AttackSO> tempInputAttackList =  new List<AttackSO>();

        for (int i = 0; i < fullAttackArray.Length; i++)
        {
            if (fullAttackArray[i].attackAnimatorReferences.Length > 1)
            {
                tempComboAttackList.Add(fullAttackArray[i]);
            }
            else
            {
                tempInputAttackList.Add(fullAttackArray[i]);
            }
        }

        comboAttackRegister = tempComboAttackList.ToArray();
        inputAttackRegister = tempInputAttackList.ToArray();
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

        if (Input.GetButtonDown(attackButtonA1) || Input.GetButtonDown(attackButtonA2))
        {
            AddButtonInput(101);
        }

        if (Input.GetButtonDown(attackButtonB1) || Input.GetButtonDown(attackButtonB2))
        {
            AddButtonInput(102);
        }

        if (Input.GetButtonDown(attackButtonC1) || Input.GetButtonDown(attackButtonC2))
        {
            AddButtonInput(103);
        }
        #endregion

        if (inputList.Count > 0)
        {
            timeSinceLastInput += Time.deltaTime;
            if (timeSinceLastInput >= maximumTimeBetweenInputs)
            {
                inputList.Clear();
            }
        }

        if (inputList.Count > 0 && inputList[0] == comboAttackRegister[0].attackInputs[buttonCombo])
        {
            anim.CrossFade(comboAttackRegister[0].attackAnimatorReferences[buttonCombo], 0.01f);
        }
    }

    void AddMovementInput(int input)
    {
        if (inputList.Count > 0 && inputList[inputList.Count - 1] == input)
        {
            timeSinceLastInput = 0;
            return;
        }

        AddInput(input);
    }

    void AddButtonInput(int input)
    {
        if (input == lastButtonInput)
        {
            buttonCombo += 1;
        }
        else
        {
            lastButtonInput = input;
            buttonCombo = 0;
        }

        AddInput(input);
    }

    void AddInput(int input)
    {
        //Debug.Log(input);

        timeBetweenInputs = timeSinceLastInput;
        timeSinceLastInput = 0;

        if (inputList.Count > maximumInputHistory)
        {
            inputList.RemoveAt(0);
        }
        inputList.Add(input);
    }

}
