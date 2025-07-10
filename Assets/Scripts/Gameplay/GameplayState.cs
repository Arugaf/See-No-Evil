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
    public class GameplayResultStorage
    {
        public enum Result
        {
            Victory,
            Killed,
            FailureByTime
        }
        public GameLevelInfoObject gameLevelInfo { get; private set; }
        public Result LastGameState { get; set; }
        public float LastGameTime { get; set; }
        public int LastGameHP { get; set; }
        public bool AquiredPrize { get; set; }
        public int AquiredHPBonusCount { get; set; }
        public float TotalLevelTime { get; set; }
        public void SetLevel(GameLevelInfoObject gameLevelInfoObject)
        {
            gameLevelInfo = gameLevelInfoObject;
            LastGameState = Result.Victory;
            LastGameTime = 0;
            TotalLevelTime = 0;
            LastGameHP = 0;
            AquiredHPBonusCount = 0;
            AquiredPrize = false;
        }
        public void FailByTime()
        {
            LastGameState = Result.FailureByTime;
            LastGameTime = 0;
            LastGameHP = 0;
        }
        public void Defeat()
        {
            LastGameState = Result.Killed;
            LastGameTime = 0;
            LastGameHP = 0;
        }
        public void Victory(float timeRemaining = 0)
        {
            LastGameState = Result.Victory;
            LastGameTime = timeRemaining;
        }
        public static string GetTimeSpec(float timeRemaining)
        {
            if (timeRemaining < 0) return "00:00";
            int minutes = Mathf.FloorToInt(timeRemaining) / 60;
            int seconds = Mathf.FloorToInt(timeRemaining) % 60;
            return $"{minutes:D2}:{seconds:D2}";
        }
    }
    public class GameplayState : ITickable
    {
        [Serializable]
        public struct Settings
        {
            public float InitialTime;
            public float TransitionDuration;
        }
        public event UnityAction OnGameOver;
        private GameplayResultStorage gameplayResultStorage;
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


        public GameplayState(Settings settings, GameplayResultStorage resultStorage)
        {
            this.settings = settings;
            timeRemaining = settings.InitialTime;
            gameplayResultStorage = resultStorage;
            resultStorage.TotalLevelTime = settings.InitialTime;
            //LastGameTime = 0;
            //LastGameState = Result.Victory;
        }

        public void Tick() {
            if (!(timeRemaining > 0f)) return;

            timeRemaining -= Time.deltaTime;

            if (!(timeRemaining <= 0f)) return;
            if (isTransitioning) return;
            Debug.Log("Timer runout triggered");
            gameplayResultStorage.FailByTime();
            TriggerTransition();
        }

        public void Victory() {
            if (isTransitioning) return;
            Debug.Log("Victory triggered");
            gameplayResultStorage.Victory(settings.InitialTime - TotalSeconds);
            TriggerTransition();
        }

        public void Defeat() {
            if (isTransitioning) return;
            Debug.Log("Defeat triggered");
            gameplayResultStorage.Defeat();
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
