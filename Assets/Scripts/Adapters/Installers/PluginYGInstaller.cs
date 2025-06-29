using Monetization;
using SaveManager;
using UnityEngine;
using VContainer;
using VContainer.Unity;
public class PluginYGInstaller
{
    public static void Configure(IContainerBuilder builder)
    {
        builder.Register<IAdManager, PluginYGAdManager>(Lifetime.Singleton);
        builder.Register<IGameSaveManager, PluginYGGameSaveManager>(Lifetime.Singleton);
        builder.RegisterComponentOnNewGameObject<PluginYGApplicationQuitHandler>
            (Lifetime.Singleton, 
            nameof(PluginYGApplicationQuitHandler))
            .DontDestroyOnLoad()
            .AsImplementedInterfaces();
    }
}
