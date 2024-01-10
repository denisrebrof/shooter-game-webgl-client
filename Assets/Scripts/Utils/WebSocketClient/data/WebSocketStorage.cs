using JetBrains.Annotations;
using NativeWebSocket;

namespace Utils.WebSocketClient.data
{
    public class WebSocketStorage
    {
        [CanBeNull] private WebSocket websocket;

        public void Set(WebSocket socket) => websocket = socket;

        public bool Get(out WebSocket socket)
        {
            socket = websocket;
            return websocket != null;
        }
    }
}