using System;
using System.Linq;
using Core.Auth.domain;
using Shooter.domain.Model;
using Shooter.domain.Repositories;
using UniRx;
using UnityEngine;
using Zenject;

namespace Shooter.presentation.UI.Stats
{
    public class GameResultText : MonoBehaviour
    {
        [Inject] private IAuthRepository authRepository;
        [Inject] private IGameStateRepository gameStateRepository;
        [SerializeField] private GameObject wonText;
        [SerializeField] private GameObject failText;

        private long playerId;

        private void Awake()
        {
            wonText.SetActive(false);
            failText.SetActive(false);
            playerId = Convert.ToInt64(authRepository.LoginUserId);
            gameStateRepository
                .state
                .Select(GetResult)
                .Subscribe(HandlerWinner)
                .AddTo(this);
        }

        private GameResult GetResult(GameState state)
        {
            if (state.Type != GameStateTypes.Finished)
                return GameResult.Undefined;

            var playerList = state
                .playerData
                .Where(data => data.playerId == playerId)
                .ToList();

            if (!playerList.Any())
                return GameResult.Undefined;

            var playerTeam = playerList.First().Team;
            return playerTeam == state.WinnerTeam ? GameResult.Win : GameResult.Fail;
        }

        private void HandlerWinner(GameResult result)
        {
            if (result == GameResult.Undefined)
                return;

            wonText.SetActive(result == GameResult.Win);
            failText.SetActive(result == GameResult.Fail);
        }

        private enum GameResult
        {
            Undefined,
            Win,
            Fail
        }
    }
}