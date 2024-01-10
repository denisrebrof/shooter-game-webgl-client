namespace Utils.WebSocketClient.data
{
    public interface IWSMessageHandler
    {
        void HandleMessage(string message);
    }
}