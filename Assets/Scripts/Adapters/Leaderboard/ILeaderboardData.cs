using Cysharp.Threading.Tasks;
using UnityEngine;
namespace Leaderboard
{
    public interface ILeaderboardData {
        public int Place { get; }
        public int Score { get; }
        public string PlayerID { get; }
        public UniTask<Texture2D> FetchProfileImage();
        public string DisplayName { get; }
    }
}