using UnityEngine;
using Utils.Pooling;

namespace Shooter.presentation.UI.Lobby.UpcomingRewards
{
    public class UpcomingRewardsItemPool : MonoPool<UpcomingRewardItem>
    {
        [ContextMenu("Generate")]
        public void GeneratePool()
        {
            Generate();
        }
    }
}