using System;
using System.Collections.Generic;
using Core.Leaderboard.domain;
using Features.Levels.domain.repositories;
using Zenject;

namespace Features.LevelScore.domain
{
    public class LastLevelLeaderBoardUseCase
    {
        [Inject] private ICurrentLevelRepository currentLevelRepository;
        [Inject] private LevelLeaderboardUseCase levelLeaderboardUseCase;

        public IObservable<List<LeaderBoardItem>> GetLeaderBoardResultsFlow()
        {
            var levelId = currentLevelRepository.GetPrevLevel().ID;
            return levelLeaderboardUseCase.GetPositionsAroundPlayer(levelId);
        }
    }
}