using Cysharp.Threading.Tasks;
using System;
using YandexMobileAds;
using YandexMobileAds.Base;
namespace Monetization
{
    public class YandexInterstitialLoader: OneThreadedFunc<AdShowResult>, IDisposable
    {
        public bool Ready => interstitial != null;
        
        InterstitialAdLoader lrdr;
        Interstitial interstitial;
        string identifier;
        private double preloadTime;

        protected override AdShowResult Default => AdShowResult.Status.Failure;
        public YandexInterstitialLoader(string identifier, double failTime, double preloadTime)
        {
            lrdr = new InterstitialAdLoader();
            lrdr.OnAdLoaded += HandleInterstitialLoaded;
            lrdr.OnAdFailedToLoad += Lrdr_OnAdFailedToLoad;
            this.identifier = identifier;
            this.preloadTime = preloadTime;
            FailTime = failTime;
        }

        private void Lrdr_OnAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
        {
            DestroyInterstitial();
            UnityEngine.Debug.Log("FAILED TO LOAD INTERSTITIAL");
        }

        private void HandleInterstitialLoaded(object sender, InterstitialAdLoadedEventArgs args)
        {
            interstitial = args.Interstitial;
            interstitial.OnAdShown += HandleInterstitialShown;
            interstitial.OnAdFailedToShow += HandleInterstitialFailedToShow;
            interstitial.OnAdDismissed += HandleInterstitialDismissed;
        }

        private void HandleInterstitialDismissed(object sender, EventArgs args)
        {
            DestroyInterstitial();
            RequestInterstitial();
        }
        private void RequestInterstitial()
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
            RequestInterstitial();
        }
        public UniTask<AdShowResult> Show()
        {
            UnityEngine.Debug.Log("SHOW INTR");
            if (Ready && !Locked)
            {
                var x = Run();
                interstitial.Show();
                return x;
            }
            UnityEngine.Debug.Log("INSTA FAIL TO SHOW");
            return UniTask.FromResult(Default);
        }
        public UniTask Preload()
        {
            RequestInterstitial();
            return UniTask.WaitUntil(() => Ready).TimeoutWithoutException(TimeSpan.FromSeconds(preloadTime));
        }
        public void Dispose()
        {
            interstitial?.Destroy();
        }
    }
}