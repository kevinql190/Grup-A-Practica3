using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class EndMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject endLevelCanvas;
    [SerializeField] private TextMeshProUGUI scoreTimer;
    [SerializeField] private TextMeshProUGUI highScoreTimer;
    [SerializeField] private Transform forksParent;
    [SerializeField] private Sprite forkSprite;
    [SerializeField] private float twoForksTime;
    [SerializeField] private float threeForksTime;
    public bool isLevelEndActive = false;
    private void OnEnable()
    {
        LevelManager.Instance.OnLevelEnd += ShowLevelEnd;
    }
    private void ShowLevelEnd()
    {
        isLevelEndActive = true;
        endLevelCanvas.SetActive(true);
        StartCoroutine(endLevelCanvas.GetComponent<AlphaLerper>().LerpAlpha(1.5f, true));
        Cursor.lockState = CursorLockMode.None;
        Cursor.SetCursor(null, new Vector2(0, 0), CursorMode.Auto);
        PlayerInputHandler.Instance.GetComponent<PlayerInput>().SwitchCurrentActionMap("Gameplay");
        AudioListener.pause = true;
        GameManager.Instance.ChangeTimeScale(0);
        GetScore();
        SetForks();
        GetHighscore();
    }

    private void SetForks()
    {
        float time = LevelManager.Instance.elapsedTime;
        if (time < twoForksTime) forksParent.GetChild(1).GetComponent<Image>().sprite = forkSprite;
        else return;
        if (time < threeForksTime) forksParent.GetChild(2).GetComponent<Image>().sprite = forkSprite;
    }

    private void GetScore()
    {
        float time = LevelManager.Instance.elapsedTime;
        int extractedDecimals = (int)((time - (int)time) * 100);
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        scoreTimer.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, extractedDecimals);
    }
    private void GetHighscore()
    {
        float time = GameManager.Instance.levels[CrossSceneInformation.CurrentLevel].highscore;
        int extractedDecimals = (int)((time - (int)time) * 100);
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        highScoreTimer.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, extractedDecimals);
    }
}
