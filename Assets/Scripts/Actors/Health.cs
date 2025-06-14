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
            
            DoDamage(0);
        }

        public void DoDamage(int hpChange) {
            if (status == Status.Dead) return;
            health -= hpChange;
            ConstraintHP();
        }
        public void AddHealth(int hpChange)
        {
            health += hpChange;
            if (health > maxHealth) health = (int)maxHealth;
            ConstraintHP();
        }
        void ConstraintHP()
        {

            status = health <= 0 ? Status.Dead : Status.Alive;

            if (status == Status.Dead)
            {
                gotHealthIsZero?.Invoke();
            }
        }
    }
}
