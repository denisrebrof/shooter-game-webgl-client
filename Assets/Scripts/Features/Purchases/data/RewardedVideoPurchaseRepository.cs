using System;
using System.Collections.Generic;
using Features.Purchases.data.dao;
using Features.Purchases.domain.repositories;
using UniRx;
using Zenject;

namespace Features.Purchases.data
{
    public class RewardedVideoPurchaseRepository : IRewardedVideoPurchaseRepository
    {
        [Inject] private IRewardedVideoWatchDao watchDao;
        [Inject] private IPurchaseEntitiesDao entitiesDao;

        private readonly Dictionary<string, ReactiveProperty<int>> watchesCountProcessors = new();

        public void AddRewardedVideoWatch(string id) => watchDao.AddWatch(id);

        public IObservable<int> GetRewardedVideoCurrentWatchesCount(string id)
        {
            if (watchesCountProcessors.ContainsKey(id))
                return watchesCountProcessors[id];

            var watchesCount = watchDao.GetWatches(id);
            var processor = new ReactiveProperty<int>(watchesCount);
            watchesCountProcessors[id] = processor;
            return processor;
        }

        public int GetRewardedVideoWatchesCount(string id) => entitiesDao.FindById(id).rewardedVideoCount;

        public interface IRewardedVideoWatchDao
        {
            void AddWatch(string id);
            int GetWatches(string id);
        }
    }
}