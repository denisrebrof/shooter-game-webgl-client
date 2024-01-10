using System;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using Utils.Pooling;

namespace Shooter.presentation.Player.Weapons
{
    public class SimpleBullet : MonoPoolItem
    {
        [SerializeField] private Transform bullet;
        [SerializeField] private float lifetime = 10f;
        [SerializeField] private float speed = 1f;

        [CanBeNull] public Action ReturnToPool;

        private Vector3 prevPos;

        private void OnDisable() => StopAllCoroutines();

        public void Reset(Transform pos) => Reset(pos.position, pos.rotation);

        public void Reset(Vector3 pos, Quaternion rot)
        {
            bullet.position = pos;
            bullet.rotation = rot;
            prevPos = pos;
            StartCoroutine(Lifecycle());
        }

        private IEnumerator Lifecycle()
        {
            var timer = lifetime;
            while (timer > 0)
            {
                var frameSpeed = speed * Time.deltaTime;
                bullet.Translate(Vector3.forward * frameSpeed);
                CheckHit();
                prevPos = bullet.position;
                timer -= Time.deltaTime;
                yield return null;
            }
            ReturnToPool?.Invoke();
        }

        private void CheckHit()
        {
            var shift = bullet.position - prevPos;
            var ray = new Ray(prevPos, shift);
            if (!Physics.Raycast(ray, out var hit, shift.magnitude))
                return;

            HandleHit(hit);
            IImpactNavigator.Instance.AddImpact(hit.point, hit.normal);
            ReturnToPool?.Invoke();
        }

        protected virtual void HandleHit(RaycastHit hit)
        {
            //Empty
        }
    }
}