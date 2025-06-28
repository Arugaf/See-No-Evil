using Monetization;
using SaveManager;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class PluginYGInstaller : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<IAdManager, PluginYGAdManager>(Lifetime.Singleton);
        builder.Register<IGameSaveManager, PluginYGGameSaveManager>(Lifetime.Singleton);
        // TODO: this should NOT be a part of PluginYGInstaller
        SaveManagerInstaller.UseHierachyInstallment(builder);
    }
}
