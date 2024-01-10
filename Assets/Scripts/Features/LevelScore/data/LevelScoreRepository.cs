using System;
using Features.LevelScore.domain;
using Features.LevelScore.domain.model;
using Zenject;

namespace Features.LevelScore.data
{
    public class LevelScoreRepository : ILevelScoreRepository
    {
        [Inject] private ILevelScoreDataSource levelScoreDataSource;

        public int GetScore(int levelId, ScoreType type) => type switch
        {
            ScoreType.Last => levelScoreDataSource.GetLastScore(levelId),
            ScoreType.Best => levelScoreDataSource.GetBestScore(levelId),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Unknown ScoreType requested")
        };

        public void UpdateScore(int levelId, int score)
        {
            var prevScore = levelScoreDataSource.GetLastScore(levelId);
            levelScoreDataSource.SetLastScore(levelId, score);
            if (score < prevScore) return;
            levelScoreDataSource.SetBestScore(levelId, score);
        }

        public interface ILevelScoreDataSource
        {
            public int GetBestScore(int levelId);
            public int GetLastScore(int levelId);
            public void SetBestScore(int levelId, int score);
            public void SetLastScore(int levelId, int score);
        }
    }
}