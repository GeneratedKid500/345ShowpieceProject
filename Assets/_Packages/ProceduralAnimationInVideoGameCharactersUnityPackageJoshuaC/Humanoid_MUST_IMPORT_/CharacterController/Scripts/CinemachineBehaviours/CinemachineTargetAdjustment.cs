using UnityEngine;
using Cinemachine;

public class CinemachineTargetAdjustment : StateMachineBehaviour
{
    [SerializeField] CinemachineVirtualCameraBase cinemachineCam;
    [SerializeField] Transform cinemachineTarget;

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (cinemachineCam == null || cinemachineCam.isActiveAndEnabled == false)
        {
            cinemachineCam = animator.transform.root.GetComponentInChildren<CinemachineVirtualCameraBase>();
        }

        if (cinemachineTarget == null)
        {
            cinemachineTarget = animator.transform.root.GetComponentInChildren<RagdollOnOff>().InactiveTarget;
        }

        cinemachineCam.LookAt = cinemachineTarget;
    }
}
