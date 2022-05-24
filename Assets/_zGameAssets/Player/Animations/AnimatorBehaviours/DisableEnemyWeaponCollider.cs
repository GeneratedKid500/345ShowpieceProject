using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableEnemyWeaponCollider : StateMachineBehaviour
{
    SwordCollider col;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
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

        col.GetCollider().enabled = false;
    }
}
