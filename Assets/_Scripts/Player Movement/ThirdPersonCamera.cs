//Adapted from this video: https://www.youtube.com/watch?v=UCwwn2q4Vys

using Cinemachine;
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

    [Header("BowAttack")]
    public float bowRotationSpeed;
    [HideInInspector]
    private BowAttack bow;


    private void Awake()
    {
        bow = FindFirstObjectByType<BowAttack>();
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        basicCamera.GetComponent<CinemachineFreeLook>().m_XAxis.m_MaxSpeed = 300 * GameMaster.Instance.MouseSensitiviy;
        basicCamera.GetComponent<CinemachineFreeLook>().m_YAxis.m_MaxSpeed = 2 * GameMaster.Instance.MouseSensitiviy;
        combatCamera.GetComponent<CinemachineFreeLook>().m_XAxis.m_MaxSpeed = 300 * GameMaster.Instance.MouseSensitiviy;
        combatCamera.GetComponent<CinemachineFreeLook>().m_YAxis.m_MaxSpeed = 2 * GameMaster.Instance.MouseSensitiviy;
        buildingCamera.GetComponent<CinemachineFreeLook>().m_XAxis.m_MaxSpeed = 300 * GameMaster.Instance.MouseSensitiviy;
        buildingCamera.GetComponent<CinemachineFreeLook>().m_YAxis.m_MaxSpeed = 2 * GameMaster.Instance.MouseSensitiviy;
    }

    private void FixedUpdate()
    {
        //if (InventoryManagementver2.inventoryManagementver2.on)
         //return;

        // Rotate orientation based on player's position
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        if (currentStyle == CameraStyle.Basic || currentStyle == CameraStyle.Building)
        {
            float horizontalInput = -GameMaster.Instance.MoveInput.x;
            float verticalInput = -GameMaster.Instance.MoveInput.y;
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
        if (currentStyle != CameraStyle.Building)
        {
            if (Input.GetMouseButtonDown(0))
            {
                // If the aim input condition is met, switch to Aim style
                SwitchCameraStyle(CameraStyle.Aim);
            }
        }
        

        if (currentStyle == CameraStyle.Aim)
        {
            //Aim();
            if (!Input.GetMouseButtonDown(0))
                SwitchCameraStyle(CameraStyle.Basic);
        }
        

        // Check for the build mode input condition
        if (GameMaster.Instance.BuildModePressed)
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
            aimCamera.transform.position = aimCamera.transform.position;
            aimCamera.SetActive(true);
        }
        else
        {
            buildingCamera.SetActive(true);
        }

        //BuildingSystem.buildingSystem.ToggleBuildMode();
        Debug.Log(currentStyle);
    }
    public void Aim()
    {
        if (bow != null)
        {
        
         // Set the aimCamera's position to match the fire point's position
         //aimCamera.transform.position = bow.firePoint.position;

         float mouseY = Input.GetAxis("Mouse Y"); // Get the vertical mouse input
         float mouseX = Input.GetAxis("Mouse X"); // Get the horizontal mouse input

         // Get the current rotation of the aimCamera
         Vector3 currentRotation = aimCamera.transform.localEulerAngles;

         // Calculate the new pitch (vertical rotation)
         float newPitch = currentRotation.x - mouseY * bowRotationSpeed * Time.deltaTime;
         float newYaw = currentRotation.y + mouseX * bowRotationSpeed * Time.deltaTime;

         // Clamp the pitch and yaw to prevent flipping and over-rotating
         newPitch = Mathf.Clamp(newPitch, -90f, 90f); // Adjust the angle limits as needed
         newYaw = Mathf.Clamp(newYaw, -90f, 90f);

         // Set the new rotation
         aimCamera.transform.localEulerAngles = new Vector3(newPitch, newYaw, currentRotation.z);
            
        }


    }
   


    public Ray GetRayFromMousePosition(Vector3 mousePosition)
    {
        // Use ScreenPointToRay or other logic to create the ray from the mouse position
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        return ray;
    } 
   
    
}
