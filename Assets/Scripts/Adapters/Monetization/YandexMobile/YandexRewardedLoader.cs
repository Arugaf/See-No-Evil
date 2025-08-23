using Cysharp.Threading.Tasks;
using System;
using System.Diagnostics;
using UnityEngine;
using YandexMobileAds;
using YandexMobileAds.Base;
namespace Monetization
{
    public class YandexRewardedLoader: OneThreadedFunc<AdShowResult>, IDisposable
    {
        public bool Ready => interstitial != null;
        RewardedAdLoader lrdr;
        RewardedAd interstitial;
        string identifier;
        private double preloadTime;
        private bool toBeReward;

        protected override AdShowResult Default => AdShowResult.Status.Failure;
        public YandexRewardedLoader(string identifier, double failTime, double preloadTime)
        {
            UnityEngine.Debug.Log("TO LOAD");
            lrdr = new RewardedAdLoader();
            lrdr.OnAdLoaded += HandleInterstitialLoaded;
            lrdr.OnAdFailedToLoad += Lrdr_OnAdFailedToLoad;
            this.identifier = identifier;
            this.preloadTime = preloadTime;
            FailTime = failTime * 10;
        }

        private void Lrdr_OnAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
        {
            DestroyInterstitial();
            UnityEngine.Debug.Log("FAILED TO LOAD");
        }

        private void HandleInterstitialLoaded(object sender, RewardedAdLoadedEventArgs args)
        {
            UnityEngine.Debug.Log("REWARDED LOADED");
            interstitial = args.RewardedAd;
            interstitial.OnRewarded += OnReward;
            interstitial.OnAdFailedToShow += HandleInterstitialFailedToShow;
            interstitial.OnAdDismissed += HandleInterstitialDismissed;
            interstitial.OnAdShown += HandleInterstitialShown;
        }

        private void HandleInterstitialDismissed(object sender, EventArgs args)
        {
            DestroyInterstitial();
            RequesRewardedAd();
        }
        private void RequesRewardedAd()
        {
            AdRequestConfiguration adRequestConfiguration = new AdRequestConfiguration.Builder(identifier).Build();
            lrdr.LoadAd(adRequestConfiguration);
        }
        private void DestroyInterstitial()
        {
            if (interstitial != null)
            {
                interstitial.Destroy();
                interstitial = null;
            }
        }
        private void HandleInterstitialShown(object sender, EventArgs args)
        {
            UnityEngine.Debug.Log($"REWARD SHOWN");
            LateReturn().Forget();
        }
        private void OnReward(object sender, EventArgs args)
        {
            toBeReward = true;
        }
        private async UniTask LateReturn()
        {
            await UniTask.WaitForSeconds(0.5f, true);
            UnityEngine.Debug.Log($"REWARD RETURN D {toBeReward}");
            Return(toBeReward ? AdShowResult.Status.Success : AdShowResult.Status.Cancelled);
        }
        private void HandleInterstitialFailedToShow(object sender, EventArgs args)
        {
            UnityEngine.Debug.Log("REWARD FAILURE");
            Return(AdShowResult.Status.Failure);
            DestroyInterstitial();
            RequesRewardedAd();
        }
        public UniTask<AdShowResult> Show()
        {
            UnityEngine.Debug.Log("TRYNA SHOW");
            if(Ready && !Locked)
            {
                UnityEngine.Debug.Log("REWARD SHOW");
                toBeReward = false;
                var x = Run();
                interstitial.Show();
                return x;
            }
            UnityEngine.Debug.Log("FAILURE INSTANT");
            return UniTask.FromResult(Default);
        }
        public UniTask Preload()
        {
            RequesRewardedAd();
            return UniTask.WaitUntil(() => Ready).TimeoutWithoutException(TimeSpan.FromSeconds(preloadTime));
        }
        public void Dispose()
        {
            interstitial?.Destroy();
        }
    }
}