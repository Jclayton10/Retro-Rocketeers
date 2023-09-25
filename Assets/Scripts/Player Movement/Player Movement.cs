using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
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

    Vector3 moveDir;
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
        if (InventoryManagement.inventoryManagement.on)
            return;

        MovePlayer();
        SpeedControl();
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

        if(Input.GetKey(jumpKey) && canJump && grounded)
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

        if(grounded)
            rb.AddForce(moveDir.normalized * currentMovingSpeed, ForceMode.Force);
        else if(!grounded)
            rb.AddForce(moveDir.normalized * currentMovingSpeed * airMultiplier, ForceMode.Force);
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
            GameObject activeCamera = GameObject.FindGameObjectWithTag("Camera");
            activeCamera.GetComponent<ThirdPersonCamera>().SwitchCameraStyle(CameraStyle.Combat);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
        {
            GameObject activeCamera = GameObject.FindGameObjectWithTag("Camera");
            activeCamera.GetComponent<ThirdPersonCamera>().SwitchCameraStyle(CameraStyle.Basic);
        }
    }

    private void UpdateAnimations()
    {
        Debug.Log(Input.GetAxis("Horizontal"));
        Debug.Log(currentMovingSpeed);
        playerAnimator.SetFloat("XVel", currentMovingSpeed * Input.GetAxis("Horizontal") / 8);
        playerAnimator.SetFloat("YVel", currentMovingSpeed * Input.GetAxis("Vertical") / 8);
    }
}
