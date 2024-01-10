using System;
using UniRx;
using Zenject;
#if YANDEX_SDK && !UNITY_EDITOR
using Plugins.Platforms.YSDK;
#endif

namespace Core.Ads.presentation.InterstitialAdNavigator.decorators
{
    public class YandexInterstitialNavigatorHitsDecorator : IInterstitialAdNavigator
    {
        [Inject] private IInterstitialAdNavigator adNavigator;
#if YANDEX_SDK && !UNITY_EDITOR
        [Inject] private YandexSDK instance;
#endif

        public IObservable<ShowInterstitialResult> ShowAd()
        {
#if YANDEX_SDK && !UNITY_EDITOR
            instance.AddHit("TryShowInterstitial");
#endif
            return adNavigator.ShowAd().Do(HandleResult);
        }

        private void HandleResult(ShowInterstitialResult result)
        {
#if YANDEX_SDK && !UNITY_EDITOR
            var hit = "ShowInterstitial/";
            hit += result.IsSuccess ? "Success" : "NotSuccess";
            instance.AddHit(hit);
#endif
        }
    }
}