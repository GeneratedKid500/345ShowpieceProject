using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetRagdolledBool : StateMachineBehaviour
{
    RagdollOnOff roo;

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (roo == null)
        {
            roo = animator.transform.root.GetComponentInChildren<RagdollOnOff>();
        }
        roo.ragdolled = false;
    }

}
