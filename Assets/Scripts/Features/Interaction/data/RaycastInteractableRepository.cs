using System;
using Features.Interaction.domain;
using UniRx;
using UnityEngine;

namespace Features.Interaction.data
{
    public class RaycastInteractableRepository : MonoBehaviour, ISelectedInteractableRepository
    {
        [SerializeField] private float interactionMaxDistance = 10f;
        [SerializeField] private int checkInteractionTimerMs = 500;
        [SerializeField] private Transform raycastSource;
        private readonly ReactiveProperty<bool> hasInteractableSubject = new(false);
        private readonly Subject<IInteractable> interactableSubject = new();

        private void Start()
        {
            if (raycastSource == null) raycastSource = transform;

            var updateInterval = TimeSpan.FromMilliseconds(checkInteractionTimerMs);
            Observable
                .Timer(updateInterval)
                .Repeat()
                .Subscribe(_ => UpdateInteractable())
                .AddTo(this);
        }

        private void UpdateInteractable()
        {
            var raycast = Physics.Raycast(
                raycastSource.position,
                raycastSource.forward,
                out var hit,
                interactionMaxDistance
            );

            if (!raycast || !hit.transform.TryGetComponent<IInteractable>(out var interactable) ||
                interactable.GetData().InteractableDistance - hit.distance < 0)
            {
                hasInteractableSubject.Value = false;
                return;
            }

            interactableSubject.OnNext(interactable);
            hasInteractableSubject.Value = true;
        }

        public IObservable<IInteractable> GetInteractableFlow() => interactableSubject.DistinctUntilChanged();

        public IObservable<bool> GetHasInteractableFlow() => hasInteractableSubject.DistinctUntilChanged();
    }
}