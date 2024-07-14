using System;
using Core.Auth.domain;
using Shooter.domain.Repositories;
using UniRx;
using UnityEngine;
using Zenject;

namespace Shooter.presentation.UI.Game
{
    public class HitIndicator : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [Inject] private IAuthRepository authRepository;
        [Inject] private IGameActionsRepository gameActionsRepository;

        private void Awake()
        {
            var currentPlayerId = Convert.ToInt64(authRepository.LoginUserId);
            gameActionsRepository
                .Hits
                .Where(hit => hit.damagerId == currentPlayerId)
                .Select(hit => hit.killed ? "kill" : "hit")
                .Subscribe(animator.SetTrigger)
                .AddTo(this);
        }
    }
}