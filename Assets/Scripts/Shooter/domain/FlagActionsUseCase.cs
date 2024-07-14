using System;
using UnityEngine;
using Utils.WebSocketClient.domain;
using Utils.WebSocketClient.domain.model;
using Zenject;

namespace Shooter.domain
{
    public class FlagActionsUseCase
    {
        [Inject(Id = IWSCommandsUseCase.AuthorizedInstance)]
        private IWSCommandsUseCase commandsUseCase;

        public IObservable<bool> TakeFlag() => SendIntent(0L);
        
        public IObservable<bool> StoreFlag() => SendIntent(1L);
        
        public IObservable<bool> ReturnFlag() {
            Debug.Log("ReturnFlag");
            return SendIntent(2L);
        }

        private IObservable<bool> SendIntent(long actionCode) => commandsUseCase
            .Request<bool, long>(Commands.IntentFlagAction, actionCode);
    }
}