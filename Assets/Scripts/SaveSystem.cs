using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Idle
{
    public class SaveSystem : MonoBehaviour
    {
        private const string SAVE_KEY = "GameData";

        private GameData _gameData;
        private string _filePath;
        public GameData GameData => _gameData;
        
        public void Initialize()
        {
            // if (PlayerPrefs.HasKey(SAVE_KEY))
            // {
            //     LoadData();
            // }
            // else
            // {
            //     _gameData = new GameData();
            // }
            _filePath = Application.persistentDataPath + "/save.data";
            if (File.Exists(_filePath))
            {
                LoadDataBin();
            }
            else
            {
                _gameData = new GameData();
            }
        }

        private void LoadData()
        {
            string data = PlayerPrefs.GetString(SAVE_KEY, string.Empty);
            _gameData = JsonUtility.FromJson<GameData>(data);
        }

        public void SaveData()
        {
            string json = JsonUtility.ToJson(_gameData);
            PlayerPrefs.SetString(SAVE_KEY, json);
        }

        private void LoadDataBin()
        {
            FileStream dataStream = new FileStream(_filePath, FileMode.Open);

            BinaryFormatter converter = new BinaryFormatter();
            _gameData = converter.Deserialize(dataStream) as GameData;
            dataStream.Close();
        }

        public void SaveDataBin()
        {
            FileStream dataStream = new FileStream(_filePath, FileMode.Create);

            BinaryFormatter converter = new BinaryFormatter();
            converter.Serialize(dataStream, _gameData);
            dataStream.Close();
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
}