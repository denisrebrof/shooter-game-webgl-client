#if PLAYFAB
using System;
using System.Collections.Generic;
using System.Linq;
using Core.Leaderboard.domain;
using PlayFab;
using PlayFab.ClientModels;
using UniRx;

namespace Core.Leaderboard.data
{
    public class PlayfabLeaderBoardRemoteDataSource: LeaderBoardPlayfabRepository.ILeaderBoardRemoteDataSource
    {
        
        public IObservable<List<LeaderBoardItem>> GetPositionsAroundPlayer(string leaderBoardId, int resultsCount) => Observable.Create(
            (IObserver<List<LeaderBoardItem>> observer) =>
            {
                var request = new GetLeaderboardAroundPlayerRequest
                {
                    StatisticName = leaderBoardId,
                    MaxResultsCount = resultsCount
                };
                PlayFabClientAPI.GetLeaderboardAroundPlayer(
                    request,
                    success =>
                    {
                        var result = success.Leaderboard.Select(GetLeaderBoardItem).ToList();
                        observer.OnNext(result);
                        observer.OnCompleted();
                    },
                    error => { observer.OnCompleted(); }
                );
                return Disposable.Create(() => { });
            }
        );

        public IObservable<bool> SendResult(string leaderBoardId, int score) => Observable.Create(
            (IObserver<bool> observer) =>
            {
                var request = new UpdatePlayerStatisticsRequest
                {
                    Statistics = new List<StatisticUpdate>
                    {
                        new()
                        {
                            StatisticName = leaderBoardId,
                            Value = score
                        }
                    }
                };
                PlayFabClientAPI.UpdatePlayerStatistics(
                    request,
                    success =>
                    {
                        observer.OnNext(true);
                        observer.OnCompleted();
                    },
                    error => { 
                        observer.OnNext(false);
                        observer.OnCompleted();
                    }
                );
                return Disposable.Create(() => { });
            }
        );

        private LeaderBoardItem GetLeaderBoardItem(PlayerLeaderboardEntry entry) => new(
            playerName: entry.DisplayName,
            position: entry.Position,
            score: entry.StatValue,
            entry.PlayFabId
        );
    }
}
#endif