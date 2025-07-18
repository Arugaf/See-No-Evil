using Cysharp.Threading.Tasks;
using KinematicCharacterController.Examples;
using SaveManager;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;
using VContainer;
using VContainer.Unity;
public class SettingsManager: ISettingsManager, IAsyncStartable
{
    public ISettingSaveManager saveManager;
    private VolumeSliderController musicVolume;
    private VolumeSliderController sfxVolume;
    private LocaleController localeController;
    private ILanguageResolver languageResolver;
    private bool showTutorial;
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
    public bool ShowTutorial { get => showTutorial; set { showTutorial = value; Sync(); } }

    private bool noSync = false;
    private void Sync()
    {
        if (noSync) return;
        saveManager.SetValue(new GameSaveData.SettingsData()
        {
            CameraSensivity = CameraSensivity,
            SFXVolume = SFXVolume,
            MusicVolume = MusicVolume,
            ShowTutorial = showTutorial
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
        showTutorial = settingsData.ShowTutorial;
        noSync = false;
        if (!languageResolver.IsInitialized) await languageResolver.Initialize();
        localeController.SetIndex(languageResolver.GetSpecifiedLanguageIndex());
    }
}
