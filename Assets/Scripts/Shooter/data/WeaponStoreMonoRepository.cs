using System;
using Shooter.domain.Model;
using Shooter.domain.Repositories;
using UniRx;
using UnityEngine;
using Utils.WebSocketClient.domain;
using Utils.WebSocketClient.domain.model;
using Zenject;

namespace Shooter.data
{
    public class WeaponStoreMonoRepository : MonoBehaviour, IWeaponStoreRepository
    {
        [Inject(Id = IWSCommandsUseCase.AuthorizedInstance)]
        private IWSCommandsUseCase commandsUseCase;

        private readonly ReplaySubject<WeaponsData> weaponsData = new(1);

        private void Start() => commandsUseCase
            .Subscribe<WeaponsData>(Commands.WeaponStates)
            .Subscribe(weaponsData.OnNext)
            .AddTo(this);

        public IObservable<WeaponsData> GetWeaponsData() => weaponsData;

        private void OnDestroy() => weaponsData.Dispose();
    }
}