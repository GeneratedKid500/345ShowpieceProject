using UnityEngine;
using Cinemachine;

public class PlayerMainStateManager : MonoBehaviour
{
    public bool paused = false;
    [Space]
    public bool lockedOn = false;
    public bool hurt = false;
    public bool ragdolled = false;
    public bool attacking = false;
    public bool grounded = false;
    public bool sprinting = false;

    private RagdollOnOff rgd;
    private PlayerActionDistributor ad;
    private WeaponLoader wLoader;

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
    private int strafeLayerID = -1;
    private int groundedAttackLayerID = -1;
    private int airAttackLayerID = -1;
    [SerializeField] float strafeBlendSpeed = 3f;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        if (anim != null)
        {
            // finds the layer name "Strafe Layer" for strafing animations
            for (int i = 0; i < anim.layerCount; i++)
            {
                string name = anim.GetLayerName(i);
                if (name == "Strafe Layer")
                {
                    strafeLayerID = i;
                    continue;
                }
                else if (name == "Generic Attack Layer")
                {
                    groundedAttackLayerID = i;
                    continue;
                }
                else if (name == "Air Attack Layer")
                {
                    airAttackLayerID = i;
                    continue;
                }

                if (strafeLayerID != -1 && groundedAttackLayerID != -1 && airAttackLayerID != -1) break;
            }
        }
        else Debug.LogError("PlayerMainStateManager - No Animator Attatched");

        rgd = GetComponentInChildren<RagdollOnOff>();
        ad = GetComponent<PlayerActionDistributor>();

        freeLook = freeCam.gameObject.GetComponent<CinemachineFreeLook>();
        reticleAlpha = reticleTemp.alpha;

        tpc = GetComponent<ThirdPersonControl>();

        lom = GetComponent<LockOnMovement>();

        wLoader = GetComponent<WeaponLoader>();
    }

    void Start()
    {

        freeCam.gameObject.SetActive(false);
    }

    // updates camera between free camera and lock on camera
    void Update()
    {
        ragdolled = rgd.ragdolled;
        attacking = ad.attacking;
        ad.canInput = !ragdolled;

        if (paused) return;

        if (ragdolled)
        {
            sprinting = false;
            lockedOn = false;
            anim.SetLayerWeight(strafeLayerID, 0);
        }

        if (hurt)
        {
            tpc.DisableCharacter();
            tpc.SetRotation(false);
            anim.SetLayerWeight(strafeLayerID, 0);
            anim.SetLayerWeight(groundedAttackLayerID, 0);
            return;
        }

        float layerWeight = anim.GetLayerWeight(strafeLayerID);
        // if locked on
        if (lockedOn && !rgd.ragdolled)
        {
            sprinting = false;
            lom.SetMovement(!attacking);
            grounded = lom.returnGrounded();

            if (tpc.Grounded)
            {
                wLoader.FadeInWeapon();
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
        // not locked on
        else
        {
            grounded = tpc.isGrounded();
            sprinting = tpc.isSprinting();

            // disables character movement when attacking
            if (attacking)
            {
                wLoader.FadeInWeapon();
                tpc.DisableCrouching();
                tpc.DisableCharacter();
            }
            else
            {
                wLoader.FadeOutWeapon();
                tpc.EnableCharacter();
            }

            // enables regular camera
            if (!freeCam.gameObject.activeSelf && !attacking)
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

        if (attacking)
        {
            // if attacking on ground
            if (grounded)
            {
                if (anim.GetLayerWeight(groundedAttackLayerID) < 1 && !anim.GetCurrentAnimatorStateInfo(3).IsName("Neutral"))
                {
                    anim.SetLayerWeight(groundedAttackLayerID, anim.GetLayerWeight(groundedAttackLayerID) + (6 * Time.deltaTime));
                }

                if (anim.GetLayerWeight(airAttackLayerID) > 0)
                {
                    anim.SetLayerWeight(airAttackLayerID, anim.GetLayerWeight(airAttackLayerID) - (6 * Time.deltaTime));
                }
            }
            // if attacking in air
            else
            {
                if (anim.GetLayerWeight(airAttackLayerID) < 1 && !anim.GetCurrentAnimatorStateInfo(4).IsName("Neutral"))
                {
                    anim.SetLayerWeight(airAttackLayerID, anim.GetLayerWeight(airAttackLayerID) + (6 * Time.deltaTime));
                }

                if (anim.GetLayerWeight(groundedAttackLayerID) > 0)
                {
                    anim.SetLayerWeight(groundedAttackLayerID, anim.GetLayerWeight(groundedAttackLayerID) - (6 * Time.deltaTime));
                }
            }
        }
        else
        {
            if (anim.GetLayerWeight(groundedAttackLayerID) > 0)
            {
                anim.SetLayerWeight(groundedAttackLayerID, anim.GetLayerWeight(groundedAttackLayerID) - (6 * Time.deltaTime));
            }

            if (anim.GetLayerWeight(airAttackLayerID) > 0)
            {
                anim.SetLayerWeight(airAttackLayerID, anim.GetLayerWeight(airAttackLayerID) - (6 * Time.deltaTime));
            }
        }
    }

    public void EnableGenericAttackingLayer()
    {
        if (grounded)
        {
            anim.SetLayerWeight(groundedAttackLayerID, 1);
        }
        else
        {
            anim.SetLayerWeight(airAttackLayerID, 1);
        }

    }

    public void RestartAnimator()
    {
        anim.updateMode = AnimatorUpdateMode.UnscaledTime;
        anim.enabled = false;
        anim.enabled = true;
        anim.updateMode = AnimatorUpdateMode.Normal;

        anim.gameObject.SetActive(false);
        anim.gameObject.SetActive(true);
    }
}
