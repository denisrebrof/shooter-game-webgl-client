using System;

namespace Features.Purchases.domain.repositories
{
    public interface IRewardedVideoPurchaseRepository
    {
        void AddRewardedVideoWatch(string id);
        IObservable<int> GetRewardedVideoCurrentWatchesCount(string id);
        int GetRewardedVideoWatchesCount(string id);
    }
}