using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AddressableAssets;


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
            AnalyticSystem.Instance.BuyBuild();
            SetModel(Level);
            UpdateButtonState();
            OnMoneySpend?.Invoke(config.UnlockPrice);
            return;
        }

        if (config.IsUpgradeExist(Level + 1))
        {
            AnalyticSystem.Instance.UpgradeBuild(name, Level);
            OnMoneySpend?.Invoke(GetCost(Level));
            Level++;
            SetModel(Level);
            UpdateButtonState();
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

    private async void SetModel(int level)
    {
        var upgradeConfig = config.GetUpgrade(level);
        if (_currentModel != null)
        {
           await _currentModel.transform.DOScaleY(0,0.3f);
            Addressables.ReleaseInstance(_currentModel);
        }

        _currentModel = await Addressables.InstantiateAsync(upgradeConfig.Key, buildingParent);
        var scale = _currentModel.transform.localScale;
        var temp = scale.y;
        scale.y = 0;
        _currentModel.transform.localScale = scale;
        var fx = Instantiate(config.BuildEffect);
        fx.transform.position = _currentModel.transform.position;
        await _currentModel.transform.DOScaleY(temp, 0.3f);

        if(_timerCoroutine==null)
            _timerCoroutine = StartCoroutine(Timer());
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
