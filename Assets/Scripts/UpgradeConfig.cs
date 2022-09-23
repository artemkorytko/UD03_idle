using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "UpgradeConfig", menuName = "Config/UpgradeConfig", order = 0)]
public class UpgradeConfig : ScriptableObject
{
    [SerializeField] private string key;
    [SerializeField] private int processResult;

    public string Key => key;
    public int ProcessResult => processResult;
}
