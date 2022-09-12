using System;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public GameData GameData => _gameData;
    
    private GameData _gameData;
    private const string SAVE_KEY = "GameData";

    public void Initialize()
    {
        if (PlayerPrefs.HasKey(SAVE_KEY))
        {
            LoadData();
        }
        else
        {
            _gameData = new GameData();
        }
    }

    public void LoadData()
    {
        string data = PlayerPrefs.GetString(SAVE_KEY);
        _gameData = JsonUtility.FromJson<GameData>(data);
    }

    public void SaveData()
    {
        string json = JsonUtility.ToJson(_gameData);
        PlayerPrefs.SetString(SAVE_KEY, json);
    }
}

[Serializable]
public class GameData
{
    public const int BUILDING_COUNT = 4;
    public const int MONEY_BY_DEFAULT = 60;
    public float Money;
    public BuildingData[] BuildingData;

    public void UpdateMoney(float value)
    {
        Money = value;
    }

    public GameData()
    {
        Money = MONEY_BY_DEFAULT;
        BuildingData = new BuildingData[BUILDING_COUNT];
        for (int i = 0; i < BUILDING_COUNT; i++)
        {
            BuildingData[i] = new BuildingData();
        }
    }
}

