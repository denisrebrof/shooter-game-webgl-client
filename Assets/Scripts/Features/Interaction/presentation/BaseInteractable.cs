using System;
using System.Collections;
using Features.Interaction.domain;
using UniRx;
using UnityEngine;
using static Features.Interaction.domain.IInteractable.SelectedState;

namespace Features.Interaction.presentation
{
    public class BaseInteractable : MonoBehaviour, IInteractable
    {
        [Header("Data"), SerializeField] private InteractableData data;

        [Header("Settings")] [SerializeField] private bool interactOnce = true;
        [SerializeField] private float cooldown = 1f;

        private readonly ReactiveProperty<bool> onCooldown = new(false);

        private bool firstInteraction = false;

        public virtual IObservable<bool> IsInteractableFlow() => onCooldown.Select(GetIsInteractable);
        protected bool IsInteractable => GetIsInteractable(onCooldown.Value);

        protected virtual void Awake() => OnSelectedStateChanged(NotSelected);


        public void Interact()
        {
            StartCoroutine(CooldownCoroutine());
            Interaction();
        }

        protected virtual void Interaction() => firstInteraction = true;

        public virtual void OnSelectedStateChanged(IInteractable.SelectedState state)
        {
            //do nothing
        }

        public InteractableData GetData() => data;

        private bool GetIsInteractable(bool isOnCooldown) => !isOnCooldown && !(interactOnce && firstInteraction);

        private IEnumerator CooldownCoroutine()
        {
            onCooldown.Value = true;
            yield return new WaitForSeconds(cooldown);
            onCooldown.Value = false;
        }
    }
}