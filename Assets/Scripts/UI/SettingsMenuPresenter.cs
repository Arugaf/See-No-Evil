using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VContainer;

public class SettingsMenuPresenter : MonoBehaviour
{
    [SerializeField] private Slider musicVolume;
    [SerializeField] private Slider sfxVolume;
    [SerializeField] private Slider cameraSensivity;
    private ISettingsManager settings;
    [Inject]
    private void Construct(ISettingsManager settingsManager)
    {
        settings = settingsManager;
    }
    private void OnEnable()
    {
        musicVolume.value = settings.MusicVolume;
        sfxVolume.value = settings.SFXVolume;
        cameraSensivity.value = settings.CameraSensivity;
        musicVolume.onValueChanged.AddListener(MusicChanged);
        sfxVolume.onValueChanged.AddListener(SFXChanged);
        cameraSensivity.onValueChanged.AddListener(SensivityChanged);
    }
    private void SFXChanged(float newValue) => settings.SFXVolume = newValue;
    private void MusicChanged(float newValue) => settings.MusicVolume = newValue;
    private void SensivityChanged(float newValue) => settings.CameraSensivity = newValue;
    public void OnDisable()
    {
        musicVolume.onValueChanged.RemoveListener(MusicChanged);
        sfxVolume.onValueChanged.RemoveListener(SFXChanged);
        cameraSensivity.onValueChanged.RemoveListener(SensivityChanged);
    }
}
