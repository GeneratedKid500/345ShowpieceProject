using UnityEngine;

public class Targetable : MonoBehaviour
{
    public float camWeight;

    public float camRadius;

    private void Awake()
    {
        // sets layer to be "Targetable" layerID 9
        transform.gameObject.layer = 9;
    }
}
