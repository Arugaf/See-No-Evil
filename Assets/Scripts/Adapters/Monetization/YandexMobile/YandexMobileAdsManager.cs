using Cysharp.Threading.Tasks;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using UnityEditor;
using YandexMobileAds;
namespace Monetization
{
    // I think that this exact class is the definition of bug-worthy code
    // Please do not use it 
    public class OneThreadedFunc<T>
    {
        private UniTaskCompletionSource<T> task;
        protected virtual T Default => default;
        protected double FailTime = 5.0;
        protected bool Locked => task != null;
        protected UniTask<T> Run()
        {
            if (task != null)
            {
                UnityEngine.Debug.LogError("You can't do simutaneous work. The solution isnt designed for that");
                return UniTask.FromResult(Default);
            }
            else
            {
                task = new UniTaskCompletionSource<T>();
                return RunAsync();
            }
        }
        private async UniTask<T> RunAsync()
        {
            UnityEngine.Debug.Log("FUNC RUNNING");
            var (isTimeout, t) = await task.Task.TimeoutWithoutException(TimeSpan.FromSeconds(FailTime), DelayType.UnscaledDeltaTime);
            task = null;
            if (isTimeout)
            {   
                return Default;
            } 
            else
            {
                return t;
            }
        }
        protected void Return(T result)
        {
            if(task != null)
            {
                UnityEngine.Debug.Log($"RETURN RESULT: {result}");
                task.TrySetResult(result);
            }
        }
    }
    [Serializable]
    public class MobileAdsManagerSettings
    {
        public string InterstitialAdKey;
        public string RewardedAdKey;
        public double AdShowTimeout = 5.0;
        public double PreloadPromiseTime = 1.0;
    }
    public class YandexMobileAdsManager : IAdManager
    {
        public bool RewardedAdsAvailable => true;
        private YandexInterstitialLoader interstitialLoader;
        private YandexRewardedLoader rewardedLoader;

        public YandexMobileAdsManager(MobileAdsManagerSettings s)
        {
            interstitialLoader = new YandexInterstitialLoader(s.InterstitialAdKey, s.AdShowTimeout, s.PreloadPromiseTime);
            rewardedLoader = new YandexRewardedLoader(s.RewardedAdKey, s.AdShowTimeout, s.PreloadPromiseTime);
        }

        public void Dispose()
        {
            interstitialLoader.Dispose();
            rewardedLoader.Dispose();
        }

        public async UniTask PreloadAdvertisement()
        {
            await UniTask.WhenAll(interstitialLoader.Preload(), rewardedLoader.Preload());
        }

        public async UniTask<AdShowResult> ShowAdvertisement()
        {
            return await interstitialLoader.Show();
        }

        public async UniTask<AdShowResult> ShowRewardedAdverticement()
        {
            return await rewardedLoader.Show();
        }
    }
}