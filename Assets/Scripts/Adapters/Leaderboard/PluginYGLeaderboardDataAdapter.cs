using Cysharp.Threading.Tasks;
using UnityEngine;
using YG.Utils.LB;
namespace Leaderboard
{
    public class PluginYGLeaderboardDataAdapter : ILeaderboardData
    {
        private LBPlayerData playerData;
        public PluginYGLeaderboardDataAdapter(LBPlayerData dat) => playerData = dat;

        public int Place => playerData.rank;

        public int Score => playerData.score;
        public string PlayerID => playerData.uniqueID;

        public string DisplayName => playerData.name;

        public async UniTask<Texture2D> FetchProfileImage()
        {
            if (string.IsNullOrEmpty(playerData.photo)) return null;
            return await WebRequest.TryFetchImage(playerData.photo);
        }
    }
}