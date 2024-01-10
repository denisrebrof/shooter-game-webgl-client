using Shooter.domain;
using Shooter.domain.Model;
using Shooter.domain.Repositories;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace Shooter.presentation.UI
{
    public class MatchProgressController : MonoBehaviour
    {
        [Inject] private CurrentPlayerStateUseCase playerStateUseCase;
        [Inject] private IGameStateRepository gameStateRepository;

        [SerializeField] private Color winningColor;
        [SerializeField] private Color losingColor;
        [SerializeField] private Color neutralColor;
        [SerializeField] private TMP_Text statusText;
        [SerializeField] private TMP_Text winningText;
        [SerializeField] private TMP_Text losingText;

        private void Awake()
        {
            var playerTeamFlow = playerStateUseCase
                .GetStateFlow()
                .First()
                .Select(state => state.Team);

            gameStateRepository
                .state
                .CombineLatest(playerTeamFlow, (state, team) => GetTeamKills(team, state))
                .Subscribe(kills => UpdateStatus(kills.Previous, kills.Current))
                .AddTo(this);

            statusText.text = "";
            winningText.enabled = false;
            losingText.enabled = false;
        }

        private void UpdateStatus(int myTeamKills, int enemyTeamKills)
        {
            var winningState = GetWinningState(myTeamKills, enemyTeamKills);
            statusText.text = $"{myTeamKills} / {enemyTeamKills}";
            statusText.color = GetColor(winningState);
            winningText.enabled = winningState == WinningState.Winning;
            losingText.enabled = winningState == WinningState.Losing;
        }

        private Color GetColor(WinningState state) => state switch
        {
            WinningState.Winning => winningColor,
            WinningState.Losing => losingColor,
            _ => neutralColor
        };

        private static WinningState GetWinningState(int myTeamKills, int enemyTeamKills)
        {
            if (myTeamKills > enemyTeamKills)
                return WinningState.Winning;

            return myTeamKills < enemyTeamKills ? WinningState.Losing : WinningState.Draw;
        }

        private static Pair<int> GetTeamKills(Teams team, GameState state) => new(
            team == Teams.Red ? state.redTeamKills : state.blueTeamKills,
            team == Teams.Red ? state.blueTeamKills : state.redTeamKills
        );

        private enum WinningState
        {
            Winning,
            Losing,
            Draw
        }
    }
}