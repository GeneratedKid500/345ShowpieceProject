using UnityEngine;

public class Targetable : MonoBehaviour
{
    public float camWeight = 0.5f;

    public float camRadius = 4;

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
