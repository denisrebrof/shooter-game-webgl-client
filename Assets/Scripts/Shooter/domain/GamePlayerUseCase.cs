using System;
using System.Linq;
using Shooter.domain.Model;
using Shooter.domain.Repositories;
using UniRx;
using UnityEngine;
using Zenject;

namespace Shooter.domain
{
    public class GamePlayerUseCase
    {
        [Inject] private IGameStateRepository stateRepository;

        public IObservable<PlayerData> GetPlayerState(long playerId) => stateRepository
            .state
            .Select(state => state.playerData)
            .Where(players => players.Any(player => player.playerId == playerId))
            .Select(players => players.Find(player => player.playerId == playerId));
    }
}