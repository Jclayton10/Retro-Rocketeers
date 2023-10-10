using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuCommands : MonoBehaviour
{
    public Camera Camera;

    [Header("Button Bunches")]
    public GameObject MenuProp;
    public GameObject MainBunch;
    public List<Button> MainButtons;
    public GameObject CreditBunch;
    public List<Button> CreditButtons;
    //public GameObject SettingsBunch;

    [Header("Camera Points")]
    public float panTime = 0f;
    public float Countdown = 0f;
    public float panSpeed = 0f;
    public GameObject Origin;
    public GameObject MainPoint;
    public GameObject CreditsPoint;
    public GameObject AstroPointLeft;
    public GameObject AstroPointRight;

    [Header("Camera Logics")]
    public bool MovetoMain = false;
    public bool MovetoCredits = false;

    [Header(" ")]
    public GameObject AudioEmmisions;

    #region Fade
    public CanvasGroup CanvasGroup;
    public bool fadeOut = false;
    public bool fadeIn = false;

    public float TimeToFade;

    void Update()
    {
        #region fade
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

        #endregion

        #region Panning
        Countdown -= Time.deltaTime;
        if (MovetoMain)
        {
            Camera.transform.position = Vector3.Lerp(Camera.transform.position, AstroPointRight.transform.position, panSpeed * Time.deltaTime);
            CreditBunch.transform.position = Vector3.Lerp(CreditBunch.transform.position, CreditsPoint.transform.position, panSpeed * Time.deltaTime);
            MainBunch.transform.position = Vector3.Lerp(MainBunch.transform.position, Origin.transform.position, panSpeed * Time.deltaTime);
            if (Countdown <= 0)
            {
                MovetoMain = false;
                foreach(Button b in MainButtons)
                {
                    b.interactable = true;
                }
            }
        }
        else if (MovetoCredits)
        {
            Camera.transform.position = Vector3.Lerp(Camera.transform.position, AstroPointLeft.transform.position, panSpeed * Time.deltaTime);
            CreditBunch.transform.position = Vector3.Lerp(CreditBunch.transform.position, Origin.transform.position, panSpeed * Time.deltaTime);
            MainBunch.transform.position = Vector3.Lerp(MainBunch.transform.position, MainPoint.transform.position, panSpeed * Time.deltaTime);
            if (Countdown <= 0)
            {
                MovetoCredits = false;
                foreach (Button b in CreditButtons)
                {
                    b.interactable = true;
                }
            }
        }


        #endregion
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
        StartCoroutine(ChangeScene());
    }

    public void ButtonCredits()
    {
        //pull over to credit page, and maybe a new background prop

        StartCoroutine(CameraPan("Credits"));
    }

    public void ButtonBacktoMenu()
    {
        StartCoroutine(CameraPan("Main"));
    }


    public IEnumerator CameraPan(string Activate)
    {
        Countdown = panTime;
        switch (Activate)
        {
            case "Main":
                foreach (Button b in CreditButtons)
                {
                    b.interactable = false;
                }
                MovetoMain = true;
                MainBunch.SetActive(true);
                break;
            case "Credits":
                foreach (Button b in MainButtons)
                {
                    b.interactable = false;
                }
                MovetoCredits = true;
                CreditBunch.SetActive(true);
                break;
        }
        yield return new WaitForSeconds(panTime);
        switch (Activate)
        {
            case "Main":
                CreditBunch.SetActive(false);
                break;
            case "Credits":
                MainBunch.SetActive(false);
                break;
        }
    }

    public void ButtonExit()
    {
        StartCoroutine(QuitGame());
    }
    public IEnumerator QuitGame()
    {
        FadeIn();
        yield return new WaitForSeconds(TimeToFade);
        Application.Quit();
    }
}
