using System;
using Core.Auth.domain;
using Shooter.domain.Model;
using Shooter.domain.Repositories;
using UniRx;
using Zenject;

namespace Shooter.domain
{
    public class CurrentPlayerHasFlagUseCase
    {
        [Inject] private IGameStateRepository gameStateRepository;
        [Inject] private CurrentPlayerTeamUseCase playerTeamUseCase;
        [Inject] private IAuthRepository authRepository;

        public IObservable<bool> HasFlagState =>
            Observable.CombineLatest(
                playerTeamUseCase.TeamFlow,
                gameStateRepository.state,
                GetHasFlagState
            ).DistinctUntilChanged();

        private bool GetHasFlagState(Teams playerTeam, GameState state) {
            var opponentTeamData = playerTeam == Teams.Red ? state.blueTeamState : state.redTeamState;
            if (opponentTeamData.flagHasOwner == false)
                return false;

            var currentPlayerId = Convert.ToInt64(authRepository.LoginUserId);
            return opponentTeamData.flagOwnerId == currentPlayerId;
        }
    }
}