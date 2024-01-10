using System;
using UniRx;
using UnityEngine;
using Zenject;

namespace Shooter.domain
{
    public class CurrentPlayerHpUseCase
    {
        [Inject] private CurrentPlayerStateUseCase playerUseCase;

        public IObservable<int> GetHpFlow() => playerUseCase
            .GetStateFlow()
            .Select(state => state.hp);
    }
}