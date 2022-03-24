using UnityEngine;
using Cinemachine;

public class PlayerMainStateManager : MonoBehaviour
{
    public bool lockedOn;
    public CinemachineVirtualCameraBase freeCam;
    public CinemachineVirtualCameraBase zTargetCam;

    private CinemachineFreeLook freeLook;
    private Vector3 baseCameraPos;

    void Start()
    {
        freeLook = freeCam.gameObject.GetComponent<CinemachineFreeLook>();
    }

    // updates camera between free camera and lock on camera
    void Update()
    {
        if (lockedOn)
        {
            if (freeCam.gameObject.activeSelf)
            {
                freeCam.gameObject.SetActive(false);
                freeLook.m_RecenterToTargetHeading.m_enabled = true;
                zTargetCam.gameObject.SetActive(true);
            }
        }
        else
        {
            if (!freeCam.gameObject.activeSelf)
            {
                freeCam.gameObject.SetActive(true);
                freeLook.m_RecenterToTargetHeading.m_enabled = false;
                zTargetCam.gameObject.SetActive(false);
            }  
        }
    }
}
