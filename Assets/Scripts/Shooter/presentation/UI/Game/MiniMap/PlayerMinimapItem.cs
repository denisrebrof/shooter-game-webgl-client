using System;
using Shooter.domain;
using Shooter.domain.Model;
using UniRx;
using UnityEngine;
using Zenject;

namespace Shooter.presentation.UI.Game.MiniMap
{
    public class PlayerMinimapItem : PlayerDataBehaviour
    {
        [SerializeField] private Transform target;
        [Inject] private CurrentPlayerTeamUseCase currentPlayerTeamUseCase;
        [Inject] private IMinimapItemHandler handler;

        private IObservable<Teams> PlayerTeamFlow => PlayerDataFlow.Select(data => data.Team);

        private bool isEnemy;

        private void Start() => GetIsAllieFlow()
            .Do(isAllie => isEnemy = !isAllie)
            .Subscribe(isAllie => handler.Add(target, !isAllie))
            .AddTo(this);

        private void OnDestroy() => handler.Remove(target, isEnemy);

        private IObservable<bool> GetIsAllieFlow() => Observable
            .CombineLatest(currentPlayerTeamUseCase.TeamFlow, PlayerTeamFlow, IsAllie)
            .First();

        private static bool IsAllie(Teams currentPlayerTeam, Teams playerTeam) => currentPlayerTeam == playerTeam;
    }
}