using System;
using UnityEngine;

namespace Features.Levels.data.dao
{
    public class PlayerPrefsCurrentLevelIdDataSource : CurrentLevelRepository.ICurrentLevelIdDataSource
    {
        private const string KeyPrefix = "CurrentLevelId";
        private const string PrevKeyPrefix = "PrevLevelId";

        public bool HasCurrentLevelId() => PlayerPrefs.HasKey(KeyPrefix);

        public int GetCurrentLevelId()
        {
            var storedLevelIdData = PlayerPrefs.GetString(KeyPrefix);
            return Convert.ToInt32(storedLevelIdData);
        }

        public int GetPrevLevelId()
        {
            if (!PlayerPrefs.HasKey(PrevKeyPrefix)) return GetCurrentLevelId();
            var storedLevelIdData = PlayerPrefs.GetString(PrevKeyPrefix);
            return Convert.ToInt32(storedLevelIdData);
        }

        public void SetCurrentLevelId(int id)
        {
            var prevLevelId = HasCurrentLevelId() ? GetCurrentLevelId() : id;
            PlayerPrefs.SetString(PrevKeyPrefix, prevLevelId.ToString());
            PlayerPrefs.SetString(KeyPrefix, id.ToString());
        }
    }
}