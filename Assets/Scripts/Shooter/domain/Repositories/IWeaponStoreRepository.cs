using System;
using Shooter.domain.Model;

namespace Shooter.domain.Repositories
{
    public interface IWeaponStoreRepository
    {
        public IObservable<WeaponsData> GetWeaponsData();
    }
}