using UnityEngine;

namespace Features.AudioManager
{
    public interface IAudioMaterialHolder
    {
        public AudioPlayDeterminedParams RetrieveAt(Vector3 point);
    }
    public class AudioMaterialHolder : MonoBehaviour, IAudioMaterialHolder
    {
        [SerializeField] private AudioStepMaterial stepMaterial;

        public AudioPlayDeterminedParams RetrieveAt(Vector3 point) => stepMaterial.Generate();
    }
}
