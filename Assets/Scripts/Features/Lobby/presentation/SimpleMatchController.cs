using System.Diagnostics.CodeAnalysis;
using ModestTree;
using UniRx;
using UnityEngine;
using Utils.WebSocketClient.domain;
using Utils.WebSocketClient.domain.model;
using Zenject;

namespace Features.Lobby.presentation
{
    public class SimpleMatchController : MonoBehaviour
    {
        [Inject(Id = IWSCommandsUseCase.AuthorizedInstance)] private IWSCommandsUseCase commandsUseCase;
        [SerializeField] private RectTransform lobbyRootUI;
        [SerializeField] private GameObject matchRoot;

        private void Start()
        {
            matchRoot.SetActive(false);
            lobbyRootUI.gameObject.SetActive(true);
            commandsUseCase
                .Subscribe<MatchData>(Commands.MatchState)
                .Subscribe(HandleMatch)
                .AddTo(this);
        }

        private void HandleMatch(MatchData matchData)
        {
            var isRealMatch = !matchData.id.IsEmpty();
            matchRoot.SetActive(isRealMatch);
            lobbyRootUI.gameObject.SetActive(!isRealMatch);
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private struct MatchData
        {
            public string id;
            public long createdTime;
            public long[] participantIds;
        }
    }
}