using System.Collections;
using Features.AudioManager;
using Unity.Cinemachine;
using UnityEngine;

namespace Actors {
    public class PatrollerAudioManager : MonoBehaviour
    {
        [SerializeField] private RandomAudioSource ambientAudio;
        [SerializeField] private RandomAudioSource aggroAudio;
        [SerializeField, MinMaxRangeSlider(1.0f, 20.0f)] private Vector2 ambientDuration;
        [SerializeField] private float aggroDuration = 3.0f;
        private Coroutine waitCoroutine;
        private IEnumerator Start()
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(ambientDuration.x, ambientDuration.y));
                ambientAudio.PlayRandomSound();
            }
        }
        public void OnAggro()
        {
            if (waitCoroutine != null) return;
            aggroAudio.PlayRandomSound();
            waitCoroutine = StartCoroutine(WaitCor());
        }
        private IEnumerator WaitCor()
        {
            yield return new WaitForSeconds(aggroDuration);
            waitCoroutine = null;
        }
    }
}
