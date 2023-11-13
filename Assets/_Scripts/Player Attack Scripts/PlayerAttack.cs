using Newtonsoft.Json.Linq;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.Playables;

public class PlayerAttack : MonoBehaviour
{

    public Animator playerAttack;
    public GameObject weaponHoslter;
    public GameObject weaponDraw;
    public EnemyMovment enemyMovment;

    public int attackDamage = 10;
    public int knockbackForce = 5;

    public float flashDuration = 0.1f;
    public Color flashColor = Color.red;

    private bool isWeapomDrawn;
    private bool isWeaponShethed;

    // Flag to check if force has been applied
    private bool forceApplied = false;

    private void Awake()
    {
        weaponDraw.SetActive(false);
        // Ensure you have a material that supports color changes on the enemy GameObject
        // If not, create a new material and assign it to the enemy's Renderer component
        // enemy.GetComponent<Renderer>().material = new Material(Shader.Find("Standard"));
        isWeapomDrawn = false;
        isWeaponShethed = true;
    }

    private void Update()
    {
        PlayAnimations();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemyHealth = other.GetComponent<Enemy>();
            Rigidbody rb = other.GetComponent<Rigidbody>();
            enemyMovment = other.GetComponent<EnemyMovment>();

            if (enemyHealth != null && rb != null)
            {
                // Apply the damage to the enemy
                enemyHealth.TakeDamage(attackDamage);
                Debug.Log("EnemyHealth: " + enemyHealth.ToString());

                // Flash the enemy red
                FlashEnemy(other.gameObject);

                // Disable NavMeshAgent temporarily

                // Apply combined forces only if force hasn't been applied yet
                if (!forceApplied)
                {
                    rb.AddForce((other.transform.forward * knockbackForce), ForceMode.Impulse);
                    forceApplied = true; // Set the flag to true to indicate force application
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (enemyMovment != null && enemyMovment.agent != null)
        {
            enemyMovment.agent.enabled = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (enemyMovment != null && enemyMovment.agent != null)
        {
            enemyMovment.agent.enabled = true;

            // Reset the flag when the enemy exits the trigger zone
            forceApplied = false;
        }
    }

    void FlashEnemy(GameObject enemy)
    {
        Renderer enemyRenderer = enemy.GetComponent<Renderer>();
        if (enemyRenderer != null)
        {
            // Get the original material or color
            Material originalMaterial = enemyRenderer.material;

            // Create a new material with the appropriate shader
            Material flashMaterial = new Material(Shader.Find("Standard"));
            flashMaterial.color = flashColor;

            // Assign the new material to the enemy
            enemyRenderer.material = flashMaterial;

            // Use a coroutine to reset the color after a short duration
            StartCoroutine(ResetMaterial(enemyRenderer, originalMaterial, flashMaterial, flashDuration));
        }
    }

    IEnumerator ResetMaterial(Renderer renderer, Material originalMaterial, Material flashMaterial, float duration)
    {
        // Wait for the specified duration
        yield return new WaitForSeconds(duration);

        // Reset the material
        renderer.material = originalMaterial;
    }

    void PlayAnimations()
    {
        if (isWeaponShethed == true && GameMaster.Instance.SheathJustPressed) // Change the input key as needed
        {
            Debug.Log("Unsheating!");
            isWeapomDrawn = true;
            isWeaponShethed = false;

            weaponHoslter.SetActive(false);
            weaponDraw.SetActive(true);

            playerAttack.SetTrigger("Withdraw");
        }
        else if (isWeapomDrawn == true && GameMaster.Instance.SheathJustPressed)
        {
            isWeapomDrawn = false;
            isWeaponShethed = true;

            weaponHoslter.SetActive(true);
            weaponDraw.SetActive(false);

            playerAttack.SetTrigger("Sheathing");
        }
        else if (isWeapomDrawn == true && GameMaster.Instance.AttackJustPressed)
        {
            playerAttack.SetTrigger("Attack");
        }
    }


}
