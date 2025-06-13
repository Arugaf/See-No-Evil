using UnityEngine;
namespace Features.AudioManager
{
    public interface IAudioCallbackReciever
    {
        public void AudioPlays(AudioPlayDeterminedParams param, Vector3 position);
    }
}