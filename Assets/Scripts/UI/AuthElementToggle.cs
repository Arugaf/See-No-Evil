using Auth;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace UI
{
    public class AuthElementToggle : MonoBehaviour
    {
        private IAuthManager authManager;
        [SerializeField] private GameObject onNoAuth;
        [SerializeField] private GameObject onAuth;
        [Inject]
        private void Construct(IAuthManager mng)
        {
            authManager = mng;
        }
        public void OnEnable()
        {
            RefreshView();
        }
        public void ShowAuthScreen()
        {
            authManager.ShowAuthScreen().AttachExternalCancellation(destroyCancellationToken)
            .ContinueWith(RefreshView).Forget();
        }
        public void RefreshView()
        {
            if (authManager.IsAvailable)
            {
                if (authManager.IsAuthenticated)
                {
                    onNoAuth.SetActive(false);
                    onAuth.SetActive(true);
                }
                else
                {
                    onNoAuth.SetActive(true);
                    onAuth.SetActive(false);
                }
            }
            else
            {
                onNoAuth.SetActive(false);
                onAuth.SetActive(false);
            }
        }

    }
}