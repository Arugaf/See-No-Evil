using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Gameplay {
    public class Timer : MonoBehaviour {
        private GameplayState gameplayState;

        private TextMeshProUGUI _text;
        [SerializeField] private Slider slider;
        private void Start() {
            _text = GetComponent<TextMeshProUGUI>();
        }
        [Inject]
        private void Construct(GameplayState gameplayState)
        {
            this.gameplayState = gameplayState;
        }

        private void Update() {
            UpdateTime();
            slider.value = (gameplayState.TotalSeconds / gameplayState.InitialTime);
        }

        private void UpdateTime() {
            _text.text = GameplayResultStorage.GetTimeSpec(gameplayState.TotalSeconds);
        }

    }
}
