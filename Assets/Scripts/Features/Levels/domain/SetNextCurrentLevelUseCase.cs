using System.Linq;
using Features.Levels.domain.model;
using Features.Levels.domain.repositories;
using Zenject;

namespace Features.Levels.domain
{
    public class SetNextCurrentLevelUseCase
    {
        [Inject] private ILevelsRepository levelsRepository;
        [Inject] private ICurrentLevelRepository currentLevelRepository;
        [Inject] private ILevelCompletedStateRepository сompletedStateRepository;

        public void SetNextCurrentLevel()
        {
            var currentLevel = currentLevelRepository.GetCurrentLevel();
            var nextLevel = GetNextLevel(currentLevel);
            currentLevelRepository.SetCurrentLevel(nextLevel.ID);
        }

        private Level GetNextLevel(Level currentLevel)
        {
            var nextLevels = levelsRepository
                .GetLevels()
                .Where(level => level.Number > currentLevel.Number)
                .OrderBy(level => level.Number)
                .ToList();

            if (nextLevels.Count == 0)
                return currentLevel;

            return nextLevels.All(level => сompletedStateRepository.GetLevelCompletedState(level.ID))
                ? nextLevels.First()
                : nextLevels.First(level => !сompletedStateRepository.GetLevelCompletedState(level.ID));
        }
    }
}