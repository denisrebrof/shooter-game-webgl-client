using Shooter.domain;
using Shooter.domain.Model;
using Shooter.domain.Repositories;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace Shooter.presentation.UI.Game
{
    public class MatchProgressController : MonoBehaviour
    {
        [SerializeField] private Color winningColor;
        [SerializeField] private Color losingColor;
        [SerializeField] private Color neutralColor;
        [SerializeField] private TMP_Text statusFlagsText;
        [SerializeField] private TMP_Text statusKillsText;
        [SerializeField] private TMP_Text winningText;
        [SerializeField] private TMP_Text losingText;
        [Inject] private IGameStateRepository gameStateRepository;
        [Inject] private CurrentPlayerTeamUseCase playerTeamUseCase;

        private void Awake() {
            Observable
                .CombineLatest(playerTeamUseCase.TeamFlow, gameStateRepository.state, GetTeamStats)
                .DistinctUntilChanged()
                .Subscribe(UpdateStatus)
                .AddTo(this);

            statusFlagsText.text = "";
            statusKillsText.text = "";
            winningText.enabled = false;
            losingText.enabled = false;
        }

        private void UpdateStatus(GameTeamStats stats) {
            var winningState = GetWinningState(stats);
            statusFlagsText.text = $"{stats.AllieFlags} / {stats.OpponentFlags}";
            statusKillsText.text = $"{stats.AllieKills} / {stats.OpponentKills}";
            statusFlagsText.color = GetColor(winningState);
            statusKillsText.color = GetColor(winningState);
            winningText.enabled = winningState == WinningState.Winning;
            losingText.enabled = winningState == WinningState.Losing;
        }

        private Color GetColor(WinningState state) {
            return state switch
            {
                WinningState.Winning => winningColor,
                WinningState.Losing => losingColor,
                _ => neutralColor
            };
        }

        private static WinningState GetWinningState(GameTeamStats stats) {
            if (stats.AllieFlags > stats.OpponentFlags)
                return WinningState.Winning;

            if (stats.AllieFlags < stats.OpponentFlags)
                return WinningState.Losing;

            if (stats.AllieKills > stats.OpponentKills)
                return WinningState.Winning;

            // ReSharper disable once ConvertIfStatementToReturnStatement
            if (stats.AllieKills < stats.OpponentKills)
                return WinningState.Losing;

            return WinningState.Draw;
        }

        private static GameTeamStats GetTeamStats(Teams team, GameState state) {
            var allieTeam = team == Teams.Red ? state.redTeamState : state.blueTeamState;
            var opponentTeam = team == Teams.Red ? state.blueTeamState : state.redTeamState;
            return new GameTeamStats
            {
                AllieKills = allieTeam.teamKills,
                OpponentKills = opponentTeam.teamKills,
                AllieFlags = allieTeam.teamFlags,
                OpponentFlags = opponentTeam.teamFlags,
            };
        }

        private struct GameTeamStats
        {
            public int AllieKills;
            public int OpponentKills;
            public int AllieFlags;
            public int OpponentFlags;
        }

        private enum WinningState
        {
            Winning,
            Losing,
            Draw
        }
    }
}