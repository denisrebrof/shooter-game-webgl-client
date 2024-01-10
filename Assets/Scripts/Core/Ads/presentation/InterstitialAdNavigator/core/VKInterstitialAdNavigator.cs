#if VK_SDK
using System;
using Plugins.Platforms.VKSDK;
using UniRx;
using Zenject;

namespace Core.Ads.presentation.InterstitialAdNavigator.core
{
    public class VKInterstitialAdNavigator : IInterstitialAdNavigator
    {
        [Inject] private VKSDK instance;

        public IObservable<ShowInterstitialResult> ShowAd()
        {
            var interstitialShownObservable = Observable.FromEvent(
                handler => instance.onInterstitialShown += handler,
                handler => instance.onInterstitialShown -= handler
            ).Select((_) => ShowInterstitialResult.Success);

            var interstitialFailedObservable = Observable.FromEvent<string>(
                handler => instance.onInterstitialFailed += handler,
                handler => instance.onInterstitialFailed -= handler
            ).Select((error) => new ShowInterstitialResult(false, error));

            return Observable
                .Merge(interstitialShownObservable, interstitialFailedObservable)
                .Take(1)
                .DoOnSubscribe( () => instance.ShowInterstitial());
        }
    }
}
#endif