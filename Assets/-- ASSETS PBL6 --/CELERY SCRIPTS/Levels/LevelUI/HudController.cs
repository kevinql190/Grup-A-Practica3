using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HudController : MonoBehaviour
{
    [Header("Canvas")]
    [SerializeField] private GameObject canvas;
    private GameObject _player;
    [Header("Life System")]
    [SerializeField] private Transform heartsContainer;
    [SerializeField] private GameObject heartPrefab;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;
    private PlayerHealth _playerHealth;
    [Header("Dash Slider")]
    [SerializeField] private float dashSliderChangeVelocity;
    [SerializeField] List<Sprite> dashHandleSprites;
    [SerializeField] Image dashFillImage;
    [SerializeField] private float dashWarningSpeed;
    [SerializeField] private float dashWarningValue;
    private Slider dashSlider;
    private bool isWarningActive = false;
    [Header("Level Name")]
    [SerializeField] GameObject levelName;
    [SerializeField] private Vector2 startMoveLevelName;
    [SerializeField] private float levelStartDuration;
    private Vector2 endPosLevelName;
    [Header("Timer")]
    [SerializeField] GameObject timer;
    [SerializeField] private float timerShowTime;
    [SerializeField] private Vector2 startMoveTimer;
    [SerializeField] TextMeshProUGUI timerMinSecs;
    [SerializeField] TextMeshProUGUI timerMilisec;
    private Vector2 endPosTimer;
    private float ElapsedTime => LevelManager.Instance.elapsedTime;
    [Header("Cooking System")]
    [SerializeField] private Image cookingSpriteImage;
    [SerializeField] private AnimationCurve cookingAlphaChange;
    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        dashSlider = canvas.transform.Find("DashSlider").transform.Find("Slider").GetComponent<Slider>();
        _playerHealth = _player.GetComponent<PlayerHealth>();

        endPosLevelName = levelName.GetComponent<RectTransform>().anchoredPosition;
        endPosTimer = timer.GetComponent<RectTransform>().anchoredPosition;
        levelName.SetActive(false);
        timer.SetActive(false);
    }
    private void Start()
    {
        SetCanvasHearts();
    }

    private void OnEnable()
    {
        LevelManager.Instance.OnStartLevel += ctx => StartLevel(ctx);
        _playerHealth.OnHealthChanged += UpdateHearts;
        _player.GetComponent<CookingSystem>().OnCookingProgressChanged += UpdateCookSlider;
        _player.GetComponent<CookingSystem>().OnSparingProgressChanged += UpdateSpareSlider;
        _player.GetComponent<PlayerMovement>().OnDashChargeChanged += DashSliderChange;
        _player.GetComponent<PanController>().OnFoodSpriteChanged += ChangeFoodSprite;
    }
    private void Update()
    {
        UpdateTimer();
    }
    #region Life HUD
    private void SetCanvasHearts()
    {
        for (int i = 0; i < _playerHealth.maxHealth; i++)
        {
            Instantiate(heartPrefab, heartsContainer);
        }
    }

    private void UpdateHearts(int health)
    {
        foreach (Transform child in canvas.transform.Find("Hearts").transform)
        {
            if (child.GetSiblingIndex() < health)
            {
                child.GetComponent<Image>().sprite = fullHeart;
            }
            else
            {
                child.GetComponent<Image>().sprite = emptyHeart;
            }
        }
    }
    #endregion
    #region Cooking Skill Slider HUD
    private void UpdateCookSlider(float amount)
    {
        Color handleColor = amount == 0f || amount == 360f ? new Color32(78, 57, 57, 255) : new Color32(255, 213, 65, 255);
        canvas.transform.Find("SkillSlider").transform.Find("PanFill").GetComponent<Image>().fillAmount = amount;
        canvas.transform.Find("SkillSlider").transform.Find("PanHandle").transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, -amount * 360));
        canvas.transform.Find("SkillSlider").transform.Find("PanHandle").GetComponent<Image>().color = handleColor;
        cookingAlphaChange.Evaluate(amount);
        cookingSpriteImage.color = new Color(1, 1, 1, amount);
    }
    private void UpdateSpareSlider(float amount)
    {
        cookingSpriteImage.color = new Color(1, 1, 1, amount == 0 ? 0 : 1);
        canvas.transform.Find("SkillSlider").transform.Find("Slider").transform.Find("SliderFill").GetComponent<Image>().fillAmount = amount;
    }
    private void ChangeFoodSprite(FoodType foodType)
    {
        if (foodType == FoodType.Default) cookingSpriteImage.sprite = null;
        else cookingSpriteImage.sprite = Array.Find(GameManager.Instance.receptariInfo, x => x.FoodType.FoodType == foodType).FoodType.skillHudSprite;
    }
    #endregion
    #region Dash Slider HUD
    private void DashSliderChange(float value)
    {
        Slider slider = canvas.transform.Find("DashSlider").transform.Find("Slider").GetComponent<Slider>();
        var unlerpedValue = Mathf.InverseLerp(0, 3, value);
        if (slider.value < unlerpedValue)
        {
            slider.value = unlerpedValue;
        }
        else
        {
            slider.value -= dashSliderChangeVelocity * Time.deltaTime;
        }
        dashSlider.handleRect.GetComponent<Image>().sprite = dashHandleSprites[(int)value];
        
        if (unlerpedValue < 0.3f && !isWarningActive) StartCoroutine(DashWarning());
        else if (unlerpedValue > 0.3f) isWarningActive = false;
    }
    private IEnumerator DashWarning()
    {
        isWarningActive = true;
        float startTime = Time.time;
        dashFillImage.color = new Color(1, 1, 1);
        while (isWarningActive)
        {
            float t = (Time.time - startTime) / dashWarningSpeed;
            float value = Mathf.Lerp(1f, dashWarningValue, Mathf.Abs(Mathf.Tan(t * Mathf.PI)));
            dashFillImage.color = new Color(value, value, value);
            yield return null;

            if (t >= 1.0f)
            {
                startTime = Time.time;
            }
        }
        dashFillImage.color = new Color(1, 1, 1);
    }
    #endregion
    #region Timer
    private void UpdateTimer()
    {
        int extractedDecimals = (int)((ElapsedTime - (int)ElapsedTime) * 100);
        int minutes = Mathf.FloorToInt(ElapsedTime / 60);
        int seconds = Mathf.FloorToInt(ElapsedTime % 60);
        timerMinSecs.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        timerMilisec.text = extractedDecimals.ToString("00");

    }
    #endregion
    #region Start Level Name
    private void StartLevel(bool isFirstTIme)
    {
        if (isFirstTIme) StartCoroutine(ShowLevelName());
        else StartCoroutine(ShowTimer());
    }
    private IEnumerator ShowLevelName()
    {
        yield return new WaitForSecondsRealtime(levelStartDuration);
        float t = 0;
        levelName.SetActive(true);
        while (t < LevelManager.Instance.startTime)
        {
            t += Time.unscaledDeltaTime;
            float value = (0.5f - Mathf.Abs(0.5f - t / LevelManager.Instance.startTime)) * 2.5f;
            levelName.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(endPosLevelName + startMoveLevelName, endPosLevelName, value);
            yield return null;
        }
        levelName.SetActive(false);
        StartCoroutine(ShowTimer());
    }
    private IEnumerator ShowTimer()
    {
        float t = 0;
        timer.SetActive(true);
        while (t< timerShowTime)
        {
            t += Time.deltaTime;
            float value = t / timerShowTime;
            timer.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(endPosTimer + startMoveTimer, endPosTimer, value);
            yield return null;
        }
        LevelManager.Instance.OnStartLevel -= ctx => StartLevel(ctx);
    }
    #endregion
}
