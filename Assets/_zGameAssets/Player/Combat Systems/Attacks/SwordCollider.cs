using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordCollider : MonoBehaviour
{
    private bool hasHit = false;

    private void OnTriggerEnter(Collider obj)
    {
        hasHit = true;

        //obj.GetComponent<HealthSystem>().DecreaseHealth();
    }

    private void OnTriggerExit(Collider obj)
    {
        hasHit = false;
    }
}
