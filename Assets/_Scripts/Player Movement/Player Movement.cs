using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public GameMaster GM;
  
    [Header("Movement")]
    public bool canMove = false;
    public float walkSpeed;
    public float runSpeed;
    public float goalMoveSpeed;
    public float currentMovingSpeed;
    public float groundDrag;
    public Rigidbody rb;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    [SerializeField] bool canJump;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask ground;
    bool grounded;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    [Header("Animations")]
    public Animator playerAnimator;

    [Header("Sounds")]
    public AudioSource PlayerSounds;
    public float Footstepcount = 1.0f;

    [Header("BowAttack")]
    public BowAttack bow;
    public float rotationSpeed;

    void Awake()
    {
        //bow = FindFirstObjectByType<BowAttack>();
        GameObject gm = GameObject.Find("Game Master");
        GM = gm.GetComponent<GameMaster>();
        PlayerSounds.volume = GM.AudioMaster * GM.AudioSFX;
    }

    Vector3 moveDir;
    private void Start()
    {
        
    }
    void Update()
    {

        //Ground Check

        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, ground);

        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;




        /*if (bow == null)
        {
            if (bow.isAiming)
            {
                // RotatePlayerInPlace();
                Aim();

            }
            else
            {

        MyInput();
        MovePlayer();
        SpeedControl();
            }       
        }
        */

        MyInput();
        MovePlayer();
        SpeedControl();

        //Need to run this after MyInput() to stop animations
        //if (InventoryManagementver2.inventoryManagementver2.on)
          //  return;

    }

    private void MyInput()
    {
        horizontalInput = GameMaster.Instance.MoveInput.x;
        verticalInput = GameMaster.Instance.MoveInput.y;

        if (horizontalInput != 0 || verticalInput != 0)
        {
            if (GameMaster.Instance.SprintBeingPressed)
                goalMoveSpeed = runSpeed;
            else
                goalMoveSpeed = walkSpeed;
        }
        else
            goalMoveSpeed = 0;

        currentMovingSpeed = Mathf.Lerp(currentMovingSpeed, goalMoveSpeed, 10 * Time.deltaTime);

        UpdateAnimations();

        if (GameMaster.Instance.JumpJustPressed && canJump && grounded)
        {
            canJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }


    }

    private void MovePlayer()
    {
        //calculates move dir
        moveDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (grounded)
        {
            rb.velocity = (moveDir.normalized * currentMovingSpeed);
            if (rb.velocity.magnitude > 0.1)
            {
                Footstepcount -= Time.deltaTime;
                if (Footstepcount <= 0 && (rb.velocity.x > 0.25f || rb.velocity.z > 0.25f))
                {
                    Footstepcount = 0.25f;
                    PlayerSounds.Play();
                }
            }
        }
        else if (!grounded)
        {
            Vector3 movement = (moveDir.normalized * currentMovingSpeed * airMultiplier);
            rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        //limits velocity if needed
        if (flatVel.magnitude > goalMoveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * goalMoveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
        playerAnimator.SetTrigger("Jump");
    }

    private void ResetJump()
    {
        canJump = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
           
            GameObject activeCamera = GameObject.FindGameObjectWithTag("Camera");
            Camera.main.GetComponent<ThirdPersonCamera>().SwitchCameraStyle(CameraStyle.Combat);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
        {
            GameObject activeCamera = GameObject.FindGameObjectWithTag("Camera");
            Camera.main.GetComponent<ThirdPersonCamera>().SwitchCameraStyle(CameraStyle.Basic);
        }
    }
    public void Aim()
    {
        GameObject activateCamera = GameObject.FindGameObjectWithTag("Camera");
        Camera.main.GetComponent<ThirdPersonCamera>().SwitchCameraStyle(CameraStyle.Aim);
        

    }

    private void UpdateAnimations()
    {
        //Debug.Log(Input.GetAxis("Horizontal"));
        //Debug.Log(currentMovingSpeed);
        playerAnimator.SetFloat("XVel", currentMovingSpeed * Input.GetAxis("Horizontal") / 8);
        playerAnimator.SetFloat("YVel", currentMovingSpeed * Input.GetAxis("Vertical") / 8);
    }
     private void RotatePlayerInPlace()
    {
        // Calculate the rotation based on mouse input
        float mouseX = Input.GetAxis("Mouse X");
        Vector3 rotation = new Vector3(0f, mouseX * rotationSpeed * Time.deltaTime * GameMaster.Instance.MouseSensitiviy, 0f);

        // Apply rotation to the player's orientation
        orientation.Rotate(rotation);
    }
    
}
