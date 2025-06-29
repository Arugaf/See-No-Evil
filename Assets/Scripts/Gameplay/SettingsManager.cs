using Cysharp.Threading.Tasks;
using KinematicCharacterController.Examples;
using SaveManager;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;
using VContainer;
using VContainer.Unity;
public interface ISettingsManager
{
    public float SFXVolume { get; set; }
    public float MusicVolume { get; set; }
    public float CameraSensivity { get; set; }
}
public class VolumeSliderController
{
    private AudioMixer audioMixer;
    private string parameter;
    public float Volume
    {
        get
        {
            if (audioMixer.GetFloat(parameter, out float db))
            {
                return DBToRatio(db);
            }
            return 1.0f;
        }
        set
        {
            float dB = RatioToDB(value);
            audioMixer.SetFloat(parameter, dB);
        }
    }
    public VolumeSliderController(AudioMixer audioMixer, string parameter)
    {
        this.audioMixer = audioMixer;
        this.parameter = parameter;
    }
    private float RatioToDB(float volumeRatio)
    {
        float dB = -144.0f;
        if (volumeRatio > 0)
            dB = 20.0f * Mathf.Log10(volumeRatio);
        return dB;
    }
    private float DBToRatio(float dB)
    {
        float volumeRatio = 0;
        if (volumeRatio > -144.0f)
            volumeRatio = Mathf.Pow(10, dB / 20.0f);
        return volumeRatio;
    }
}
public class SettingsManager: ISettingsManager, IStartable
{
    public ISettingSaveManager saveManager;
    private VolumeSliderController musicVolume;
    private VolumeSliderController sfxVolume;
    [Inject]
    public SettingsManager(ISettingSaveManager saveManager, AudioMixer mainAudioMixer)
    {
        this.saveManager = saveManager;
        musicVolume = new VolumeSliderController(mainAudioMixer, "Volume_Music");
        sfxVolume = new VolumeSliderController(mainAudioMixer, "Volume_SFX");
    }

    public float SFXVolume { get => sfxVolume.Volume; set { sfxVolume.Volume = value; Sync(); } }
    public float MusicVolume { get => musicVolume.Volume; set { musicVolume.Volume = value; Sync(); } }
    public float CameraSensivity { get => ExamplePlayer.PlayerCameraSensivityCoeff; set { ExamplePlayer.PlayerCameraSensivityCoeff = value; Sync(); } }
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

    public void Start()
    {
        var settingsData = saveManager.GetValue();
        noSync = true;
        CameraSensivity = settingsData.CameraSensivity;
        SFXVolume = settingsData.SFXVolume;
        MusicVolume = settingsData.MusicVolume;
        noSync = false;
    }
}
