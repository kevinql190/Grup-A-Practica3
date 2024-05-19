using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

#region data
[Serializable]
public class PlayerData
{
    public List<float> unlockedLevelsTime;
    public List<int> cookedFoodCount;

    public int CurrentLoadedLevel;
    public int CurrentLoadedLevelRoom;
    public int CurrentLoadedLevelCheckpoint;
    public float CurrentTimeValue;
}
#endregion
public class SaveLoadJSON : MonoBehaviour
{
    [SerializeField] private string fileName;
    private string dataFilePath;
    public PlayerData data = new PlayerData();
    private void Awake()
    {
        dataFilePath = Application.persistentDataPath + "/" + fileName + ".celery";
    }
    public bool LoadData()
    {
        if (!File.Exists(dataFilePath)) return false;

        string readText = File.ReadAllText(dataFilePath);
        data = JsonUtility.FromJson<PlayerData>(readText);

        LoadLevelsInfo();
        LoadFoodInfo();
        return true;
    }

    public void SaveData()
    {
        SaveLevelInfo();
        SaveFoodInfo();

        string temp = JsonUtility.ToJson(data);
        File.WriteAllText(dataFilePath, temp);
    }
    #region Update Data
    private void SaveLevelInfo()
    {
        data.CurrentLoadedLevel = CrossSceneInformation.CurrentLevel;
        data.CurrentLoadedLevelCheckpoint = CrossSceneInformation.CurrentCheckpoint;
        data.CurrentLoadedLevelRoom = CrossSceneInformation.CurrentRoom;
        data.CurrentTimeValue = CrossSceneInformation.CurrentTimerValue;

        data.unlockedLevelsTime = new();
        foreach(LevelInfo level in GameManager.Instance.levels)
        {
            if(level.unlocked == true)
            {
                data.unlockedLevelsTime.Add(level.highscore);
            }
            else break;
        }
    }
    private void LoadLevelsInfo()
    {
        CrossSceneInformation.CurrentLevel = data.CurrentLoadedLevel;
        CrossSceneInformation.CurrentCheckpoint = data.CurrentLoadedLevelCheckpoint;
        CrossSceneInformation.CurrentRoom = data.CurrentLoadedLevelRoom;
        CrossSceneInformation.CurrentTimerValue = data.CurrentTimeValue;

        for (int i = 0; i < data.unlockedLevelsTime.Count; i++)
        {
            GameManager.Instance.levels[i].unlocked = true;
            GameManager.Instance.levels[i].highscore = data.unlockedLevelsTime[i];
        }
    }
    private void SaveFoodInfo()
    {
        data.cookedFoodCount = new();
        foreach (ReceptariInfo food in GameManager.Instance.receptariInfo)
        {
            if (food.found == true)
            {
                data.unlockedLevelsTime.Add(food.cookCount);
            }
            else break;
        }
    }
    private void LoadFoodInfo()
    {
        for (int i = 0; i < data.cookedFoodCount.Count; i++)
        {
            GameManager.Instance.receptariInfo[i].found = true;
            GameManager.Instance.receptariInfo[i].cookCount = data.cookedFoodCount[i];
        }
    }
    #endregion
}
