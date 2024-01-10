using UnityEngine;

namespace Features.Purchases.data.dao
{
    public class PlayerPrefsRewardedVideoWatchesDao: RewardedVideoPurchaseRepository.IRewardedVideoWatchDao
    {
        private const string PrefsKeyPrefix = "RewardedVideoWatches";
        public void AddWatch(string id)
        {
            var prefKey = GetPrefKey(id);
            var currentWatches = GetWatches(id);
            PlayerPrefs.SetInt(prefKey, currentWatches + 1);
        }

        public int GetWatches(string id)
        {
            var prefKey = GetPrefKey(id);
            return PlayerPrefs.GetInt(prefKey, 0);
        }
        
        private static string GetPrefKey(string purchaseId) => $"{PrefsKeyPrefix}_{purchaseId}";
    }
}