using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using VContainer.Unity;
public interface IFirmwareLoadScreen
{
    public bool Enabled { set; }
    public float Progress { set; }
}
public class EntryPoint : MonoBehaviour
{
    public AssetReferenceGameObject firmware;
    public List<AssetReference> webglPreloadAssets;
    private IFirmwareLoadScreen screen;
    public async Awaitable Start()
    {
        screen = CoreInstaller.GetFirmwareLoadScreen();
        Debug.Log("GAME INIT...");
        screen.Enabled = true;
        // I REALLY don't want the GAME to be broken later because of a bad internet
        // So I load everything at the start, and never release it.
        // Yes, this is a bad approach, but our game isn't too big to take this into account.
#if UNITY_WEBGL
        foreach (var reference in webglPreloadAssets)
        {
            await ProgressBarUpdator(Addressables.LoadResourceLocationsAsync(reference), screen);
        }
#endif
        GameObject x = await ProgressBarUpdator(firmware.LoadAssetAsync(), screen);
        var instance = Instantiate(x);
        LifetimeScope.EnqueueParent(instance.GetComponent<LifetimeScope>());
        DontDestroyOnLoad(instance);
    }
    public async UniTask<T> ProgressBarUpdator<T>(AsyncOperationHandle<T> handle, IFirmwareLoadScreen screen)
    {
        float wasPerc = 0;
        while (!handle.IsDone)
        {
            if (handle.PercentComplete > wasPerc)
            {
                wasPerc = handle.PercentComplete;
                screen.Progress = wasPerc;
            }
            await UniTask.WaitForEndOfFrame();
        }
        screen.Progress = 1;
        return handle.Result;
    }
    public void OnDestroy()
    {
        screen.Enabled = false;
    }
}
