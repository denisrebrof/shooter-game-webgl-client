using System;
using System.Collections.Generic;
using Core.Leaderboard.domain;
using UniRx;

namespace Core.Leaderboard.data
{
    public class StubLeaderBoardRemoteDataSource : DefaultLeaderBoardRepository.ILeaderBoardRemoteDataSource
    {
        private List<LeaderBoardItem> stubList = new();
        public IObservable<List<LeaderBoardItem>> GetPositionsAroundPlayer(
            string leaderBoardId,
            int resultsCount
        ) => Observable.Return(stubList);

        public IObservable<bool> SendResult(string leaderBoardId, int score)
        {
            return Observable.Return(true);
        }
    }
}