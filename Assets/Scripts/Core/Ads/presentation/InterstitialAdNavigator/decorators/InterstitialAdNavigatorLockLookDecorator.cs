using System;
using UniRx;
using UnityEngine;
using Zenject;

namespace Core.Ads.presentation.InterstitialAdNavigator.decorators
{
    public class InterstitialAdNavigatorLockLookDecorator : IInterstitialAdNavigator
    {
        [Inject] private IInterstitialAdNavigator Target;

        public IObservable<ShowInterstitialResult> ShowAd()
        {
            SetLockedState(true);
            return Target
                .ShowAd()
                .Do(_ => SetLockedState(false))
                .DoOnError(_ => SetLockedState(false));
        }

        private void SetLockedState(bool locked)
        {
            Debug.Log("SetLockedState: " + locked);
            Time.timeScale = locked ? 0f : 1f;
        }
    }
}