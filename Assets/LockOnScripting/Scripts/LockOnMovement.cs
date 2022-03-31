using UnityEngine;

public class LockOnMovement : MonoBehaviour
{
    private CapsuleCollider pCollider;

    [SerializeField] LayerMask groundedMask;

    public Camera cam;
    public Vector3 camForward;
    public Vector3 camRight;

    private void Awake()
    {
        cam = Camera.main;
    }

    void Start()
    {
        pCollider = GetComponent<CapsuleCollider>();
    }

    void Update()
    {
        camForward = cam.transform.forward;
        camForward.y = 0;
        camForward.Normalize();

        camRight = cam.transform.right;
        camRight.y = 0;
        camRight.Normalize();
    }

    bool isGrounded() //checks if grounded 
    {
        if (Physics.SphereCast(transform.position, pCollider.radius, -transform.up, out RaycastHit Hit, 1 + .1f, groundedMask)) return true;
        else return false;
    }
}
