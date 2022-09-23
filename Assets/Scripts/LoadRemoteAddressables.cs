
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class LoadRemoteAddressables : MonoBehaviour
{
    private GameObject _loadedObject;

    public void Instantiate(string key)
    {
        Addressables.InstantiateAsync(key).Completed += OnLoadDone;
    }

    private void OnLoadDone(AsyncOperationHandle<GameObject> obj)
    {
        _loadedObject = obj.Result;
    }
}
