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
            slider.value = (gameplayState.TotalSeconds / gameplayState.initialTime);
        }

        private void UpdateTime() {
            _text.text = GameplayState.GetTimeSpec(gameplayState.TotalSeconds);
        }

    }
}
