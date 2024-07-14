namespace Features.Levels.data.dao
{
    public interface ILevelCompletedStateDao
    {
        bool IsCompleted(long levelId);
        void SetCompleted(long levelId);
    }
}