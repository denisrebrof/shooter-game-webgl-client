namespace Features.LevelScore.domain
{
    public interface ILevelMaxScoreRepository
    {
        public int GetMaxScore(long levelId);
    }
}