using Features.Levels.domain.model;
using Features.Levels.domain.repositories;
using UnityEngine;
using Zenject;

namespace Features.Levels.presentation
{
    public class CurrentLevelLoadingNavigator : MonoBehaviour
    {
        [Inject] private ICurrentLevelRepository currentLevelRepository;
        [Inject] private ILevelsRepository levelsRepository;
        [Inject] private LevelLoadingNavigator levelLoadingNavigator;

        [SerializeField] private bool loadCurrentLevel;
        [SerializeField] private int initialLevelId;

        private void Awake()
        {
            if (loadCurrentLevel)
            {
                LoadCurrent();
                return;
            }

            var initialLevel = levelsRepository.GetLevel(initialLevelId);
            LoadLevel(initialLevel);
        }

        public void LoadCurrent() => LoadLevel(currentLevelRepository.GetCurrentLevel());

        public void LoadPrevious() => LoadLevel(currentLevelRepository.GetPrevLevel());

        private void LoadLevel(Level level) => levelLoadingNavigator.LoadLevel(level.ID);
    }
}