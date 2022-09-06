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
            string data = JsonUtility.GetString(SAVE_KEY);
            _gameData = JsonUtility.FromJson<GameData>(data);
        }

        public void SaveData()
        {
           string json= JsonUtility.ToJson(_gameData);
           PlayerPrefs.SetString(SAVE_KEY, json);
        }
    }

    [System.Serializable]
    public class GameData
    {
        public float Money = 60;

        public void UpdateMoney(float value)
        {
            Money = value;
        }
    }
}