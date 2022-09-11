using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
        private SaveSystem _saveSystem;
        private FieldManager _fieldManager;
        private float _money;
        private GameData _gameData;
        private UiController _uiController;

        public static event Action<float> OnMoneyValueChanged; 

        public float Money
        {
                get => _money;
                set
                {
                        if (value<0)
                        {
                                _money = 0;
                        }

                        _money = value;
                        _money = (float)Math.Round(_money, 2);
                        OnMoneyValueChanged?.Invoke(_money);
                }
        }

        private void Awake()
        {
                _uiController = FindObjectOfType<UiController>();
                _saveSystem = GetComponent<SaveSystem>();
                _fieldManager = GetComponentInChildren<FieldManager>();
        }

        private void Start()
        {
                _uiController.OnResetClick += ResetProgress;
                _fieldManager.OnMoneyAdd += OnMoneyAdd;
                _fieldManager.OnMoneySpend += OnMoneySpend;
                _saveSystem.Initialize();
                _gameData = _saveSystem.GameData;
                _fieldManager.Initialize(_gameData);
                Money = _gameData.Money;
        }

        private void OnDestroy()
        {
                _uiController.OnResetClick -= ResetProgress;
                _fieldManager.OnMoneyAdd -= OnMoneyAdd;
                _fieldManager.OnMoneySpend -= OnMoneySpend;
        }

        private void OnMoneySpend(float value)
        {
                Money -= value;
        }

        private void OnMoneyAdd(int value)
        {
                Money += value;
        }

        private void OnApplicationQuit()
        {
                SaveState();
        }

        private void SaveState()
        {
                _gameData.Money = _money;
                _gameData.BuildingData = _fieldManager.GetBuildingData();
                _saveSystem.SaveData();
        }

        private void ResetProgress()
        {
                _fieldManager.ResetProgress();
                _money = 60;
                Money = _money;
        }
}