using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class LanguageSettings : MonoBehaviour
{
    private int CurrentPage => GetComponent<SwipeController>().currentPage;
    private void Start()
    {
        switch (LocalizationSettings.SelectedLocale.Identifier.Code)
        {
            case "ca":
                break;
            case "es":
                if (!Screen.fullScreen) GetComponent<SwipeController>().GoToPage(1);
                break;
            case "en":
                if (!Screen.fullScreen) GetComponent<SwipeController>().GoToPage(2);
                break;
        }
    }
    public void SetLocaleLanguage()
    {
        StartCoroutine(SetLocale(GetIdentifier(CurrentPage)));
    }
    private string GetIdentifier(int currentPage)
    {
        switch (currentPage)
        {
            case 0:
                return "ca";
            case 1:
                return "es";
            case 2:
                return "en";
            default:
                return null;
        }
    }
    private IEnumerator SetLocale(string languageCode)
    {
        // Ensure the localization system is ready
        yield return LocalizationSettings.InitializationOperation;

        Locale localeToUse = null;
        foreach (var locale in LocalizationSettings.AvailableLocales.Locales)
        {
            if (locale.Identifier.Code == languageCode)
            {
                localeToUse = locale;
                break;
            }
        }

        if (localeToUse != null)
        {
            LocalizationSettings.SelectedLocale = localeToUse;
        }
        else
        {
            Debug.LogError("Requested locale not found: " + languageCode);
        }
    }
}
