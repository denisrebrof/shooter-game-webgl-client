using Shooter.domain;
using Shooter.domain.Model;
using UniRx;
using UnityEngine;
using Zenject;

namespace Shooter.presentation.Player
{
    public class PlayerMaterialSelector : PlayerDataBehaviour
    {
        [SerializeField] private SkinnedMeshRenderer target;
        [SerializeField] private SkinnedMeshRenderer corpse;
        [SerializeField] private Material firstMat;
        [SerializeField] private Material secondMat;

        private void Awake() => PlayerDataFlow
            .Select(data => data.Team)
            .DistinctUntilChanged()
            .Select(GetPlayerMaterial)
            .Subscribe(SetPlayerMaterial)
            .AddTo(this);

        private Material GetPlayerMaterial(Teams team) => team == Teams.Red ? firstMat : secondMat;

        private void SetPlayerMaterial(Material mat)
        {
            target.material = mat;
            corpse.material = mat;
        }
    }
}