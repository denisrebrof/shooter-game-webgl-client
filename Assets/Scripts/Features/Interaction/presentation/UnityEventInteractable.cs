using System;
using Features.Interaction.domain;
using UnityEngine;
using UnityEngine.Events;

namespace Features.Interaction.presentation
{
    public class UnityEventInteractable : BaseInteractable
    {
        [SerializeField] private UnityEvent interaction;
        [SerializeField] private UnityEvent onSelected;
        [SerializeField] private UnityEvent onSelectedInteractable;
        [SerializeField] private UnityEvent onDeselected;

        protected override void Interaction()
        {
            base.Interaction();
            interaction?.Invoke();
        }

        public override void OnSelectedStateChanged(IInteractable.SelectedState state)
        {
            base.OnSelectedStateChanged(state);
            switch (state)
            {
                case IInteractable.SelectedState.NotSelected:
                    onDeselected?.Invoke();
                    break;
                case IInteractable.SelectedState.Selected:
                    onSelected?.Invoke();
                    break;
                case IInteractable.SelectedState.SelectedInteractable:
                    onSelectedInteractable?.Invoke();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }
    }
}