using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Utils.WebSocketClient.domain;
using Zenject;

namespace Utils.WebSocketClient.data
{
    public class InMemoryWSCommandsRepository : IWSCommandsRepository, IWSMessageHandler
    {
        private const char MessageDelimiter = '$';

        [Inject] private WebSocketStorage webSocketStorage;
        [Inject] private WSSettings settings;

        private readonly Dictionary<string, IDisposable> requests = new();

        private readonly Subject<MessageData> messages = new();

        public IObservable<string> Listen(long commandId) => messages
            .Where(data => data.CommandId == commandId)
            .Select(data => data.Message);

        public IObservable<string> Request(long commandId, string commandParams)
        {
            if (!webSocketStorage.Get(out var websocket))
                return Observable.Empty<string>();

            var requestId = Guid.NewGuid().ToString();
            var messageContent = $"{commandId}${commandParams}${requestId}";
            if (settings.logMessages)
                Debug.Log("Send Message " + messageContent);

            requests[requestId] = websocket.SendText(messageContent).ToObservable().Subscribe();
            return GetResponse(commandId, requestId);
        }

        private IObservable<string> GetResponse(long commandId, string requestId) => messages
            .Where(data => data.RequestId == requestId)
            .Where(data => data.CommandId == commandId)
            .Do(data => requests.Remove(data.RequestId))
            .Select(data => data.Message)
            .DoOnCancel(() => CancelRequest(requestId))
            .First();

        private void CancelRequest(string requestId)
        {
            if (!requests.ContainsKey(requestId))
                return;

            requests[requestId].Dispose();
            requests.Remove(requestId);
        }

        public void HandleMessage(string message)
        {
            var messageParts = message.Split(MessageDelimiter);
            if (messageParts.Length < 3)
                return;

            if (!long.TryParse(messageParts[0], out var commandId))
                return;

            var messageData = new MessageData
            {
                CommandId = commandId,
                RequestId = messageParts[1],
                Message = messageParts[2]
            };
            messages.OnNext(messageData);
        }

        private struct MessageData
        {
            public long CommandId;
            public string Message;
            public string RequestId;
        }
    }
}