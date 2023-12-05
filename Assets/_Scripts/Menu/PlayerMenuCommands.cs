using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerMenuCommands : MonoBehaviour
{
    public static bool GameIsPaused;

    [SerializeField] private GameObject PauseOverlay;
    [SerializeField] private GameObject MainGroup;
    [SerializeField] private GameObject LeaveGroup;
    [SerializeField] private GameObject AudioGroup;
    [SerializeField] private GameObject ControlGroup;
    [SerializeField] private GameObject InvPrompt;
    [SerializeField] private GameObject BuildPrompt;
    [SerializeField] private Sprite InvDefault;
    [SerializeField] private Sprite BuildDefault;

    [SerializeField] private GameObject basiccam;
    [SerializeField] private GameObject attackcam;
    [SerializeField] private GameObject buildcam;

    public Slider MasterSlider;
    public Slider MusicSlider;
    public Slider SFXSlider;
    public Slider MouseSlider;
    [SerializeField] private InputActionAsset _inputAction;

    void Update()
    {
        #region PauseScreen
        if (GameMaster.Instance.PauseJustPressed)
        {
            if (GameIsPaused)
            {
                Resume();

            }
            else
            {

                Pause();
            }
        }

        #endregion


        #region fade
        if (fadeIn)
        {
            if (CanvasGroup.alpha < 1)
            {
                CanvasGroup.alpha += TimeToFade * Time.deltaTime;
                if (CanvasGroup.alpha >= 1)
                {
                    fadeIn = false;
                }
            }
        }
        if (fadeOut)
        {
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
    }

    #region Pause/Resume
    public void Resume()
    {
        LeaveGroup.SetActive(false);
        AudioGroup.SetActive(false);
        ControlGroup.SetActive(false);
        PauseOverlay.SetActive(false);
        MainGroup.SetActive(true);
        Time.timeScale = 1f;
        GameIsPaused = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Pause()
    {
        PauseOverlay.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }
    #endregion

    #region Fade
    public CanvasGroup CanvasGroup;
    public bool fadeOut = false;
    public bool fadeIn = false;

    public float TimeToFade;

    public void FadeIn()
    {
        fadeIn = true;
    }
    public void FadeOut()
    {
        fadeOut = true;
    }
    #endregion

    #region SwitchingScreens

    public void ToAudio()
    {
        MainGroup.gameObject.SetActive(false);
        AudioGroup.gameObject.SetActive(true);
    }
    public void ToControls()
    {
        MainGroup.gameObject.SetActive(false);
        ControlGroup.gameObject.SetActive(true);
    }
    public void ToLeave()
    {
        MainGroup.gameObject.SetActive(false);
        LeaveGroup.gameObject.SetActive(true);
    }
    public void AToMain()
    {
        MainGroup.gameObject.SetActive(true);
        AudioGroup.gameObject.SetActive(false);

    }
    public void CToMain()
    {
        MainGroup.gameObject.SetActive(true);
        ControlGroup.gameObject.SetActive(false);
    }




    #endregion



    public void SaveControls()
    {
        InvPrompt.GetComponent<Image>().sprite = InvDefault;
        BuildPrompt.GetComponent<Image>().sprite = BuildDefault;

        GameMaster.Instance.MouseSensitiviy = MouseSlider.value;

        basiccam.GetComponent<CinemachineFreeLook>().m_XAxis.m_MaxSpeed = 300 * GameMaster.Instance.MouseSensitiviy;
        basiccam.GetComponent<CinemachineFreeLook>().m_YAxis.m_MaxSpeed = 2 * GameMaster.Instance.MouseSensitiviy;
        attackcam.GetComponent<CinemachineFreeLook>().m_XAxis.m_MaxSpeed = 300 * GameMaster.Instance.MouseSensitiviy;
        attackcam.GetComponent<CinemachineFreeLook>().m_YAxis.m_MaxSpeed = 2 * GameMaster.Instance.MouseSensitiviy;
        buildcam.GetComponent<CinemachineFreeLook>().m_XAxis.m_MaxSpeed = 300 * GameMaster.Instance.MouseSensitiviy;
        buildcam.GetComponent<CinemachineFreeLook>().m_YAxis.m_MaxSpeed = 2 * GameMaster.Instance.MouseSensitiviy;
    }

    public void SaveAudio()
    {
        GameMaster.Instance.AudioMaster = MasterSlider.value / 100;
        GameMaster.Instance.AudioMusic = MusicSlider.value / 100;
        GameMaster.Instance.AudioSFX = SFXSlider.value / 100;
    }

    public void ResetAudio()
    {
        InvPrompt.GetComponent<Image>().sprite = InvDefault;
        BuildPrompt.GetComponent<Image>().sprite = BuildDefault;

        GameMaster.Instance.AudioMaster = 0.8f;
        GameMaster.Instance.AudioMusic = 0.8f;
        GameMaster.Instance.AudioSFX = 0.8f;
        MasterSlider.value = 80;
        MusicSlider.value = 80;
        SFXSlider.value = 80;
    }

    public void ResetControls()
    {
        GameMaster.Instance.MouseSensitiviy = MouseSlider.value = 1.0f;
        foreach (InputActionMap map in _inputAction.actionMaps)
        {
            map.RemoveAllBindingOverrides();
        }
    }



    #region ToMenu
    public void ButtonToMenu()
    {
        Time.timeScale = 1f;
        StartCoroutine(ChangeScene());
    }

    public IEnumerator ChangeScene()
    {
        FadeIn();
        yield return new WaitForSeconds(TimeToFade);
        SceneManager.LoadScene(0);
    }
    #endregion
}
