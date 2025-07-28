using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Auth
{
    public interface IAuthManager
    {
        public bool IsAvailable { get; }
        public UniTask ShowAuthScreen();
        public bool IsAuthenticated { get; }
        public string AuthUserID { get; }
        public string AuthUserName { get; }
        public UniTask<Texture2D> FetchUserProfilePicture();
    }
}