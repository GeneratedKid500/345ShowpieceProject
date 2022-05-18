using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class SwapTracks : MonoBehaviour
{
    [SerializeField] AudioSource disableSource;
    [SerializeField] AudioSource enableSource;

    private void OnTriggerEnter(Collider other)
    {
        disableSource.enabled = false;
        enableSource.enabled = true;
    }
}
