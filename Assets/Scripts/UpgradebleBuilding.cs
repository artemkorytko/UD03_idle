using System;
using System.Collections;
using UnityEngine;

namespace Idle
{
    public class UpgradebleBuilding : MonoBehaviour
    {
        private const float TIMER_DELAY = 1f;
        
        
        [SerializeField] private UpgradableBuildingConfig config;
        [SerializeField] private Transform buildingParent;

        private BildingButtonController _button;
        private GameObject _currentModel;
        private Coroutine _timerCoroutina;
        
        public bool IsUnlock { get; private set; }
        public int Level { get; private set; }

        public event Action<int> OnProcessFinished;
        public event Action<float> OnMoneySpend; 


        private void Awake()
        {
            _button = GetComponentInChildren<BildingButtonController>();
        }

        private void Start()
        {
            _button.OnClick += OnButtonClick;
            GameManager.OnMoneyValueChange += OnMoneyChanged;
        }

        private void OnDestroy()
        {
            _button.OnClick -= OnButtonClick;
            GameManager.OnMoneyValueChange -= OnMoneyChanged;
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

            if (config.IsUpgradeExist(Level+1))
            {
                Level++;
                SetModel(Level);
                UpdateButtonState();
                OnMoneySpend?.Invoke(GetCost(Level));
            }
        }
        
        private void SetModel(int level)
        {
            var upgradeConfig = config.GetUpgrade(level);

            if (_currentModel != null)
            {
                Destroy(_currentModel);
            }

            _currentModel = Instantiate(upgradeConfig.Model, buildingParent);
            _currentModel.transform.localPosition = Vector3.zero;
            if (_timerCoroutina == null)
                _timerCoroutina = StartCoroutine(Timer());
            
        }
        private void UpdateButtonState()
        {
            if (!IsUnlock)
            {
                _button.UpdateButton("BUY", config.UnlockPrice);
                return;
                ;
            }

            if (config.IsUpgradeExist(Level +1))
            {
                _button.UpdateButton("UPGADE", GetCost(Level));
            }
        }

        private float GetCost(int level)
        {
           return  (float)Math.Round(config.DefaultUpgrateCost * Math.Pow(config.CostMultiplayer, level), 2);
        }

        private IEnumerator Timer()
        {
            while (true)
            {
                yield return new WaitForSeconds(TIMER_DELAY);
                OnProcessFinished?.Invoke(config.GetUpgrade(Level).ProcessResul);
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

            if (isUnlock)
            {
                SetModel(Level);
            }

            UpdateButtonState();
        }

       
    }
}