using UniRx;
using UnityEngine;
using Utils.WebSocketClient.domain;
using Utils.WebSocketClient.domain.model;
using Zenject;

public class PlayerBotHitHandler : MonoBehaviour
{
    [Inject(Id = IWSCommandsUseCase.AuthorizedInstance)]
    private IWSCommandsUseCase commandsUseCase;

    public void HandleHit(long botId) =>
        commandsUseCase
            .Request<bool, long>(Commands.HitByBot, botId)
            .Subscribe()
            .AddTo(this);
}
