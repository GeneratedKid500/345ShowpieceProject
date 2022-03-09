using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

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

    private void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().GetHeadLookAt();

        if (useScriptSize) objectCollider.size = colliderSize;
        else colliderSize = objectCollider.size;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.tag.Equals("Player")) return;

        playerRef.SetNewLookAt(transform.parent.transform, drawStrength, transform.parent.tag);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.tag.Equals("Player")) return;

        playerRef.DisableHeadIK();
    }
}
