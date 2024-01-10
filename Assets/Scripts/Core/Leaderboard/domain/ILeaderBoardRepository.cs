using System;
using System.Collections.Generic;

namespace Core.Leaderboard.domain
{
    public interface ILeaderBoardRepository
    {
        IObservable<List<LeaderBoardItem>> GetPositionsAroundPlayer(string leaderBoardId, int resultsCount);
        IObservable<bool> SendResult(string leaderBoardId, int score);
    }
}