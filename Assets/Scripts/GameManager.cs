using System;
using UnityEngine;

namespace Idle
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        private SaveSystem _saveSystem;
        private FieldManager _fieldManager;
        private UiController _uiController;
        private float _money;
        private GameData _gameData;
        public event Action<float> OnMoneyValueChange;

        public float Money
        {
            get => _money;
            set
            {
                if (value < 0)
                {
                    _money = 0;
                }

                _money = value;
                _money = (float) Math.Round(_money, 2);
                OnMoneyValueChange?.Invoke(_money);
            }
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                DestroyImmediate(gameObject);
                return;
            }

            _saveSystem = GetComponent<SaveSystem>();
            _fieldManager = GetComponentInChildren<FieldManager>();
            _uiController = FindObjectOfType<UiController>();
            _saveSystem.Initialize();
            _gameData = _saveSystem.GameData;
            //Money = _gameData.Money;
        }

        private void Start()
        {
            _fieldManager.OnMoneyAdd += OnMoneyAdd;
            _fieldManager.OnMoneySpend += OnMoneySpend;
            _uiController.ShowMenuScreen();
            _uiController.OnStartGameButtonClick += StartGame;
        }

        private void StartGame()
        {
            _uiController.ShowGameScreen();
            _fieldManager.Initialize(_gameData);
        }

#if !UNITY_EDITOR
         private void OnApplicationFocus(bool hasFocus)
        {
            if (!hasFocus)
            {
                SaveState();
            }
        }
#else
        private void OnApplicationQuit()
        {
            SaveState();
        }
#endif
        private void OnDestroy()
        {
            _fieldManager.OnMoneyAdd -= OnMoneyAdd;
            _fieldManager.OnMoneySpend -= OnMoneySpend;
            _uiController.OnStartGameButtonClick -= StartGame;
        }

        private void OnMoneyAdd(int value)
        {
            Money += value;
        }

        private void OnMoneySpend(float value)
        {
            Money -= value;
        }

        private void SaveState()
        {
            _gameData.Money = Money;
            _gameData.BuildingData = _fieldManager.GetBuildingData();
            // _saveSystem.SaveData();
            // _saveSystem.SaveDataBin();
            _saveSystem.SaveDataCloud();
        }

        public void DoQuit()
        {
            Application.Quit();
        }
    }
}