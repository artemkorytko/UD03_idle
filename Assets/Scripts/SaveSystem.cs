using System;
using UnityEngine;

namespace Idle
{
    public class SaveSystem : MonoBehaviour
    {
        private const string SAVE_KEY = "GameData";
        
        private GameData _gameData;
        public GameData GameData => _gameData;

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
           string json= JsonUtility.ToJson(_gameData);
           PlayerPrefs.SetString(SAVE_KEY, json);
        }
    }

    [Serializable]
    public class GameData
    {
        public const int BUILDING_COUNT = 4;
        public const int MONEY_BY_DEFAULT = 60;
        public float Money;
        public BuldingData[] BuldingData; 
        

        public void UpdateMoney(float value)
        {
            Money = value;
        }

        public GameData()
        {
            Money = MONEY_BY_DEFAULT;
            BuldingData = new BuldingData[BUILDING_COUNT];
            for (int i = 0; i < BUILDING_COUNT; i++)
            {
                BuldingData[i] = new BuldingData();
            }
        }
    }
}