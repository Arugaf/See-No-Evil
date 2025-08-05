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
    private IFirmwareLoadScreen screen;
    public async Awaitable Start()
    {
        screen = CoreInstaller.GetFirmwareLoadScreen();
        Debug.Log("GAME INIT...");
        screen.Enabled = true;
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
