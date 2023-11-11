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

    void Update()
    {
        PlayAnimations();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (isWeapomDrawn && other.CompareTag("Enemy"))
        {
            Enemy enemyHealth = other.GetComponent<Enemy>();

            if (enemyHealth != null)
            {
                // Apply the damage to the enemy
                enemyHealth.TakeDamage(attackDamage);
                Debug.Log("EnemyHealth: " + enemyHealth.ToString());
            }
        }
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