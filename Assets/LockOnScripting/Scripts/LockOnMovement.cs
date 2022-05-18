using UnityEngine;

public class LockOnMovement : MonoBehaviour
{
    private Transform cam;
    private Transform t;
    private Rigidbody rb;
    private Animator anim;
    private CapsuleCollider pCollider;

    // lock on target
    private PlayerLockOnSystem los;
    private Transform lockOnTarget;

    [Header("Movement")]
    [SerializeField] float walkSpeed = 8;
    [SerializeField] float rotationSpeed = 10f;
    private Vector3 moveAmount = Vector3.zero;
    private Vector3 smoothMoveVelocity;
    private float vert;
    private float horz;
    private bool grounded;

    [SerializeField] LayerMask groundedMask;

    private void Awake()
    {
        cam = Camera.main.transform;
        t = transform;
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        pCollider = GetComponent<CapsuleCollider>();
    }

    void Start()
    {
        los = GetComponent<PlayerLockOnSystem>();
    }

    void Update()
    {
        lockOnTarget = los.GetSelectedTarget();

        MovementInput();
    }

    private void FixedUpdate()
    {
        grounded = isGrounded();

        rb.velocity = new Vector3(moveAmount.x, rb.velocity.y, moveAmount.z);

        if (lockOnTarget != null && grounded)
        {
            RotatingToTarget();
        }
    }


    void MovementInput()
    {
        vert = Input.GetAxisRaw("Vertical");
        horz = Input.GetAxisRaw("Horizontal");

        Vector3 targetDir = Vector3.zero;
        targetDir = cam.forward * vert;
        targetDir = targetDir + cam.right * horz;
        targetDir.Normalize();
        targetDir.y = 0;

        moveAmount = Vector3.SmoothDamp(moveAmount, targetDir * walkSpeed, ref smoothMoveVelocity, .15f);

        anim.SetFloat("MoveX", (float)System.Math.Round(transform.InverseTransformDirection(targetDir).x, 2));
        anim.SetFloat("MoveZ", (float)System.Math.Round(transform.InverseTransformDirection(targetDir).z, 2));
    }

    void RotatingToTarget()
    {
        Vector3 directionToTarget = lockOnTarget.transform.position - t.position;
        float angleToTarget = Mathf.Atan2(directionToTarget.x, directionToTarget.z) * Mathf.Rad2Deg;

        Quaternion desiredRot = Quaternion.Euler(t.eulerAngles.x, angleToTarget, t.eulerAngles.z);
        t.rotation = Quaternion.Slerp(Quaternion.Euler(t.eulerAngles), desiredRot, rotationSpeed * Time.deltaTime);
    }

    bool isGrounded() //checks if grounded 
    {
        Vector3 positionStore = t.position;
        positionStore.y += 1;

        if (Physics.Raycast(positionStore, -t.up, out RaycastHit hit, 1.1f, groundedMask))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
