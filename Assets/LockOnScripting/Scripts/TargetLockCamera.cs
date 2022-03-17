using UnityEngine;
using Cinemachine;

public class TargetLockCamera : MonoBehaviour
{
    public bool targetLockCam;
    public CinemachineVirtualCameraBase freeCam;
    public CinemachineVirtualCamera zTargetCam;
    private CinemachineFreeLook freeLook;

    void Start()
    {
        freeLook = freeCam.gameObject.GetComponent<CinemachineFreeLook>();
    }

    // updates camera between free camera and lock on camera
    void Update()
    {
        if (targetLockCam)
        {
            freeCam.gameObject.SetActive(false);
            freeLook.m_RecenterToTargetHeading.m_enabled = true;
            zTargetCam.gameObject.SetActive(true);
        }
        else
        {
            zTargetCam.gameObject.SetActive(false);
            freeLook.m_RecenterToTargetHeading.m_enabled = false;
            freeCam.gameObject.SetActive(true);
        }
    }
}
