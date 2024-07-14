using System;
using Shooter.domain;
using UniRx;
using UnityEngine;
using Zenject;

namespace Shooter.presentation.UI.Game
{
    public class CurrentPlayerHp : MonoBehaviour
    {
        [SerializeField] private Transform hpBar;
        [Inject] private CurrentPlayerHpUseCase currentPlayerHpUseCase;

        private IDisposable handler = Disposable.Empty;

        private void OnEnable()
        {
            handler.Dispose();
            handler = currentPlayerHpUseCase
                .GetHpFlow()
                .Subscribe(hp => hpBar.localScale = new Vector3(hp / 100f, 1f, 1f))
                .AddTo(this);
        }

        private void OnDisable()
        {
            handler.Dispose();
        }
    }
}