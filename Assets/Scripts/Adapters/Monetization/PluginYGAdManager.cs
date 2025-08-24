using Cysharp.Threading.Tasks;
using UnityEngine;
using YG;
namespace Monetization
{
    public class PluginYGAdManager : IAdManager
    {
        public bool RewardedAdsAvailable => true;
        private const string ID = "idk";
        public PluginYGAdManager()
        {

        }
        
        public void Dispose()
        {
            
        }
        public UniTask PreloadAdvertisement()
        {
            return UniTask.CompletedTask;
        }

        public UniTask<AdShowResult> ShowAdvertisement()
        {
            if (YG2.isTimerAdvCompleted)
            {
                UniTaskCompletionSource<AdShowResult> result = new UniTaskCompletionSource<AdShowResult>();
                YG2.onCloseInterAdvWasShow = (bool x) =>
                {
                    AdShowResult ret = new AdShowResult(x ? AdShowResult.Status.Success : AdShowResult.Status.Failure);
                    result.TrySetResult(ret);
                    YG2.onCloseInterAdvWasShow = null;
                };
                YG2.InterstitialAdvShow();
                return result.Task;
            }
            else
            {
                Debug.LogWarning("Could not push the ad very often; failing.");
                return UniTask.FromResult(new AdShowResult(AdShowResult.Status.Failure));
            }
            
        }

        public UniTask<AdShowResult> ShowRewardedAdverticement()
        {
            UniTaskCompletionSource<AdShowResult> result = new UniTaskCompletionSource<AdShowResult>();
            // Do I mention how i HATE this API?
            YG2.onErrorRewardedAdv = () =>
            {
                AdShowResult ret = new AdShowResult(AdShowResult.Status.Failure);
                result.TrySetResult(ret);
                YG2.onErrorRewardedAdv = null;
                YG2.onRewardAdv = null;
            };
            YG2.onRewardAdv = (string id) =>
            {
                AdShowResult ret = new AdShowResult(AdShowResult.Status.Success);
                result.TrySetResult(ret);
                YG2.onRewardAdv = null;
                YG2.onErrorRewardedAdv = null;
            };
            YG2.RewardedAdvShow(ID);
            return result.Task;
        }
    }
}