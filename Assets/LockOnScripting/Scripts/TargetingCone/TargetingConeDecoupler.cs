using UnityEngine;

public class TargetingConeDecoupler : MonoBehaviour
{
    public void Detach()
    {
        transform.parent = null;
    }
}
