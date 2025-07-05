using Cysharp.Threading.Tasks;
using Features.VFX;
using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using VContainer.Unity;

namespace Gameplay 
{
    public class GameplayState : ITickable
    {
        [Serializable]
        public struct Settings
        {
            public float InitialTime;
            public float TransitionDuration;
        }
        public enum Result
        {
            Victory,
            Killed,
            FailureByTime
        }
        public event UnityAction OnGameOver;
        public static Result LastGameState { get; private set; }
        public static float LastGameTime { get; private set; }
        public int Minutes
        {
            get => Mathf.FloorToInt(timeRemaining / 60);
        }

        public int Seconds
        {
            get => Mathf.FloorToInt(timeRemaining % 60);
        }
        public float TotalSeconds => timeRemaining;
        public float InitialTime => settings.InitialTime;
        private float timeRemaining;
        private Settings settings;
        private bool isTransitioning = false;
        public static string GetTimeSpec(float timeRemaining)
        {
            if (timeRemaining < 0) return "00:00";
            int minutes = Mathf.FloorToInt(timeRemaining) / 60;
            int seconds = Mathf.FloorToInt(timeRemaining) % 60;
            return $"{minutes:D2}:{seconds:D2}";
        }

        public GameplayState(Settings settings)
        {
            this.settings = settings;
            timeRemaining = settings.InitialTime;
            LastGameTime = 0;
            LastGameState = Result.Victory;
        }

        public void Tick() {
            if (!(timeRemaining > 0f)) return;

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
            LastGameTime = settings.InitialTime - TotalSeconds;
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
            isTransitioning = true;
            TransitionTask().Forget();
        }
        private async UniTask TransitionTask()
        {
            isTransitioning = true;
            OnGameOver?.Invoke();
            await UniTask.WaitForSeconds(settings.TransitionDuration);
            GameStateManager.LoadGameOver();
        }
    }
}
