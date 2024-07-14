using Features.Levels.data;
using Features.Levels.domain.model;
using Zenject;

namespace Features.Levels.presentation.ui
{
    public class LevelItemControllerAnalyticsDecorator : LevelItem.ILevelItemController
    {
        private readonly LevelItem.ILevelItemController target;
        private readonly LevelAnalyticsRepository analyticsRepository;

        [Inject]
        public LevelItemControllerAnalyticsDecorator(
            LevelItem.ILevelItemController decorationTarget,
            LevelAnalyticsRepository analytics
        )
        {
            target = decorationTarget;
            analyticsRepository = analytics;
        }

        public void OnItemClick(int levelId)
        {
            analyticsRepository.SendLevelEvent(levelId, LevelEvent.Load);
            target.OnItemClick(levelId);
        }
    }
}