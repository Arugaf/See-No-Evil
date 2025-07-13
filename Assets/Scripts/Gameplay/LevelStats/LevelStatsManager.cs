using Cysharp.Threading.Tasks;
using SaveManager;
using UnityEngine;
using VContainer.Unity;
namespace Gameplay.LevelStats
{
    public interface ILevelStatsManager
    {
        public struct LevelStats
        {
            public int Score;
            public float Time;
        }
        /// <returns>True if it was a highscore and is actually saved</returns>
        bool SubmitResult(GameLevelInfoObject level, int score, float time);
        bool TryGetData(string levelID, out LevelStats stats);
        bool HaveDataFor(GameLevelInfoObject level);
        string LastPlayedLevelID { get; set; }
        
    }
    public static class LevelStatsManagerExtensions
    {
        public static bool SubmitResult(this ILevelStatsManager manager, int score, GameplayResultStorage storage)
        {
            return manager.SubmitResult(storage.gameLevelInfo, score, storage.LastGameTime);
        }
    }
    public class LevelStatsManager : ILevelStatsManager, IStartable
    {
        public ILevelStatsSaveManager SaveManager;
        private GameSaveData.LevelStatsData levelStatsData;
        public LevelStatsManager(ILevelStatsSaveManager saveManager)
        {
            SaveManager = saveManager;
        }

        public bool HaveDataFor(GameLevelInfoObject level)
        {
            if (levelStatsData.TryGetValue(level.ID, out _)) return true;
            return false;
        }

        public string LastPlayedLevelID
        {
            get => levelStatsData.LastPlayedLevelID;
            set {
                if (levelStatsData.LastPlayedLevelID != value)
                {
                    levelStatsData.LastPlayedLevelID = value;
                    SaveManager.SetValue(levelStatsData);
                }
            }
        }
        public void Start()
        {
            levelStatsData = SaveManager.GetValue();
        }

        public bool SubmitResult(GameLevelInfoObject level, int score, float time)
        {
            bool changed = false;
            if (levelStatsData.TryGetValue(level.ID, out var result))
            {
                if(result.BestScore < score)
                {
                    result.BestScore = score;
                    changed = true;
                }
                if (result.BestTime > time)
                {
                    result.BestTime = time;
                    changed = true;
                }
                if (changed)
                {
                    levelStatsData.SetValue(level.ID, result);
                }
                
            }
            else
            {
                changed = true;
                levelStatsData.SetValue(level.ID, new GameSaveData.LevelStatsData.LevelCompletion(time, score));
            }
            if (changed)
            {
                SaveManager.SetValue(levelStatsData);
            }
            return changed;
        }

        public bool TryGetData(string levelID, out ILevelStatsManager.LevelStats stats)
        {
            if (levelStatsData.TryGetValue(levelID, out var x))
            {
                stats.Score = x.BestScore;
                stats.Time = x.BestTime;
                return true;
            }
            stats = default;
            return false;
        }
    }
}