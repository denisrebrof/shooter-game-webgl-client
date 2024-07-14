using Shooter.domain;
using Shooter.presentation.Camera;
using UniRx;
using UnityEngine;
using Zenject;

namespace Shooter.presentation.Player
{
    public class CurrentPlayerKilledStateObserver : MonoBehaviour
    {
        [SerializeField] private Transform playerCam;
        [SerializeField] private Transform character;
        [Inject] private CurrentPlayerAliveStateUpdatesUseCase aliveStateUpdatesUseCase;

        [Inject] private KilledCameraController killedCameraController;

        private void Start()
        {
            aliveStateUpdatesUseCase
                .GetAliveStateUpdatesFlow()
                .Subscribe(OnAliveStateUpdate)
                .AddTo(this);
        }

        private void OnAliveStateUpdate(AliveStateUpdate update)
        {
            var pos = update.Position.Pos;
            var rot = Quaternion.Euler(0f, update.Position.r, 0f);
            character.position = pos;
            character.rotation = rot;
            gameObject.SetActive(update.Alive);
            if (update.Alive) killedCameraController.StopFlying();
            else killedCameraController.FlyAway(playerCam);
        }
    }
}