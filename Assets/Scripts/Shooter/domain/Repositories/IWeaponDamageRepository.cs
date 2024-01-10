namespace Shooter.domain.Repositories
{
    public interface IWeaponDamageRepository
    {
        int GetDamage(long weaponId);
    }
}