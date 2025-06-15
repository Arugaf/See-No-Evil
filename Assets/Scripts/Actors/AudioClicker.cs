using InputModule;
using UnityEngine;

namespace Actors {
    public class AudioClicker : MonoBehaviour {
        [SerializeField] private RandomAudioSource[] randomAudioSource;

        private void Start() {
            InputHandlerOld.GotPrimaryMouseButtonDown += OnAudioClick;
        }

        private void OnAudioClick() {
            var sound = Random.Range(0, randomAudioSource.Length);
            randomAudioSource[sound].PlayRandomSound();
        }
    }
}
