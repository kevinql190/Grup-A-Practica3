using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ASyncLoader : MonoBehaviour
{
    [Header("Menu Screens")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject mainMenu;

    [Header("Slider")]
    [SerializeField] private AlphaLerper alphaLerper;
    [SerializeField] private Slider loadingSlider;
    [SerializeField] private Image loadingIconDisplay;
    [SerializeField] private Sprite[] loadingSprites;
    [SerializeField] private float spriteDisplayTime = 1.0f;
    private int spriteIndex = 0;

    public void LoadLevelBtn(string levelToLoad)
    {
        mainMenu.SetActive(false);
        loadingScreen.SetActive(true);
        StartCoroutine(LoadLevelASync(levelToLoad));
    }

    IEnumerator LoadLevelASync (string leveltoLoad)
    {
        if (alphaLerper != null) StartCoroutine(alphaLerper.LerpAlpha(5f, true));
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(leveltoLoad);
        while (!loadOperation.isDone)
        {
            float progressValue = Mathf.Clamp01(loadOperation.progress / 0.9f);
            loadingSlider.value = progressValue;
            yield return null;
        }
    }
    IEnumerator LoadLevelAsync(string levelToLoad)
    {
        if (alphaLerper != null) StartCoroutine(alphaLerper.LerpAlpha(5f, true));
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelToLoad);
        loadOperation.allowSceneActivation = false; // Prevent the scene from switching automatically

        float lastSpriteUpdateTime = Time.time;
        while (!loadOperation.isDone)
        {
            // Loading bar
            float progressValue = Mathf.Clamp01(loadOperation.progress / 0.9f);
            loadingSlider.value = progressValue;

            // Loading Sprites
            if (Time.time - lastSpriteUpdateTime > spriteDisplayTime)
            {
                loadingIconDisplay.sprite = loadingSprites[spriteIndex];
                spriteIndex = (spriteIndex + 1) % loadingSprites.Length;
                lastSpriteUpdateTime = Time.time;
            }

            // Check if the load has actually finished and can be activated
            if (loadOperation.progress >= 0.9f)
            {
                loadOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }

}
