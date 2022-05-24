using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] bool onPlayer = false;

    EnemyMovement em;
    Animator anim;
    Rigidbody rb;
    RagdollOnOff ragdoll;

    bool alreadyHitByThisAnimation = false;

    [SerializeField] float health = 100;
    float currenthealth;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        ragdoll = GetComponentInChildren<RagdollOnOff>();

        em = GetComponent<EnemyMovement>();

        currenthealth = health;
    }

    void Update()
    {
        
    }

    public void TakeDamage(Transform source, float strength, int damage, bool heavy)
    {
        if (ragdoll.ragdolled || currenthealth < 0 || alreadyHitByThisAnimation) return;

        Vector3 dir = transform.position - source.position;
        if (!heavy) dir.y = 0;
        else dir.y = Mathf.Abs(dir.y);
        dir.Normalize();

        if (rb != null)
        {
            //rb.AddForce(dir * strength, ForceMode.Impulse);
            if (heavy)
            {
                ragdoll.RagdollToggle(true);
                ragdoll.AddRagdollForce(dir * strength);
            }
            else
            {
                // rotate to face player
                if (!onPlayer)
                {
                    em.LocalAlert(source);
                    em.RotateToTarget(source);
                }

                anim.Play("GetHurt");
            }

        }

        currenthealth -= damage;
        if (currenthealth < 0)
        {
            currenthealth = 0;

            ragdoll.RagdollToggle(true);

            //die
        }
        else
        {
            alreadyHitByThisAnimation = true;
        }
    }

    public void ResetAlreadyHitBool()
    {
        alreadyHitByThisAnimation = false;
    }

}
