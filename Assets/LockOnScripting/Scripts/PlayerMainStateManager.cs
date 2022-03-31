using UnityEngine;
using Cinemachine;

public class PlayerMainStateManager : MonoBehaviour
{
    public bool lockedOn;
    [SerializeField] CinemachineVirtualCameraBase freeCam;
    [SerializeField] CinemachineVirtualCameraBase zTargetCam;

    [SerializeField] CanvasGroup reticleTemp;
    private float reticleAlpha;

    private CinemachineFreeLook freeLook;

    void Start()
    {
        freeLook = freeCam.gameObject.GetComponent<CinemachineFreeLook>();

        reticleAlpha = reticleTemp.alpha;
    }

    // updates camera between free camera and lock on camera
    void Update()
    {
        // enables cameras for lock on system
        if (lockedOn)
        {
            if (freeCam.gameObject.activeSelf)
            {
                freeCam.gameObject.SetActive(false);
                freeLook.m_RecenterToTargetHeading.m_enabled = true;
                zTargetCam.gameObject.SetActive(true);
            }

            if (reticleAlpha < 0.8f)
            {
                reticleAlpha += 0.01f;
                reticleTemp.alpha = reticleAlpha;
            }
        }
        // enables camera for regular system
        else
        {
            if (!freeCam.gameObject.activeSelf)
            {
                freeCam.gameObject.SetActive(true);
                freeLook.m_RecenterToTargetHeading.m_enabled = false;
                zTargetCam.gameObject.SetActive(false);
            }

            if (reticleAlpha > 0)
            {
                reticleAlpha = 0;
                reticleTemp.alpha = reticleAlpha;
            }
        }
    }
}
