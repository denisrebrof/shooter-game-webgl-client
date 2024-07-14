using System;
using System.Collections;
using Features.Lobby.domain;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Features.Lobby.presentation
{
    public class SimpleLobbyScreen : MonoBehaviour
    {
        [Inject] private LobbyUseCase lobbyUseCase;

        [SerializeField] private RectTransform loader;
        [SerializeField] private RectTransform loaderStub;
        [SerializeField] private Button enterLobbyButton;
        [SerializeField] private GameObject enterLobbyButtonSearchText;
        [SerializeField] private GameObject enterLobbyButtonIdleText;
        [SerializeField] private Button exitLobbyButton;
        [SerializeField] private Button openGamesButton;

        private IDisposable stateHandler = Disposable.Empty;

        private void Awake()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            enterLobbyButton.onClick.AddListener(JoinLobby);
            exitLobbyButton.onClick.AddListener(LeaveLobby);
        }

        private void OnEnable()
        {
            HandleLobbyState(false);
            stateHandler = lobbyUseCase
                .GetLobbyStateFlow()
                .Subscribe(HandleLobbyState);
        }

        private void OnDisable() => stateHandler.Dispose();

        private void HandleLobbyState(bool inLobby)
        {
            loader.gameObject.SetActive(inLobby);
            loaderStub.gameObject.SetActive(!inLobby);
            enterLobbyButton.interactable = !inLobby;
            enterLobbyButtonSearchText.SetActive(inLobby);
            enterLobbyButtonIdleText.SetActive(!inLobby);
            exitLobbyButton.gameObject.SetActive(inLobby);
            openGamesButton.gameObject.SetActive(!inLobby);
        }

        private void JoinLobby() => lobbyUseCase
            .JoinLobby()
            .AddTo(this);

        private void LeaveLobby() => lobbyUseCase
            .LeaveLobby()
            .AddTo(this);

        private void OnDestroy() => StopAllCoroutines();
    }
}