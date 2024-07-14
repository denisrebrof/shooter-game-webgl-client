using InfimaGames.LowPolyShooterPack;
using Shooter.domain;
using Zenject;

namespace Shooter.presentation.Player.Weapons
{
    public class WeaponInventory : Inventory
    {
        [Inject] private SelectWeaponUseCase selectWeaponUseCase;
        [Inject] private ShootPlayerUseCase shootPlayerUseCase;

        protected override void OnWeaponEquipped(bool isSecondary)
        {
            var id = isSecondary ? PlayerDataStorage.SecondaryWeaponId : PlayerDataStorage.PrimaryWeaponId;
            selectWeaponUseCase.Select(id);
        }

        protected override void OnWeaponShoot(ShootEventData data, bool isSecondary)
        {
            var id = isSecondary ? PlayerDataStorage.SecondaryWeaponId : PlayerDataStorage.PrimaryWeaponId;
            shootPlayerUseCase.Shoot(data.Pos, data.Dir, id);
        }
    }
}