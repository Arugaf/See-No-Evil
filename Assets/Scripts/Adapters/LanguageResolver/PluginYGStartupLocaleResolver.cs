using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using YG;

public class PluginYGStartupLocaleResolver : IStartupLocaleSelector
{
    public Locale GetStartupLocale(ILocalesProvider availableLocales)
    {
#if PLUGIN_YG_2
        foreach (var q in LocalizationSettings.AvailableLocales.Locales)
        {
            if (q.Identifier.Code == YG2.lang)
            {
                return q;
            }
        }
        return availableLocales.GetLocale("en");
#endif
        return null;
    }
}
