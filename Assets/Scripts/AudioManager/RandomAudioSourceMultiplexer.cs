using UnityEngine;
namespace Features.AudioManager
{
    public class RandomAudioSourceMultiplexer : MonoBehaviour
    {
        [SerializeField] private RandomAudioSource[] randomAudioSources;
        private void PlayOn(int idx)
        {
            if (randomAudioSources.Length <= idx || idx < 0)
            {
                Debug.LogError($"You should set up the multiplexer for index {idx}", gameObject);
                return;
            }
            randomAudioSources[idx].PlayRandomSound();
        }
    }
}