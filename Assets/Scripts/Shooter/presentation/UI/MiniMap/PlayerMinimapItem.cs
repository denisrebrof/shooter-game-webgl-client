using System;
using Shooter.domain;
using UniRx;
using UnityEngine;
using Zenject;

namespace Shooter.presentation.UI.MiniMap
{
    public class PlayerMinimapItem : PlayerDataBehaviour
    {
        [SerializeField] private Transform target;
        [Inject] private IMinimapItemHandler handler;
        [Inject] private CurrentPlayerStateUseCase currentPlayerStateUseCase;

        private bool isEnemy = false;

        private void Start() => GetIsAllieFlow()
            .First()
            .Do(isAllie => isEnemy = !isAllie)
            .Subscribe(isAllie => handler.Add(target, !isAllie))
            .AddTo(this);

        private IObservable<bool> GetIsAllieFlow() => Observable.CombineLatest(
            currentPlayerStateUseCase.GetStateFlow().Select(data => data.Team),
            PlayerDataFlow.Select(data => data.Team),
            (currentTeam, playerTeam) => currentTeam == playerTeam
        );

        private void OnDestroy() => handler.Remove(target, isEnemy);
    }
}