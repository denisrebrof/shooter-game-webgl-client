using Plugins.FileIO;

namespace Features.Purchases.data.dao
{
    public class LocalStoragePurchasedStateDao : ISavedPurchasedStateDao
    {
        private const string PrefsKeyPrefix = "RewardedVideoWatches";

        public bool GetPurchasedState(string purchaseId)
        {
            var prefKey = GetPrefKey(purchaseId);
            return LocalStorageIO.GetInt(prefKey, 0) > 0;
        }

        public void SetPurchasedState(string purchaseId)
        {
            var prefKey = GetPrefKey(purchaseId);
            LocalStorageIO.SetInt(prefKey, 1);
        }

        private static string GetPrefKey(string purchaseId) => $"{PrefsKeyPrefix}_{purchaseId}";
    }
}