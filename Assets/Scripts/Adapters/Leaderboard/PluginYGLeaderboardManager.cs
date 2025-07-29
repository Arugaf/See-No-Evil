using System;
using System.Linq;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using YG;
using YG.Utils.LB;
namespace Leaderboard
{
    public class PluginYGLeaderboardManager : ILeaderboard
    {
        // yeah why not?
        public const string GLOBAL_LEADERBOARD_ID = "totalscore";
        public bool IsAvailable => true;
        private LBData data;
        public async UniTask<IReadOnlyCollection<ILeaderboardData>> FetchLeaderboard(int topPlaces = 3, int nearbySelfPlaces = 3)
        {
            if (data == null)
            {
                data = await FF(topPlaces, nearbySelfPlaces);
            }
            var coll = new List<ILeaderboardData>(from f in data.players select (ILeaderboardData)new PluginYGLeaderboardDataAdapter(f));
            return coll;
        }

        public async UniTask<ILeaderboardData> SelfEntry()
        {
            if (data == null)
            {
                data = await FF(3, 3);
            }
            return new PluginYGCurrentPlayerAdapter(data.currentPlayer);
        }

        public void SetScore(int score)
        {
            YG2.SetLeaderboard(GLOBAL_LEADERBOARD_ID, score);
        }
        private UniTask<LBData> FF(int topPlaces, int nearbySelfPlaces)
        {
            UniTaskCompletionSource<LBData> task = new UniTaskCompletionSource<LBData>();
            YG2.onGetLeaderboard += (LBData dat) =>
            {
                task.TrySetResult(dat);
                YG2.onGetLeaderboard = null;
            };
            YG2.GetLeaderboard(GLOBAL_LEADERBOARD_ID, topPlaces, nearbySelfPlaces);
            return task.Task.Timeout(TimeSpan.FromSeconds(10));

        }

        public async UniTask<int> TryGetScore()
        {
            if (data == null)
            {
                data = await FF(3, 3);
            }
            return data.currentPlayer.score;
        }
    }
    public class PluginYGLeaderboardMaster : ILeaderboardManager
    {
        public bool IsAvailable => true;

        public ILeaderboard GetLeaderboard(string key)
        {
            if (key == ILeaderboardManager.LEADERBOARD_GLOBAL)
            {
                return new PluginYGLeaderboardManager();
            }
            return null;
        }
    }
}