using Features.Collectables.domain;
using Plugins.FileIO;

namespace Features.Collectables.data
{
    public class LocalStorageCollectableRepository : ICollectableRepository
    {
        private const string PREFS_KEY_PREFIX = "Collectable_item_";

        public bool IsCollected(string id) => LocalStorageIO.GetInt(PREFS_KEY_PREFIX + id, 0) > 0;

        public void Collect(string id)
        {
            LocalStorageIO.SetInt(PREFS_KEY_PREFIX + id, 1);
            LocalStorageIO.Save();
        }
    }
}