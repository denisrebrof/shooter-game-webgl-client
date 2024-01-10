using Core.Auth.domain;
using Plugins.FileIO;

namespace Core.Auth.data
{
    public class LocalStoragePlayerIdRepository : ILocalPlayerIdRepository
    {
        private const string PREFS_KEY = "LocalPlayerId";

        public bool Fetch(out string id)
        {
            id = null;
            if (!LocalStorageIO.HasKey(PREFS_KEY))
                return false;

            id = LocalStorageIO.GetString(PREFS_KEY);
            return true;
        }

        public void Set(string id) => LocalStorageIO.SetString(PREFS_KEY, id);
    }
}