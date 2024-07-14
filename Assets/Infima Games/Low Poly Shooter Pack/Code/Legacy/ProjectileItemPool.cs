using UnityEngine;
using Utils.Pooling;

namespace InfimaGames.LowPolyShooterPack.Legacy
{
    public class ProjectileItemPool: MonoPool<ProjectileItemBase>
    {
        [ContextMenu("Generate")]
        public void GeneratePool() => Generate();
    }
}