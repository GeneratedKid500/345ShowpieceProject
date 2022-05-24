using UnityEngine;

/// Calls the Jump function on the ThirdPersonControl script through Animation Events if Animation is enabled
public class GenericStartJump : MonoBehaviour
{
    PlayerMainStateManager pmsm;

    ThirdPersonControl fpControl;
    Animator anim;
    private void Start()
    {
        pmsm = GetComponentInParent<PlayerMainStateManager>();
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
        Vector3 deltaPos = anim.deltaPosition;
        if (pmsm.attacking || pmsm.hurt)
        {
            transform.parent.rotation = anim.rootRotation;
            deltaPos.y = 0f;
            transform.parent.position += deltaPos;
        }
        else
        {
            float delta = Time.deltaTime;
            Vector3 vel = deltaPos / delta;
            fpControl.ApplyRootMotion(vel);
        }
    }
}
