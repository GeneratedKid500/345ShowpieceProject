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

    // temporary function
    public void GetKilled()
    {
        CameraTargetDetatch ctd = GetComponentInChildren<CameraTargetDetatch>();

        if (ctd != null)
        {
            ctd.Detatch();
        }

        Destroy(this.gameObject);
    }
}
