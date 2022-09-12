using System;
using UnityEngine;

namespace Idle
{
    public class FieldManager : MonoBehaviour
    {
        private UpgradebleBuilding[] _upgradebleBuildings;

        public event Action<int> OnMoneyAdd;
        public event Action<float> OnMoneySpend;

        private void Awake()
        {
            _upgradebleBuildings = GetComponents<UpgradebleBuilding>();
        }

        private void Start()
        {
            foreach (var building in _upgradebleBuildings)
            {
                building.OnProcessFinished += OnMoneyAdd;
                building.OnMoneySpend += OnMoneySpend;
            }
        }

        private void OnDestroy()
        {
            foreach (var building in _upgradebleBuildings)
            {
                building.OnProcessFinished -= OnMoneyAdd;
                building.OnMoneySpend -= OnMoneySpend;
            }
        }

        public void Initialize(GameData gameData)
        {
            var buildingsData = gameData.BuldingData;
            for (int i = 0; i < buildingsData.Length; i++)
            {
                _upgradebleBuildings[i].Initialize(buildingsData[i].IsUnlock, buildingsData[i].UpgradeLevel);
            }
        }

        public BuldingData[] GetBuildingData()
        {
            var data = new BuldingData[GameData.BUILDING_COUNT];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = new BuldingData(_upgradebleBuildings[i].IsUnlock, _upgradebleBuildings[i].Level);
            }

            return data;
        }
    }
}