using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ThirdPersonControl : MonoBehaviour
{
    private PlayerIKManager pim;

    private Rigidbody rb;
    private CapsuleCollider pCollider;

    [Header("Toggles")]
    [SerializeField] bool enableMovement = false;
    [SerializeField] bool enableCamera = false;
    [SerializeField] bool enableJumping = false;
    [SerializeField] bool enableCrouching = false;
    [SerializeField] bool enableStairs = false;
    [SerializeField] bool enableAnim = false;
    bool[] baseSystems;

    [Header("CONTROLS")]
    [SerializeField] string jumpA;
    [SerializeField] string jumpB;
    [SerializeField] string crouchA, crouchB;
    [SerializeField] string sprintA, sprintB;
    [SerializeField] float CameraSensitivityX = 15;
    [SerializeField] float CameraSensitivityY = 15;

    [Header("Movement")]
    ////walking
    [SerializeField] float walkSpeed = 8;
    [SerializeField] float sprintMultiplier = 1.25f;
    [SerializeField] float rotationSpeed = 10f;
    private bool sprintOn;
    private float vert;
    private float horz;
    private Vector3 moveAmount;
    private Vector3 smoothMoveVelocity;
    ////crouching
    private int crouchLayerID;
    private float standHeight;
    private float crouchHeight;
    private float crouchTimer;
    private bool crouching;
    private bool crouched;

    [Header("Jumping")]
    [SerializeField] float jumpForce = 220;
    [SerializeField] float fastFallMultiplier = 2.5f;
    [SerializeField] float lowJumpMultiplier = 2.5f;
    private bool grounded;
    [SerializeField] LayerMask groundedMask;

    [Header("Stairs")]
    [SerializeField] GameObject stepRayUpper;
    [SerializeField] GameObject stepRayLower;
    [SerializeField] float stepSmooth = 2f;
    [SerializeField] float stepDistance = 0.1f;
    [SerializeField] LayerMask stairMask; //layer mask to determine what a stair is

    [Header("Camera Rotation")]
    // cam follow
    [SerializeField] float camFollowSpeed = 0.2f;
    [SerializeField] float cameraLookSpeed = 2f;
    [SerializeField] float cameraPivotSpeed = 2f;
    private Vector3 cameraFollowVel = Vector3.zero;
    private Vector3 cameraVectorPos;
    // cam rotation
    private float lookAngle; //up down
    private float pivotAngle; //left right
    [SerializeField] float minPivotAngle = -35;
    [SerializeField] float maxPivotAngle = 35;
    private float defaultPos;
    // cam collision
    [SerializeField] float cameraCollisionRadius = 0.2f;
    [SerializeField] float cameraCollisionOffset = 0.2f;
    [SerializeField] float minCollisionOffset = 0.2f;
    [SerializeField] LayerMask collisionLayers;
    private Transform cameraTransform;
    private Transform cameraMain;
    private Transform cameraPivot;

    [Header("Animation")]
    [SerializeField] Animator anim;
    private float moveAdd = 0;
    // animator parameter hashes
    private readonly int animMoveSpeedHash = Animator.StringToHash("MoveSpeed");
    private readonly int animYVelocityHash = Animator.StringToHash("Y Velocity");
    private readonly int animisGroundedHash = Animator.StringToHash("isGrounded");
    private readonly int animJumpingHash = Animator.StringToHash("jumping");

    void Awake()
    {
        if (enableAnim) pim = GetComponent<PlayerIKManager>();

        SetBaseSystems();

        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
    }

    void Start()
    {
        cameraTransform = Camera.main.transform; //gets camera's transform
        cameraMain = GetComponentsInChildren<Transform>()[1];
        cameraPivot = GetComponentsInChildren<Transform>()[2];

        defaultPos = cameraTransform.localPosition.z;

        //StepCheck
        if (enableStairs)
        {
            stepRayLower.transform.position = new Vector3(stepRayUpper.transform.position.x,
            stepRayLower.transform.position.y, stepRayUpper.transform.position.z);
        }

        pCollider = GetComponent<CapsuleCollider>();
        standHeight = pCollider.height;
        if (enableCrouching) crouchHeight = pCollider.height / 2;

        cameraLookSpeed = CameraSensitivityX / 10;
        cameraPivotSpeed = CameraSensitivityY / 10;

        if (enableAnim)
        {
            anim = GetComponentInChildren<Animator>();
            if (anim != null)
            {
                for (int i = 0; i < anim.layerCount; i++)
                {
                    if (anim.GetLayerName(i) == "Crouch Layer")
                    {
                        crouchLayerID = i;
                        break;
                    }
                }
            }
            else
            {
                Debug.LogError("Animation Enabled with no attatched animator!");
            }
        }
    }

    void Update()
    {
        GetMoveInput();

        if (enableCrouching) Crouch();

        JUMP();
    }

    void FixedUpdate()
    {
        grounded = isGrounded(); //updates is grounded

        // applies movement
        rb.velocity = new Vector3(moveAmount.x, rb.velocity.y, moveAmount.z);

        ////STAIR CLIMB
        if (enableStairs)
        {
            STAIRS(new Vector3(1.5f, 0, 1f)); //45' X
            STAIRS(Vector3.forward);
            STAIRS(new Vector3(-1.5f, 0, 1f)); //-45' X
        }

        if (enableAnim)
        {
            AnimationSprintBlend(Mathf.Abs(rb.velocity.x) + Mathf.Abs(rb.velocity.z));
            AnimationCrouchBlend();
        }

        RotateModel();
    }

    void LateUpdate()
    {
        CAMERA();
        AnimationSetVars();
    }

    #region MOVEMENT & ROTATION
    void GetMoveInput()
    {
        if (enableMovement)
        {
            vert = Input.GetAxisRaw("Vertical");
            horz = Input.GetAxisRaw("Horizontal");

            Vector3 moveDIR = cameraTransform.forward * vert;
            moveDIR = moveDIR + cameraTransform.right * horz;
            moveDIR.Normalize();
            moveDIR.y = 0;

            Vector3 targetMoveAmount = Vector3.zero;
            if (grounded) // only check for speed change when on floor
            {
                if (!isSprinting()) targetMoveAmount = moveDIR * walkSpeed;
                else if (crouching) targetMoveAmount = moveDIR * (walkSpeed / sprintMultiplier);
                else targetMoveAmount = moveDIR * (walkSpeed * sprintMultiplier);
            }
            else
            {
                if (!sprintOn) targetMoveAmount = moveDIR * walkSpeed;
                else targetMoveAmount = moveDIR * (walkSpeed * sprintMultiplier);
            }
            moveAmount = Vector3.SmoothDamp(moveAmount, targetMoveAmount, ref smoothMoveVelocity, .15f);
            //calculates the player's move from current to wanted position via SmoothDamp to ensure smooth movement
            //passes current velocity as a ref to ensure it gets updated
        }
    }

    void RotateModel()
    {
        if (enableMovement)
        {
            Vector3 targetDirection = Vector3.zero;

            targetDirection = cameraTransform.forward * vert;
            targetDirection = targetDirection + cameraTransform.right * horz;
            targetDirection.Normalize();
            targetDirection.y = 0;

            if (targetDirection == Vector3.zero)
                targetDirection = transform.forward;

            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            transform.rotation = playerRotation;
        }
    }
    #endregion

    #region CAMERA
    void CAMERA() //camera movement (including player on x val)
    {
        // cam follow
        if (enableCamera)
        {
            Vector3 tp = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            Vector3 targetPosition = Vector3.SmoothDamp(cameraMain.position, tp, ref cameraFollowVel, camFollowSpeed);
            cameraMain.position = targetPosition;

            // cam rot
            lookAngle = lookAngle + (Input.GetAxis("Mouse X") * cameraLookSpeed);

            pivotAngle = pivotAngle - (Input.GetAxis("Mouse Y") * cameraPivotSpeed);
            pivotAngle = Mathf.Clamp(pivotAngle, minPivotAngle, maxPivotAngle);

            Vector3 rot = Vector3.zero;
            rot.y = lookAngle;
            Quaternion targetRot = Quaternion.Euler(rot);
            cameraMain.rotation = targetRot;

            rot = Vector3.zero;
            rot.x = pivotAngle;
            targetRot = Quaternion.Euler(rot);
            cameraPivot.localRotation = targetRot;
            CameraCollisions();
        }
    }

    private void CameraCollisions()
    {
        float targetPos = defaultPos;
        float offsetSpeed = 0.01f;
        RaycastHit hit;
        Vector3 direction = cameraTransform.position - cameraPivot.position;
        direction.Normalize();

        if (Physics.SphereCast(cameraPivot.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(targetPos), collisionLayers))
        {
            float distance = Vector3.Distance(cameraPivot.position, hit.point);
            targetPos =- (distance - cameraCollisionOffset);
            offsetSpeed = 0.2f;
        }

        if (Mathf.Abs(targetPos) > minCollisionOffset)
        {
            targetPos = targetPos - minCollisionOffset;
        }

        cameraVectorPos.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPos, offsetSpeed);
        cameraTransform.localPosition = cameraVectorPos;
    }
    #endregion

    #region CROUCH
    void Crouch() //gets crouch input
    {
        if (Input.GetButtonDown(crouchA) || Input.GetButtonDown(crouchB))
        {
            crouchTimer = 0;
            ApplyCrouch();
        }
        else if (crouchTimer > 3 && Input.GetButtonUp(crouchA) || crouchTimer > 3 && Input.GetButtonUp(crouchB))
        {
            ApplyCrouch();
        }

        // if player holds button, will auto stand up once key is released.
        //this should be toggleable on the main menu
        if (Input.GetButton(crouchA) || Input.GetButton(crouchB))
        {
            crouchTimer += Time.deltaTime;
        }
    }
    void ApplyCrouch() //applies crouch movement
    {
        if (crouched) crouched = false;
        else crouching = !crouching;

        if (!enableAnim)
        {
            // halves collider / mesh height
            if (crouching)
                pCollider.height = crouchHeight; //lower height
            else
                pCollider.height = standHeight; //upper height
        }
        else
        {
            if (crouching)
            {
                pCollider.height = 1.40444f; //lower height
                pCollider.center = new Vector3(-0.006775737f, -0.2977802f, -1.192093e-07f);
            }
            else
            {
                if (isAnythingAboveHead())
                {
                    ApplyCrouch();
                }
                else
                {
                    pCollider.height = standHeight; //upper height
                    pCollider.center = new Vector3(-0.006775737f, -0.09381586f, -1.192093e-07f);
                }
            }

        }
    }
    #endregion

    void STAIRS(Vector3 rayDir) 
    {
        //uses raycasts to check if the player is hitting stairs and if they should go up
        RaycastHit hitlower;
        if (Physics.Raycast(stepRayLower.transform.position, transform.TransformDirection(rayDir), out hitlower, stepDistance, stairMask)) //check lower bound
        {
            RaycastHit hitUpper;
            if (!Physics.Raycast(stepRayUpper.transform.position, transform.TransformDirection(rayDir), out hitUpper, stepDistance + stepDistance, stairMask)) //check upper bound
            {
                ///rb.MovePosition(rb.position - new Vector3(0f, -stepSmooth * Time.deltaTime, 0f)); //move to normal, world up
                rb.MovePosition(rb.position - (transform.up * -stepSmooth) * Time.deltaTime); //Move towards relative up
            }
        }

        //debug draws raycasts 
        Debug.DrawRay(stepRayLower.transform.position, transform.TransformDirection(rayDir));
        Debug.DrawRay(stepRayUpper.transform.position, transform.TransformDirection(rayDir));
    }

    #region JUMP
    void JUMP() // handles the player jump input, begins the animation process
    {
        if (!enableJumping) return;

        if (Input.GetButtonDown(jumpA) && grounded || Input.GetButtonDown(jumpB) && grounded)
        {
            if (!enableAnim) ApplyJump();
            AnimationJump();
        }

        if (rb.velocity.y < 0) //Fast Fall if no longer going up
            rb.velocity += transform.up * Physics.gravity.y * (fastFallMultiplier - 1) * Time.deltaTime;
        else if (rb.velocity.y > 0 && !Input.GetButton(jumpA) && !Input.GetButton(jumpB)) //Low Jump if no longer pressing jump button
            rb.velocity += transform.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
    }

    public void ApplyJump() // called from an animation event, enables correctly done animated jumping
    {
        if (!enableJumping) return;

        rb.AddForce(transform.up * jumpForce);
    }
    #endregion

    bool isSprinting()
    {
        if (Input.GetButton(sprintA) || Input.GetButton(sprintB))
        {
            if (crouching || crouched)
            {
                if (!isAnythingAboveHead())
                {
                    ApplyCrouch();
                }
                else
                {
                    return false;
                }
            }

            if (!sprintOn) sprintOn = true;
            return true;
        }
        else
        {
            if (sprintOn) sprintOn = false;
            return false;
        }
    }

    bool isGrounded() //checks if grounded 
    {
        if (Physics.SphereCast(transform.position, pCollider.radius, -transform.up, out RaycastHit Hit, 1 + .1f, groundedMask)) return true;
        else
        {
            crouched = false;
            return false;
        }
    }

    bool isAnythingAboveHead()
    {
        if (Physics.SphereCast(transform.position, pCollider.radius, transform.up, out RaycastHit Hit, 0.5f)) return true;
        else return false;
    }

    #region ANIMATION
    void AnimationSetVars()
    {
        if (!enableAnim) return;

        anim.SetBool(animisGroundedHash, grounded);
        anim.SetFloat(animYVelocityHash, rb.velocity.y);
    }

    void AnimationSprintBlend(float movement)
    {
        if (!enableAnim) return;

        if (movement > 1)
        {
            if (sprintOn)
            {
                pim.DisableArmMovers();
                if (moveAdd < 2) moveAdd += 0.1f;
            }
            else
            {
                pim.EnableArmMovers();
                if (moveAdd > 1) moveAdd -= 0.1f;

                if (moveAdd <= 1) moveAdd += 0.1f;
            }
        }
        else
        {
            if (moveAdd > 0)
            {
                if (crouching) moveAdd -= 0.075f;

                else moveAdd -= 0.15f;
            }
        }

        anim.SetFloat(animMoveSpeedHash, moveAdd);
        if (moveAdd > -0.2f && moveAdd < 0.05f)
        {
            anim.SetFloat(animMoveSpeedHash, 0);
        }
        else if (moveAdd > 0.95f && moveAdd < 1.1f)
        {
            anim.SetFloat(animMoveSpeedHash, 1);
        }
        else if (moveAdd > 1.95f && moveAdd < 2.1f)
        {
            anim.SetFloat(animMoveSpeedHash, 2);
        }
    }

    void AnimationCrouchBlend()
    {
        float layerWeight = anim.GetLayerWeight(crouchLayerID);
        float incrementAmount = 0.05f;
        if (crouching || crouched)
        {
            if (layerWeight < 1)
            {
                anim.SetLayerWeight(crouchLayerID, layerWeight + incrementAmount);
            }
            else
            {
                if (crouching && !crouched)
                {
                    crouching = false;
                    crouched = true;
                }
            }
        }
        else
        {
            crouched = false;
            if (layerWeight > 0)
            {
                anim.SetLayerWeight(crouchLayerID, layerWeight - incrementAmount);
            }
        }
    }

    void AnimationJump()
    {
        if (!enableAnim) return;

        anim.SetTrigger(animJumpingHash);

        if (crouching)
        {
            ApplyCrouch();
        }
    }

    public void ApplyRootMotion(Vector3 velocity)
    {
        //rb.drag = 0;
        velocity.y = rb.velocity.y;
        rb.velocity = velocity;
    }
    #endregion

    #region Application Management
    void SetBaseSystems()
    {
        baseSystems = new bool[6];

        baseSystems[0] = enableMovement;
        baseSystems[1] = enableCamera;
        baseSystems[2] = enableJumping;
        baseSystems[3] = enableCrouching;
        baseSystems[4] = enableStairs;
        baseSystems[5] = enableAnim;
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            enableMovement = baseSystems[0];
            enableCamera = baseSystems[1];
            enableJumping = baseSystems[2];
            enableCrouching = baseSystems[3];
            enableStairs = baseSystems[4];
            enableAnim = baseSystems[5];
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            enableMovement = false;
            enableCamera = false;
            enableJumping = false;
            enableCrouching = false;
            enableStairs = false;
            enableAnim = false;
        }
    }
    // can be referenced by anything to end game
    public void ExitGame()
    {
        Application.Quit();
    }
    #endregion
}

