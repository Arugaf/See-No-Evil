using Cysharp.Threading.Tasks;
using UnityEngine;
using YG;
using YG.Utils.LB;
namespace Leaderboard
{
    public class PluginYGCurrentPlayerAdapter : ILeaderboardData {
        private LBCurrentPlayerData currentPlayerData;

        public PluginYGCurrentPlayerAdapter(LBCurrentPlayerData currentPlayer)
        {
            currentPlayerData = currentPlayer;
        }

        public int Place => currentPlayerData.rank;

        public int Score => currentPlayerData.score;

        public string PlayerID => YG2.player.id;

        public string DisplayName => YG2.player.name;

        public async UniTask<Texture2D> FetchProfileImage()
        {
            if (string.IsNullOrEmpty(YG2.player.photo)) return null;
            return await WebRequest.TryFetchImage(YG2.player.photo);
            
        }
    }
}