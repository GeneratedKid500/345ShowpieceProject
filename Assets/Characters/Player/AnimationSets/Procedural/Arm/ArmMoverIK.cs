using UnityEngine;

/// Moves the arm of a Unity Humanoid Avatar to a desired position detected by raycast 
public class ArmMoverIK : MonoBehaviour
{
    Animator anim;
    [SerializeField] bool leftArm;

    [Header("Objects")]
    [Tooltip("Origin point of the raycast cast - indicates where the arm will be placed")]
    [SerializeField] Transform rayOrigin;

    [Tooltip("Game Object that the arm will reach towards")]
    [SerializeField] Transform armPoint;

    [SerializeField] LayerMask wallLayerMask;

    [Header("Values")]
    [SerializeField] bool ikActive = true;
    [SerializeField] bool nearWall = true;

    [Tooltip("How strongly the character will reach towards the wall")]
    [Range(0.0f, 1.0f)]
    [SerializeField] float targetReachWeight = 1;
    private float currentReachWeight;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (leftArm) ArmRayCast(-transform.right);
        else ArmRayCast(transform.right);
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (leftArm) ArmIKAnimator(AvatarIKGoal.LeftHand);
        else ArmIKAnimator(AvatarIKGoal.RightHand);
    }

    private void ArmRayCast(Vector3 transformDir)
    {
        if (Physics.Raycast(rayOrigin.position, transformDir, out RaycastHit hit, 0.85f, wallLayerMask) && Vector3.Distance(rayOrigin.position, hit.point) > 0.35f && ikActive)
        {
            nearWall = true;
            if (leftArm) armPoint.transform.position = hit.point - transform.TransformDirection(new Vector3(transform.localPosition.x - 0.05f, 0, transform.localPosition.z - 0.05f));
            else armPoint.transform.position = hit.point - transform.TransformDirection(new Vector3(transform.localPosition.x + 0.05f, 0, transform.localPosition.z + 0.05f));
        }
        else nearWall = false;
    }

    private void ArmIKAnimator(AvatarIKGoal arm)
    {
        Vector3 armPos = armPoint.transform.position;

        if (nearWall)
        {
            if (targetReachWeight > currentReachWeight) currentReachWeight += 0.0075f; // goes up if below 
            else if (currentReachWeight > targetReachWeight) currentReachWeight -= 0.0075f; // goes down if above

            anim.SetIKPosition(arm, armPos);

            Quaternion handRotation = Quaternion.LookRotation(armPos - transform.position);
            anim.SetIKRotationWeight(arm, targetReachWeight);
            anim.SetIKRotation(arm, handRotation);
        }
        else
        {
            if (currentReachWeight > 0)
            {
                currentReachWeight -= 0.0125f;
                anim.SetIKPosition(arm, armPos);
            }
        }
        anim.SetIKPositionWeight(arm, currentReachWeight);
    }

    public void EnableArmIK() => ikActive = true;
    public void DisableArmIK() => ikActive = false;
}
