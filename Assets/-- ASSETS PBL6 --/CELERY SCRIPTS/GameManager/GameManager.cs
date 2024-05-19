using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ReceptariInfo
{
    public FoodScriptableObject FoodType;
    public bool found;
    public int cookCount;
}
[Serializable]
public class LevelInfo
{
    public bool unlocked;
    public float highscore;
    public string levelName;
    public string levelScene;
    public List<FoodType> recipeFood;
    public Sprite recipeSprite;
}
public class GameManager : SingletonPersistent<GameManager>
{
    public ReceptariInfo[] receptariInfo;
    public LevelInfo[] levels;

    public void UnlockFood(FoodType foodToUnlock)
    {
        foreach (ReceptariInfo food in receptariInfo)
        {
            if (food.FoodType.FoodType == foodToUnlock)
            {
                food.found = true;
                break;
            }
        }
    }
    public void ChangeTimeScale(float value, float lerpTime = 0)
    {
        if (lerpTime == 0) Time.timeScale = value;
        else StartCoroutine(LerpTimeScale(value, lerpTime));
    }
    private IEnumerator LerpTimeScale(float endScale, float lerpTime)
    {
        float t = 0;
        float startScale = Time.timeScale;
        while (t < lerpTime)
        {
            t += Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Lerp(startScale, endScale, t / lerpTime);
            yield return null;
        }
    }
}
