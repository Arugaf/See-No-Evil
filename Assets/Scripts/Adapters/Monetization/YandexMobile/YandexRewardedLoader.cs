using Cysharp.Threading.Tasks;
using System;
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

        protected override AdShowResult Default => AdShowResult.Status.Failure;
        public YandexRewardedLoader(string identifier, double failTime, double preloadTime)
        {
            lrdr = new RewardedAdLoader();
            lrdr.OnAdLoaded += HandleInterstitialLoaded;
            this.identifier = identifier;
            this.preloadTime = preloadTime;
            FailTime = failTime * 10;
        }
        private void HandleInterstitialLoaded(object sender, RewardedAdLoadedEventArgs args)
        {
            interstitial = args.RewardedAd;
            interstitial.OnRewarded += HandleInterstitialShown;
            interstitial.OnAdFailedToShow += HandleInterstitialFailedToShow;
            interstitial.OnAdDismissed += HandleInterstitialDismissed;
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
            Return(AdShowResult.Status.Success);
        }
        private void HandleInterstitialFailedToShow(object sender, EventArgs args)
        {
            Return(AdShowResult.Status.Failure);
            DestroyInterstitial();
            RequesRewardedAd();
        }
        public UniTask<AdShowResult> Show()
        {
            if(interstitial != null && !Locked)
            {
                var x = Run();
                interstitial.Show();
                return x;
            }
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