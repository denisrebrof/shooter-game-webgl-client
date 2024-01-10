using System;
using System.Collections.Generic;
using Core.Auth.domain;
using Core.Leaderboard.domain;
using UniRx;
using Zenject;

namespace Core.Leaderboard.data
{
    public class DefaultLeaderBoardRepository : ILeaderBoardRepository
    {
        [Inject] private IAuthRepository authRepository;
        [Inject] private ILeaderBoardRemoteDataSource leaderBoardRemoteDataSource;

        public IObservable<List<LeaderBoardItem>> GetPositionsAroundPlayer(string leaderBoardId, int resultsCount) => authRepository
            .GetLoggedInFlow()
            .Where(loggedIn => loggedIn)
            .Select(_ => leaderBoardRemoteDataSource.GetPositionsAroundPlayer(leaderBoardId, resultsCount))
            .Switch();

        public IObservable<bool> SendResult(string leaderBoardId, int score) => authRepository
            .GetLoggedInFlow()
            .Where(loggedIn => loggedIn)
            .Select(_ => leaderBoardRemoteDataSource.SendResult(leaderBoardId, score))
            .Switch();

        public interface ILeaderBoardRemoteDataSource
        {
            IObservable<List<LeaderBoardItem>> GetPositionsAroundPlayer(string leaderBoardId, int resultsCount);
            IObservable<bool> SendResult(string leaderBoardId, int score);
        }
    }
}