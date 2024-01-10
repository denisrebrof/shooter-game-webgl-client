using System;

namespace Utils.WebSocketClient.domain
{
    public interface IWSCommandsRepository
    {
        IObservable<string> Listen(long commandId);
        IObservable<string> Request(long commandId, string commandParams = "");
    }
}