using UnityEngine;
using Cinemachine;

public class FreeLookAxisControl : MonoBehaviour
{
    void Start()
    {
        CinemachineCore.GetInputAxis = GetAxisCustom;
    }

    public float GetAxisCustom(string axisName)
    {
        float r = 0.0f;
        if (axisName.Equals("X Axis"))
        {
            r += Input.GetAxis("Mouse X");
            return Mathf.Clamp(r, -1.0f, 1.0f);
        }
        else if (axisName.Equals("Y Axis"))
        {
            r += Input.GetAxis("Mouse Y");
            return Mathf.Clamp(r, -1.0f, 1.0f);
        }

        return 0;
    }
}
