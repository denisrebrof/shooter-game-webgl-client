using UnityEngine;

namespace Shooter.domain.Repositories
{
    public interface IWeaponIconRepository
    {
        Sprite GetIcon(long weaponId);
    }
}