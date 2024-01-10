using System;
using UniRx;
using Utils.WebSocketClient.domain;
using Zenject;

namespace Core.Auth.domain
{
    public class WSAuthCommandsUseCase : IWSCommandsUseCase
    {
        [Inject(Id = IWSCommandsUseCase.BaseInstance)]
        private IWSCommandsUseCase commandsUseCase;

        [Inject] private IAuthRepository authRepository;

        public IObservable<T> Listen<T>(long commandId) => commandsUseCase.Listen<T>(commandId);

        public IObservable<T> Request<T>(long commandId) => Request<T, string>(commandId, "");

        public IObservable<T> Request<T, TP>(long commandId, TP commandParams) => authRepository
            .GetLoggedInFlow()
            .Where(loggedIn => loggedIn)
            .Select(_ => commandsUseCase.Request<T, TP>(commandId, commandParams))
            .Switch();

        public IObservable<T> Subscribe<T>(long commandId) => Subscribe<T, string>(commandId, "");

        public IObservable<T> Subscribe<T, TP>(long commandId, TP commandParams)
        {
            var requestFlow = Request<T, TP>(commandId, commandParams);
            var listenFlow = commandsUseCase.Listen<T>(commandId);
            return requestFlow.Merge(listenFlow);
        }
    }
}