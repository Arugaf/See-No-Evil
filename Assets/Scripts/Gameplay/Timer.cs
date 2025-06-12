using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay {
    public class Timer : MonoBehaviour {
        [SerializeField] private GameplayState gameplayState;

        private TextMeshProUGUI _text;

        private void Start() {
            _text = GetComponent<TextMeshProUGUI>();
        }

        private void Update() {
            UpdateTime();
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
