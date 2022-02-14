using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private HeadLookAt hla;

    void Start()
    {
        SetNewHLA();
    }

    protected void SetNewHLA(){ hla = GetComponentInChildren<HeadLookAt>(); }
    public HeadLookAt GetHeadLookAt(){ return hla; }
}
