using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableEnemyAttackingBool : StateMachineBehaviour
{
    [SerializeField] bool duel;
    [SerializeField] bool range;
    [SerializeField] bool swarm;

    DuelMove duelAI;
    RangeMovement rangeAI;
    RushdownMovementScript swarmAI;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (duel)
        {
            if (duelAI == null) duelAI = animator.transform.root.GetComponent<DuelMove>();
            duelAI.SetAttacking(false);
        }
        else if (range)
        {
            if (rangeAI == null) rangeAI = animator.transform.root.GetComponent<RangeMovement>();
            rangeAI.SetAttacking(false);
        }
        else if (swarm)
        {
            if (swarmAI == null) swarmAI = animator.transform.root.GetComponent<RushdownMovementScript>();
            swarmAI.SetAttacking(false);
        }
    }
}
