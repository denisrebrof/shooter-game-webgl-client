using System;
using Core.Auth.domain;
using Shooter.domain;
using Shooter.domain.Repositories;
using UniRx;
using UnityEngine;
using Zenject;

namespace Shooter.presentation.UI
{
    public class HitIndicator : MonoBehaviour
    {
        [Inject] private IAuthRepository authRepository;
        [Inject] private IGameActionsRepository gameActionsRepository;
        [SerializeField] private Animator animator;

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