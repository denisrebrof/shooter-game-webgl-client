using System;
using Core.SDK.GameState;
using Shooter.domain;
using UniRx;
using UnityEngine;
using Zenject;

namespace Shooter.presentation.UI.Pause
{
    public class PauseMenuController : MonoBehaviour, IPausedStateHandler
    {
        [Inject] private GameStateNavigator gameStateNavigator;
        [SerializeField] private Canvas canvas;

        private readonly BehaviorSubject<bool> pausedSubject = new(false);
        private bool paused;

        public IObservable<bool> GetPausedStateFlow() => pausedSubject;

        private void Start()
        {
            ApplyPausedMode();
            Observable
                .EveryUpdate()
                .Where(_ => Input.GetKeyDown(KeyCode.Escape))
                .Subscribe(_ => SwitchPausedMode())
                .AddTo(this);
        }

        private void SwitchPausedMode()
        {
            paused = !paused;
            pausedSubject.OnNext(paused);
            ApplyPausedMode();
        }

        private void ApplyPausedMode()
        {
            Cursor.lockState = paused ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = paused;
            canvas.enabled = paused;
            gameStateNavigator.SetMenuShownState(paused);
        }
    }
}