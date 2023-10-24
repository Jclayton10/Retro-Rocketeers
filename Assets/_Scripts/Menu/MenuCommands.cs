using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;

public class MenuCommands : MonoBehaviour
{
    public GameMaster GM;
    public Camera Camera;

    [Header("Button Bunches")]
    public GameObject MenuProp;
    public GameObject MainBunch;
    public List<Button> MainButtons;
    public GameObject CreditBunch;
    public List<Button> CreditButtons;
    public GameObject SettingsBunch;
    public List<Button> SettingsButtons;

    [Header("Camera Points")]
    public float panTime = 0f;
    public float Countdown = 0f;
    public float panSpeed = 0f;
    public GameObject Origin;
    public GameObject MainPoint;
    public GameObject CreditsPoint;
    public GameObject MainPointDown;
    public GameObject SettingsPoint;
    public GameObject AstroPointLeft;
    public GameObject AstroPointRight;
    public GameObject AstroPointUp;

    [Header("Camera Logics")]
    public bool MovetoMain = false;
    public bool MovetoCredits = false;
    public bool MovetoSettings = false;

    [Header("Settings Options")]
    public Slider MasterSlider;
    public Slider MusicSlider;
    public Slider SFXSlider;

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
        else if (fadeOut)
        {
            if (AudioEmmisions.GetComponent<AudioSource>().volume < GM.AudioMaster * GM.AudioMusic)
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
            Camera.transform.SetPositionAndRotation(Vector3.LerpUnclamped(Camera.transform.position, AstroPointRight.transform.position, panSpeed * Time.deltaTime), Quaternion.LerpUnclamped(Camera.transform.rotation, AstroPointRight.transform.rotation, panSpeed * Time.deltaTime));
            CreditBunch.transform.position = Vector3.LerpUnclamped(CreditBunch.transform.position, CreditsPoint.transform.position, panSpeed * Time.deltaTime);
            SettingsBunch.transform.SetPositionAndRotation(Vector3.LerpUnclamped(SettingsBunch.transform.position, SettingsPoint.transform.position, panSpeed * Time.deltaTime), Quaternion.LerpUnclamped(SettingsBunch.transform.rotation, SettingsPoint.transform.rotation, panSpeed * Time.deltaTime));
            MainBunch.transform.SetPositionAndRotation(Vector3.LerpUnclamped(MainBunch.transform.position, Origin.transform.position, panSpeed * Time.deltaTime), Quaternion.LerpUnclamped(MainBunch.transform.rotation, Origin.transform.rotation, panSpeed * Time.deltaTime));
            if (Countdown <= 0)
            {
                MovetoMain = false;
                foreach (Button b in MainButtons)
                {
                    b.interactable = true;
                }
            }
        }
        else if (MovetoCredits)
        {
            Camera.transform.position = Vector3.LerpUnclamped(Camera.transform.position, AstroPointLeft.transform.position, panSpeed * Time.deltaTime);
            CreditBunch.transform.position = Vector3.LerpUnclamped(CreditBunch.transform.position, Origin.transform.position, panSpeed * Time.deltaTime);
            MainBunch.transform.position = Vector3.LerpUnclamped(MainBunch.transform.position, MainPoint.transform.position, panSpeed * Time.deltaTime);
            if (Countdown <= 0)
            {
                MovetoCredits = false;
                foreach (Button b in CreditButtons)
                {
                    b.interactable = true;
                }
            }
        }
        else if (MovetoSettings)
        {
            Camera.transform.SetPositionAndRotation(Vector3.LerpUnclamped(Camera.transform.position, AstroPointUp.transform.position, panSpeed * Time.deltaTime), Quaternion.LerpUnclamped(Camera.transform.rotation, AstroPointUp.transform.rotation, panSpeed * Time.deltaTime));
            SettingsBunch.transform.position = Vector3.LerpUnclamped(SettingsBunch.transform.position, Origin.transform.position, panSpeed * Time.deltaTime);
            MainBunch.transform.position = Vector3.LerpUnclamped(MainBunch.transform.position, MainPointDown.transform.position, panSpeed * Time.deltaTime);
            SettingsBunch.transform.rotation = Quaternion.LerpUnclamped(SettingsBunch.transform.rotation, Origin.transform.rotation, panSpeed * Time.deltaTime);
            MainBunch.transform.rotation = Quaternion.LerpUnclamped(MainBunch.transform.rotation, MainPointDown.transform.rotation, panSpeed * Time.deltaTime);
            if (Countdown <= 0)
            {
                MovetoSettings = false;
                foreach (Button b in SettingsButtons)
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

    public void ButtonSettings()
    {
        //pull over to credit page, and maybe a new background prop

        StartCoroutine(CameraPan("Settings"));
    }

    public void ButtonApply()
    {
        //use new menu features to adjust values
        GM.AudioMaster = MasterSlider.value / 100;
        GM.AudioMusic = MusicSlider.value / 100;
        GM.AudioSFX = SFXSlider.value / 100;
        AudioEmmisions.GetComponent<AudioSource>().volume = GM.AudioMaster * GM.AudioMusic;

        /*GM.MouseSensitiviy = 1.0f;

        GM.rightKey = KeyCode.D;
        GM.leftKey = KeyCode.A;
        GM.forwardKey = KeyCode.W;
        GM.backKey = KeyCode.S;
        GM.jumpKey = KeyCode.Space;

        GM.buildKey = KeyCode.B;
        GM.invKey = KeyCode.Tab;*/
    }

    public void ButtonReset()
    {
        //update visuals as well
        GM.AudioMaster = MasterSlider.value = 0.8f;
        GM.AudioMusic = MusicSlider.value = 0.8f;
        GM.AudioSFX = SFXSlider.value = 0.8f;
        MasterSlider.value = 80;
        MusicSlider.value = 80;
        SFXSlider.value = 80;
        AudioEmmisions.GetComponent<AudioSource>().volume = GM.AudioMaster * GM.AudioMusic;


        GM.MouseSensitiviy = 1.0f;

        GM.rightKey = KeyCode.D;
        GM.leftKey = KeyCode.A;
        GM.forwardKey = KeyCode.W;
        GM.backKey = KeyCode.S;
        GM.jumpKey = KeyCode.Space;

        GM.buildKey = KeyCode.B;
        GM.invKey = KeyCode.Tab;

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
                foreach (Button b in SettingsButtons)
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
            case "Settings":
                foreach (Button b in MainButtons)
                {
                    b.interactable = false;
                }
                MovetoSettings = true;
                SettingsBunch.SetActive(true);
                break;
        }
        yield return new WaitForSeconds(panTime);
        switch (Activate)
        {
            case "Main":
                CreditBunch.SetActive(false);
                SettingsBunch.SetActive(false);
                break;
            case "Credits":
                MainBunch.SetActive(false);
                break;
            case "Settings":
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
