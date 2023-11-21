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


    //Achievemt 01 Specific
    public static int ach01Count;
    public int ach01Trigger = 5;
    public int ach01Code;



    void Update()
    {
        Debug.Log("Update - Collected5Wood: " + ach01Count);
        
        if (ach01Count == ach01Trigger && ach01Code != 12345)
        {
            ach01Code = PlayerPrefs.GetInt("Ach01");
            StartCoroutine(Collected5Wood());
        }
    }
    IEnumerator Collected5Wood()
    {
        Debug.Log("Collected5Wood- Starting");
        achActive = true;
        ach01Code = 12345;
        PlayerPrefs.SetInt("Collected5Wood", ach01Code);
        achsound.Play();
        achImage.SetActive(true);
        achTitle.GetComponent<Text>().text = "FIRST TREE CUT!";
        achDesc.GetComponent<Text>().text = "You collected 5 wood!";
        achNote.SetActive(true);
        Debug.Log("Collected5Wood - Waiting for 7 seconds");
        yield return new WaitForSeconds(4);

        Debug.Log("Collected5Wood - Resetting UI");
        //Reset UI
        ResetUI();
        Debug.Log("Collected5Wood - Completed");
    }

    /*IEnumerator Trigger02Ach()
    {
        achActive = true;
        achsound.Play();
        achTitle.GetComponent<Text>().text = "COLLECTION";
        achDesc.GetComponent<Text>().text = "Created a collection based achivment";
        achNote.SetActive(true);
        yield return new WaitForSeconds(5);
        //Reset UI
        ResetUI();


    }
    */
    public void ResetUI()
    {
        achNote.SetActive(false);
        achImage.SetActive(false);
        achTitle.GetComponent<Text>().text = "";
        achDesc.GetComponent<Text>().text = "";
        achActive = false;
    }
}
