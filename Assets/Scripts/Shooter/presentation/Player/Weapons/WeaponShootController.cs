using System;
using System.Collections;
using DG.Tweening;
using JetBrains.Annotations;
using Shooter.domain.Model;
using Shooter.domain.Repositories;
using UniRx;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Shooter.presentation.Player.Weapons
{
    public class WeaponShootController : PlayerBehavior
    {
        [Inject] private IGameActionsRepository actionsRepository;
        [Inject] private SimpleBulletPool bulletPool;

        [Header("Flame")] [SerializeField] private Transform shootEffect;
        [SerializeField] private float flameDuration = 0.1f;
        [SerializeField] private float flameScale = 0.2f;

        [Header("Bullet")] [SerializeField] private Transform bulletRoot;

        [Header("Recoil")] [SerializeField] private Transform recoilRoot;
        [SerializeField] private AnimationCurve recoilCurve;
        [SerializeField] private Vector2 recoilAngleRange = new(5f, 15f);
        [SerializeField] private Vector2 recoilOffsetRange = new(0.1f, 0.2f);
        [SerializeField] private Vector2 recoilDurationRange = new(0.05f, 0.15f);

        [Header("Audio")] [SerializeField] private float minSoundDelay = 0.5f;
        [SerializeField] private AudioSource source;

        private IDisposable handler = Disposable.Empty;
        [CanBeNull] private Coroutine recoilCoroutine;

        private void OnEnable()
        {
            if (!GetPlayerId(out var playerId))
                return;

            handler = GetShootFlow(playerId)
                .Select(action => action.direction.Pos)
                .Subscribe(Shoot)
                .AddTo(this);
        }

        private void OnDisable()
        {
            handler.Dispose();
            AbortRecoil();
        }

        private IObservable<ActionShoot> GetShootFlow(long playerId) => actionsRepository
            .Shoots
            .Where(shoot => shoot.shooterId == playerId);

        private IEnumerator RecoilCoroutine()
        {
            var currentPos = recoilRoot.localPosition;
            var currentRot = recoilRoot.localRotation;
            var targetPos = -Vector3.forward * Random.Range(recoilOffsetRange.x, recoilOffsetRange.y);
            var targetRotAngle = Random.Range(recoilAngleRange.x, recoilAngleRange.y);
            var targetRot = Quaternion.Euler(targetRotAngle, 0f, 0f);
            var duration = Random.Range(recoilDurationRange.x, recoilDurationRange.y);
            var timer = duration;
            while (timer > 0f)
            {
                var time = timer / duration;
                if (time > 0.5f)
                {
                    time = 1f - time;
                    currentPos = Vector3.zero;
                    currentRot = Quaternion.identity;
                }

                var interpolation = recoilCurve.Evaluate(time * 2);
                recoilRoot.localPosition = Vector3.Lerp(currentPos, targetPos, interpolation);
                recoilRoot.localRotation = Quaternion.Slerp(currentRot, targetRot, interpolation);
                timer -= Time.deltaTime;
                yield return null;
            }
        }

        private float lastShotTime = 0f;

        private void Shoot(Vector3 direction)
        {
            if (Time.time - lastShotTime > minSoundDelay)
            {
                lastShotTime = Time.time;
                source.Play();
            }

            var sequence = DOTween.Sequence();
            sequence.Append(shootEffect.DOScale(Vector3.one * flameScale, flameDuration));
            sequence.Append(shootEffect.DOScale(Vector3.zero, flameDuration));
            sequence.Play();

            var bul = bulletPool.Pop();
            var rootPos = bulletRoot.position;
            var rotVector = direction - rootPos;
            var rotation = Quaternion.LookRotation(rotVector);
            bul.ReturnToPool = () => bulletPool.Return(bul);
            bul.Reset(rootPos, rotation);

            AbortRecoil();
            StartCoroutine(RecoilCoroutine());
        }

        private void AbortRecoil()
        {
            if (recoilCoroutine == null)
                return;

            StopCoroutine(recoilCoroutine);
        }
    }
}