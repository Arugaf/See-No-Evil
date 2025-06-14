using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UI;
namespace UI
{
    [RequireComponent(typeof(Slider))]
    public class VolumeSetting: MonoBehaviour
    {
        [SerializeField] private AudioMixer mixerGroup;
        [SerializeField] private string exposedParameter;
        private Slider slider;
        private void Awake()
        {
            slider = GetComponent<Slider>();
            if(mixerGroup.GetFloat(exposedParameter, out float db))
            {
                slider.value = DBToRatio(db);
            }
            slider.onValueChanged.AddListener(Sync);
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
        private void Sync(float ratio)
        {
            float dB = RatioToDB(ratio);
            mixerGroup.SetFloat(exposedParameter, dB);
        }
    }
}