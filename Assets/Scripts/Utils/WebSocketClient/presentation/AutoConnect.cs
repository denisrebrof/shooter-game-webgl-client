using UniRx;
using UnityEngine;
using Utils.WebSocketClient.domain;
using Zenject;

namespace Utils.WebSocketClient.presentation
{
    public class AutoConnect : MonoBehaviour
    {
        [Inject] private IWSConnectionRepository connectionRepository;

        private void Start() => connectionRepository
            .Connect()
            .Subscribe()
            .AddTo(this);
    }
}