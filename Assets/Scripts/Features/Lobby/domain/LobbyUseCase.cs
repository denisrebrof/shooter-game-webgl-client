using System;
using Plugins.GoogleAnalytics;
using UniRx;
using Utils.WebSocketClient.domain;
using Utils.WebSocketClient.domain.model;
using Zenject;

namespace Features.Lobby.domain
{
    public class LobbyUseCase
    {
        [Inject(Id = IWSCommandsUseCase.AuthorizedInstance)]
        private IWSCommandsUseCase commandsUseCase;

        private const long EnterLobbyIntentCode = 0L;
        private const long ExitLobbyIntentCode = 1L;

        public IDisposable JoinLobby()
        {
            GoogleAnalyticsSDK.SendEvent("join_lobby");
            return commandsUseCase
                .Request<long, long>(Commands.LobbyAction, EnterLobbyIntentCode)
                .DistinctUntilChanged()
                .Subscribe();
        }

        public IDisposable LeaveLobby()
        {
            GoogleAnalyticsSDK.SendEvent("leave_lobby");
            return commandsUseCase
                .Request<long, long>(Commands.LobbyAction, ExitLobbyIntentCode)
                .DistinctUntilChanged()
                .Subscribe();
        }

        public IObservable<bool> GetLobbyStateFlow() => commandsUseCase
            .Subscribe<long>(Commands.LobbyState)
            .DistinctUntilChanged()
            .Select(code => code == 1L);
    }
}