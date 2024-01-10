using System;
using System.Collections.Generic;
using UniRx;
using Utils.WebSocketClient.domain;
using Utils.WebSocketClient.domain.model;
using Zenject;

namespace Features.Rating.domain
{
    public class RatingUseCase
    {
        [Inject(Id = IWSCommandsUseCase.AuthorizedInstance)]
        private IWSCommandsUseCase commandsUseCase;

        [Inject(Id = "RatingRequestSize")] private int requestSize;

        public IObservable<List<RatingData>> GetRating() => commandsUseCase
            .Request<RatingDataResponse, int>(Commands.Rating, requestSize)
            .Select(response => response.items);

        public IObservable<UserStatsData> GetStats() => commandsUseCase
            .Request<UserStatsData>(Commands.UserStats);
    }
}