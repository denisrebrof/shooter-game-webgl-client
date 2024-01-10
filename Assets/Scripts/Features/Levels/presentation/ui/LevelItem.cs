using Features.Levels.domain.model;
using Features.Levels.domain.repositories;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Features.Levels.presentation.ui
{
    public class LevelItem : MonoBehaviour
    {
        [Inject] private ILevelItemController itemController;
        [Inject] private ILevelsRepository levelsRepository;
        [Inject] private ILevelCompletedStateRepository completedStateRepository;

        [SerializeField] private Text numberText;
        [SerializeField] private GameObject checkmark;
        private int? levelId;

        [Inject]
        public LevelItem(ILevelItemController itemController, ILevelsRepository levelsRepository)
        {
            this.itemController = itemController;
            this.levelsRepository = levelsRepository;
        }

        private void OnEnable()
        {
            if (levelId == null)
                return;

            var level = levelsRepository.GetLevel(levelId.Value);
            Setup(level);
        }

        public void Setup(Level level)
        {
            levelId = level.ID;
            numberText.text = level.Number.ToString();
            checkmark.SetActive(completedStateRepository.GetLevelCompletedState(level.ID));
        }

        public void HandleClick()
        {
            if (levelId.HasValue) itemController.OnItemClick(levelId.Value);
        }

        public interface ILevelItemController
        {
            void OnItemClick(int levelId);
        }

        public class Factory : PlaceholderFactory<LevelItem>
        {
        }
    }
}