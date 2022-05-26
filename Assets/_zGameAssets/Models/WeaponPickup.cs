using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField] string weaponType;

    PlayerActionDistributor pad;

    // Start is called before the first frame update
    void Start()
    {
        pad = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerActionDistributor>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.tag == "Player")
        {
            if (weaponType == "Axe")
            {
                pad.ActivateWeapon(2);
            }
            else
            {
                pad.ActivateWeapon(3);
            }
            gameObject.SetActive(false);
        }
    }
}
