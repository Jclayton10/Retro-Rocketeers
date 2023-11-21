using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalAchivments : MonoBehaviour
{
 

    //Genral Variables
    public GameObject achNote;
    public GameObject achTitle;
    public GameObject achDesc;
    public GameObject achImage;

    public AudioSource achsound;
    
    public bool achActive = false;

    [Header("Colleted 5 Wood Achievement")]
    //Achievemt 01 Specific
    public static int ach01Count;
    public int ach01Trigger = 5;
    public int ach01Code;



    void Update()
    {
        
        
        if (ach01Count == ach01Trigger && ach01Code != 1)
        {
            ach01Code = PlayerPrefs.GetInt("Ach01");
            StartCoroutine(Collected5Wood());
        }
    }
    IEnumerator Collected5Wood()
    {
        achActive = true;
        ach01Code = 1;
        PlayerPrefs.SetInt("Collected5Wood", ach01Code);
        achsound.Play();
        achImage.SetActive(true);
        achTitle.GetComponent<Text>().text = "FIRST TREE CUT!";
        achDesc.GetComponent<Text>().text = "You collected 5 wood!";
        achNote.SetActive(true);
       
        yield return new WaitForSeconds(4);

        //Reset UI
        ResetUI();
      
    }
    
    public void ResetUI()
    {
        achNote.SetActive(false);
        achImage.SetActive(false);
        achTitle.GetComponent<Text>().text = "";
        achDesc.GetComponent<Text>().text = "";
        achActive = false;
    }
}
