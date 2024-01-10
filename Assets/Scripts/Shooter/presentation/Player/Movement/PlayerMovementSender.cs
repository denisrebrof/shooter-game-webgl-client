using System;
using Shooter.domain;
using Shooter.domain.Model;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using Utils.WebSocketClient.domain;
using Utils.WebSocketClient.domain.model;
using Zenject;

namespace Shooter.presentation.Player.Movement
{
    public class PlayerMovementSender : MonoBehaviour
    {
        [Inject(Id = IWSCommandsUseCase.AuthorizedInstance)]
        private IWSCommandsUseCase commandsUseCase;

        [SerializeField] private float syncPeriod = 0.1f;
        [SerializeField] private Transform lookAngleSource;
        [SerializeField] private Transform target;


        private float timer = 0f;

        private SubmitPositionRequestData testData = new SubmitPositionRequestData(
            Vector3.one.GetSnapshot(), 45f
        );

        private SubmitPositionRequestData CurrentRequestData => new(
            pos: target.GetSnapshot(),
            verticalLookAngle: lookAngleSource.rotation.eulerAngles.x
        );

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

        private void SendSnapshot() => commandsUseCase
            .Request<long, SubmitPositionRequestData>(Commands.IntentSubmitPosition, CurrentRequestData)
            .Subscribe()
            .AddTo(this);

        [Serializable]
        private struct SubmitPositionRequestData
        {
            public TransformSnapshot pos;
            public float verticalLookAngle;

            public SubmitPositionRequestData(TransformSnapshot pos, float verticalLookAngle)
            {
                this.pos = pos;
                this.verticalLookAngle = verticalLookAngle;
            }
        }
    }
}