using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonInstantiator : MonoBehaviour
{
    [Header("Levels Instantiate")]
    [SerializeField] private GameObject buttonsLevelsParent;
    [SerializeField] private GameObject buttonsLevelsPrefab;
    #region Levels Instantiate

    public void GenerateLevelPanel()
    {
        foreach (LevelInfo info in GameManager.Instance.levels)
        {
            GameObject button = Instantiate(buttonsLevelsPrefab, buttonsLevelsParent.transform);
            button.GetComponent<TMP_Text>().text = info.levelName;
            button.GetComponent<Button>().interactable = info.unlocked;
            button.GetComponent<Button>().onClick.AddListener(() => GameManager.Instance.GetComponent<ASyncLoader>().LoadLevelBtn(info.levelScene));
        }
    }
    #endregion
}
