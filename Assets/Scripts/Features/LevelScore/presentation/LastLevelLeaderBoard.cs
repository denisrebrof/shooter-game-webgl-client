using System;
using System.Collections.Generic;
using Core.Leaderboard.domain;
using Core.Leaderboard.presentation;
using Features.LevelScore.domain;
using Zenject;

namespace Features.LevelScore.presentation
{
    public class LastLevelLeaderBoard : LeaderBoardView
    {
        [Inject] private LastLevelLeaderBoardUseCase lastLevelLeaderBoardUseCase;

        protected override IObservable<List<LeaderBoardItem>> GetLeaderBoardResultsFlow() =>
            lastLevelLeaderBoardUseCase.GetLeaderBoardResultsFlow();
    }
}