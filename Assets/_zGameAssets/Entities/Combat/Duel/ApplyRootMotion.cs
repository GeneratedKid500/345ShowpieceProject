using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyRootMotion : MonoBehaviour
{
    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnAnimatorMove()
    {
        transform.parent.position += anim.deltaPosition;
        transform.parent.rotation = anim.rootRotation;
    }
}
