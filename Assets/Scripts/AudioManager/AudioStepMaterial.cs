using UnityEngine;

namespace Features.AudioManager
{
    [CreateAssetMenu(fileName = "AudioStepMaterial", menuName = "Scriptable Objects/AudioStepMaterial")]
    public class AudioStepMaterial : ScriptableObject, IAudioPlayParamGenerator
    {
        [SerializeField] private AudioPlayRandomParameters Parameters;

        public AudioPlayDeterminedParams Generate() => Parameters.Generate();

    }
}
