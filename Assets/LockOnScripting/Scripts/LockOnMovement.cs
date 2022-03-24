using UnityEngine;

public class LockOnMovement : MonoBehaviour
{
    public Camera cam;
    public Vector3 camForward;
    public Vector3 camRight;

    void Start()
    {
        cam = Camera.main;
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
}
