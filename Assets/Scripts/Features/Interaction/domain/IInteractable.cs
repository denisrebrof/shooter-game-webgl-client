using System;

namespace Features.Interaction.domain
{
    public interface IInteractable
    {
        public IObservable<bool> IsInteractableFlow();
        public void Interact();
        public InteractableData GetData();
        public void OnSelectedStateChanged(SelectedState state);
        
        public enum SelectedState
        {
            NotSelected,
            Selected,
            SelectedInteractable,
        }
    }
}
