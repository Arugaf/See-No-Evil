using Cysharp.Threading.Tasks;
using KinematicCharacterController.Examples;
using SaveManager;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using VContainer;
using VContainer.Unity;
public interface ILocaleInfo
{
    public string Name { get; }
    public string Identifier { get; }
}
public struct LocaleInfo : ILocaleInfo
{
    public readonly string Name { get; }

    public readonly string Identifier { get; }
    public LocaleInfo(string name, string identifier)
    {
        Name = name;
        Identifier = identifier;
    }
    public LocaleInfo(Locale locale)
    {
        Name = locale.Identifier.Code switch
        {
            "ru" => "Русский",
            "en" => "English",
            _ => locale.Identifier.CultureInfo.Name
        };
        Identifier = locale.Identifier.Code;
    }
}
public class LocaleController
{
    public int CurrentLanguageIndex { get => currIndex; set => SetIndex(value); }
    private int currIndex;
    private bool awaiting = false;
    public void SetIndex(int idx)
    {
        if (!awaiting)
        {
            if (!LocalizationSettings.InitializationOperation.IsDone)
            {
                currIndex = idx;
                awaiting = true;
                LocalizationSettings.InitializationOperation.Completed += InitializationOperation_Completed;
            }
            else
            {
                currIndex = idx < 0 ? 0 : idx % LocalizationSettings.AvailableLocales.Locales.Count;
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[currIndex];
            }
        }
    }

    private void InitializationOperation_Completed(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<LocalizationSettings> obj)
    {
        currIndex = currIndex < 0 ? 0 : currIndex % LocalizationSettings.AvailableLocales.Locales.Count;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[currIndex];
        awaiting = false;
    }

    public IEnumerable<ILocaleInfo> GetLocales()
    {
        foreach(var loc in LocalizationSettings.AvailableLocales.Locales)
        {
            yield return new LocaleInfo(loc);
        }
    }
    
}
public class SettingsManager: ISettingsManager, IAsyncStartable
{
    public ISettingSaveManager saveManager;
    private VolumeSliderController musicVolume;
    private VolumeSliderController sfxVolume;
    private LocaleController localeController;
    private ILanguageResolver languageResolver;
    [Inject]
    public SettingsManager(ISettingSaveManager saveManager, ILanguageResolver languageResolver, AudioMixer mainAudioMixer)
    {
        this.saveManager = saveManager;
        musicVolume = new VolumeSliderController(mainAudioMixer, "Volume_Music");
        sfxVolume = new VolumeSliderController(mainAudioMixer, "Volume_SFX");
        localeController = new LocaleController();
        this.languageResolver = languageResolver;
        languageResolver.OnLanguageChanged += (int x) => CurrentLanguageIndex = x;
    }

    public float SFXVolume { get => sfxVolume.Volume; set { sfxVolume.Volume = value; Sync(); } }
    public float MusicVolume { get => musicVolume.Volume; set { musicVolume.Volume = value; Sync(); } }
    public float CameraSensivity { get => ExamplePlayer.PlayerCameraSensivityCoeff; set { ExamplePlayer.PlayerCameraSensivityCoeff = value; Sync(); } }

    public int CurrentLanguageIndex { get => localeController.CurrentLanguageIndex; set => localeController.CurrentLanguageIndex = value; }

    private bool noSync = false;
    private void Sync()
    {
        if (noSync) return;
        saveManager.SetValue(new GameSaveData.SettingsData()
        {
            CameraSensivity = CameraSensivity,
            SFXVolume = SFXVolume,
            MusicVolume = MusicVolume
        });
    }
    public IEnumerable<ILocaleInfo> GetLocales() => localeController.GetLocales();

    public async Awaitable StartAsync(CancellationToken cancellation = default)
    {
        var settingsData = saveManager.GetValue();
        noSync = true;
        CameraSensivity = settingsData.CameraSensivity;
        SFXVolume = settingsData.SFXVolume;
        MusicVolume = settingsData.MusicVolume;
        noSync = false;
        if (!languageResolver.IsInitialized) await languageResolver.Initialize();
        localeController.SetIndex(languageResolver.GetSpecifiedLanguageIndex());
    }
}
