using Features.Interaction.domain;
using UnityEngine;

namespace Features.Interaction.presentation
{
    public class OutlineInteractable : BaseInteractable
    {
#if OUTLINE
        [SerializeField] private Outline outline;
#endif
        [SerializeField] private Color interactableOutlineColor = Color.cyan;
        [SerializeField] private Color notInteractableOutlineColor = Color.gray;

        public override void OnSelectedStateChanged(IInteractable.SelectedState state)
        {
            base.OnSelectedStateChanged(state);
#if OUTLINE
            switch (state)
            {
                case IInteractable.SelectedState.NotSelected:
                    outline.enabled = false;
                    break;
                case IInteractable.SelectedState.Selected:
                    outline.enabled = true;
                    outline.OutlineColor = notInteractableOutlineColor;
                    break;
                case IInteractable.SelectedState.SelectedInteractable:
                    outline.enabled = true;
                    outline.OutlineColor = interactableOutlineColor;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
#endif
        }
    }
}