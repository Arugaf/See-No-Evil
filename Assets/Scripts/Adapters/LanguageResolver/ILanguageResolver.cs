using Cysharp.Threading.Tasks;
using UnityEngine.Events;

public interface ILanguageResolver
{
    public int GetSpecifiedLanguageIndex();
    public bool IsInitialized { get; }
    public UniTask Initialize();
    event UnityAction<int> OnLanguageChanged;
}
