using System;
using System.Collections.Generic;
using System.Linq;
using Core.Auth.domain;
using ModestTree;
using Plugins.GoogleAnalytics;
using Shooter.domain;
using Shooter.domain.Model;
using UniRx;
using UnityEngine;
using Zenject;

namespace Shooter.presentation.Scene
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private GameObject playerCanvas;
        [SerializeField] private PlayerIdProvider playerPrefab;
        [SerializeField] private GameObject currentPlayerPrefab;
        private readonly Dictionary<long, GameObject> spawnedPlayers = new();

        [Inject(Id = "PlayersRoot")] private Transform playersRoot;
        [Inject] private IAuthRepository authRepository;

        private IDisposable handler = Disposable.Empty;
        [Inject] private GamePlayerUpdatesUseCase playerUpdatesUseCase;

        private void OnEnable() {
            handler = playerUpdatesUseCase
                .getUpdates()
                .Subscribe(HandlePlayersUpdate)
                .AddTo(this);
        }

        private void OnDisable() {
            spawnedPlayers.Values.ToList().ForEach(Destroy);
            spawnedPlayers.Clear();
            handler?.Dispose();
        }

        public bool TryGetSpawnedPlayer(
            long playerId,
            out GameObject player
        ) => spawnedPlayers.TryGetValue(playerId, out player);

        private void HandlePlayersUpdate(GamePlayersUpdate update) {
            if (update.Type == GamePlayersUpdateType.Added) AddPlayers(update.Players);
            else RemovePlayers(update.Players);
        }

        private void RemovePlayers(IEnumerable<PlayerData> players) {
            players
                .Where(player => spawnedPlayers.ContainsKey(player.playerId))
                .Select(player => spawnedPlayers.GetValueAndRemove(player.playerId))
                .ToList()
                .ForEach(Destroy);
        }

        private void AddPlayers(List<PlayerData> players) {
            players.ForEach(TrySpawnAndSetupPlayer);
        }

        private void TrySpawnAndSetupPlayer(PlayerData player) {
            if (spawnedPlayers.ContainsKey(player.playerId))
                return;

            var isCurrentUser = authRepository.LoginUserId == player.playerId.ToString();
            var prefab = isCurrentUser ? currentPlayerPrefab : playerPrefab.gameObject;
            var rotation = Quaternion.Euler(0f, player.pos.r, 0f);
            if (isCurrentUser)
            {
                prefab.transform.rotation = rotation;
                GoogleAnalyticsSDK.SendEvent("respawn");
            }

            var playerObject = Instantiate(prefab, player.pos.Pos, rotation, playersRoot);
            spawnedPlayers[player.playerId] = playerObject;
            if (isCurrentUser)
            {
                playerCanvas.SetActive(true);
                return;
            }

            playerObject.GetComponent<PlayerIdProvider>().SetPlayerId(player.playerId);
        }
    }
}