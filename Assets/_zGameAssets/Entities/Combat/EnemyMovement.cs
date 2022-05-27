using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public abstract class EnemyMovement : MonoBehaviour
{
    public bool enableMove = true;

    protected Animator anim;
    protected Rigidbody rb;
    protected NavMeshAgent agent;
    protected RagdollOnOff roo;

    [SerializeField] protected bool randomNavigation;
    [SerializeField] protected Transform[] allWaypoints;
    protected int pointer = 0;

    [SerializeField] protected LayerMask detectionLayer;

    [Header("Sight")]
    [SerializeField] protected int maxNumberOfDetections = 10;
    [SerializeField] public int sightAngle;
    [SerializeField] public int sightRadius;
    [SerializeField] [Range(-1f, 1f)] protected float minmimumDotLevel = -0.5f;
    [SerializeField] protected float inSightRadiusIncrease = 1.2f;
    protected bool canSeeTarget;

    public Vector3 fromVector
    {
        get
        {
            float leftAngle = -sightAngle / 2;
            leftAngle += transform.eulerAngles.y;
            return new Vector3(Mathf.Sin(leftAngle * Mathf.Deg2Rad), 0, Mathf.Cos(leftAngle * Mathf.Deg2Rad));
        }
    }


    [Header("Enemy Behaviour")]
    [SerializeField] protected bool urgent;
    public Transform currentTarget;
    protected float targetDistance;
    [SerializeField] protected float waypointStoppingDistance = 1;
    [SerializeField] protected float stoppingDistance = 1f;
    [SerializeField] protected float moveSpeed = 3;
    [SerializeField] protected float runSpeed = 6;
    [SerializeField] protected float speedChangeSpeed = 5;
    protected float speed;
    [SerializeField] protected float rotationSpeed = 15;
    protected bool travellingToWaypoint = true;

    protected bool targeting = false;

    protected void Awake()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();

        anim = GetComponentInChildren<Animator>();
        roo = GetComponentInChildren<RagdollOnOff>();
    }

    protected void Start()
    {
        enableMove = true;
        GetAllWaypoints();
        NextWaypoint();
    }

    protected void FixedUpdate()
    {
        if (roo.ragdolled) return;

        #region Enemy Detection
        if (currentTarget == transform) NextWaypoint();

        Collider[] colliders = Physics.OverlapSphere(transform.position, sightRadius, detectionLayer);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (currentTarget.tag != "Player")
            {
                if (colliders[i].tag != transform.tag && colliders[i].tag != "Untagged")
                {
                    canSeeTarget = Sight(colliders[i].transform);
                }
            }

            if (currentTarget.tag != "Waypoint" && colliders[i].tag == transform.tag)
            {
                colliders[i].GetComponent<EnemyMovement>().LocalAlert(colliders[i].transform);
            }

        }
        #endregion

        // if close enough to target
        if (currentTarget.tag != "Waypoint")
        {
            if (targetDistance > sightRadius * inSightRadiusIncrease)
            {
                NextWaypoint();
            }

            if (targetDistance < stoppingDistance)
            {
                AttackSubroutine();
            }
            else
            {
                RetreatSubroutine();
            }
        }
    }

    public virtual void AttackSubroutine()
    {
        targeting = true;

        RotateToTarget(currentTarget);

        agent.enabled = true;
        agent.SetDestination(transform.position);
        agent.isStopped = true;
    }

    public virtual void RetreatSubroutine() 
    {
        targeting = false;

        agent.enabled = true;
        agent.isStopped = false;
        agent.SetDestination(currentTarget.position);
    }

    protected void Update()
    {
        targetDistance = Vector3.Distance(transform.position, currentTarget.transform.position);

        if (!roo.ragdolled && enableMove)
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;

            if (!targeting)
            {
                agent.enabled = true;
                agent.isStopped = false;

                AlterSpeed();
            }

            MoveToWaypoint();
            MoveToTarget();
        }
        else
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation;

            if (agent.enabled)
            {
                agent.speed = 0;
                agent.isStopped = true;
                agent.enabled = false;
            }
        }
    }

    #region Navigation - GetAllWaypoints, NextWaypoint, MoveToWaypoint, MoveToTarget, AlterSpeed
    protected void GetAllWaypoints()
    {
        if (allWaypoints.Length == 0)
        {
            GameObject[] waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
            allWaypoints = new Transform[waypoints.Length];
            for (int i = 0; i < waypoints.Length; i++)
            {
                allWaypoints[i] = waypoints[i].transform;
            }
            pointer = 0;
        }
    }

    protected void NextWaypoint(Transform tOverride = null)
    {
        if (pointer < allWaypoints.Length - 1)
            pointer++;
        else
            pointer = 0;

        if (randomNavigation)
        {
            pointer = Random.Range(0, allWaypoints.Length - 1);
        }

        if (tOverride != null)
        {
            currentTarget = tOverride;
            switch (tOverride.tag)
            {
                case "Waypoint":
                    travellingToWaypoint = true;
                    break;

                default:
                    travellingToWaypoint = false;
                    break;
            }
        }
        else
        {
            currentTarget = allWaypoints[pointer];
            travellingToWaypoint = true;
        }

        if (agent.enabled)
        {
            agent.SetDestination(currentTarget.position);
        }
    }

    protected void MoveToWaypoint()
    {
        if (travellingToWaypoint)
        {
            // hide weapon(s)

            if (agent.enabled && agent.remainingDistance < waypointStoppingDistance)
            {
                NextWaypoint();
                Debug.Log("MoveToWaypoint");
            }
        }
    }

    protected void MoveToTarget()
    {
        if (!travellingToWaypoint)
        {
            //RotateToTarget(currentTarget);
        }
    }

    protected void AlterSpeed(float desiredSpeed = -1)
    {
        if (desiredSpeed == -1)
        {
            desiredSpeed = moveSpeed;
            if (urgent) desiredSpeed = runSpeed;
            if (targeting) desiredSpeed = 0;
        }

        if (speed < desiredSpeed)
        {
            speed += speedChangeSpeed * Time.deltaTime;
        }
        else if (speed > desiredSpeed)
        {
            speed -= speedChangeSpeed * Time.deltaTime;
        }

        agent.speed = speed;
        anim.SetFloat("MoveSpeed", speed);
    }
    #endregion

    public void RotateToTarget(Transform target, float rotateSpeedOverride = -1)
    {
        if (rotateSpeedOverride == -1)
        {
            rotateSpeedOverride = rotationSpeed;
        }

        Vector3 directionToTarget = target.position - transform.position;
        float angleToTarget = Mathf.Atan2(directionToTarget.x, directionToTarget.z) * Mathf.Rad2Deg;

        Quaternion desiredRot = Quaternion.Euler(transform.eulerAngles.x, angleToTarget, transform.eulerAngles.z);
        transform.rotation = Quaternion.Slerp(Quaternion.Euler(transform.eulerAngles), desiredRot, rotateSpeedOverride * Time.deltaTime);
    }

    #region Enemy Detection - Sight & LocalAlert
    protected bool Sight(Transform target)
    {
        Vector3 directionVector = (transform.position - target.transform.position).normalized;
        float dotProduct = Vector3.Dot(directionVector, transform.forward);

        if (dotProduct < minmimumDotLevel)
        {
            if (target.GetComponent<HealthSystem>() != null)
            {
                Vector3 targetDir = (target.transform.position - transform.position);
                targetDir.y = 0;

                Debug.DrawRay(transform.position, targetDir*sightRadius, Color.yellow);
                if (Physics.Raycast(transform.position, targetDir, out RaycastHit hit, sightRadius))
                {
                    if (hit.transform.root.tag == target.root.tag)
                    {
                        NextWaypoint(target);
                        return true;
                    }
                    else
                    {
                        //Debug.Log("No tag");
                        //Debug.Log("Hit tag: " + hit.transform.tag);
                    }
                }
                else
                {
                    //Debug.Log("No Ray");
                    //Debug.Log("Ray:" + hit.transform.root.tag);
                }
            }
            else
            {
                //Debug.Log("No Health");
            }
        }
        else
        {
            //Debug.Log("No Detect");
        }

        return false;
    }

    public void LocalAlert(Transform target)
    {
        if (currentTarget.tag != "Player")
        {
            currentTarget = target;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 0, 0.5f);
        //Gizmos.DrawSphere(transform.position, sightRadius);
    }
    #endregion
}
