using System;
using Shooter.domain.Model;
using UniRx;
using UnityEngine;
using Utils.WebSocketClient.domain;
using Utils.WebSocketClient.domain.model;
using Zenject;

namespace Shooter.presentation.Player.Movement
{
    public class PlayerMovementSender : MonoBehaviour
    {
        [SerializeField] private float syncPeriod = 0.1f;
        [SerializeField] private InfimaGames.LowPolyShooterPack.Movement movement;
        [SerializeField] private Transform lookAngleSource;
        [SerializeField] private Transform target;

        [Inject(Id = IWSCommandsUseCase.AuthorizedInstance)]
        private IWSCommandsUseCase commandsUseCase;

        private float timer;

        private SubmitPositionRequestData CurrentRequestData => new()
        {
            pos = target.GetSnapshot(),
            verticalLookAngle = lookAngleSource.rotation.eulerAngles.x,
            crouching = movement.IsCrouching(),
            jumping = movement.IsJumping()
        };

        private void Update()
        {
            if (timer > 0f)
            {
                timer -= Time.deltaTime;
                return;
            }

            timer = syncPeriod;
            SendSnapshot();
        }

        private void SendSnapshot()
        {
            commandsUseCase
                .Request<long, SubmitPositionRequestData>(Commands.IntentSubmitPosition, CurrentRequestData)
                .Subscribe()
                .AddTo(this);
        }

        [Serializable]
        private struct SubmitPositionRequestData
        {
            public TransformSnapshot pos;
            public float verticalLookAngle;
            public bool crouching;
            public bool jumping;
        }
    }
}