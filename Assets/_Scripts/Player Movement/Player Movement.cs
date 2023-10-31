using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public GameMaster GM;
    [Header("Health")]
    public int playerHealth;
    private int currentPlayerHealth;
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

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode runKey = KeyCode.LeftShift;

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

    void Awake()
    {
        bow = FindFirstObjectByType<BowAttack>();
        GameObject gm = GameObject.Find("Game Master");
        GM = gm.GetComponent<GameMaster>();
        PlayerSounds.volume = GM.AudioMaster * GM.AudioSFX;
        jumpKey = GM.jumpKey;
        runKey = GM.runKey;
    }

    Vector3 moveDir;
    private void Start()
    {
        currentPlayerHealth = playerHealth;
    }
    void Update()
    {

        //Ground Check

        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, ground);

        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;

        MyInput();

        //Need to run this after MyInput() to stop animations
        //if (InventoryManagement.inventoryManagement.on)
        // return;

        MovePlayer();
        SpeedControl();

        //if(bow.isAiming)
        // {

        //  Aim();
        //  }

    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (horizontalInput != 0 || verticalInput != 0)
        {
            if (Input.GetKey(runKey))
                goalMoveSpeed = runSpeed;
            else
                goalMoveSpeed = walkSpeed;
        }
        else
            goalMoveSpeed = 0;

        currentMovingSpeed = Mathf.Lerp(currentMovingSpeed, goalMoveSpeed, 10 * Time.deltaTime);

        UpdateAnimations();

        if (Input.GetKey(jumpKey) && canJump && grounded)
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
            Footstepcount -= Time.deltaTime;
            if (Footstepcount <= 0)
            {
                Footstepcount = 0.25f;
                PlayerSounds.Play();
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
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
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
            other.gameObject.GetComponent<BoxCollider>();
            GameObject activeCamera = GameObject.FindGameObjectWithTag("Camera");
            Camera.main.GetComponent<ThirdPersonCamera>().SwitchCameraStyle(CameraStyle.Combat);
        }
    }
    private void OnCollisionEnter(Collision collider)
    {
        if (collider.gameObject.CompareTag("Enemy"))
        {
            // Get the Enemy script component from the collided enemy object
            Enemy enemy = collider.gameObject.GetComponent<Enemy>();

            if (enemy != null)
            {
                // Apply damage to the player based on the enemy's damage amount
                playerHealth -= enemy.damageAmount;
                Debug.Log("PlayerMovment/PlayerHealth:" + playerHealth);
                // Call a function to handle the player's health and death logic
                //HandleHealthAndDeath();
            }
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
        Debug.Log(Input.GetAxis("Horizontal"));
        Debug.Log(currentMovingSpeed);
        playerAnimator.SetFloat("XVel", currentMovingSpeed * Input.GetAxis("Horizontal") / 8);
        playerAnimator.SetFloat("YVel", currentMovingSpeed * Input.GetAxis("Vertical") / 8);
    }
}
