using UnityEngine;

/// Tilts the head of a Unity Humanoid Avatar towards areas specified through the prefab collider
public class HeadLookAt : MonoBehaviour
{
    Animator anim;

    [Range(0.0f, 1.0f)]
    [SerializeField] float targetLookAtWeight = 1;
    private float currentLookAtWeight = 0;
    [SerializeField] bool ikActive = true;
    [SerializeField] bool nearLookObj = true;
    [SerializeField] Transform lookObj;
    private string objTag;

    private void Start()
    {
        anim = GetComponent<Animator>();

        if (lookObj = null)
        {
            nearLookObj = false;
        }
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (ikActive && nearLookObj)
        {
            if (lookObj != null)
            {
                Vector3 dirToObj = lookObj.position - transform.parent.position;
                if (Physics.Raycast(transform.parent.position, dirToObj, out RaycastHit hit, Mathf.Infinity) && hit.transform.tag == objTag)
                {
                    Debug.DrawRay(transform.parent.position, dirToObj, Color.green);
                    if (currentLookAtWeight < targetLookAtWeight)
                    {
                        currentLookAtWeight += 0.0075f;
                    }
                    else if (targetLookAtWeight > currentLookAtWeight)
                    {
                        currentLookAtWeight -= 0.0075f;
                    }
                }
                else
                {
                    Debug.DrawRay(transform.parent.position, dirToObj, Color.red);
                    if (currentLookAtWeight > 0)
                    {
                        currentLookAtWeight -= 0.0075f;
                    }
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

    public void SetNewLookAt(Transform obj, float strength, string newTag)
    {
        nearLookObj = true;
        lookObj = obj;
        targetLookAtWeight = Mathf.Clamp01(strength);
        objTag = newTag;
    }

    public void EnableHeadIK() => ikActive = true;
    public void DisableHeadIK() => ikActive = false;

    private void OnDisable()
    {
        nearLookObj = false;
        lookObj = null;
        objTag = null;
    }

}
