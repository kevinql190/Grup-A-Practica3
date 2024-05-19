using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;
using TMPro;
using UnityEngine.UI;

public class ReceptariManager : MonoBehaviour
{
    [Header("Receptari Opcions")]
    [SerializeField] private GameObject buttonsReceptariParent;
    [SerializeField] private GameObject buttonsReceptariPrefab;
    [Header("Receptari Detalls")]
    [SerializeField] private TextMeshProUGUI firstName;
    [SerializeField] private LocalizeStringEvent localizedStringEvent;
    [SerializeField] private Image foodProfile;
    [SerializeField] private Image circleBackground;
    [SerializeField] private Transform heartsParent;
    [SerializeField] private GameObject heartPrefab;
    [SerializeField] private TextMeshProUGUI attackType;
    [SerializeField] private TextMeshProUGUI cookedCount;
    [SerializeField] private Transform recipesContent;
    [SerializeField] private GameObject recipeContentPrefab;
    [SerializeField] private List<Sprite> recipeContentSprites;
    private List<ReceptariInfo> currentUnlockedList;
    private int displayedFood;
    [Header("Lerps")]
    [SerializeField] private RectTransform background;
    [SerializeField] private float backgroundAngle;
    [SerializeField] private GameObject detailsPanel;
    [SerializeField] private float lerpTime;
    [SerializeField] private RectTransform detailsTransform;
    [SerializeField] private RectTransform circleTransform;
    [SerializeField] private RectTransform foodSpriteTransform;
    [SerializeField] private Vector2 detailsDirection;
    [SerializeField] private float circleAngle;
    [SerializeField] private float foodSpriteAngle;
    [SerializeField] private AlphaLerper buttonRtnLerper;
    private Vector2 detailsStartPos;
    private bool isLerping;
    private void Awake()
    {
        detailsStartPos = detailsTransform.position;
    }
    private void OnEnable()
    {
        GenerateReceptariPanel();
    }
    public void EnterDetails(ReceptariInfo currentFood)
    {
        if (!isLerping) StartCoroutine(EnteringDetails(currentFood));
    }
    private IEnumerator EnteringDetails(ReceptariInfo currentFood)
    {
        isLerping = true;
        detailsTransform.position = detailsStartPos + detailsDirection;
        circleTransform.rotation = Quaternion.Euler(0, 0, circleAngle);
        foodSpriteTransform.rotation = Quaternion.Euler(0, 0, foodSpriteAngle);
        detailsPanel.SetActive(true);
        yield return RotateBackground(true);
        ChangeDetallsInfo(currentFood);
        StartCoroutine(EasingReceptari(lerpTime, true));
        isLerping = false;
    }
    public void ExitDetails()
    {
        if (!isLerping) StartCoroutine(ExitingDetails());
    }
    private IEnumerator ExitingDetails()
    {
        isLerping = true;
        yield return StartCoroutine(EasingReceptari(lerpTime, false));
        yield return RotateBackground(false);
        detailsPanel.SetActive(false);
        isLerping = false;
    }
    private IEnumerator RotateBackground(bool doEnter)
    {
        StartCoroutine(buttonRtnLerper.LerpAlpha(lerpTime, doEnter));
        float t = 0;
        while (t < lerpTime)
        {
            t += Time.unscaledDeltaTime;
            float curve = AnimationCurve.EaseInOut(0, 0, 1, 1).Evaluate(t / lerpTime);
            float value = doEnter ? 1 - curve : curve;
            background.rotation = Quaternion.Euler(0, 0, Mathf.LerpAngle(0, backgroundAngle, value));
            yield return null;
        }
    }
    public void NextDetails(bool isNext)
    {
        if (isLerping) return;
        displayedFood += isNext ? 1 : -1;
        if (displayedFood < 0) displayedFood = currentUnlockedList.Count - 1;
        else if (displayedFood >= currentUnlockedList.Count) displayedFood = 0;
        StartCoroutine(ChangeDetails(currentUnlockedList[displayedFood]));
    }
    private IEnumerator ChangeDetails(ReceptariInfo currentFood)
    {
        isLerping = true;
        yield return StartCoroutine(EasingReceptari(lerpTime, false));
        ChangeDetallsInfo(currentFood);
        yield return StartCoroutine(EasingReceptari(lerpTime, true));
        isLerping = false;
    }
    private IEnumerator EasingReceptari(float time, bool doEnter)
    {
        float t = 0;
        while (t < time)
        {
            t += Time.unscaledDeltaTime;
            float curve = AnimationCurve.EaseInOut(0, 0, 1, 1).Evaluate(t / time);
            float value = doEnter ? 1 - curve : curve;
            detailsTransform.position = Vector2.Lerp(detailsStartPos, detailsStartPos + detailsDirection, value);
            circleTransform.rotation = Quaternion.Euler(0,0, Mathf.Lerp(0, circleAngle, value));
            foodSpriteTransform.rotation = Quaternion.Euler(0, 0, Mathf.LerpAngle(-10, foodSpriteAngle, value));
            yield return null;
        }
    }
    private void ChangeDetallsInfo(ReceptariInfo currentFood)
    {
        displayedFood = currentUnlockedList.IndexOf(currentFood);
        firstName.text = currentFood.FoodType.firstName;
        localizedStringEvent.StringReference = (currentFood.FoodType.lastName);
        foodProfile.sprite = currentFood.FoodType.receptariSprite;
        cookedCount.text = currentFood.cookCount.ToString();
        attackType.text = currentFood.FoodType.attackType.ToString(); //Canviar a possibilitat de traduir
        circleBackground.color = currentFood.FoodType.receptariColor;
        foreach (Transform child in heartsParent)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < currentFood.FoodType.enemyHealth; i++)
        {
            Instantiate(heartPrefab, heartsParent);
        }
        foreach (Transform child in recipesContent)
        {
            Destroy(child.gameObject);
        }
        int levelid = 0;
        foreach(LevelInfo level in GameManager.Instance.levels)
        {
            levelid++;
            if (level.recipeFood.Contains(currentFood.FoodType.FoodType))
            {
                GameObject newContainer = Instantiate(recipeContentPrefab, recipesContent);
                Image image = newContainer.GetComponent<Image>();
                image.sprite = level.recipeSprite;
                image.SetNativeSize();
                newContainer.GetComponentInChildren<TextMeshProUGUI>().text += levelid;
            }
        }
    }
    public void GenerateReceptariPanel()
    {
        List<ReceptariInfo> newList = UpdateFoodList();
        if (currentUnlockedList == newList) return;
        else currentUnlockedList = newList;

        foreach(Transform child in buttonsReceptariParent.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (ReceptariInfo info in GameManager.Instance.receptariInfo)
        {
            GameObject button = Instantiate(buttonsReceptariPrefab, buttonsReceptariParent.transform);
            button.GetComponent<Button>().interactable = info.found;
            Image component = button.transform.GetChild(0).GetComponent<Image>();
            if (info.FoodType.receptariSprite != null && info.found)
            {
                button.GetComponent<Button>().onClick.AddListener(() => EnterDetails(info));
                component.sprite = info.FoodType.receptariSprite;
            }
            else
            {
                component.color = new Color(1,1,1,0);
            }
        }
    }
    public ReceptariInfo FindFoodByFoodType(FoodType food)
    {
        return Array.Find(GameManager.Instance.receptariInfo, x => x.FoodType.FoodType == food);
    }
    private List<ReceptariInfo> UpdateFoodList()
    {
        List<ReceptariInfo> info = new();
        foreach (ReceptariInfo food in GameManager.Instance.receptariInfo)
        {
            if (food.found)
                info.Add(food);
        }
        return info;
    }
}
