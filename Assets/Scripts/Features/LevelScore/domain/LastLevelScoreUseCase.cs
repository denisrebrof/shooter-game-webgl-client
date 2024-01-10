using System;
using Features.Levels.domain.repositories;
using Features.LevelScore.domain.model;
using UniRx;
using Zenject;

namespace Features.LevelScore.domain
{
    public class LastLevelScoreUseCase
    {
        [Inject] private ICurrentLevelRepository currentLevelRepository;
        [Inject] private ILevelScoreRepository scoreRepository;
        [Inject] private LevelLeaderboardUseCase leaderBoardUseCase;

        private int LastLevelId => currentLevelRepository.GetPrevLevel().ID;

        public int GetScore(ScoreType scoreType) => scoreRepository.GetScore(LastLevelId, scoreType);

        public IObservable<bool> SetScore(int score)
        {
            var maxScore = scoreRepository.GetScore(LastLevelId, ScoreType.Best);
            scoreRepository.UpdateScore(LastLevelId, score);
            return score <= maxScore ? Observable.Return(true) : leaderBoardUseCase.SendLevelScore(LastLevelId, score);
        }
    }
}