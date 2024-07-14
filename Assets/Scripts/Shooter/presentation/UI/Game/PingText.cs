using System;
using Plugins.GoogleAnalytics;
using TMPro;
using UniRx;
using UnityEngine;
using Utils.WebSocketClient.domain;
using Utils.WebSocketClient.domain.model;
using Zenject;

namespace Shooter.presentation.UI.Game
{
    public class PingText : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;

        private readonly TimeSpan delay = TimeSpan.FromMilliseconds(500);
        private readonly int sendPingCount = 10;
        private readonly TimeSpan timeout = TimeSpan.FromSeconds(1);

        [Inject(Id = IWSCommandsUseCase.AuthorizedInstance)]
        private IWSCommandsUseCase commandsUseCase;

        private IDisposable handler = Disposable.Empty;

        private bool lastRequestCompleted = true;
        private int pingAccumulator;

        private int pingCounter;

        private void Update()
        {
            if (!lastRequestCompleted)
                return;

            lastRequestCompleted = false;
            handler.Dispose();
            handler = GetPing()
                .Delay(delay)
                .Subscribe(OnPingReceived)
                .AddTo(this);
        }

        private void OnDisable()
        {
            handler.Dispose();
            lastRequestCompleted = true;
        }

        private IObservable<double> GetPing()
        {
            var requestStartTime = DateTime.Now;
            return commandsUseCase
                .Request<long>(Commands.Ping)
                .Timeout(timeout)
                .OnErrorRetry()
                .Select(_ => DateTime.Now.Subtract(requestStartTime).TotalMilliseconds);
        }

        private void OnPingReceived(double ping)
        {
            pingCounter += 1;
            pingAccumulator += (int)ping;
            if (pingCounter >= sendPingCount)
            {
                var medianPing = (int)((float)pingAccumulator / pingCounter);
                GoogleAnalyticsSDK.SendNumEvent("ping_" + sendPingCount, "ping", medianPing);
                pingCounter = 0;
                pingAccumulator = 0;
            }

            text.text = IntToString((int)ping);
            lastRequestCompleted = true;
        }

        private static string IntToString(int a)
        {
            var str = string.Empty;
            var isNegative = false;
            if (a < 0)
            {
                isNegative = true;
                a = -a;
            }

            do
            {
                str = (char)(byte)(a % 10 + 48) + str;
                a /= 10;
            } while (a > 0);

            return isNegative ? '-' + str : str;
        }
    }
}