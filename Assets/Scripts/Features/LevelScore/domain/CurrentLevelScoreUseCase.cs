using System;
using Features.Levels.domain.repositories;
using Features.LevelTime.domain;
using UniRx;
using UnityEngine;
using Zenject;

namespace Features.LevelScore.domain
{
    public class CurrentLevelScoreUseCase
    {
        [Inject] private ICurrentLevelRepository currentLevelRepository;
        [Inject] private ILevelTimerRepository levelTimerRepository;
        [Inject] private ILevelTimeRepository levelTimeRepository;
        [Inject] private ILevelMaxScoreRepository levelMaxScoreRepository;

        public int GetLastScore() => CalculateScore(levelTimerRepository.GetTimerResult());

        public IObservable<int> GetCurrentScoreFlow() => levelTimerRepository
            .GetTimerFlow()
            .Select(CalculateScore);

        private int CalculateScore(long timer)
        {
            var levelId = currentLevelRepository.GetCurrentLevel().ID;
            var maxTime = levelTimeRepository.GetMaxTime(levelId);
            var maxScore = levelMaxScoreRepository.GetMaxScore(levelId);
            if (maxTime == 0)
                return maxScore;
            
            float timerSeconds = timer / 1000;
            var relativeScore = Mathf.Clamp(1f - timerSeconds / maxTime, 0, maxScore);
            var score = relativeScore * levelMaxScoreRepository.GetMaxScore(levelId);
            return Convert.ToInt32(score);
        }
    }
}