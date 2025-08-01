using Cysharp.Threading.Tasks;
using Leaderboard;
using UnityEngine;
using VContainer.Unity;
using YG;

namespace Auth
{
    public class PluginYGAuthManager : IAuthManager, IStartable
    {
        public bool IsAvailable => true;

        public bool IsAuthenticated => YG2.player.auth;

        public string AuthUserID => YG2.player.id;

        public string AuthUserName => YG2.player.name;

        public UniTask<Texture2D> FetchUserProfilePicture()
        {
            return WebRequest.TryFetchImage(YG2.player.photo);
        }

        public UniTask ShowAuthScreen()
        {
            YG2.OpenAuthDialog();
            return UniTask.CompletedTask;
        }

        public void Start()
        {
            YG2.GetAuth();
        }
    }
}