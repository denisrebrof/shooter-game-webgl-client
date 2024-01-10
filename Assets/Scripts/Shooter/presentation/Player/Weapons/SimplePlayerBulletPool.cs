using UnityEngine;
using Utils.Pooling;

namespace Shooter.presentation.Player.Weapons
{
    public class SimplePlayerBulletPool: MonoPool<SimpleBulletPlayer>
    {
        [ContextMenu("Generate")]
        public void GeneratePool() => Generate();
    }
}