using Cysharp.Threading.Tasks;
using Monetization;
using VContainer;
namespace Features.OutroScene
{
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
}