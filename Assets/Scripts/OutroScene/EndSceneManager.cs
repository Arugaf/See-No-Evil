using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Monetization;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;
using VContainer;
namespace Features.OutroScene
{
    public class EndSceneManagerBehaviour : MonoBehaviour
    {
        public virtual UniTask Init() => UniTask.CompletedTask;
        public virtual UniTask DoProcess()=> UniTask.CompletedTask;
        public virtual UniTask TransitionProcess(bool toGameplay)=> UniTask.CompletedTask;
    }
    public class EndSceneAdManager : EndSceneManagerBehaviour
    {
        private IAdManager adManager;
        [Inject]
        private void Construct(IAdManager manager)
        {
            adManager = manager;
        }
        public override UniTask Init()
        {
            return adManager.PreloadAdvertisement();
        }
        public override UniTask TransitionProcess(bool toGameplay)
        {
            return adManager.ShowAdvertisement();
        }
    }
    public class EndSceneManager : MonoBehaviour
    {
        public EndSceneAdManager[] Managers;
        private bool canQuit = false;
        private GameStateManager stateManager;
        [Inject]
        private void Construct(GameStateManager mng)
        {
            stateManager = mng;
        }
        public async void Start()
        {
            await UniTask.WhenAll(from m in Managers select m.Init());
            foreach (var x in Managers)
            {
                await x.DoProcess();
            }
            canQuit = true;
        }
        public void Transition(bool toGameplay)
        {
            if (canQuit)
            {
                TransitionProcess(toGameplay).Forget();
                canQuit = false;
            }
        }
        private async UniTask TransitionProcess(bool toGameplay)
        {
            foreach (var x in Managers)
            {
                await x.TransitionProcess(toGameplay);
            }
            if (!toGameplay)
            {
                stateManager.LoadMenu();
            }
            else
            {
                stateManager.LoadGame();
            }
        }
    }
}