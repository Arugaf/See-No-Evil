using Monetization;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class PluginYGInstaller : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<IAdManager, PluginYGAdManager>(Lifetime.Singleton);
    }
}
