using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Auth
{
    public class DummyAuthManager : IAuthManager
    {
        public bool IsAvailable => false;

        public bool IsAuthenticated => false;

        public string AuthUserID => string.Empty;

        public string AuthUserName => string.Empty;

        public UniTask<Texture2D> FetchUserProfilePicture()
        {
            return UniTask.FromResult<Texture2D>(null);
        }

        public UniTask ShowAuthScreen()
        {
            return UniTask.CompletedTask;
        }
    }
}