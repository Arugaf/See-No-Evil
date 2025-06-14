using UnityEngine;
using UnityEngine.Events;

namespace Actors {
    public enum Status {
        Alive,
        Dead,
    }

    public class Health : MonoBehaviour {
        public float initialHealth = 100;
        public float maxHealth = 100;
        public float health;

        public Status Status { get; private set; } = Status.Alive;

        public UnityEvent gotHealthIsZero;

        [SerializeField] private float gotDamageDelay = 5f;
        public bool ReceivedDamageRecently { get; private set; } = false;
        private float _gotDamageCooldown = 0f;

        private void Start() {
            health = initialHealth;
        }

        private void Update() {
            // todo: remove in release version
            if (Status == Status.Dead) {
                gotHealthIsZero?.Invoke();
            }

            DoDamage(0);

            if (!ReceivedDamageRecently) return;
            
            _gotDamageCooldown -= Time.deltaTime;

            if (_gotDamageCooldown <= 0f) {
                ReceivedDamageRecently = false;
            }
        }

        public void DoDamage(float hpChange) {
            if (Status == Status.Dead) return;
            health -= hpChange;
            ConstraintHP();

            ReceivedDamageRecently = true;
            _gotDamageCooldown = gotDamageDelay;
        }

        public void AddHealth(float hpChange) {
            health += hpChange;
            if (health > maxHealth) health = maxHealth;
            ConstraintHP();
        }

        void ConstraintHP() {
            Status = health <= 0 ? Status.Dead : Status.Alive;

            if (Status == Status.Dead) {
                gotHealthIsZero?.Invoke();
            }
        }
    }
}
