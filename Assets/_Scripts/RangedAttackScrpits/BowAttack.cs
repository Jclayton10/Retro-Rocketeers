using System.Collections.Generic;
using UnityEngine;

public class BowAttack : MonoBehaviour
{
    public GameObject arrowPrefab;
    public GameObject Player;

    public Transform firePoint;

    public float bowForce = 30.0f;
    public float fireRate = 1.0f;
    public float bowRotationSpeed;
    private float nextFireTime;
  

    public bool isAiming = false;

    // Crosshair settings
    public Texture2D crosshairTexture;
    public float crosshairSize = 20;

   

    private void Start()
    {
    
    }

    private void Update()
    {
        

        if (Input.GetMouseButtonDown(1))
        {
            
            isAiming = true;
           

            Debug.Log("isAiming: " + isAiming);
        }

        else if (Input.GetMouseButtonUp(1))
        {
            isAiming = false;
            Debug.Log("isAiming: " + isAiming);
        }

        else if (GameMaster.Instance.AttackJustPressed&&isAiming)
        {
            
            // Fire when the left mouse button is pressed
            Fire();
        }
    }

    void Fire()
    {
        if (Time.time > nextFireTime)
        {
            GameObject arrowClone = Instantiate(arrowPrefab, firePoint.position, Quaternion.identity);
            Rigidbody rb = arrowClone.GetComponent <Rigidbody>();

            if (rb != null)
            {
                rb.AddForce(transform.right * bowForce, ForceMode.Impulse);
            }

            nextFireTime = Time.time + 1.0f / fireRate;
        }
    }

    void RotateBow()
    {
        float mouseY = Input.GetAxis("Mouse Y"); // Get the vertical mouse input
        float mouseX = Input.GetAxis("Mouse X"); // Get the horizontal mouse input

        // Get the current rotation of the bow
        Vector3 currentRotation = transform.localEulerAngles;

        // Calculate the new pitch (vertical rotation)
        float newPitch = currentRotation.x - mouseY * bowRotationSpeed * Time.deltaTime;
        float newYaw = currentRotation.y + mouseX * bowRotationSpeed * Time.deltaTime;

        // Clamp the pitch and yaw to prevent flipping and over-rotating
        //newPitch = Mathf.Clamp(newPitch, -90f, 90f); // Adjust the angle limits as needed
       newYaw = Mathf.Clamp(newYaw, -135f, 0f); // Adjust the angle limits as needed

        // Set the new rotation
        transform.localEulerAngles = new Vector3(currentRotation.x, newYaw, currentRotation.z);
    }

    private void OnGUI()
    {
        // Calculate the GUI position of the crosshair based on the mouse position
        Vector2 mousePosition = Event.current.mousePosition;
        float crosshairX = mousePosition.x - crosshairSize / 2;
        float crosshairY = mousePosition.y - crosshairSize / 2;

        // Draw the crosshair at the calculated position
        GUI.DrawTexture(new Rect(crosshairX, crosshairY, crosshairSize, crosshairSize), crosshairTexture);
    }

}
