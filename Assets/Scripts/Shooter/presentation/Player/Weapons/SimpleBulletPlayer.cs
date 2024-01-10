using Shooter.domain;
using UnityEngine;
using Zenject;

namespace Shooter.presentation.Player.Weapons
{
    public class SimpleBulletPlayer : SimpleBullet
    {
        [Inject] private ShootPlayerUseCase shootPlayerUseCase;

        public long weaponId;

        protected override void HandleHit(RaycastHit hit)
        {
            base.HandleHit(hit);
            var playerIdProvider = hit.transform.GetComponentInParent<PlayerIdProvider>();
            if (playerIdProvider == null)
            {
                return;
            }

            if (!playerIdProvider.GetPlayerId(out var playerId))
                return;

            shootPlayerUseCase.Hit(playerId, weaponId, hit.point);
        }
    }
}