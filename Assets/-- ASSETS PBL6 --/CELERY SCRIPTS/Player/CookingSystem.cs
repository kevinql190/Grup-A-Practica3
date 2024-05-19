using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CookingSystem : MonoBehaviour
{
    [Header("Pan Burnt")]
    [SerializeField] private float stunTime;
    [SerializeField] private float stunSpeedDecrease;
    [SerializeField] private int lifeLoss;
    [Header("Skills")]
    public bool skillCasted = false;

    public event Action<float> OnCookingProgressChanged;
    public event Action<float> OnSparingProgressChanged;
    public float CurrentCookingProgress
    {
        get { return _cookingprogress; }
        set { _cookingprogress = value; OnCookingProgressChanged?.Invoke(value); }
    }
    public float CurrentSparingProgress
    {
        get { return _sparingprogress; }
        set { _sparingprogress = value; OnSparingProgressChanged?.Invoke(value); }
    }
    private float _cookingprogress;
    private float _sparingprogress;
    private SkillAbilities _skillAbilities;
    private void Awake()
    {
        _skillAbilities = GetComponent<SkillAbilities>();
    }
    public IEnumerator CookingProcess(float cookingTime, float spareTime)
    {
        AudioManager.Instance.PlaySFXLoop("cooking_loop", 0.3f, 1f);
        _skillAbilities.SetAbility(true);
        //-----------------------COOKING-----------------------//
        float timer = 0f;
        while (timer < cookingTime) //Cooking
        {
            timer += Time.deltaTime;
            CurrentCookingProgress = timer / cookingTime;
            yield return null;
        }
        CurrentCookingProgress = 0f;
        //-----------------------COOKED-----------------------//
        StartCoroutine(_skillAbilities.HandleSkill());
        AudioManager.Instance.PlaySFXOnce("cooked");
        AudioManager.Instance.PlaySFXLoop("timer_loop", 0.5f, 1f);
        timer = 0f;
        while (timer < spareTime && !skillCasted) //Spare Time
        {
            timer += Time.deltaTime;
            CurrentSparingProgress = timer / spareTime;
            yield return null;
        }
        CurrentSparingProgress = 0f;
        //-----------------------END-------------------------//
        if (!skillCasted) PanBurnt(); //Burnt
        _skillAbilities.SetAbility(false);
        GetComponent<PanController>().ChangeFoodType(FoodType.Default);
        AudioManager.Instance.StopLoop("cooking_loop", 1f);
        AudioManager.Instance.StopLoop("timer_loop", 1f);
    }
    private void PanBurnt()
    {
        Debug.Log("Pan Burnt!");
        AudioManager.Instance.PlaySFXOnce("burned", 2.5f);
        StartCoroutine(GetComponent<PlayerMovement>().StunPlayer(stunTime, stunSpeedDecrease));
        GetComponent<IDamageable>().TakeDamage(-lifeLoss);
    }
}
