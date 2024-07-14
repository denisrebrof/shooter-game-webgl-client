using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using Shooter.domain.Model;
using Shooter.domain.Repositories;
using UniRx;
using Zenject;

namespace Shooter.domain
{
    public class WeaponRewardsUseCase
    {
        [Inject] private ILevelProgressionRepository levelProgressionRepository;
        [Inject] private IWeaponStoreRepository weaponStoreRepository;

        public IObservable<WeaponState> GetClosestRewardFlow() =>
            GetNextRewards()
                .Where(rewards => !rewards.IsEmpty())
                .Select(FindClosestState);

        public IObservable<List<WeaponState>> GetSortedNextRewards() => GetNextRewards().Select(SortByLevel);

        private IObservable<List<WeaponState>> GetNextRewards() =>
            // ReSharper disable once InvokeAsExtensionMethod
            Observable.CombineLatest(
                levelProgressionRepository.Get().Select(data => data.level),
                weaponStoreRepository.GetWeaponsData().DistinctUntilChanged(),
                GetNextRewards
            );

        private List<WeaponState> SortByLevel(List<WeaponState> rewards) =>
            rewards
                .OrderBy(reward => reward.info.availableFromLevel)
                .ToList();

        private WeaponState FindClosestState(List<WeaponState> rewards)
        {
            var min = rewards[0];
            for (var index = 1; index < rewards.Count; index++)
            {
                var reward = rewards[index];
                if (reward.info.availableFromLevel < min.info.availableFromLevel)
                    min = reward;
            }

            return min;
        }

        private List<WeaponState> GetNextRewards(int currentLevel, WeaponsData data)
        {
            return data.weapons.Where(weapon => weapon.info.availableFromLevel > currentLevel).ToList();
        }
    }
}