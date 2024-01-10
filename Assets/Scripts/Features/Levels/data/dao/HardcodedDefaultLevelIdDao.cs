namespace Features.Levels.data.dao
{
    public class HardcodedDefaultLevelIdDao: CurrentLevelRepository.IDefaultLevelIdDao
    {
        private int defaultLevelId = 0;
        public int GetDefaultLevelId() => defaultLevelId;
    }
}