using System;
using System.Collections.Generic;
using Core.Leaderboard.domain;
using Zenject;

namespace Features.LevelScore.domain
{
    public class LevelLeaderboardUseCase
    {
        [Inject] private ILeaderBoardRepository leaderBoardRepository;

        private const int ResultsCount = 7;
        
        public IObservable<List<LeaderBoardItem>> GetPositionsAroundPlayer(long levelId)
        {
            var leaderBoardName = GetLeaderBoardName(levelId);
            return leaderBoardRepository.GetPositionsAroundPlayer(leaderBoardName, ResultsCount);
        }

        public IObservable<bool> SendLevelScore(long levelId, int score)
        {
            var leaderBoardName = GetLeaderBoardName(levelId);
            return leaderBoardRepository.SendResult(leaderBoardName, score);
        }

        private static string GetLeaderBoardName(long levelId) => "LevelScore_" + levelId;
    }
}