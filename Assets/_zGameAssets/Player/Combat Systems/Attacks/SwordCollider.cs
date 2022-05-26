using System.Collections.Generic;
using UnityEngine;

public class SwordCollider : MonoBehaviour
{
    [SerializeField] Transform root;

    [SerializeField] Collider coll;

    [SerializeField] string location = "";

    private List<HealthSystem> targetsHit;

    private float attackKnockbackStrength;
    private int attackDamage;
    private bool isHeavy;

    private void Start()
    {
        root = transform.root;

        targetsHit = new List<HealthSystem>();

        if (location == "")
        {
            location = "Sword";
        }

        if (coll == null)
        {
            coll = GetComponent<BoxCollider>();
        }
    }

    private void OnTriggerEnter(Collider obj)
    {
        if (obj.transform == transform.root) return;

        HealthSystem target = obj.transform.GetComponent<HealthSystem>();
        if (target != null && target.transform.root != transform.root)
        {
            Debug.Log(target.transform.root.name);
            targetsHit.Add(target);
            target.TakeDamage(root, attackKnockbackStrength, attackDamage, isHeavy);
        }
    }

    public void ApplyStats(int atkdmg, float atkKnbckStr, bool heavy)
    {
        attackDamage = atkdmg;
        attackKnockbackStrength = atkKnbckStr;
        isHeavy = heavy;
    }

    public void HealthSystemResets()
    {
        if (targetsHit.Count == 0) return;
        foreach (HealthSystem target in targetsHit)
        {
            target.ResetAlreadyHitBool();
        }
        targetsHit.Clear();
    }

    public string GetLocation()
    {
        return location;
    }

    public Collider GetCollider()
    {
        return coll;
    }
}
