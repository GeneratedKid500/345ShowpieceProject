using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadLookAt : MonoBehaviour
{
    Animator anim;

    [Range(0.0f, 1.0f)]
    public float targetLookAtWeight = 1;
    private float currentLookAtWeight = 0;
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
                if (currentLookAtWeight < targetLookAtWeight)
                {
                    currentLookAtWeight += 0.0075f;
                }
                anim.SetLookAtWeight(currentLookAtWeight);
                anim.SetLookAtPosition(lookObj.position);
            }
        }
        else
        {
            if (currentLookAtWeight > 0)
            {
                currentLookAtWeight -= 0.0075f;
                if (lookObj != null)
                {
                    anim.SetLookAtPosition(lookObj.position);
                }
            }
            anim.SetLookAtWeight(currentLookAtWeight);
        }
    }

    public void SetNewLookAt(Transform obj, float strength)
    {
        lookObj = obj;
        targetLookAtWeight = Mathf.Clamp01(strength);
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
