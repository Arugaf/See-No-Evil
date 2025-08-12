using System;
using System.Threading;
using System.Threading.Tasks;
using Auth;
using Cysharp.Threading.Tasks;
using External;
using Leaderboard;
using Monetization;
using SaveManager;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using YG;
public class PluginYGFirmwareScreen : IFirmwareLoadScreen
{
    public bool Enabled { set => YG2.SetLoadPageVisible(value); }
    public float Progress { set => YG2.SetLoadPageProgress(value); }
}
public static class PluginYGInstaller
{
    public static IFirmwareLoadScreen GetFirmwareLoadScreen()
    {
        return new PluginYGFirmwareScreen();
    }
    public static void Configure(IContainerBuilder builder)
    {
        builder.Register<IAdManager, PluginYGAdManager>(Lifetime.Singleton);
        builder.Register<IGameSaveManager, PluginYGGameSaveManager>(Lifetime.Singleton);
        builder.Register<ILanguageResolver, PluginYGLanguageResolver>(Lifetime.Singleton);
        builder.Register<PluginYGAuthManager>(Lifetime.Singleton).AsImplementedInterfaces();
        builder.Register<PluginYGLeaderboardMaster>(Lifetime.Singleton).AsImplementedInterfaces();
        builder.RegisterComponentOnNewGameObject<PluginYGApplicationQuitHandler>
            (Lifetime.Singleton,
            nameof(PluginYGApplicationQuitHandler))
            .DontDestroyOnLoad()
            .AsImplementedInterfaces();
        builder.RegisterEntryPoint<PluginYGDefiblirator>(Lifetime.Singleton);
        builder.Register<ILinkOpener, PluginYGLinkOpener>(Lifetime.Singleton);
        builder.Register<IReviewOpener, PluginYGReviewOpener>(Lifetime.Singleton);
        builder.Register<IAddAsLinkButton, PluginYGAddAsLinkButton>(Lifetime.Singleton);
        builder.Register<IGameReporter, PluginYGGameReporter>(Lifetime.Singleton);
    }
    // I hate this shit so much you cant imagine ADDRESSABLES HAVE BEEN LOBOTOMIZED MY PLUGINYG TwT
    public class PluginYGDefiblirator : IAsyncStartable
    {
        [Inject]
        public PluginYGDefiblirator()
        {
            
        }
        public async Awaitable StartAsync(CancellationToken cancellation = default)
        {
            while (true)
            {
                try
                {
                    // to be cool (69 is cool)
                    await UniTask.WaitForSeconds(0.69f, true);
                    if (!YG2.isSDKEnabled)
                    {
                        YG2.StartInit();
                    }
                    else
                    {
                        break;
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError("DEFIBLIRATOR ERROR: " + ex.ToString());
                }
            }
        }
    }
}
