using System;
using System.Collections;
using UnityEngine;


public class UpgradableBuilding : MonoBehaviour
{
    private const float TIMER_DELAY = 1f;
    
    [SerializeField] private UpgradableBuildingConfig config;
    [SerializeField] private Transform buildingParent;

    public event Action<int> OnProcessFinished;
    public event Action<float> OnMoneySpend; 

    public bool IsUnlock { get; private set; }
    public int Level { get; private set; }
    
    private BuildingButtonController _button;
    private GameObject _currentModel;
    private Coroutine _timerCoroutine;

    private void Awake()
    {
        _button = GetComponentInChildren<BuildingButtonController>();
        GameManager.OnMoneyValueChange += OnMoneyChanged;
    }

    private void Start()
    {
        _button.OnClick += OnButtonClick;
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

        if (config.IsUpgradeExist(Level + 1))
        {
            Level++;
            SetModel(Level);
            UpdateButtonState();
            OnMoneySpend?.Invoke(GetCost(Level));
        }
    }
    
    private void UpdateButtonState()
    {
        if (!IsUnlock)
        {
            _button.UpdateButton("BUY",config.UnlockPrice);
            return;
        }

        if (config.IsUpgradeExist(Level + 1))
        {
            _button.UpdateButton("UPGRADE", GetCost(Level));
        }
    }

    private float GetCost(int level)
    {
        return (float)Math.Round(config.DefaultUpgradeCoast * Math.Pow(config.CostMultiplier, level), 2);
    }

    private void SetModel(int level)
    {
        var upgradeConfig = config.GetUpgrade(level);
        if (_currentModel != null)
        {
            Destroy(_currentModel);
        }

        _currentModel = Instantiate(upgradeConfig.Model, buildingParent);

        if(_timerCoroutine==null)
            _timerCoroutine = StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        while (true)
        {
            yield return new WaitForSeconds(TIMER_DELAY);
            OnProcessFinished?.Invoke(config.GetUpgrade(Level).ProcessResult);
            print("tick");
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
    }
    
}
