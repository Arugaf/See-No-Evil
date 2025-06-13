using UnityEngine;
using UnityEngine.Audio;
namespace Features.AudioManager
{ 
    public class MixerByDarknessValue: MonoBehaviour
    {
        [SerializeField] private AudioMixer audioMixer;
        private void Update()
        {
            audioMixer.SetFloat("DARKNESS_FACTOR", VFX.DarknessManager.DarknessFactor);
        }
        private void OnDestroy()
        {
            audioMixer.SetFloat("DARKNESS_FACTOR", 0);
        }
    }
}
