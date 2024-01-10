using System;
using UnityEngine;

namespace Utils.WebSocketClient.data
{
    [CreateAssetMenu(menuName = "Settings/Web Socket Settings")]
    public class WSSettings : ScriptableObject
    {
        [SerializeField] private ProtocolType protocol = ProtocolType.Secure;
        [SerializeField] private string host = "localhost";
        [SerializeField] private string path = "websocket";
        public bool logConnection = true;
        public bool logMessages = true;

        public string URL => $"{Prefix}://{host}/{path}";

        private string Prefix => protocol switch
        {
            ProtocolType.Secure => "wss",
            ProtocolType.Unsecure => "ws",
            _ => throw new ArgumentOutOfRangeException()
        };

        private enum ProtocolType
        {
            Secure,
            Unsecure
        }
    }
}