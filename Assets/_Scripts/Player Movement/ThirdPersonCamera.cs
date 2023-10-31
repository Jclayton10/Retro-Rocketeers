//Adapted from this video: https://www.youtube.com/watch?v=UCwwn2q4Vys

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraStyle
{
    Basic,
    Combat,
    Building,
    Aim
}

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform player;
    public Transform playerObj;
    public Rigidbody rb;

    public float rotationSpeed;

    public Transform combatLookAt;
    public CameraStyle currentStyle;

    [Header("Cameras")]
    public GameObject basicCamera;
    public GameObject combatCamera;
    public GameObject buildingCamera;
    public GameObject aimCamera;

    [HideInInspector]
    private bool isAiming;
    public BowAttack bow;
    

    private void Awake()
    {
        bow=FindFirstObjectByType<BowAttack>();
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void FixedUpdate()
    {
        // if (InventoryManagement.inventoryManagement.on)
        //  return;

        // Rotate orientation based on player's position
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        if (currentStyle == CameraStyle.Basic || currentStyle == CameraStyle.Building)
        {
            float horizontalInput = -Input.GetAxis("Horizontal");
            float verticalInput = -Input.GetAxis("Vertical");
            Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

            if (inputDir != Vector3.zero)
            {
                playerObj.forward = Vector3.Slerp(playerObj.forward, -inputDir.normalized, Time.deltaTime * rotationSpeed);
            }

            // Check for the aim input condition (e.g., right mouse button)
            
        }
        else if (currentStyle == CameraStyle.Combat)
        {
            Vector3 dirToCombatLookAt = combatLookAt.position - new Vector3(transform.position.x, combatLookAt.position.y, transform.position.z);
            orientation.forward = dirToCombatLookAt.normalized;

            playerObj.forward = dirToCombatLookAt.normalized;
        }
        else if(currentStyle == CameraStyle.Building)
        {
            if (Input.GetMouseButtonDown(0))
            {
                // If the aim input condition is met, switch to Aim style
                SwitchCameraStyle(CameraStyle.Aim);
                currentStyle = CameraStyle.Aim;
            }
        }

        if(currentStyle == CameraStyle.Aim)
            Aim();

        // Check for the build mode input condition
        if (Input.GetKeyDown(GameMaster.Instance.buildModeKey))
        {
            // Want to stay in Combat if applicable
            if (currentStyle != CameraStyle.Combat)
            {
                // If currently building, switch to walking
                if (currentStyle == CameraStyle.Building)
                {
                    SwitchCameraStyle(CameraStyle.Basic);
                    currentStyle = CameraStyle.Basic;
                }
                // If currently walking, switch to building
                else
                {
                    SwitchCameraStyle(CameraStyle.Building);
                    currentStyle = CameraStyle.Building;
                }
            }
        }
    }

    public void SwitchCameraStyle(CameraStyle newStyle)
    {
        currentStyle = newStyle;

        Debug.Log(newStyle);
        //Turns off all cameras
        combatCamera.SetActive(false);
        basicCamera.SetActive(false);
        buildingCamera.SetActive(false);
        aimCamera.SetActive(false);
        

        //Sets the chosen camera to active
        if (newStyle == CameraStyle.Basic)
        {
            basicCamera.SetActive(true);
            basicCamera.transform.position = combatCamera.transform.position;
        }
        else if (newStyle == CameraStyle.Combat)
        {
            combatCamera.transform.position = basicCamera.transform.position;
            combatCamera.SetActive(true);
        }
        else if (newStyle == CameraStyle.Aim)
        {
            aimCamera.transform.position = basicCamera.transform.position;
            aimCamera.SetActive(true);
        }
        else
        {
            buildingCamera.SetActive(true);
        }

        BuildingSystem.buildingSystem.ToggleBuildMode();
        Debug.Log(currentStyle);
    }
    public void Aim()
    {
        float mouseY = Input.GetAxis("Mouse Y"); // Get the vertical mouse input

        // Get the current rotation of the aimCamera
        Vector3 currentRotation = aimCamera.transform.localEulerAngles;

        // Calculate the new pitch (vertical rotation)
        float newPitch = currentRotation.x - mouseY * rotationSpeed * Time.deltaTime;

        // Clamp the pitch to prevent flipping
        newPitch = Mathf.Clamp(newPitch, 0, -90f);

        // Set the new rotation
        aimCamera.transform.localEulerAngles = new Vector3(newPitch, currentRotation.y, currentRotation.z);

        // Check if aiming is still active
        if (Mathf.Abs(mouseY) > 0.01f)
        {
            // Player is actively aiming
            isAiming = true;
        }
        else
        {
            // Player is not aiming anymore, switch to "Basic" camera
            isAiming = false;
            SwitchCameraStyle(CameraStyle.Basic);
            currentStyle = CameraStyle.Basic;
        }
    }
}
