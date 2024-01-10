using System;
using UniRx;
using Zenject;

namespace Core.Ads.presentation.InterstitialAdNavigator.decorators
{
    public class InterstitialAdNavigatorCounterDecorator : IInterstitialAdNavigator
    {
        private IInterstitialAdNavigator adNavigator;

        //TODO: replace with di
        private readonly IInterstitialShowIntervalProvider intervalProvider = new NoIntervalProvider();

        private int invokeTimes;
        private int showInterval = 1;

        private readonly IDisposable observeShowIntervalDisposable;

        [Inject]
        public InterstitialAdNavigatorCounterDecorator(IInterstitialAdNavigator adNavigator)
        {
            this.adNavigator = adNavigator;
            observeShowIntervalDisposable = intervalProvider
                .GetShowInterval()
                .Subscribe(interval => showInterval = interval);
        }

        public IObservable<ShowInterstitialResult> ShowAd()
        {
            if (showInterval == 0)
                return Observable.Return(new ShowInterstitialResult(false, "zero show interval"));

            invokeTimes++;
            if (invokeTimes < showInterval)
                Observable.Return(new ShowInterstitialResult(false, "period not reached"));

            invokeTimes = 0;
            return adNavigator.ShowAd();
        }

        ~InterstitialAdNavigatorCounterDecorator() => observeShowIntervalDisposable.Dispose();

        public interface IInterstitialShowIntervalProvider
        {
            public IObservable<int> GetShowInterval();
        }
        
        private class NoIntervalProvider : IInterstitialShowIntervalProvider
        {
            public IObservable<int> GetShowInterval() => Observable.Return(1);
        }
    }
}