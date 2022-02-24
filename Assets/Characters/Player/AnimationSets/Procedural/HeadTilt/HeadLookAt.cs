using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadLookAt : MonoBehaviour
{
    Animator anim;

    [Range(0.0f, 1.0f)]
    [SerializeField] float targetLookAtWeight = 1;
    [SerializeField] bool useDistanceFromTarget;
    private float maxDistanceFromTarget = 10;
    private float currentLookAtWeight = 0;
    [SerializeField] bool ikActive = true;
    [SerializeField] Transform lookObj;

    private void Start()
    {
        anim = GetComponent<Animator>();

        if (lookObj = null)
        {
            ikActive = false;
        }
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (ikActive)
        {
            if (lookObj != null)
            {
                if (currentLookAtWeight > targetLookAtWeight)
                {
                    currentLookAtWeight = targetLookAtWeight;
                }

                if (currentLookAtWeight < targetLookAtWeight)
                {
                    if (useDistanceFromTarget)
                    {
                        float currentDistanceFromTarget = Vector3.Distance(transform.position, lookObj.position);
                        if (currentDistanceFromTarget < maxDistanceFromTarget)
                        {
                            currentLookAtWeight = currentDistanceFromTarget / maxDistanceFromTarget;
                        }
                        else
                        {
                            currentLookAtWeight = 0;
                        }
                    }
                    else currentLookAtWeight += 0.0075f;

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

    public void SetNewLookAt(Transform obj, float strength, bool distanceBased = false, float maxDistance = 10)
    {
        ikActive = true;
        lookObj = obj;
        targetLookAtWeight = Mathf.Clamp01(strength);

        useDistanceFromTarget = distanceBased;
        maxDistanceFromTarget = maxDistance;
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
