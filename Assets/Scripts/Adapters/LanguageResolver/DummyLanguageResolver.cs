using Cysharp.Threading.Tasks;
using UnityEngine.Events;
using UnityEngine.Localization.Settings;

public class DummyLanguageResolver : ILanguageResolver
{
    public bool IsInitialized => LocalizationSettings.InitializationOperation.IsDone;

    public event UnityAction<int> OnLanguageChanged;
    public int GetSpecifiedLanguageIndex()
    {
        int idx = 0;
        foreach (var q in LocalizationSettings.AvailableLocales.Locales)
        {
            if (q.Identifier.Code == LocalizationSettings.SelectedLocale.Identifier.Code)
            {
                return idx;
            }
            idx++;
        }
        return 0;
    }

    public async UniTask Initialize()
    {
        await LocalizationSettings.InitializationOperation;
    }
}