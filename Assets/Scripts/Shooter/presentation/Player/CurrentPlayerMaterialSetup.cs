using Shooter.domain;
using Shooter.domain.Model;
using UniRx;
using UnityEngine;
using Zenject;

namespace Shooter.presentation.Player
{
    public class CurrentPlayerMaterialSetup : MonoBehaviour
    {
        [SerializeField] private Renderer target;
        [SerializeField] private Material redTeamMaterial;
        [SerializeField] private Material blueTeamMaterial;

        [Inject] private CurrentPlayerStateUseCase playerStateUseCase;

        private void Awake()
        {
            playerStateUseCase
                .GetStateFlow()
                .First()
                .Select(state => GetPlayerMaterial(state.Team))
                .Subscribe(SetPlayerMaterial).AddTo(this);
        }

        private Material GetPlayerMaterial(Teams team)
        {
            return team == Teams.Red ? redTeamMaterial : blueTeamMaterial;
        }

        private void SetPlayerMaterial(Material mat)
        {
            target.material = mat;
        }
    }
}