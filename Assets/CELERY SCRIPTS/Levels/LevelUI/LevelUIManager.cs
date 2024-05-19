using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelUIManager : MonoBehaviour
{
    [SerializeField] private BlackFade blackFade;
    [SerializeField] private float fadeTime = 1.5f;
    private PlayerHealth _playerHealth;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject pauseMainPanel;
    [SerializeField] private GameObject pauseFirstSelected;
    [SerializeField] private GameObject receptariPanel;
    [SerializeField] private GameObject receptariDetails;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject restartPopup;
    [SerializeField] private GameObject endLevelCanvas;
    [SerializeField] private GameObject endLevelFirstSelected;
    private PauseMenuManager _pauseMenu;
    private EndMenuManager _endMenu;
    private void Awake()
    {
        _pauseMenu = GetComponent<PauseMenuManager>();
        _endMenu = GetComponent<EndMenuManager>();
    }
    void Start()
    {
        if (blackFade != null) StartCoroutine(blackFade.FadeFromBlack(LevelManager.Instance.elapsedTime == 0 ? LevelManager.Instance.startTime : fadeTime));
    }
    private void OnEnable()
    {
        _playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        _playerHealth.OnPlayerDeath += ReturnCheckpoint;
    }
    private void OnDisable()
    {
        _playerHealth.OnPlayerDeath -= ReturnCheckpoint;
    }
    private void Update()
    {
        HandleEscape();
    }
    private void HandleEscape()
    {
        if (!PlayerInputHandler.PauseJustPressed) return;
        ReturnFromReceptari();
    }

    public void ReturnFromReceptari()
    {
        receptariDetails.SetActive(false);
        if (_pauseMenu.isPaused && !pauseMainPanel.activeSelf || restartPopup.activeSelf) //Pause Panel to Pause
        {
            settingsPanel.SetActive(false);
            receptariPanel.SetActive(false);
            pauseMainPanel.SetActive(true);
            restartPopup.SetActive(false);
            EventSystem.current.SetSelectedGameObject(pauseFirstSelected); 
        }
        else if (!_endMenu.isLevelEndActive) //Pause -> Unpause
        {
            _pauseMenu.SetPause(!_pauseMenu.isPaused);
            Cursor.lockState = _pauseMenu.isPaused ? CursorLockMode.None : CursorLockMode.Locked;
            pausePanel.SetActive(_pauseMenu.isPaused);
            if(_pauseMenu.isPaused)
                EventSystem.current.SetSelectedGameObject(pauseFirstSelected);
        }
        else //Receptari to Endlevel
        {
            endLevelCanvas.SetActive(true);
            receptariPanel.SetActive(false);
            EventSystem.current.SetSelectedGameObject(endLevelFirstSelected);
        }
    }
    #region Return Checkpoint
    public void ReturnCheckpoint()
    {
        CrossSceneInformation.CurrentRoom = LevelManager.Instance.rooms.IndexOf(LevelManager.Instance.checkpoints[CrossSceneInformation.CurrentCheckpoint]);
        CrossSceneInformation.CurrentTimerValue = LevelManager.Instance.elapsedTime;
        StartCoroutine(RestartSequence());
    }
    public void ReturnStartLevel()
    {
        CrossSceneInformation.CurrentRoom = 0;
        CrossSceneInformation.CurrentTimerValue = 0;
        StartCoroutine(RestartSequence());
    }
    private IEnumerator RestartSequence()
    {
        PlayerInputHandler.Instance.DisableInputs();
        GameManager.Instance.ChangeTimeScale(0);
        AudioManager.Instance.StopAllLoops(fadeTime);
        yield return StartCoroutine(blackFade.FadeToBlack(fadeTime));
        GameManager.Instance.ChangeTimeScale(1);
        PlayerInputHandler.Instance.EnableInputs();
        //AudioManager.Instance.StopAllLoops();
        GetComponent<ASyncLoader>().LoadLevelBtn(SceneManager.GetActiveScene().name);
    }
    #endregion
    #region Exit Level
    private IEnumerator ExitSequence(string scene)
    {
        AudioManager.Instance.StopAllLoops(fadeTime);
        yield return StartCoroutine(blackFade.FadeToBlack(fadeTime));
        AudioListener.pause = false;
        GameManager.Instance.ChangeTimeScale(1);
        CrossSceneInformation.CurrentCheckpoint = 0;
        CrossSceneInformation.CurrentTimerValue = 0;
        PlayerInputHandler.Instance.EnableInputs();
        GetComponent<ASyncLoader>().LoadLevelBtn(scene);
    }
    public void ExitEndLevel(string scene)
    {
        StartCoroutine(ExitSequence(scene));
    }
    public void RestartLevel()
    {
        StartCoroutine(ExitSequence(SceneManager.GetActiveScene().name));
    }
    #endregion
}
