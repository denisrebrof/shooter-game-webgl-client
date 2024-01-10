using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils.WebSocketClient.domain;
using Utils.WebSocketClient.domain.model;
using Zenject;

namespace Utils.WebSocketClient.presentation
{
    public class ConnectionController : MonoBehaviour
    {
        [Inject] private IWSConnectionRepository connectionRepository;

        [SerializeField] private GameObject disconnectedView;
        [SerializeField] private GameObject connectionView;
        [SerializeField] private GameObject connectionErrorView;

        private bool connected;

        private void Awake()
        {
            connectionView.SetActive(false);
            disconnectedView.SetActive(false);
            connectionErrorView.SetActive(false);
            connectionRepository
                .ConnectionState
                .Subscribe(OnConnectionStateChanged)
                .AddTo(this);
        }

        private void OnConnectionStateChanged(WSConnectionState state)
        {
            switch (state)
            {
                case WSConnectionState.Disconnected:
                    OnDisconnected();
                    break;
                case WSConnectionState.Connected:
                    OnConnected();
                    break;
                case WSConnectionState.Connecting:
                    connectionView.SetActive(true);
                    break;
            }
        }

        private void OnConnected()
        {
            connected = true;
            connectionView.SetActive(false);
        }

        private void OnDisconnected()
        {
            UnloadAllScenesExcept(gameObject.scene.name);
            Cursor.lockState = CursorLockMode.None;
            connectionView.SetActive(false);
            if (connected)
                disconnectedView.SetActive(true);
            else
                connectionErrorView.SetActive(true);
        }

        private void UnloadAllScenesExcept(string sceneName)
        {
            var c = SceneManager.sceneCount;
            for (var i = 0; i < c; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (scene.name != sceneName)
                {
                    SceneManager.UnloadSceneAsync(scene);
                }
            }
        }
    }
}