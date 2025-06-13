using UnityEngine;
using UnityEngine.Events;

namespace Actors {
    public enum Status {
        Alive,
        Dead,
    }

    public class Health : MonoBehaviour {
        public uint initialHealth = 100;
        public uint maxHealth = 100;
        public int health;

        [SerializeField] private Status status = Status.Alive;

        public UnityEvent gotHealthIsZero;

        private void Start() {
            health = (int)initialHealth;
        }

        private void Update() {
            // todo: remove in release version
            if (status == Status.Dead) {
                gotHealthIsZero?.Invoke();
            }
            
            ApplyHpChange(0);
        }

        public void ApplyHpChange(int hpChange) {
            health -= hpChange;
            status = health <= 0 ? Status.Dead : Status.Alive;
            
            if (status == Status.Dead) {
                gotHealthIsZero?.Invoke();
            }
        }
    }
}
