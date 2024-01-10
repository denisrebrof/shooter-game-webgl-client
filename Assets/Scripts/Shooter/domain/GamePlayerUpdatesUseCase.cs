using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using Shooter.domain.Model;
using Shooter.domain.Repositories;
using UniRx;
using UnityEngine;
using Zenject;

namespace Shooter.domain
{
    public class GamePlayerUpdatesUseCase
    {
        [Inject] private IGameStateRepository gameStateRepository;

        public IObservable<GamePlayersUpdate> getUpdates() => Observable
            .Create<GamePlayersUpdate>(CreateUpdatesSubscription);

        private IDisposable CreateUpdatesSubscription(IObserver<GamePlayersUpdate> observer)
        {
            var playersData = new Dictionary<long, PlayerData>();
            return gameStateRepository
                .state
                .Where(state => state.Type == GameStateTypes.Playing)
                .Select(GetPlayersMap)
                .Do(players =>
                {
                    var addedPlayers = players
                        .Values
                        .Where(player => !playersData.ContainsKey(player.playerId))
                        .ToList();
                    if (!addedPlayers.IsEmpty())
                        observer.OnNext(new GamePlayersUpdate(addedPlayers, GamePlayersUpdateType.Added));

                    var removedPlayers = playersData
                        .Values
                        .Where(player => !players.ContainsKey(player.playerId))
                        .ToList();
                    if (!removedPlayers.IsEmpty())
                        observer.OnNext(new GamePlayersUpdate(removedPlayers, GamePlayersUpdateType.Removed));

                    playersData = players;
                })
                .DoOnCompleted(observer.OnCompleted)
                .Subscribe();
        }

        private Dictionary<long, PlayerData> GetPlayersMap(GameState state) => state
            .playerData
            .ToDictionary(player => player.playerId);
    }

    public struct GamePlayersUpdate
    {
        public List<PlayerData> Players;
        public GamePlayersUpdateType Type;

        public GamePlayersUpdate(List<PlayerData> players, GamePlayersUpdateType type)
        {
            Players = players;
            Type = type;
        }
    }

    public enum GamePlayersUpdateType
    {
        Added,
        Removed
    }
}