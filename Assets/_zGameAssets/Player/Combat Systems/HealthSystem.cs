using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    Rigidbody rb;
    RagdollOnOff ragdoll;

    bool alreadyHitByThisAnimation = false;

    [SerializeField] float health = 100;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ragdoll = GetComponentInChildren<RagdollOnOff>();
    }

    void Update()
    {
        
    }

    public void TakeDamage(Transform source, float strength, int damage, bool heavy)
    {
        if (ragdoll.ragdolled || health < 0 || alreadyHitByThisAnimation) return;

        Debug.Log(strength);

        Vector3 dir = transform.position - source.position;
        if (!heavy) dir.y = 0;
        else dir.y = Mathf.Abs(dir.y);
        dir.Normalize();

        if (rb != null)
        {
            if (heavy)
            {
                ragdoll.RagdollToggle(true);
            }
            ragdoll.AddRagdollForce(dir * strength);
        }

        health -= damage;
        if (health < 0)
        {
            health = 0;

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
