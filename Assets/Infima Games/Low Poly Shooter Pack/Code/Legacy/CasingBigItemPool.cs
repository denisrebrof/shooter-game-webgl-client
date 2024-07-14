using InfimaGames.LowPolyShooterPack.Legacy;
using UnityEngine;
using Utils.Pooling;

namespace Infima_Games.Low_Poly_Shooter_Pack.Code.Legacy
{
    public class CasingBigItemPool : MonoPool<CasingPoolItem>
    {
        [ContextMenu("Generate")]
        public void GeneratePool() => Generate();
    }
}