using System;
using System.Collections;
using UnityEngine;

public class WeaponActivate : MonoBehaviour
{
    public Animator playerAttack;
    public GameObject weaponHoslter;
    public GameObject weaponGrip;

    public AudioSource weaponSoundSource;
    public AudioClip weaponShealthSound;
    public AudioClip weaponDrawSound;


   

    private bool isWeapomDrawn;
    private bool isWeaponShethed;

    private void Awake()
    { 
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
            playerAttack.SetTrigger("Withdraw");
        }
        else if (isWeapomDrawn == true && GameMaster.Instance.SheathJustPressed)
        {
            playerAttack.SetTrigger("Sheathing");

            isWeapomDrawn = false;
            isWeaponShethed = true;

            weaponHoslter.SetActive(true);
            weaponGrip.SetActive(false);
        }
        else if (isWeapomDrawn == true && GameMaster.Instance.AttackJustPressed)
        {
            isWeapomDrawn = true;
            weaponGrip.SetActive(true);
            weaponHoslter.SetActive(false);
            playerAttack.SetTrigger("Attack");
            
        }
    }


}