using System;
using UnityEngine;
using VContainer;
namespace Gameplay
{
    public interface IScoreEvaluator
    {
        public int Evaluate(in GameplayResultStorage resultStorage);
    }
    public class BasicScoreEvaluator: IScoreEvaluator
    {
        [Serializable]
        public struct Settings
        {
            public int TimeSecondsGain;
            public int ArtifactBonus;
            public int AdditionalScoreForHPGain;
            public int HPMultiplier;
        }
        
        private Settings mySettings;
        [Inject]
        public BasicScoreEvaluator(Settings settings) => mySettings = settings;
        public int Evaluate(in GameplayResultStorage resultStorage)
        {
            float spareTime = resultStorage.TotalLevelTime - resultStorage.LastGameTime;
            int totalScore = Mathf.RoundToInt(spareTime) * mySettings.TimeSecondsGain;
            totalScore += resultStorage.AquiredPrize ? mySettings.ArtifactBonus : 0;
            totalScore += resultStorage.LastGameHP * mySettings.HPMultiplier;
            totalScore += resultStorage.AquiredHPBonusCount * mySettings.AdditionalScoreForHPGain;
            return totalScore;
        }


    }
    public interface IGameplayScoreManager
    {
        public void CollectedItemAdded();
    }
    public class GameplayScoreManager: IGameplayScoreManager
    {
        private GameplayResultStorage resultStorage;
        public GameplayScoreManager(GameplayResultStorage resultStorage)
        {
            this.resultStorage = resultStorage;
        }

        public void CollectedItemAdded() => resultStorage.AquiredHPBonusCount++;
    }
}
