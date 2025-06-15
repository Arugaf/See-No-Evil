using System.Collections;
using UnityEngine;

namespace Actors {
    [RequireComponent(typeof(Health))]
    public class HealthRegenerator : MonoBehaviour {
        private Health _health;

        [SerializeField] private float regenerationRate = 1f;

        private bool _isRegenerating = false;

        private void Start() {
            _health = GetComponent<Health>();
        }

        private void Update() {
            if (_health.Status == Status.Dead || _health.ReceivedDamageRecently ||
                Mathf.Approximately(_health.health, _health.maxHealth)) {
                StopRegeneration();
            }

            if (_isRegenerating) return;

            StopRegeneration();
            StartCoroutine(RegenerateHealth());
        }

        private IEnumerator RegenerateHealth() {
            _isRegenerating = true;
            while (_health.health < _health.maxHealth) {
                var healAmount = regenerationRate * Time.deltaTime;
                _health.AddHealth(healAmount);
                yield return null;
            }

            _isRegenerating = false;
        }

        private void StopRegeneration() {
            if (!_isRegenerating) return;

            StopCoroutine(RegenerateHealth());
            _isRegenerating = false;
        }
    }
}
