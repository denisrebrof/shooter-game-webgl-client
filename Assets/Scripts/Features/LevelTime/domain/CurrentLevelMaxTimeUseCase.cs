using System;
using Features.Levels.domain.repositories;
using UniRx;
using Zenject;

namespace Features.LevelTime.domain
{
    public class CurrentLevelMaxTimeUseCase
    {
        [Inject] private ICurrentLevelRepository currentLevelRepository;
        [Inject] private ILevelTimeRepository levelTimeRepository;

        public IObservable<int> GetMaxTimeSecondsFlow() => currentLevelRepository
            .GetCurrentLevelFlow()
            .Select(level => level.ID)
            .Select(levelTimeRepository.GetMaxTime);
    }
}