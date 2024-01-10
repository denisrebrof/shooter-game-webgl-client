using System;
using Shooter.domain.Model;
using Shooter.domain.Repositories;
using UniRx;
using UnityEngine;
using Utils.WebSocketClient.domain;
using Utils.WebSocketClient.domain.model;
using Zenject;

namespace Shooter.domain
{
    public class ShootPlayerUseCase: IDisposable
    {
        [Inject(Id = IWSCommandsUseCase.AuthorizedInstance)]
        private IWSCommandsUseCase commandsUseCase;
        [Inject] private IWeaponDamageRepository weaponDamageRepository;

        private readonly CompositeDisposable composite = new();

        private readonly int shootDirectionDistance = 10000;

        public void Hit(long receiverId, long weaponId, Vector3 position)
        {
            var damage = weaponDamageRepository.GetDamage(weaponId);
            var data = new HitRequestData(weaponId, receiverId, damage, position);
            RequestHit(data);
        }
        
        public void Shoot(Vector3 position, Vector3 direction, long weaponId)
        {
            var shootDirection = position + direction.normalized * shootDirectionDistance;
            var snapshot = shootDirection.GetSnapshot();
            var data = new ShootRequestData(weaponId, snapshot);
            RequestShoot(data);
        }
        
        private void RequestShoot(ShootRequestData data) => commandsUseCase
            .Request<Unit, ShootRequestData>(Commands.IntentShoot, data)
            .Subscribe()
            .AddTo(composite);
        
        private void RequestHit(HitRequestData data) => commandsUseCase
            .Request<Unit, HitRequestData>(Commands.IntentHit, data)
            .Subscribe()
            .AddTo(composite);

        public void Dispose() => composite.Dispose();
        
        [Serializable]
        private struct HitRequestData
        {
            public long weaponId;
            public int damage;
            public TransformSnapshot hitPos;
            public long receiverId;
            
            public HitRequestData(long weaponId, long receiverId, int damage, Vector3 hitPos) : this()
            {
                this.weaponId = weaponId;
                this.damage = damage;
                this.hitPos = hitPos.GetSnapshot();
                this.receiverId = receiverId;
            }
        }

        [Serializable]
        private struct ShootRequestData
        {
            public long weaponId;
            public TransformSnapshot direction;

            public ShootRequestData(long weaponId, TransformSnapshot direction)
            {
                this.weaponId = weaponId;
                this.direction = direction;
            }
        }
    }
}