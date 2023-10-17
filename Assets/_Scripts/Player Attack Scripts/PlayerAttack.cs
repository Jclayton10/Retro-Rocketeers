using System.Collections;
using System.Collections.Generic;
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
      
    }
    void PlayAnimations()
    {
        if (isWeaponShethed==true &&Input.GetKeyDown(KeyCode.R)) // Change the input key as needed
        {
            isWeapomDrawn = true;
            isWeaponShethed = false;
           
            weaponHoslter.SetActive(false);
            weaponDraw.SetActive(true);
       
            playerAttack.SetTrigger("Withdraw");

        }
        if (isWeapomDrawn == true && Input.GetKeyDown(KeyCode.E ))
        {
            isWeapomDrawn = false;
            isWeaponShethed = true;

            weaponHoslter.SetActive(true);
            weaponDraw.SetActive(false);

            playerAttack.SetTrigger("Sheathing");
        }
        if (isWeapomDrawn == true && Input.GetKeyDown(KeyCode.Mouse0))
        {
            playerAttack.SetTrigger("Attack");
            Attack();
        }
    }
    void Attack()
    {

    }
    
}
