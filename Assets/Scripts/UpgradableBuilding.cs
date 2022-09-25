using UnityEngine;
using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Firebase.Analytics;
using UnityEngine.AddressableAssets;

namespace Idle
{
    public class UpgradableBuilding : MonoBehaviour
    {
         private const float TIMER_DELAY = 1f;

        [SerializeField] private UpgradableBuildingConfig config;
        [SerializeField] private Transform buildingParent;

        private BuildingButtonController _button;
        private GameObject _currentModel;
        private Coroutine _timerCoroutine;
        public bool IsUnlock { get; private set; }
        public int Level { get; private set; }

        public event Action<int> OnProcessFinished;
        public event Action<float> OnMoneySpend;

        private void Awake()
        {
            _button = GetComponentInChildren<BuildingButtonController>();
        }

        private void Start()
        {
            _button.OnClick += OnButtonClick;
            GameManager.Instance.OnMoneyValueChange += OnMoneyChanged;
        }

        private void OnDestroy()
        {
            _button.OnClick -= OnButtonClick;
           GameManager.Instance.OnMoneyValueChange -= OnMoneyChanged;
        }

        private void OnButtonClick()
        {
            DoUpgrade();
        }

        private void DoUpgrade()
        {
            if (!IsUnlock)
            {
                IsUnlock = true;
                SetModel(Level);
                UpdateButtonState();
                OnMoneySpend?.Invoke(config.UnlockPrice);
                return;
            }

            if (config.IsUpgradeExist(Level))
            {
               OnMoneySpend?.Invoke(GetCost(Level));
               Level++;
                SetModel(Level);
                UpdateButtonState();
                FirebaseAnalytics.LogEvent("got_update",new Parameter("type","money"));
            }
        }

        private async void SetModel(int level)
        {
            var upgradeConfig = config.GetUpgrade(level);

            if (_currentModel != null)
            {
               await _currentModel.transform.DOScaleY(0f, 0.5f);
               Addressables.ReleaseInstance(_currentModel);
            }

            _currentModel = await Addressables.InstantiateAsync(upgradeConfig.Model, buildingParent);
            _currentModel.transform.localPosition = Vector3.zero;
            var scale = _currentModel.transform.localScale;
            var temp = scale.y;
            scale.y = 0f;
            _currentModel.transform.localScale = scale;
            var fx = Instantiate(config.BuildEffect);
            fx.transform.position = _currentModel.transform.position;
            await _currentModel.transform.DOScaleY(temp, 0.5f);

           if (_timerCoroutine == null)
               _timerCoroutine = StartCoroutine(Timer());
        }

        private void UpdateButtonState()
        {
            if (!IsUnlock)
            {
                _button.UpdateButton("BUY", config.UnlockPrice);
                return;
            }

            if (config.IsUpgradeExist(Level + 1))
            {
                _button.UpdateButton("UPGRADE", GetCost(Level));
            }
        }

        private float GetCost(int level)
        {
            return (float) Math.Round(config.DefaultUpgradeCost * Math.Pow(config.CostMultiplier, level), 2);
        }

        private IEnumerator Timer()
        {
            while (true)
            {
                yield return new WaitForSeconds(TIMER_DELAY);
                OnProcessFinished?.Invoke(config.GetUpgrade(Level).ProcessResult);
            }
        }

        private void OnMoneyChanged(float value)
        {
            _button.OnMoneyValueChanged(value);
        }

        public void Initialize(bool isUnlock, int upgradeLevel)
        {
            IsUnlock = isUnlock;
            Level = upgradeLevel;

            if (IsUnlock)
            {
                SetModel(Level);
            }

            UpdateButtonState();
            OnMoneyChanged(GameManager.Instance.Money);
        }
    }
}