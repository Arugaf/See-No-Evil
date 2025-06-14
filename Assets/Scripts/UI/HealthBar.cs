using Actors;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class HpBar : MonoBehaviour {
        private Slider _slider;
        [SerializeField] private TextMeshProUGUI healthValue;
        [SerializeField] private Health health;

        private void Awake() {
            _slider = GetComponent<Slider>();
        }

        public void Update() {
            _slider.maxValue = health.maxHealth;
            _slider.value = health.health;
            healthValue.text = health.health.ToString();
        }
    }
}
