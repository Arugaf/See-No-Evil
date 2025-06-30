using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization.Settings;
using YG;

public interface ILanguageResolver
{
    public int GetSpecifiedLanguageIndex();
    public bool IsInitialized { get; }
    public UniTask Initialize();
    event UnityAction<int> OnLanguageChanged;
}
public class PluginYGLanguageResolver : ILanguageResolver
{
    private static Action<string> OnLanguageChangedYGBridge;
    private UnityAction<int> onLanguageChangedEvent;

    public bool IsInitialized => LocalizationSettings.InitializationOperation.IsDone;

    public PluginYGLanguageResolver()
    {
        OnLanguageChangedYGBridge += LanguageCorrection;
    }

    event UnityAction<int> ILanguageResolver.OnLanguageChanged
    {
        add
        {
            onLanguageChangedEvent += value;
        }

        remove
        {
            onLanguageChangedEvent -= value;
        }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void MethodSubscribe()
    {
        YG2.onSwitchLang += (string x) => OnLanguageChangedYGBridge.Invoke(x);
    }
    private void LanguageCorrection(string lang)
    {
        onLanguageChangedEvent?.Invoke(GetSpecifiedLanguageIndex());
    }
    public int GetSpecifiedLanguageIndex()
    {
        if (!IsInitialized) return -1;
        int idx = 0;
        foreach(var q in LocalizationSettings.AvailableLocales.Locales)
        {
            if(q.Identifier.Code == YG2.lang)
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