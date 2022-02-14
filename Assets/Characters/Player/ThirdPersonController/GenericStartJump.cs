using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericStartJump : MonoBehaviour
{
    ThirdPersonControl fpControl;
    Animator anim;
    private void Start()
    {
        fpControl = GetComponentInParent<ThirdPersonControl>();
        anim = GetComponent<Animator>();
    }

    public void StartJump()
    {
        if (fpControl != null)
        {
            fpControl.ApplyJump();
        }
    }

    private void OnAnimatorMove()
    {
        float delta = Time.deltaTime;
        Vector3 deltaPos = anim.deltaPosition;
        Vector3 vel = deltaPos / delta;
        fpControl.ApplyRootMotion(vel);
    }
}
