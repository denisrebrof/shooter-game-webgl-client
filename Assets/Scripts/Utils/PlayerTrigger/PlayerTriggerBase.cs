using UnityEngine;

namespace Utils.PlayerTrigger
{
    public abstract class PlayerTriggerBase : MonoBehaviour
    {
        [SerializeField] private string triggerTag = "Player";

        [SerializeField] private bool triggerOnce = false;
        private bool triggeredEnter;
        private bool triggeredExit;

        private bool activeState;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(triggerTag) || (triggerOnce && triggeredEnter))
                return;

            OnPlayerEntersTrigger();
            activeState = true;
            triggeredEnter = true;
        }

        protected virtual void OnDestroy()
        {
            if (activeState)
                ExitTrigger();
        }

        private void OnDisable()
        {
            if (activeState)
                ExitTrigger();
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag(triggerTag) || (triggerOnce && triggeredExit))
                return;

            ExitTrigger();
            activeState = false;
            triggeredExit = true;
        }

        private void ExitTrigger() => OnPlayerExitTrigger();

        protected abstract void OnPlayerEntersTrigger();
        protected abstract void OnPlayerExitTrigger();
    }
}