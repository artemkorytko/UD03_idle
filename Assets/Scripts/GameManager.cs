using System;
using UnityEngine;

namespace Idle
{
    public class GameManager : MonoBehaviour
    {
        private SaveSystem _saveSystem;
        private FieldManager _fieldManager;

        private float _money;
        private GameData _gameData;

        public static event Action<float> OnMoneyValueChange;

        public float Money
        {
            get => _money;
            set
            {
                if (value <0)
                {
                    _money = 0;
                }

                _money = value;
                _money = (float)Math.Round(_money, 2);
                OnMoneyValueChange?.Invoke(_money);
            }
        }

        private void Awake()
        {
            _saveSystem = GetComponent<SaveSystem>();
            _fieldManager = GetComponent<FieldManager>();
            
        }


        private void Start()
        {
            _fieldManager.OnMoneyAdd += OnMoneyAdd;
            _fieldManager.OnMoneySpend += OnMoneySpend;
            _saveSystem.Initialize();
            _gameData = _saveSystem.GameData;
            _fieldManager.Initialize(_gameData);
            Money = _gameData.Money;
        }

        private void OnApplicationQuit()
        {
            SaveState();
        }

        private void OnMoneySpend(float value)
        {
            Money -= value;
        }

        private void OnMoneyAdd(int value)
        {
            Money += value;
        }

        private void OnDestroy()
        {
            _fieldManager.OnMoneyAdd -= OnMoneyAdd;
            _fieldManager.OnMoneySpend -= OnMoneySpend;
        }

        private void SaveState()
        {
            _gameData.Money = _money;
            _gameData.BuldingData = _fieldManager.GetBuildingData();
            _saveSystem.SaveData();
        }
    }
}