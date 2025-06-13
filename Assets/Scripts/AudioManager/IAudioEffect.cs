using UnityEngine;

namespace Features.AudioManager
{
    public interface IAudioEffect
    {
        public void Play(in AudioPlayDeterminedParams playParams);
        public void Delete();
        public void SetActive(bool active);
        public void SetPosition(Vector3 position);
    }
}
