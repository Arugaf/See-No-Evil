using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using VContainer.Unity;

public class EntryPoint : MonoBehaviour
{
    public AssetReferenceGameObject firmware;
    public async Awaitable Start()
    {
        GameObject x = await firmware.LoadAssetAsync();
        var instance = Instantiate(x);
        LifetimeScope.EnqueueParent(instance.GetComponent<LifetimeScope>());
        DontDestroyOnLoad(instance);
    }
}
