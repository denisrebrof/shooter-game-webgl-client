using InfimaGames.LowPolyShooterPack.Legacy;
using Shooter.domain;
using UnityEngine;
using Zenject;

namespace Shooter.presentation.Player.Weapons
{
    public class PlayerProjectile : ProjectileItemBase
    {
        [Inject] private ShootPlayerUseCase shootPlayerUseCase;

        protected override void HandleHit(RaycastHit hit)
        {
            var hitCollider = hit.collider;
            var type = ImpactUtils.GetImpactType(hit.collider);
            IImpactNavigator.Instance.AddImpact(hit.point, hit.normal, type);

            if (!hitCollider.CompareTag("OtherPlayer"))
                return;

            var playerIdProvider = hitCollider.GetComponentInParent<PlayerIdProvider>();
            if (playerIdProvider == null) return;

            if (!playerIdProvider.GetPlayerId(out var playerId))
                return;

            shootPlayerUseCase.Hit(playerId, weaponId, damage, hit.point);
        }
    }
}