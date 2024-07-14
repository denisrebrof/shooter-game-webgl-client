using System;
using Shooter.domain;
using UniRx;
using UnityEngine;
using Zenject;

namespace Shooter.presentation.Scene
{
    public class StoreFlagTrigger : MonoBehaviour
    {
        [Inject] private CurrentPlayerHasFlagUseCase hasFlagUseCase;
        [Inject] private FlagActionsUseCase actionsUseCase;

        [SerializeField] private float storeFlagCommandTimer = 0.1f;
        private TimeSpan storeFlagTimerSpan;
        
        private bool playerHasFlag;

        private IDisposable handler = Disposable.Empty;

        private void Start() {
            storeFlagTimerSpan = TimeSpan.FromSeconds(storeFlagCommandTimer);
            hasFlagUseCase
                .HasFlagState
                .Subscribe(hasFlag => playerHasFlag = hasFlag)
                .AddTo(this);
        }

        private void OnTriggerEnter(Collider other) {
            if (!enabled || !playerHasFlag || !other.CompareTag("Player"))
                return;

            handler = Observable
                .Timer(storeFlagTimerSpan)
                .Repeat()
                .Select(_ => StoreFlag())
                .Switch()
                .Subscribe()
                .AddTo(this);
        }

        private void OnTriggerExit(Collider other) {
            if (!other.CompareTag("Player"))
                return;

            handler.Dispose();
        }

        private IObservable<Unit> StoreFlag() => actionsUseCase
            .StoreFlag()
            .AsUnitObservable();

        private void OnDestroy() => handler.Dispose();
    }
}