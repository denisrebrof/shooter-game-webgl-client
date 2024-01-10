using TMPro;
using UniRx;
using UnityEngine;
using Utils.WebSocketClient.domain;
using Utils.WebSocketClient.domain.model;
using Zenject;

namespace Core.User.presentation
{
    public class UserNameText : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;

        [Inject(Id = IWSCommandsUseCase.AuthorizedInstance)]
        private IWSCommandsUseCase commandsUseCase;

        private void Start() => commandsUseCase
            .Subscribe<string>(Commands.GetUsername)
            .Subscribe(userName => text.text = userName)
            .AddTo(this);
    }
}