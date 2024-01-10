using UniRx;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Shooter.presentation.UI.Pause
{
    public class PausedStatePlayerController : MonoBehaviour
    {
        [Inject] private IPausedStateHandler pausedStateHandler;

        [SerializeField] private UnityEvent pause;
        [SerializeField] private UnityEvent unpause;

        private void Start() => pausedStateHandler
            .GetPausedStateFlow()
            .Subscribe(UpdatePausedState)
            .AddTo(this);

        private void UpdatePausedState(bool paused)
        {
            if (paused)
                pause.Invoke();
            else
                unpause.Invoke();
        }
    }
}