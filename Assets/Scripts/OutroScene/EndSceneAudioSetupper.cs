using Cysharp.Threading.Tasks;
using Features.OutroScene;
using Gameplay;
using UnityEngine;
using VContainer;
namespace Features.OutroScene
{
    public class EndSceneAudioSetupper : EndSceneManagerBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip failureClip;
        [SerializeField] private AudioClip successClip;
        [SerializeField] private AudioClip transitionFogClip;
        [SerializeField] private AudioClip transitionDarknessClip;
        private GameplayResultStorage resStorage;
        [Inject]
        private void Construct(GameplayResultStorage storage)
        {
            resStorage = storage;
        }
        public override UniTask DoProcess()
        {
            if (resStorage.LastGameState == GameplayResultStorage.Result.Victory)
            {
                audioSource.PlayOneShot(successClip);
            }
            else
            {
                audioSource.PlayOneShot(failureClip);
            }
            return UniTask.CompletedTask;
        }
        public override UniTask Init()
        {
            return UniTask.CompletedTask;
        }
        public override UniTask TransitionProcess(bool toGameplay)
        {
            if (toGameplay)
            {
                audioSource.PlayOneShot(transitionDarknessClip);
            }
            else
            {
                audioSource.PlayOneShot(transitionFogClip);
            }
            return UniTask.CompletedTask;
        }
    }
}