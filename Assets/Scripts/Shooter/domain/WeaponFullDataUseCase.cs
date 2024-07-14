using System;
using System.Linq;
using Features.Balance.domain.repositories;
using Shooter.domain.Model;
using Shooter.domain.Repositories;
using UniRx;
using Zenject;

namespace Shooter.domain
{
    public class WeaponFullDataUseCase
    {
        [Inject] private IWeaponStoreRepository weaponStoreRepository;
        [Inject] private ILevelProgressionRepository levelProgressionRepository;
        [Inject] private IBalanceRepository balanceRepository;

        public IObservable<WeaponFullData> Get(long weaponId) =>
            Observable.CombineLatest(
                GetWeaponDataFlow(weaponId),
                GetPlayerLevelFlow(),
                balanceRepository.GetBalanceFlow("primary"),
                GetWeaponFullData
            );

        private WeaponFullData GetWeaponFullData(WeaponData data, int playerLevel, int balance)
        {
            var state = data.State;
            var isLocked = !state.Purchased && state.info.availableFromLevel > playerLevel;
            var isPurchasable = !state.Purchased && !isLocked && balance > state.Cost;
            var isUpgradable = state.Upgradable && balance > state.Cost;
            return new WeaponFullData
            {
                Weapon = state,
                IsPrimary = data.IsPrimary,
                IsSecondary = data.IsSecondary,
                IsLocked = isLocked,
                IsPurchasable = isPurchasable,
                IsUpgradable = isUpgradable,
            };
        }

        private IObservable<int> GetPlayerLevelFlow() =>
            levelProgressionRepository
                .Get()
                .First()
                .Select(data => data.level);

        private IObservable<WeaponData> GetWeaponDataFlow(long weaponId) =>
            weaponStoreRepository
                .GetWeaponsData()
                .Where(data => data.weapons.Any(weapon => weapon.info.id == weaponId))
                .Select(data => new WeaponData
                {
                    State = data.weapons.First(weapon => weapon.info.id == weaponId),
                    IsPrimary = weaponId == data.primaryId,
                    IsSecondary = weaponId == data.secondaryId
                });

        private struct WeaponData
        {
            public WeaponState State;
            public bool IsPrimary;
            public bool IsSecondary;
        }
    }
}