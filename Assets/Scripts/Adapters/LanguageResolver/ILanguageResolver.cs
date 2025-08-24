using Cysharp.Threading.Tasks;
using SaveManager;
using UnityEngine.Events;
using YG;

public interface ILanguageResolver
{
    public int GetSpecifiedLanguageIndex();
    public bool IsInitialized { get; }
    public UniTask Initialize();
    event UnityAction<int> OnLanguageChanged;
}
