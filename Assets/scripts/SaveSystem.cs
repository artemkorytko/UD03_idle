using System;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

namespace DefaultNamespace
{
    public class SaveSystem : MonoBehaviour
    {
        private GameData _gameData;
        private const string SAVE_KEY = "GameData";
        
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
            string json = JsonUtility.ToJson(_gameData);
            PlayerPrefs.SetString(SAVE_KEY, json);
        }
    }

    [Serializable]
    public class GameData
    {
        public  float Money = 60;

        public void UpdateMoney(float value)
        {
            Money = value;
        }
    }
}