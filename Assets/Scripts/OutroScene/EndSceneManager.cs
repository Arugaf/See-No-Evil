using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
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
    public class EndSceneManager : MonoBehaviour
    {
        public EndSceneManagerBehaviour[] Managers;
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
            EnsureTransition(toGameplay);
        }
        public bool EnsureTransition(bool toGameplay)
        {
            if (canQuit)
            {
                TransitionProcess(toGameplay).Forget();
                canQuit = false;
                return true;
            }
            return false;
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