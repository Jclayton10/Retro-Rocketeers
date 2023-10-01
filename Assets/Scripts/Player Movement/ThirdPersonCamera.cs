//Adapted from this video: https://www.youtube.com/watch?v=UCwwn2q4Vys

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraStyle
{
    Basic,
    Combat,
    Building
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

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void FixedUpdate()
    {
        if (InventoryManagement.inventoryManagement.on)
            return;

        //rotate orientation
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        if(currentStyle == CameraStyle.Basic || currentStyle == CameraStyle.Building)
        {
            //rotate player object
            float horizontalInput = -Input.GetAxis("Horizontal");
            float verticalInput = -Input.GetAxis("Vertical");
            Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

            if (inputDir != Vector3.zero)
                playerObj.forward = Vector3.Slerp(playerObj.forward, -inputDir.normalized, Time.deltaTime * rotationSpeed);
        }
        else if(currentStyle == CameraStyle.Combat)
        {
            Vector3 dirToCombatLookAt = combatLookAt.position - new Vector3(transform.position.x, combatLookAt.position.y, transform.position.z);
            orientation.forward = dirToCombatLookAt.normalized;

            playerObj.forward = dirToCombatLookAt.normalized;
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            //Want to stay in Combat if applicable
            if(currentStyle != CameraStyle.Combat)
            {
                //If currently building, switch to walking
                if(currentStyle == CameraStyle.Building)
                {
                    SwitchCameraStyle(CameraStyle.Basic);
                    currentStyle = CameraStyle.Basic;
                }
                //If currently walking, switch to building
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
        Debug.Log(newStyle);
        //Turns off all cameras
        combatCamera.SetActive(false);
        basicCamera.SetActive(false);
        buildingCamera.SetActive(false);

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
        else
            buildingCamera.SetActive(true);

        Debug.Log(currentStyle);
    }
}
