using System.Collections;
using Core.SDK.GameState;
using Plugins.GoogleAnalytics;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils.WebSocketClient.domain;
using Utils.WebSocketClient.domain.model;
using Zenject;
#if YANDEX_SDK
using Plugins.Platforms.YSDK;
#endif

namespace Features.Lobby.presentation
{
    public class LoadSceneMatchController : MonoBehaviour
    {
#if YANDEX_SDK
        [Inject] private YandexSDK sdk;
#endif
        [Inject] private GameStateNavigator gameStateNavigator;

        [Inject(Id = IWSCommandsUseCase.AuthorizedInstance)]
        private IWSCommandsUseCase commandsUseCase;

        [SerializeField] private string matchSceneName;
        [SerializeField] private string lobbySceneName;

        private bool gameReadyApiInvoked;

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            commandsUseCase
                .Subscribe<bool>(Commands.MatchState)
                .Subscribe(SetMatchActive)
                .AddTo(this);
        }

        private void SetMatchActive(bool active)
        {
            gameStateNavigator.SetLevelPlayingState(active);
            var matchLoaded = false;
            var lobbyLoaded = false;
            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                var sceneName = scene.name;
                if (sceneName == matchSceneName)
                    matchLoaded = true;
                else if (sceneName == lobbySceneName)
                    lobbyLoaded = true;
            }

            if (matchLoaded != active)
            {
                GoogleAnalyticsSDK.SendEvent("load_match_scene");
                SetSceneLoaded(active, matchSceneName);
            }

            if (lobbyLoaded == active)
            {
                GoogleAnalyticsSDK.SendEvent("load_lobby_scene");
                SetSceneLoaded(!active, lobbySceneName);
            }

            HandleGameReadyApi();
        }

#if YANDEX_SDK
        private IEnumerator InvokeGRA()
        {
            yield return new WaitForSeconds(0.5f);
#if !UNITY_EDITOR
            sdk.InvokeGameReadyApi();
#endif
        }
#endif

        private void HandleGameReadyApi()
        {
            if (gameReadyApiInvoked)
                return;
            
#if YANDEX_SDK
            StartCoroutine(InvokeGRA());
#endif
            gameReadyApiInvoked = true;
        }

        private static void SetSceneLoaded(bool loaded, string sceneName)
        {
            try
            {
                if (loaded)
                    SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
                else
                    SceneManager.UnloadScene(sceneName);
            }
            catch
            {
                Debug.LogError("SetSceneLoaded failure");
            }
        }
    }
}