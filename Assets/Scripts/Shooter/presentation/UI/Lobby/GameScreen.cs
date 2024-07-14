using System;
using Core.Sound.presentation;
using Features.Lobby.domain;
using Michsky.MUIP;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Localization;
using Zenject;

namespace Shooter.presentation.UI.Lobby
{
    public class GameScreen : MonoBehaviour
    {
        [Inject] private LobbyUseCase lobbyUseCase;
        [Inject] private PlaySoundNavigator playSoundNavigator;

        [SerializeField] private LocalizedString readyForSearchText;
        [SerializeField] private LocalizedString searchingText;
        [SerializeField] private LocalizedString enterLobbyButtonStopText;
        [SerializeField] private LocalizedString enterLobbyButtonSearchText;
        [SerializeField] private GameObject loader;
        [SerializeField] private TMP_Text statusText;
        [SerializeField] private ButtonManager lobbyButton;
        [SerializeField] private ButtonManager openGamesButton;

        private IDisposable stateHandler = Disposable.Empty;

        private bool lobbyState;

        private void Awake() {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            lobbyButton.onClick.AddListener(SwitchLobbyState);
        }

        private void OnEnable() {
            HandleLobbyState(false);
            stateHandler = lobbyUseCase
                .GetLobbyStateFlow()
                .Subscribe(HandleLobbyState);
        }

        private void OnDisable() => stateHandler.Dispose();

        private void HandleLobbyState(bool inLobby) {
            lobbyState = inLobby;
            loader.gameObject.SetActive(inLobby);
            SetLocalizedText(inLobby ? searchingText : readyForSearchText, text =>
            {
                if (statusText != null) statusText.text = text;
            });
            SetLocalizedText(inLobby ? enterLobbyButtonStopText : enterLobbyButtonSearchText, text =>
            {
                if (lobbyButton != null) lobbyButton.SetText(text);
            });
            openGamesButton.Interactable(!inLobby);
        }

        private static void SetLocalizedText(
            LocalizedString key,
            Action<string> handle
        ) => key
            .GetLocalizedStringAsync()
            .Completed += result => handle.Invoke(result.Result);

        private void SwitchLobbyState() {
            if (lobbyState) LeaveLobby();
            else JoinLobby();
        }

        private void JoinLobby() {
            playSoundNavigator.Play(SoundType.ButtonOk);
            lobbyUseCase
                .JoinLobby()
                .AddTo(this);
        }

        private void LeaveLobby() {
            playSoundNavigator.Play(SoundType.ButtonCancel);
            lobbyUseCase
                .LeaveLobby()
                .AddTo(this);
        }
    }
}