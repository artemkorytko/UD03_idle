using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Idle
{
    [CreateAssetMenu(fileName = "UpgradeConfig", menuName = "Configs/UpgradeConfig", order = 0)]
    public class UpgradeConfig : ScriptableObject
    {
        [SerializeField] private AssetReference model;
        [SerializeField] private int processResult;

        public AssetReference Model => model;

        public int ProcessResult => processResult;
    }
}