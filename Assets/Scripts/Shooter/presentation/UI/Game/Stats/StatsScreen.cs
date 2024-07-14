using System;
using System.Collections.Generic;
using System.Linq;
using Core.Auth.domain;
using Shooter.domain.Model;
using Shooter.domain.Repositories;
using UniRx;
using UnityEngine;
using Zenject;

namespace Shooter.presentation.UI.Game.Stats
{
    public class StatsScreen : MonoBehaviour
    {
        [SerializeField] private bool activateByTab;
        [SerializeField] private Canvas canvas;
        [SerializeField] private List<StatsItemView> leftItems = new();
        [SerializeField] private List<StatsItemView> rightItems = new();
        [Inject] private IAuthRepository authRepository;
        [Inject] private IGameStateRepository gameStateRepository;

        private IDisposable handler = Disposable.Empty;

        private bool initialized;
        private long playerId;
        private Teams playerTeam = Teams.Undefined;

        private void OnEnable()
        {
            if (!initialized) Initialize();
            handler = gameStateRepository.state.Subscribe(RenderState).AddTo(this);
        }

        private void OnDisable()
        {
            handler.Dispose();
        }

        private void Initialize()
        {
            long.TryParse(authRepository.LoginUserId, out playerId);
            if (activateByTab)
                Observable
                    .EveryUpdate()
                    .Select(_ => Input.GetKey(KeyCode.Tab))
                    .DistinctUntilChanged()
                    .Subscribe(visible => canvas.enabled = visible)
                    .AddTo(this);
            initialized = true;
        }

        private void RenderState(GameState state)
        {
            var players = state.playerData;
            UpdatePlayerTeam(players);
            var leftIndex = 0;
            var rightIndex = 0;
            foreach (var player in state.playerData)
            {
                var isPlayerTeam = player.Team == playerTeam;
                var list = isPlayerTeam ? leftItems : rightItems;
                var index = isPlayerTeam ? leftIndex++ : rightIndex++;
                if (index >= list.Count)
                    continue;

                var current = list[index];
                current.gameObject.SetActive(true);
                current.SetData(
                    player.playerId,
                    player.kills,
                    player.death,
                    player.playerId == playerId
                );
            }

            while (leftIndex < leftItems.Count)
                leftItems[leftIndex++].gameObject.SetActive(false);

            while (rightIndex < rightItems.Count)
                rightItems[rightIndex++].gameObject.SetActive(false);
        }

        private void UpdatePlayerTeam(IEnumerable<PlayerData> players)
        {
            if (playerTeam != Teams.Undefined)
                return;

            players
                .Where(player => player.playerId == playerId)
                .ToList()
                .ForEach(currentPlayer => playerTeam = currentPlayer.Team);
        }
    }
}