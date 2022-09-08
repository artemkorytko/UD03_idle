using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeConfig", menuName = "configs/UpgradeConfig", order = 0)]
public class UpgradeConfig : ScriptableObject
{
        [SerializeField] private GameObject modelPref;
        [SerializeField] private int processResult;

        public GameObject ModelPref => modelPref;
        public int ProcessResult => processResult;
}