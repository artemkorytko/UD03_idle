using UnityEngine;
using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Firebase.Extensions;

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

        private async void Awake()
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
           await _saveSystem.Initialize();
            _gameData = _saveSystem.GameData;
            Money = _gameData.Money;
            FetchDataAsync();
        }

        public Task FetchDataAsync() {
            Debug.Log("Fetching data...");
            System.Threading.Tasks.Task fetchTask =
                Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.FetchAsync(
                    TimeSpan.Zero);
            return fetchTask.ContinueWithOnMainThread(FetchComplete);
        }
        //[END fetch_async]
        
        public void DebugLog(string s) {
            print(s);
        }


        void FetchComplete(Task fetchTask) {
            if (fetchTask.IsCanceled) {
                Debug.Log("Fetch canceled.");
            } else if (fetchTask.IsFaulted) {
                Debug.Log("Fetch encountered an error.");
            } else if (fetchTask.IsCompleted) {
                Debug.Log("Fetch completed successfully!");
            }

            var info = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.Info;
            switch (info.LastFetchStatus) {
                case Firebase.RemoteConfig.LastFetchStatus.Success:
                    Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.ActivateAsync()
                        .ContinueWithOnMainThread(task => {
                            Debug.Log(String.Format("Remote data loaded and ready (last fetch time {0}).",
                                info.FetchTime));
                            var value = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue("MoneyOnStart").LongValue;
                            Money = value;
                        });

                    break;
                case Firebase.RemoteConfig.LastFetchStatus.Failure:
                    switch (info.LastFetchFailureReason) {
                        case Firebase.RemoteConfig.FetchFailureReason.Error:
                            Debug.Log("Fetch failed for unknown reason");
                            break;
                        case Firebase.RemoteConfig.FetchFailureReason.Throttled:
                            Debug.Log("Fetch throttled until " + info.ThrottledEndTime);
                            break;
                    }
                    break;
                case Firebase.RemoteConfig.LastFetchStatus.Pending:
                    Debug.Log("Latest Fetch call still pending.");
                    break;
            }
        }

        private void Start()
        {
            Firebase.Analytics.FirebaseAnalytics.LogEvent(Firebase.Analytics.FirebaseAnalytics.EventLogin);
            _fieldManager.OnMoneyAdd += OnMoneyAdd;
            _fieldManager.OnMoneySpend += OnMoneySpend;
            _uiController.ShowMenuScreen();
            _uiController.OnStartGameButtonClick += StartGame;
            
            // Initialize Firebase
            Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
                var dependencyStatus = task.Result;
                if (dependencyStatus == Firebase.DependencyStatus.Available)
                {
                    // Create and hold a reference to your FirebaseApp,
                    // where app is a Firebase.FirebaseApp property of your application class.
                    // Crashlytics will use the DefaultInstance, as well;
                    // this ensures that Crashlytics is initialized.
                    Firebase.FirebaseApp app = Firebase.FirebaseApp.DefaultInstance;

                    // Set a flag here for indicating that your project is ready to use Firebase.
                }
                else
                {
                    UnityEngine.Debug.LogError(System.String.Format(
                        "Could not resolve all Firebase dependencies: {0}",dependencyStatus));
                    // Firebase Unity SDK is not safe to use here.
                }
            });
        }

        private void StartGame()
        {
            _uiController.ShowGameScreen();
            _fieldManager.Initialize(_gameData);
            //throw new Exception("Game fault on start");
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
             //_saveSystem.SaveData();
            //_saveSystem.SaveDataBin();
            _saveSystem.SaveDataCloud();
        }

        public void DoQuit()
        {
            Application.Quit();
        }
    }
}