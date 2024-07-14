using System;
using Shooter.domain;
using Shooter.domain.Model;
using Shooter.domain.Repositories;
using UniRx;
using UnityEngine;
using Utils.WebSocketClient.domain;
using Utils.WebSocketClient.domain.model;
using Zenject;

namespace Shooter.data
{
    public class GameStateMonoRepository : MonoBehaviour, IGameStateRepository
    {
        private readonly Subject<GameState> stateSubject = new();
        private GameState? current;
        
        [SerializeField] private GameState preview;

        public IObservable<GameState> state => current.HasValue
            ? stateSubject.StartWith(current.Value) 
            : stateSubject;

        [Inject(Id = IWSCommandsUseCase.AuthorizedInstance)]
        private IWSCommandsUseCase commandsUseCase;

        private void OnEnable() => commandsUseCase
                .Listen<GameState>(Commands.GameState)
                .Do(stateUpdate =>
                {
                    preview = stateUpdate;
                    current = stateUpdate;
                })
                .Subscribe(stateSubject.OnNext)
                .AddTo(this);

        private void ClearSubscriptions()
        {
            stateSubject.Dispose();
            current = null;
        }

        private void OnDisable() => ClearSubscriptions();
    }
}