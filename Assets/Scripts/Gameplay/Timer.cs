using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay {
    public class Timer : MonoBehaviour {
        [SerializeField] private GameplayState gameplayState;

        private TextMeshProUGUI _text;
        [SerializeField] private Slider slider;
        private void Start() {
            _text = GetComponent<TextMeshProUGUI>();
        }

        private void Update() {
            UpdateTime();
            slider.value = 1.0f - (gameplayState.TotalSeconds / gameplayState.initialTime);
        }

        private void UpdateTime() {
            var minutes = gameplayState.minutes;
            var seconds = gameplayState.seconds;

            _text.text = (minutes > 0
                ? minutes.ToString()
                : 0) + ":" + (seconds > 0 ? (seconds / 10 != 0 ? seconds : "0" + seconds) : "00");
        }

    }
}
