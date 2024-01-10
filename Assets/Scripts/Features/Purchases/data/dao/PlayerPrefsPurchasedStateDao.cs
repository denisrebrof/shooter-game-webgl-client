using UnityEngine;

namespace Features.Purchases.data.dao
{
    public class PlayerPrefsPurchasedStateDao : ISavedPurchasedStateDao
    {
        private const string PrefsKeyPrefix = "RewardedVideoWatches";

        public bool GetPurchasedState(string purchaseId)
        {
            var prefKey = GetPrefKey(purchaseId);
            return PlayerPrefs.GetInt(prefKey, 0) > 0;
        }

        public void SetPurchasedState(string purchaseId)
        {
            var prefKey = GetPrefKey(purchaseId);
            PlayerPrefs.SetInt(prefKey, 1);
        }

        private static string GetPrefKey(string purchaseId) => $"{PrefsKeyPrefix}_{purchaseId}";
    }
}