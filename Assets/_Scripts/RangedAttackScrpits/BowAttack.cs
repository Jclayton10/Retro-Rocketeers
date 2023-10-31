using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BowAttack : MonoBehaviour
{
    public GameObject arrow;
    public Transform firePoint;
    public GameObject bow; // Assign the bow GameObject in the Unity Inspector

    public float bowForce;
    public float fireRate = 1.0f;
    public float rotationSpeed = 5.0f;
    private float nextFireTime;
    public int damageAmount;

    [HideInInspector]
    public bool isAiming = false;

    void OnGUI()
    {
        if (isAiming)
        {
            // Draw a crosshair in the center of the screen
            float crosshairSize = 20;
            float crosshairX = Screen.width / 2 - crosshairSize / 2;
            float crosshairY = Screen.height / 2 - crosshairSize / 2;

            GUI.Box(new Rect(crosshairX, crosshairY, crosshairSize, crosshairSize), "");
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isAiming = true;
            Debug.Log("isAiming:"+isAiming);
        }

        if (Input.GetMouseButtonUp(1))
        {
            isAiming = false;
            Debug.Log("isAiming:" + isAiming);
            // Fire when the left mouse button is released
            Vector3 fireDirection = Camera.main.transform.forward;
            Fire(fireDirection);
        }

        /*if (isAiming)
        {
            RotateBow();
        }
        */
        
    }

    void Fire(Vector3 direction)
    {
        if (Time.time > nextFireTime)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Calculate the direction to the hit point
                Vector3 targetPoint = hit.point;
                direction = (targetPoint - firePoint.position).normalized;

                // Create the arrow clone at the firePoint position
                GameObject arrowClone = Instantiate(arrow, firePoint.position, Quaternion.identity);
                Rigidbody rb = arrowClone.GetComponent<Rigidbody>();

                if (rb != null)
                {
                    rb.AddForce(direction * bowForce, ForceMode.Impulse);
                }
            }

            nextFireTime = Time.time + 1.0f / fireRate;
        }
    }

     //public void RotateBow()
     /*{
         float mouseY = Input.GetAxis("Mouse Y"); // Get the vertical mouse input
         Vector3 rotation = bow.transform.localEulerAngles;
         rotation.z += mouseY * rotationSpeed * Time.deltaTime;
         rotation.z = Mathf.Clamp(rotation.z, -90f, 90f); // Limit the pitch rotation to prevent flipping
         bow.transform.localEulerAngles = rotation;
     }
     */
   


}
