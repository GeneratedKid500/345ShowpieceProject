using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveArmToWall : MonoBehaviour
{
    Animator anim;
    [SerializeField] bool ikActive = true;

    [Range(0.0f, 1.0f)]
    [SerializeField] float reachWeight = 0;
    [SerializeField] Transform reachObj;
    Vector3 reachPoint;

    [SerializeField] Transform origin;

    private void Start()
    {
        anim = GetComponent<Animator>();

        if (reachObj == null)
        {
            ikActive = false;
        }
    }

    private void FixedUpdate()
    {
        Vector3 dirToObj = reachObj.position - origin.position;
        if (Physics.Raycast(origin.position, transform.right, out RaycastHit hit, 1.2f))
        {
            if (hit.transform.tag == "Respawn")
            {
                ikActive = true;
                reachPoint = hit.point;
                Debug.DrawRay(origin.position, reachPoint, Color.green);
            }
        }
        else
        {
            Debug.DrawRay(origin.position, dirToObj, Color.red);
            ikActive = false;
        }
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (ikActive)
        {
            anim.SetIKPositionWeight(AvatarIKGoal.RightHand, reachWeight);
            anim.SetIKPosition(AvatarIKGoal.RightHand, reachPoint);

            Quaternion handRotation = Quaternion.LookRotation(reachObj.position - transform.position);
            anim.SetIKRotationWeight(AvatarIKGoal.RightHand, reachWeight);
            anim.SetIKRotation(AvatarIKGoal.RightHand, handRotation);
        }
        else
        {

        }
    }
}
