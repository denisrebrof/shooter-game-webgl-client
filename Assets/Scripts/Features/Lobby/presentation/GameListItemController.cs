using System;
using Features.Lobby.domain.model;
using JetBrains.Annotations;
using Michsky.MUIP;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using Utils.Pooling;

namespace Features.Lobby.presentation
{
    public class GameListItemController : MonoPoolItem
    {
        [SerializeField] private TMP_Text title;
        [SerializeField] private TMP_Text participantsCounter;
        [SerializeField] private ButtonManager joinButton;
        [SerializeField] private LocalizedString joinText;
        [SerializeField] private LocalizedString fullText;
        [SerializeField] private LocalizedString gamePrefixText;

        [CanBeNull] private Action onClickJoinAction;

        private void Awake() => joinButton
            .onClick
            .AddListener(() => onClickJoinAction?.Invoke());

        public void Setup(GameListItemData data, Action onClickJoin)
        {
            title.text = gamePrefixText.GetLocalizedString() + " # " + TruncateLongString(data.matchId, 6);
            participantsCounter.text = $"{data.currentParticipants}/{data.maxParticipants}";
            var isFull = data.currentParticipants >= data.maxParticipants;
            var buttonText = isFull ? fullText : joinText;
            joinButton.buttonText = buttonText.GetLocalizedString();
            joinButton.Interactable(!isFull);
            onClickJoinAction = onClickJoin;
        }

        private static string TruncateLongString(string str, int maxLength) => string.IsNullOrEmpty(str)
            ? str
            : str[..Math.Min(str.Length, maxLength)];
    }
}