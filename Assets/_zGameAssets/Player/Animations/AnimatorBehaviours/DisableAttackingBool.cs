using UnityEngine;

public class DisableAttackingBool : StateMachineBehaviour
{
    PlayerMainStateManager playerMainStateManager;
    PlayerActionDistributor actionDistributor;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (playerMainStateManager == null)
            playerMainStateManager = animator.GetComponentInParent<PlayerMainStateManager>();

        if (actionDistributor == null)
        {
            actionDistributor = animator.GetComponentInParent<PlayerActionDistributor>();
        }

        actionDistributor.DisableAttack();
    }
}
