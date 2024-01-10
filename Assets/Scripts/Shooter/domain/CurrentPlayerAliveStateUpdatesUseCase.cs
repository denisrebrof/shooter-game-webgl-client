using System;
using Core.Auth.domain;
using UniRx;
using Zenject;

namespace Shooter.domain
{
    public class CurrentPlayerAliveStateUpdatesUseCase
    {
        [Inject] private IAuthRepository authRepository;
        [Inject] private GamePlayerAliveStateUpdatesUseCase aliveStateUpdatesUseCase;

        public IObservable<AliveStateUpdate> GetAliveStateUpdatesFlow() => long
            .TryParse(authRepository.LoginUserId, out var userId)
            ? aliveStateUpdatesUseCase.GetAliveStateUpdatesFlow(userId)
            : Observable.Empty<AliveStateUpdate>();
    }
}