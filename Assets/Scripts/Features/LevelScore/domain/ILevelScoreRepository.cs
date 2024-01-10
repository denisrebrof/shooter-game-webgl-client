using Features.LevelScore.domain.model;

namespace Features.LevelScore.domain
{
    public interface ILevelScoreRepository
    {
        public int GetScore(int levelId, ScoreType type);
        public void UpdateScore(int levelId, int score);
    }
}