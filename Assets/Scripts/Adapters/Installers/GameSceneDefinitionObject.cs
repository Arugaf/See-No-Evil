using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
public interface IGameSceneDefinition
{
    public int LevelCount
    {
        get;
    }
    public UniTask LoadMenu();
    public UniTask LoadGameplay(int levelIndex);
    public UniTask LoadGameOver();
}
[CreateAssetMenu(fileName = "GameSceneDefinitionObject", menuName = "Scriptable Objects/GameSceneDefinitionObject")]
public class GameSceneDefinitionObject : ScriptableObject, IGameSceneDefinition
{
    [field: SerializeField] public AssetReference MenuSceneReference { get; private set; }
    [field: SerializeField] public AssetReference[] GameplayScenesReferences { get; private set; }

    [field: SerializeField] public AssetReference GameOverSceneReference { get; private set; }
    public int LevelCount { get => GameplayScenesReferences.Length; }
    public UniTask LoadMenu() => MenuSceneReference.LoadSceneAsync().ToUniTask();
    public UniTask LoadGameplay(int levelIndex) => GameplayScenesReferences[levelIndex].LoadSceneAsync().ToUniTask();

    public UniTask LoadGameOver() => GameOverSceneReference.LoadSceneAsync().ToUniTask();

}
