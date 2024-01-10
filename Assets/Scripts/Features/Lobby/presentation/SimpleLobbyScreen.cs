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
        [SerializeField] private Toggle autoJoinToggle;
        [SerializeField] private Button exitLobbyButton;
        [SerializeField] private Button openGamesButton;

        private IDisposable stateHandler = Disposable.Empty;

        private Coroutine autoJoinCoroutine;

        private bool lobbyState;

        private void Awake()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            enterLobbyButton.onClick.AddListener(JoinLobby);
            exitLobbyButton.onClick.AddListener(LeaveLobby);
            autoJoinToggle.isOn = AutoJoinUseCase.Instance.AutoJointSetting;
            autoJoinToggle.onValueChanged.AddListener(isOn => AutoJoinUseCase.Instance.AutoJointSetting = isOn);
        }

        private void OnEnable()
        {
            HandleLobbyState(false);
            stateHandler = lobbyUseCase
                .GetLobbyStateFlow()
                .Subscribe(HandleLobbyState);
            autoJoinCoroutine = StartCoroutine(AutoJoinCoroutine());
        }

        private void OnDisable()
        {
            stateHandler.Dispose();
            if (autoJoinCoroutine == null)
                return;

            StopCoroutine(autoJoinCoroutine);
        }

        private void HandleLobbyState(bool inLobby)
        {
            loader.gameObject.SetActive(inLobby);
            loaderStub.gameObject.SetActive(!inLobby);
            enterLobbyButton.interactable = !inLobby;
            enterLobbyButtonSearchText.SetActive(inLobby);
            enterLobbyButtonIdleText.SetActive(!inLobby);
            exitLobbyButton.gameObject.SetActive(inLobby);
            openGamesButton.gameObject.SetActive(!inLobby);
            lobbyState = inLobby;
        }

        private void JoinLobby() => lobbyUseCase
            .JoinLobby()
            .AddTo(this);

        private void LeaveLobby() => lobbyUseCase
            .LeaveLobby()
            .AddTo(this);

        private IEnumerator AutoJoinCoroutine()
        {
            yield return new WaitForSeconds(0.5f);
            if (lobbyState || !AutoJoinUseCase.Instance.AutoJoinAvailable)
                yield break;

            JoinLobby();
        }

        private void OnDestroy() => StopAllCoroutines();
    }
}