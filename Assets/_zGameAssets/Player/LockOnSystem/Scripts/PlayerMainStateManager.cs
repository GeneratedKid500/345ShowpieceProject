using UnityEngine;
using Cinemachine;

public class PlayerMainStateManager : MonoBehaviour
{
    public bool lockedOn = false;
    public bool ragdolled;

    private RagdollOnOff rgd;

    [Header("Cameras")]
    [SerializeField] CinemachineVirtualCameraBase freeCam;
    [SerializeField] CinemachineVirtualCameraBase zTargetCam;
    private CinemachineFreeLook freeLook;

    [Header("Reticle")]
    [SerializeField] CanvasGroup reticleTemp;
    private float reticleAlpha;

    // free move
    private ThirdPersonControl tpc;

    // lock on
    private LockOnMovement lom;

    [Header("Animation Blending")]
    private Animator anim;
    private int strafeLayerID;
    [SerializeField] float strafeBlendSpeed = 3f;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        if (anim != null)
        {
            // finds the layer name "Strafe Layer" for strafing animations
            for (int i = 0; i < anim.layerCount; i++)
            {
                if (anim.GetLayerName(i) == "Strafe Layer")
                {
                    strafeLayerID = i;
                    break;
                }
            }
        }
        else Debug.LogError("PlayerMainStateManager - No Animator Attatched");

        rgd = GetComponentInChildren<RagdollOnOff>();

        freeLook = freeCam.gameObject.GetComponent<CinemachineFreeLook>();
        reticleAlpha = reticleTemp.alpha;

        tpc = GetComponent<ThirdPersonControl>();

        lom = GetComponent<LockOnMovement>();
    }

    void Start()
    {
        freeCam.gameObject.SetActive(false);
    }

    // updates camera between free camera and lock on camera
    void Update()
    {
        ragdolled = rgd.ragdolled;

        float layerWeight = anim.GetLayerWeight(strafeLayerID);
        if (ragdolled)
        {
            lockedOn = false;
            anim.SetLayerWeight(strafeLayerID, 0);
        }

        if (lockedOn && !rgd.ragdolled)
        {
            if (tpc.Grounded)
            {
                // enables lock-on camera
                if (freeCam.gameObject.activeSelf)
                {
                    freeCam.gameObject.SetActive(false);
                    freeLook.m_RecenterToTargetHeading.m_enabled = true;
                    zTargetCam.gameObject.SetActive(true);

                    // swaps movement scripts, disables crouch
                    tpc.DisableCrouching();
                    tpc.DisableCharacter();
                    tpc.SetRotation(false);
                    lom.enabled = true;
                }

                // reticle fades in
                if (reticleAlpha < 0.8f)
                {
                    reticleAlpha += 0.01f;
                    reticleTemp.alpha = reticleAlpha;
                }

                // enables strafing animations
                if (layerWeight < 1)
                {
                    anim.SetLayerWeight(strafeLayerID, layerWeight + (strafeBlendSpeed * Time.deltaTime));
                }
            }
        }
        else
        {
            // enables regular camera
            if (!freeCam.gameObject.activeSelf)
            {
                freeCam.gameObject.SetActive(true);
                freeLook.m_RecenterToTargetHeading.m_enabled = false;
                zTargetCam.gameObject.SetActive(false);

                // swaps movement scripts
                lom.enabled = false;
                tpc.EnableCharacter();
                tpc.SetRotation(true);
            }

            // reticle fades out
            if (reticleAlpha > 0)
            {
                reticleAlpha = 0;
                reticleTemp.alpha = reticleAlpha;
            }

            // disables strafing animations
            if (layerWeight > 0)
            {
                anim.SetLayerWeight(strafeLayerID, layerWeight - (strafeBlendSpeed * Time.deltaTime));
            }
        }
    }
}
