using InfimaGames.LowPolyShooterPack.Legacy;
using UnityEngine;
using Utils.Pooling;

namespace InfimaGames.LowPolyShooterPack
{
    public class CasingSmallItemPool: MonoPool<CasingPoolItem>
    {
        [ContextMenu("Generate")]
        public void GeneratePool() => Generate();
    }
}