using System;
using Features.Lobby.domain.model;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils.Pooling;

namespace Features.Lobby.presentation
{
    public class GameListItemController : MonoPoolItem
    {
        [SerializeField] private TMP_Text title;
        [SerializeField] private TMP_Text participantsCounter;
        [SerializeField] private GameObject gameFullIndicator;
        [SerializeField] private Button joinButton;

        [CanBeNull] private Action onClickJoinAction;

        private void Awake() => joinButton
            .onClick
            .AddListener(() => onClickJoinAction?.Invoke());

        public void Setup(GameListItemData data, Action onClickJoin)
        {
            title.text = "Match # " + TruncateLongString(data.matchId, 6);
            participantsCounter.text = $"{data.currentParticipants}/{data.maxParticipants}";
            var isFull = data.currentParticipants >= data.maxParticipants;
            gameFullIndicator.SetActive(isFull);
            joinButton.gameObject.SetActive(!isFull);
            onClickJoinAction = onClickJoin;
        }

        private static string TruncateLongString(string str, int maxLength) => string.IsNullOrEmpty(str)
            ? str
            : str[..Math.Min(str.Length, maxLength)];
    }
}