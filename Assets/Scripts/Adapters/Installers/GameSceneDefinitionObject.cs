using Cysharp.Threading.Tasks;
using Registries;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using VContainer.Unity;
public interface IGameSceneDefinition
{
    public UniTask LoadMenu();
    public UniTask LoadGameplay(GameLevelInfoObject levelObject);
    public UniTask LoadGameOver();
}
public interface ILevelDefinition
{
    public int LevelCount
    {
        get;
    }
    IReadOnlyCollection<GameLevelInfoObject> Levels { get; }
    public bool TryGetNext(string currentID, out GameLevelInfoObject next);
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
        using (LifetimeScope.Enqueue(levelObject))
        {
            await levelObject.SceneReference.LoadSceneAsync();
        }
    }

    public UniTask LoadGameOver() => GameOverSceneReference.LoadSceneAsync().ToUniTask();

    public bool TryGetNext(string currentID, out GameLevelInfoObject next)
    {
        next = null;
        bool selectNext = false;
        foreach (var x in Values)
        {
            if (x.ID == currentID)
            {
                selectNext = true;
            }
            else if (selectNext)
            {
                next = x;
                return true;
            }
        }
        return false;
    }
}
