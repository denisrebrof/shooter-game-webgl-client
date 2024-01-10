using System;

namespace Features.Levels.domain.repositories
{
    public interface ILevelCompletedStateRepository
    {
        IObservable<bool> GetLevelCompletedStateFlow(long levelId);
        bool GetLevelCompletedState(long levelId);
        void SetLevelCompleted(long levelId);
    }
}