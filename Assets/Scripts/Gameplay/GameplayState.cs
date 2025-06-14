using Features.VFX;
using System.Collections;
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
        public static Result LastGameState { get; private set; }
        public float initialTime = 121f;
        [SerializeField] private float timeRemaining;
        [SerializeField] private bool activeTimer = true;
        private bool isTransitioning = false;
        public int minutes {
            get => Mathf.FloorToInt(timeRemaining / 60);
        }

        public int seconds {
            get => Mathf.FloorToInt(timeRemaining % 60);
        }
        public float TotalSeconds => timeRemaining;

        [SerializeField] private Animator gameplayUIAnimator;
        [SerializeField] private float transitionDuration;


        private void Start() {
            timeRemaining = initialTime;
        }

        private void Update() {
            if (!activeTimer || !(timeRemaining > 0f)) return;

            timeRemaining -= Time.deltaTime;

            if (!(timeRemaining <= 0f)) return;
            if (isTransitioning) return;
            Debug.Log("Timer runout triggered");
            LastGameState = Result.FailureByTime;
            TriggerTransition();
        }

        public void Victory() {
            if (isTransitioning) return;
            Debug.Log("Victory triggered");
            LastGameState = Result.Victory;
            TriggerTransition();
        }

        public void Defeat() {
            if (isTransitioning) return;
            Debug.Log("Defeat triggered");
            LastGameState = Result.Killed;
            TriggerTransition();
        }
        public void TriggerTransition()
        {
            StartCoroutine(TransitionCoroutine());
        }
        private IEnumerator TransitionCoroutine()
        {
            isTransitioning = true;
            gameplayUIAnimator.SetBool("Hide", true);
            yield return new WaitForSeconds(transitionDuration);
            GameStateManager.LoadGameOver();
        }
    }
}
