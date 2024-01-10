using UnityEngine;
using UnityEngine.Events;

namespace Utils.PlayerTrigger
{
    public class UnityEventPlayerTrigger : PlayerTriggerBase
    {
        [SerializeField] private UnityEvent triggerEvent;
        [SerializeField] private UnityEvent exitTriggerEvent;

        protected override void OnPlayerEntersTrigger() => triggerEvent?.Invoke();

        protected override void OnPlayerExitTrigger() => exitTriggerEvent?.Invoke();
    }
}