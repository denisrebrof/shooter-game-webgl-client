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
    public class GameActionsMonoRepository: MonoBehaviour, IGameActionsRepository
    {
        private readonly Subject<ActionShoot> shootsSubject = new();
        private readonly Subject<ActionHit> hitsSubject = new ();
        private readonly Subject<ActionJoinedStateChange> joinedChangesSubject = new ();

        public IObservable<ActionHit> Hits => hitsSubject;
        public IObservable<ActionShoot> Shoots => shootsSubject;
        public IObservable<ActionJoinedStateChange> JoinedStateChanges => joinedChangesSubject;
        
        [Inject(Id = IWSCommandsUseCase.AuthorizedInstance)]
        private IWSCommandsUseCase commandsUseCase;

        private void Awake()
        {
            commandsUseCase
                .Listen<ActionShoot>(Commands.ActionShoot)
                .Subscribe(shootsSubject.OnNext)
                .AddTo(this);
            commandsUseCase
                .Listen<ActionHit>(Commands.ActionHit)
                .Subscribe(hitsSubject.OnNext)
                .AddTo(this);
            commandsUseCase
                .Listen<ActionJoinedStateChange>(Commands.ActionJoinedStateChange)
                .Subscribe(joinedChangesSubject.OnNext)
                .AddTo(this);
        }
    }
}