using Plugins.FileIO;

namespace Features.Purchases.data.dao
{
    public class LocalStorageRewardedVideoWatchesDao: RewardedVideoPurchaseRepository.IRewardedVideoWatchDao
    {
        private const string PrefsKeyPrefix = "RewardedVideoWatches";
        public void AddWatch(string id)
        {
            var prefKey = GetPrefKey(id);
            var currentWatches = GetWatches(id);
            LocalStorageIO.SetInt(prefKey, currentWatches + 1);
        }

        public int GetWatches(string id)
        {
            var prefKey = GetPrefKey(id);
            return LocalStorageIO.GetInt(prefKey, 0);
        }
        
        private static string GetPrefKey(string purchaseId) => $"{PrefsKeyPrefix}_{purchaseId}";
    }
}