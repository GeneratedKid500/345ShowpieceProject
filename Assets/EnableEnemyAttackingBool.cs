using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableEnemyAttackingBool : StateMachineBehaviour
{
    DuelMove duelAI;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (duelAI == null) duelAI = animator.transform.root.GetComponent<DuelMove>();

        duelAI.SetAttacking(true);
    }


}
