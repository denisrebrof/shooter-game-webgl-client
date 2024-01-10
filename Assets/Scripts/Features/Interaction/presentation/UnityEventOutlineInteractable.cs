using UnityEngine;
using UnityEngine.Events;

namespace Features.Interaction.presentation
{
    public class UnityEventOutlineInteractable : OutlineInteractable
    {
        [SerializeField] private UnityEvent interaction;

        protected override void Interaction()
        {
            base.Interaction();
            interaction?.Invoke();
        }
    }
}