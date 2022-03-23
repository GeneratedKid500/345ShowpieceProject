using UnityEngine;

public class TargetingConeTrigger : MonoBehaviour
{
    public GameObject selectedEnemy;
    public Vector3 coneScale;

    private void Awake()
    {
        coneScale.x = transform.localScale.x;
        coneScale.y = transform.localScale.y;
        coneScale.z = transform.localScale.z;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Targetable>())
        {
            selectedEnemy = other.gameObject;
        }
    }
}
