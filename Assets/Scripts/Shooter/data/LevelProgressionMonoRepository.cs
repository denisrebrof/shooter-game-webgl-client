using System;
using Shooter.domain.Model;
using Shooter.domain.Repositories;
using Shooter.presentation.UI.Lobby;
using UniRx;
using UnityEngine;
using Utils.WebSocketClient.domain;
using Utils.WebSocketClient.domain.model;
using Zenject;

namespace Shooter.data
{
    public class LevelProgressionMonoRepository : MonoBehaviour, ILevelProgressionRepository
    {
        [Inject(Id = IWSCommandsUseCase.AuthorizedInstance)]
        private IWSCommandsUseCase commandsUseCase;

        private readonly ReplaySubject<LevelProgressionData> dataSubject = new(1);

        private void Awake() =>
            commandsUseCase
                .Request<LevelProgressionData>(Commands.LevelProgression)
                .Subscribe(dataSubject.OnNext)
                .AddTo(this);

        public IObservable<LevelProgressionData> Get() => dataSubject;
    }
}