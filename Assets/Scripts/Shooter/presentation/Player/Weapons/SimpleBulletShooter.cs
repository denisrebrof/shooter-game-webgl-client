using System.Collections;
using DG.Tweening;
using Shooter.domain;
using UnityEngine;
using Zenject;

namespace Shooter.presentation.Player.Weapons
{
    public class SimpleBulletShooter : MonoBehaviour
    {
        [Inject] private ShootPlayerUseCase shootPlayerUseCase;
        [Inject] private SimplePlayerBulletPool bulletPool;
        
        [SerializeField] private long weaponId;
        [SerializeField] private float sec = 0.1f;
        [SerializeField] private Transform shooter;
        [SerializeField] private Transform shootEffect;

        private bool isShooting = false;

        private void OnEnable()
        {
            StartCoroutine(FireCoroutine());
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        private IEnumerator FireCoroutine()
        {
            while (true)
            {
                if (isShooting)
                {
                    Shoot();
                }

                yield return new WaitForSeconds(sec);
            }
        }

        private void Shoot()
        {
            var shooterPos = shooter.position;
            var bul = bulletPool.Pop();
            bul.weaponId = weaponId;
            bul.ReturnToPool = () => bulletPool.Return(bul);
            bul.Reset(shooter);
            var sequence = DOTween.Sequence();
            sequence.Append(shootEffect.DOScale(Vector3.one * 0.2f, 0.1f));
            sequence.Append(shootEffect.DOScale(Vector3.zero, 0.1f));
            sequence.Play();
            shootPlayerUseCase.Shoot(shooterPos, shooter.forward, weaponId);
        }

        private void Update()
        {
            isShooting = Input.GetMouseButton(0);
        }
    }
}