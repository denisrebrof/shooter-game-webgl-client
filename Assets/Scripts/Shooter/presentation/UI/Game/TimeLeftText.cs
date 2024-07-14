using System;
using System.Collections;
using JetBrains.Annotations;
using Shooter.domain.Model;
using Shooter.domain.Repositories;
using TMPro;
using UniRx;
using UnityEngine;
using Utils.WebSocketClient.domain;
using Utils.WebSocketClient.domain.model;
using Zenject;

namespace Shooter.presentation.UI.Game
{
    public class TimeLeftText : MonoBehaviour
    {
        [SerializeField] private TMP_Text target;

        [Inject(Id = IWSCommandsUseCase.AuthorizedInstance)]
        private IWSCommandsUseCase commandsUseCase;

        [CanBeNull] private Coroutine countdown;
        private IDisposable gameStateHandler = Disposable.Empty;

        [Inject] private IGameStateRepository gameStateRepository;
        private IDisposable requestHandler = Disposable.Empty;

        private void OnEnable()
        {
            target.enabled = false;
            gameStateHandler = gameStateRepository
                .state
                .Select(state => state.Type == GameStateTypes.Playing)
                .DistinctUntilChanged()
                .Subscribe(OnPlayingStateChanged);
        }

        private void OnDestroy()
        {
            gameStateHandler.Dispose();
            requestHandler.Dispose();
            if (countdown != null) StopCoroutine(countdown);
        }

        private void OnPlayingStateChanged(bool playing)
        {
            if (playing) ShowTimer();
            else HideTimer();
        }

        private void ShowTimer()
        {
            requestHandler.Dispose();
            requestHandler = commandsUseCase
                .Request<long>(Commands.TimeLeft)
                .Subscribe(StartCountdown);
        }

        private void HideTimer()
        {
            requestHandler.Dispose();
            if (countdown != null) StopCoroutine(countdown);
            target.enabled = false;
        }

        private void StartCountdown(long timeLeft)
        {
            if (countdown != null) StopCoroutine(countdown);
            countdown = StartCoroutine(CountdownCoroutine(timeLeft));
        }

        private IEnumerator CountdownCoroutine(long timeLeft)
        {
            var timerSeconds = timeLeft / 1000;
            while (timerSeconds > 0)
            {
                target.enabled = true;
                var minutes = timerSeconds / 60;
                var seconds = timerSeconds % 60;
                target.text = $"{minutes}:{seconds}";
                yield return new WaitForSecondsRealtime(1f);
                timerSeconds -= 1;
            }

            target.enabled = false;
        }
    }
}