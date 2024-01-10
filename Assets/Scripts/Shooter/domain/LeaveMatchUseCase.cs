using System;
using Plugins.GoogleAnalytics;
using UniRx;
using Utils.WebSocketClient.domain;
using Utils.WebSocketClient.domain.model;
using Zenject;

namespace Shooter.domain
{
    public class LeaveMatchUseCase
    {
        [Inject(Id = IWSCommandsUseCase.AuthorizedInstance)]
        private IWSCommandsUseCase commandsUseCase;

        public IDisposable Leave()
        {
            GoogleAnalyticsSDK.SendEvent("leave_match");
            return commandsUseCase
                .Request<bool>(Commands.LeaveMatch)
                .Subscribe();
        }
    }
}