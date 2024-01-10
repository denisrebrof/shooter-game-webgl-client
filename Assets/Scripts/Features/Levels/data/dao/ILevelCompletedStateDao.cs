namespace Features.Levels.data
{
    public interface ILevelCompletedStateDao
    {
        bool IsCompleted(long levelId);
        void SetCompleted(long levelId);
    }
}