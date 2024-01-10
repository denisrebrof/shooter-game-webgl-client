using System.Collections.Generic;
using System.Linq;
using Features.Lobby.domain;
using Features.Lobby.domain.model;
using Plugins.GoogleAnalytics;
using UniRx;
using UnityEngine;
using Zenject;

namespace Features.Lobby.presentation
{
    public class GameListScreen : MonoBehaviour
    {
        [Inject] private LobbyGamesUseCase gamesUseCase;

        [SerializeField] private GameListItemControllerPool pool;
        [SerializeField] private Transform root;
        [SerializeField] private GameObject loader;
        [SerializeField] private GameObject emptyGamesStub;

        private readonly List<GameListItemController> activeItems = new();

        private readonly CompositeDisposable composite = new();

        private void OnEnable()
        {
            GoogleAnalyticsSDK.SendEvent("open_games_list");
            emptyGamesStub.SetActive(false);
            loader.SetActive(true);
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
                newItem.transform.SetParent(root);
            }

            for (var index = 0; index < matches.Count; index++)
            {
                var match = matches[index];
                activeItems[index].Setup(match, () => Join(match.matchId));
            }

            emptyGamesStub.SetActive(matches.Count == 0);
        }

        private void Join(string matchId) => gamesUseCase
            .JoinGame(matchId)
            .Subscribe()
            .AddTo(composite);

        private void OnDisable() => composite.Clear();
    }
}