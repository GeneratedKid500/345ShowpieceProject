using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableEnemyAttackingBool : StateMachineBehaviour
{
    DuelMove duelAI;
    SwordCollider col;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (duelAI == null) duelAI = animator.transform.root.GetComponent<DuelMove>();

        if (col == null)
        {
            SwordCollider[] cols = animator.GetComponentsInChildren<SwordCollider>();
            foreach (SwordCollider coll in cols)
            {
                if (coll.GetLocation() == "Sword")
                {
                    col = coll;
                }
            }
        }

        duelAI.SetAttacking(false);
        col.GetCollider().enabled = false;
    }
}
