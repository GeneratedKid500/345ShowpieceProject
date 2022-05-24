using UnityEngine;

public class AnimatorPassThroughAttackValues : StateMachineBehaviour
{
    [SerializeField] string destination = "";
    [Space]
    [SerializeField] int attackDamage = 1;
    [SerializeField] [Range(0, 20)] float knockbackStrength = 10;
    [SerializeField] bool heavyAttack;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (destination == "")
        {
            destination = "Sword";
        }

        SwordCollider[] cols = animator.GetComponentsInChildren<SwordCollider>();
        SwordCollider col = null;
        foreach (SwordCollider coll in cols)
        {
            coll.GetCollider().enabled = false;
            if (coll.GetLocation() == destination)
            {
                col = coll;
                break;
            }
        }

        if (col != null)
        {
            col.HealthSystemResets();
            col.GetCollider().enabled = true;  
            col.ApplyStats(attackDamage, knockbackStrength, heavyAttack);
        }

    }

}
