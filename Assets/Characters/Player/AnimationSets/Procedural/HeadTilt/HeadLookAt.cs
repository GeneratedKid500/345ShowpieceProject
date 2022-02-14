using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadLookAt : MonoBehaviour
{
    Animator anim;

    [Range(0.0f, 1.0f)]
    public float lookAtWeight = 1;
    public bool ikActive = true;
    public Transform lookObj;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (ikActive)
        {
            if (lookObj != null)
            {
                anim.SetLookAtWeight(lookAtWeight);
                anim.SetLookAtPosition(lookObj.position);
            }
        }
        else
        {
            anim.SetLookAtWeight(0);
        }
    }

    public void SetNewLookAt(Transform obj, float strength)
    {
        lookObj = obj;
        lookAtWeight = Mathf.Clamp01(strength);
    }

    public void EnableHeadIK()
    {
        ikActive = true;
    }

    public void DisableHeadIK()
    {
        ikActive = false;
    }

}
