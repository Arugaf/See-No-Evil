using Cysharp.Threading.Tasks;
namespace Monetization
{
    public class DummyAdManager : IAdManager
    {
        public void Dispose()
        {
            
        }

        public UniTask PreloadAdvertisement()
        {
            return UniTask.CompletedTask;
        }

        public UniTask<AdShowResult> ShowAdvertisement()
        {
            return UniTask.FromResult<AdShowResult>(AdShowResult.Status.Failure);
        }

        public UniTask<AdShowResult> ShowRewardedAdverticement()
        {
            return UniTask.FromResult<AdShowResult>(AdShowResult.Status.Failure);
        }
    }
}