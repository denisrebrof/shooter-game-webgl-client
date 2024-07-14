using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NativeWebSocket;
using UniRx;
using UnityEngine;
using Utils.WebSocketClient.domain;
using Utils.WebSocketClient.domain.model;
using Zenject;

namespace Utils.WebSocketClient.data
{
    public class MonoWSConnectionRepository : MonoBehaviour, IWSConnectionRepository
    {
        [Inject] private IWSMessageHandler messageHandler;
        [Inject] private WebSocketStorage webSocketStorage;
        [Inject] private WSSettings settings;

        private readonly BehaviorSubject<WSConnectionState> connectionState = new(WSConnectionState.None);

        private WSConnectionState currentState = WSConnectionState.None;
        public WSConnectionState CurrentState => currentState; 

        public IObservable<WSConnectionState> ConnectionState => connectionState.DistinctUntilChanged();

        public IObservable<Unit> ConnectBaseAuth(WSAuthData authData)
        {
            var headers = CreateHeaders(authData);
            return ConnectAsync(headers).ToObservable();
        }

        public IObservable<Unit> Connect() => ConnectAsync().ToObservable();

        private void UpdateConnectionState(WSConnectionState newState)
        {
            Debug.Log("Connection State: " + newState);
            currentState = newState;
            connectionState.OnNext(newState);
        }

        private async Task ConnectAsync(Dictionary<string, string> headers = null)
        {
            await ShutDownExistingSocket();
            UpdateConnectionState(WSConnectionState.Connecting);
            var websocket = new WebSocket(settings.URL, headers);
            Debug.Log("Connect with url " + settings.URL);
            websocket.OnOpen += () =>
            {
                if (settings.logConnection)
                    Debug.LogWarning("Connection open!");

                UpdateConnectionState(WSConnectionState.Connected);
            };
            websocket.OnError += (e) =>
            {
                Debug.LogWarning("Connection Error! " + e);
                UpdateConnectionState(WSConnectionState.Disconnected);
            };
            websocket.OnClose += (closeCode) =>
            {
                if (settings.logConnection)
                    Debug.LogWarning($"Connection closed! code: {closeCode}");

                UpdateConnectionState(WSConnectionState.Disconnected);
            };
            websocket.OnMessage += (bytes) =>
            {
                // Reading a plain text message
                var message = Encoding.UTF8.GetString(bytes);
                if (settings.logMessages)
                    Debug.Log("Received Message (" + bytes.Length + " bytes) " + message);

                messageHandler.HandleMessage(message);
            };
            webSocketStorage.Set(websocket);
            await websocket.Connect();
        }

        private static Dictionary<string, string> CreateHeaders(WSAuthData authData)
        {
            var data = Encoding.UTF8.GetBytes($"{authData.Username}:{authData.Password}");
            var authHeader = "Basic " + Convert.ToBase64String(data);
            return new Dictionary<string, string>
            {
                { "Authorization", authHeader }
            };
        }

        private async Task ShutDownExistingSocket()
        {
            if (!webSocketStorage.Get(out var websocket))
                return;

            await websocket.Close();
            UpdateConnectionState(WSConnectionState.Disconnected);
        }

        private void Update()
        {
            if (currentState != WSConnectionState.Connected)
                return;

            DispatchQueue();
        }

        private void DispatchQueue()
        {
            if (!webSocketStorage.Get(out var socket))
                return;

#if !UNITY_WEBGL || UNITY_EDITOR
            socket.DispatchMessageQueue();
#endif
        }

        private async void OnApplicationQuit() => await ShutDownExistingSocket();
    }
}