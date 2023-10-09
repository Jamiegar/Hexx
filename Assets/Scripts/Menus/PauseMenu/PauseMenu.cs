using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PauseMenu : MonoBehaviour, IInitalizable
{
    public UnityEvent<bool> ApplicationToogledPause; 
    
    [SerializeField] private GameObject _pauseMenuUI;
    
    private static bool _isPaused;
    public bool ApplicationPaused
    {
        set{_isPaused = value; TogglePause(); } //Sets pause menu to toggle if application is set

        get { return _isPaused; }
    }

    public void Init()
    {
        _pauseMenuUI.SetActive(false); //Set pause menu to be inactive at start of app
    }

    public void TogglePause() 
    {
        if (_isPaused == true)
            ResumeApplication();
        else
            PauseApplication();
    }

    public void ResumeApplication()
    {
        _pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        _isPaused = false;
        ApplicationToogledPause?.Invoke(_isPaused);
    }

    private void PauseApplication()
    {
        _pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        _isPaused = true;
        ApplicationToogledPause?.Invoke(_isPaused);
    }

    public void ExitApplication()
    {
        Application.Quit();
    }

}
