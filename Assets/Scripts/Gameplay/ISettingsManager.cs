using System.Collections.Generic;

public interface ISettingsManager
{
    public float SFXVolume { get; set; }
    public float MusicVolume { get; set; }
    public float CameraSensivity { get; set; }
    public int CurrentLanguageIndex { get; set; }
    public IEnumerable<ILocaleInfo> GetLocales();
}
