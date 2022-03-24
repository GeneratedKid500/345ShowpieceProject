using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTargetDetatch : MonoBehaviour
{
    public Transform player;
    private bool detatched = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void Detatch()
    {
        transform.parent = player;
        detatched = true;
    }

    public bool GetDetatchedBool()
    {
        return detatched;
    }

    public void ResetDetatchedBool() => detatched = false;
}
