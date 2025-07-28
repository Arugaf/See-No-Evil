using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
namespace Leaderboard
{
    public static class  WebRequest {
        public static async UniTask<Texture2D> TryFetchImage(string webURL)
        {
            try
            {
                using UnityWebRequest handle = UnityWebRequestTexture.GetTexture(webURL);
                var result = await handle.SendWebRequest();
                return DownloadHandlerTexture.GetContent(handle);
            }
            catch
            {
                return null;
            }
        }
    }
    public interface ILeaderboardManager
    {
        public const string LEADERBOARD_GLOBAL = "global";
        public bool IsAvailable { get; }
        public ILeaderboard GetLeaderboard(string key);
    }
    public interface ILeaderboard
    {
        public void SetScore(int score);
        public UniTask<int> TryGetScore();
        public UniTask<ILeaderboardData> SelfEntry();
        public UniTask<IReadOnlyCollection<ILeaderboardData>> FetchLeaderboard(int topPlaces = 3, int nearbySelfPlaces = 3);
    }
}