using SaveManager;
using UnityEngine;
using UnityEngine.Audio;
using VContainer;
using VContainer.Unity;
public class GameActionOnQuit : IApplicationQuitAction
{
    private IGameSaveManager saveManager;
    [Inject]
    public GameActionOnQuit(IGameSaveManager saveManager)
    {
        this.saveManager = saveManager;
    }
    public void OnApplicationQuit()
    {
        saveManager.Save().GetAwaiter().GetResult();
    }
}
public class CoreInstaller: LifetimeScope
{
    [SerializeField] private AudioMixer mainAudioMixer;
    protected override void Configure(IContainerBuilder builder)
    {
        SaveManagerInstaller.UseHierachyInstallment(builder);
        builder.RegisterInstance(mainAudioMixer);
        builder.Register<SettingsManager>(Lifetime.Singleton).AsImplementedInterfaces();
        builder.Register<IApplicationQuitAction, GameActionOnQuit>(Lifetime.Singleton);
        PluginYGInstaller.Configure(builder);
    }
}
