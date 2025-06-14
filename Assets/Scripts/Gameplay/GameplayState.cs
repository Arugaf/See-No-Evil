using UnityEngine;
using UnityEngine.Events;

namespace Gameplay {
    public class GameplayState : MonoBehaviour {
        public enum Result
        {
            Victory,
            Killed,
            FailureByTime
        }
        public static Result LastInitState { get; private set; }
        public float initialTime = 121f;
        [SerializeField] private float timeRemaining;
        [SerializeField] private bool activeTimer = true;

        public int minutes {
            get => Mathf.FloorToInt(timeRemaining / 60);
        }

        public int seconds {
            get => Mathf.FloorToInt(timeRemaining % 60);
        }

        public UnityEvent gotTimerRunOut;

        public UnityEvent gotDeadEvent;

        public UnityEvent gotVictoryEvent;

        private void Start() {
            timeRemaining = initialTime;
        }

        private void Update() {
            if (!activeTimer || !(timeRemaining > 0f)) return;

            timeRemaining -= Time.deltaTime;

            if (!(timeRemaining <= 0f)) return;
            
            Debug.Log("Timer runout triggered");
            LastInitState = Result.FailureByTime;
            gotTimerRunOut?.Invoke();
        }

        public void Victory() {
            Debug.Log("Victory triggered");
            LastInitState = Result.Victory;
            gotVictoryEvent?.Invoke();
        }

        public void Defeat() {
            Debug.Log("Defeat triggered");
            LastInitState = Result.Killed;
            gotDeadEvent?.Invoke();
        }
    }
}
