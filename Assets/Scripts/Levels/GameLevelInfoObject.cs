using Registries;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "GameLevelInfoObject", menuName = "Scriptable Objects/GameLevelInfoObject")]
public class GameLevelInfoObject : IdentifiableScriptableObject
{
    [field: SerializeField] public AssetReference SceneReference { get; private set; }
    [field: SerializeField] public string scoreboardName { get; private set; } // maybe its own struct IDK
    [field: SerializeField] public AbstractGameSettingsInstaller LevelSettings { get; private set; }
    [field: SerializeField] public LocalizedString LocalizedName { get; private set; }
}
