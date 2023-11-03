using UnityEngine;

public class BowAttack : MonoBehaviour
{
    public GameObject arrowPrefab;
    public Transform firePoint;
    public float bowForce = 30.0f;
    public float fireRate = 1.0f;
    private float nextFireTime;

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
            Debug.Log("isAiming: " + isAiming);
        }

        if (Input.GetMouseButtonUp(1))
        {
            isAiming = false;
            Debug.Log("isAiming: " + isAiming);
        }

        if (isAiming)
        {
            // Fire when the left mouse button is released
            if (GameMaster.Instance.AttackJustPressed)
            {
                Fire();
            }
        }
    }

    void Fire()
    {
        if (Time.time > nextFireTime)
        {
            GameObject arrowClone = Instantiate(arrowPrefab, firePoint.position, Quaternion.identity);
            Rigidbody rb = arrowClone.GetComponent<Rigidbody>();
            
            if (rb != null)
            {
                rb.AddForce(-transform.right * bowForce, ForceMode.Impulse);

            }
            
            nextFireTime = Time.time + 1.0f / fireRate;
        }
    }
}
