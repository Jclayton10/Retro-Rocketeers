using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMenuCommands : MonoBehaviour
{
    public KeyCode PauseKey = KeyCode.Escape;
    public KeyCode BackupKey = KeyCode.Delete;

    public static bool GameIsPaused;

    [SerializeField] private GameObject PauseOverlay;

    void Update()
    {
        #region PauseScreen
        if (Input.GetKeyDown(PauseKey) || Input.GetKeyDown(BackupKey))
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
        Debug.Log("Off");
        PauseOverlay.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Pause()
    {
        Debug.Log("ON!");
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
