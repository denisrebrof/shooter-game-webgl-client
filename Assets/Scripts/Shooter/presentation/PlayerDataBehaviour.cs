using System;
using Shooter.domain;
using Shooter.domain.Model;
using UniRx;
using Zenject;

namespace Shooter.presentation
{
    public class PlayerDataBehaviour : PlayerBehavior
    {
        [Inject] protected GamePlayerUseCase PlayerUseCase;

        protected IObservable<PlayerData> PlayerDataFlow => PlayerIdFlow
            .Select(PlayerUseCase.GetPlayerState)
            .Switch();
    }
}