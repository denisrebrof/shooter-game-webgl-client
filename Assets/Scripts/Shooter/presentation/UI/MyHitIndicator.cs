using System;
using System.Collections;
using Core.Auth.domain;
using Plugins.GoogleAnalytics;
using Shooter.domain;
using Shooter.domain.Repositories;
using UniRx;
using UnityEngine;
using Zenject;

namespace Shooter.presentation.UI
{
    public class MyHitIndicator : MonoBehaviour
    {
        [Inject] private IAuthRepository authRepository;
        [Inject] private IGameActionsRepository gameActionsRepository;
        [SerializeField] private CanvasGroup hitCanvas;
        [SerializeField] private float duration = 0.5f;
        [SerializeField] private AnimationCurve alphaCurve;

        private IDisposable handler = Disposable.Empty;

        private void OnEnable()
        {
            hitCanvas.alpha = 0f;
            var currentPlayerId = Convert.ToInt64(authRepository.LoginUserId);
            handler = gameActionsRepository
                .Hits
                .Where(hit => hit.receiverId == currentPlayerId)
                .Subscribe(_ => RenderHit())
                .AddTo(this);
        }

        private void RenderHit()
        {
            GoogleAnalyticsSDK.SendEvent("received_hit");
            StopAllCoroutines();
            StartCoroutine(RenderHitCoroutine());
        }

        private IEnumerator RenderHitCoroutine()
        {
            var startAlpha = hitCanvas.alpha;
            var timer = duration;
            while (timer > 0f)
            {
                timer -= Time.deltaTime;
                yield return null;
                var progress = timer / duration;
                var alpha = alphaCurve.Evaluate(progress);
                alpha = Mathf.Lerp(startAlpha, 1f, progress);
                hitCanvas.alpha = alpha;
            }
            hitCanvas.alpha = 0f;
        }

        private void OnDisable()
        {
            StopAllCoroutines();
            handler.Dispose();
        }
    }
}