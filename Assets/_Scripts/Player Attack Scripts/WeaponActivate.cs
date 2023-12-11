using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponActivate : MonoBehaviour
{
    public static WeaponActivate weaponController;
    public Animator playerAttackAim;
    public GameObject weaponHoslter;
    public GameObject weaponGrip;

   
    private bool isWeapomDrawn;
    private bool isWeaponShethed;
    public bool isAttacking;

    private void Awake()
    {
        weaponController = this;
        weaponGrip.SetActive(false);
        isWeapomDrawn = false;
        isWeaponShethed = true;

    }

    private void Update()
    {
        PlayAnimations();
    }



    void PlayAnimations()
    {
        if (isWeaponShethed == true && GameMaster.Instance.SheathJustPressed)
        {
            Debug.Log("Unsheating!");
            isWeapomDrawn = true;
            isWeaponShethed = false;

            weaponHoslter.SetActive(false);
            weaponGrip.SetActive(true);
            playerAttackAim.SetTrigger("Withdraw");
            isAttacking = false;
        }
        else if (isWeapomDrawn == true && GameMaster.Instance.SheathJustPressed)
        {
            playerAttackAim.SetTrigger("Sheathing");

            isWeapomDrawn = false;
            isWeaponShethed = true;

            weaponHoslter.SetActive(true);
            weaponGrip.SetActive(false);
            isAttacking = false;
            
        }
        else if (isWeapomDrawn == true && GameMaster.Instance.AttackJustPressed)
        {
           isAttacking = true;
           playerAttackAim.SetTrigger("Attack");
        }
        
       
     
    }
   
    
   
    
}
