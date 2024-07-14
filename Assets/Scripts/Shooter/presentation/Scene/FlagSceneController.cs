using System;
using Shooter.domain;
using Shooter.domain.Model;
using Shooter.domain.Repositories;
using UniRx;
using UnityEngine;
using Zenject;

namespace Shooter.presentation.Scene
{
    public class FlagSceneController : MonoBehaviour
    {
        [Inject] private IGameStateRepository gameStateRepository;
        [Inject] private CurrentPlayerTeamUseCase currentPlayerTeamUseCase;
        
        [SerializeField] private PlayerController playerController;
        [SerializeField] private FlagController redFlag;
        [SerializeField] private StoreFlagTrigger redStable;
        [SerializeField] private FlagController blueFlag;
        [SerializeField] private StoreFlagTrigger blueStable;

        private void Start() {
            currentPlayerTeamUseCase.TeamFlow.Subscribe(SetPlayerTeam).AddTo(this);
            GetTeamDataFlow(Teams.Red).Subscribe(data => UpdateFlagState(data, redFlag)).AddTo(this);
            GetTeamDataFlow(Teams.Blue).Subscribe(data => UpdateFlagState(data, blueFlag)).AddTo(this);
        }

        private void SetPlayerTeam(Teams playerTeam) {
            redFlag.isEnemyFlag = playerTeam == Teams.Blue;
            blueFlag.isEnemyFlag = playerTeam == Teams.Red;
            redStable.enabled = playerTeam == Teams.Red;
            blueStable.enabled = playerTeam == Teams.Blue;
        }

        private IObservable<TeamData> GetTeamDataFlow(Teams team) => gameStateRepository
            .state
            .Select(state => team == Teams.Red ? state.redTeamState : state.blueTeamState)
            .DistinctUntilChanged();

        private void UpdateFlagState(TeamData teamData, FlagController flagController) {
            if (teamData.flagHasOwner) SetFlagOwner(teamData.flagOwnerId, flagController);
            else SetFlagPosition(teamData.flagPos.Pos, flagController);
        }

        private void SetFlagOwner(long flagOwnerId, FlagController flagController) {
            var hasPlayer = playerController.TryGetSpawnedPlayer(flagOwnerId, out var spawnedPlayer);
            if (!hasPlayer)
            {
                flagController.Activate(false);
                return;
            }

            var hasFlagOrigin = spawnedPlayer.TryGetComponent<PlayerFlagOrigin>(out var origin);
            if (!hasFlagOrigin)
            {
                flagController.Activate(false);
                return;
            }

            flagController.Activate(true);
            flagController.SetOrigin(origin.origin);
        }

        private static void SetFlagPosition(Vector3 position, FlagController flagController) {
            flagController.Activate(true);
            flagController.SetPosition(position);
        }
    }
}