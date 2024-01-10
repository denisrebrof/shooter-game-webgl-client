using System;
using Features.GlobalScore.domain.model;

namespace Features.GlobalScore.domain
{
    public interface IGlobalScoreRepository
    {
        public IObservable<int> GetScoreFlow(GlobalScoreType type);
        public IObservable<bool> SendScore(int score);
    }
}