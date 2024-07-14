using System.Collections.Generic;
using System.Linq;
using Features.Lobby.domain;
using Features.Lobby.domain.model;
using Michsky.MUIP;
using Plugins.GoogleAnalytics;
using UniRx;
using UnityEngine;
using UnityEngine.Localization;
using Zenject;

namespace Features.Lobby.presentation
{
    public class GameListScreen : MonoBehaviour
    {
        [Inject] private LobbyGamesUseCase gamesUseCase;

        [SerializeField] private GameListItemControllerPool pool;

        [Header("Components Local")]

        [SerializeField] private ModalWindowManager manager;
        [SerializeField] private Transform spawnRoot;
        [SerializeField] private GameObject scrollRoot;
        [SerializeField] private GameObject loader;
        [SerializeField] private GameObject emptyGamesStub;

        [SerializeField] private LocalizedString title;

        private readonly List<GameListItemController> activeItems = new();

        private readonly CompositeDisposable composite = new();

        public void Open()
        {
            GoogleAnalyticsSDK.SendEvent("open_games_list");
            emptyGamesStub.SetActive(false);
            scrollRoot.SetActive(false);
            loader.SetActive(true);
            manager.titleText = title.GetLocalizedString();
            manager.UpdateUI();
            manager.Open();
            gamesUseCase
                .GetGamesListFlow()
                .Select(list => list.matches)
                .Subscribe(UpdateGamesList)
                .AddTo(composite);
        }

        private void UpdateGamesList(List<GameListItemData> matches)
        {
            loader.SetActive(false);
            while (activeItems.Count > matches.Count)
            {
                var lastItem = activeItems.Last();
                activeItems.Remove(lastItem);
                pool.Return(lastItem);
            }

            while (activeItems.Count < matches.Count)
            {
                var newItem = pool.Pop();
                activeItems.Add(newItem);
                newItem.transform.SetParent(spawnRoot);
                newItem.transform.localScale = Vector3.one;
            }

            for (var index = 0; index < matches.Count; index++)
            {
                var match = matches[index];
                activeItems[index].Setup(match, () => Join(match.matchId));
            }

            var isEmpty = matches.Count == 0;
            scrollRoot.SetActive(!isEmpty);
            emptyGamesStub.SetActive(isEmpty);
        }

        private void Join(string matchId) =>
            gamesUseCase
                .JoinGame(matchId)
                .Subscribe()
                .AddTo(composite);

        private void OnDisable() => composite.Clear();
    }
}