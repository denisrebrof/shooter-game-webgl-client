using Core.Analytics.levels;
using Features.Levels.data;
using Features.Levels.domain.repositories;
using Zenject;

namespace Features.Levels.domain
{
    public class CompleteCurrentLevelUseCase
    {
        [Inject] private LevelAnalyticsRepository analyticsRepository;
        [Inject] private ILevelCompletedStateRepository levelsRepository;
        [Inject] private ICurrentLevelRepository currentLevelRepository;
        [Inject] private SetNextCurrentLevelUseCase setNextCurrentLevelUseCase;

        public void CompleteCurrentLevel()
        {
            var currentLevel = currentLevelRepository.GetCurrentLevel();
            levelsRepository.SetLevelCompleted(currentLevel.ID);
            setNextCurrentLevelUseCase.SetNextCurrentLevel();
            analyticsRepository.SendLevelEvent(currentLevel.ID, LevelEvent.Complete);
        }
    }
}