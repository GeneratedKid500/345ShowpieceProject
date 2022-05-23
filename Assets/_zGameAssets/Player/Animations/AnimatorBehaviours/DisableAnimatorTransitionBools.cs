using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAnimatorTransitionBools : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("HEAVY", false);
        animator.SetBool("NEXT", false);
    }

}
