﻿using System;
using UnityEngine;

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
                        building.OnProcessFinished += OnMoneyAdd;
                        building.OnMoneySpend += OnMoneySpend;
                }
        }

        private void OnDestroy()
        {
                foreach (var building in _upgradableBuildings)
                {
                        building.OnProcessFinished -= OnMoneyAdd;
                        building.OnMoneySpend -= OnMoneySpend;
                }
        }

        public void Initialize(GameData gameData)
        {
                var buildingsData = gameData.BuildingData;
                for (int i = 0; i < buildingsData.Length; i++)
                {
                        _upgradableBuildings[i].Initialize(buildingsData[i].IsUnlock, buildingsData[i].UpgradeLevel);
                }
        }

        public void ResetProgress()
        {
                foreach (var buildingsData in _upgradableBuildings)
                {
                        buildingsData.ResetProgress();
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