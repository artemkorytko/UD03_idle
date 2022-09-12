using UnityEngine;

namespace Idle
{
    [CreateAssetMenu(fileName = "UpgradeConfig", menuName = "Configs/UpgradeConfig", order = 0)]
    public class UpgradeConfig : ScriptableObject
    {
        [SerializeField] private GameObject model;
        [SerializeField] private int processResult;

        public GameObject Model => model;

        public int ProcessResul => processResult;
    }
}