using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveParent : MonoBehaviour
{
    [SerializeField] bool awake = false;

    private void Awake()
    {
        if (awake) this.gameObject.transform.parent = null;
    }

    void Start()
    {
        if (!awake) this.gameObject.transform.parent = null;
    }
}
