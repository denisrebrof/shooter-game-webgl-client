using System;
using UniRx;
using Zenject;

namespace Features.LevelTime.domain
{
    public class LevelTimeLeftUseCase
    {
        [Inject] private CurrentLevelMaxTimeUseCase currentLevelMaxTimeUseCase;
        [Inject] private ILevelTimerRepository levelTimerRepository;

        public IObservable<long> GetTimeLeftFlow() => levelTimerRepository
            .GetTimerFlow()
            .CombineLatest(
                currentLevelMaxTimeUseCase.GetMaxTimeSecondsFlow(),
                (timer, maxTime) => Math.Max(0, maxTime * 1000 - timer)
            );
    }
}