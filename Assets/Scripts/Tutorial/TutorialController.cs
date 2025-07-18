using System;
using System.Collections;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using VContainer;
namespace Tutorial
{
    public abstract class BaseTutorialController : MonoBehaviour
    {
        [SerializeField] private BaseTutorialView baseTutorialView;
        private ISettingsManager manager;
        [Inject]
        private void Construct(ISettingsManager manager)
        {
            this.manager = manager;
        }
        public void Start()
        {
            if (manager.ShowTutorial)
            {
                baseTutorialView.gameObject.SetActive(true);
                ShowTutorial().AttachExternalCancellation(destroyCancellationToken).Forget();
            }
            else baseTutorialView.gameObject.SetActive(false);
        }
        private async UniTask ShowTutorial()
        {
            await baseTutorialView.Show();
            await GetTutorialSection().Perform(baseTutorialView);
            manager.ShowTutorial = false;
            await baseTutorialView.Hide();
            baseTutorialView.gameObject.SetActive(false);
        }
        protected abstract ITutorialSection GetTutorialSection();
    }
}