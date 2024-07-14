using System;
using Core.SDK.GameState;
using Core.Sound.presentation;
using Michsky.MUIP;
using Shooter.domain;
using UniRx;
using UnityEngine;
using Zenject;

namespace Shooter.presentation.UI.Game.Pause
{
    public class PauseMenuController : MonoBehaviour, IPausedStateHandler
    {
        [SerializeField] private ButtonManager leaveButton;
        [SerializeField] private Canvas canvas;

        private readonly BehaviorSubject<bool> pausedSubject = new(false);
        [Inject] private LeaveMatchUseCase leaveMatchUseCase;
        [Inject] private GameStateNavigator gameStateNavigator;
        [Inject] private PlaySoundNavigator playSoundNavigator;
        private bool paused;

        private void Start()
        {
            ApplyPausedMode();
            Observable
                .EveryUpdate()
                .Where(_ => Input.GetKeyDown(KeyCode.Escape))
                .Subscribe(_ => SwitchPausedMode())
                .AddTo(this);
            leaveButton.onClick.AddListener(Leave);
        }
        
        private void Leave()
        {
            playSoundNavigator.Play(SoundType.Warning);
            leaveMatchUseCase.Leave().AddTo(this);
        }

        public IObservable<bool> GetPausedStateFlow() => pausedSubject;

        private void SwitchPausedMode()
        {
            paused = !paused;
            pausedSubject.OnNext(paused);
            ApplyPausedMode();
            playSoundNavigator.Play(SoundType.ButtonDefault);
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