using System;
using Core.User.domain;
using UniRx;
using Utils.WebSocketClient.domain;
using Utils.WebSocketClient.domain.model;
using Zenject;
using static Core.User.domain.ICurrentUserNameRepository;

namespace Core.User.data
{
    public class WebSocketCurrentUserNameRepository : ICurrentUserNameRepository
    {
        [Inject(Id = IWSCommandsUseCase.AuthorizedInstance)]
        private IWSCommandsUseCase commandsUseCase;

        public IObservable<string> GetUserNameFlow() => commandsUseCase.Subscribe<string>(Commands.GetUsername);

        public IObservable<UpdateUserNameResult> UpdateUserName(string newName) => commandsUseCase
            .Request<long, string>(Commands.SetUsername, newName)
            .Select(GetResult);

        private static UpdateUserNameResult GetResult(long code) => code switch
        {
            0 => UpdateUserNameResult.Success,
            1 => UpdateUserNameResult.NotAvailable,
            _ => UpdateUserNameResult.Error
        };
    }
}