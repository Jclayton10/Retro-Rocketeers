using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponActivate : MonoBehaviour
{
    public static WeaponActivate weaponController;
    public Animator playerAttackAim;
    public GameObject weaponHoslter;
    public GameObject weaponGrip;
    public bool isAttacking= false;
    bool isCooldown = false;
   
    private bool isWeapomDrawn;
    private bool isWeaponShethed;

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
        }
        else if (isWeapomDrawn == true && GameMaster.Instance.SheathJustPressed)
        {
            playerAttackAim.SetTrigger("Sheathing");

            isWeapomDrawn = false;
            isWeaponShethed = true;

            weaponHoslter.SetActive(true);
            weaponGrip.SetActive(false);
            
        }
        else if (isWeapomDrawn == true && GameMaster.Instance.AttackJustPressed&& !isAttacking&&!isCooldown)
        {
           playerAttackAim.SetTrigger("Attack");
            isAttacking = true;
            StartCoroutine(Cooldown());
        }
       
       
    }
    IEnumerator Cooldown()
    {
        isCooldown = true;
        yield return new WaitForSeconds(1.0f);
        isCooldown = false;
        isAttacking=false;
    }
}
