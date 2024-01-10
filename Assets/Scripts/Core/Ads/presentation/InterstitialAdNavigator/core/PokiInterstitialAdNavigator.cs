#if POKI_SDK
using System;
using Poki.Plugins.Platforms.Poki;
using UniRx;
using Zenject;

namespace Core.Ads.presentation.InterstitialAdNavigator.core
{
    public class PokiInterstitialAdNavigator : IInterstitialAdNavigator
    {
        [Inject] private PokiUnitySDK sdk;

        public IObservable<ShowInterstitialResult> ShowAd()
        {
            if (sdk.adsBlocked())
                return Observable.Return(new ShowInterstitialResult(false, "adsBlocked"));

            if (sdk.isShowingAd)
                return Observable.Return(new ShowInterstitialResult(false, "isShowingAd"));

            return Observable.Create((IObserver<ShowInterstitialResult> observer) =>
                {
                    Action callback = () => {
                        observer.OnNext(ShowInterstitialResult.Success);
                        observer.OnCompleted();
                    };
                    var callbackDelegate = new PokiUnitySDK.CommercialBreakDelegate(callback);
                    callbackDelegate += () => sdk.commercialBreakCallBack -= callbackDelegate;
                    sdk.commercialBreakCallBack += callbackDelegate;
                    
                    sdk.commercialBreak();
                    
                    //Double check for editor case
                    if (sdk.isShowingAd) 
                        return Disposable.Create(() => { });
                    
                    observer.OnNext(new ShowInterstitialResult(false, "!isShowingAd"));
                    observer.OnCompleted();
                    sdk.commercialBreakCallBack -= callbackDelegate;

                    return Disposable.Create(() => { });
                }
            );
        }
    }
}
#endif