using System;
using UnityEngine;

namespace Idle
{
    public class FieldManager : MonoBehaviour
    {
        private UpgradableBuilding[] _upgradableBuildings;

        public event Action<int> OnMoneyAdd;
        public event Action<float> OnMoneySpend;

        private void Awake()
        {
            _upgradableBuildings = GetComponentsInChildren<UpgradableBuilding>();
        }

        private void Start()
        {
            foreach (var building in _upgradableBuildings)
            {
                building.OnProcessFinished += OnMoneyAdded;
                building.OnMoneySpend += OnMoneySpended;
            }
        }

        private void OnDestroy()
        {
            foreach (var building in _upgradableBuildings)
            {
                building.OnProcessFinished -= OnMoneyAdded;
                building.OnMoneySpend -= OnMoneySpended;
            }
        }

        private void OnMoneyAdded(int value)
        {
            OnMoneyAdd?.Invoke(value);
        }

        private void OnMoneySpended(float value)
        {
            OnMoneySpend?.Invoke(value);
        }

        public void Initialize(GameData gameData)
        {
            var buildingsData = gameData.BuildingData;
            for (int i = 0; i < buildingsData.Length; i++)
            {
                _upgradableBuildings[i].Initialize(buildingsData[i].IsUnlock, buildingsData[i].UpgradeLevel);
            }
        }

        public BuildingData[] GetBuildingData()
        {
            var data = new BuildingData[GameData.BUILDING_COUNT];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = new BuildingData(_upgradableBuildings[i].IsUnlock, _upgradableBuildings[i].Level);
            }

            return data;
        }
    }
}