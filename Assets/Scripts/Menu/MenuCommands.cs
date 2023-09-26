using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuCommands : MonoBehaviour
{
    public GameObject MenuProp;
    public GameObject MainBunch;
    public GameObject CreditBunch;
    //public GameObject SettingsBunch;

    public GameObject AudioEmmisions;

    #region Fade
    public CanvasGroup CanvasGroup;
    public bool fadeOut = false;
    public bool fadeIn = false;

    public float TimeToFade;

    void Update()
    {
        if (fadeIn)
        {
            if (CanvasGroup.alpha < 1)
            {
                AudioEmmisions.GetComponent<AudioSource>().volume -= TimeToFade * Time.deltaTime;
                CanvasGroup.alpha += TimeToFade * Time.deltaTime;
                if (CanvasGroup.alpha >= 1)
                {
                    fadeIn = false;
                }
            }
        }
        if (fadeOut)
        {
            if (AudioEmmisions.GetComponent<AudioSource>().volume < 0.8)
            {
                AudioEmmisions.GetComponent<AudioSource>().volume += TimeToFade * Time.deltaTime;
            }
            if (CanvasGroup.alpha >= 0)
            {
                CanvasGroup.alpha -= TimeToFade * Time.deltaTime;
                if (CanvasGroup.alpha <= 0)
                {
                    fadeOut = false;
                }
            }
        }
    }

    public void FadeIn()
    {
        fadeIn = true;
    }
    public void FadeOut()
    {
        fadeOut = true;
    }
    #endregion


    private void Start()
    {
        MenuProp.GetComponent<Rigidbody>().AddForce(0, 0, -0.1f, ForceMode.Acceleration);
    }

    public IEnumerator ChangeScene()
    {
        FadeIn();
        yield return new WaitForSeconds(TimeToFade);
        SceneManager.LoadScene(1);
    }

    public void ButtonStart()
    {
        //make fade effect
        StartCoroutine(ChangeScene());
    }

    public void ButtonCredits()
    {
        //pull over to credit page, and maybe a new background prop
        MainBunch.SetActive(false);
        CreditBunch.SetActive(true);
    }

    public void ButtonBacktoMenu()
    {
        //pull over to main page
        MainBunch.SetActive(true);
        CreditBunch.SetActive(false);
    }

    public void ButtonExit()
    {
        Application.Quit();
    }
}
