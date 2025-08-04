using Features.VFX;
using Gameplay;
using Gameplay.Loot;
using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;
public abstract class AbstractGameSettingsInstaller : ScriptableObject, IInstaller
{
    public abstract void Install(IContainerBuilder builder);
}
[Serializable]
public class GameplayLootSettings
{
    public GameObject LootBoxPrefab;
}
[CreateAssetMenu(fileName = "GameSettingsInstaller", menuName = "Scriptable Objects/GameSettingsInstaller")]
public class GameSettingsInstaller : AbstractGameSettingsInstaller
{
    [SerializeField] private GameplayState.Settings GameplayStateSettings;
    [SerializeField] private DarknessMeterController.Settings DarknessMeterControllerSettings;
    [SerializeField] private GameplayDarknessManager.Settings GameplayDarknessManagerSettings;
    [SerializeField] private DarknessAudioManager.Settings DarknessAudioSettings;
    [Header("The prefab that would be spawned in all scenes")]
    [SerializeField] private GameObject GameSceneFixture;
    public override void Install(IContainerBuilder builder)
    {
        builder.RegisterInstance(DarknessMeterControllerSettings);
        builder.RegisterInstance(GameplayDarknessManagerSettings);
        builder.RegisterInstance(DarknessAudioSettings);
        builder.Register((irp) =>
        {
            GameplayLootSettings stx = new GameplayLootSettings();
            stx.LootBoxPrefab = irp.Resolve<GameLevelInfoObject>().RandomLootObject.InGamePrefab;
            return stx;
        }, Lifetime.Singleton);
        builder.Register((irp) =>
        {
            GameplayState.Settings settings = GameplayStateSettings;
            settings.InitialTime = irp.Resolve<GameLevelInfoObject>().LevelTime;
            return settings;
        }, Lifetime.Singleton);
        builder.Register<IGameplayScoreManager, GameplayScoreManager>(Lifetime.Singleton);
        builder.RegisterEntryPoint<DarknessAudioManager>(Lifetime.Singleton).AsSelf();
        builder.RegisterEntryPoint<GameplayState>(Lifetime.Singleton).AsSelf();
        builder.RegisterEntryPoint<DarknessMeterController>(Lifetime.Singleton).AsSelf();
        builder.RegisterEntryPoint<GameplayDarknessManager>(Lifetime.Singleton).AsSelf();
        builder.RegisterEntryPoint<GameplayLootManager>(Lifetime.Singleton).AsSelf();
        builder.RegisterBuildCallback((_) =>
        {
            if(GameSceneFixture != null) Instantiate(GameSceneFixture);
        });
    }
}
