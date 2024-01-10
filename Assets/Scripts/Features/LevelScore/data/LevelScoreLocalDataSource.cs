using Plugins.FileIO;

namespace Features.LevelScore.data
{
    public class LevelScoreLocalDataSource : LevelScoreRepository.ILevelScoreDataSource
    {
        private const string BEST_PREFIX = "LEVEL_SCORE_BEST_";
        private const string LAST_PREFIX = "LEVEL_SCORE_PREV_";

        public int GetBestScore(int levelId) => LocalStorageIO.HasKey(BEST_PREFIX + levelId)
            ? LocalStorageIO.GetInt(BEST_PREFIX + levelId)
            : 0;

        public int GetLastScore(int levelId) => LocalStorageIO.HasKey(LAST_PREFIX + levelId)
            ? LocalStorageIO.GetInt(LAST_PREFIX + levelId)
            : 0;

        public void SetBestScore(int levelId, int score) => LocalStorageIO.SetInt(BEST_PREFIX + levelId, score);

        public void SetLastScore(int levelId, int score) => LocalStorageIO.SetInt(LAST_PREFIX + levelId, score);
    }
}