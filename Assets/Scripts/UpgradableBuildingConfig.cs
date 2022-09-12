using UnityEngine;

namespace Idle
{
    [CreateAssetMenu(fileName = "UpgradableBuildingConfig", menuName = "Configs/UpgradableBuildingConfig", order = 0)]
    public class UpgradableBuildingConfig : ScriptableObject
    {
        [SerializeField] private float unlockPrice = 30;
        [SerializeField] private float defaultUpgrateCost = 10;
        [SerializeField] private float costMultiplayer = 1.7f;
        [SerializeField] private UpgradeConfig[] configs;

        public float UnlockPrice => unlockPrice;

        public float DefaultUpgrateCost => defaultUpgrateCost;

        public float CostMultiplayer => costMultiplayer;

        public bool IsUpgradeExist(int index)
        {
            return index >= 0 && index < configs.Length;
        }

        public UpgradeConfig GetUpgrade(int index)
        {
            if (index <0|| index >= configs.Length)
            {
                return null;    
            }
            
            return configs[index];
        }
    }
}