using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanController : MonoBehaviour
{
    [Header("FoodType Properties")]
    [SerializeField] private GameObject fryingPanObject;
    [SerializeField] private GameObject foodSpriteCanvas;
    [SerializeField] private GameObject foodSpriteImage;
    [SerializeField] private float spriteDuration;
    [SerializeField] private AnimationCurve spriteCurve;

    public FoodType currentFoodType = FoodType.Default;
    private FoodScriptableObject CurrentFood => Array.Find(GameManager.Instance.receptariInfo, x => x.FoodType.FoodType == currentFoodType).FoodType;
    private GameObject CurrentPrefabAssigned => CurrentFood.prefabAssigned;
    private float CurrentSpareCookingTime => CurrentFood.spareCookingTime;
    private float CurrentCookingTime => CurrentFood.cookingTime;
    private Sprite CurrentSkillHUDSprite => CurrentFood.skillHudSprite;
    [HideInInspector] public Sprite CurrentSkillSprite => CurrentFood.skillSprite;
    public event Action<FoodType> OnFoodSpriteChanged;

    private void Start()
    {
        UpdatePanPrefab();
        foodSpriteCanvas.SetActive(false);
    }

    // Mètode per canviar FoodType
    public void ChangeFoodType(FoodType newState)
    {
        currentFoodType = newState;
        if (newState != FoodType.Default)
        {
            //AudioManager.Instance.PlaySFXOnce("agafar_ingredient", 0.5f);
            StartCoroutine(GetComponent<CookingSystem>().CookingProcess(CurrentCookingTime, CurrentSpareCookingTime));
            OnFoodSpriteChanged?.Invoke(newState);
            StartCoroutine(ShowHeadFoodSprite());
        }
        UpdatePanPrefab();
    }
    private IEnumerator ShowHeadFoodSprite()
    {
        float t = 0;
        foodSpriteCanvas.GetComponent<Image>().sprite = CurrentSkillHUDSprite;
        foodSpriteImage.SetActive(true);
        Vector3 startPos = foodSpriteCanvas.transform.localPosition;
        while (t < spriteDuration)
        {
            t += Time.deltaTime;
            float value = spriteCurve.Evaluate(t / spriteDuration);
            foodSpriteCanvas.transform.localPosition = startPos + Vector3.up * value;
            foodSpriteCanvas.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
            yield return null;
        }
        foodSpriteImage.SetActive(false);
    }

    // Update the prefab based on the current pan state
    private void UpdatePanPrefab()
    {
        Transform childToDestroy = transform.Find("PanContent");
        if (childToDestroy != null)
        {
            Destroy(childToDestroy.gameObject);
        }
        GameObject newObject = Instantiate(CurrentPrefabAssigned, transform);
        newObject.name = "PanContent";
    }
}