using System;
using Features.Interaction.domain;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

namespace Features.Interaction.presentation
{
    public class InteractionView : MonoBehaviour
    {
        [SerializeField] private UnityEvent showInteraction;
        [SerializeField] private UnityEvent hideInteraction;
        [SerializeField] private Image interaction;
        [SerializeField] private Text description;
        [SerializeField] private Text key;

        [Inject] private ISelectedInteractableRepository selectedInteractableRepository;

        private void Start()
        {
            selectedInteractableRepository
                .GetHasInteractableFlow()
                .Subscribe(SetInteractionMarkVisible)
                .AddTo(this);

            selectedInteractableRepository
                .GetInteractableFlow()
                .Select(GetStateFlow)
                .Switch()
                .Subscribe(UpdateData)
                .AddTo(this);
        }

        private void SetInteractionMarkVisible(bool visible)
        {
            var currentEvent = visible ? showInteraction : hideInteraction;
            currentEvent?.Invoke();
        }

        private void UpdateData(InteractableState state)
        {
            var color = state.IsInteractable ? Color.white : Color.gray;
            interaction.color = color;
            description.color = color;
            key.color = color;

            interaction.sprite = state.Data.Sprite;
            description.text = state.Data.Text;
            key.text = state.Data.InteractionKey.ToString("g");
        }

        private static IObservable<InteractableState> GetStateFlow(IInteractable interactable) => interactable
            .IsInteractableFlow()
            .Select(isInteractable => new InteractableState
            {
                Data = interactable.GetData(),
                IsInteractable = isInteractable
            });

        private struct InteractableState
        {
            public InteractableData Data;
            public bool IsInteractable;
        }
    }
}