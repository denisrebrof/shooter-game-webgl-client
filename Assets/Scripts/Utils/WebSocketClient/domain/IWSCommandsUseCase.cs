using System;

namespace Utils.WebSocketClient.domain
{
    public interface IWSCommandsUseCase
    {
        public const string BaseInstance = "BaseInstanceOf" + nameof(IWSCommandsUseCase);
        public const string AuthorizedInstance = "AuthorizedInstanceOf" + nameof(IWSCommandsUseCase);
        
        IObservable<T> Listen<T>(long commandId);
        IObservable<T> Request<T>(long commandId);
        IObservable<T> Request<T, TP>(long commandId, TP commandParams);
        IObservable<T> Subscribe<T>(long commandId);
        IObservable<T> Subscribe<T, TP>(long commandId, TP commandParams);
    }
}