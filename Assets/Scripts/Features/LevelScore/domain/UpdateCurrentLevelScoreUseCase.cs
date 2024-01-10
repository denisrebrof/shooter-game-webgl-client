using System;
using Features.LevelTime.domain;
using Zenject;

namespace Features.LevelScore.domain
{
    public class UpdateCurrentLevelScoreUseCase
    {
        [Inject] private CurrentLevelScoreUseCase currentLevelScoreUseCase;
        [Inject] private LastLevelScoreUseCase lastLevelScoreUseCase;
        [Inject] private ILevelTimerRepository timerRepository;
        
        public IObservable<bool> UpdateScore()
        {
            timerRepository.StopTimer();
            var score = currentLevelScoreUseCase.GetLastScore();
            return lastLevelScoreUseCase.SetScore(score);
        }
    }
}