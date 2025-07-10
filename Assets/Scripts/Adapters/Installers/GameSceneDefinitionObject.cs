using Cysharp.Threading.Tasks;
using Registries;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using VContainer.Unity;
public interface IGameSceneDefinition
{
    public int LevelCount
    {
        get;
    }
    public UniTask LoadMenu();
    public UniTask LoadGameplay(GameLevelInfoObject levelObject);
    public UniTask LoadGameOver();
}
public interface ILevelDefinition
{
    IReadOnlyCollection<GameLevelInfoObject> Levels { get; }
}
[CreateAssetMenu(fileName = "GameSceneDefinitionObject", menuName = "Scriptable Objects/GameSceneDefinitionObject")]
public class GameSceneDefinitionObject : IdentifiableRegistry<GameLevelInfoObject>, IGameSceneDefinition, ILevelDefinition
{
    [field: SerializeField] public AssetReference MenuSceneReference { get; private set; }

    [field: SerializeField] public AssetReference GameOverSceneReference { get; private set; }
    public int LevelCount { get => Values.Length; }

    public IReadOnlyCollection<GameLevelInfoObject> Levels => Values;

    public UniTask LoadMenu() => MenuSceneReference.LoadSceneAsync().ToUniTask();
    public async UniTask LoadGameplay(GameLevelInfoObject levelObject)
    {
        using (LifetimeScope.Enqueue(levelObject.LevelSettings))
        {
            await levelObject.SceneReference.LoadSceneAsync();
        }
    }

    public UniTask LoadGameOver() => GameOverSceneReference.LoadSceneAsync().ToUniTask();

}
