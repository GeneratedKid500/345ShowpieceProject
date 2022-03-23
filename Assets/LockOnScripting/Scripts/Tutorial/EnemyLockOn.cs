using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class EnemyLockOn : MonoBehaviour
{
    public float range;
    public Transform emptyTarget;

    public CinemachineTargetGroup group;
    public int enemyCount;
    public List<Transform> enemiesToLock;
    public Transform closestEnemy;
    public Transform selectedEnemy;
    public Transform priorityEnemy;
    public bool foundPriorityEnemy;

    public Vector3 scaleIncrement;
    public float xScaleMax;
    public float zScaleMax;
    public GameObject targetingCone;
    public GameObject targetingConePivot;
    public Transform coneHolder;
    private Vector3 selectorDirection;
    private bool parentChangeInitialisationPerformed;

    public bool lockedOn;
    private bool temp;

    private PlayerMainStateManager cam;
    private LockOnMovement characterMovement;
    private TargetingConeTrigger coneTrigger;

    private void Awake()
    {
        cam = GetComponent<PlayerMainStateManager>();
        characterMovement = GetComponent<LockOnMovement>();
        coneTrigger = GetComponentInChildren<TargetingConeTrigger>();
    }

    void Update()
    {
        if (Input.GetButton("R1") || Input.GetKey(KeyCode.F)) temp = true;
        else temp = false;

        if (temp)
        {
            RunEnemySearchSphereCollider();
        }
        else
        {
            enemiesToLock.Clear();
            cam.lockedOn = false;
        }

        enemyCount = enemiesToLock.Count;

        if (enemyCount == 0 || priorityEnemy == null)
        {
            InitialiseTargetGroup();
            cam.lockedOn = false;
            foundPriorityEnemy = false;

            closestEnemy = null;
            selectedEnemy = null;
            priorityEnemy = null;
            InitialiseConeParent();
            ResetTargetingCone();
            lockedOn = false;
        }

        if (enemyCount != 0)
        {
            cam.lockedOn = true;
            FindClosestEnemy();
            if (closestEnemy != null && foundPriorityEnemy == false) SetPriorityEnemy(closestEnemy);
            SwitchTarget();
            if (selectedEnemy != null) SetPriorityEnemy(selectedEnemy);
            if (priorityEnemy != null) BuildTargetGroup();
            lockedOn = true;
        }
    }

    private void RunEnemySearchSphereCollider()
    {
        Collider[] enemyDetect = Physics.OverlapSphere(transform.position, range);
        enemiesToLock = new List<Transform>();
        foreach(Collider col in enemyDetect)
        {
            Targetable enemy = col.GetComponent<Targetable>();
            if (enemy != null) enemiesToLock.Add(col.transform);
        }
    }
    private void FindClosestEnemy()
    {
        float closest = range;
        closestEnemy = null;
        for (int i = 0; i < enemyCount; i++)
        {
            float distanceToPlayer = Vector3.Distance(enemiesToLock[i].position, transform.position);
            if (distanceToPlayer < closest)
            {
                closest = distanceToPlayer;
                closestEnemy = enemiesToLock[i];
            }
        }
    }
    private void SetPriorityEnemy(Transform pEnemy)
    {
        priorityEnemy = pEnemy;
        foundPriorityEnemy = true;
        SetConeParentToTarget();
    }
    private void BuildTargetGroup()
    {
        CinemachineTargetGroup.Target enemy;
        enemy.target = priorityEnemy;
        enemy.weight = priorityEnemy.GetComponent<Targetable>().camWeight;
        enemy.radius = priorityEnemy.GetComponent<Targetable>().camRadius;

        group.m_Targets[1].target = enemy.target;
        group.m_Targets[1].weight = enemy.weight;
        group.m_Targets[1].radius = enemy.radius;
    }
    private void InitialiseTargetGroup() 
    {
        CinemachineTargetGroup.Target defaultTarget;
        defaultTarget.target = emptyTarget;
        defaultTarget.weight = emptyTarget.GetComponent<EmptyTargetData>().camWeight;
        defaultTarget.radius = emptyTarget.GetComponent<EmptyTargetData>().camRadius;

        group.m_Targets[1].target = defaultTarget.target;
        group.m_Targets[1].weight = defaultTarget.weight;
        group.m_Targets[1].radius = defaultTarget.radius;
    }
    private void SwitchTarget()
    {
        if (SubVertical() == 0 && SubHorizontal() == 0) ResetTargetingCone();
        else
        {
            selectorDirection = ((characterMovement.camForward * SubVertical()) + (characterMovement.camRight * SubHorizontal())).normalized;
            targetingConePivot.transform.rotation = Quaternion.LookRotation(selectorDirection);
            targetingCone.SetActive(true);

            if (coneTrigger.selectedEnemy != null && coneTrigger.selectedEnemy != priorityEnemy)
            {
                parentChangeInitialisationPerformed = false;
                selectedEnemy = coneTrigger.selectedEnemy.transform;
            }
            else
            {
                if (targetingCone.transform.localScale.y <= range) targetingCone.transform.localScale += new Vector3(0, scaleIncrement.y, 0);
                if (targetingCone.transform.localScale.x <= xScaleMax) targetingCone.transform.localScale += new Vector3(scaleIncrement.x, 0, 0);
                if (targetingCone.transform.localScale.z <= zScaleMax) targetingCone.transform.localScale += new Vector3(0, 0, scaleIncrement.z);
            }
        }
    }
    private void SetConeParentToTarget()
    {
        targetingConePivot.transform.SetParent(priorityEnemy);
        if (!parentChangeInitialisationPerformed)
        {
            targetingConePivot.transform.localPosition = Vector3.zero;
            targetingCone.transform.localScale = new Vector3(coneTrigger.coneScale.x, coneTrigger.coneScale.y, coneTrigger.coneScale.z);
            parentChangeInitialisationPerformed = true;
        }
    }
    private void InitialiseConeParent()
    {
        targetingConePivot.transform.SetParent(coneHolder);
        targetingConePivot.transform.localPosition = Vector3.zero;
        targetingConePivot.transform.localRotation = Quaternion.identity;
        parentChangeInitialisationPerformed = false;
    }
    private void ResetTargetingCone()
    {
        coneTrigger.selectedEnemy = null;
        targetingCone.SetActive(false);
        targetingConePivot.transform.rotation = transform.rotation;
        targetingCone.transform.localScale = new Vector3(coneTrigger.coneScale.x, coneTrigger.coneScale.y, coneTrigger.coneScale.z);
    }

    private float SubHorizontal()
    {
        float x = 0.0f;
        x += Input.GetAxis("Mouse X");
        return Mathf.Clamp(x, -1.0f, 1.0f);
    }
    private float SubVertical()
    {
        float y = 0.0f;
        y += Input.GetAxis("Mouse Y");
        return Mathf.Clamp(y, -1.0f, 1.0f);
    }
}
