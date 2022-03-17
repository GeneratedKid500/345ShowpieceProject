using UnityEngine;

/// Bundled with a collider to enable head IK and rotation on a Unity Humanoid Avatar.
[RequireComponent(typeof(BoxCollider))]
public class HeadLookTarget : MonoBehaviour
{
    HeadLookAt playerRef;

    [Header("Objects")]
    [SerializeField] BoxCollider objectCollider;

    [Header("Fields")]
    [SerializeField] bool useScriptSize;
    [SerializeField] Vector3 colliderSize;
    [Range(0.0f, 1.0f)]
    [SerializeField] float drawStrength;
    Transform parentTransform;
    string parentTag;

    private void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerIKManager>().GetHeadLookAt();

        parentTransform = transform.parent.transform;
        parentTag = transform.parent.tag;

        if (useScriptSize) objectCollider.size = colliderSize;
        else colliderSize = objectCollider.size;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.tag.Equals("Player")) return;

        playerRef.SetNewLookAt(parentTransform, drawStrength, parentTag);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.tag.Equals("Player")) return;

        //playerRef.DisableHeadIK();
    }
}
