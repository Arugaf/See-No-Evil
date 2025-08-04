using Gameplay;
using Gameplay.LevelStats;
using Gameplay.Loot;
using Leaderboard;
using Levels;
using SaveManager;
using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
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
        // guys we HAVE to save that shit NGL
        saveManager.Save().GetAwaiter().GetResult();
    }
}
[Serializable]
public class GameplayCanvasInstaller
{
    [SerializeField] private GameObject PCGameplayCanvas;
    [SerializeField] private GameObject MobileGameplayCanvas;
    public void Configure(IContainerBuilder builder)
    {
        bool isMobile = StaticPlatformDefiner.IsMobile();
        GameObject chosenOne = (isMobile ? MobileGameplayCanvas : PCGameplayCanvas);
        builder.RegisterFactory<AbstractGameplayUIView>(container => 
        {
            return () => container.Instantiate(chosenOne).GetComponent<AbstractGameplayUIView>(); // Execute per factory invocation
        }, Lifetime.Scoped);
    }
}
public class CoreInstaller: LifetimeScope
{
    [SerializeField] private AudioMixer mainAudioMixer;
    [SerializeField] private InputActionAsset mainInputActionAsset;
    [SerializeField] private GameplayCanvasInstaller gameplayCanvasInstaller;
    [SerializeField] private GameSceneDefinitionObject sceneDefinitionObject;
    [SerializeField] private LootRegistryScriptableObject lootRegistry;
    [SerializeField] private BasicScoreEvaluator.Settings scoreSettings;
    [SerializeField] private GameLevelManager.Settings gameManagerSettings;
    protected override void Configure(IContainerBuilder builder)
    {
        // Main systems
        builder.RegisterEntryPoint<GameStateManager>(Lifetime.Singleton).AsSelf();
        builder.Register<GameplayResultStorage>(Lifetime.Singleton).AsSelf();
        builder.Register<IScoreEvaluator, BasicScoreEvaluator>(Lifetime.Singleton);
        SaveManagerInstaller.UseHierachyInstallment(builder);
        builder.RegisterInstance(gameManagerSettings);
        builder.RegisterInstance(scoreSettings);
        builder.RegisterInstance(mainAudioMixer);
        builder.RegisterInstance(mainInputActionAsset);
        builder.RegisterInstance(lootRegistry);
        builder.Register<SettingsManager>(Lifetime.Singleton).AsImplementedInterfaces();
        builder.Register<LevelStatsManager>(Lifetime.Singleton).AsImplementedInterfaces();
        builder.Register<GameLootManager>(Lifetime.Singleton).AsImplementedInterfaces();
        builder.Register<GlobalLeaderboardScoreSaver>(Lifetime.Singleton).AsImplementedInterfaces();
        builder.Register<IApplicationQuitAction, GameActionOnQuit>(Lifetime.Singleton);
        builder.RegisterInstance(sceneDefinitionObject).AsImplementedInterfaces();
        builder.Register<IGameLevelManager, GameLevelManager>(Lifetime.Singleton);
        gameplayCanvasInstaller.Configure(builder);
        // who thought it was a good idea UNITY? WHYYYY
        mainInputActionAsset.Enable();
        
#if PLUGIN_YG_2
        PluginYGInstaller.Configure(builder);
        #else
        #error "Should not hold YG2"
        #endif
    }
}
