using Features.Interaction.domain;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;
using Zenject;
using static Features.Interaction.domain.IInteractable.SelectedState;

namespace Features.Interaction.presentation
{
    public class InteractionHandler : MonoBehaviour
    {
        [Inject] private ISelectedInteractableRepository interactableRepository;
        [CanBeNull] private IInteractable lastInteractable = null;
        private bool hasInteractable = false;
        private bool isInteractable = false;

        private void Start()
        {
            interactableRepository
                .GetInteractableFlow()
                .Do(_ =>
                {
                    lastInteractable?.OnSelectedStateChanged(NotSelected);
                    lastInteractable = _;
                })
                .Select(_ => _.IsInteractableFlow())
                .Switch()
                .Subscribe(_ =>
                {
                    isInteractable = _;
                    UpdateInteractable();
                })
                .AddTo(this);
            interactableRepository.GetHasInteractableFlow().Subscribe(_ =>
            {
                hasInteractable = _;

                UpdateInteractable();
            }).AddTo(this);
        }

        private void UpdateInteractable()
        {
            if (hasInteractable)
                lastInteractable?.OnSelectedStateChanged(isInteractable ? SelectedInteractable : Selected);
            else
                lastInteractable?.OnSelectedStateChanged(NotSelected);
        }

        private void Update()
        {
            if (!hasInteractable || lastInteractable == null) return;
            if (!Input.GetKeyDown(lastInteractable.GetData().InteractionKey)) return;
            if (!isInteractable) return;
            lastInteractable.Interact();
        }
    }
}