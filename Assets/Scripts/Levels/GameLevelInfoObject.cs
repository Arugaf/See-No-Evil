using Gameplay.Loot;
using Registries;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;
using VContainer;
using VContainer.Unity;

[CreateAssetMenu(fileName = "GameLevelInfoObject", menuName = "Scriptable Objects/GameLevelInfoObject")]
public class GameLevelInfoObject : IdentifiableScriptableObject, IInstaller
{
    [field: SerializeField] public AssetReference SceneReference { get; private set; }
    [field: SerializeField] public string scoreboardName { get; private set; } // maybe its own struct IDK
    [field: SerializeField] public AbstractGameSettingsInstaller LevelSettings { get; private set; }
    [field: SerializeField] public LocalizedString LocalizedName { get; private set; }
    [field: SerializeField] public GambleBoxLootObject RandomLootObject { get; private set; }
    [field: SerializeField] public float LevelTime { get; private set; }

    public void Install(IContainerBuilder builder)
    {
        builder.RegisterInstance<GameLevelInfoObject>(this);
        LevelSettings.Install(builder);
    }
}