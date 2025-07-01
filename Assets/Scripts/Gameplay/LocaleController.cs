using System.Collections.Generic;
using UnityEngine.Localization.Settings;
using UnityEngine;
public class LocaleController
{
    public int CurrentLanguageIndex { get => currIndex; set => SetIndex(value); }
    private int currIndex;

    public void SetIndex(int idx)
    {
        if (idx >= 0)
        {
            Debug.Log($"CHANGED LANGUAGE IDX: {idx}");
            currIndex = idx % LocalizationSettings.AvailableLocales.Locales.Count;
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[currIndex];
        }
    }

    public IEnumerable<ILocaleInfo> GetLocales()
    {
        foreach(var loc in LocalizationSettings.AvailableLocales.Locales)
        {
            yield return new LocaleInfo(loc);
        }
    }
    
}
