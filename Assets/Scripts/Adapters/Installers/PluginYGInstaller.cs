using Auth;
using Leaderboard;
using Monetization;
using SaveManager;
using UnityEngine;
using VContainer;
using VContainer.Unity;
public static class PluginYGInstaller
{
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
    }
}
