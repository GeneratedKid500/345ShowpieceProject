using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] bool onPlayer = false;

    PlayerMainStateManager pmsm;

    EnemyMovement em;
    Animator anim;
    Rigidbody rb;
    RagdollOnOff ragdoll;

    bool alreadyHitByThisAnimation = false;
    [SerializeField] float timeUntilRemoveAtDeath = 2f;
    float deathTimer;

    [SerializeField] float health = 100;
    float currenthealth;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        ragdoll = GetComponentInChildren<RagdollOnOff>();

        em = GetComponent<EnemyMovement>();
        pmsm = GetComponent<PlayerMainStateManager>();

        currenthealth = health;
    }

    void Update()
    {
        if (!onPlayer && currenthealth <= 0 && ragdoll.ragdolled)
        {
            deathTimer += Time.deltaTime;
            if (deathTimer > timeUntilRemoveAtDeath)
            {
                int rand = Random.Range(0, 5);
                if (rand == 4)
                {
                    GetComponent<EnemyDrops>().Drop();
                }
                gameObject.SetActive(false);
            }
        }
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
                ragdoll.RagdollToggle();
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
                else
                {
                    pmsm.hurt = true;
                }

                anim.Play("GetHurt");
            }
        }

        currenthealth -= damage;
        if (currenthealth < 0)
        {
            currenthealth = 0;

            ragdoll.RagdollToggle(true);
            ragdoll.ToggleAutoStand(1);
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

    public float GetMaxHP()
    {
        return health;
    }

    public float GetHP()
    {
        return currenthealth;
    } 

    public void AddHP(int amount)
    {
        currenthealth = Mathf.Min(currenthealth + amount, health);
    }

}
