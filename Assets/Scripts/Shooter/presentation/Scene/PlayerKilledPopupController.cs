using System;
using Core.Auth.domain;
using Shooter.domain.Model;
using Shooter.domain.Repositories;
using Shooter.presentation.Player.Weapons;
using UniRx;
using UnityEngine;
using Zenject;

namespace Shooter.presentation.Scene
{
    public class PlayerKilledPopupController : PlayerDataBehaviour
    {
        [SerializeField] private Transform emitter;
        [Inject] private GamePopupsNavigator popupsNavigator;
        [Inject] private IGameActionsRepository gameActionsRepository;
        [Inject] private IAuthRepository authRepository;

        private long currentPlayerId;
        private IDisposable subscription = Disposable.Empty;

        private void Awake()
        {
            currentPlayerId = Convert.ToInt64(authRepository.LoginUserId);
        }

        private void OnEnable()
        {
            subscription = gameActionsRepository.Hits.Subscribe(ProcessHit);
        }

        private void ProcessHit(ActionHit hit)
        {
            if (!GetPlayerId(out var playerId)) return;
            if (hit.receiverId != playerId || hit.damagerId != currentPlayerId || !hit.killed) return;
            popupsNavigator.ShowKilled(emitter.position);
        }

        private void OnDisable() => subscription.Dispose();

        private void OnDestroy() => subscription.Dispose();
    }
}