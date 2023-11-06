using UnityEngine;

public class Arrow : MonoBehaviour
{
    public int damageAmount; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();

            if (enemy != null)
            {
                enemy.TakeDamage(damageAmount);
            }

            // Always destroy the arrow upon hitting an enemy
            Destroy(gameObject);
        }
    }
}
