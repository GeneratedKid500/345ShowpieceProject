using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableEnemyAttackingBool : StateMachineBehaviour
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
            duelAI.SetAttacking(true);
        }
        else if (range)
        {
            if (rangeAI == null) rangeAI = animator.transform.root.GetComponent<RangeMovement>();
            rangeAI.SetAttacking(true);
        }
        else if (swarm)
        {
            if (swarmAI == null) swarmAI = animator.transform.root.GetComponent<RushdownMovementScript>();
            //swarmAI.SetAttacking(true);
        }
    }


}
