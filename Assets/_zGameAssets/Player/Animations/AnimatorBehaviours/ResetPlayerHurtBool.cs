using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPlayerHurtBool : StateMachineBehaviour
{
    PlayerMainStateManager pmsm;

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (pmsm == null) pmsm = animator.GetComponentInParent<PlayerMainStateManager>();

        pmsm.hurt = false;
    }
}
