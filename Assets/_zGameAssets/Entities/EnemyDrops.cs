using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDrops : MonoBehaviour
{
    [SerializeField] GameObject healthPickup;

    public void Drop()
    {
        Instantiate(healthPickup, transform.position, healthPickup.transform.rotation);
    }
}