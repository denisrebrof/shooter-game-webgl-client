using UniRx;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Core.Ads.presentation.InterstitialAdNavigator
{
    public class ShowInterstitialBridge : MonoBehaviour
    {
        [Inject(Id = IInterstitialAdNavigator.DefaultInstance)]
        private IInterstitialAdNavigator navigator;

        [SerializeField] private UnityEvent shownEvent;
        [SerializeField] private UnityEvent failedEvent;

        public void TryShow()
        {
            navigator.ShowAd().Subscribe(
                res =>
                {
                    var currentEvent = res.IsSuccess ? shownEvent : failedEvent;
                    currentEvent?.Invoke();
                },
                e => failedEvent?.Invoke()
            ).AddTo(this);
        }
    }
}