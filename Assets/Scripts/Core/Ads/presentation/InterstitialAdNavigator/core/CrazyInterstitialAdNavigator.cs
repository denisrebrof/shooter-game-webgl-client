#if CRAZY_SDK
using System;
using CrazyGames;
using UniRx;

namespace Core.Ads.presentation.InterstitialAdNavigator.core
{
    public class CrazyInterstitialAdNavigator : IInterstitialAdNavigator
    {
        public IObservable<ShowInterstitialResult> ShowAd() =>
            Observable.Create((IObserver<ShowInterstitialResult> observer) =>
                {
                    CrazyAds.Instance.beginAdBreak(
                        () =>
                        {
                            observer.OnNext(ShowInterstitialResult.Success);
                            observer.OnCompleted();
                        },
                        () =>
                        {
                            observer.OnNext(new ShowInterstitialResult(false));
                            observer.OnCompleted();
                        }
                    );
                    return Disposable.Create(() => { });
                }
            );
    }
}
#endif