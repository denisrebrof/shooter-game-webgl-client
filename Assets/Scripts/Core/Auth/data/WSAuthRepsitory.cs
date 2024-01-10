using System;
using System.Diagnostics.CodeAnalysis;
using Core.Auth.domain;
using Core.Auth.domain.model;
using UniRx;
using Utils.WebSocketClient.domain;
using Utils.WebSocketClient.domain.model;
using Zenject;

namespace Core.Auth.data
{
    public class WSAuthRepository : IAuthRepository
    {
        [Inject(Id = IWSCommandsUseCase.BaseInstance)]
        private IWSCommandsUseCase commandsUseCase;

        private readonly BehaviorSubject<bool> loggedInFlow = new(false);

        public IObservable<bool> GetLoggedInFlow() => loggedInFlow.DistinctUntilChanged();

        public string LoginUserId { get; private set; }

        public IObservable<bool> Login(AuthData authData)
        {
            var request = new LogInRequest
            {
                yandexId = authData.Type == AuthType.YandexId ? authData.Content : "",
                localId = authData.Type == AuthType.LocalId ? authData.Content : ""
            };
            return commandsUseCase
                .Request<LogInResponse, LogInRequest>(Commands.LogIn, request)
                .Do(response => { if (response.authSuccessful) LoginUserId = response.userId.ToString(); })
                .Select(response => response.authSuccessful)
                .Do(loggedInFlow.OnNext);
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private struct LogInRequest
        {
            public string yandexId;
            public string localId;
            public string username;
            public string password;
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private struct LogInResponse
        {
            public bool authSuccessful;
            public string token;
            public long userId;
        }
    }
}