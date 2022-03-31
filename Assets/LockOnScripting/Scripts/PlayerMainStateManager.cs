using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class PlayerMainStateManager : MonoBehaviour
{
    public bool lockedOn;
    public CinemachineVirtualCameraBase freeCam;
    public CinemachineVirtualCameraBase zTargetCam;

    public CanvasGroup reticleTemp;
    private float reticleAlpha;

    private CinemachineFreeLook freeLook;
    private Vector3 baseCameraPos;

    void Start()
    {
        freeLook = freeCam.gameObject.GetComponent<CinemachineFreeLook>();

        reticleAlpha = reticleTemp.alpha;
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

            if (reticleAlpha < 0.8f)
            {
                reticleAlpha += 0.01f;
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

            if (reticleAlpha > 0)
            {
                reticleAlpha = 0;
            }
        }

        reticleTemp.alpha = reticleAlpha;
    }
}
