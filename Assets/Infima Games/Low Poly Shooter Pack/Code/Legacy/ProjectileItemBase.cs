using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using Utils.Pooling;

namespace InfimaGames.LowPolyShooterPack.Legacy
{
    public class ProjectileItemBase : MonoPoolItem
    {
        [SerializeField] private float maxDistance = 100f;
        [SerializeField] private UnityEvent Reset;
        
        [CanBeNull] public Action ReturnToPool;

        private LayerMask mask;

        private Vector3 prevPos;

        private float timer;
        private float speed;
        private float gravitySpeed;

        protected int damage;
        protected long weaponId;

        public Transform Transform => target;

        private const float G = 9.8f * 0.1f;

        private void Awake()
        {
            mask = LayerMask.GetMask("Default");
        }

        public void Setup(
            Vector3 pos,
            Quaternion rot,
            long weaponId,
            int damage,
            float impulse
        )
        {
            this.damage = damage;
            this.weaponId = weaponId;
            target.position = pos;
            target.rotation = rot;
            timer = maxDistance / impulse;
            speed = impulse;
            prevPos = pos;
            Reset.Invoke();
        }

        public override void OnGetFromPool()
        {
            base.OnGetFromPool();
            gravitySpeed = 0f;
        }

        protected virtual void HandleHit(RaycastHit hit)
        {
        }

        [SerializeField] private Vector3 lastRayPos;
        [SerializeField] private Vector3 lastRayDir;

        private void OnDrawGizmos()
        {
            Gizmos.DrawLine(lastRayPos, lastRayPos + lastRayDir);
        }

        private void CheckHit()
        {
            var shift = target.position - prevPos;
            var ray = new Ray(prevPos, shift);
            lastRayPos = ray.origin;
            lastRayDir = shift;
            if (!Physics.Raycast(ray, out var hit, shift.magnitude, mask))
                return;
                
            HandleHit(hit);
            ReturnToPool?.Invoke();
        }

        private void Update()
        {
            
            gravitySpeed += G * Time.deltaTime;
            var frameTranslation = Vector3.forward * speed * Time.deltaTime + Vector3.down * gravitySpeed;
            target.Translate(frameTranslation);
            
            CheckHit();
            
            prevPos = target.position;
            
            timer -= Time.deltaTime;
            if (timer < 0) ReturnToPool?.Invoke();
        }
    }
}