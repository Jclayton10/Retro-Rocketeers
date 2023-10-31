using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    public Animator playerAttack;

    public GameObject weaponHoslter;
    public GameObject weaponDraw;

    public int attackDamage = 10;
    private bool isWeapomDrawn;
    private bool isWeaponShethed;
    private void Awake()
    {
        weaponDraw.SetActive(false);
        weaponDraw.GetComponent<MeshCollider>();
        isWeapomDrawn = false;
        isWeaponShethed = true;
    }
    private void Start()
    {

    }
    void Update()
    {
        PlayAnimations();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (isWeapomDrawn && other.CompareTag("Enemy"))
        {
            // Calculate the damage (using your attackDamage variable)
            int damage = attackDamage;

            // Get the enemy's script that handles taking damage
            Enemy enemyHealth = other.GetComponent<Enemy>();

            // Check if the enemy has a script to take damage
            if (enemyHealth != null)
            {
                // Apply the damage to the enemy
                enemyHealth.TakeDamage(damage);
                Debug.Log("EnemyHealth: " + enemyHealth.ToString());
            }
        }
    }
    void PlayAnimations()
    {
        if (isWeaponShethed == true && Input.GetKeyDown(GameMaster.Instance.sheathKey)) // Change the input key as needed
        {
            isWeapomDrawn = true;
            isWeaponShethed = false;

            weaponHoslter.SetActive(false);
            weaponDraw.SetActive(true);

            playerAttack.SetTrigger("Withdraw");

        }
        else if (isWeapomDrawn == true && Input.GetKeyDown(GameMaster.Instance.sheathKey))
        {
            isWeapomDrawn = false;
            isWeaponShethed = true;

            weaponHoslter.SetActive(true);
            weaponDraw.SetActive(false);

            playerAttack.SetTrigger("Sheathing");
        }
        else if (isWeapomDrawn == true && Input.GetKeyDown(GameMaster.Instance.attackKey))
        {
            playerAttack.SetTrigger("Attack");

        }
    }


}
