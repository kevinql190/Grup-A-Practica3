using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainMenuManager : Singleton<MainMenuManager>
{
    [Header("Start Game Text")]
    [SerializeField] private TMP_Text startGameText;
    [SerializeField] private Button selectLevel;
    [Header("Start Game Sequence")]
    [SerializeField] private BlackFade blackFade;
    [SerializeField] private float fadeTime = 1.5f;
    [SerializeField] private string sceneName;
    [Header("Panel Lerp")]
    [SerializeField] private GameObject firstSelectedLevels;
    [SerializeField] private GameObject firstSelectedMenu;
    public GameObject firstSelectedReceptari;
    [SerializeField] private GameObject panelMainMenu;
    [SerializeField] private GameObject panelLevels;
    [SerializeField] private GameObject panelReceptari;
    [SerializeField] private GameObject panelSettings;
    [SerializeField] private float menuFadeTime;
    [SerializeField] private float receptariFadeTime;
    [SerializeField] private float levelsFadeTime;
    [SerializeField] private float settingsFadeTime;
    [SerializeField] private float lerpReceptariTime;
    [SerializeField] private float lerpSettingsTime;
    [SerializeField] private float lerpLevelsTime;
    private UILerper lerper;
    [Header("SFX")]
    [SerializeField] private SoundValues menuSound;

    private void Start()
    {
        lerper = GetComponent<UILerper>();
        PlayerInputHandler.Instance.playerInput.SwitchCurrentActionMap("UI");
        GetComponent<ButtonInstantiator>().GenerateLevelPanel();
        SetStartGameText();
        StartCoroutine(blackFade.FadeFromBlack(1.5f));
    }
    private void OnEnable()
    {
        AudioManager.Instance.PlayMusicLoop("MenuThemeCelery", 1, 1);
    }
    #region Start Game Text
    private void SetStartGameText()
    {
        startGameText.text = GameManager.Instance.levels[0].unlocked ? "CONTINUE GAME" : "START NEW GAME";
        selectLevel.interactable = GameManager.Instance.levels[0].unlocked;
    }
    #endregion
    public void StartGameSequence()
    {
        StartCoroutine(GameSequence());
    }
    private IEnumerator GameSequence()
    {
        AudioManager.Instance.StopMusicLoop(2.5f);
        yield return StartCoroutine(blackFade.FadeToBlack(fadeTime));
        GetComponent<ASyncLoader>().LoadLevelBtn(sceneName);
    }
    #region Panel Lerp
    public void LerpReceptari(bool toReceptari)
    {
        if (toReceptari)
        {
            StartCoroutine(lerper.LerpPanel(panelMainMenu, panelReceptari, menuFadeTime, receptariFadeTime, lerpReceptariTime, firstSelectedReceptari));
        }
        else
        {
            StartCoroutine(lerper.LerpPanel(panelReceptari, panelMainMenu, receptariFadeTime, menuFadeTime, lerpReceptariTime, firstSelectedMenu));
        }
    }
    public void LerpLevels(bool toLevel)
    {
        if (toLevel)
        {
            StartCoroutine(lerper.LerpPanel(panelMainMenu, panelLevels, menuFadeTime, levelsFadeTime, lerpLevelsTime, firstSelectedLevels));
        }
        else
        {
            StartCoroutine(lerper.LerpPanel(panelLevels, panelMainMenu, levelsFadeTime, menuFadeTime, lerpLevelsTime, firstSelectedMenu));
        }
    }
    public void LerpSettings(bool toSettings)
    {
        if (toSettings)
        {
            StartCoroutine(lerper.LerpPanel(panelMainMenu, panelSettings, menuFadeTime, settingsFadeTime, lerpSettingsTime));
        }
        else
        {
            StartCoroutine(lerper.LerpPanel(panelSettings, panelMainMenu, settingsFadeTime, menuFadeTime, lerpSettingsTime, firstSelectedMenu));
        }
    }
    #endregion
    #region Exit
    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // original code to quit Unity player
#endif
    }
    #endregion
    public void UIRandomSound()
    {
        AudioManager.Instance.PlaySFXOnce("so_pagina_" + Random.Range(1, 3), 3f);
    }
    public void UISound(string name)
    {
        AudioManager.Instance.PlaySFXOnce(name, 0.5f);
    }
    public void UISoundRandomPitch()
    {
        AudioManager.Instance.PlaySFXOnceRandomPitch(menuSound);
    }
}
