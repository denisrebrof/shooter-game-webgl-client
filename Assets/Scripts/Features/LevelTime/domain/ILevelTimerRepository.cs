using System;

namespace Features.LevelTime.domain
{
    public interface ILevelTimerRepository
    {
        public IObservable<long> GetTimerFlow();
        public long GetTimerResult();
        public void StartTimer();
        public void StopTimer();
        public void SetPaused(bool pausedState);
    }
}