using System;
using Core.Auth.domain;
using Shooter.domain.Model;
using UniRx;
using UnityEngine;
using Zenject;

namespace Shooter.domain
{
    public class CurrentPlayerStateUseCase
    {
        [Inject] private IAuthRepository authRepository;
        [Inject] private GamePlayerUseCase playerUseCase;

        public IObservable<PlayerData> GetStateFlow()
        {
            var parsedSuccess = long.TryParse(authRepository.LoginUserId, out var userId);
            return parsedSuccess
                ? playerUseCase.GetPlayerState(userId)
                : Observable.Empty<PlayerData>();
        }
    }
}