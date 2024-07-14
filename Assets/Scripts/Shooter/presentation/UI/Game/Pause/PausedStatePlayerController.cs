using UniRx;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Shooter.presentation.UI.Game.Pause
{
    public class PausedStatePlayerController : MonoBehaviour
    {
        [SerializeField] private UnityEvent pause;
        [SerializeField] private UnityEvent unpause;
        [Inject] private IPausedStateHandler pausedStateHandler;

        private bool paused;
        private bool needUpdateInput;

        private void Start()
        {
            pausedStateHandler
                .GetPausedStateFlow()
                .Subscribe(UpdatePausedState)
                .AddTo(this);
        }

        private void UpdatePausedState(bool paused)
        {
            this.paused = paused;
            if (paused)
                pause.Invoke();
            else
                unpause.Invoke();
        }

        private void OnEnable() => needUpdateInput = true;

        private void Update()
        {
            if (!needUpdateInput)
                return;

            if (paused)
                pause.Invoke();
            else
                unpause.Invoke();
            needUpdateInput = false;
        }
    }
}