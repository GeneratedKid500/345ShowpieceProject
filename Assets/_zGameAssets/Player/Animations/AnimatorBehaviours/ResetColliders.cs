using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetColliders : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SwordCollider[] cols = animator.GetComponentsInChildren<SwordCollider>();

        foreach (SwordCollider coll in cols)
        {
            coll.GetCollider().enabled = false;
            coll.HealthSystemResets();
        }
    }
}
