using UnityEngine;
using Utils.Pooling;

namespace Shooter.presentation.UI.Lobby.ClaimLevelRewards
{
    public class ClaimLevelRewardsItemPool : MonoPool<ClaimLevelRewardsItem>
    {
        [ContextMenu("Generate")]
        public void GeneratePool() => Generate();
    }
}