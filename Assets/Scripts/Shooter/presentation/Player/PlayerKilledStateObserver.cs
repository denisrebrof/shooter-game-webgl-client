using System;
using JetBrains.Annotations;
using Shooter.domain;
using UniRx;
using UnityEngine;
using Zenject;

namespace Shooter.presentation.Player
{
    public class PlayerKilledStateObserver : PlayerBehavior
    {
        [Inject] private GamePlayerAliveStateUpdatesUseCase aliveStateUpdatesUseCase;
        [SerializeField] private GameObject character;
        [SerializeField] private GameObject corpsePrefab;

        [CanBeNull] private GameObject existingCorpse;

        private IDisposable handler = Disposable.Empty;

        private void OnEnable()
        {
            handler = PlayerIdFlow
                .Select(aliveStateUpdatesUseCase.GetAliveStateUpdatesFlow)
                .Switch()
                .Subscribe(OnAliveStateUpdate);
        }

        private void OnDisable() => handler.Dispose();

        private void OnAliveStateUpdate(AliveStateUpdate update)
        {
            character.SetActive(update.Alive);
            if (existingCorpse != null)
                Destroy(existingCorpse);

            if (update.Alive)
                return;

            var characterTransform = character.transform;
            existingCorpse = Instantiate(
                original: corpsePrefab,
                position: characterTransform.position,
                rotation: characterTransform.rotation,
                parent: transform
            );
            existingCorpse.SetActive(true);
        }
    }
}