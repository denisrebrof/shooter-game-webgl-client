using System;
using Michsky.MUIP;
using Shooter.domain;
using UniRx;
using UnityEngine;
using Zenject;

namespace Shooter.presentation.UI.Game
{
    public class PlayerHasFlagNotification : MonoBehaviour
    {
        [Inject] private CurrentPlayerHasFlagUseCase hasFlagUseCase;

        [SerializeField] private NotificationManager manager;

        private IDisposable handler = Disposable.Empty;

        private void OnEnable() {
            handler = hasFlagUseCase
                .HasFlagState
                .Subscribe(UpdateState)
                .AddTo(this);
        }

        private void UpdateState(bool hasFlag) {
            if (hasFlag) manager.Open();
            else manager.Close();
        }

        private void OnDisable() {
            if (manager.gameObject.activeInHierarchy) manager.Close();
            handler.Dispose();
        }
    }
}