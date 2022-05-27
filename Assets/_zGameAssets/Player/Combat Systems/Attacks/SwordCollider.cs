using System.Collections.Generic;
using UnityEngine;

public class SwordCollider : MonoBehaviour
{
    [SerializeField] bool isOnPlayer;
    [SerializeField] Transform root;

    [SerializeField] Collider coll;

    [SerializeField] string location = "";

    private List<HealthSystem> targetsHit;

    private float attackKnockbackStrength;
    private int attackDamage;
    private bool isHeavy;

    bool stop;
    float stopTime = 0.05f;

    private void Start()
    {
        root = transform.root;
        if (root.tag == "Player") isOnPlayer = true;

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
            //if (isOnPlayer && !stop)
            //{
            //    stop = false;
            //    Time.timeScale = 0;

            //    //StartCoroutine("ReturnTimeScale");
            //}
        }
    }

    System.Collections.IEnumerator ReturnTimeScale()
    {
        yield return new WaitForSecondsRealtime(stopTime);

        Time.timeScale = 1;
        stop = false;
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
