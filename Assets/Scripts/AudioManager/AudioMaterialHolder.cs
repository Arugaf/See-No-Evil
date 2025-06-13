using UnityEngine;

namespace Features.AudioManager
{
    public interface IAudioMaterialHolder : IAudioPlayParamGenerator { }
    public class AudioMaterialHolder : MonoBehaviour, IAudioMaterialHolder
    {
        [SerializeField] private AudioStepMaterial stepMaterial;

        public AudioPlayDeterminedParams Generate() => stepMaterial.Generate();
    }
}
