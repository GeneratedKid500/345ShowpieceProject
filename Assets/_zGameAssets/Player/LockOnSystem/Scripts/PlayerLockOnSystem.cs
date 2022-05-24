using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerLockOnSystem : MonoBehaviour
{
    [Header("Inputs")]
    public string lockOnInputA;
    public string lockOnInputB;
    [Space]
    public string changeTargetInputA;
    public string changeTargetInputB;
    private bool awaitTargetChange;

    [Header("Objects")]
    public Transform cameraTarget;
    private CameraTargetDetatch ctd;
    public Targetable emptyTarget;
    public LayerMask targetableLayer;

    [Header("Values")]
    public bool toggleMode;
    private bool lockOnPressed;
    public int range;

    private CinemachineTargetGroup targetGroup;
    private List<Targetable> targetsToLock;
    private int targetCount;
    private Targetable closestTarget;
    private Targetable selectedTarget;
    private int selectedTargetIndex;
    private Targetable priorityTarget;

    private PlayerMainStateManager cam;

    private void Awake()
    {
        cam = GetComponent<PlayerMainStateManager>();
        ctd = GetComponentInChildren<CameraTargetDetatch>();
        targetGroup = GetComponentInChildren<CinemachineTargetGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        InputGrabber();

        if (lockOnPressed)
        {
            if (targetCount > 0)
            {
                if (closestTarget == null)
                {
                    FindClosestTarget();
                }

                if (selectedTarget == null && priorityTarget == null || selectedTarget == emptyTarget && priorityTarget == null || !targetsToLock.Contains(selectedTarget))
                {
                    FindClosestTarget();
                    ChangeTarget(closestTarget);
                }
                else if (priorityTarget != null && selectedTarget == null)
                {
                    ChangeTarget(priorityTarget);
                }

                if (awaitTargetChange)
                {
                    ChangeTarget();
                }
            }
            else
            {
                AssignTargetValues(emptyTarget);
            }
        }
        else
        {
            if (targetGroup.m_Targets[1].target != emptyTarget.transform || emptyTarget == selectedTarget)
            {
                AssignTargetValues(emptyTarget);
                closestTarget = null;
                selectedTarget = null;
                selectedTargetIndex = 0;
            }
        }
    }

    private void FixedUpdate()
    {
        CheckForEnemies();
    }

    void InputGrabber()
    {
        if (toggleMode)
        {
            if (Input.GetButtonDown(lockOnInputA) || Input.GetButtonDown(lockOnInputB))
            {
                lockOnPressed = !lockOnPressed;
            }
        }
        else
        {
            if (Input.GetButton(lockOnInputA) || Input.GetButton(lockOnInputB))
            {
                lockOnPressed = true;
            }
            else
            {
                lockOnPressed = false;
            }
        }
        cam.lockedOn = lockOnPressed;

        if (lockOnPressed)
        {
            if (Input.GetButtonDown(changeTargetInputA) || Input.GetButtonDown(changeTargetInputB))
            {
                awaitTargetChange = true;
            }
        }
    }

    void CheckForEnemies()
    {
        targetsToLock = new List<Targetable>();
        foreach (Collider col in Physics.OverlapSphere(transform.position, range, targetableLayer))
        {
            Targetable enemy = col.GetComponent<Targetable>();
            if (enemy != null)
            {
                Vector3 dirToCol = transform.position - col.transform.position;
                //if (Physics.Raycast(transform.position, dirToCol))
                {
                    targetsToLock.Add(enemy);
                }
            }
        }
        targetCount = targetsToLock.Count;
    }

    void FindClosestTarget()
    {
        float closest = range;
        closestTarget = null;
        for (int i = 0; i < targetCount; i++)
        {
            float distanceToPlayer = Vector3.Distance(targetsToLock[i].transform.position, transform.position);
            if (distanceToPlayer < closest)
            {
                closest = distanceToPlayer;
                closestTarget = targetsToLock[i];
            }
        }
    }

    void ChangeTarget(int indexOverride = -1)
    {
        awaitTargetChange = false;

        if (targetCount < 2) return;

        if (indexOverride != -1)
        {
            selectedTargetIndex = indexOverride;
        }
        else
        {
            selectedTargetIndex += 1;
        }

        if (selectedTargetIndex > targetCount - 1)
        {
            selectedTargetIndex = 0;
        }

        AssignTargetValues(targetsToLock[selectedTargetIndex]);
    }
    void ChangeTarget(Targetable targetOverride)
    {
        for (int i = 0; i < targetCount; i++)
        {
            if (targetOverride == targetsToLock[i])
            {
                AssignTargetValues(targetsToLock[i]);
                selectedTargetIndex = i;
                break;
            }
        }
    }

    void AssignTargetValues(Targetable pTarget)
    {
        ctd.ResetDetatchedBool();
        CinemachineTargetGroup.Target target;
        target.target = pTarget.transform;
        target.weight = pTarget.camWeight;
        target.radius = pTarget.camRadius;

        targetGroup.m_Targets[1].target = target.target;
        targetGroup.m_Targets[1].weight = target.weight;
        targetGroup.m_Targets[1].radius = target.radius;

        selectedTarget = pTarget;
        cameraTarget.parent = selectedTarget.GetTargetHolder();
        cameraTarget.localPosition = Vector3.zero;
        //if (cameraTarget.position.y < emptyTarget.transform.position.y)
        //{
        //    cameraTarget.position = new Vector3(cameraTarget.position.x, emptyTarget.transform.position.y, cameraTarget.position.z);
        //}
    }

    private void OnTransformChildrenChanged()
    {
        if (ctd.GetDetatchedBool())
        {
            ctd.ResetDetatchedBool();
            FindClosestTarget();
            ChangeTarget(closestTarget);
        }
    }

    // "priority" enemy override
    public void SetPriorityEnemy(Transform t)
    {
        Targetable asset = t.GetComponent<Targetable>();
        if (asset == null) return;
        priorityTarget = asset;
    }
    public void SetPriorityEnemy(Targetable t) => priorityTarget = t;
    public void ResetPriorityTarget() => priorityTarget = null;

    // target pass
    public Transform GetSelectedTarget()
    {
        if (selectedTarget == null || selectedTarget == emptyTarget) return null;

        return selectedTarget.transform;
    }
}
