using System;
using Features.Lobby.domain.model;
using Plugins.GoogleAnalytics;
using UniRx;
using Utils.WebSocketClient.domain;
using Utils.WebSocketClient.domain.model;
using Zenject;

namespace Features.Lobby.domain
{
    public class LobbyGamesUseCase
    {
        [Inject(Id = IWSCommandsUseCase.AuthorizedInstance)]
        private IWSCommandsUseCase commandsUseCase;

        private readonly TimeSpan gamesRequestInterval = TimeSpan.FromMilliseconds(5000L);

        public IObservable<GameList> GetGamesListFlow() => Observable
            .Timer(gamesRequestInterval)
            .Repeat()
            .StartWith(0L)
            .Select(_ => RequestGamesListFlow())
            .Switch();

        public IObservable<bool> JoinGame(string matchId)
        {
            GoogleAnalyticsSDK.SendEvent("join_game_from_list");
            return commandsUseCase
                .Request<bool, string>(Commands.JoinMatch, matchId);
        }

        private IObservable<GameList> RequestGamesListFlow()
        {
            return commandsUseCase
                .Request<GameList>(Commands.GetGames);
        }
    }
}