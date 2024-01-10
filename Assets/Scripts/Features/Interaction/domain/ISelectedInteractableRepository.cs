using System;

namespace Features.Interaction.domain
{
    public interface ISelectedInteractableRepository
    {
        public IObservable<IInteractable> GetInteractableFlow();
        public IObservable<bool> GetHasInteractableFlow();
    }
}