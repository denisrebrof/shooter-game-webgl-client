using System;
using Core.Auth.domain;
using Shooter.domain;
using Shooter.domain.Repositories;
using UniRx;
using UnityEngine;
using Zenject;

namespace Shooter.presentation.Camera
{
    public class KilledCameraController : MonoBehaviour
    {
        [SerializeField] private Transform camTransform;

        [SerializeField] private float killerDistance = 2f;
        [SerializeField] private float flyUpHeight;
        [SerializeField] private float flyAwayForce;
        [SerializeField] private float rotateForce;

        private readonly BehaviorSubject<long> killerId = new(-1);
        [Inject] private IGameActionsRepository actionsRepository;
        [Inject] private CurrentPlayerAliveStateUpdatesUseCase aliveStateUpdatesUseCase;
        [Inject] private IAuthRepository authRepository;

        private IDisposable flyAwayHandler = Disposable.Empty;
        [Inject] private GamePlayerUseCase playerUseCase;

        private void Awake()
        {
            var currentUserId = Convert.ToInt64(authRepository.LoginUserId);
            actionsRepository
                .Hits
                .Where(hit => hit.killed && hit.receiverId == currentUserId)
                .Subscribe(hit => killerId.OnNext(hit.damagerId))
                .AddTo(this);
            aliveStateUpdatesUseCase
                .GetAliveStateUpdatesFlow()
                .Where(update => update.Alive)
                .Subscribe(_ => killerId.OnNext(-1))
                .AddTo(this);
        }

        public void FlyAway(Transform playerCam)
        {
            var playerCamPos = playerCam.position;
            camTransform.position = playerCamPos;
            camTransform.rotation = playerCam.rotation;
            var defaultKillerPos = playerCamPos + playerCam.forward * 10f;
            flyAwayHandler = GetKillerPosition(defaultKillerPos)
                .Select(killerPos => Observable.EveryUpdate().Select(_ => killerPos))
                .Switch()
                .Subscribe(FlyAway)
                .AddTo(this);
        }

        public void StopFlying()
        {
            killerId.OnNext(-1);
            flyAwayHandler.Dispose();
        }

        private void FlyAway(Vector3 killer)
        {
            var camPos = camTransform.position;
            var toCam = camPos - killer;
            var targetPos = killer + new Vector3(toCam.x, 0, toCam.y).normalized * killerDistance +
                            Vector3.up * flyUpHeight;
            camTransform.position = Vector3.Lerp(camPos, targetPos, Time.deltaTime * flyAwayForce);
            var targetRot = Quaternion.LookRotation(killer - camPos, Vector3.up);
            camTransform.rotation = Quaternion.Slerp(camTransform.rotation, targetRot, Time.deltaTime * rotateForce);
        }

        private IObservable<Vector3> GetKillerPosition(Vector3 defaultPos)
        {
            return killerId
                .Select(id => GetPlayerPosition(id, defaultPos))
                .Switch();
        }

        private IObservable<Vector3> GetPlayerPosition(long playerId, Vector3 defaultPos)
        {
            if (playerId < 0)
                return Observable.Return(defaultPos);

            return playerUseCase
                .GetPlayerState(playerId)
                .Select(state => state.pos.Pos);
        }
    }
}