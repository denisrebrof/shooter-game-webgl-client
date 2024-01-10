using Features.Levels.domain.repositories;
using Zenject;

namespace Features.Levels.presentation.ui
{
    public class DefaultLevelItemController : LevelItem.ILevelItemController
    {
        [Inject] private LevelLoadingNavigator levelLoadingNavigator;
        [Inject] private ICurrentLevelRepository currentLevelRepository;

        public void OnItemClick(int levelId)
        {
            currentLevelRepository.SetCurrentLevel(levelId);
            levelLoadingNavigator.LoadLevel(levelId);
        }
    }
}