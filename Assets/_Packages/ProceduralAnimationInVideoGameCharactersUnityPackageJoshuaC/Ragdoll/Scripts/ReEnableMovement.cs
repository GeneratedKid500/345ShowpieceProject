using UnityEngine;

public class ReEnableMovement : StateMachineBehaviour
{
    ThirdPersonControl tpc;
    RagdollOnOff roo;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (tpc == null)
        {
            tpc = animator.transform.root.GetComponent<ThirdPersonControl>();
        }
        if (roo == null)
        {
            roo = animator.transform.root.GetComponentInChildren<RagdollOnOff>();
        }

        roo.ragdolled = true;
        tpc.DisableCrouching();
        tpc.DisableCharacter();
        tpc.SetRotation(false);
    }

    // OnStateExit is called before OnStateExit is called on any state inside this state machine
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (tpc == null)
        {
            tpc = animator.transform.root.GetComponent<ThirdPersonControl>();
        }
        if (roo == null)
        {
            roo = animator.transform.root.GetComponentInChildren<RagdollOnOff>();
        }

        roo.ragdolled = false;
        tpc.DisableCrouching();
        tpc.EnableCharacter();
        tpc.SetRotation(true);
    }
}
