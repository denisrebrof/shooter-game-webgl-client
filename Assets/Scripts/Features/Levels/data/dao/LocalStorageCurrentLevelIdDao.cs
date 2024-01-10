using System;
using Plugins.FileIO;

namespace Features.Levels.data.dao
{
    public class LocalStorageCurrentLevelIdDataSource : CurrentLevelRepository.ICurrentLevelIdDataSource
    {
        private const string KeyPrefix = "CurrentLevelId";
        private const string PrevKeyPrefix = "PrevLevelId";

        public bool HasCurrentLevelId() => LocalStorageIO.HasKey(KeyPrefix);

        public int GetCurrentLevelId()
        {
            var storedLevelIdData = LocalStorageIO.GetString(KeyPrefix);
            return Convert.ToInt32(storedLevelIdData);
        }

        public int GetPrevLevelId()
        {
            if (!LocalStorageIO.HasKey(PrevKeyPrefix)) return GetCurrentLevelId();
            var storedLevelIdData = LocalStorageIO.GetString(PrevKeyPrefix);
            return Convert.ToInt32(storedLevelIdData);
        }

        public void SetCurrentLevelId(int id)
        {
            var prevLevelId = HasCurrentLevelId() ? GetCurrentLevelId() : id;
            LocalStorageIO.SetString(PrevKeyPrefix, prevLevelId.ToString());
            LocalStorageIO.SetString(KeyPrefix, id.ToString());
        }
    }
}