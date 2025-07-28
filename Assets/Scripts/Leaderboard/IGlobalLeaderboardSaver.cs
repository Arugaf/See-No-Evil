
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using VContainer.Unity;

namespace Leaderboard
{
    public interface IGlobalLeaderboardScoreSaver
    {
        public SaveManager.GameSaveData UpdateGlobalScore(SaveManager.GameSaveData  data);
    }
    public class GlobalLeaderboardScoreSaver : IGlobalLeaderboardScoreSaver, IAsyncStartable
    {
        private LootRegistryScriptableObject lootRegistry;
        private ILeaderboardManager leaderboardMaster;
        private int scoreInTable;
        public GlobalLeaderboardScoreSaver(ILeaderboardManager master, LootRegistryScriptableObject obj)
        {
            leaderboardMaster = master;
            lootRegistry = obj;
        }
        public SaveManager.GameSaveData UpdateGlobalScore(SaveManager.GameSaveData data)
        {
            int score = CalculateScore(data);
            if ((data.Leaderboard?.GlobalScore ?? 0) < score)
            {
                data.Leaderboard = new SaveManager.GameSaveData.LeaderboardData() { GlobalScore = score };
            }
            if (scoreInTable < score && leaderboardMaster.IsAvailable)
            {
                leaderboardMaster.GetLeaderboard(ILeaderboardManager.LEADERBOARD_GLOBAL).SetScore(score);
                scoreInTable = score;
            }
            return data;
        }
        private int CalculateScore(SaveManager.GameSaveData data)
        {
            int score = 0;
            foreach (var x in data.LevelStats.Values)
            {
                score += x.BestScore;
            }
            foreach (var x in data.Loot.Values)
            {
                score += lootRegistry.Get(x.Id)?.ScoreToGrant ?? 0;
            }
            return score;
        }

        public async Awaitable StartAsync(CancellationToken cancellation = default)
        {
            if (leaderboardMaster.IsAvailable)
            {
                var leaderboardManager = leaderboardMaster.GetLeaderboard(ILeaderboardManager.LEADERBOARD_GLOBAL);
                if (leaderboardManager != null)
                {
                    scoreInTable = await leaderboardManager.TryGetScore();
                }
            }
        }
    }
}