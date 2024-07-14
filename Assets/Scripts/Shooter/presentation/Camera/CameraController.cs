using System;
using Shooter.domain;
using Shooter.domain.Model;
using Shooter.domain.Repositories;
using UniRx;
using UnityEngine;
using Zenject;

namespace Shooter.presentation.Camera
{
    public class CameraController : MonoBehaviour
    {
        [Inject] private ICameraNavigator cameraNavigator;
        [Inject] private IGameStateRepository gameStateRepository;
        [Inject] private CurrentPlayerStateUseCase playerStateUseCase;

        private IObservable<bool> PlayerAliveFlow => playerStateUseCase
            .GetStateFlow()
            .Select(data => data.alive);

        private IObservable<GameStateTypes> StateTypeFlow => gameStateRepository
            .state
            .Select(state => state.Type);

        private void Start() => Observable
            // ReSharper disable once InvokeAsExtensionMethod
            .CombineLatest(StateTypeFlow, PlayerAliveFlow, GetCameraType)
            .Subscribe(cameraNavigator.SetActiveCam)
            .AddTo(this);

        private CameraType GetCameraType(GameStateTypes type, bool alive)
        {
            if (type == GameStateTypes.Finished)
                return CameraType.Finished;

            if (type == GameStateTypes.Pending)
                return CameraType.Overview;

            return alive ? CameraType.Player : CameraType.Killed;
        }
    }
}