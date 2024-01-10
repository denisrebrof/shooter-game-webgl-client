using System;
using System.Collections;
using UniRx;
using UnityEngine;

namespace Core.Ads.presentation.InterstitialAdNavigator.core
{
    public class DebugLogInterstitialAdNavigator : IInterstitialAdNavigator
    {
        private float delay = 0.5f;

        public IObservable<ShowInterstitialResult> ShowAd()
        {
            Debug.Log("Debug Show interstitial");
            return Observable
                .FromCoroutine(() => WaitForRealSeconds(delay))
                .Select(_ => ShowInterstitialResult.Success);
        }

        private static IEnumerator WaitForRealSeconds(float seconds)
        {
            var startTime = Time.realtimeSinceStartup;
            while (Time.realtimeSinceStartup - startTime < seconds)
            {
                yield return null;
            }
        }
    }
}