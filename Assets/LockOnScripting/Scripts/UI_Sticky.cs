using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Sticky : MonoBehaviour
{
    public GameObject sticky;
    private Camera activeCam;
    private Vector3 pos;

    // Start is called before the first frame update
    void Start()
    {
        activeCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        pos = activeCam.WorldToScreenPoint(sticky.transform.position);
    }

    private void LateUpdate()
    {
        transform.position = pos;
    }
}
